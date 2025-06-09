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
    public class StoreroomTransferProcessIssuesModel
    {
        public long StoreroomTransferId { get; set; }
        public decimal IssueQty { get; set; }
    }
}