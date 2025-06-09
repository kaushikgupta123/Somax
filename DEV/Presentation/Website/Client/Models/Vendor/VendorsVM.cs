using Client.Models.Vendor.UIConfiguration;
using Client.Models.VendorPrint;
using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models
{
    public class VendorsVM : LocalisationBaseVM
    {
        public VendorsVM()
        {
            VendorPunchoutSetupModel = new VendorPunchoutSetupModel();
        }
        public VendorsModel vendors { get; set; }
        public ContactModel contactModel { get; set; }
        public PartVendorXrefModel partVendorXrefModel { get; set; }
        public NotesModel notesModel { get; set; }
        public AttachmentModel attachmentModel { get; set; }
        public VendorPrintModel vendorPrintModel { get; set; }
        public string ClientLookupId { get; set; }
        public Security security { get; set; }
        public UserData udata { get; set; }
        public bool UseVendorMaster { get; set; }
        public int attachmentCount { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
        public List<string> hiddenColumnList { get; set; }
        public bool VendorMaster_AllowLocal { get; set; }
        public List<string> requiredColumnList { get; set; }
        public List<string> disabledColumnList { get; set; }
        public ChangeVendorIDModel _ChangeVendorIDModel { get; set; }
        public VendorPunchoutSetupModel VendorPunchoutSetupModel { get; set; }
        #region V2-642
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        public IEnumerable<UILookupList> AllRequiredLookUplist { get; set; }
        public AddVendorModelDynamic AddVendor { get; set; }
        public bool IsAddVendorDynamic { get; set; }
        public EditVendorModelDynamic EditVendor { get; set; }
        //public bool IsEditVendorDynamic { get; set; }
        public IEnumerable<SelectListItem> VendorDomainList { get; set; }
        public IEnumerable<SelectListItem> SenderDomainList { get; set; }
        public ViewVendorModelDynamic ViewVendor { get; set; }
        public VendorEmailConfigurationSetupModel VendorEmailConfigurationSetupModel { get; set; } // V2-750
        #endregion
        #region V2-796
        public bool OracleVendorMasterImport { get; set; }
        #endregion

        #region V2-929
        public VendorInsuranceModel VendorInsuranceModel { get; set; }
        public VendorInsuranceWidgetModel VendorInsuranceWidgetModel { get; set; }
        #endregion

        #region V2-933
        public VendorAssetMgtModel VendorAssetManagementModel { get; set; }
        public VendorAssetMgtWidgetModal VendorAssetManagementWidgetModel { get; set; }
        #endregion
    }
}