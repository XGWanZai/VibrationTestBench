using GalaSoft.MvvmLight;
using Arction.Wpf.Charting;
using System;
using System.Windows;
using VibrationTestBench.View;
using System.Windows.Media;
using Arction.Wpf.Charting.Axes;
using Arction.Wpf.Charting.Views.ViewXY;
using HWcommunication;
using SqlSugar;
using VibrationTestBench.Models;

namespace VibrationTestBench.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {

        public MainWindow thiss;
        HWcommunicate hWcommunication;
        public SqlSugarClient db;
        public MainViewModel(MainWindow mainWindow)
        {
            this.thiss = mainWindow;
            this.ButCommand = new CommandBase
            {
                ExecuteAction = new Action<object>(this.Change)
            };
            Init();
        }
        public MainViewModel()
        {
        }

        public void Init()
        {
            hwCommunication();
            SqlSugarClientInit();
            var motoConfig = db.Queryable<MotoModelConfig>().First(it => it.id == 1);
            if (motoConfig!=null)
            {
                testProductModel = motoConfig.motoName;
                MotoViewManage.currentlyTestedModel = motoConfig.motoName;
            }
        }

        public HWcommunicate hwCommunication()
        {
            hWcommunication = new HWcommunicate { Pass_Check = "732576" };
            if (hWcommunication.Kv_Connected().Equals("disconnect"))
            {
                string isok = hWcommunication.Con_Kv("192.168.3.249", 8501);
                if (!isok.Equals("ok"))
                {
                    MessageBox.Show("plc连接失败!");
                }
            }
            return hWcommunication;
        }

        #region 数据库连接
        public SqlSugarClient SqlSugarClientInit()
        {
            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "Data Source=USER-DENGJIYANG;Initial Catalog=VibrationTest;Integrated Security=True",
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });
            return db;
        }
        #endregion

        private string isOpen;
        public string IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                this.RaisePropertyChanged();
            }
        }

        public static string testProductModel;
        public string TestProductModel
        {
            get { return testProductModel; }
            set
            {
                testProductModel = value;
                this.RaisePropertyChanged();
            }
        }

        public CommandBase ButCommand { get; set; }

        private void Change(object parameter)
        {
            string Para = parameter.ToString();
            switch (Para)
            {
                case "0":
                    MotoManage isw = new MotoManage();
                    isw.Owner = thiss;
                    isw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    isw.ShowDialog();
                    break;
                case "1":
                    ModelSelection modelSelection = new ModelSelection();
                    modelSelection.Owner = thiss;
                    modelSelection.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    var r = modelSelection.ShowDialog();
                    if (r.Value)
                    {
                        this.TestProductModel = MotoViewManage.currentlyTestedModel;
                        var id = MotoViewManage.manageModelsIdInset;

                        MotoModelConfig modelConfig = new MotoModelConfig();
                        modelConfig.motoName = TestProductModel;
                        modelConfig.motoId = id;
                        db.Updateable(modelConfig).Where(it => it.id == 1).ExecuteCommand();

                        var models = db.Queryable<MotoModelDetails>().Where(it => it.motoId == id).ToList();
                        hWcommunication.Write_Int16("EM11000", models.Count.ToString());
                        string[] shu1 = new string[models.Count];
                        string[] shu2 = new string[models.Count];
                        for (int i = 0; i < models.Count; i++)
                        {
                            shu1[i] = models[i].suduxiaxianzhi;
                            shu2[i] = models[i].sudushangxianzhi;
                        }
                        hWcommunication.Write_Float("EM10000", shu1);
                        hWcommunication.Write_Float("EM10400", shu2);
                    }
                    break;
                case "2":
                   // var reda =   hWcommunication.Read_Float("EM10000",10);
                    IsOpen = "false";
                    break;
                case "3":
                    IsOpen = "true";
                    break;
                case "4":

                    if (System.Windows.MessageBox.Show("您确认退出吗？", "提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        thiss.Close();
                    }
                    break;
            }
        }
    }
}