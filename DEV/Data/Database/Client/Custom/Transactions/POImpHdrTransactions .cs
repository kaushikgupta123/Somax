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

    public class POImpHdr_CustomBase : POImpHdr_TransactionBaseClass
    {
        public Int64 POClientId { get; set; }
    }
    public class POImpHdr_ValidateImport : POImpHdr_CustomBase
    {

        public override void PerformLocalValidation()
        {
            this.POClientId = this.POImpHdr.ClientId;
            base.PerformLocalValidation();
            this.POImpHdr.ClientId = POClientId;
        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            POImpHdr.POImpHdr_ValidateImport(this.Connection, this.Transaction, this.CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }
        public override void Preprocess()
        {
            /*throw new NotImplementedException()*/
            ;
        }
    }


    public class POImpHdr_ProcessImport : POImpHdr_CustomBase
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (POImpHdr.POImpHdrId == 0)
            {
                string message = "POImpHdr has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            POImpHdr.POImpHdr_ProcessImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(POImpHdr.POImpHdrId > 0);
        }
    }


    //public class POImpHdr_RetrievebyPOIdFromDatabase : POImpHdr_CustomBase
    //{

    //    public override void PerformLocalValidation()
    //    {
    //        base.UseTransaction = false;    // moved from PerformWorkItem
    //        base.PerformLocalValidation();
    //        if (POImpHdr.EXPOID == 0)
    //        {
    //            string message = "ExPOId has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
    //        POImpHdr.RetrievePOImpHdrbyPOIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }
    //}

    public class POImpHdr_RetrievebyPRIdFromDatabase : POImpHdr_CustomBase
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (POImpHdr.EXPRID == 0)
            {
                string message = "EXPRID has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            POImpHdr.RetrievePOImpHdrbyPRIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    //public class POImpHdr_RetrievebySomaxPRIdFromDatabase : POImpHdr_CustomBase
    //{

    //    public override void PerformLocalValidation()
    //    {
    //        base.UseTransaction = false;    // moved from PerformWorkItem
    //        base.PerformLocalValidation();
    //        if (POImpHdr.SOMAXPRID == 0)
    //        {
    //            string message = "SOMAXPRID has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
    //        POImpHdr.RetrievePOImpHdrbySomaxPRIDFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }
    //}

    public class POImpHdr_RetrievebyExPOIdFromDatabase : POImpHdr_CustomBase
    {
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            this.POClientId = this.POImpHdr.ClientId;
            base.PerformLocalValidation();
            this.POImpHdr.ClientId = POClientId;

            if (POImpHdr.EXPOID == 0)
            {
                string message = "EXPOID has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            POImpHdr.RetrievePOImpHdrbyEXPOIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    //public class POImpHdr_RetrieveByPKCustom : POImpHdr_CustomBase
    //{

    //    public override void PerformLocalValidation()
    //    {
    //        base.UseTransaction = false;    // moved from PerformWorkItem
    //        base.PerformLocalValidation();
    //        if (POImpHdr.POImpHdrId == 0)
    //        {
    //            string message = "POImpHdrId has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
    //        POImpHdr.RetrieveByPKCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }
    //}

    public class POImpHdr_CreateCustom : POImpHdr_CustomBase
    {
        public override void PerformLocalValidation()
        {
            this.POClientId = this.POImpHdr.ClientId;
            base.PerformLocalValidation();
            this.POImpHdr.ClientId = POClientId;
            if (POImpHdr.POImpHdrId > 0)
            {
                string message = "POImpHdr has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            POImpHdr.CustomInsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(POImpHdr.POImpHdrId > 0);
        }
    }


    public class POImpHdr_UpdateCustom : POImpHdr_CustomBase
    {
        public override void PerformLocalValidation()
        {
            this.POClientId = this.POImpHdr.ClientId;
            base.PerformLocalValidation();
            this.POImpHdr.ClientId = POClientId;
            if (POImpHdr.POImpHdrId == 0)
            {
                string message = "POImpHdr has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            POImpHdr.UpdateInDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }



}


