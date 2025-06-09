using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Common.Charts.Fusions
{
    public class chartDefaults
    {
        public static string Default1Value { get => "1"; }
        public static string LineColor1 { get => "#5D62B5"; }
        public static string LineColor2 { get => "#29C3BE"; }
        public static string DefaultTheme { get => "fusion"; }
        public static string PlotFillAlpha { get => "80"; }
        public static string MsColumn2dChartType { get => "mscolumn2d"; }
        public static string OverlappedChartType { get => "overlappedbar2d"; }
        public static string xAxisName { get; set; }
        public static string showValues { get => "0"; }
        public static string OverlappedChartPlotToolText { get => "<b>$seriesName</b> $label : <b>$dataValue</b>"; }

    }
}