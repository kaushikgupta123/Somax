using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartsManagementRequestVM : LocalisationBaseVM
    {
        public PartsManagementRequestModel partsManagementRequestModel { get; set; }
        public AssignPartMastertoIndusnetBakeryModel assignPartMastertoIndusnetBakeryModel { get; set; }
        public ReplacePartModal replacePartModal { get; set; }
        public PartsManagementAttachmentModel partsManagementAttachmentModel { get; set; }
        public PartManagementReviewLog partManagementReviewLog { get; set; }
        public InactivePartModel inactivePartModel { get; set; }
        public ReplaceSXPartModel replaceSXPartModel { get; set; }
        public PartMRequestDenyModel partMRequestDenyModel { get; set; }
        public PartMRequestReturnRequesterModel partMRequestReturnRequesterModel { get; set; }
        public PartMRequestSendApprovalModel partMRequestSendApprovalModel { get; set; }
        public PartManagementGridDataModel partManagementGridDataModel { get; set; }
        public PartManagementReplaceGridModel partManagementReplaceGridModel { get; set; }
        public PartsManagementRequestDetailModel partsManagementRequestDetailModel { get; set; }        
        public PartMgmtManufacGridModel partMgmtManufacGridModel { get; set; }
        public string Part_ClientLookupId { get; set; }
        public bool SomaxPartNoVisiblity { get; set; }
        public string PageHeader { get; set; }
        public bool PmMenuVisibility { get; set; }
    }
}