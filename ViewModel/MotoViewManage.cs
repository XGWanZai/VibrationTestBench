using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HWcommunication;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VibrationTestBench.Models;
using VibrationTestBench.View;

namespace VibrationTestBench.ViewModel
{
    class MotoViewManage : ViewModelBase
    {
        private View.MotoManage motoModelManage;

        public SqlSugarClient db;
        public CommandBase ButMotoCommand { get; set; }
        public int isbaocun = 0;
        public int manageModelsId;
        public static int manageModelsIdInset; 
        public int modetailsId;
        public bool isshifoubaocun =false;
        private HWcommunicate wcommunicate;


        private MotoModelManage selectMenultemCommand;
        public MotoModelManage SelectMenultemCommand
        {
            get { return selectMenultemCommand; }
            set { selectMenultemCommand = value;
                this.RaisePropertyChanged();
                OnSelect();
            }
        }

        private MotoModelDetails selectDataGridCommand;
        public MotoModelDetails SelectDataGridCommand
        {
            get { return selectDataGridCommand; }
            set
            {
                selectDataGridCommand = value;
                OnSelect1();
                this.RaisePropertyChanged();
               
            }
        }

        public void OnSelect1()
        {
            if (selectDataGridCommand != null)
            {
                this.modetailsId = selectDataGridCommand.id;
            }

        }

        private int dataGridIndex;
        public int DataGridIndex
        {
            get { return dataGridIndex; }
            set
            {
                dataGridIndex = value;
                this.RaisePropertyChanged();
            }
        }

        public static string editedProductName;
        public string EditedProductName
        {
            get { return editedProductName; }
            set
            {
                editedProductName = value;
                this.RaisePropertyChanged();
            }
        }
        

        public void OnSelect()
        {
            if (selectMenultemCommand!=null)
            {
                if ( isshifoubaocun == true)
                {
                    if (MessageBox.Show("您编辑的参数尚未保存,是否需要为您保存？", "保存提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        NewSaveEditSave();
                    }
                    isshifoubaocun = false;
                }
                this.EditedProductName = selectMenultemCommand.testedProduct;
                this.manageModelsId = selectMenultemCommand.Id;
                motoModelDetailsModels.Clear();
                List<MotoModelDetails> motoModelDetailsList = db.Queryable<MotoModelDetails>().Where(it => it.motoId == selectMenultemCommand.Id).ToList();
                if (motoModelDetailsList.Count>0)
                {
                    for (int i = 0; i < motoModelDetailsList.Count; i++)
                    {
                        motoModelDetailsModels.Add(motoModelDetailsList[i]);
                    }
                }
            }
        }


        protected ObservableCollection<MotoModelManage> motoModelManageModels;
        public ObservableCollection<MotoModelManage> MotoModelManageModels
        {
            get { return motoModelManageModels; }
            set
            {
                if (motoModelManageModels != value)
                {
                    motoModelManageModels = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        protected ObservableCollection<MotoModelDetails> motoModelDetailsModels;
        public ObservableCollection<MotoModelDetails> MotoModelDetailsModels
        {
            get { return motoModelDetailsModels; }
            set
            {
                if (motoModelDetailsModels != value)
                {
                    motoModelDetailsModels = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        protected ObservableCollection<MotoModelManage> motoModelManageModelsMain;
        public ObservableCollection<MotoModelManage> MotoModelManageModelsMain
        {
            get { return motoModelManageModelsMain; }
            set
            {
                if (motoModelManageModelsMain != value)
                {
                    motoModelManageModelsMain = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        private MotoModelManage selectMenultemCommandMain;
        public MotoModelManage SelectMenultemCommandMain
        {
            get { return selectMenultemCommandMain; }
            set
            {
                selectMenultemCommandMain = value;
                this.RaisePropertyChanged();
                OnSelectMain();
            }
        }
        public static string currentlyTestedModel;
        public string CurrentlyTestedModel
        {
            get { return currentlyTestedModel; }
            set
            {
                currentlyTestedModel = value;
                this.RaisePropertyChanged();
            }
        }
        public void OnSelectMain()
        {
            if (selectMenultemCommandMain != null)
            {
                CurrentlyTestedModel = selectMenultemCommandMain.testedProduct;
                manageModelsIdInset = selectMenultemCommandMain.Id;
               
            }
        }
        public MotoViewManage(View.MotoManage motoModelManage)
        {
            this.motoModelManage = motoModelManage;
            this.ButMotoCommand = new CommandBase
            {
                ExecuteAction = new Action<object>(this.Change)
            };
            //初始查询产品型号
            Init();
        }

        public void Init()
        {
            MainViewModel m = new MainViewModel();
            db = m.SqlSugarClientInit();
            //数据库连接
            wcommunicate = m.hwCommunication();
            MotoModelManageModels = new ObservableCollection<MotoModelManage>();
            MotoModelDetailsModels = new ObservableCollection<MotoModelDetails>();
            MotoModelManageModelsMain = new ObservableCollection<MotoModelManage>();
            motoModelDetailsModels.Clear();
            var models = db.Queryable<MotoModelManage>().ToList();
            models.ForEach(
                   arg => { MotoModelManageModels.Add(arg); }
                   );
            models.ForEach(
                   arg => { MotoModelManageModelsMain.Add(arg); }
                   );
        }

        public MotoViewManage()
        {
            Init();
        }

        #region 新建
        public void createNew(MotoModelManage modelManage)
        {
            this.EditedProductName = modelManage.testedProduct;
            this.manageModelsId = db.Insertable(modelManage).ExecuteReturnIdentity();
        }
        #endregion

        #region 刷新被测产品列表
        public void savedMachinesRefresh()
        {
            motoModelManageModels.Clear();
            var models = db.Queryable<MotoModelManage>().ToList();
            models.ForEach(
                   arg => { MotoModelManageModels.Add(arg); }
                   );
        }
        #endregion

        #region 刷新测试数据表
        public void motoModelDetailsRefresh()
        {
            motoModelDetailsModels.Clear();
            if (selectMenultemCommand!=null)
            {
                var models = db.Queryable<MotoModelDetails>().Where(it => it.motoId == selectMenultemCommand.Id).ToList();

                models.ForEach(
                       arg => { MotoModelDetailsModels.Add(arg); }
                       );
            }
            

        }
        #endregion



        #region 新建保存和编辑尚未保存
        public void NewSaveEditSave()
        {
            bool isWeibianji = false;
            isshifoubaocun = false;
            var item = motoModelManage.dgData.ItemsSource;
            var dt = db.Ado.ExecuteCommand("DELETE FROM MotoModelDetails WHERE motoId = " + this.manageModelsId + "");
            foreach (MotoModelDetails it in item)
            {
                if (it.sudushangxianzhi != null && it.suduxiaxianzhi != null && it.jiasuduzhishangxian != null && it.jiasuduzhixiaxian != null)
                {
                    MotoModelDetails modelDetails = new MotoModelDetails();
                    modelDetails.sudushangxianzhi = it.sudushangxianzhi;
                    modelDetails.suduxiaxianzhi = it.suduxiaxianzhi;
                    modelDetails.jiasuduzhishangxian = it.jiasuduzhishangxian;
                    modelDetails.jiasuduzhixiaxian = it.jiasuduzhixiaxian;
                    modelDetails.motoId = this.manageModelsId;
                    var index = db.Insertable(modelDetails).ExecuteCommand();
                }
                else
                {
                    isWeibianji = true;
                }
            }
            if (isWeibianji)
            {
                MessageBox.Show("请编辑完成之后再保存哦!");

            }
        }
        #endregion

        private void Change(object parameter)
        {
            string Para = parameter.ToString();

            var item = motoModelManage.dgData.ItemsSource;

            switch (Para)
            {
                case "5":
                    if (isbaocun == 0)
                    {
                        if (selectMenultemCommand != null)
                        {
                            if (System.Windows.MessageBox.Show("您确定要保存吗？", "提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            {
                                isshifoubaocun = false;
                                var i = db.Ado.ExecuteCommand("  UPDATE MotoModelManage SET testedProduct = '" + motoModelManage.beibianjicpText.Text.Trim() + "' where Id = " + manageModelsId + "");

                                foreach (MotoModelDetails it in item)
                                {
                                    var dt = db.Ado.ExecuteCommand("UPDATE MotoModelDetails SET sudushangxianzhi = '" + it.sudushangxianzhi + "', suduxiaxianzhi = '" + it.suduxiaxianzhi + "', jiasuduzhishangxian = '" + it.jiasuduzhishangxian + "', jiasuduzhixiaxian='" + it.jiasuduzhixiaxian + "' WHERE id = " + it.id + "");
                                }
                                savedMachinesRefresh();
                            }
                        }
                        else
                        {
                            MessageBox.Show("请选择再保存哦!");
                        }
                    }
                    else
                    {
                        if (System.Windows.MessageBox.Show("您确定要保存吗？", "提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            NewSaveEditSave();
                        }
                    }
                    break;
                case "6":
                    MotoModelManage modelManage = new MotoModelManage();
                    CreateNewFile isw = new CreateNewFile(modelManage);
                    isw.Owner = motoModelManage;//将主窗口设置为AboutWindow的拥有者
                    isw.WindowStartupLocation = WindowStartupLocation.CenterOwner;//将AboutWindow的打开的初始位置设置为在Owner的中央
                    var r =  isw.ShowDialog();
                    if (r.Value)
                    {
                        isshifoubaocun = false;
                        createNew(modelManage);
                        this.savedMachinesRefresh();
                        MotoModelDetails modelDetails = new MotoModelDetails();
                        modelDetails.sudushangxianzhi = null;
                        modelDetails.suduxiaxianzhi = null;
                        modelDetails.jiasuduzhishangxian = null;
                        modelDetails.jiasuduzhixiaxian = null;
                        modelDetails.motoId = this.manageModelsId;
                        var index = db.Insertable(modelDetails).ExecuteCommand();
                        isbaocun = 1;
                        motoModelDetailsModels.Clear();

                       var  motoModelDetailsList = db.Queryable<MotoModelDetails>().Where(it => it.motoId == this.manageModelsId).ToList();
                        motoModelDetailsList.ForEach(
                          arg => { MotoModelDetailsModels.Add(arg); }
                          );
                    }
                    break;
                case "7":
                    if (this.modetailsId!=0)
                    {
                        if (System.Windows.MessageBox.Show("您确定要删除吗？", "提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            //db.Deleteable<MotoModelDetails>().In(selectMenultemCommand.Id).ExecuteCommand();
                            var dt = db.Ado.ExecuteCommand("DELETE FROM MotoModelDetails WHERE id = " + this.modetailsId + "");
                            motoModelDetailsRefresh();
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择再删除哦!");
                    }
                    
                    break;
                case "8":
                    if (System.Windows.MessageBox.Show("您确认退出吗？", "提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        if (isshifoubaocun == true)
                        {
                            if (MessageBox.Show("您编辑的参数尚未保存,是否需要为您保存？", "保存提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            {
                                NewSaveEditSave();
                            }
                            isshifoubaocun = false;
                        }

                        motoModelManage.Close();
                    }
                    break;
                case "9":
                    isbaocun = 1;
                    int selectIndex = DataGridIndex;
                    MotoModelDetails moto = new MotoModelDetails();
                    moto.sudushangxianzhi = null;
                    moto.suduxiaxianzhi = null;
                    moto.jiasuduzhishangxian = null;
                    moto.jiasuduzhixiaxian = null;
                    moto.motoId = this.manageModelsId;
                    if (selectIndex == 0 || selectIndex==-1)       // 如果没有选中行就默认添加，添加到最后一行
                    {
                        MotoModelDetailsModels.Add(moto);
                    }
                    else
                    {
                        MotoModelDetailsModels.Insert(selectIndex + 1, moto);
                    }
                    break;
                case "10":
                    if (selectMenultemCommand!=null)
                    {
                        if (System.Windows.MessageBox.Show("将会删除全部参数,您确定吗", "提示：", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            db.Deleteable<MotoModelManage>().In(selectMenultemCommand.Id).ExecuteCommand();
                            var dt = db.Ado.ExecuteCommand("DELETE FROM MotoModelDetails WHERE motoId = " + selectMenultemCommand.Id + "");
                            motoModelDetailsRefresh();
                            savedMachinesRefresh();
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选中后再删除!");
                    }
                    break;
                case "11":
                    isshifoubaocun = true;
                    isbaocun = 0;
                    break;
                case "12":
                    isbaocun = 0;
                    break;
            }
        }
    }
}
