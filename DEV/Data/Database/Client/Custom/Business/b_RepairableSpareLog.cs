using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    public partial class b_RepairableSpareLog
    {
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string PersonnelName { get; set; }
        public string ParentClientLookupId { get; set; }
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public string AssignedClientLookupId { get; set; }
        public string TransactionDateS { get; set; }
        public void RetrieveByEquipmentId(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
       ref List<b_RepairableSpareLog> results
        )
        {
            Database.SqlClient.ProcessRow<b_RepairableSpareLog> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results= Database.StoredProcedure.usp_RepairableSpareLog_RetrieveByEquipmentId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);


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

        public static b_RepairableSpareLog ProcessRowForRepSpareAssetByEquipmentId(SqlDataReader reader)
        {
            b_RepairableSpareLog RepairableSpare = new b_RepairableSpareLog();

            RepairableSpare.LoadFromDatabaseRetriveByEquipmentId(reader);
            return RepairableSpare;
        }

        public int LoadFromDatabaseRetriveByEquipmentId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // RepairableSpareLogId column, bigint, not null
                RepairableSpareLogId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

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
                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // Location column, nvarchar(63), not null
                Location = reader.GetString(i++);

                // ParentId column, bigint, not null
                ParentId = reader.GetInt64(i++);

                // AssetGroup1 column, bigint, not null
                AssetGroup1 = reader.GetInt64(i++);

                // AssetGroup2 column, bigint, not null
                AssetGroup2 = reader.GetInt64(i++);

                // AssetGroup3 column, bigint, not null
                AssetGroup3 = reader.GetInt64(i++);

                // Assigned column, bigint, not null
                Assigned = reader.GetInt64(i++);

                PersonnelName = reader.GetString(i++);
                ParentClientLookupId = reader.GetString(i++);
                AssetGroup1Name = reader.GetString(i++);
                AssetGroup2Name = reader.GetString(i++);
                AssetGroup3Name = reader.GetString(i++);
                AssignedClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["RepairableSpareLogId"].ToString(); }
                catch { missing.Append("RepairableSpareLogId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["TransactionDate"].ToString(); }
                catch { missing.Append("TransactionDate "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["Location"].ToString(); }
                catch { missing.Append("Location "); }

                try { reader["ParentId"].ToString(); }
                catch { missing.Append("ParentId "); }

                try { reader["AssetGroup1"].ToString(); }
                catch { missing.Append("AssetGroup1 "); }

                try { reader["AssetGroup2"].ToString(); }
                catch { missing.Append("AssetGroup2 "); }

                try { reader["AssetGroup3"].ToString(); }
                catch { missing.Append("AssetGroup3 "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName "); }

                try { reader["ParentClientLookupId"].ToString(); }
                catch { missing.Append("ParentClientLookupId "); }

                try { reader["AssetGroup1Name"].ToString(); }
                catch { missing.Append("AssetGroup1Name "); }

                try { reader["AssetGroup2Name"].ToString(); }
                catch { missing.Append("AssetGroup2Name "); }

                try { reader["AssetGroup3Name"].ToString(); }
                catch { missing.Append("AssetGroup3Name "); }

                try { reader["AssignedClientLookupId"].ToString(); }
                catch { missing.Append("AssignedClientLookupId "); }



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
