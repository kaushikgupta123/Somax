using Client.Models.Common;
using Client.Models.Work_Order.UIConfiguration;
using Client.Models.Work_Order;

using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Sanitation
{
    public class SanitationVM : LocalisationBaseVM
    {
        public SanitationJobSearchModel sanitationJobSearchModel { get; set; }
        public SanitationJobModel sanitationJobModel { get; set; }
        public SanitationVerificationModel SanitationVerificationModel { get; set; }
        public SanitationJobDetailsModel JobDetailsModel { get; set; }
        public SanitationJobPrintModel sanitationJobPrintModel { get; set; }
        public SanitationCancelAndPrintListModel sanitationCancelAndPrintListModel { get; set; }
        public NotesModel notesModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public LaborModel laborModel { get; set; } 
        public SanitationJobTaskModel sanitationJobTaskModel { get; set; }
        public AssignmentModel assignmentModel { get; set; }
        public ToolsModel toolModel { get; set; }
        public ChemicalSuppliesModel chemicalSuppliesModel { get; set; }
        public Security security { get; set; }
        public List<ToolsModel> ToolsModelList { get; set; }
        public List<ChemicalSuppliesModel> ChemicalSuppliesModelList { get; set; }
        public List<SanitationJobTaskModel> SanitationJobTaskModelList { get; set; }
        public List<LaborModel> LaborModelList { get; set; }
        public int attachmentCount { get; set; }
        // V2-609
        public bool AssetTree { get; set; }
        #region common Dropdown
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public AddODemandModel DemandModel { get; set; }
        public AddODescribeModel ODescribeModel { get; set; }

        public bool IsJobAddFromDashboard { get; set; }
        public bool IsJobAddFromIndex { get; set; }
        public bool IsJobOnDemandAdd { get; set; }
        public bool IsJobDescribeAdd { get; set; }
        public bool IsAddForRequest { get; set; }
        public bool IsAddForJob { get; set; }

        public string TchargeType { get; set; }
        public long? TplantLocationId { get; set; }
        public string TplantLocationDescription { get; set; }
        public string TchargeToClientLookupId { get; set; }
        public bool IsRedirectFromWorkorder { get; set; }

        #endregion

        #region V2-841
        public List<ImageAttachmentModel> imageAttachmentModels { get; set; }
        public UserData _userdata { get; set; }
        #endregion

        public List<SanitationJobSearchModel> SanitationJobModelList { get; set; } //V2-910
        public FailVarificationModel failVarificationModel { get; set; } //V2-912

        //V2-1055
        public UserData userData { get; set; }
        public AddWorkRequestModelDynamic AddWorkRequest { get; set; }
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public List<WOAddUILookupList> AllRequiredLookUplist { get; set; }
        public bool IsAddWoRequestDynamic { get; set; }
        public bool IsDepartmentShow { get; set; }
        public bool IsTypeShow { get; set; }
        public bool IsDescriptionShow { get; set; }
        public string ChargeType { get; set; }
        public string BusinessType { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForWO { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForWOCreatedate { get; set; }
        public long? SanitationJobId { get; set; }
        //
    }
}