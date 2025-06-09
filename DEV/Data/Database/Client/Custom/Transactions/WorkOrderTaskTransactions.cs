using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;
using Database.StoredProcedure;

namespace Database
{
    public class WorkOrderTask_RetrieveByEquipmentId : WorkOrderTask_TransactionBaseClass
    {
        public List<b_WorkOrderTask> WorkOrderTaskList { get; set; }

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
            List<b_WorkOrderTask> tmpArray = null;

            WorkOrderTask.RetrieveByEquipmentIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderTaskList = new List<b_WorkOrderTask>();
            foreach (b_WorkOrderTask tmpObj in tmpArray)
            {
                WorkOrderTaskList.Add(tmpObj);
            }
        }
    }

    public class WorkOrderTask_RetrieveByWorkOrderId : WorkOrderTask_TransactionBaseClass
    {
        public List<b_WorkOrderTask> WorkOrderTaskList { get; set; }

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
            List<b_WorkOrderTask> tmpArray = null;

            WorkOrderTask.RetrieveByWorkOrderIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderTaskList = new List<b_WorkOrderTask>();
            foreach (b_WorkOrderTask tmpObj in tmpArray)
            {
                WorkOrderTaskList.Add(tmpObj);
            }
        }
    }
    public class WorkOrderTask_RetrieveByWorkOrderIdV2 : WorkOrderTask_TransactionBaseClass
    {
        public List<b_WorkOrderTask> WorkOrderTaskList { get; set; }

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
            List<b_WorkOrderTask> tmpArray = null;

            WorkOrderTask.RetrieveWorkOrderTaskByWorkOrderIdV2FromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderTaskList = new List<b_WorkOrderTask>();
            foreach (b_WorkOrderTask tmpObj in tmpArray)
            {
                WorkOrderTaskList.Add(tmpObj);
            }
        }
    }
    public class WorkOrderTask_RetrieveByPKForeignKeys : WorkOrderTask_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderTask.WorkOrderTaskId == 0)
            {
                string message = "WorkOrderTask has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            WorkOrderTask.RetrieveByPKForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class WorkOrderTask_ValidateByClientLookupId : WorkOrderTask_TransactionBaseClass
    {
        public WorkOrderTask_ValidateByClientLookupId()
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

                WorkOrderTask.ValidateByClientLookupIdFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class WorkOrderTask_CreateByForeignKeys : WorkOrderTask_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderTask.WorkOrderTaskId > 0)
            {
                string message = "WorkOrderTask has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderTask.InsertByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrderTask.WorkOrderTaskId > 0);
        }
    }

    public class WorkOrderTask_UpdateByForeignKeys : WorkOrderTask_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderTask.WorkOrderTaskId == 0)
            {
                string message = "WorkOrderTask has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderTask.UpdateByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrderTask.WorkOrderTaskId > 0);
        }
    }

    public class WorkOrderTask_UpdateByForeignKeysFroTask : WorkOrderTask_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderTask.WorkOrderTaskId == 0)
            {
                string message = "WorkOrderTask has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderTask.UpdateByForeignKeysIntoDatabaseForTask(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrderTask.WorkOrderTaskId > 0);
        }
    }

    public class WorkOrderTask_RetrieveChargeToClientLookupIdBySearchCriteria : WorkOrderTask_TransactionBaseClass
    {
        public List<b_WorkOrderTask> WorkOrderTaskList { get; set; }

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
            base.UseTransaction = false;
            List<b_WorkOrderTask> tmpList = null;

            WorkOrderTask.RetrieveChargeToClientLookupIdBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            WorkOrderTaskList = new List<b_WorkOrderTask>();
            foreach (b_WorkOrderTask tmpObj in tmpList)
            {
                WorkOrderTaskList.Add(tmpObj);
            }
        }
    }

    public class WorkOrderTask_ReOrderTask : WorkOrderTask_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderTask.WorkOrderTaskId == 0)
            {
                string message = "WorkOrderTask has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderTask.ReOrderTask(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrderTask.WorkOrderTaskId > 0);
        }
    }
}
