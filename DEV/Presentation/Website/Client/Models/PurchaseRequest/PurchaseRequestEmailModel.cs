using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseRequest
{
    public class PurchaseRequestEmailModel
    {
        [Required(ErrorMessage = "Please enter an email id")]
        [EmailAddress(ErrorMessage = "validEmailRequiredMessage|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string ToEmailId { get; set; }
        [RegularExpression("^((\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*)\\s*[,]{0,1}\\s*)+$",ErrorMessage = "validEmailRequiredMessage|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string CcEmailId { get; set; }
        public string MailBodyComments { get; set; }
    }
}