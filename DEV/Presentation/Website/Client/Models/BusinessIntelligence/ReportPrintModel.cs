using System.Collections.Generic;
using System.Data;

namespace Client.Models.BusinessIntelligence
{
    public class ReportPrintModel
    {
        public ReportPrintModel()
        {
            gridColumnsProps = new List<GridColumnsProp>();
            ReportData = new DataTable();
            ChildGrids = new List<ChildGridtPrintModel>();
        }
        public List<GridColumnsProp> gridColumnsProps { get; set; }
        public DataTable ReportData { get; set; }
        public bool hasChild { get; set; }
        public bool IsGrouped { get; set; }
        public string GroupColumn { get; set; }
        public List<ChildGridtPrintModel> ChildGrids { get; set; }
    }
    public class ChildGridtPrintModel
    {
        public ChildGridtPrintModel()
        {
            gridColumnsProps = new List<GridColumnsProp>();
            ReportData = new DataTable();
        }
        public List<GridColumnsProp> gridColumnsProps { get; set; }
        public DataTable ReportData { get; set; }
    }
}