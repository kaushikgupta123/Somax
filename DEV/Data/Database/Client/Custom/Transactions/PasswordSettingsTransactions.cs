using Common.Enumerations;
using Database;
using Database.Business;
using Database.StoredProcedure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database
{
 
        public class PasswordSettings_UpdateByClientId_V2 : PasswordSettings_TransactionBaseClass
        {


            public List<b_PasswordSettings> PasswordSettingsList { get; set; }


            public override void PerformLocalValidation()
            {
                base.PerformLocalValidation();
            }

            public override void PerformWorkItem()
            {
                List<b_PasswordSettings> tmpList = new List<b_PasswordSettings>();

            PasswordSettings.UpdatePasswordSettingsByClientId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
                if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
            }

            public override void Postprocess()
            {
                base.Postprocess();
            }

        }

    public class PasswordSettings_RetrieveByClientId : PasswordSettings_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PasswordSettings.RetrieveByClientId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

}
