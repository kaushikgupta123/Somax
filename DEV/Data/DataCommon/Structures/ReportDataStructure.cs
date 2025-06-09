using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Structures
{
    public struct ReportDataStructure
    {
        public string GroupByKey;
        public List<string> RowTextEntries;
        public List<decimal> RowNumericEntries;
    }
}
