using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.StoreroomTransfer
{
    public class AddTransferRequest
    {
        public long PartId { get; set; }
        [Required(ErrorMessage = "Please Select Part Id")]
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public long RequestPartStoreroomId { get; set; }
        public long RequestStoreroomId { get; set; }
        public string RequestStoreroomName { get; set; }
        public long IssueStoreroomId { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Required(ErrorMessage = "Please Enter Request Quantity")]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(1, 999999999.999999, ErrorMessage = "GlobalMaxRangeOneToNieDigitWithSixDecimalPlace|" + LocalizeResourceSetConstants.PartDetails)]
        public decimal? RequestQuantity { get; set; }
        public string Reason { get; set; }
        [Required(ErrorMessage = "spnStoreroomErrorMsg|" + LocalizeResourceSetConstants.Global)]
        public string IssuePartStoreroomIdAndStoreroomId { get; set; }
        public long StoreroomTransferId { get; set; }
    }
}