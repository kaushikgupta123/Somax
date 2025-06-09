using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Client.CustomValidation;
namespace Client.Models.PartCycleCount
{
    public class PartCycleCountModel
    {
        public string Area { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public string Shelf { get; set; }
        public string Bin { get; set; }
        public string StockType { get; set; }
        public bool CriticalFlag { get; set; }
        public bool Consignment { get; set; }
        [Required(ErrorMessage = "DateErrMsg|" + LocalizeResourceSetConstants.Global)]
        public DateTime? GenerateThrough { get; set; }
        public IEnumerable<SelectListItem> StockTypeList { get; set; }
        public bool MultiStoreroom { get; set; }

        [RequiredIf("MultiStoreroom", true, ErrorMessage = "GlobalStoreroomSelect|" + LocalizeResourceSetConstants.Global)]
        public long? StoreroomId { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
    }
}