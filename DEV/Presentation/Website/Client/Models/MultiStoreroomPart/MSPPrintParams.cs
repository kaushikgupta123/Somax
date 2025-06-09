using Client.Models.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MultiStoreroomPart
{
    [Serializable]
    public class MSPPrintParams
    {      
        public MSPPrintParams()
        {
            tableHaederProps = new List<TableHaederProp>();
        }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public string colname { get; set; }
        public string coldir { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string PartID { get; set; }
        public string Description { get; set; }
        public string Manufracturer { get; set; }
        public string ManufracturerID { get; set; }
        public string Section { get; set; }
        public string StockType { get; set; }
        public string Row { get; set; }
        public string Bin { get; set; }
        public string Shelf { get; set; }
        public string txtSearchval { get; set; }
        public List<string> Storerooms { get; set; }
    }
}