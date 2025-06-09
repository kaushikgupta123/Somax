using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.ShipToAddress
{
    public class ShipToModel
    {
        [Required(ErrorMessage = "globalValidShipToAddressId|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "spnShipToIDcontainsinvalidcharacters|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }
        public long ShipToId { get; set; }
        public string AttnName { get; set; }
        [EmailAddress(ErrorMessage = "validEmailRequiredMessage|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressCountry { get; set; }
        public int TotalCount { get; set; }
    }
}