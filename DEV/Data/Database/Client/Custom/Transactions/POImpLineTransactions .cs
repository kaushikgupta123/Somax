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
    public class POImpLine_CustomBase : POImpLine_TransactionBaseClass
    {
        public Int64 POClientId { get; set; }
    }
    public class POImpLine_ValidateImport : POImpLine_CustomBase
    {
        
        public override void PerformLocalValidation()
        {
            this.POClientId = this.POImpLine.ClientId;
            base.PerformLocalValidation();
            this.POImpLine.ClientId = POClientId;

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            POImpLine.POImpLineValidateImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
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


    public class POImpLine_ProcessImport : POImpLine_CustomBase
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (POImpLine.POImpLineId == 0)
            {
                string message = "POImpLine has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            POImpLine.POImpLineProcessImport(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(POImpLine.POImpLineId > 0);
        }
    }




    //public class POImpLine_RetrievebyPOIdFromDatabase : POImpLine_CustomBase
    //{

    //    public override void PerformLocalValidation()
    //    {
    //        base.UseTransaction = false;    // moved from PerformWorkItem
    //        base.PerformLocalValidation();
    //        if (POImpLine.EXPOID == 0)
    //        {
    //            string message = "EXPOID has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
    //        POImpLine.RetrievePOImpLinebyPOIDFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }
    //}


    public class POImpLine_RetrievebyEXPOEXPOLine : POImpLine_CustomBase
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            this.POClientId = this.POImpLine.ClientId;
            base.PerformLocalValidation();
            this.POImpLine.ClientId = POClientId;
            if (POImpLine.EXPOLineID == 0)
            {
                string message = "EXPOLineID has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            POImpLine.POImpLine_RetrievebyEXPOEXPOLineFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }



    public class POImpLine_RetrievebySomaxPRLineIdFromDatabase : POImpLine_CustomBase
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            this.POClientId = this.POImpLine.ClientId;
            base.PerformLocalValidation();
            this.POImpLine.ClientId = POClientId;
            if (POImpLine.SOMAXPRLineId == 0)
            {
                string message = "SOMAXPRLineId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            POImpLine.RetrievePOImpLinebySomaxPRLineIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }



    //public class POImpLine_RetrieveCustom : POImpLine_CustomBase
    //{

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //        if (POImpLine.POImpLineId == 0)
    //        {
    //            string message = "POImpLine has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        base.UseTransaction = false;
    //        POImpLine.RetrieveByPKFromDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }
    //}

    public class POImpLine_CreateCustom : POImpLine_CustomBase
    {
        public override void PerformLocalValidation()
        {
            this.POClientId = this.POImpLine.ClientId;
            base.PerformLocalValidation();
            this.POImpLine.ClientId = POClientId;
            if (POImpLine.POImpLineId > 0)
            {
                string message = "POImpLine has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            POImpLine.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(POImpLine.POImpLineId > 0);
        }
    }


    public class POImpLine_UpdateCustom : POImpLine_CustomBase
    {
        public override void PerformLocalValidation()
        {
            this.POClientId = this.POImpLine.ClientId;
            base.PerformLocalValidation();
            this.POImpLine.ClientId = POClientId;
            if (POImpLine.POImpLineId == 0)
            {
                string message = "POImpLine has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            POImpLine.UpdateInDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
}


