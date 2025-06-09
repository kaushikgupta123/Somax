using Common.Enumerations;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    #region Validate
    public class ServiceTasks_ValidateByClientLookupId : ServiceTasks_TransactionBaseClass
        {
            public ServiceTasks_ValidateByClientLookupId()
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

                    ServiceTasks.ValidateClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class ServiceTasks_ValidateByInactivateorActivate : ServiceTasks_TransactionBaseClass
    {
        public ServiceTasks_ValidateByInactivateorActivate()
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

                ServiceTasks.ValidateByInactivateorActivate(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    #region Search
    public class ServiceTask_RetrieveFleetAssetChunkSearchV2 : ServiceTasks_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ServiceTasks.ServiceTasksId > 0)
            {
                string message = "Service Task has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_ServiceTasks tmpList = null;
            ServiceTasks.RetrieveServiceTaskChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region GetAll ServiceTask
    public class ServiceTask_RetrieveAllCustom : AbstractTransactionManager
    {

        public ServiceTask_RetrieveAllCustom()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public b_ServiceTasks ServiceTasks { get; set; }

    


        public List<b_ServiceTasks> ServiceTasksList { get; set; }

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
            b_ServiceTasks[] tmpArray = null;
            b_ServiceTasks o = new b_ServiceTasks();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;

            
            o.RetrieveAllActiveFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            //o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ServiceTasksList = new List<b_ServiceTasks>(tmpArray);
        }
    }
    #endregion

}
