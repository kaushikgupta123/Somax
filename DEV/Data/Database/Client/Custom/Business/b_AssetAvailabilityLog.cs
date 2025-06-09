using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    public partial class b_AssetAvailabilityLog
    {

        public string PersonnelName { get; set; }

        public static object ProcessRowAssetAvailabilityLog_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_AssetAvailabilityLog obj = new b_AssetAvailabilityLog();

            // Load the object from the database
            obj.LoadFromDatabaseForAssetAvailabilityLog_V2(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseForAssetAvailabilityLog_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {                      

                // ObjectId column, bigint, not null
                ObjectId = reader.GetInt64(i++);

                // TransactionDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    TransactionDate = reader.GetDateTime(i);
                }
                else
                {
                    TransactionDate = DateTime.MinValue;
                }
                i++;
                // Event column, nvarchar(15), not null
                Event = reader.GetString(i++);              

                // ReturnToService column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ReturnToService = reader.GetDateTime(i);
                }
                else
                {
                    ReturnToService = DateTime.MinValue;
                }
                i++;              
                // Reason column, nvarchar(MAX), not null
                Reason = reader.GetString(i++);              

                // ReasonCode column, nvarchar(15), not null
                ReasonCode = reader.GetString(i++);

                // PersonnelName column,nvarchar(70), not null
                PersonnelName = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();          

               try { reader["ObjectId"].ToString(); }
                catch { missing.Append("ObjectId "); }

                try { reader["TransactionDate"].ToString(); }
                catch { missing.Append("TransactionDate "); }

                try { reader["Event"].ToString(); }
                catch { missing.Append("Event "); }

                try { reader["ReturnToService"].ToString(); }
                catch { missing.Append("ReturnToService "); }
             
                try { reader["Reason"].ToString(); }
                catch { missing.Append("Reason "); }              

                try { reader["ReasonCode"].ToString(); }
                catch { missing.Append("ReasonCode "); }

                try { reader["ReasonCode"].ToString(); }
                catch { missing.Append("ReasonCode "); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName "); }

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
