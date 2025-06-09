using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class IoTDevice_RetrieveV2 : IoTDevice_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (IoTDevice.IoTDeviceId > 0)
            {
                string message = "IoTDevice has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_IoTDevice tmpList = null;
            IoTDevice.RetrieveV2Search(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class IoTDevice_RetrieveByForeignKeys : IoTDevice_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (IoTDevice.IoTDeviceId == 0)
            {
                string message = "IoTDevice has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            IoTDevice.RetrieveByForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    public class IoTDevice_RetrieveWorkOrderForPrint : IoTDevice_TransactionBaseClass
    {

        public List<b_IoTDevice> IoTDeviceList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_IoTDevice> tmpList = null;
            IoTDevice.RetrieveIotDeviceForPrint(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            IoTDeviceList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    #region V2-537
    public class IoTDevice_RetrieveChunkSearchByEquipmentIdV2 : IoTDevice_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_IoTDevice tmpList = null;
            IoTDevice.RetrieveChunkSearchByEqipmentId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
    #region V2-536
    public class IoTDevice_ValidateByClientLookupId_V2 : IoTDevice_TransactionBaseClass
    {
        public IoTDevice_ValidateByClientLookupId_V2()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public bool CreateMode { get; set; }
        public System.Data.DataTable lulist { get; set; }


        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {

                List<b_StoredProcValidationError> errors = null;


                IoTDevice.ValidateByClientLookupIdFromDatabase_V2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    public class IoTDevice_ValidateAdd_V2 : IoTDevice_TransactionBaseClass
    {
        public IoTDevice_ValidateAdd_V2()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public bool CreateMode { get; set; }
        public System.Data.DataTable lulist { get; set; }


        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {

                List<b_StoredProcValidationError> errors = null;


                IoTDevice.ValidateAdd_V2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
