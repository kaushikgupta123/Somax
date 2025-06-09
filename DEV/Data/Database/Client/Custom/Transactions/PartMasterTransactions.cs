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
    public class PartMasterRetrieveAll_ByInactiveFlag : PartMaster_TransactionBaseClass
    {
        public bool InactiveFlag { get; set; }
        public List<b_PartMaster> PartMasterList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformWorkItem()
        {
            b_PartMaster[] tmpArray = null;
           
            // Explicitly set id from dbkey
            PartMaster.ClientId = this.dbKey.Client.ClientId;


            PartMaster.PartMaster_RetrieveAll_ByInactiveFlag(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray, InactiveFlag);

            PartMasterList = new List<b_PartMaster>(tmpArray);
        }

    }
    public class PartMaster_ValidateByClientlookupId : PartMaster_TransactionBaseClass
    {
        public string Mode { get; set; }

        public PartMaster_ValidateByClientlookupId()
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
                PartMaster.ValidateByClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    //SOM-1497
    public class PartMasterReview_RetrieveBySearchCriteria : PartMaster_TransactionBaseClass
    {
        public string SearchCriteria { get; set; }
        public long Siteid { get; set; }
        public List<b_PartMaster> PartMasterList { get; set; }


        public override void PerformWorkItem()
        {
            b_PartMaster[] tmpArray = null;

            // Explicitly set id from dbkey
            PartMaster.ClientId = this.dbKey.Client.ClientId;


            PartMaster.PartMasterReview_RetrieveBySearchCriteria(this.Connection, this.Transaction, SearchCriteria, Siteid, ref tmpArray);

            PartMasterList = new List<b_PartMaster>(tmpArray);
        }

    }
     
    public class Partmaster_RetrieveByImageURL : PartMaster_TransactionBaseClass
    {
        public List<b_PartMaster> PartMasterList { get; set; }

        public override void PerformWorkItem()
        {
            b_PartMaster[] tmpArray = null;

            // Explicitly set id from dbkey
            PartMaster.ClientId = this.dbKey.Client.ClientId;

            PartMaster.PartMaster_RetrieveByImageURL(this.Connection, this.Transaction, ref tmpArray);

            PartMasterList = new List<b_PartMaster>(tmpArray);
        }

    }

    public class PartMaster_ValidateForChangeClientlookupId : PartMaster_TransactionBaseClass
    {

        public PartMaster_ValidateForChangeClientlookupId()
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
                PartMaster.ValidateForChangeClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    public class ShoppingCart_RetrieveBySearchCriteria : PartMaster_TransactionBaseClass
    {
        public string SearchCriteria { get; set; }
        public long Siteid { get; set; }
        public List<b_PartMaster> PartMasterList { get; set; }
       

        public override void PerformWorkItem()
        {
            b_PartMaster[] tmpArray = null;

            // Explicitly set id from dbkey
            PartMaster.ClientId = this.dbKey.Client.ClientId;


            PartMaster.ShoppingCart_RetrieveBySearchCriteria(this.Connection, this.Transaction, SearchCriteria, Siteid, ref tmpArray);

            PartMasterList = new List<b_PartMaster>(tmpArray);
        }

    }
    public class PartMaster_RetrieveByForeignKeys : PartMaster_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartMaster.PartMasterId == 0)
            {
                string message = "PartMaster has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PartMaster.RetrieveByForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class PartMaster_ChangeClientLookupId : PartMaster_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PartMaster.PartMasterId == 0)
            {
                string message = "PartMaster has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            PartMaster.ChangeClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class PartMaster_RetrieveVendorCatalogBySearchCriteria : PartMaster_TransactionBaseClass
    {

        public List<b_PartMaster> PartMasterList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_PartMaster> tmpList = null;
            PartMaster.RetrieveVendorCatalogBySearchCriteria(this.Connection, this.Transaction, ref tmpList);
            PartMasterList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class PartMasterRetrieve_ByPart : PartMaster_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (PartMaster.PartMasterId == 0)
            //{
            //    string message = "PartMaster has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PartMaster.PartMaster_Retrieve_ByPart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class PartMaster_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public PartMaster_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string ShortDescription { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_PartMaster> PartMasterList { get; set; }
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

                PartMasterList = usp_PartMaster_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, ShortDescription,
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

    public class PartMaster_RetrieveNumberLookup : AbstractTransactionManager
    {
        public PartMaster_RetrieveNumberLookup()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string ShortDescription { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_PartMaster> PartMasterList { get; set; }
        public b_PartMaster PMaster { get; set; }
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

                PartMasterList = usp_PartMaster_NumberLookup.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName,PMaster );

                ResultCount = PMaster.ResultCount;
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

    public class PartMaster_RetrieveSomaxPartLookup : AbstractTransactionManager
    {
        public PartMaster_RetrieveSomaxPartLookup()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        // Result Sets
        public List<b_PartMaster> PartMasterList { get; set; }
        public b_PartMaster PMaster { get; set; }
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

                PartMasterList = usp_PartMaster_SomaxPartLookup.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, PMaster);

                ResultCount = PMaster.ResultCount;
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

    

}
