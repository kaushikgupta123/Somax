using Client.Models.Parts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartCycleCount
{
    public class PartCycleCountVM : LocalisationBaseVM
    {
        public PartCycleCountModel partCycleCountModel { get; set; }
        public PartModel PartModel { get; set; }
    }
}