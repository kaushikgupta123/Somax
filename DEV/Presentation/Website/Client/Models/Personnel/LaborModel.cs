using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Personnel
{
    public class LaborModel
    {
        public long PersonnelId { get; set; }
        public long TimecardId { get; set; }

        public string PersonnelClientLookupId { get; set; }
        public string ChargeTo { get; set; }        
        public decimal Hours { get; set; }
        public decimal Cost { get; set; }             
        public DateTime? Date { get; set; }        
    }
}