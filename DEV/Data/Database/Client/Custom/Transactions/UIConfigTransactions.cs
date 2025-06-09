using Common.Enumerations;
using Common.Structures;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class    UIConfig_RetrieveHiddenByViewOrTable : UIConfig_TransactionBaseClass
    {
        public List<b_UIConfig> UIConfigList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (UIConfig.UIConfigId == 0)
            //{
            //    string message = "UIConfig has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            List<b_UIConfig> tempList = null;
            base.UseTransaction = false;
            UIConfig.RetrieveHiddenByViewOrTable(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            this.UIConfigList = tempList;
        }
    }
}
