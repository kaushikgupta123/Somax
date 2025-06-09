/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person            Description
* =========== ========= ================= ========================================================
* 2014-Oct-12 SOM-363    Roger Lawton     Retrieve summary of ALL costs for the object
*                                         Currently only working for work order
**************************************************************************************************
*/

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the Equipment_TechSpecs table.
    /// </summary>
    public partial class b_OtherCosts
    {
        public string VendorClientLookupId { get; set; }
        public decimal TotalPartCost { get; set; }
        public decimal TotalCraftCost { get; set; }
        public decimal TotalExternalCost { get; set; }
        public decimal TotalInternalCost { get; set; }
        public static object ProcessRowForOtherCostsCrossReference(SqlDataReader reader)
        {
            // Create instance of object
            b_OtherCosts obj = new b_OtherCosts();

            // Load the object from the database
            obj.LoadFromDatabaseByClientLookupId(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseByClientLookupId(SqlDataReader reader)
        {
            //int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // OtherCostsId column, bigint, not null
                OtherCostsId = reader.GetInt64(i++);

                // ObjectType column, nvarchar(15), not null
                ObjectType = reader.GetString(i++);

                // ObjectId column, bigint, not null
                ObjectId = reader.GetInt64(i++);

                // Category column, nvarchar(15), not null
                Category = reader.GetString(i++);

                // CategoryId column, bigint, not null
                CategoryId = reader.GetInt64(i++);

                // Description column, nvarchar(127), not null
                Description = reader.GetString(i++);

                // UnitCost column, decimal(15,5), not null
                UnitCost = reader.GetDecimal(i++);

                // Quantity column, decimal(10,2), not null
                Quantity = reader.GetDecimal(i++);

                // Source column, nvarchar(15), not null
                Source = reader.GetString(i++);

                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                 //ObjectType_Secondary column, nvarchar(15), not null
                ObjectType_Secondary = reader.GetString(i++);

                 //ObjectId_Secondary column, bigint, not null
                ObjectId_Secondary = reader.GetInt64(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);


                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientLookupId = string.Empty;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["OtherCostsId"].ToString(); }
                catch { missing.Append("OtherCostsId "); }

                try { reader["ObjectType"].ToString(); }
                catch { missing.Append("ObjectType "); }

                try { reader["ObjectId"].ToString(); }
                catch { missing.Append("ObjectId "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["CategoryId"].ToString(); }
                catch { missing.Append("CategoryId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["Quantity"].ToString(); }
                catch { missing.Append("Quantity "); }

                try { reader["Source"].ToString(); }
                catch { missing.Append("Source "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static object ProcessRowForOtherCostsSummeryCrossReference(SqlDataReader reader)
        {
            // Create instance of object
            b_OtherCosts obj = new b_OtherCosts();

            // Load the object from the database
            obj.LoadSummeryFromDatabaseByClientLookupId(reader);

            // Return result
            return (object)obj;

        }
        public void LoadSummeryFromDatabaseByClientLookupId(SqlDataReader reader)
        {
            int i = 0;
            if (false == reader.IsDBNull(i))
            {
                TotalPartCost = reader.GetDecimal(i);
            }
            else
            {
                TotalPartCost = 0;
            }
            i++;
            if (false == reader.IsDBNull(i))
            {
                TotalCraftCost = reader.GetDecimal(i);
            }
            else
            {
                TotalCraftCost = 0;
            }
            i++;
            if (false == reader.IsDBNull(i))
            {
                TotalExternalCost = reader.GetDecimal(i);
            }
            else
            {
                TotalExternalCost = 0;
            }
            i++;
            if (false == reader.IsDBNull(i))
            {
                TotalInternalCost = reader.GetDecimal(i);
            }
            else
            {
                TotalInternalCost = 0;
            }
            i++;

        }
        public void RetrieveByObjectIdFromDatabase(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_OtherCosts> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_OtherCosts> results = null;
            data = new List<b_OtherCosts>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_OtherCosts_RetrieveByObjectId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_OtherCosts>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        // SOM-363
        public void SummeryRetrieveByObjectIdFromDatabase(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,
             ref List<b_OtherCosts> data
         )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_OtherCosts> results = null;
            data = new List<b_OtherCosts>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_OtherCosts_SummeryRetrieveByObjectId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_OtherCosts>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrieveByTypeAndObjectId(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,
             ref List<b_OtherCosts> data
         )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_OtherCosts> results = null;
            data = new List<b_OtherCosts>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_OtherCosts_RetrieveByObjectId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_OtherCosts>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrieveByObjectIdandOtherCostId(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_OtherCosts> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_OtherCosts> results = null;
            data = new List<b_OtherCosts>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_OtherCosts_RetrieveByServiceOrderIdandOthercostsid_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_OtherCosts>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        public static b_OtherCosts ProcessRowFortmpOtherCostsPrint(SqlDataReader reader)
        {
            // Create instance of object
            b_OtherCosts obj = new b_OtherCosts();

            // Load the object from the database
            obj.LoadFromDatabaseForOtherCostsPrint(reader);

            // Return result
            return obj;
        }   
        public void LoadFromDatabaseForOtherCostsPrint(SqlDataReader reader)
        {
            //int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {               

                // ObjectId column, bigint, not null
                ObjectId = reader.GetInt64(i++);
                
                // Description column, nvarchar(127), not null
                Description = reader.GetString(i++);

                // UnitCost column, decimal(15,5), not null
                UnitCost = reader.GetDecimal(i++);

                // Quantity column, decimal(10,2), not null
                Quantity = reader.GetDecimal(i++);

                // Source column, nvarchar(15), not null
                Source = reader.GetString(i++);
                               
                if (false == reader.IsDBNull(i))
                {
                    VendorClientLookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientLookupId = string.Empty;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
              
                try { reader["ObjectId"].ToString(); }
                catch { missing.Append("ObjectId "); }
              

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["Quantity"].ToString(); }
                catch { missing.Append("Quantity "); }

                try { reader["Source"].ToString(); }
                catch { missing.Append("Source "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public static b_OtherCosts ProcessRowForSummeryPrint(SqlDataReader reader)
        {
            // Create instance of object
            b_OtherCosts obj = new b_OtherCosts();

            // Load the object from the database
            obj.LoadFromDatabaseForSummeryPrint(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForSummeryPrint(SqlDataReader reader)
        {
            int i = 0;
            // ObjectId column, bigint, not null
            ObjectId = reader.GetInt64(i++);
            if (false == reader.IsDBNull(i))
            {
                TotalPartCost = reader.GetDecimal(i);
            }
            else
            {
                TotalPartCost = 0;
            }
            i++;
            if (false == reader.IsDBNull(i))
            {
                TotalCraftCost = reader.GetDecimal(i);
            }
            else
            {
                TotalCraftCost = 0;
            }
            i++;
            if (false == reader.IsDBNull(i))
            {
                TotalExternalCost = reader.GetDecimal(i);
            }
            else
            {
                TotalExternalCost = 0;
            }
            i++;
            if (false == reader.IsDBNull(i))
            {
                TotalInternalCost = reader.GetDecimal(i);
            }
            else
            {
                TotalInternalCost = 0;
            }
            i++;

        }
       
    }
}
