using System;
using System.Text;
using System.Data.SqlClient;

namespace Database.Business
{
    [Serializable()]
    public class b_StoredProcValidationError
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_StoredProcValidationError()
        {
            TableName = string.Empty;
            ErrorCode = -1;
            Arg0 = string.Empty;
            Arg1 = string.Empty;
        }

        /// <summary>
        /// TableName property
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// ErrorCode property
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Arg0 property
        /// </summary>
        public string Arg0 { get; set; }

        /// <summary>
        /// Arg1 property
        /// </summary>
        public string Arg1 { get; set; }

        public static object ProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_StoredProcValidationError obj = new b_StoredProcValidationError();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Client object.
        /// This routine should be applied to the usp_Client_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Client_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // TableName column, nvarchar(127), not null
                TableName = reader.GetString(i++);

                // ErrorCode column, int, not null
                ErrorCode = reader.GetInt32(i++);

                // Arg0 column, nvarchar(255), not null
                Arg0 = reader.GetString(i++);

                // Arg1 column, nvarchar(255), not null
                Arg1 = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["TableName"].ToString(); }
                catch { missing.Append("TableName "); }

                try { reader["ErrorCode"].ToString(); }
                catch { missing.Append("ErrorCode "); }

                try { reader["Arg0"].ToString(); }
                catch { missing.Append("Arg0 "); }

                try { reader["Arg1"].ToString(); }
                catch { missing.Append("Arg1 "); }

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
