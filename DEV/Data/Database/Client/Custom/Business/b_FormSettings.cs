using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_FormSettings
    {
        public void RetrieveByClientId(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
  string callerUserName
    )
        {
            Database.SqlClient.ProcessRow<b_FormSettings> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_FormSettings>(reader => { this.LoadFromDatabaseRetrieveByClientId(reader); return this; });
                Database.StoredProcedure.usp_FormSettings_RetrieveByClientId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

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

        public int LoadFromDatabaseRetrieveByClientId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // FormSettingsId column, bigint, not null
                FormSettingsId = reader.GetInt64(i++);

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // WOLaborRecording column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    WOLaborRecording = reader.GetBoolean(i);
                }
                else
                {
                    WOLaborRecording = false;
                }
                i++;

                // WOUIC column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    WOUIC = reader.GetBoolean(i);
                }
                else
                {
                    WOUIC = false;
                }
                i++;

                // WOScheduling column, bit, not null               
                if (false == reader.IsDBNull(i))
                {
                    WOScheduling = reader.GetBoolean(i);
                }
                else
                {
                    WOScheduling = false;
                }
                i++;
                // WOSummary column, bit, not null               
                if (false == reader.IsDBNull(i))
                {
                    WOSummary = reader.GetBoolean(i);
                }
                else
                {
                    WOSummary = false;
                }
                i++;

                // WOPhotos column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    WOPhotos = reader.GetBoolean(i);
                }
                else
                {
                    WOPhotos = false;
                }
                i++;

                // WOComments column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    WOComments = reader.GetBoolean(i);
                }
                else
                {
                    WOComments = false;
                }
                i++;

                // PRUIC column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    PRUIC = reader.GetBoolean(i);
                }
                else
                {
                    PRUIC = false;
                }
                i++;

                // PRLine2 column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    PRLine2 = reader.GetBoolean(i);
                }
                else
                {
                    PRLine2 = false;
                }
                i++;

                // PRLIUIC column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    PRLIUIC = reader.GetBoolean(i);
                }
                else
                {
                    PRLIUIC = false;
                }
                i++;

                // PRComments column, bit, not null
                if (false == reader.IsDBNull(i))
                {
                    PRComments = reader.GetBoolean(i);
                }
                else
                {
                    PRComments = false;
                }
                i++;

                //V2-946
                // POUIC column, bit, not null
                POUIC = reader.GetBoolean(i++);

                // POLine2 column, bit, not null
                POLine2 = reader.GetBoolean(i++);

                // POLIUIC column, bit, not null
                POLIUIC = reader.GetBoolean(i++);

                // POComments column, bit, not null
                POComments = reader.GetBoolean(i++);

                // POTandC column, bit, not null
                POTandC = reader.GetBoolean(i++);

                // POTandCURL column, nvarchar(511), not null
                POTandCURL = reader.GetString(i++);

                //V2-947
                // PORHeader column, bit, not null
                PORHeader = reader.GetBoolean(i++);

                // PORLine2 column, bit, not null
                PORLine2 = reader.GetBoolean(i++);

                // PORPrint column, bit, not null
                PORPrint = reader.GetBoolean(i++);

                //V2-1011
                // PORUIC column, bit, not null
                PORUIC = reader.GetBoolean(i++);

                // PORComments column, bit, not null
                PORComments = reader.GetBoolean(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["FormSettingsId"].ToString(); }
                catch { missing.Append("FormSettingsId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WOLaborRecording"].ToString(); }
                catch { missing.Append("WOLaborRecording "); }

                try { reader["WOUIC"].ToString(); }
                catch { missing.Append("WOUIC "); }

                try { reader["WOScheduling"].ToString(); }
                catch { missing.Append("WOScheduling "); }

                try { reader["WOSummary"].ToString(); }
                catch { missing.Append("WOSummary "); }

                try { reader["WOPhotos"].ToString(); }
                catch { missing.Append("WOPhotos "); }

                try { reader["WOComments"].ToString(); }
                catch { missing.Append("WOComments "); }
                
                try { reader["PRUIC"].ToString(); }
                catch { missing.Append("PRUIC "); }

                try { reader["PRLine2"].ToString(); }
                catch { missing.Append("PRLine2 "); }

                try { reader["PRLIUIC"].ToString(); }
                catch { missing.Append("PRLIUIC "); }

                try { reader["PRComments"].ToString(); }
                catch { missing.Append("PRComments "); }
                //V2-946
                try { reader["POUIC"].ToString(); }
                catch { missing.Append("POUIC "); }

                try { reader["POLine2"].ToString(); }
                catch { missing.Append("POLine2 "); }

                try { reader["POLIUIC"].ToString(); }
                catch { missing.Append("POLIUIC "); }

                try { reader["POComments"].ToString(); }
                catch { missing.Append("POComments "); }

                try { reader["POTandC"].ToString(); }
                catch { missing.Append("POTandC "); }

                try { reader["POTandCURL"].ToString(); }
                catch { missing.Append("POTandCURL "); }

                //V2-947
                try { reader["PORHeader"].ToString(); }
                catch { missing.Append("PORHeader "); }

                try { reader["PORLine2"].ToString(); }
                catch { missing.Append("PORLine2 "); }

                try { reader["PORPrint"].ToString(); }
                catch { missing.Append("PORPrint "); }

                //V2-1011
                try { reader["PORUIC"].ToString(); }
                catch { missing.Append("PORUIC "); }

                try { reader["PORComments"].ToString(); }
                catch { missing.Append("PORComments "); }

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
