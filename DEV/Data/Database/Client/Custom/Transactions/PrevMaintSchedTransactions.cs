using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;

namespace Database.Client.Custom.Transactions
{
    public class PrevMaintSched_RetrieveByEquipmentId : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

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
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrieveByEquipmentIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }

    public class PrevMaintSched_RetrieveByBIMGuid : PrevMaintSched_TransactionBaseClass
    {
      public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }
      public Guid BIMGuid { get; set; }

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
        List<b_PrevMaintSched> tmpArray = null;

        PrevMaintSched.RetrieveByBIMGuidFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, BIMGuid, ref tmpArray);

        PrevMaintSchedList = new List<b_PrevMaintSched>();
        foreach (b_PrevMaintSched tmpObj in tmpArray)
        {
          PrevMaintSchedList.Add(tmpObj);
        }
      }
    }

  
  public class PrevMaintSched_RetrieveByLocationId : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

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
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrieveByLocationIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }

    public class PrevMaintSched_RetrieveByPrevMaintMasterId : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

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
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrieveByPrevMaintMasterIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }

    public class PrevMaintSched_RetrieveByPrevMaintMasterId_V2 : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrieveByPrevMaintMasterIdFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }

    public class PrevMaintSched_ValidateByClientLookupId : PrevMaintSched_TransactionBaseClass
    {
        public PrevMaintSched_ValidateByClientLookupId()
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
            List<b_StoredProcValidationError> errors = null;

            PrevMaintSched.ValidateByClientLookupIdFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

            StoredProcValidationErrorList = errors;
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

    public class PrevMaintSched_CreateByPKForeignKeys : PrevMaintSched_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintSched.PrevMaintSchedId > 0)
            {
                string message = "PrevMaintSched has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PrevMaintSched.InsertByPKForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PrevMaintSched.PrevMaintSchedId > 0);
        }
    }
    public class PrevMaintSched_CreateByPKForeignKeys_V2 : PrevMaintSched_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintSched.PrevMaintSchedId > 0)
            {
                string message = "PrevMaintSched has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PrevMaintSched.InsertByPKForeignKeysIntoDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PrevMaintSched.PrevMaintSchedId > 0);
        }
    }

    public class PrevMaintSched_RetrieveByPKForeignKeys : PrevMaintSched_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintSched.PrevMaintSchedId == 0)
            {
                string message = "PrevMaintSched has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PrevMaintSched.RetrieveByPKForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class PrevMaintSched_RetrieveByPKForeignKeys_V2 : PrevMaintSched_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintSched.PrevMaintSchedId == 0)
            {
                string message = "PrevMaintSched has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PrevMaintSched.RetrieveByPKForeignKeysFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class PrevMaintSched_RetrievePMSchedulingRecords : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

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
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrievePMSchedulingRecordsFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }
    //SOM-1018
    public class PrevMaintSched_PMCalendarForecast : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

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
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrievePMCalendarForecastFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }

    //--SOM-1669--//
    public class PrevMaintSched_PMCalendarForecastFromPrevMaintLibrary : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

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
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrievePMCalendarForecastFromPrevMaintLibrary(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }

    public class PrevMaintSched_PMOnDemandForecast : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

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
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrievePMOnDemandForecastFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }

    //--SOM-1669--//
    public class PrevMaintSched_PMOnDemandForecastFromPrevMaintLibrary : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

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
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrievePMOnDemandForecastFromPrevMaintLibrary(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }

    public class PrevMaintSched_UpdateByPKForeignKeys : PrevMaintSched_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintSched.PrevMaintSchedId == 0)
            {
                string message = "PrevMaintSched has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PrevMaintSched.UpdateByPKForeignKeysInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class PrevMaintSched_UpdateByPKForeignKeys_V2 : PrevMaintSched_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintSched.PrevMaintSchedId == 0)
            {
                string message = "PrevMaintSched has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PrevMaintSched.UpdateByPKForeignKeysInDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class PrevMaintSched_UpdateAssignToPersonnelByPrevMaintSchedId : PrevMaintSched_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintSched.PrevMaintSchedId == 0)
            {
                string message = "PrevMaintSched has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PrevMaintSched.UpdateAssignToPersonnelByPrevMaintSchedId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class PrevMaintSched_ValidateInsert : PrevMaintSched_TransactionBaseClass
    {
        public PrevMaintSched_ValidateInsert()
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
            List<b_StoredProcValidationError> errors = null;
            
            PrevMaintSched.ValidateProcessFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

            StoredProcValidationErrorList = errors;
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

    //V2-739//
    public class PrevMaintSched_PMCalendarForecastFromPrevMaintLibrary_ConsiderExcludeDOW : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }
        public string timeoutErrors  { get; set; }
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
            List<b_PrevMaintSched> tmpArray = null;
            string timeoutError = null;
            PrevMaintSched.RetrievePMCalendarForecastFromPrevMaintLibrary_ConsiderExcludeDOW(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray ,ref timeoutError);
              timeoutErrors = timeoutError;
                PrevMaintSchedList = new List<b_PrevMaintSched>();
                foreach (b_PrevMaintSched tmpObj in tmpArray)
                {
                    PrevMaintSchedList.Add(tmpObj);
                }
          
           
        }
    }
    #region V2-712
    public class PrevMaintSched_PMOnDemandForecastFromPrevMaintLibrary_V2 : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }
        public string timeoutErrors { get; set; }
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
            List<b_PrevMaintSched> tmpArray = null;
            string timeoutError = null;
            PrevMaintSched.RetrievePMOnDemandForecastFromPrevMaintLibrary_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray,ref timeoutError);
            timeoutErrors = timeoutError;
            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }
    #endregion
    #region V2-1005
    public class PrevMaintSched_Delete_V2 : PrevMaintSched_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            PrevMaintSched.PrevMaintSchedDelete_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    #endregion

    #region V2-1212
    public class PrevMaintSched_RetrieveByPrevMaintMasterIdChunkSeaarch_V2 : PrevMaintSched_TransactionBaseClass
    {
        public List<b_PrevMaintSched> PrevMaintSchedList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_PrevMaintSched> tmpArray = null;

            PrevMaintSched.RetrieveByPrevMaintMasterIdChunckSearchFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintSchedList = new List<b_PrevMaintSched>();
            foreach (b_PrevMaintSched tmpObj in tmpArray)
            {
                PrevMaintSchedList.Add(tmpObj);
            }
        }
    }
    #endregion
}
