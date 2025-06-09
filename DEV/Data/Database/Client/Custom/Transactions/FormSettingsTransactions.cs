using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class FormSettings_RetrieveByClientId : FormSettings_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            FormSettings.RetrieveByClientId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
}
