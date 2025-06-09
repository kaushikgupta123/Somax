using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DashboardContent_GetAllV2 : DashboardContent_TransactionBaseClass
    {
        public List<b_DashboardContent> DashboardContentList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }

        public override void PerformWorkItem()
        {
            List<b_DashboardContent> tempList = null;
            base.UseTransaction = false;
            DashboardContent.GetAllDashboardContent(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            this.DashboardContentList = tempList;
        }
    }
}
