using Arction.Wpf.Charting;
using Arction.Wpf.Charting.Axes;
using Arction.Wpf.Charting.Views.ViewXY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VibrationTestBench.View
{
    /// <summary>
    /// SpeedCurve.xaml 的交互逻辑
    /// </summary>
    public partial class SpeedCurve : Page
    {
        public SpeedCurve()
        {
            InitializeComponent();
            BBottomChartLoad();
        }

        private LightningChartUltimate BBottomChart;
        private AxisX BBottomChartAxisX;
        private AxisY BBottomChartAxisY;
        private LineSeriesCursor BTopChartCursor;
        private LightningChartUltimate BTopChart;

        private void BBottomChartLoad()
        {
            BBottomChart = new LightningChartUltimate();
            BBottomChart.BeginUpdate();
            BBottomChart.Height = 210;
            BBottomChart.Width = 650;
            BBottomChart.ColorTheme = ColorTheme.SkyBlue;
            BBottomChart.Title.Visible = false;
            //BBottomChart.Title.Text = "转速-角度";
            BBottomChart.Title.Font.Size = 20;
            BBottomChart.Title.MoveByMouse = false;
            BBottomChart.Title.MouseHighlight = MouseOverHighlight.None;
            BBottomChart.Title.Color = Colors.DeepSkyBlue;
            BBottomChart.Title.Font.Bold = false;
            BBottomChart.ChartBackground.Color = Color.FromArgb(0, 0, 0, 0);
            BBottomChart.ChartBackground.GradientFill = GradientFill.Solid;

            BBottomChart.ViewXY.GraphBackground.Color = Colors.White;
            BBottomChart.ViewXY.GraphBackground.GradientFill = GradientFill.Solid;
            BBottomChart.ViewXY.LegendBoxes[0].Visible = false;
            BBottomChart.ViewXY.LegendBoxes[0].Layout = LegendBoxLayout.Vertical;
            BBottomChart.ViewXY.LegendBoxes[0].Position = LegendBoxPositionXY.LeftCenter;
            BBottomChart.ViewXY.LegendBoxes[0].MoveByMouse = false;
            BBottomChart.ViewXY.LegendBoxes[0].AllowMouseResize = false;
            BBottomChart.ViewXY.LegendBoxes[0].Shadow.Visible = false;
            BBottomChart.ViewXY.LegendBoxes[0].BorderWidth = 0;
            BBottomChart.ViewXY.LegendBoxes[0].MouseHighlight = MouseOverHighlight.None;
            BBottomChart.ViewXY.LegendBoxes[0].Fill.GradientFill = GradientFill.Solid;
            BBottomChart.ViewXY.LegendBoxes[0].UnitsColor = Colors.White;
            BBottomChart.ViewXY.LegendBoxes[0].Fill.Color = Color.FromArgb(120, 0, 0, 0);
            BBottomChart.ViewXY.ZoomPanOptions.MiddleMouseButtonAction = MouseButtonAction.Pan;
            BBottomChart.ViewXY.ZoomPanOptions.LeftMouseButtonAction = MouseButtonAction.Zoom;
            BBottomChart.ViewXY.ZoomPanOptions.MouseWheelZooming = MouseWheelZooming.Off;
            BBottomChart.ViewXY.ZoomPanOptions.RightMouseButtonAction = MouseButtonAction.Zoom;
            BBottomChartAxisX = BBottomChart.ViewXY.XAxes[0];
            BBottomChartAxisX.ScrollMode = XAxisScrollMode.None;
            BBottomChartAxisX.ValueType = AxisValueType.Number;
            BBottomChartAxisX.AxisColor = Colors.Black;
            BBottomChartAxisX.Title.Text = "转速 r/min";
            BBottomChartAxisX.Title.MoveByMouse = false;
            BBottomChartAxisX.Title.MouseHighlight = MouseOverHighlight.None;
            BBottomChartAxisX.Title.Color = Colors.DeepSkyBlue;
            BBottomChartAxisX.SetRange(0, 50);


            BBottomChartAxisY = BBottomChart.ViewXY.YAxes[0];
            BBottomChartAxisY.AxisColor = Colors.Black;
            BBottomChartAxisY.Title.Visible = false;
            BBottomChartAxisY.Title.Text = "角度 °";
            BBottomChartAxisY.Title.MoveByMouse = false;
            BBottomChartAxisY.Title.Fill.GradientColor = Colors.Black;
            BBottomChartAxisY.Title.MouseHighlight = MouseOverHighlight.None;
            BBottomChartAxisY.Title.Color = Colors.DeepSkyBlue;
            BBottomChartAxisY.SetRange(0, 360);
            BBottomChart.EndUpdate();
            this.SpeedV.Children.Add(BBottomChart);
            //this.BBottomChartAddAnnotation();
        }
    }
}
