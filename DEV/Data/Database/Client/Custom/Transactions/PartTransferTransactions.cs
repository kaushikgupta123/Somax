/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =========================================================
* 2014-Sep-30 SOM-346  Roger Lawton        Added bActive property to WorkOrder_RetrieveByEquipmentId
****************************************************************************************************
 */

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
    public class PartTransfer_RetrieveBySearchCriteria : AbstractTransactionManager
    {
        public PartTransfer_RetrieveBySearchCriteria()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }
               // Result Sets
        public List<b_PartTransfer> partTransferList { get; set; }
        public int ResultCount { get; set; }
        public string Search { get; set; }

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

            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                int tmp=0;

              
                ResultCount = tmp;
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
    }

    public class PartTransfer_RetrieveAllForSearch : PartTransfer_TransactionBaseClass
    {       
        public List<b_PartTransfer> PartTransferList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }
        public override void PerformWorkItem()
        {
            List<b_PartTransfer> tmpList = null;
            PartTransfer.RetrieveAllForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            PartTransferList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class PartTransfer_RetrieveByPKForeignKey : PartTransfer_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartTransfer.PartTransferId == 0)
            {
                string message = "PartTransfer has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PartTransfer.RetrieveByPKForeignKey(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class PartTransfer_UpdateIssue : PartTransfer_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartTransfer.PartTransferId == 0)
            {
                string message = "PartTransferId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PartTransfer.UpdatePartTransferIssueIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PartTransfer.PartTransferId > 0);
        }
    }

    public class PartTransfer_UpdateReceive : PartTransfer_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartTransfer.PartTransferId == 0)
            {
                string message = "PartTransferId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PartTransfer.UpdatePartTransferReceiveIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PartTransfer.PartTransferId > 0);
        }
    }
    public class PartTransfer_UpdateSend : PartTransfer_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartTransfer.PartTransferId == 0)
            {
                string message = "PartTransferId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PartTransfer.UpdatePartTransferSendIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PartTransfer.PartTransferId > 0);
        }
    }

    public class PartTransfer_UpdateCancelDeny : PartTransfer_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartTransfer.PartTransferId == 0)
            {
                string message = "PartTransferId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PartTransfer.UpdatePartTransferCancelDenyIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PartTransfer.PartTransferId > 0);
        }
    }
    public class PartTransfer_UpdateForceComplete : PartTransfer_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartTransfer.PartTransferId == 0)
            {
                string message = "PartTransferId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            PartTransfer.UpdatePartTransferForceCompleteIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PartTransfer.PartTransferId > 0);
        }
    }

    public class PartTransfer_Shipment : PartTransfer_TransactionBaseClass
    {
        public List<b_PartTransfer> PartTransferList { get; set; }       
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_PartTransfer> tmpList = null;
            PartTransfer.RetrieveShipment(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            PartTransferList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}

