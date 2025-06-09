using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using System.Data.SqlClient;
using Database.StoredProcedure;
using Database.Business;

namespace Database.Client.Custom.Transactions
{
    public class POReceiptHeader_RetrieveByPurchaseOrderLineItem : POReceiptHeader_TransactionBaseClass
    {
        public List<b_POReceiptHeader> POReceiptHeaderList { get; set; }

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
            List<b_POReceiptHeader> tmpArray = null;

            POReceiptHeader.RetrieveByPurchaseOrderLineItemFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            POReceiptHeaderList = new List<b_POReceiptHeader>();
            foreach (b_POReceiptHeader tmpObj in tmpArray)
            {
                POReceiptHeaderList.Add(tmpObj);
            }
        }
    }

    public class POReceiptHeader_RetrieveByPurchaseOrderId : POReceiptHeader_TransactionBaseClass
    {
        public List<b_POReceiptHeader> POReceiptHeaderList { get; set; }

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
            List<b_POReceiptHeader> tmpArray = null;

            POReceiptHeader.RetrieveByPurchaseOrderIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            POReceiptHeaderList = new List<b_POReceiptHeader>();
            foreach (b_POReceiptHeader tmpObj in tmpArray)
            {
                POReceiptHeaderList.Add(tmpObj);
            }
        }
    }

    //------------Call From API SOM 938---------------------------------------------------------------------
    public class POReceiptHeader_ValidatePOAndUOMTransaction : POReceiptHeader_TransactionBaseClass
    {

        public POReceiptHeader_ValidatePOAndUOMTransaction()
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
                POReceiptHeader.ValidatePOAndUOMFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    public class POReceiptHeader_CreateByPK : POReceiptHeader_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (POReceiptHeader.POReceiptHeaderId > 0)
            {
                string message = "POReceiptHeader has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            POReceiptHeader.InsertIntoDatabaseByPk(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(POReceiptHeader.POReceiptHeaderId > 0);
        }
    }
   
}
