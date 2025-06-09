using System.Collections.Generic;

namespace Client.Models.Dashboard
{
    public class ScrollViewModel<T>
    {
        public List<T> LookupList { get; set; }
        public List<int> PageNos { get; set; }    
    }

}