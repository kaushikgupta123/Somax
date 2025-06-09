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
    class Equipment_RetrieveInitialSearchConfigurationData : AbstractTransactionManager
    {
        public Equipment_RetrieveInitialSearchConfigurationData()
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
                SearchCriteria = StoredProcedure.usp_Equipment_RetrieveInitialSearchConfigurationData.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId);
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

    public class Equipment_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public Equipment_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public long SiteId { get; set; }
        public string Type { get; set; }
        public string SerialNumber { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Equipment> EquipmentList { get; set; }
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

                EquipmentList = StoredProcedure.usp_Equipment_RetrieveLookupListBySearchCriteria_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name, Model, SiteId,
                     Type, SerialNumber, PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out tmp);

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

    public class Equipment_FleetLookupListBySearchCriteria : AbstractTransactionManager
    {
        public Equipment_FleetLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

       
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public long SiteId { get; set; }
    
        public string Make { get; set; }
        public string VIN { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Equipment> EquipmentList { get; set; }
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

                EquipmentList = StoredProcedure.usp_Equipment_FleetLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name, Model, SiteId,
                     Make, VIN, PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out tmp);

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

    public class RetrieveEquipmentReport : AbstractTransactionManager
    {
        public RetrieveEquipmentReport()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<ReportDataStructure> RawList { get; set; }

        public List<string> SelectedValues { get; set; }
        public List<string> SelectedColumns { get; set; }
        public List<string> SelectedNumerics { get; set; }

        public string PrimaryColumn { get; set; }
        public string PrimaryTable { get; set; }

        public string JoinOnColumn { get; set; }
        public string SearchOnColumn { get; set; }
        public string JoinedTable { get; set; }

        public string DateSelection { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Column { get; set; }
        public string SearchText { get; set; }
        public bool UseLike { get; set; }

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

                RawList = StoredProcedure.usp_Equipment_RetrieveForListReport.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId,
                    PrimaryColumn, PrimaryTable, JoinOnColumn, SearchOnColumn, JoinedTable, SelectedValues, SelectedColumns, SelectedNumerics,
                    DateSelection, DateStart, DateEnd, Column, SearchText, UseLike);
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

    public class RetrieveEquipmentBySearchCriteria : AbstractTransactionManager
    {
        public RetrieveEquipmentBySearchCriteria()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public string Query { get; set; }
        public string Site { get; set; }
        public string Area { get; set; }
        public string Department { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string DateSelection { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Column { get; set; }
        public string SearchText { get; set; }
        public bool UseLike { get; set; }
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Equipment> EquipmentList { get; set; }
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

                // Call the stored procedure to retrieve the data
                SqlClient.ProcessRow<b_Equipment> processRow =
                    new SqlClient.ProcessRow<b_Equipment>(reader => { b_Equipment obj = new b_Equipment(); obj.LoadFromDatabase(reader); return obj; });

                int tmp;

                EquipmentList = StoredProcedure.usp_Equipment_RetrieveBySearchCriteria.CallStoredProcedure(command, processRow, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId,
                    Query, Site, Area, Department, Type, Status, DateSelection, DateStart, DateEnd, Column, SearchText, Page, ResultsPerPage, UseLike, OrderColumn, OrderDirection, out tmp);
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
    public class Equipment_ValidateEquipmentIsParentOfAnother : Equipment_TransactionBaseClass
    {
        public Equipment_ValidateEquipmentIsParentOfAnother()
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


                Equipment.ValidateEquipmentIsParentOfAnother(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, CreateMode, lulist, ref errors);

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


    public class Equipment_ValidateByClientLookupId : Equipment_TransactionBaseClass
    {
        public Equipment_ValidateByClientLookupId()
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


                Equipment.ValidateByClientLookupIdFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, CreateMode, lulist, ref errors);

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
    //SOM-899
    public class Equipment_ValidateByProcessSystem : Equipment_TransactionBaseClass
    {
        public Equipment_ValidateByProcessSystem()
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
                Equipment.ValidateByProcessSystem(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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


    public class Equipment_ValidateByInactivateorActivate : Equipment_TransactionBaseClass
    {
        public Equipment_ValidateByInactivateorActivate()
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


                Equipment.ValidateByInactivateorActivate(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    public class Equipment_UpdateByForeignKeys : Equipment_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Equipment.UpdateByForeignKeysInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class Equipment_UpdateByForeignKeys_V2 : Equipment_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Equipment.UpdateByForeignKeysInDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class Equipment_UpdateForPlantLocation : Equipment_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Equipment.UpdateForPlantLocationInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }


         public class Equipment_UpdateForVoidbyFleetMeterandEquipmentId : Equipment_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Equipment.UpdateForVoidbyFleetMeterandEquipmentIdInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }



    public class Equipment_UpdateForVoidFromFuelTracking : Equipment_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Equipment.UpdateEquipmentForVoidFromFuelTrackingInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class Equipment_UpdateFoFeetMeter : Equipment_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Equipment.UpdateForFleetMeterInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }


    public class Equipment_UpdateFORFuelTracking : Equipment_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Equipment.EquipmentUpdateFORFuelTrackingInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class Equipment_RetrieveByClientLookupId : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            Equipment.RetrieveByClientLookupIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    public class Equipment_RetrieveByEquipmentIdandFuelTrackingId : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            Equipment.RetrieveByEquipmentIdandFuelTrackingIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Equipment_RetrieveByForeignKeys : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            Equipment.RetrieveByForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Equipment_RetrieveByForeignKeys_V2 : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            Equipment.RetrieveByForeignKeysFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Equipment_RetrieveAllBySiteId_V2 : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> equipList = new List<b_Equipment>();
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                //string message = "Equipment has an invalid ID.";
                //throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            List<b_Equipment> tempList = new List<b_Equipment>();
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            Equipment.RetrieveAllBySiteId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            equipList = tempList;
        }
    }

    public class Equipment_CreateByForeignKeys : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Equipment.InsertByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Equipment.EquipmentId > 0);
        }
    }

    public class Equipment_CreateByForeignKeys_V2 : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Equipment.InsertByForeignKeysIntoDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Equipment.EquipmentId > 0);
        }
    }


    public class Equipment_RetrieveClientLookupIdBySearchCriteria : Equipment_TransactionBaseClass
    {

        public List<b_Equipment> EquipmentList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Equipment> tmpList = null;
            Equipment.RetrieveClientLookupIdBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            EquipmentList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Equipment_RetrieveByLocationId : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }

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
            List<b_Equipment> tmpArray = null;

            Equipment.RetrieveByLocationIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            EquipmentList = new List<b_Equipment>();
            foreach (b_Equipment tmpObj in tmpArray)
            {
                EquipmentList.Add(tmpObj);
            }
        }
    }

    public class AssetGroup1_ValidateIfUsedInEquipment : Equipment_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Equipment.AssetGroup1_ValidateIfUsedInEquipment(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }



    public class AssetGroup2_ValidateIfUsedInEquipment : Equipment_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Equipment.AssetGroup2_ValidateIfUsedInEquipment(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class AssetGroup3_ValidateIfUsedInEquipment : Equipment_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Equipment.AssetGroup3_ValidateIfUsedInEquipment(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    //------------------------Added By Indusnet Technologies--------------------------------------------------
    public class RetrieveAllEquipmentBySiteId : AbstractTransactionManager
    {
        public RetrieveAllEquipmentBySiteId()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public long SiteId { get; set; }

        // Result Sets
        public List<b_Equipment> EquipmentList { get; set; }
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

                // Call the stored procedure to retrieve the data
                SqlClient.ProcessRow<b_Equipment> processRow =
                   new SqlClient.ProcessRow<b_Equipment>(reader =>
                   {
                       b_Equipment obj = new b_Equipment();
                       obj.LoadFromDatabaseforSearch(reader);
                       return obj;
                   });

                int tmp;

                EquipmentList = StoredProcedure.usp_Equipment_RetrieveAllBySiteID.CallStoredProcedure(command, processRow, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, SiteId, out tmp);

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
    public class RetrieveAllBasedOnCriteria : AbstractTransactionManager
    {
        public RetrieveAllBasedOnCriteria()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public System.Data.DataTable EquipIds { get; set; }
        public string Type { get; set; }
        public long SiteId { get; set; }

        // Result Sets
        public List<b_Equipment> EquipmentList { get; set; }
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

                // Call the stored procedure to retrieve the data
                SqlClient.ProcessRow<b_Equipment> processRow =
                   new SqlClient.ProcessRow<b_Equipment>(reader =>
                   {
                       b_Equipment obj = new b_Equipment();
                       obj.LoadFromDatabaseforSearch(reader);
                       return obj;
                   });

                int tmp;

                EquipmentList = StoredProcedure.usp_Equipment_RetrieveAllBasedOnCriteria.CallStoredProcedure(command, processRow, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, SiteId, Type, EquipIds, out tmp);

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

    public class GetAllEquipment : AbstractTransactionManager
    {
        public List<b_Equipment> EquipmentList;

        #region Constructor
        public GetAllEquipment()
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

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                // Call the stored procedure to retrieve the data
                EquipmentList = StoredProcedure.usp_Equipment_GetAll_V2.CallStoredProcedure
                    (command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId);
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


    public class Equipment_GetAllDeptLineSys : Equipment_TransactionBaseClass
    {

        public List<b_Equipment> EquipmentList { get; set; }

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
            List<b_Equipment> tmpList = null;
            Equipment.GetAllDeptLineSys(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            EquipmentList =new List<b_Equipment>();

            foreach (b_Equipment tmpObj in tmpList)
            {
                EquipmentList.Add(tmpObj);
            }
        }

        
    }

   
    public class Equipment_DeleteByFK : Equipment_TransactionBaseClass
    {
        public bool retValue { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            bool result = false;
            Equipment.DeleteEquipment(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref result);
            retValue = result;
        }
    }

    //-----SOM-774-----//
    public class Equipment_ChangeClientLookupId : Equipment_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Equipment.ChangeClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    //-----SOM-784-----//
    public class Equipment_RetrieveCreateModifyDate : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            Equipment.RetrieveCreateModifyDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    //-----SOM - 893------//
    public class Equipment_GetAllEquipmentChildren : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            List<b_Equipment> tempList = null;
            base.UseTransaction = false;
            Equipment.GetAllEquipmentChildren(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            this.EquipmentList = tempList;
        }
    }
    public class Equipment_GetAllEquipmentFreeChildren : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            List<b_Equipment> tempList = null;
            base.UseTransaction = false;
            Equipment.GetAllEquipmentFreeChildren(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            this.EquipmentList = tempList;
        }
    }
    public class Equipment_ValidateClientLookupId : Equipment_TransactionBaseClass
    {
        public Equipment_ValidateClientLookupId()
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

                Equipment.ValidateClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class Equipment_ValidateForeignField : Equipment_TransactionBaseClass
    {
        public Equipment_ValidateForeignField()
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

                Equipment.ValidateForeignField(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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


    public class Department_ValidateIfUsedInEquipment : Equipment_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Equipment.Department_ValidateIfUsedInEquipment(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class Line_ValidateIfUsedInEquipment : Equipment_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Equipment.Line_ValidateIfUsedInEquipment(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class SystemInfo_ValidateIfUsedInEquipment : Equipment_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Equipment.SystemInfo_ValidateIfUsedInEquipment(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class Equipment_RetrieveAll_V2 : AbstractTransactionManager
    {

        public Equipment_RetrieveAll_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Equipment> EquipmentList { get; set; }

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
            b_Equipment[] tmpArray = null;
            b_Equipment o = new b_Equipment();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;


            o.RetrieveAll_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            EquipmentList = new List<b_Equipment>(tmpArray);
        }
    }

    public class Equipment_UpdateBulk : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (string.IsNullOrEmpty(Equipment.EquipmentIdList))
            {
                string message = "Equipment List is empty";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Equipment.UpdateInBulk(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }


    public class Equipment_RetrieveforMentionAlert : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            Equipment.RetrieveForMentionAlert(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class Equipment_RetrieveChunkSearchV2 : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_Equipment tmpList = null;
            Equipment.RetrieveChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Equipment_RetrieveFleetAssetChunkSearchV2 : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_Equipment tmpList = null;
            Equipment.RetrieveFleetAssetChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #region V2-1213
    public class Equipment_RetrieveAllChildrenChunkSearchV2 : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();           
        }
        public override void PerformWorkItem()
        {
            b_Equipment tmpList = null;
            Equipment.RetrieveAllChildrenChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    #endregion
    public class Equipment_RetrieveFleetMeterReadingChunkSearchV2 : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_Equipment tmpList = null;
            Equipment.RetrieveFleetMeterReadingChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Equipment_RetrieveFleetFuelChunkSearchV2 : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_Equipment tmpList = null;
            Equipment.RetrieveAllChildrenChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class FleetFuel_ValidateMeter1Reading : Equipment_TransactionBaseClass
    {
        public FleetFuel_ValidateMeter1Reading()
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

                Equipment.ValidateFleetFuelMeter1CurrentReading(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class FleetMeterReading_ValidateMeterReadingForBothMeter : Equipment_TransactionBaseClass
    {
        public FleetMeterReading_ValidateMeterReadingForBothMeter()
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

                Equipment.ValidateMeterReadingForBothMeter(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class FleetMeterReading_ValidiationForUnvoidforFleetMeterandFuelTracking : Equipment_TransactionBaseClass
    {
        public FleetMeterReading_ValidiationForUnvoidforFleetMeterandFuelTracking()
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

                Equipment.ValidiationForUnvoidforFleetMeterandFuelTracking(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

  

    #region Fuel Tracking Retrieve by Equipment Id
    public class Equipment_RetrieveFleetFuelByEquipmentIdV2 : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Equipment> tmpList = null;
            Equipment.RetrieveFleetFuelByEquipmentIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EquipmentList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region
    public class Equipment_RetrieveFleetMeterReadingByEquipmentIdV2 : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Equipment> tmpList = null;
            Equipment.RetrieveFleetMeterReadingByEquipmentIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EquipmentList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Equipment Lookuplist chunk search
    public class Equipment_RetrieveChunkSearchLookupListV2 : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Equipment> tmpList = null;
            Equipment.RetrieveEquipmentLookuplistChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EquipmentList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Equipment Lookuplist chunk search for Mobile V2
    public class Equipment_RetrieveChunkSearchLookupListMobile_V2 : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Equipment> tmpList = null; 
            Equipment.RetrieveEquipmentLookuplistChunkSearchMobile_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EquipmentList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Retrieve EquipmentId by ClientLookupId V2-625
    public class Equipment_RetrieveEquipmentIdbyClientLookupId_V2 : Equipment_TransactionBaseClass
    {

        public b_Equipment EquipmentResult { get; set; }

        public override void PerformLocalValidation()
        {
            if (string.IsNullOrEmpty(Equipment.ClientLookupId))
            {
                string message = "Equipment has an Client Lookup ID.";
                throw new Exception(message);
            }
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Equipment tmpList = null;
            Equipment.RetrieveEquipmentIdByClientLookupIdV2FromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            EquipmentResult = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Validate by scrap
    public class Equipment_ValidateByScrap : Equipment_TransactionBaseClass
    {
        public Equipment_ValidateByScrap()
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


                Equipment.ValidateByScrap(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    #region V2-637 Repairable Spare Asset
    public class Equipment_RetrieveRepSpareAssetChunkSearchLookupListV2 : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Equipment> tmpList = null;
            Equipment.RetrieveEquipmentLookuplistForRepSpareAssetChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EquipmentList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Equipment_RepairableSpareAsset_V2 : Equipment_TransactionBaseClass
    {
        public b_RepairableSpareLog RepairableSpareLog { get; set; } 
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Equipment.RepairableSpareLog = RepairableSpareLog;
            Equipment.AddRepairableSpareAssetWizard(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Equipment.EquipmentId > 0);
        }
    }
    #endregion

    #region  V2-835
    public class Equipment_RetrieveChunkSearchMobileV2 : Equipment_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId > 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_Equipment tmpList = null;
            Equipment.RetrieveChunkSearchMobile_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region V2-948
    public class Equipment_RetrieveAccountByEquipmentIdV2 : Equipment_TransactionBaseClass
    {

        public b_Equipment EquipmentResult { get; set; }

        public override void PerformLocalValidation()
        {
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid id";
                throw new Exception(message);
            }
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Equipment tmpList = null;
            Equipment.RetrieveAccountByEquipmentIdV2FromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            EquipmentResult = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region V2-846 
    public class GetAllEquipmentParentV2 : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_Equipment> tmpList = null;
            Equipment.GetAllEquipmentParent(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EquipmentList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Equipment_GetAllEquipmentChildrenForParent : Equipment_TransactionBaseClass
    {
        public List<b_Equipment> EquipmentList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Equipment.EquipmentId == 0)
            {
                string message = "Equipment has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            List<b_Equipment> tempList = null;
            base.UseTransaction = false;
            Equipment.GetAllEquipmentChildrenForParent(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            this.EquipmentList = tempList;
        }
    }
    #endregion
}
