using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SOMAX.G4.Presentation.Common
{
    /// <summary>
    /// This class is basically used for PreventiveMaintenance WorkOrder Generation Thread Process parameter
    /// </summary>
    public class PMWorkOrderGenerationThreadParameter
    {
        public readonly string Cachekey = "LastWOId";
        /// <summary>
        /// 
        /// </summary>
        public PMWorkOrderGenerationThreadParameter()
        {
            this.PrevMaintBatchEntryIdList = new List<int>();
            this.httpContext = null;
            this.UserData = null;
            this.IsBusy = false;
        }

        public void Clear()
        {
            this.PrevMaintBatchEntryIdList.Clear();
            this.httpContext.Cache.Remove(this.Cachekey);
            this.IsBusy = false;
            //Do not clear userdata 
            //this.UserData = null;
        }

        public List<int> PrevMaintBatchEntryIdList { get; set; }
        public Object UserData { get; set; }
        public System.Web.HttpContext httpContext { get; set; }
        public bool IsBusy { get; set; }
    }



    public class workorderReturn
    {
        public string Batch { get; set; }
        public string WorkOrderCreated { get; set; }
        public string CurrentWorkOrder { get; set; }
        public string pagethreadStatus { get; set; }
    }

    /// <summary>
    /// enum for thread state
    /// </summary>
    [DefaultValue(0)]
    public enum ThreadStatus
    {
        None = 0,
        Started = 1,
        Completed = 2,
        Cancled = 3,

    } 
}
