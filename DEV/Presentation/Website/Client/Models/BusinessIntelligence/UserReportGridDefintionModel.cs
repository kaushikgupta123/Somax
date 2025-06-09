using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.BusinessIntelligence
{
    public class UserReportGridDefintionModel
    {
        public long UserReportGridDefintionId { get; set; }
        public long ReportId { get; set; }
        public int Sequence { get; set; }
        public string Columns { get; set; }
        public string Heading { get; set; }
        public string Alignment { get; set; }
        public bool Display { get; set; }
        public bool Required { get; set; }
        public bool AvailableonFilter { get; set; }
        public bool IsGroupTotaled { get; set; }
        public bool IsGrandTotal { get; set; }
        public bool LocalizeDate { get; set; }
        public bool IsChildColumn { get; set; }
        public string Filter { get; set; }
        public int NumofDecPlaces { get; set; }
        public string NumericFormat { get; set; }
        public string FilterMethod { get; set; }
        public bool DateDisplay { get; set; }
    }
}