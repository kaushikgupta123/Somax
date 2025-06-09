using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.LaborScheduling
{
    public class LaborSchedulingModel
    {

        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "invalid input")]
        [Range(1, 99999999.99, ErrorMessage = "Invalid Input")]
        [Required(ErrorMessage = "Please enter hours")]
        public decimal? Hours { get; set; }

        [Required(ErrorMessage = "Please select Assigned Id")]
        public long PersonnelId { get; set; }
        [Required(ErrorMessage = "Please enter Date")]
        public DateTime? Date { get; set; }
        public IEnumerable<SelectListItem> PersonnelIdList { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public long WorkOrderSchedId { get; set; }
        public int PartsOnOrder { get; set; } //V2-838
    }



}