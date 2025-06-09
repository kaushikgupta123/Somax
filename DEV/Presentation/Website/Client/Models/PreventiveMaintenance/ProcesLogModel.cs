using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PreventiveMaintenance
{
    public class ProcesLogModel
    {
        public int ItemsReviewed { get; set; }
        public int HeadersCreated { get; set; }
        public int DetailsCreated { get; set; }
    }
}