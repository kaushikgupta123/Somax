using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    partial class b_POLineUDF
    {
        #region Variables
        public string Select1Name { get; set; }
        public string Select2Name { get; set; }
        public string Select3Name { get; set; }
        public string Select4Name { get; set; }

        #region V2-946
        public long PurchaseOrderId { get; set; }
        public string Text1Label { get; set; }
        public string Text2Label { get; set; }
        public string Text3Label { get; set; }
        public string Text4Label { get; set; }
        public string Date1Label { get; set; }
        public string Date2Label { get; set; }
        public string Date3Label { get; set; }
        public string Date4Label { get; set; }
        public string Bit1Label { get; set; }
        public string Bit2Label { get; set; }
        public string Bit3Label { get; set; }
        public string Bit4Label { get; set; }
        public string Numeric1Label { get; set; }
        public string Numeric2Label { get; set; }
        public string Numeric3Label { get; set; }
        public string Numeric4Label { get; set; }
        public string Select1Label { get; set; }
        public string Select2Label { get; set; }
        public string Select3Label { get; set; }
        public string Select4Label { get; set; }
        #endregion
        #endregion

        #region Retreieve by PurchaseOrderLineItemId
        public static b_POLineUDF ProcessRowForRetrieveByPurchaseOrderLineItemId(SqlDataReader reader)
        {
            // Create instance of object
            b_POLineUDF obj = new b_POLineUDF();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveByPurchaseOrderLineItemId(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseForRetrieveByPurchaseOrderLineItemId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // POLineUDFId column, bigint, not null
                POLineUDFId = reader.GetInt64(i++);

                // PurchaseOrderLineItemId column, bigint, not null
                PurchaseOrderLineItemId = reader.GetInt64(i++);
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

                try { reader["POLineUDFId"].ToString(); }
                catch { missing.Append("POLineUDFId "); }

                try { reader["PurchaseOrderLineItemId"].ToString(); }
                catch { missing.Append("PurchaseOrderLineItemId "); }

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
        public void RetrieveByPurchaseOrderLineItemId(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref b_POLineUDF results
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

                results = Database.StoredProcedure.usp_POLineUDF_RetrieveByPurchaseOrderLineItemId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region deletion by PurchaseOrderLineItemId
        public void DeleteByPurchaseOrderLineItemId(
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
                Database.StoredProcedure.usp_POLineUDF_DeleteByPurchaseOrderLineItemId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #endregion
        public static b_POLineUDF ProcessRowForPurchaseOrderLineItemUDFPrint(SqlDataReader reader)
        {
            // Create instance of object
            b_POLineUDF obj = new b_POLineUDF();

            // Load the object from the database
            obj.LoadFromDatabaseForPurchaseOrderLineItemUDFPrint(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForPurchaseOrderLineItemUDFPrint(SqlDataReader reader)
        {
            //   int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {

                // PurchaseOrderLineItemId column, bigint, not null
                PurchaseOrderLineItemId = reader.GetInt64(i++);

                // PurchaseOrderId  column, bigint, not null
                PurchaseOrderId = reader.GetInt64(i++);

                Text1 = reader.GetString(i++);

                Text2 = reader.GetString(i++);

                Text3 = reader.GetString(i++);

                Text4 = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Date1 = reader.GetDateTime(i);
                }
                else
                {
                    Date1 = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Date2 = reader.GetDateTime(i);
                }
                else
                {
                    Date2 = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Date3 = reader.GetDateTime(i);
                }
                else
                {
                    Date3 = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Date4 = reader.GetDateTime(i);
                }
                else
                {
                    Date4 = DateTime.MinValue;
                }
                i++;

                Bit1 = reader.GetBoolean(i++);

                Bit2 = reader.GetBoolean(i++);

                Bit3 = reader.GetBoolean(i++);

                Bit4 = reader.GetBoolean(i++);

                Numeric1 = reader.GetDecimal(i++);

                Numeric2 = reader.GetDecimal(i++);

                Numeric3 = reader.GetDecimal(i++);

                Numeric4 = reader.GetDecimal(i++);

                Select1 = reader.GetString(i++);

                Select2 = reader.GetString(i++);

                Select3 = reader.GetString(i++);

                Select4 = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Text1Label = reader.GetString(i);
                }
                else
                {
                    Text1Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Text2Label = reader.GetString(i);
                }
                else
                {
                    Text2Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Text3Label = reader.GetString(i);
                }
                else
                {
                    Text3Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Text4Label = reader.GetString(i);
                }
                else
                {
                    Text4Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Date1Label = reader.GetString(i);
                }
                else
                {
                    Date1Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Date2Label = reader.GetString(i);
                }
                else
                {
                    Date2Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Date3Label = reader.GetString(i);
                }
                else
                {
                    Date3Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Date4Label = reader.GetString(i);
                }
                else
                {
                    Date4Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Bit1Label = reader.GetString(i);
                }
                else
                {
                    Bit1Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Bit2Label = reader.GetString(i);
                }
                else
                {
                    Bit2Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Bit3Label = reader.GetString(i);
                }
                else
                {
                    Bit3Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Bit4Label = reader.GetString(i);
                }
                else
                {
                    Bit4Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Numeric1Label = reader.GetString(i);
                }
                else
                {
                    Numeric1Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Numeric2Label = reader.GetString(i);
                }
                else
                {
                    Numeric2Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Numeric3Label = reader.GetString(i);
                }
                else
                {
                    Numeric3Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Numeric4Label = reader.GetString(i);
                }
                else
                {
                    Numeric4Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Select1Label = reader.GetString(i);
                }
                else
                {
                    Select1Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Select2Label = reader.GetString(i);
                }
                else
                {
                    Select2Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Select3Label = reader.GetString(i);
                }
                else
                {
                    Select3Label = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Select4Label = reader.GetString(i);
                }
                else
                {
                    Select4Label = "";
                }
                i++;



            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PurchaseOrderLineItemId"].ToString(); }
                catch { missing.Append("PurchaseOrderLineItemId "); }

                try { reader["PurchaseOrderId"].ToString(); }
                catch { missing.Append("PurchaseOrderId "); }

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

                try { reader["Text1Label"].ToString(); }
                catch { missing.Append("Text1Label "); }

                try { reader["Text2Label"].ToString(); }
                catch { missing.Append("Text2Label "); }

                try { reader["Text3Label"].ToString(); }
                catch { missing.Append("Text3Label "); }

                try { reader["Text4Label"].ToString(); }
                catch { missing.Append("Text4Label "); }

                try { reader["Date1Label"].ToString(); }
                catch { missing.Append("Date1Label "); }

                try { reader["Date2Label"].ToString(); }
                catch { missing.Append("Date2Label "); }

                try { reader["Date3Label"].ToString(); }
                catch { missing.Append("Date3Label "); }

                try { reader["Date4Label"].ToString(); }
                catch { missing.Append("Date4Label "); }

                try { reader["Bit1Label"].ToString(); }
                catch { missing.Append("Bit1Label "); }

                try { reader["Bit2Label"].ToString(); }
                catch { missing.Append("Bit2Label "); }

                try { reader["Bit3Label"].ToString(); }
                catch { missing.Append("Bit3Label "); }

                try { reader["Bit4Label"].ToString(); }
                catch { missing.Append("Bit4Label "); }

                try { reader["Numeric1Label"].ToString(); }
                catch { missing.Append("Numeric1Label "); }

                try { reader["Numeric2Label"].ToString(); }
                catch { missing.Append("Numeric2Label "); }

                try { reader["Numeric3Label"].ToString(); }
                catch { missing.Append("Numeric3Label "); }

                try { reader["Numeric4Label"].ToString(); }
                catch { missing.Append("Numeric4Label "); }

                try { reader["Select1Label"].ToString(); }
                catch { missing.Append("Select1Label "); }

                try { reader["Select2Label"].ToString(); }
                catch { missing.Append("Select2Label "); }

                try { reader["Select3Label"].ToString(); }
                catch { missing.Append("Select3Label "); }

                try { reader["Select4Label"].ToString(); }
                catch { missing.Append("Select4Label "); }

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
