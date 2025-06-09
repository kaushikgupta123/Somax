
/*
 * 
 */

using System;
using System.Collections.Generic;
using Database.Business;

namespace Database.Transactions
{
    public class RetrieveSystemInfoByClientIdSiteId : SystemInfo_TransactionBaseClass
    {

        public List<b_SystemInfo> SystemInfoList { get; set; }

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
            List<b_SystemInfo> tmpArray = null;
            SystemInfo.RetrieveByClientIdSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            SystemInfoList = new List<b_SystemInfo>();
            foreach (b_SystemInfo tmpObj in tmpArray)
            {
                SystemInfoList.Add(tmpObj);
            }
        }

    }
    public class RetrieveBySystemInfoId : SystemInfo_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SystemInfo.SystemInfoId == 0)
            {
                string message = "SystemInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            SystemInfo.RetrieveSystemInfoId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            
        }

    }



    public class RetrieveByInActiveFlag : SystemInfo_TransactionBaseClass
    {

        public List<b_SystemInfo> SystemInfoList { get; set; }

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
            List<b_SystemInfo> tmpArray = null;
            SystemInfo.RetrieveInActiveFlag(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            SystemInfoList = new List<b_SystemInfo>();
            foreach (b_SystemInfo tmpObj in tmpArray)
            {
                SystemInfoList.Add(tmpObj);
            }
        }

    }
  
    public class ValidateNewClientLookupIdSystemInfoTransaction : SystemInfo_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            SystemInfo.ValidateByNewClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class ValidateOldClientLookupIdSystemInfoTransaction : SystemInfo_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            SystemInfo.ValidateByOldClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class SystemInfo_RetrieveAllCustom : SystemInfo_TransactionBaseClass
    {

        public SystemInfo_RetrieveAllCustom()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = Common.Enumerations.DatabaseTypeEnum.Client;
        }


        public List<b_SystemInfo> SystemInfoList { get; set; }

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
            b_SystemInfo[] tmpArray = null;
            b_SystemInfo o = new b_SystemInfo();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;


            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SystemInfoList = new List<b_SystemInfo>(tmpArray);
        }
    }
}
