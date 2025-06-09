using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.MasterSanitationSchedule
{
    public class MasterSanitTaskModel
    {
        public long ClientId { get; set; }
        public long SanitationMasterTaskId { get; set; }
        public long SanitationMasterId { get; set; }
        public long ProcedureMasterId { get; set; }
        public long ProcedureMasterTaskId { get; set; }
        [Required(ErrorMessage = "spnReqDescription|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
        public string TaskId { get; set; }
        public string PerformTime { get; set; }
        public int UpdateIndex { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string OrderNumber { get; set; }
        public string ProcedureMaster_ClientLookupId { get; set; }
    }
}