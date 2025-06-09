using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class AutoTRGenerationVM : LocalisationBaseVM
    {
        public string StoreroomIds { get; set; }
        public bool IsMaintain { get; set; }
    }
}