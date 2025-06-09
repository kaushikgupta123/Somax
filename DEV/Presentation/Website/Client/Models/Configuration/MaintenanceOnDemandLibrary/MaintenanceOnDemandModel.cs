using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.MaintenanceOnDemandLibrary
{
    public class MaintenanceOnDemandModel
    {       
        public long MaintOnDemandMasterId {get; set;}
        [Required(ErrorMessage = "spnOnDemandIDErrorMsg|" + LocalizeResourceSetConstants.LibraryDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "spnOnDemandIDcontainsinvalidcharacters|" + LocalizeResourceSetConstants.LibraryDetails)]
        public string ClientLookUpId {get;set;}        
        [Required(ErrorMessage = "spnDescriptionErrorMsg|" + LocalizeResourceSetConstants.LibraryDetails)]
        public string Description {get;set;}     
        public string Type {get;set;}
        public DateTime CreateDate { get; set; }
        public long MasterIdForCancel { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; } 
    }
}