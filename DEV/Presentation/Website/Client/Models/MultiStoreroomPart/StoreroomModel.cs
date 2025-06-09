
using Common.Constants;

using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.MultiStoreroomPart
{
    public class StoreroomModel
    {
        public long Id { get; set; }
        public long PartId { get; set; }
        public long PartStoreroomId { get; set; }
        [Required(ErrorMessage = "spnStoreroomErrorMsg|" + LocalizeResourceSetConstants.Global)]
        public long StoreroomId { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public string Shelf { get; set; }
        public string Bin { get; set; }
        public string Section2 { get; set; }
        public string Row2 { get; set; }
        public string Shelf2 { get; set; }
        public string Bin2 { get; set; }
        [Range(0, 9999999.99, ErrorMessage = "globalTwoDecimalAfterTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QuantityOnHand { get; set; }
        [Range(0, 9999999.99, ErrorMessage = "globalTwoDecimalAfterTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MaximumQuantity { get; set; }
        [Range(0, 9999999.99, ErrorMessage = "globalTwoDecimalAfterTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? MinimumQuantity { get; set; }
        public int CountFrequency { get; set; }
        public DateTime? LastCounted { get; set; }
        public bool Maintain { get; set; }
        public string ErrorMessages { get; set; }
        public bool Critical { get; set; }
        public bool AutoPurchase { get; set; }
        public long ? PartVendorId { get; set; }
        public string PartVendorIdClientLookupId { get; set; }
    }
}