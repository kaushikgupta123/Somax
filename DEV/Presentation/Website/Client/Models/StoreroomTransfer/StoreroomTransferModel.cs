using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Constants;
using Client.CustomValidation;

namespace Client.Models.StoreroomTransfer
{
    public class StoreroomTransferModel
    {
        public long StoreroomTransferId { get; set; }
        public string PartClientLookupId { get; set; }
        public string PartDescription { get; set; }
        public decimal QuantityIssued { get; set; }
        public string Status { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal RequestQuantity { get; set; }
        public string StoreroomName { get; set; }
        public int TotalCount { get; set; }
        public long StoreroomId { get; set; }
        public long CustomQueryDisplayId { get; set; }
    }
}