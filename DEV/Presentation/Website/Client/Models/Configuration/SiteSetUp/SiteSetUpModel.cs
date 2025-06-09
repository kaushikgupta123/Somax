using Client.CustomValidation;

using Common.Constants;

using DataContracts;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.SiteSetUp
{
    public class SiteSetUpModel : LocalisationBaseVM
    {
        [Required(ErrorMessage = "spnNameErrorMsg|" + LocalizeResourceSetConstants.Global)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public string Language { get; set; }
        public IEnumerable<SelectListItem> LanguageList { get; set; }
        [Required(ErrorMessage = "spnTimeZoneErrorMsg|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string TimeZone { get; set; }
        public IEnumerable<SelectListItem> TimeZoneList { get; set; }
        public IEnumerable<SelectListItem> CreatorList { get; set; }
        public IEnumerable<SelectListItem> NoStockDefaultAccountList { get; set; }
        public bool PMLibrary { get; set; }
        public bool IsondemandWorkorderaccess { get; set; }
        //changes for  V2-576
        public bool IsUsePlanning { get; set; }
        public string PrintType { get; set; }
        public IEnumerable<SelectListItem> PrintTypeList { get; set; }


        public string MobileWOTimer { get; set; }
        public IEnumerable<SelectListItem> MobileWOTimerList { get; set; }
        #region Bill To 
        public string BillToName { get; set; }
        public string BillToAddress1 { get; set; }
        public string BillToAddress2 { get; set; }
        public string BillToAddress3 { get; set; }
        public string BillToComment { get; set; }
        public string BillToCity { get; set; }
        public string BillToState { get; set; }
        public string BillToPostalCode { get; set; }
        public string BillToCountry { get; set; }
        #endregion

        #region Add Purchasing
        //[Required(ErrorMessage = "spnPRPrefixErrorMsg|" + LocalizeResourceSetConstants.Global)]
        public string PRPrefix { get; set; }
        //[Required(ErrorMessage = "spnPOPrefixErrorMsg|" + LocalizeResourceSetConstants.Global)]
        public string POPrefix { get; set; }
        public string AutoPurchPrefix { get; set; }
        public long? CreatorID { get; set; }
        public long? NonStockDefaultAccount { get; set; }
        public bool Isincludeinautopurchasing { get; set; }
        #region V2-820
        public bool IsIncludePRPreview { get; set; }
        public long? ShoppingCartReviewDefault { get; set; }
        public bool IsShoppingCartIncludeBuyer { get; set; }
        public long? DefaultBuyer { get; set; }
        public IEnumerable<SelectListItem> DefaultReviewerList { get; set; }
        public IEnumerable<SelectListItem> DefaultBuyerList { get; set; }

        #endregion
        #region V2-894
        public bool IsOnOrderCheck { get; set; }
        #endregion
        #endregion

        #region Ship To 
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToPostalCode { get; set; }
        public string ShipToCountry { get; set; }
        #endregion
        public string ImageUrl { get; set; }
        public UserData _userdata { get; set; }
        public string PackageLevel { get; set; }
        public long SiteId { get; set; }

        public bool ClientOnPremise { get; set; }
        #region AssetGroup
        [Unlike("AssetGroup2Name", "AssetGroup3Name", ErrorMessage = "AssetgrpDupValMsg|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string AssetGroup1Name { get; set; }

        [Unlike("AssetGroup1Name", "AssetGroup3Name", ErrorMessage = "AssetgrpDupValMsg|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string AssetGroup2Name { get; set; }

        [Unlike("AssetGroup1Name", "AssetGroup2Name", ErrorMessage = "AssetgrpDupValMsg|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string AssetGroup3Name { get; set; }
        #endregion

        //V2-676
        public string WOBarCode { get; set; }

        public IEnumerable<SelectListItem> WOBarCodeList { get; set; }
        #region *****V2-720
        public ApprovalGroupSettingsModel approvalGroupSettingsModel { get; set; }
        public string tabType { get; set; }
        #endregion
        public Security security { get; set; }
        #region V2-948
        public bool SourceAssetAccount { get; set; }
        #endregion
        #region V2-1032
        public bool SingleStockLineItem { get; set; }
        #endregion

        #region V2-1061
        public bool InvoiceVarianceCheck { get; set; }
        public int ? InvoiceVariance { get; set; }
        #endregion
    }
}