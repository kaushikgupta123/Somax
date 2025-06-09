using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Client.Models.Parts
{
    public class PartMasterModel
    {
        public long PartMasterId { get; set; }
        public string ClientLookupId { get; set; }
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public long PartId { get; set; }

    }
}