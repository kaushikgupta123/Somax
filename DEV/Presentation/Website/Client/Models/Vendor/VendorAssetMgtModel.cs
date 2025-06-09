using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class VendorAssetMgtModel
    {
        public long VendorId { get; set; }
        public long VendorAssetMgtId { get; set; }
        [Required(ErrorMessage = "CompanyValidationErrmsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Company { get; set; }
        public string Contact { get; set; }
        public string Contract { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public long AssetMgtSource { get; set; }
        public int TotalCount { get; set; }
        public bool AssetMgtRequired { get; set; }
        public DateTime? AssetMgtExpireDate { get; set; }
        public bool AssetMgtOverride { get; set; }
        public DateTime? AssetMgtOverrideDate { get; set; }
        #region
        public DateTime? EffectiveDate { get; set; }
        public DateTime? PKGContactorRecBy { get; set; }
        public DateTime? PKGReceiveBy { get; set; }
        public DateTime? PKGSent { get; set; }
        public string SentVia { get; set; }
        #endregion
    }
}