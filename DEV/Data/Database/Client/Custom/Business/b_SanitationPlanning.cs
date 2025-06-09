/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2015-Mar-12 SOM-585  Roger Lawton        Added Items to support sanitation
***************************************************************************************************
*/
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_SanitationPlanning
    {

        public void RetrieveBySanitationMasterId(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           long ClientId,
           long SanitationMasterId,
           string Category,
           ref List<b_SanitationPlanning> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_SanitationPlanning> results = null;
            Database.SqlClient.ProcessRow<b_SanitationPlanning> processRow = null;
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationPlanning>(reader => 
                                { 
                                    b_SanitationPlanning obj = new b_SanitationPlanning(); 
                                    obj.LoadFromDatabase(reader);
                                    return obj; 
                                });
                results = Database.StoredProcedure.usp_SanitationPlanning_RetrieveByMasterId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SanitationMasterId,Category);

                // Extract the results
                if (null != results)
                {
                   data = new List<b_SanitationPlanning>(results);
                }
                else
                {
                    data = new List<b_SanitationPlanning>();
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void RetrieveBy_SanitationJobId(
              SqlConnection connection,
              SqlTransaction transaction,
              long callerUserInfoId,
              string callerUserName,
              ref List<b_SanitationPlanning> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_SanitationPlanning> results = null;
           Database.SqlClient.ProcessRow<b_SanitationPlanning> processRow = null;
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationPlanning>(reader =>
                {
                    b_SanitationPlanning obj = new b_SanitationPlanning();
                    obj.LoadSanitationPlanning_FromDatabase(reader);
                    return obj;
                });

                results =Database.StoredProcedure.usp_SanitationPlanning_RetriveBy_SanitationJobId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = new List<b_SanitationPlanning>(results);
                }
                else
                {
                    data = new List<b_SanitationPlanning>();
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public int LoadSanitationPlanning_FromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SanitationPlanningId column, bigint, not null
                SanitationPlanningId = reader.GetInt64(i++);

                // SanitationMasterId column, bigint, not null
                SanitationMasterId = reader.GetInt64(i++);

                SanitationJobId = reader.GetInt64(i++);

                // Category column, nvarchar(15), not null
                Category = reader.GetString(i++);

                // CategoryValue column, nvarchar(15), not null
                CategoryValue = reader.GetString(i++);

                // CategoryId column, bigint, not null
                CategoryId = reader.GetInt64(i++);

                // Description column, nvarchar(127), not null
                Description = reader.GetString(i++);

                // Dilution column, nvarchar(31), not null
                Dilution = reader.GetString(i++);

                // Instructions column, nvarchar(MAX), not null
                Instructions = reader.GetString(i++);

                // Quantity column, decimal(15,6), not null
                Quantity = reader.GetDecimal(i++);

                // UnitCost column, decimal(15,5), not null
                UnitCost = reader.GetDecimal(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SanitationPlanningId"].ToString(); }
                catch { missing.Append("SanitationPlanningId "); }

                try { reader["SanitationMasterId"].ToString(); }
                catch { missing.Append("SanitationMasterId "); }

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }


                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["CategoryValue"].ToString(); }
                catch { missing.Append("CategoryValue "); }

                try { reader["CategoryId"].ToString(); }
                catch { missing.Append("CategoryId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Dilution"].ToString(); }
                catch { missing.Append("Dilution "); }

                try { reader["Instructions"].ToString(); }
                catch { missing.Append("Instructions "); }

                try { reader["Quantity"].ToString(); }
                catch { missing.Append("Quantity "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }
        public static b_SanitationPlanning ProcessRowForDevExpressSanitationPlanning(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationPlanning obj = new b_SanitationPlanning();

            // Load the object from the database
            obj.LoadFromDatabaseForDevExpressSanitationPlanning(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForDevExpressSanitationPlanning(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                SanitationJobId = reader.GetInt64(i++);
                // SanitationPlanningId column, bigint, not null
                SanitationPlanningId = reader.GetInt64(i++);
                // CategoryValue column, nvarchar(15), not null
                CategoryValue = reader.GetString(i++);
                // Description column, nvarchar(127), not null
                Description = reader.GetString(i++);
                // Instructions column, nvarchar(MAX), not null
                Instructions = reader.GetString(i++);
                // Quantity column, decimal(15,6), not null
                Quantity = reader.GetDecimal(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }

                try { reader["SanitationPlanningId"].ToString(); }
                catch { missing.Append("SanitationPlanningId "); }

                try { reader["CategoryValue"].ToString(); }
                catch { missing.Append("CategoryValue "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }

                try { reader["Instructions"].ToString(); }
                catch { missing.Append("Instructions "); }

                try { reader["Quantity"].ToString(); }
                catch { missing.Append("Quantity "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }

        public static b_SanitationPlanning ProcessRowForDevExpressSanitationPlanningChemical(SqlDataReader reader)
        {
            // Create instance of object
            b_SanitationPlanning obj = new b_SanitationPlanning();

            // Load the object from the database
            obj.LoadFromDatabaseForDevExpressSanitationPlanningChemical(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForDevExpressSanitationPlanningChemical(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                SanitationJobId = reader.GetInt64(i++);
                // SanitationPlanningId column, bigint, not null
                SanitationPlanningId = reader.GetInt64(i++);
                // CategoryValue column, nvarchar(15), not null
                CategoryValue = reader.GetString(i++);
                // Description column, nvarchar(127), not null
                Description = reader.GetString(i++);
                // Instructions column, nvarchar(MAX), not null
                Instructions = reader.GetString(i++);
                // Quantity column, decimal(15,6), not null
                Quantity = reader.GetDecimal(i++);
                // Dilution column
                Dilution = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }

                try { reader["SanitationPlanningId"].ToString(); }
                catch { missing.Append("SanitationPlanningId "); }

                try { reader["CategoryValue"].ToString(); }
                catch { missing.Append("CategoryValue "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }

                try { reader["Instructions"].ToString(); }
                catch { missing.Append("Instructions "); }

                try { reader["Quantity"].ToString(); }
                catch { missing.Append("Quantity "); }

                try { reader["Dilution"].ToString(); }
                catch { missing.Append("Dilution "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }
    }
}
