using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
   
    public class WoCompletionSettings_RetrieveByClientId : WOCompletionSettings_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            WOCompletionSettings.RetrieveByClientId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
}
