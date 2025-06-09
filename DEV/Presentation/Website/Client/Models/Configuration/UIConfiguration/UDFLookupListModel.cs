using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.UIConfiguration
{
    public class UDFLookupListModel
    {
        public long LookupListId { get; set; }
        [Required(ErrorMessage = "validationenterDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter value")]
        public string ListValue { get; set; }
        public bool InactiveFlag { get; set; }
        public string ListName { get; set; }
        public string ErrorMessage { get; set; }
        public int UpdateIndex { get; set; }
    }
}