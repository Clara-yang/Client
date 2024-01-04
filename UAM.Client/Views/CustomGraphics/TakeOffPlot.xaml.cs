using System;
using System.Windows;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using UAM.Plugin.Common;
using UAM.Client.ViewModel;

namespace UAM.Client.Views.CustomGraphics
{
    /// <summary>
    /// TakeOffPlot.xaml 的交互逻辑
    /// </summary>
    public partial class TakeOffPlot : Page
    {
        private Queue<double> dataQueue = new Queue<double>(50);
        TakeOffPlotViewModel vm = new TakeOffPlotViewModel();
        public TakeOffPlot()
        {
            InitializeComponent();
            this.DataContext = vm;

            chartHeight.Series.Clear();
            ChartArea chartHeightArea = new ChartArea();
            chartHeight.ChartAreas.Add(chartHeightArea);

            chartStartPoint.Series.Clear();
            ChartArea chartStartPointArea = new ChartArea();
            chartStartPoint.ChartAreas.Add(chartStartPointArea);

            chartNorthHeight.Series.Clear();
            ChartArea chartNorthHeightArea = new ChartArea();
            chartNorthHeight.ChartAreas.Add(chartNorthHeightArea);

            chartEastHeight.Series.Clear();
            ChartArea chartEastHeightArea = new ChartArea();
            chartEastHeight.ChartAreas.Add(chartEastHeightArea);

            Load();
        }
        public void Load()
        {
            ChartHelper.AddSeries(chartHeight, "随时间变化的高度", SeriesChartType.Point, Color.White, Color.White, true);
            ChartHelper.SetTitle(chartHeight, "", new Font("微软雅黑", 12), Docking.Bottom, Color.White);
            ChartHelper.SetStyle(chartHeight, Color.Transparent);
            ChartHelper.SetXY(chartHeight, "时间/min", "高度/m", StringAlignment.Far, 10, 200,
                TickMarkStyle.None, string.Empty, string.Empty, 50, 500, 0, 0, AxisArrowStyle.Triangle, AxisArrowStyle.Triangle, false, true);
            ChartHelper.SetMajorGrid(chartHeight, Color.Transparent, Color.Transparent);

            ChartHelper.AddSeries(chartStartPoint, "垂直起飞", SeriesChartType.Point, Color.White, Color.White, true);
            ChartHelper.SetTitle(chartStartPoint, "", new Font("微软雅黑", 12), Docking.Bottom, Color.White);
            ChartHelper.SetStyle(chartStartPoint, Color.Transparent);
            ChartHelper.SetXY(chartStartPoint, "", "北/m", StringAlignment.Far, 10, 10,
                TickMarkStyle.None, 0.ToString(), 0.ToString(), 30, 30, -30, -30, AxisArrowStyle.Triangle, AxisArrowStyle.Triangle, true, true);
            ChartHelper.SetMajorGrid(chartStartPoint, Color.Transparent, Color.Transparent);

            ChartHelper.AddSeries(chartNorthHeight, "随时间变化的北方向高度", SeriesChartType.Point, Color.White, Color.White, true);
            ChartHelper.SetTitle(chartNorthHeight, "", new Font("微软雅黑", 12), Docking.Bottom, Color.White);
            ChartHelper.SetStyle(chartNorthHeight, Color.Transparent);
            ChartHelper.SetXY(chartNorthHeight, "", "", StringAlignment.Center, 200, 10,
                TickMarkStyle.None, "0", "0", 0, 20, -200, -20, AxisArrowStyle.None, AxisArrowStyle.Triangle, false, true); ;
            ChartHelper.SetMajorGrid(chartNorthHeight, Color.Transparent, Color.Gray);

            ChartHelper.AddSeries(chartEastHeight, "随时间变化的东方向高度", SeriesChartType.Point, Color.White, Color.White, true);
            ChartHelper.SetTitle(chartEastHeight, "", new Font("微软雅黑", 12), Docking.Bottom, Color.White);
            ChartHelper.SetStyle(chartEastHeight, Color.Transparent);
            ChartHelper.SetXY(chartEastHeight, "东/m", "", StringAlignment.Far, 10, 200,
                TickMarkStyle.None, "0", "0", 50, 500, -50, 0, AxisArrowStyle.Triangle, AxisArrowStyle.Triangle, true, true);
            ChartHelper.SetMajorGrid(chartEastHeight, Color.Gray, Color.Transparent);

            RefreshData();
        }

        public void RefreshData()
        {
            Random r = new Random();
            for (int i = 0; i < 30; i++)
            {
                //对象添加到 System.Collections.Generic.Queue<T> 的结尾处。用于存放绘图点(y轴数据点) 随机数
                dataQueue.Enqueue(r.Next(0, 30));
            }
            //this.chartStartPoint.Series[0].Points.Clear();

            for (int i = 0; i < dataQueue.Count; i++)
            {
                this.chartStartPoint.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));
                this.chartHeight.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));
                this.chartEastHeight.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));
            }

            for (int i = 0; i < dataQueue.Count; i++)
            {
                this.chartNorthHeight.Series[0].Points.AddXY((-i - 1), dataQueue.ElementAt(i));
            }
        }

    }
}
