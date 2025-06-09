using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.Client.Custom.Business
{
  public partial class b_OracleReceiptExtract
  {

    public long ClientId { get; set; }
    public long SiteId { get; set; }
    public string ClientLookupId { get; set; }
    public Int64 PurchaseRequestId { get; set; }
    public string Reason { get; set; }
    public Int64 ExVendorId { get; set; }
    public Int64 ExVendorSiteId { get; set; }
    public string ExOracleUserId { get; set; }
    public Int64 PurchaseRequestLineItemId { get; set; }
    public int PurchaseRequestLineNumber { get; set; }
    public string ExOrganizationId { get; set; }
    public DateTime RequiredDate { get; set; }
    public Int64 ExPartId { get; set; }
    public string PartMasterClientLookupId { get; set; }
    public long ExVendorCatalogSourceId { get; set; } // rkl 2021-09-30
    public string ExSourceDocument { get; set; }
    public long ExVendorCatalogItemSourceId { get; set; } // rkl 2021-09-30
    public int VendorCatalogLineNummber { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public decimal OrderQuantity { get; set; }
    public string PurchaseUOM { get; set; }
    public decimal UnitCost { get; set; }
    public string ExpenseAccount { get; set; }

    public string RequestedBy { get; set; }
    public string OraclePlantId { get; set; }
    public DateTime NeedByDate { get; set; }
    public string OraclePartNumber { get; set; }
    public int OracleSourceDocumentLineId { get; set; }
    public int LoadFromDatabase(SqlDataReader reader)
    {
      int i = 0;
      try
      {
        ClientId = reader.GetInt64(i++);
        SiteId = reader.GetInt64(i++);
        ClientLookupId = reader.GetString(i++);
        PurchaseRequestId = reader.GetInt64(i++);
        Reason = reader.GetString(i++);
        ExVendorId = reader.GetInt64(i++);
        ExVendorSiteId = reader.GetInt64(i++);
        if (false == reader.IsDBNull(i))
        {
          ExOracleUserId = reader.GetString(i);
        }
        else
        {
          ExOracleUserId = string.Empty;
        }
        i++;
        PurchaseRequestLineItemId = reader.GetInt64(i++);
        PurchaseRequestLineNumber = reader.GetInt32(i++);
        ExOrganizationId = reader.GetString(i++);
        if (false == reader.IsDBNull(i))
        {
          RequiredDate = reader.GetDateTime(i);
        }
        else
        {
          RequiredDate = DateTime.MinValue;
        }
        i++;
        ExPartId = reader.GetInt64(i++);
        PartMasterClientLookupId = reader.GetString(i++);
        ExVendorCatalogSourceId = reader.GetInt64(i++);  // RKL 2021-09-30
        ExSourceDocument = reader.GetString(i++);
        ExVendorCatalogItemSourceId = reader.GetInt64(i++); // RKL 2021-09-30
        VendorCatalogLineNummber = reader.GetInt32(i++);
        Category = reader.GetString(i++);
        Description = reader.GetString(i++);
        OrderQuantity = reader.GetDecimal(i++);
        PurchaseUOM = reader.GetString(i++);
        UnitCost = reader.GetDecimal(i++);
        ExpenseAccount = reader.GetString(i++);
      }
      catch (Exception ex)
      {
        // Diagnostics
        StringBuilder missing = new StringBuilder();

        try { reader["ClientId"].ToString(); }
        catch { missing.Append("ClientId "); }

        try { reader["SiteId"].ToString(); }
        catch { missing.Append("SiteId "); }

        try { reader["ClientLookupId"].ToString(); }
        catch { missing.Append("ClientLookupId "); }

        try { reader["Reason"].ToString(); }
        catch { missing.Append("Reason "); }

        try { reader["ExVendorId"].ToString(); }
        catch { missing.Append("ExVendorId "); }

        try { reader["ExVendorSiteId"].ToString(); }
        catch { missing.Append("ExVendorSiteId "); }

        try { reader["ExOracleUserId"].ToString(); }
        catch { missing.Append("ExOracleUserId "); }

        try { reader["PurchaseRequestLineItemId"].ToString(); }
        catch { missing.Append("PurchaseRequestLineItemId "); }

        try { reader["PurchaseRequestLineNumber"].ToString(); }
        catch { missing.Append("PurchaseRequestLineNumber "); }

        try { reader["ExOrganizationId"].ToString(); }
        catch { missing.Append("ExOrganizationId "); }

        try { reader["RequiredDate"].ToString(); }
        catch { missing.Append("RequiredDate "); }

        try { reader["ExPartId"].ToString(); }
        catch { missing.Append("ExPartId "); }

        try { reader["PartMasterClientLookupId"].ToString(); }
        catch { missing.Append("PartMasterClientLookupId "); }

        try { reader["ExVendorCatalogSourceId"].ToString(); }
        catch { missing.Append("ExVendorCatalogSourceId "); }

        try { reader["ExSourceDocument"].ToString(); }
        catch { missing.Append("ExSourceDocument "); }
        try { reader["ExVendorCatalogItemSourceId"].ToString(); }
        catch { missing.Append("ExVendorCatalogItemSourceId "); }

        try { reader["VendorCatalogLineNummber"].ToString(); }
        catch { missing.Append("VendorCatalogLineNummber "); }

        try { reader["Category"].ToString(); }
        catch { missing.Append("Category "); }

        try { reader["Description"].ToString(); }
        catch { missing.Append("Description "); }

        try { reader["OrderQuantity"].ToString(); }
        catch { missing.Append("OrderQuantity "); }

        try { reader["PurchaseUOM"].ToString(); }
        catch { missing.Append("PurchaseUOM "); }
  
        try { reader["UnitCost"].ToString(); }
        catch { missing.Append("UnitCost "); }

        try { reader["ExpenseAccount"].ToString(); }
        catch { missing.Append("ExpenseAccount "); }

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
    public void RetrieveAllOraclePurchaseRequestForExport(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_OracleReceiptExtract> data,
      long ClientId
)
    {
      Database.SqlClient.ProcessRow<b_OracleReceiptExtract> processRow = null;
      List<b_OracleReceiptExtract> results = null;
      SqlCommand command = null;
      string message = String.Empty;

      // Initialize the results
      data = new List<b_OracleReceiptExtract>();

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_OracleReceiptExtract>(reader => { b_OracleReceiptExtract obj = new b_OracleReceiptExtract(); obj.LoadFromDatabase(reader); return obj; });
        results = Database.StoredProcedure.usp_OraclePurchaseRequestExtract_ExtractData.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this, ClientId);

        // Extract the results
        if (null != results)
        {
          data = results;
        }
        else
        {
          data = new List<b_OracleReceiptExtract>();
        }

        // Clear the results collection
        //if (null != results)
        //{
        //    results.Clear();
        //    results = null;
        //}
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
  }
}
