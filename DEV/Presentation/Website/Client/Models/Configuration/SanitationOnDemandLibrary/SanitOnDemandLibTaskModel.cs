using Common.Constants;
using System.ComponentModel.DataAnnotations;
namespace Client.Models.Configuration.SanitationOnDemandLibrary
{
    public class SanitOnDemandLibTaskModel
    {
        public long ClientId { get; set; }
        public long SanOnDemandMasterTaskId { get; set; }
        public long SanOnDemandMasterId { get; set; }
        [Required(ErrorMessage = "spnOrderErrorMsg|" + LocalizeResourceSetConstants.LibraryDetails)]
        public string TaskId { get; set; }
        [Required(ErrorMessage = "spnDescriptionErrorMsg|" + LocalizeResourceSetConstants.LibraryDetails)]
        public string Description { get; set; }
        public bool Del { get; set; }
        public int UpdateIndex { get; set; }
        public string ClientLookUpId { get; set; }
    }
}