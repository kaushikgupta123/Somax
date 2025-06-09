using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTDataLayer.EL
{
    public class SiteEL
    {
        public long ClientId { set; get; }
        public long SiteId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string BIMURN { set; get; }
        public int UpdateIndex { set; get; }
    }
}
