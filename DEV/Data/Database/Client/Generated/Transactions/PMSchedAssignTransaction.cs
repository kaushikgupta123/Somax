using Common.Enumerations;
using Database.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Transactions
{
    public class PMSchedAssign_TransactionBaseClass : AbstractTransactionManager
    {
        public PMSchedAssign_TransactionBaseClass()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PMSchedAssign == null)
            {
                string message = "PMSchedAssign has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;


            // Explicitly set tenant id from dbkey
            this.PMSchedAssign.ClientId = this.dbKey.Client.ClientId;

        }

        public b_PMSchedAssign PMSchedAssign { get; set; }
        public b_ChangeLog ChangeLog { get; set; }

        public override void PerformWorkItem()
        {
            // 
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

    public class PMSchedAssign_Create : PMSchedAssign_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PMSchedAssign.PMSchedAssignId > 0)
            {
                string message = "PMSchedAssign has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PMSchedAssign.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PMSchedAssign.PMSchedAssignId > 0);
        }
    }

    public class PMSchedAssign_Retrieve : PMSchedAssign_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PMSchedAssign.PMSchedAssignId == 0)
            {
                string message = "PMSchedAssign has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PMSchedAssign.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class PMSchedAssign_Update : PMSchedAssign_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PMSchedAssign.PMSchedAssignId == 0)
            {
                string message = "PMSchedAssign has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PMSchedAssign.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class PMSchedAssign_Delete : PMSchedAssign_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PMSchedAssign.PMSchedAssignId == 0)
            {
                string message = "PMSchedAssign has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PMSchedAssign.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class PMSchedAssign_RetrieveAll : AbstractTransactionManager
    {

        public PMSchedAssign_RetrieveAll()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_PMSchedAssign> PMSchedAssignList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            b_PMSchedAssign[] tmpArray = null;
            b_PMSchedAssign o = new b_PMSchedAssign();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;


            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PMSchedAssignList = new List<b_PMSchedAssign>(tmpArray);
        }
    }
}
