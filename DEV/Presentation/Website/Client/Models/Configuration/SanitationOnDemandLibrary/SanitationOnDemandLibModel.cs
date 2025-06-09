using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.SanitationOnDemandLibrary
{
    public class SanitationOnDemandLibModel
    {
        public long ClientId { get; set; }
        public long SanOnDemandMasterId { get; set; }
        public long SiteId { get; set; }
        [Required(ErrorMessage = "spnOnDemandIDErrorMsg|" + LocalizeResourceSetConstants.LibraryDetails)]
        [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "spnAlphabetsNumbersAllowed|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookUpId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool InactiveFlag { get; set; }
        public bool Del { get; set; }
        public int UpdateIndex { get; set; }
        public DateTime? CreateDate { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
    }
}