/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2018 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================

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

    public class MasterSanLibrary_ValidateClientLookupIdTransaction : MasterSanLibrary_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (MasterSanLibrary.MasterSanLibraryId > 0)
            {
                string message = "MasterSanLibrary has an invalid ID.";
                //throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            MasterSanLibrary.ValidateByClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }
        public override void Postprocess()
        {

        }
    }
    public class MasterSanLibrary_RetrieveAllCustom : MasterSanLibrary_TransactionBaseClass
    {

        public MasterSanLibrary_RetrieveAllCustom()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_MasterSanLibrary> MasterSanLibraryList { get; set; }

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
            b_MasterSanLibrary[] tmpArray = null;
            b_MasterSanLibrary o = new b_MasterSanLibrary();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;


            o.RetrieveAllCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            MasterSanLibraryList = new List<b_MasterSanLibrary>(tmpArray);
        }
    }


}
