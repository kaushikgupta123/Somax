using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOMAX.G4.Presentation.Common
{
    /// <summary>
    /// This class is basically used for Sanitation Job Generation Thread Process parameter
    /// </summary>
    public class SanitationJobGenerationThreadParameter
    {
        public readonly string Cachekey = "SanitationJobId";

        /// <summary>
        /// Constructor
        /// </summary>
        public SanitationJobGenerationThreadParameter()
        {
            this.SanitationJobBatchEntryIdList = new List<int>();
            this.httpContext = null;
            this.UserData = null;
            this.IsBusy = false;
        }

        public void Clear()
        {
            this.SanitationJobBatchEntryIdList.Clear();
            this.httpContext.Cache.Remove(this.Cachekey);
            this.IsBusy = false;
            //Do not clear userdata 
            //this.UserData = null;
        }

        public List<int> SanitationJobBatchEntryIdList { get; set; }
        public Object UserData { get; set; }
        public System.Web.HttpContext httpContext { get; set; }
        public bool IsBusy { get; set; }
    }

    public class sanitationReturn
    {
        public string Batch { get; set; }
        public string JobCreated { get; set; }
        public string CurrentJob { get; set; }
        public string pagethreadStatus { get; set; }
    }
}
