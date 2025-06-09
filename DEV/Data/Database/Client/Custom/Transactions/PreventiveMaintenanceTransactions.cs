/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012-2020 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class PreventiveMaintenance_RetrieveBySearchCriteria : AbstractTransactionManager
    {
        public PreventiveMaintenance_RetrieveBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public string Query { get; set; }
        public string Area { get; set; }
        public string Site { get; set; }
        public string Department { get; set; }

        public string ChargeType { get; set; }
        public string ScheduleType { get; set; }
        public string Crew { get; set; }
        public string Priority { get; set; }
        public string Shift { get; set; }

        public string ColumnName { get; set; }
        public string ColumnSearchText { get; set; }
        public bool MatchAnywhere { get; set; }

        public string DateSelection { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<KeyValuePair<b_PrevMaintMaster, List<b_PrevMaintSched>>> PrevMaintList { get; set; }
        public int ResultCount { get; set; }

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

                int tmp;

                Dictionary<long, List<b_PrevMaintSched>> dictSched = new Dictionary<long, List<b_PrevMaintSched>>();
                PrevMaintList = new List<KeyValuePair<b_PrevMaintMaster, List<b_PrevMaintSched>>>();

                // The Database query returns the data with multiple copies of the b_Part object if there are more than one b_PartStoreroom associated.
                // The data structure that gets returned will eliminate redundant copies.

                List<KeyValuePair<b_PrevMaintMaster, b_PrevMaintSched>> temp = usp_PreventiveMaintenance_RetrieveBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Query, Site, Area, Department,
                    ChargeType, ScheduleType, Crew, Priority, Shift, DateSelection, DateStart, DateEnd, ColumnName, ColumnSearchText, PageNumber, ResultsPerPage, MatchAnywhere, OrderColumn, OrderDirection, out tmp);

                foreach (KeyValuePair<b_PrevMaintMaster, b_PrevMaintSched> kvp in temp)
                {
                    if (dictSched.ContainsKey(kvp.Key.PrevMaintMasterId))
                    {
                        // Add b_PartStoreroom to list
                        dictSched[kvp.Key.PrevMaintMasterId].Add(kvp.Value);
                    }
                    else
                    {
                        dictSched.Add(kvp.Key.PrevMaintMasterId, new List<b_PrevMaintSched>());

                        KeyValuePair<b_PrevMaintMaster, List<b_PrevMaintSched>> newKVP = new KeyValuePair<b_PrevMaintMaster, List<b_PrevMaintSched>>(kvp.Key, dictSched[kvp.Key.PrevMaintMasterId]);
                        newKVP.Value.Add(kvp.Value);
                        PrevMaintList.Add(newKVP);
                    }
                }

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


    public class PreventiveMaintenance_RetrieveInitialSearchConfigurationData : AbstractTransactionManager
    {
        public PreventiveMaintenance_RetrieveInitialSearchConfigurationData()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria;

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

                // Call the stored procedure to retrieve the data
                SearchCriteria = usp_PreventiveMaintenance_RetrieveInitialSearchConfigurationData.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId);
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

    public class PreventiveMaintenance_RetrieveToSearchCriteria : AbstractTransactionManager
    {
        #region Properties
        public int FilterType { get; set; }
        public int FilterValue { get; set; }


        public List<b_PrevMaintMaster> SearchResult;
        #endregion

        #region Constructor
        public PreventiveMaintenance_RetrieveToSearchCriteria()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }
        #endregion

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }



        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;
            int resultcount;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                // Call the stored procedure to retrieve the data
                SearchResult = usp_PreventiveMaintenance_RetrieveToSearchCriteria.CallStoredProcedure
                    (command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId
                    , FilterType, FilterValue, out resultcount);
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


        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }


    }



    public class PreventiveMaintenance_ValidateAdd : PrevMaintMaster_TransactionBaseClass
    {
        public PreventiveMaintenance_ValidateAdd()
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

                PrevMaintMaster.ValidateProcessAddFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
                        ref errors);

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

    public class PreventiveMaintenance_ValidateLink : PrevMaintMaster_TransactionBaseClass
    {
        public PreventiveMaintenance_ValidateLink()
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

                PrevMaintMaster.ValidateProcessLinkFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
                        ref errors);

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

    public class PreventiveMaintenance_DeleteMasterDetails : PrevMaintMaster_TransactionBaseClass
    {
        public PreventiveMaintenance_DeleteMasterDetails()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets

        public override void PerformWorkItem()
        {
                PrevMaintMaster.DeletePreventiveMaintenanceMasterChild
                    (this.Connection,
                    this.Transaction,
                    this.CallerUserInfoId,
                    this.CallerUserName);
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
  

    public class PreventiveMaintenance_RetrieveByForeignKeys : PrevMaintMaster_TransactionBaseClass
    {
        public PreventiveMaintenance_RetrieveByForeignKeys()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets

        public override void PerformWorkItem()
        {
            PrevMaintMaster.PreventiveMaintenance_RetrieveByForeignKey(this.Connection,this.Transaction,this.CallerUserInfoId, this.CallerUserName);
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

    public class PreventiveMaintenance_CreateFromPMLibrary : PrevMaintMaster_TransactionBaseClass
    {
        public PreventiveMaintenance_CreateFromPMLibrary()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets

        public override void PerformWorkItem()
        {
            PrevMaintMaster.PreventiveMaintenance_CreateFromPMLibrary
                (this.Connection,
                this.Transaction,
                this.CallerUserInfoId,
                this.CallerUserName);
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

    public class PreventiveMaintenance_ValidateByClientlookupIdForChange : PrevMaintMaster_TransactionBaseClass
    {
        public PreventiveMaintenance_ValidateByClientlookupIdForChange()
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
                PrevMaintMaster.ValidateByClientLookupIdForChange(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class PreventiveMaintenance_ChunkSearch : PrevMaintMaster_TransactionBaseClass
    {
        public List<b_PrevMaintMaster> PrevMaintMasterList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();            
        }
        public override void PerformWorkItem()
        {
            List<b_PrevMaintMaster> tmpArray = null;
            PrevMaintMaster.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,ref tmpArray);
            PrevMaintMasterList = new List<b_PrevMaintMaster>();
            foreach (b_PrevMaintMaster tmpObj in tmpArray)
            {
                PrevMaintMasterList.Add(tmpObj);
            }
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(PrevMaintMaster.PrevMaintMasterId == 0);
        }
    }


    public class PrevMaint_UpdateForClientLookupId_V2 : PrevMaintMaster_TransactionBaseClass
    {
    

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintMaster.PrevMaintMasterId == 0)
            {
                string message = "PrevMaintMasterId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PrevMaintMaster.UpdateForClientLookupId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    // SOM-1737
    public class CreatePrevMaintMaster_CopyFromPMLibrary : PrevMaintMaster_TransactionBaseClass
    {
      public CreatePrevMaintMaster_CopyFromPMLibrary()
      {
      }

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();

      }

      // Result Sets

      public override void PerformWorkItem()
      {
        PrevMaintMaster.CreatePrevMaintMaster_CopyFromPMLibrary
            (this.Connection,
            this.Transaction,
            this.CallerUserInfoId,
            this.CallerUserName);
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


}
