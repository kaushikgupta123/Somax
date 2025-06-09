using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class VendorInsuranceModel
    {
        public long VendorId { get; set; }
        public long VendorInsuranceId { get; set; }
        [Required(ErrorMessage = "CompanyValidationErrmsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Company { get; set; }
        public string Contact { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? Amount { get; set; }
        public long Vendor_InsuranceSource { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public int TotalCount { get; set; }
        public bool AdditionalInsurance { get; set; }
        public int? Amount_Medical { get; set; }
        public int? Amount_OCC { get; set; }
        public int? Amount_PIN { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string InsuranceCertificate { get; set; }
        public bool LiabilityAgreement { get; set; }
        public DateTime? PKGContractorRecBy { get; set; }
        public DateTime? PKGReceiveBy { get; set; }
        public DateTime? PKGSent { get; set; }
        public string SentVia { get; set; }
        public bool PreQualifySurvey { get; set; }
    }
}