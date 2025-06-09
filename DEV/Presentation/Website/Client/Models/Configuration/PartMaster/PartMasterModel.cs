using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Configuration.PartMaster
{
    public class PartMasterModel
    {
        public long PartMasterId { get; set; }
        [Required(ErrorMessage = "spnOnPartIDErrorMsg|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "spnOnPartIDcontainsinvalidcharacters|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        public string ClientLookupId { get; set; }
        public bool OEMPart { get; set; }
        public long? EXPartId { get; set; }
        public string EXAltPartId1 { get; set; }
        public string EXAltPartId2 { get; set; }
        public string EXAltPartId3 { get; set; }
        public System.Guid ExUniqueId { get; set; }
        public bool InactiveFlag { get; set; }
        public string LongDescription { get; set; }
        //[Required(ErrorMessage = "Please Select Manufacturer")]
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }

        [Required(ErrorMessage = "spnValidationShortDescription|" + LocalizeResourceSetConstants.ConfigMasterDetail)]
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }
        //[Required(ErrorMessage = "Please Select Unit Of Measure")]
        public string UnitOfMeasure { get; set; }

        //[Required(ErrorMessage = "Please Select Category")]
        public string Category { get; set; }
        public string UPCCode { get; set; }
        public string ImageURL { get; set; }

        public bool SXPart { get; set; }
        public string UnitofMeasureDescription { get; set; }

        /// <summary>
        /// //////////////////
        /// </summary>
        public string Inactive { get; set; }
        public string CategoryDescription { get; set; }
        public long Siteid { get; set; }
        public long PartId { get; set; }
        public long VendorId { get; set; }
        public string PartClientLookupId { get; set; }
        public string PartDescription { get; set; }
        public string PartNumber { get; set; }
        public string CategoryMasterDescription { get; set; }
        public bool ClientOnPremise { get; set; }
        public IEnumerable<SelectListItem> CategoryDescriptionList { get; set; }
        public IEnumerable<SelectListItem> UnitofMeasureList { get; set; }
        public IEnumerable<SelectListItem> ManufacturerList { get; set; }
    }
}