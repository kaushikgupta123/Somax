using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class SanitationJobSchedule_RetriveAllBySanitationJobId : SanitationJobSchedule_TransactionBaseClass
    {
        public SanitationJobSchedule_RetriveAllBySanitationJobId()
        {           
            UseDatabase = DatabaseTypeEnum.Client;
        }

  
        public List<b_SanitationJobSchedule> SanitationJobScheduleList { get; set; }

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
            b_SanitationJobSchedule[] tmpArray = null;

            // Explicitly set tenant id from dbkey
            SanitationJobSchedule.ClientId = this.dbKey.Client.ClientId;

            SanitationJobSchedule.RetrieveAllFromDatabaseBy_SanitationJobId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanitationJobScheduleList = new List<b_SanitationJobSchedule>(tmpArray);
        }
    }


    public class SanitationJobSchedule_RetrieveSingleBySanitationJobId : SanitationJobSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJobSchedule.SanitationJobScheduleId == 0)
            {
                string message = "SanitationJobSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SanitationJobSchedule.RetrieveSingleBy_SanitationJobScheduleId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class SanitationJobSchedule_CreateForSanitationJob : SanitationJobSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJobSchedule.SanitationJobScheduleId > 0)
            {
                string message = "SanitationJobSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            SanitationJobSchedule.InsertIntoDatabase_ForSanitationJob(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SanitationJobSchedule.SanitationJobScheduleId > 0);
        }
    }
    public class SanitationJobSchedule_UpdateForSanitationJob : SanitationJobSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJobSchedule.SanitationJobScheduleId == 0)
            {
                string message = "SanitationJobSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SanitationJobSchedule.UpdateInDatabase_ForSanitationJob(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class SanitationJobSchedule_DeleteForSanitationJob : SanitationJobSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJobSchedule.SanitationJobScheduleId == 0)
            {
                string message = "SanitationJobSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SanitationJobSchedule.DeleteFromDatabase_ForSanitationJob(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
}
