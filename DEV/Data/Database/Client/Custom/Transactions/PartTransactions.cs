/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
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
using Common.Structures;

namespace Database
{
    /// <summary>
    /// Retrieve Part and Storeroom Data for Search Results
    /// </summary>
    //public class PartsStorerooms_RetrieveBySearchCriteria : AbstractTransactionManager
    //{
    //    public PartsStorerooms_RetrieveBySearchCriteria()
    //    {
    //        UseDatabase = DatabaseTypeEnum.Client;
    //    }

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //    }

    //    public string Query { get; set; }
    //    public string Area { get; set; }
    //    public string Site { get; set; }
    //    public string Department { get; set; }
    //    public string StockType { get; set; }
    //    public string AccountId { get; set; }
    //    public string Storeroom { get; set; }   // Not Used at present

    //    public string ColumnName { get; set; }
    //    public string ColumnSearchText { get; set; }
    //    public bool MatchAnywhere { get; set; }

    //    public string DateSelection { get; set; }
    //    public DateTime DateStart { get; set; }
    //    public DateTime DateEnd { get; set; }

    //    public int PageNumber { get; set; }
    //    public int ResultsPerPage { get; set; }

    //    public string OrderColumn { get; set; }
    //    public string OrderDirection { get; set; }

    //    // Result Sets
    //    public List<b_Part> PartsStoreroomsList { get; set; }
    //    public int ResultCount { get; set; }

    //    public override void PerformWorkItem()
    //    {
    //        SqlCommand command = null;
    //        string message = String.Empty;

    //        try
    //        {
    //            // Create the command to use in calling the stored procedures
    //            command = new SqlCommand()
    //            {
    //                Connection = this.Connection,
    //                Transaction = this.Transaction
    //            };

    //            // Call the stored procedure to retrieve the data
    //            Database.SqlClient.ProcessRow<b_Part> processRow =
    //                new Database.SqlClient.ProcessRow<b_Part>(reader => { b_Part obj = new b_Part(); obj.LoadFromDatabaseExtended(reader); return obj; });

    //            int tmp;

    //            PartsStoreroomsList = usp_PartsStorerooms_RetrieveBySearchCriteria.CallStoredProcedure(command, processRow, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Query, Site, Area, Department,
    //                StockType, AccountId, DateSelection, DateStart, DateEnd, ColumnName, ColumnSearchText, PageNumber, ResultsPerPage, MatchAnywhere, OrderColumn, OrderDirection, out tmp);

    //            ResultCount = tmp;
    //        }
    //        finally
    //        {
    //            if (null != command)
    //            {
    //                command.Dispose();
    //                command = null;
    //            }

    //            message = String.Empty;
    //        }
    //    }

    //    public override void Preprocess()
    //    {
    //        // throw new NotImplementedException();
    //    }

    //    public override void Postprocess()
    //    {
    //        // throw new NotImplementedException();
    //    }
    //}

    /// <summary>
    /// Retrieve Part and Storeroom Data for Edit Page
    /// </summary>
    public class PartsStorerooms_RetrieveByPartId : Part_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId == 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
            base.UseTransaction = false;
        }

        public override void PerformWorkItem()
        {
            // This is too late to set this - the transaction has already been created
            //base.UseTransaction = false;
            Part.RetrieveByPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class PartsStorerooms_UpdateByPartId : Part_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId == 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = true;
            Part.UpdateByPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class Part_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public Part_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }

        public long SiteId { get; set; }
        /*Changeby Int 3/7/23*/
        public string PartId { get; set; }
        public string UPCCode { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }

        public string StockType { get; set; }
        // public string Location1_1 { get; set; }
        /*END*/
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Part> PartList { get; set; }
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

                int tmp;

                PartList = usp_Part_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Description, SiteId, PartId, Manufacturer, ManufacturerId, StockType, UPCCode, PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out tmp);

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


    public class Part_ProcessPart : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            Part.ProcessPart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class Part_RetrieveBySiteId : AbstractTransactionManager
    {
        public Part_RetrieveBySiteId()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<PartMultipleDataStructure> RawList { get; set; }
        public long SiteId { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                RawList = usp_Part_RetrieveBySiteId.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.User.UserName, dbKey.Client.ClientId, SiteId);
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

            //Part.RetrieveBySiteIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            //PartList = new List<b_Part>();
            //foreach (b_Part tmpObj in tmpArray)
            //{
            //    PartList.Add(tmpObj);
            //}
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

    public class Part_RetrieveByPartId : Part_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Part.RetrieveByPartIdForPI(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Parts_RetrieveClientLookupIdBySearchCriteria : Part_TransactionBaseClass
    {

        public List<b_Part> PartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId > 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.RetrieveClientLookupIdBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Part_ValidateClientLookupIdTransaction : Part_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Part.ValidateClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }
    public class Part_ValidateClientLookupIdForPI : Part_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Part.ValidateClientLookupIdForPI(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class Part_ValidateByInactivateorActivate : Part_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Part.ValidateByInactivateorActivate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    //public class Part_RetrieveInitialSearchConfigurationData : AbstractTransactionManager
    //{
    //    public Part_RetrieveInitialSearchConfigurationData()
    //    {
    //        UseDatabase = DatabaseTypeEnum.Client;
    //    }

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //    }

    //    public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria;

    //    public override void PerformWorkItem()
    //    {
    //        SqlCommand command = null;
    //        string message = String.Empty;

    //        try
    //        {
    //            // Create the command to use in calling the stored procedures
    //            command = new SqlCommand()
    //            {
    //                Connection = this.Connection,
    //                Transaction = this.Transaction
    //            };

    //            // Call the stored procedure to retrieve the data
    //            SearchCriteria = usp_PartStoreroom_RetrieveInitialSearchConfigurationData.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId);
    //        }
    //        finally
    //        {
    //            if (null != command)
    //            {
    //                command.Dispose();
    //                command = null;
    //            }

    //            message = String.Empty;
    //        }
    //    }

    //    public override void Preprocess()
    //    {
    //        // throw new NotImplementedException();
    //    }

    //    public override void Postprocess()
    //    {
    //        // throw new NotImplementedException();
    //    }
    //}

    /****************************Added By Indusnet Technologies******************************/

    public class Parts_RetrieveForSearchBySiteId : Part_TransactionBaseClass
    {

        public List<b_Part> PartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId > 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.RetrieveForSearchBySiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Part_RetrievePartSiteReview : Part_TransactionBaseClass
    {

        public List<b_Part> PartList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId > 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;

            Part.RetrievePartSiteReview(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Parts_RetrieveForSearchForMultipltSite : Part_TransactionBaseClass
    {

        public List<b_Part> PartList { get; set; }
        //public String SearchText { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId > 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;

            Part.RetrieveForSearchForMultipleSite(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Part_RetrieveByClientLookUpIdNUPCCode : Part_TransactionBaseClass
    {
        //public b_Part objRetPart { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId > 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            b_Part temp = null;
            base.UseTransaction = false;
            Part.RetrieveByPartClientLookUpIdNUPCCode(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref temp);

            this.Part = temp;
        }
    }

    public class RetrievePartListByFilterText : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.RetrievePartByFilterText(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    public class SearchForCartTrans : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.SearchForCart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class SearchForCartWOTrans : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.SearchForCartWO(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class SearchForCartTrans_VendorCatalog : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.SearchForCart_VendorCatalog(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class SearchForCartTrans_VendorCatalogWO : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.SearchForCart_VendorCatalogWO(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class Part_RetrievesAll : AbstractTransactionManager
    {

        public Part_RetrievesAll()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Part> PartList { get; set; }
        public long ClientId { get; set; }

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
            b_Part[] tmpArray = null;
            b_Part o = new b_Part();
            o.ClientId = ClientId;

            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartList = new List<b_Part>(tmpArray);
        }
    }

    //-----SOM-786-----//
    public class Part_RetrieveCreateModifyDate : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (Part.PartId == 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            Part.RetrieveCreateModifyDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    /*************************************End************************************************/

    //-------------------Call From API-------------------------------------------------------
    public class Part_RetrieveBySiteIdAndClientLookUpId : Part_TransactionBaseClass
    {

        public List<b_Part> PartList { get; set; }
        //public String SearchText { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId > 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;

            Part.RetrieveBySiteIdAndClientLookUpId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Part_ValidateByStockTypeIssueUnit : Part_TransactionBaseClass
    {



        public Part_ValidateByStockTypeIssueUnit()
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
                Part.ValidateByIssueUnitStockTypeFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

        public class Part_ValidateAddTransaction : Part_TransactionBaseClass
        {
            public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
            public override void PerformLocalValidation()
            {
                base.PerformLocalValidation();

            }
            public override void PerformWorkItem()
            {
                List<b_StoredProcValidationError> errors = null;
                Part.ValidateAdd(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
                StoredProcValidationErrorList = errors;
            }

            public override void Postprocess()
            {

            }
        }
    }

    public class Part_RetrieveAll_V2 : AbstractTransactionManager
    {

        public Part_RetrieveAll_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Part> PartList { get; set; }

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
            b_Part[] tmpArray = null;
            b_Part o = new b_Part();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;


            o.RetrieveAll_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartList = new List<b_Part>(tmpArray);
        }
    }

    public class Part_UpdateBulk : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (string.IsNullOrEmpty(Part.PartIdList))
            {
                string message = "Part List is empty";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Part.UpdateInBulk(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }


    public class Part_RetrievePOandPR : Part_TransactionBaseClass
    {

        public Part_RetrievePOandPR()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Part> PartList { get; set; }

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
            b_Part[] tmpArray = null;



            // Explicitly set tenant id from dbkey
            //o.ClientId = this.dbKey.Client.ClientId;
            //o.SiteId = this.dbKey.User.DefaultSiteId;


            Part.RetrievePOandPRforPart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PartList = new List<b_Part>(tmpArray);
        }
    }

    public class Part_RetrieveforMentionAlert : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            Part.PartRetrieveForMentionAlert(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    public class Part_ChunkSearchV2 : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Part.PartId == 0)
            //{
            //    string message = "Part has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartRetrieveChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Part_SearchForPrintV2 : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId > 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartRetrieveSearchForPrintV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }


    public class Part_ChunkSearchLookupListV2 : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Part.PartId == 0)
            //{
            //    string message = "Part has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartChunkSearchLookupListV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Parts_RetrievePartIdClientLookupId : Part_TransactionBaseClass
    {

        public b_Part PartResult { get; set; }

        public override void PerformLocalValidation()
        {
            if (string.IsNullOrEmpty(Part.ClientLookupId))
            {
                string message = "Part has an Client Lookup ID.";
                throw new Exception(message);
            }
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.RetrievePartIdByClientLookupIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartResult = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Part_SearchForCalatogEntriesForPartChunkSearch : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.SearchForCalatogEntriesForPartChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    public class Part_CycleCountChunkSearch : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartRetrieveCycleCountChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    #region V2-668
    public class PartsStorerooms_RetrieveByPartId_V2 : Part_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId == 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
            base.UseTransaction = false;
        }

        public override void PerformWorkItem()
        {
            // This is too late to set this - the transaction has already been created
            //base.UseTransaction = false;
            Part.RetrieveByPartId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    #endregion

    #region V2-670
    public class Part_ChunkSearchForMultiPartStoreroomV2 : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Part.PartId == 0)
            //{
            //    string message = "Part has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.MultiPartStoreroomRetrieveChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class MultiStoreroomPart_RetrieveByPartId : Part_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId == 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
            base.UseTransaction = false;
        }

        public override void PerformWorkItem()
        {
            // This is too late to set this - the transaction has already been created
            //base.UseTransaction = false;
            Part.MultiStoreroomRetrieveByPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Part_ValidateClientLookupIdV2Transaction : Part_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Part.ValidateClientLookupIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }
    #endregion
    #region V2-687
    public class Parts_RetrievePartIdByStoreroomIdAndClientLookupId : Part_TransactionBaseClass
    {

        public b_Part PartResult { get; set; }

        public override void PerformLocalValidation()
        {
            if (string.IsNullOrEmpty(Part.ClientLookupId))
            {
                string message = "Part has an Client Lookup ID.";
                throw new Exception(message);
            }
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.RetrievePartIdByStoreroomIdAndClientLookupIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartResult = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Part_ChunkSearchLookupListForMultiStoreroom_V2 : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Part.PartId == 0)
            //{
            //    string message = "Part has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartChunkSearchLookupListForMultiStoreroom_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Part_CycleCountChunkSearchForMultiStoreroom : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartRetrieveCycleCountChunkSearchForMultiStoreroom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

   
    public class PartsStorerooms_RetrieveByPartIdForMultiStoreroom_V2 : Part_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId == 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
            base.UseTransaction = false;
        }

        public override void PerformWorkItem()
        {
            // This is too late to set this - the transaction has already been created
            //base.UseTransaction = false;
            Part.RetrieveByPartIdForMultiStoreroom_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    #endregion

    #region V2-732
    public class SearchForCartTrans_VendorCatalogWOMultiStoreroom : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.SearchForCart_VendorCatalogWOMultiStoreroom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class SearchForCartWOTransMultiStoreroom : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.SearchForCartWOMultiStoreroom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion

    #region V2-738
    public class SearchForCartTransMultiStoreroom : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.SearchForCartMultiStoreroom_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class SearchForCartTrans_VendorCatalogForMultiStoreroom : Part_TransactionBaseClass
    {

        public List<b_Part> RetPartList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Part> tmpList = null;
            Part.SearchForCart_VendorCatalogForMultiStoreroom_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetPartList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion

    #region V2-736

    public class Part_ChunkSearchLookupListForMultiStoreroomMobile_V2 : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartChunkSearchLookupListForMultiStoreroomMobile_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Part_ChunkSearchLookupListMobileV2 : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartChunkSearchLookupListMobileV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
    #region Change Part Clientlookup
    public class Part_ChangeClientLookupId_V2 : Part_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId == 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Part.ChangeClientLookupId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion

    #region V2-1045
    public class Parts_RetrievePartIdClientLookupIdForFindPart : Part_TransactionBaseClass
    {

        public b_Part PartResult { get; set; }

        public override void PerformLocalValidation()
        {
            if (string.IsNullOrEmpty(Part.ClientLookupId))
            {
                string message = "Part has an Client Lookup ID.";
                throw new Exception(message);
            }
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.RetrievePartIdByClientLookupIdForFindPartFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PartResult = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region  RKL-MAIL-Label Printing from Receipts
    public class PartsStorerooms_RetrieveByPartIdAndPartStoreroomId_V2 : Part_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Part.PartId == 0)
            {
                string message = "Part has an invalid ID.";
                throw new Exception(message);
            }
            base.UseTransaction = false;
        }

        public override void PerformWorkItem()
        {
            // This is too late to set this - the transaction has already been created
            //base.UseTransaction = false;
            Part.RetrieveByPartIdAndPartStoreroomId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    #endregion
    #region V2-1167
    public class Part_ChunkSearchLookupListForSingleStockLineItemV2 : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Part.PartId == 0)
            //{
            //    string message = "Part has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartChunkSearchLookupListForSingleStockLineItemV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Part_ChunkSearchLookupListForSingleStockLineItemMultiStoreroom_V2 : Part_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (Part.PartId == 0)
            //{
            //    string message = "Part has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            b_Part tmpList = null;
            Part.PartChunkSearchLookupListForForSingleStockLineMultiStoreroom_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
}
