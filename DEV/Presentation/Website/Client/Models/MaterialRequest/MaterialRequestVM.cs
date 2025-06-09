using Client.Models.Common;

using DataContracts;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Client.Models.MaterialRequest
{
    public class MaterialRequestVM : LocalisationBaseVM
    {
        public MaterialRequestVM()
        {

        }
        public Security security { get; set; }
        public UserData udata { get; set; }
        public MaterialRequestModel MaterialRequestModel { get; set; }
        public ChildGridModel ChildGridModel { get; set; }
        public List<ChildGridModel> listChildGridModel { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public List<MaterialRequestPDFPrintModel> materialRequestPDFPrintModel { get; set; }
        public MaterialRequestSummaryModel materialRequestSummaryModel { get; set; }
        public PartNotInInventoryModel PartNotInInventoryModel { get; set; }
        public PartInInventoryModel PartInInventoryModel { get; set; }
        public ApprovalRouteModel ApprovalRouteModel { get;set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; } //V2-732
        public MaterialRequestMultiStoreroomModel MaterialRequestMultiStoreroomModel { get;set;}//V2-732
        public IEnumerable<SelectListItem> UnitOfmesureListMR { get; set; } //V2-1148
    }
}