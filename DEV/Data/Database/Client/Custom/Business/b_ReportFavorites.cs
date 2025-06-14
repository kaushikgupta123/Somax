/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2020 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the ReportListing table.InsertIntoDatabase
    /// </summary>

    public partial class b_ReportFavorites
    {
        #region Properties
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string ReportGroup { get; set; }
        public string SourceName { get; set; }
        public bool UseSP { get; set; }
        public string PrimarySortColumn { get; set; }
        public string SecondarySortColumn { get; set; }
        public bool IsGrouped { get; set; }
        public string GroupColumn { get; set; }
        public bool IncludePrompt { get; set; }
        public string Prompt1Source { get; set; }
        public string Prompt1Type { get; set; }
        public string Prompt1ListSource { get; set; }
        public string Prompt1List { get; set; }
        public string Prompt2Source { get; set; }
        public string Prompt2Type { get; set; }
        public string Prompt2ListSource { get; set; }
        public string Prompt2List { get; set; }
        public string SaveType { get; set; }

        #endregion

        public void RetrieveMyFavorites(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
       string callerUserName,
             ref List<b_ReportFavorites> data
         )
        {
            Database.SqlClient.ProcessRow<b_ReportFavorites> processRow = null;
            List<b_ReportFavorites> results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new List<b_ReportFavorites>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ReportFavorites>(reader => { b_ReportFavorites obj = new b_ReportFavorites(); obj.LoadFromDatabaseFavorites(reader); return obj; });
                results = Database.StoredProcedure.usp_ReportFavorites_RetrieveMyFavorites.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = null;
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
        public void UpdateMyFavorite(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName
         )
        {
            Database.SqlClient.ProcessRow<b_ReportFavorites> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                Database.StoredProcedure.usp_ReportFavorites_UpdateMyFavorites.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void LoadFromDatabaseFavorites(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                ReportName = reader.GetString(i++);
                Description = reader.GetString(i++);
                ReportGroup = reader.GetString(i++);
                SourceName = reader.GetString(i++);
                UseSP = reader.GetBoolean(i++);
                PrimarySortColumn = reader.GetString(i++);
                SecondarySortColumn = reader.GetString(i++);
                IsGrouped = reader.GetBoolean(i++);
                GroupColumn = reader.GetString(i++);
                IncludePrompt = reader.GetBoolean(i++);
                Prompt1Source = reader.GetString(i++);
                Prompt1Type = reader.GetString(i++);
                Prompt1ListSource = reader.GetString(i++);
                Prompt1List = reader.GetString(i++);

                Prompt2Source = reader.GetString(i++);
                Prompt2Type = reader.GetString(i++);
                Prompt2ListSource = reader.GetString(i++);
                Prompt2List = reader.GetString(i++);

                SaveType = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();

                try { reader["ReportName"].ToString(); }
                catch { missing.Append("ReportName "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ReportGroup"].ToString(); }
                catch { missing.Append("ReportGroup "); }

                try { reader["UseSP"].ToString(); }
                catch { missing.Append("UseSP"); }

                try { reader["PrimarySortColumn"].ToString(); }
                catch { missing.Append("PrimarySortColumn"); }

                try { reader["SecondarySortColumn"].ToString(); }
                catch { missing.Append("SecondarySortColumn"); }

                try { reader["GroupColumn"].ToString(); }
                catch { missing.Append("GroupColumn"); }

                try { reader["IncludePrompt"].ToString(); }
                catch { missing.Append("IncludePrompt"); }

                try { reader["Prompt1Source"].ToString(); }
                catch { missing.Append("Prompt1Source"); }

                try { reader["Prompt1Type"].ToString(); }
                catch { missing.Append("Prompt1Type"); }

                try { reader["Prompt1ListSource"].ToString(); }
                catch { missing.Append("Prompt1ListSource"); }

                try { reader["Prompt1List"].ToString(); }
                catch { missing.Append("Prompt1List"); }



                try { reader["Prompt2Source"].ToString(); }
                catch { missing.Append("Prompt2Source"); }

                try { reader["Prompt2Type"].ToString(); }
                catch { missing.Append("Prompt2Type"); }

                try { reader["Prompt2List"].ToString(); }
                catch { missing.Append("Prompt2List"); }

                try { reader["Prompt2ListSource"].ToString(); }
                catch { missing.Append("Prompt2ListSource"); }

                try { reader["SaveType"].ToString(); }
                catch { missing.Append("SaveType"); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
    }

}
