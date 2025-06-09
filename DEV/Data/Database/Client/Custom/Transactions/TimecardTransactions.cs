using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;
using Database.StoredProcedure;
using Common.Enumerations;

namespace Database.Client.Custom.Transactions
{
    public class Timecard_RetrieveByWorkOrderId : Timecard_TransactionBaseClass
    {
        public List<b_Timecard> TimecardList { get; set; }

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
            List<b_Timecard> tmpArray = null;

            Timecard.RetrieveByWorkOrderIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            TimecardList = new List<b_Timecard>();
            foreach (b_Timecard tmpObj in tmpArray)
            {
                TimecardList.Add(tmpObj);
            }
        }
    }
    public class Timecard_RetrieveBySanId : Timecard_TransactionBaseClass
    {
        public List<b_Timecard> TimecardList { get; set; }

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
            List<b_Timecard> tmpArray = null;

            Timecard.RetrieveBySanIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            TimecardList = new List<b_Timecard>();
            foreach (b_Timecard tmpObj in tmpArray)
            {
                TimecardList.Add(tmpObj);
            }
        }
    }

    public class Timecard_RetrieveByPersonnelId : Timecard_TransactionBaseClass
    {
        public List<b_Timecard> TimecardList { get; set; }

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
            List<b_Timecard> tmpArray = null;

            Timecard.RetrieveByPersonnelIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            TimecardList = new List<b_Timecard>();
            foreach (b_Timecard tmpObj in tmpArray)
            {
                TimecardList.Add(tmpObj);
            }
        }
    }

    public class Timecard_RetrieveByPKForeignKeys : Timecard_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId == 0)
            {
                string message = "Timecard has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Timecard.RetrieveByPKForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Timecard_ValidateByClientLookupId : Timecard_TransactionBaseClass
    {
        public Timecard_ValidateByClientLookupId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                Timecard.ValidateByClientLookupIdFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
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

    public class Timecard_CreateByForeignKeys : Timecard_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId > 0)
            {
                string message = "Timecard has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Timecard.InsertByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Timecard.TimecardId > 0);
        }
    }

    public class Timecard_UpdateByForeignKeys : Timecard_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId == 0)
            {
                string message = "Timecard has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Timecard.UpdateByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Timecard.TimecardId > 0);
        }
    }

    /********************************
     **
     */
    public class Timecard_RetrieveByPKWithPersonal : Timecard_TransactionBaseClass
    {
        public List<b_Timecard> TimecardList { get; set; }

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
            List<b_Timecard> tmpArray = null;

            Timecard.RetrieveByPKWithPersonal(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            TimecardList = new List<b_Timecard>();
            foreach (b_Timecard tmpObj in tmpArray)
            {
                TimecardList.Add(tmpObj);
            }
        }
    }


    /********************************************************************************/
    public class TimeCard_CreateByForeignKeysForWorkOrderCompletion : Timecard_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId > 0)
            {
                string message = "TimeCard has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Timecard.InsertByForeignKeysForWorkOrderCompletionIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Timecard.TimecardId > 0);
        }
    }

    /*
      // Removed by RKL - Not Needed
      public class TimeCard_CreateTimecardForWorkOrderCompletionByWorkOrderId : Timecard_TransactionBaseClass
      {
          public override void PerformLocalValidation()
          {
              base.PerformLocalValidation();
              if (Timecard.TimecardId > 0)
              {
                  string message = "TimeCard has an invalid ID.";
                  throw new Exception(message);
              }
          }
          public override void PerformWorkItem()
          {
              Timecard.InsertTimeCardWorkOrderCompletionByWorkOrderId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
          }

          public override void Postprocess()
          {
              base.Postprocess();
              System.Diagnostics.Debug.Assert(Timecard.TimecardId > 0);
          }
      }
     */
    //SOM-1249
    public class Timecard_RetrieveBySaniatationId : Timecard_TransactionBaseClass
    {
        public List<b_Timecard> TimecardList { get; set; }

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
            List<b_Timecard> tmpArray = null;

            Timecard.RetrieveBy_SanitationJobIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            TimecardList = new List<b_Timecard>();
            foreach (b_Timecard tmpObj in tmpArray)
            {
                TimecardList.Add(tmpObj);
            }
        }
    }
    public class Timecard_RetrieveByServiceOrderId : Timecard_TransactionBaseClass
    {
        public List<b_Timecard> TimecardList { get; set; }

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
            List<b_Timecard> tmpArray = null;

            Timecard.RetrieveByServiceOrderId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            TimecardList = new List<b_Timecard>();
            foreach (b_Timecard tmpObj in tmpArray)
            {
                TimecardList.Add(tmpObj);
            }
        }
    }
    public class Timecard_CreateByForeignKeys_V2 : Timecard_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId > 0)
            {
                string message = "Timecard has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Timecard.InsertByForeignKeysIntoDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Timecard.TimecardId > 0);
        }
    }
    public class Timecard_UpdateByForeignKeys_V2 : Timecard_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Timecard.TimecardId == 0)
            {
                string message = "Timecard has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Timecard.UpdateByForeignKeysIntoDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Timecard.TimecardId > 0);
        }
    }

    public class Timecard_RetrieveByWorkOrderIdForMaintananceWorkbenchDetails : Timecard_TransactionBaseClass
    {
        public List<b_Timecard> TimecardList { get; set; }

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
            List<b_Timecard> tmpArray = null;

            Timecard.RetrieveByWorkOrderIdForMaintananceWorkbenchDetails(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            TimecardList = new List<b_Timecard>();
            foreach (b_Timecard tmpObj in tmpArray)
            {
                TimecardList.Add(tmpObj);
            }
        }
    }

    public class TimeCard_RetrieveSumOfLabourHours : Timecard_TransactionBaseClass
    {
        public TimeCard_RetrieveSumOfLabourHours()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }          
        public List<b_Timecard> timeCardSumOfHours { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_Timecard> tmpArray = null;
            Timecard.RetrieveSumLabourHours(this.Connection, this.Transaction, this.dbKey.User.UserInfoId, this.dbKey.UserName, ref tmpArray);
            timeCardSumOfHours = new List<b_Timecard>();
            foreach (b_Timecard tmpObj in tmpArray)
            {
                timeCardSumOfHours.Add(tmpObj);
            }
        }

        public override void Preprocess()
        {
            // nothing to do here
        }

        public override void Postprocess()
        {
            // nothing to do here
        }
    }
}
