using Csf.Imets.ToolCore.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Csf.Imets.ToolCore.Common.Controls.BrowsableTypeConverter;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using Color = System.Drawing.Color;
using LabelStyle = System.Windows.Forms.DataVisualization.Charting.LabelStyle;
using AxisType = System.Windows.Forms.DataVisualization.Charting.AxisType;

namespace UAM.Plugin.Common
{
    public class ChartHelper
    {
        /// <summary>
        /// Name：添加序列
        /// Author：by boxuming 2019-04-28 13:59
        /// </summary>
        /// <param name="chart">图表对象</param>
        /// <param name="seriesName">序列名称</param>
        /// <param name="chartType">图表类型</param>
        /// <param name="color">颜色</param>
        /// <param name="markColor">标记点颜色</param>
        /// <param name="showValue">是否显示数值</param>
        public static void AddSeries(Chart chart, string seriesName, SeriesChartType chartType, Color color, Color markColor, bool showValue = false)
        {
            chart.Series.Add(seriesName);
            chart.Series[seriesName].ChartType = chartType;
            chart.Series[seriesName].Color = color;
            if (showValue)
            {
                //chart.Series[seriesName].IsValueShownAsLabel = true;
                chart.Series[seriesName].MarkerStyle = MarkerStyle.None;
                chart.Series[seriesName].MarkerColor = markColor;
                chart.Series[seriesName].LabelForeColor = color;
                chart.Series[seriesName].LabelAngle = -90;
            }
        }

        /// <summary>
        /// Name：设置标题
        /// Author：by boxuming 2019-04-28 14:25
        /// </summary>
        /// <param name="chart">图表对象</param>
        /// <param name="chartName">图表名称</param>
        public static void SetTitle(Chart chart, string chartName, Font font, Docking docking, Color foreColor)
        {
            chart.Titles.Add(chartName);
            chart.Titles[0].Font = font;
            chart.Titles[0].Docking = docking;
            chart.Titles[0].ForeColor = foreColor;
        }

        /// <summary>
        /// Name：设置chart样式
        /// Author：by boxuming 2019-04-23 14:04
        /// </summary>
        /// <param name="chart">图表对象</param>
        /// <param name="backColor">背景颜色</param>
        public static void SetStyle(Chart chart, Color backColor)
        {
            chart.BackColor = Color.Transparent;
            chart.ChartAreas[0].BackColor = backColor;
            chart.ForeColor = Color.White;
        }

        /// <summary>
        /// Name：设置XY轴
        /// Author：by boxuming 2019-04-23 14:35
        /// </summary>
        /// <param name="chart">图表对象</param>
        /// <param name="xTitle">X轴标题</param>
        /// <param name="yTitle">Y轴标题</param>
        /// <param name="align">坐标轴标题对齐方式</param>
        /// <param name="xInterval">X轴的间距</param>
        /// <param name="yInterval">Y轴的间距</param>
        /// <param name="tickMark">刻度线的样式</param>
        /// <param name="xOrigion">X轴的原点位置</param>
        /// <param name="yOrigion">Y轴的原点位置</param>
        public static void SetXY(Chart chart, string xTitle, string yTitle, StringAlignment align, double xInterval, double yInterval, TickMarkStyle tickMark, string xOrigion, string yOrigion, double XmaxValue, double YmaxValue, double XminValue, double YminValue, AxisArrowStyle xArrowStyle, AxisArrowStyle yArrowStyle, bool Xlabel, bool Ylabel)
        {
            chart.ChartAreas[0].AxisX.Title = xTitle;
            chart.ChartAreas[0].AxisY.Title = yTitle;
            chart.ChartAreas[0].AxisX.TitleAlignment = align;
            chart.ChartAreas[0].AxisY.TitleAlignment = align;
            chart.ChartAreas[0].AxisX.Minimum = XminValue;
            chart.ChartAreas[0].AxisY.Minimum = YminValue;
            chart.ChartAreas[0].AxisX.Maximum = XmaxValue;
            chart.ChartAreas[0].AxisY.Maximum = YmaxValue;
            chart.ChartAreas[0].AxisX.Interval = xInterval;
            chart.ChartAreas[0].AxisY.Interval = yInterval;
            chart.ChartAreas[0].AxisX.MajorTickMark.TickMarkStyle = tickMark;
            chart.ChartAreas[0].AxisY.MajorTickMark.TickMarkStyle = tickMark;
            chart.ChartAreas[0].AxisX.MajorTickMark.LineColor = Color.White;
            chart.ChartAreas[0].AxisY.MajorTickMark.LineColor = Color.White;
            chart.ChartAreas[0].AxisX.TitleForeColor = Color.White;
            chart.ChartAreas[0].AxisY.TitleForeColor = Color.White;
            chart.ChartAreas[0].AxisX.LabelStyle = new LabelStyle { ForeColor = Color.White, Enabled = Xlabel };
            chart.ChartAreas[0].AxisY.LabelStyle = new LabelStyle { ForeColor = Color.White, Enabled = Ylabel };
            chart.ChartAreas[0].AxisX.LineColor = Color.White;
            chart.ChartAreas[0].AxisY.LineColor = Color.White;
            chart.ChartAreas[0].AxisX.ArrowStyle = xArrowStyle;
            chart.ChartAreas[0].AxisY.ArrowStyle = yArrowStyle;
            if (xOrigion != "" && yOrigion != "")
            {
                chart.ChartAreas[0].AxisX.Crossing = Convert.ToDouble(xOrigion);
                chart.ChartAreas[0].AxisY.Crossing = Convert.ToDouble(yOrigion);
            }
        }

        /// <summary>
        /// Name：设置网格
        /// Author：by boxuming 2019-04-23 14:55
        /// </summary>
        /// <param name="chart">图表对象</param>
        /// <param name="lineColor">网格线颜色</param> 
        public static void SetMajorGrid(Chart chart, Color xLineColor, Color yLinColor)
        {
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = xLineColor;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = yLinColor;
        }

        public static void CreatSecondaryXY(Chart chart, string seriesName, SeriesChartType chartType, bool showValue = false)
        {
            chart.Series.Add(seriesName);

            chart.Series[seriesName].XAxisType = AxisType.Primary;
            chart.Series[seriesName].YAxisType = AxisType.Secondary;
            chart.Series[seriesName].ChartType = chartType;
            chart.Series[seriesName].Color = Color.White;
            if (showValue)
            {
                chart.Series[seriesName].MarkerStyle = MarkerStyle.Circle;
                chart.Series[seriesName].MarkerColor = Color.White;
                chart.Series[seriesName].LabelForeColor = Color.White;
                chart.Series[seriesName].LabelAngle = -90;
            }
        }

    }


}
