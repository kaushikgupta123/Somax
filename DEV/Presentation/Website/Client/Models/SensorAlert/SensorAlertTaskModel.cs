using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.SensorAlert
{
    public class SensorAlertTaskModel
    {
        public long SensorAlertProcedureTaskId { get; set; }
        public long SensorAlertProcedureId { get; set; }
         [Required(ErrorMessage = "spnOrderErrorMessage|" + LocalizeResourceSetConstants.SensorAlertDetails)]
        public string TaskId { get; set; }
        [Required(ErrorMessage = "spnDescriptionErrorMessage|" + LocalizeResourceSetConstants.SensorAlertDetails)]
        public string Description { get; set; }
        public int UpdateIndex { get; set; }
        public string Inactive { get; set; }
        public string ClientLookUpId { get; set; }
    }
}