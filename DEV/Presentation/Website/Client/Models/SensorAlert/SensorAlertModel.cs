using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.SensorAlert
{
    public class SensorAlertModel
    {
        public long ClientId { get; set; }
        public long SensorAlertProcedureId { get; set; }
        public long SiteId { get; set; }
        [Required(ErrorMessage = "spnOnDemandIDErrorMessage|" + LocalizeResourceSetConstants.SensorAlertDetails)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "spnAlphabetsNumbersAllowed|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookUpId { get; set; }
        [Required(ErrorMessage = "spnDescriptionErrorMessage|" + LocalizeResourceSetConstants.SensorAlertDetails)]
        public string Description { get; set; }
        public string Type { get; set; }
        public bool InactiveFlag { get; set; }
        public bool Del { get; set; }
        public int UpdateIndex { get; set; }
        public long Creator_PersonneIId { get; set; }
        public DateTime? CreateDate { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public long TaskId { get; set; }
        #region V2-536
        public int TotalCount { get; set; }
        #endregion
    }
}