using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
   public class AutoGenerateKey
    {
        public const string WO_Annual = "WO_Annual";
        public const string PR_ANNUAL = "PR_ANNUAL";
        public const string PO_ANNUAL = "PO_ANNUAL";
        public const string WO_Task = "WO_Task";
        public const string SANIT_ANNUAL = "SANIT_ANNUAL";
        public const string EQUIP_LOOKUP = "EQUIP_LOOKUP";
        public const string PurchaseRequest_AutoGeneratePrefix = "R";
        public const bool PurchaseRequest_AutoGenerateEnabled= true;
        public const bool WorkOrder_FromWorkOrder_AutoGenerateEnabled = true;
        public const string WorkOrder_FromWorkOrder_AutoGenerateKey = "WO_Annual";
        public const string SO_Annual = "SO_Annual";
        public const string Vendor_LOOKUP = "Vendor_LOOKUP"; //V2-915
        public const string EPM_PO_ANNUAL = "EPM_PO_ANNUAL";//V2-1112

    }
}
