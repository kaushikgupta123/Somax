using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Parts
{
    public class PartBulkUpdateModel
    {
        public long PartId { get; set; }

        //[Required]
        public string StockType { get; set; }

        //[Required]
        public string IssueUnit { get; set; }

        //[Required]
        public string Manufacturer { get; set; }

        //[Required]
        public string ManufacturerID { get; set; }

        //[Required]
        public long? AccountId { get; set; }
        public string Account_ClientLookupId { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public IEnumerable<SelectListItem> LookupStokeTypeList { get; set; }
        public IEnumerable<SelectListItem> LookupIssueUnitList { get; set; }
        public string PartIdList { get; set; }
    }
}