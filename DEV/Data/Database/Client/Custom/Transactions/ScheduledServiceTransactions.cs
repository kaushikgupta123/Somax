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
    #region Retrieve
    public class ScheduledService_RetrievecheduledServiceChunkSearchV2 : ScheduledService_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ScheduledService.ScheduledServiceId > 0)
            {
                string message = "ScheduledService has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_ScheduledService tmpList = null;
            ScheduledService.RetrieveScheduledServiceChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
    #region Edit By Id
    public class ScheduledService_RetrieveByEquipmentIdandScheduledServiceId : ScheduledService_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            ScheduledService.RetrieveByEquipmentIdandScheduledServiceIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    #endregion
    #region Insert
    public class ScheduledService_CreateCustom : ScheduledService_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ScheduledService.ScheduledServiceId > 0)
            {
                string message = "ScheduledService has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ScheduledService.InsertIntoDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(ScheduledService.ScheduledServiceId > 0);
        }
    }
    #endregion
    #region Update
    public class ScheduledService_UpdateCustom : ScheduledService_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ScheduledService.ScheduledServiceId == 0)
            {
                string message = "ScheduledService has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ScheduledService.UpdateInDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion

    #region Active/Inactive Sc Service
    public class ScheduledService_RetrieveByForeignKeys_V2 : ScheduledService_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (ScheduledService.ScheduledServiceId == 0)
            {
                string message = "ScheduledService has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            ScheduledService.RetrieveByForeignKeysFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class ScheduledService_UpdateByForeignKeys_V2 : ScheduledService_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ScheduledService.ScheduledServiceId == 0)
            {
                string message = "ScheduledService has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ScheduledService.UpdateByForeignKeysInDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion
    #region Validate Equipment and Service Task Id
    public class ScheduledService_ValidateByEquipmentAndServiceTaskId : ScheduledService_TransactionBaseClass
    {
        public ScheduledService_ValidateByEquipmentAndServiceTaskId()
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

                ScheduledService.ValidateEquipmentAndServiceTaskId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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



    #endregion

    #region Fleet Only
    public class ScheduledService_RetrieveDashboardChart : ScheduledService_TransactionBaseClass
    {
        public List<b_ScheduledService> ScheduledServiceList { get; set; }

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
            List<b_ScheduledService> tmpArray = null;

            ScheduledService.ScheduledService_RetrieveDashboardChart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ScheduledServiceList = new List<b_ScheduledService>();
            foreach (b_ScheduledService tmpObj in tmpArray)
            {
                ScheduledServiceList.Add(tmpObj);
            }
        }
    }

    #endregion

    #region Scheduled Service Retrieve By Equipment Id
    public class ScheduledService_RetrieveByEquipmentIdV2 : ScheduledService_TransactionBaseClass
    {
        public List<b_ScheduledService> ScheduledServiceList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ScheduledService.EquipmentId == 0)
            {
                string message = "ScheduledService has an invalid EquipmentID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        { 
            List<b_ScheduledService> tmpList = null;
            ScheduledService.RetrieveScheduledServiceByEquipmentIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ScheduledServiceList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Schduled service for Service order
    public class ScheduledService_RetrieveScheduledServiceForServiceOrder : ScheduledService_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ScheduledService.ScheduledServiceId > 0)
            {
                string message = "ScheduledService has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_ScheduledService tmpList = null;
            ScheduledService.RetrieveScheduledServiceForServiceOrder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Validate for activate inactivate
    public class ScheduledService_ValidateInactivateOrActivate : ScheduledService_TransactionBaseClass
    {
        public ScheduledService_ValidateInactivateOrActivate()
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

                ScheduledService.ValidateInactivateOrActivate(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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



    #endregion
}
