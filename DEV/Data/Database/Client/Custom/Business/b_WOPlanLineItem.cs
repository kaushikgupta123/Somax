using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_WOPlanLineItem
    {
        #region Property
        public string WorkOrderClientLookupId { get; set; }
        public string Description { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Status { get; set; }
        public DateTime? CompleteDate { get; set; }

        #endregion

        #region WOPlanLineItem Retrive By Work Order Plan Id
        public static b_WOPlanLineItem ProcessRowForWOPlanLineItemRetriveByWorkOrderPlanId(SqlDataReader reader)
        {
            // Create instance of object
            b_WOPlanLineItem WOPL = new b_WOPlanLineItem();
            WOPL.LoadFromDatabaseForWOPlanLineItemRetriveByWorkOrderPlanId(reader);
            return WOPL;
        }

        public int LoadFromDatabaseForWOPlanLineItemRetriveByWorkOrderPlanId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderClientLookupId column, nvarchar(15), not null             
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrderClientLookupId = "";
                }
                i++;
                // Description column, string, not null               
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;
                // StartDate column, datetime2, not null             
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;
                            
                if (false == reader.IsDBNull(i))
                {
                    EquipmentClientLookupId = reader.GetString(i);
                }
                else
                {
                    EquipmentClientLookupId = "";
                }
                i++;
                // ChargeTo_Name column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i);
                }
                else
                {
                    ChargeTo_Name = "";
                }
                i++;
                // Status column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;
                // CompleteDate column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                // Type column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["WorkOrderClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["EquipmentClientLookupId"].ToString(); }
                catch { missing.Append("EquipmentClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

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

        public void WOPlanLineItem_RetriveByWorkOrderPlanId(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WOPlanLineItem> results
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

                results = Database.StoredProcedure.usp_WOPlanLineItem_RetrieveByWorkOrderPlanId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
