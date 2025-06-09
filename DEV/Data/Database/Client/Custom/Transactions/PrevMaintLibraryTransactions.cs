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
using  Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{  

  public class PrevMaintLibrary_ValidateClientLookupIdTransaction : PrevMaintLibrary_TransactionBaseClass
  {
    public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (PrevMaintLibrary.PrevMaintLibraryId > 0)
      {
        string message = "PrevMaintLibrary has an invalid ID.";
        //throw new Exception(message);
      }
    }
    public override void PerformWorkItem()
    {
      List<b_StoredProcValidationError> errors = null;
      PrevMaintLibrary.ValidateByClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
      StoredProcValidationErrorList = errors;
    }
    public override void Postprocess()
    {

    }
  }
    public class PrevMaintLibrary_RetrieveAllCustom : PrevMaintLibrary_TransactionBaseClass
    {

        public PrevMaintLibrary_RetrieveAllCustom()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_PrevMaintLibrary> PrevMaintLibraryList { get; set; }

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
            b_PrevMaintLibrary[] tmpArray = null;
            b_PrevMaintLibrary o = new b_PrevMaintLibrary();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;


            o.RetrieveAllCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PrevMaintLibraryList = new List<b_PrevMaintLibrary>(tmpArray);
        }
    }

    //*** V2-694
    public class PrevMaintLibrary_RetrieveByInactiveFlag : PrevMaintLibrary_TransactionBaseClass
    {
        public int InactiveFlg { get; set; }
        public PrevMaintLibrary_RetrieveByInactiveFlag()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_PrevMaintLibrary> PrevMaintLibraryList { get; set; }

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
            b_PrevMaintLibrary[] tmpArray = null;
            b_PrevMaintLibrary o = new b_PrevMaintLibrary();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;           
            o.RetrieveByInactiveFlag(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, InactiveFlg, ref tmpArray);
            PrevMaintLibraryList = new List<b_PrevMaintLibrary>(tmpArray);
        }
    }
    //--end
    public class PrevMaintLibrary_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public PrevMaintLibrary_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_PrevMaintLibrary> PrevMaintLibraryList { get; set; }
        public int ResultCount { get; set; }
        
        public override void PerformWorkItem()
        {
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

                int tmp = 0;

                PrevMaintLibraryList = usp_PrevMaintLibrary_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Description,
                        PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out tmp);

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

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

    public class PMLibrary_ValidateByClientlookupIdForChange : PrevMaintLibrary_TransactionBaseClass
    {

        
        public PMLibrary_ValidateByClientlookupIdForChange()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;
                PrevMaintLibrary.ValidateLibByClientLookupIdForChange(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class PMLibrary_LibUpdateForClientLookupId_V2 : PrevMaintLibrary_TransactionBaseClass
    {


        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintLibrary.PrevMaintLibraryId == 0)
            {
                string message = "PrevMaintLibraryId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PrevMaintLibrary.UpdateClientLookupIdForLibrary_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

}
