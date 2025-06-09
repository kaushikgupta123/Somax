using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_ServiceOrderLineItem
    {
        #region Properties
        public string Description { get; set; }
        public string Labor { get; set; }
        public string Materials { get; set; }
        public string Others { get; set; }
        public decimal Total { get; set; }
        public bool DeleteAllowed { get; set; }
        public bool LabourExists { get; set; }
        public bool PartExists { get; set; }
        public bool OthersExists { get; set; }
        public Int64 EquipmentId { get; set; }
        public string FIDescription { get; set; }
        public string Status { get; set; }
        #endregion

        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            //int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {

                //ClientId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ServiceOrderLineItemId = reader.GetInt64(i);
                }
                else
                {
                    ServiceOrderLineItemId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ServiceOrderId = reader.GetInt64(i);
                }
                else
                {
                    ServiceOrderId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FleetIssueId = reader.GetInt64(i);
                }
                else
                {
                    FleetIssueId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FIDescription = reader.GetString(i);
                }
                else
                {
                    FIDescription = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Labor = reader.GetString(i);
                }
                else
                {
                    Labor = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Materials = reader.GetString(i);
                }
                else
                {
                    Materials = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Others = reader.GetString(i);
                }
                else
                {
                    Others = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Total = reader.GetDecimal(i);
                }
                else
                {
                    Total = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    UpdateIndex = reader.GetInt32(i);
                }
                else
                {
                    UpdateIndex = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    RepairReason = reader.GetString(i);
                }
                else
                {
                    RepairReason = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Comment = reader.GetString(i);
                }
                else
                {
                    Comment = "";
                }

                i++;
                if (false == reader.IsDBNull(i))
                {
                    ServiceTaskId = reader.GetInt64(i);
                }
                else
                {
                    ServiceTaskId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    EquipmentId = reader.GetInt64(i);
                }
                else
                {
                    EquipmentId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SchedServiceId = reader.GetInt64(i);
                }
                else
                {
                    SchedServiceId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VMRSSystem = reader.GetString(i);
                }
                else
                {
                    VMRSSystem = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VMRSAssembly = reader.GetString(i);
                }
                else
                {
                    VMRSAssembly = "";
                }
                i++;
            }

            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ServiceOrderLineItemId"].ToString(); }
                catch { missing.Append("ServiceOrderLineItemId "); }

                try { reader["ServiceOrderId"].ToString(); }
                catch { missing.Append("ServiceOrderId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Labor"].ToString(); }
                catch { missing.Append("Labor "); }

                try { reader["Materials"].ToString(); }
                catch { missing.Append("Materials "); }

                try { reader["Others"].ToString(); }
                catch { missing.Append("Others "); }

                try { reader["Total"].ToString(); }
                catch { missing.Append("Total "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["RepairReason"].ToString(); }
                catch { missing.Append("RepairReason "); }

                try { reader["Comment"].ToString(); }
                catch { missing.Append("Comment "); }

                try { reader["ServiceTaskId"].ToString(); }
                catch { missing.Append("ServiceTaskId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["SchedServiceId"].ToString(); }
                catch { missing.Append("SchedServiceId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void ServiceOrderLineItem_RetrieveByServiceOrderId(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,

           ref List<b_ServiceOrderLineItem> serviceOrderLineItemList
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
                serviceOrderLineItemList = Database.StoredProcedure.usp_ServiceOrderLineItem_RetrieveByServiceOrderId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
                ClientId = 0;
            }
        }

        public void ValidateForDeleteLineItem(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref b_ServiceOrderLineItem objLineItem
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
                objLineItem = Database.StoredProcedure.usp_ServiceOrderLineItem_ValidateForDelete_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
                ClientId = 0;
            }
        }

        public  void ServiceOrderLineDeleteFromDatabaseCustom(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
     string callerUserName
       )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_ServiceOrderLineItem_DeleteByPKCustom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    }
}
