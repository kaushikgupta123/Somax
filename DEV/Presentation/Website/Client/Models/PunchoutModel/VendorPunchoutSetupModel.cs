using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{
    public class VendorPunchoutSetupModel
    {
        public long VendorId { get; set; }
        public bool PunchoutIndicator { get; set; }
        public string VendorDomain { get; set; }
        public string VendorIdentity { get; set; }
        public string SharedSecret { get; set; }
        public string SenderDomain { get; set; }
        public string SenderIdentity { get; set; }
        [Url(ErrorMessage ="Please enter a valid url.")]
        public string PunchoutURL { get; set; }
        public bool AutoSendPunchOutPO { get; set; }
        public IEnumerable<SelectListItem> VendorDomainList { get; set; }
        public IEnumerable<SelectListItem> SenderDomainList { get; set; }
        //V2-582
        public string SendPunchoutPOURL { get; set; }
        //V2-587
        public string SendPunchoutPOEmail { get; set; }
    }
}