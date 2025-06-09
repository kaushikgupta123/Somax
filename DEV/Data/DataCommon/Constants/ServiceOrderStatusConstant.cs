using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public class ServiceOrderStatusConstant
    {
        public const string Open = "Open";
        public const string Scheduled = "Scheduled";
        public const string AwaitingApproval = "AwaitApproval";
        public const string Complete = "Complete";
        public const string Canceled = "Canceled";
        public const string ReOpen = "ReOpen";
        public const bool So_AutoGenerateEnabled = true;
    }
}
