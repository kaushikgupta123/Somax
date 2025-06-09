using System;
using System.Collections.Generic;
using System.Linq;

namespace SOMAX.G4.Data.DataContracts
{
    public class WebServiceResponseData
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionType { get; set; }

    }
}
