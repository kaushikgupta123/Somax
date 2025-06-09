using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
   public class PartTransferStatus
    {
        public const string Open = "Open";
        public const string Waiting = "Waiting";
        public const string InTransit = "InTransit";
        public const string Complete = "Complete";
        public const string Canceled = "Canceled";
        public const string Denied = "Denied";
        public const string ForceCompPend = "ForceCompPend";
        public const string Issue = "Issue";
        public const string Receipt = "Receipt";
        public const string Sent = "Sent";
        public const string ForceComplete = "ForceComplete";
    }
}
