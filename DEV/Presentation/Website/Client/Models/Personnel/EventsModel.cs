using System;
using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Personnel
{
    public class EventsModel
    {
        public long PersonnelId { get; set; }
        public long EventsId { get; set; }

        public string PersonnelClientLookupId { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "TypeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }       
        public DateTime? ExpireDate { get; set; }

        [Required(ErrorMessage = "DateErrMsg|" + LocalizeResourceSetConstants.Global)]
        public DateTime? CompleteDate { get; set; }        
    }
}