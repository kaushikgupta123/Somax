using System;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    public partial class b_ProjectUDF
    {
        #region Variables
        public string Select1Name { get; set; }
        public string Select2Name { get; set; }
        public string Select3Name { get; set; }
        public string Select4Name { get; set; }
        #endregion 

        #region Retreieve by ProjectId
        public static b_ProjectUDF ProcessRowForRetrieveByProjectId(SqlDataReader reader)
        {
            // Create instance of object
            b_ProjectUDF obj = new b_ProjectUDF();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveByProjectId(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseForRetrieveByProjectId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // ProjectUDFId column, bigint, not null
                ProjectUDFId = reader.GetInt64(i++);

                // ProjectId column, bigint, not null
                ProjectId = reader.GetInt64(i++);

                // Text1 column, nvarchar(67), not null
                Text1 = reader.GetString(i++);

                // Text2 column, nvarchar(67), not null
                Text2 = reader.GetString(i++);

                // Text3 column, nvarchar(67), not null
                Text3 = reader.GetString(i++);

                // Text4 column, nvarchar(67), not null
                Text4 = reader.GetString(i++);

                // Date1 column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Date1 = reader.GetDateTime(i);
                }
                else
                {
                    Date1 = DateTime.MinValue;
                }
                i++;
                // Date2 column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Date2 = reader.GetDateTime(i);
                }
                else
                {
                    Date2 = DateTime.MinValue;
                }
                i++;
                // Date3 column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Date3 = reader.GetDateTime(i);
                }
                else
                {
                    Date3 = DateTime.MinValue;
                }
                i++;
                // Date4 column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Date4 = reader.GetDateTime(i);
                }
                else
                {
                    Date4 = DateTime.MinValue;
                }
                i++;
                // Bit1 column, bit, not null
                Bit1 = reader.GetBoolean(i++);

                // Bit2 column, bit, not null
                Bit2 = reader.GetBoolean(i++);

                // Bit3 column, bit, not null
                Bit3 = reader.GetBoolean(i++);

                // Bit4 column, bit, not null
                Bit4 = reader.GetBoolean(i++);

                // Numeric1 column, decimal(9,2), not null
                Numeric1 = reader.GetDecimal(i++);

                // Numeric2 column, decimal(9,2), not null
                Numeric2 = reader.GetDecimal(i++);

                // Numeric3 column, decimal(9,2), not null
                Numeric3 = reader.GetDecimal(i++);

                // Numeric4 column, decimal(9,2), not null
                Numeric4 = reader.GetDecimal(i++);

                // Select1 column, nvarchar(15), not null
                Select1 = reader.GetString(i++);

                // Select2 column, nvarchar(15), not null
                Select2 = reader.GetString(i++);

                // Select3 column, nvarchar(15), not null
                Select3 = reader.GetString(i++);

                // Select4 column, nvarchar(15), not null
                Select4 = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Select1Name = reader.GetString(i);
                }
                else
                {
                    Select1Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Select2Name = reader.GetString(i);
                }
                else
                {
                    Select2Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Select3Name = reader.GetString(i);
                }
                else
                {
                    Select3Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Select4Name = reader.GetString(i);
                }
                else
                {
                    Select4Name = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ProjectUDFId"].ToString(); }
                catch { missing.Append("ProjectUDFId "); }

                try { reader["ProjectId"].ToString(); }
                catch { missing.Append("ProjectId "); }

                try { reader["Text1"].ToString(); }
                catch { missing.Append("Text1 "); }

                try { reader["Text2"].ToString(); }
                catch { missing.Append("Text2 "); }

                try { reader["Text3"].ToString(); }
                catch { missing.Append("Text3 "); }

                try { reader["Text4"].ToString(); }
                catch { missing.Append("Text4 "); }

                try { reader["Date1"].ToString(); }
                catch { missing.Append("Date1 "); }

                try { reader["Date2"].ToString(); }
                catch { missing.Append("Date2 "); }

                try { reader["Date3"].ToString(); }
                catch { missing.Append("Date3 "); }

                try { reader["Date4"].ToString(); }
                catch { missing.Append("Date4 "); }

                try { reader["Bit1"].ToString(); }
                catch { missing.Append("Bit1 "); }

                try { reader["Bit2"].ToString(); }
                catch { missing.Append("Bit2 "); }

                try { reader["Bit3"].ToString(); }
                catch { missing.Append("Bit3 "); }

                try { reader["Bit4"].ToString(); }
                catch { missing.Append("Bit4 "); }

                try { reader["Numeric1"].ToString(); }
                catch { missing.Append("Numeric1 "); }

                try { reader["Numeric2"].ToString(); }
                catch { missing.Append("Numeric2 "); }

                try { reader["Numeric3"].ToString(); }
                catch { missing.Append("Numeric3 "); }

                try { reader["Numeric4"].ToString(); }
                catch { missing.Append("Numeric4 "); }

                try { reader["Select1"].ToString(); }
                catch { missing.Append("Select1 "); }

                try { reader["Select2"].ToString(); }
                catch { missing.Append("Select2 "); }

                try { reader["Select3"].ToString(); }
                catch { missing.Append("Select3 "); }

                try { reader["Select4"].ToString(); }
                catch { missing.Append("Select4 "); }

                try { reader["Select1Name"].ToString(); }
                catch { missing.Append("Select1Name "); }

                try { reader["Select2Name"].ToString(); }
                catch { missing.Append("Select2Name "); }

                try { reader["Select3Name"].ToString(); }
                catch { missing.Append("Select3Name "); }

                try { reader["Select4Name"].ToString(); }
                catch { missing.Append("Select4Name "); }


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

        public void RetrieveByProjectId(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref b_ProjectUDF results
     )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_ProjectUDF_RetrieveByProjectId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #endregion
    }
}
