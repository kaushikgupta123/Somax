/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* b_Attachment.cs - Custom Business Object
**************************************************************************************************
* Copyright (c) 2013-2018 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person         Description
* =========== ========= ============= ==========================================================
* 2018-Nov-05 SOM-1650  Roger Lawton  Added the DeleteDbStoredAttachment Method
**************************************************************************************************
*/
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;

namespace Database.Business
{
  public partial class b_Attachment : DataBusinessBase
  {
    public string CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string UploadedBy { get; set; }
    public int URLCount { get; set; }
    public long siteid { get; set; }

        #region V2-716
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public int TotalCount { get; set; }
        #endregion

        /// <summary>
        /// DeleteDbStoredAttachment
        /// Delete attachments that are stored in the database
        /// SP deletes the Attachment Record, FileAttachment Record 
        /// and if no more references exist the FileInfo Record
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void DeleteDbStoredAttachment(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
    {
      SqlCommand command = null;

      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_Attachment_DeleteDbStoredAttachment.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
      }
    }
    /// <summary>
    /// UpdateForMigrate
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="transaction"></param>
    /// <param name="callerUserInfoId"></param>
    /// <param name="callerUserName"></param>
    public void UpdateForMigrate(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
    {
      SqlCommand command = null;

      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_Attachment_UpdateForMigrate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
      }
    }

    /// <summary>
    /// Retrieve the URL Count for this Attachment Record
    /// </summary>
    /// <param name="connection">SqlConnection containing the database connection</param>
    /// <param name="transaction">SqlTransaction containing the database transaction</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    public void RetrieveURLCount(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
    {
      Database.SqlClient.ProcessRow<b_Attachment> processRow = null;
      SqlCommand command = null;
      string message = String.Empty;
      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;
        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_Attachment>(reader => { this.LoadFromDbForURLCount(reader); return this; });
        Database.StoredProcedure.usp_Attachment_RetrieveURLCount.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void AttachmentRetrieveURLCount_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Attachment> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;
            
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                // Call the stored procedure to retrieve the data
                //processRow = new Database.SqlClient.ProcessRow<b_Attachment>(reader => { this.LoadFromDbForURLCount(reader); return this; });
                URLCount=Database.StoredProcedure.usp_Attachment_RetrieveURLCount_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        /// <summary>
        /// RetrieveListByFileNameFromDatabase
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="callerUserInfoId"></param>
        /// <param name="callerUserName"></param>
        public void RetrieveListByFileNameFromDatabase(
                   SqlConnection connection,
                   SqlTransaction transaction,
                   long callerUserInfoId,
                   string callerUserName
               )
    {
      Database.SqlClient.ProcessRow<b_Attachment> processRow = null;
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_Attachment>(reader => { this.LoadFromDbForAttachment(reader); return this; });
        Database.StoredProcedure.usp_Attachment_RetrieveListByFileName.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

    public void RetrieveAllAttachmentsForObject(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref b_Attachment[] data)
    {
      Database.SqlClient.ProcessRow<b_Attachment> processRow = null;
      ArrayList results = null;
      SqlCommand command = null;
      string message = String.Empty;

      // Initialize the results
      data = new b_Attachment[0];

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_Attachment>(reader => { b_Attachment obj = new b_Attachment(); obj.LoadFromDbForAttachment(reader); return obj; });
        results = Database.StoredProcedure.usp_Attachment_RetrieveAllAttachmentsForObject.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

        // Extract the results
        if (null != results)
        {
          data = (b_Attachment[])results.ToArray(typeof(b_Attachment));
        }
        else
        {
          data = new b_Attachment[0];
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



    public void RetrieveListFromDatabase(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
       string callerUserName,
             ref b_Attachment[] data
         )
    {
      Database.SqlClient.ProcessRow<b_Attachment> processRow = null;
      ArrayList results = null;
      SqlCommand command = null;
      string message = String.Empty;

      // Initialize the results
      data = new b_Attachment[0];

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_Attachment>(reader => { b_Attachment obj = new b_Attachment(); obj.LoadFromDbForAttachment(reader); return obj; });
        results = Database.StoredProcedure.usp_Attachment_RetrieveAllAttachment.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

        // Extract the results
        if (null != results)
        {
          data = (b_Attachment[])results.ToArray(typeof(b_Attachment));
        }
        else
        {
          data = new b_Attachment[0];
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
    public void RetrieveProfileAttachments(SqlConnection connection, SqlTransaction transaction
          , long callerUserInfoId, string callerUserName, ref b_Attachment[] data)
    {
      Database.SqlClient.ProcessRow<b_Attachment> processRow = null;
      ArrayList results = null;
      SqlCommand command = null;
      string message = String.Empty;

      // Initialize the results
      data = new b_Attachment[0];

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_Attachment>(reader => { b_Attachment obj = new b_Attachment(); obj.LoadFromDbForAttachment(reader); return obj; });
        results = Database.StoredProcedure.usp_Attachment_RetrieveProfileAttachments.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

        // Extract the results
        if (null != results)
        {
          data = (b_Attachment[])results.ToArray(typeof(b_Attachment));
        }
        else
        {
          data = new b_Attachment[0];
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
    /// <summary>
    /// LoadFromDbForURLCount
    /// SOM-1650
    /// Used the base LoadFromDatabase method
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public int LoadFromDbForURLCount(SqlDataReader reader)
    {

      int i = LoadFromDatabase(reader);
      try
      {
        // UploadedBy string 
        URLCount = reader.GetInt32(i++);
      }
      catch (Exception ex)
      {
        // Diagnostics
        StringBuilder missing = new StringBuilder();

        try { reader["URLCount"].ToString(); }
        catch { missing.Append("URLCount "); }

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
    /// <summary>
    /// Retrieve Attachment table records with specified primary key from the database.
    /// </summary>
    /// <param name="connection">SqlConnection containing the database connection</param>
    /// <param name="transaction">SqlTransaction containing the database transaction</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    public void RetrieveLogo(SqlConnection connection,SqlTransaction transaction,long callerUserInfoId,string callerUserName)
    {
      Database.SqlClient.ProcessRow<b_Attachment> processRow = null;
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_Attachment>(reader => { this.LoadFromDatabase(reader); return this; });
        Database.StoredProcedure.usp_Attachment_RetrieveLogo.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


    /// <summary>
    /// LoadFromDbForAttachment
    /// SOM-1650
    /// Used the base LoadFromDatabase method
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public int LoadFromDbForAttachment(SqlDataReader reader)
    {

      int i = LoadFromDatabase(reader);
      try
      {
        // UploadedBy string 
        UploadedBy = reader.GetString(i++);

        // CreateDate column, int, not null
        CreateDate = reader.GetDateTime(i++);

        // CreateBy column, int, not null
        CreateBy = reader.GetString(i++);

        // ModifiedDate column, int, not null
        ModifiedDate = reader.GetDateTime(i++);

        // ModifiedBy column, int, not null
        ModifiedBy = reader.GetString(i++);
      }
      catch (Exception ex)
      {
        // Diagnostics
        StringBuilder missing = new StringBuilder();

        try { reader["UploadedBy"].ToString(); }
        catch { missing.Append("UploadedBy "); }

        try { reader["CreateDate"].ToString(); }
        catch { missing.Append("CreateDate "); }

        try { reader["CreateBy"].ToString(); }
        catch { missing.Append("CreateBy "); }

        try { reader["ModifiedDate"].ToString(); }
        catch { missing.Append("ModifiedDate "); }

        try { reader["ModifiedBy"].ToString(); }
        catch { missing.Append("ModifiedBy "); }

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

        public static b_Attachment ProcessRowForAttachmentPrint(SqlDataReader reader)
        {
            // Create instance of object
            b_Attachment obj = new b_Attachment();

            // Load the object from the database
            obj.LoadFromDatabaseForAttachmentPrint(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForAttachmentPrint(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ObjectId column, bigint, not null
                ObjectId = reader.GetInt64(i++);

                // AttachmentURL column, nvarchar(511), not null
                AttachmentURL = reader.GetString(i++);

                // FileName column, nvarchar(511), not null
                FileName = reader.GetString(i++);


                // ContentType column, nvarchar(127), not null
                ContentType = reader.GetString(i++);

                // FileSize column, int, not null
                FileSize = reader.GetInt32(i++);

                // Image column, bit, not null
                Image = reader.GetBoolean(i++);

                // Profile column, bit, not null
                Profile = reader.GetBoolean(i++);

                // PrintwithForm column, bit, not null
                PrintwithForm = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ObjectId"].ToString(); }
                catch { missing.Append("ObjectId "); }

                try { reader["AttachmentURL"].ToString(); }
                catch { missing.Append("AttachmentURL "); }

                try { reader["FileName"].ToString(); }
                catch { missing.Append("FileName "); }

                try { reader["ContentType"].ToString(); }
                catch { missing.Append("ContentType "); }

                try { reader["FileSize"].ToString(); }
                catch { missing.Append("FileSize "); }

                try { reader["Image"].ToString(); }
                catch { missing.Append("Image "); }

                try { reader["Profile"].ToString(); }
                catch { missing.Append("Profile "); }

                try { reader["PrintwithForm"].ToString(); }
                catch { missing.Append("PrintwithForm "); }

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

        #region V2-716
        public void RetrieveMultipleProfileAttachments(SqlConnection connection, SqlTransaction transaction
          , long callerUserInfoId, string callerUserName, ref b_Attachment[] data)
        {
            Database.SqlClient.ProcessRow<b_Attachment> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Attachment[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Attachment>(reader => { b_Attachment obj = new b_Attachment(); obj.LoadFromDbForMultipleImage(reader); return obj; });
                results = Database.StoredProcedure.usp_Attachment_RetrieveMultipleImages_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_Attachment[])results.ToArray(typeof(b_Attachment));
                }
                else
                {
                    data = new b_Attachment[0];
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
        public int LoadFromDbForMultipleImage(SqlDataReader reader)
        {

            int i = LoadFromDatabase(reader);
            try
            {
                // UploadedBy string 
                UploadedBy = reader.GetString(i++);

                // CreateDate column, int, not null
                CreateDate = reader.GetDateTime(i++);

                // CreateBy column, int, not null
                CreateBy = reader.GetString(i++);

                // ModifiedDate column, int, not null
                ModifiedDate = reader.GetDateTime(i++);

                // ModifiedBy column, int, not null
                ModifiedBy = reader.GetString(i++);

                TotalCount= reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["UploadedBy"].ToString(); }
                catch { missing.Append("UploadedBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["ModifiedDate"].ToString(); }
                catch { missing.Append("ModifiedDate "); }

                try { reader["ModifiedBy"].ToString(); }
                catch { missing.Append("ModifiedBy "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

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

        
        public void AttachmentRetrieveURLCount_ByObjectAndFileName_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Attachment> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                // Call the stored procedure to retrieve the data
                //processRow = new Database.SqlClient.ProcessRow<b_Attachment>(reader => { this.LoadFromDbForURLCount(reader); return this; });
                URLCount=Database.StoredProcedure.usp_Attachment_RetrieveURLCount_ByObjectAndFileName_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        #endregion

        #region V2-949
        public static b_Attachment ProcessRowForPOAttachmentPrint(SqlDataReader reader)
        {
            // Create instance of object
            b_Attachment obj = new b_Attachment();

            // Load the object from the database
            obj.LoadFromDatabaseForPOAttachmentPrint(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForPOAttachmentPrint(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ObjectId column, bigint, not null
                ObjectId = reader.GetInt64(i++);

                // AttachmentURL column, nvarchar(511), not null
                AttachmentURL = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ObjectId"].ToString(); }
                catch { missing.Append("ObjectId "); }

                try { reader["AttachmentURL"].ToString(); }
                catch { missing.Append("AttachmentURL "); }

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
        #endregion
    }
}
