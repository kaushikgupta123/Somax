using Client.Models.Work_Order;
using DevExpress.DataAccess.ObjectBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseRequest
{
    [HighlightedClass]
    public class PurchaseRequestDevExpressPrintDataSource
    {
        public List<PurchaseRequestDevExpressPrintModel> PurchaseRequestDevExpressPrintModelList { get; set; }
    }
}