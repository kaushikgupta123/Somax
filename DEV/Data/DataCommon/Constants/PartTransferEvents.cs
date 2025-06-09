using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
   public class PartTransferEvents
    {
        public const string Created = "Created";
        public const string Sent = "Sent";
        public const string Issue = "Issue";
        public const string Receipt = "Receipt";
        public const string Complete = "Complete";
        public const string ForceCompPend = "ForceCompPend";
        public const string ForceComplete = "ForceComplete";
        public const string Canceled = "Canceled";
        public const string Denied = "Denied";
    }
}
