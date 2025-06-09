using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using DataContracts;
using InterfaceAPI.Models.Account;
using InterfaceAPI.Models.BBU.PurchaseRequestExport;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using Common.Constants;
using System.IO;
using System.Data;
using System.Reflection;

namespace InterfaceAPI.BusinessWrapper.Implementation.BBU
{
    public class PurchaseRequestExportWrapper : CommonWrapper, IPurchaseRequestExportWrapper
    {
        Int64 logID;
        public static string dirpath;
        public static int rowsProcessed;
        public PurchaseRequestExportWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Convert data row to model
        public List<PurchaseRequestExportModel> ConvertDataRowToModel(Int64 LogId)
        {
            // Temporary
            DataContracts.ShoppingCart shoppingCart = new DataContracts.ShoppingCart();
            shoppingCart.ClientId = _userData.DatabaseKey.Client.ClientId;
            shoppingCart.ShoppingCart_ConvertToPurchaseRequest(_userData.DatabaseKey);
            OracleReceiptExtract OracleReceiptExtract = new OracleReceiptExtract();
            OracleReceiptExtract.ClientId = _userData.DatabaseKey.Client.ClientId;
            List<OracleReceiptExtract> ReceiptExtract = OracleReceiptExtract.OraclePurchaseRequestExtract_ExtractData(_userData.DatabaseKey);
            List<PurchaseRequestExportModel> PurchaseExportModelList = new List<PurchaseRequestExportModel>();
            if (ReceiptExtract.Count > 0)
            {
               
                foreach (var item in ReceiptExtract)
                {
                    PurchaseRequestExportModel purchaseExportModel = new PurchaseRequestExportModel();
                    purchaseExportModel.SOMAXRequisitionNumber = item.SOMAXRequisitionNumber;
                    purchaseExportModel.SOMAXRequisitionID = item.SOMAXRequisitionID;
                    purchaseExportModel.SOMAXRequisitionDescription = item.SOMAXRequisitionDescription;
                    purchaseExportModel.OracleVendorID = item.OracleVendorID;
                    purchaseExportModel.OracleVendorSiteId = item.OracleVendorSiteId;
                    purchaseExportModel.RequestedBy = item.RequestedBy;
                    purchaseExportModel.SOMAXRequisitionLineItemId = item.SOMAXRequisitionLineItemId;
                    purchaseExportModel.SOMAXRequisitionLineNumber = item.SOMAXRequisitionLineNumber;
                    purchaseExportModel.OraclePlantId = item.OraclePlantId;
                    purchaseExportModel.NeedByDate = item.NeedByDate.ToString("MM/dd/yyyy");
                    purchaseExportModel.OraclePartID = item.OraclePartID;
                    purchaseExportModel.OraclePartNumber = item.OraclePartNumber;
                    purchaseExportModel.OracleSourceDocumentId = item.OracleSourceDocumentId;
                    purchaseExportModel.OracleSourceDocumentNumber = item.OracleSourceDocumentNumber;
                    purchaseExportModel.OracleSourceDocumentLineId = item.OracleSourceDocumentLineId;
                    purchaseExportModel.SourceDocumentLineNumber = item.SourceDocumentLineNumber;
                    purchaseExportModel.UNSPSCCodeIDTree = item.UNSPSCCodeIDTree;
                    purchaseExportModel.LineDescription = item.LineDescription;
                    purchaseExportModel.PurchasingUOM = item.UOMPurchasing;
                    purchaseExportModel.UnitCost = item.UnitCost;
                    purchaseExportModel.OrderQuantity = item.Quantity;
                    purchaseExportModel.ExpenseAccount = item.ExpenseAccount;
                    PurchaseExportModelList.Add(purchaseExportModel);
                }
            }
            return PurchaseExportModelList;
        }
        #endregion

        #region Convert To data file
        public string ConvertToDataFile(List<PurchaseRequestExportModel> ExportModelList, out int RowsExtracted)
        {
            string txt = string.Empty;
            string filePath = string.Empty;
            dirpath = System.Configuration.ConfigurationManager.AppSettings["LocalExportDirectory"].ToString().Trim()
                    + System.Configuration.ConfigurationManager.AppSettings[ApiConstants.OraclePurchaseRequestExport].ToString().Trim();
            if (!Directory.Exists(dirpath))
            {
                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dirpath));
            }
            string cFilePrefix = System.Configuration.ConfigurationManager.AppSettings[ApiConstants.OraclePurchaseRequestExport].ToString().Trim();     // SOMAXREQ
            // Per Request from BBU 
            string cFileExt = (SFTPCred.filesEncrypted == true) ? ".txt.pgp" :".txt";
            //string cFileExt = (SFTPCred.filesEncrypted == true) ? ".pgp" :".txt";
            string cFileName = cFilePrefix + "-DAT-BBU" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + cFileExt;
            //string cFileName = cFilePrefix + "-DAT-BBU" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
            // SOMAXREQ-DAT-BBUYYYY-MM-DD-HH-mm-ss.TXT 
            //string cOutFile = dirpath + "\\" + cFileName;
            string cOutFile = Path.Combine(dirpath, cFileName);
            TextWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(cOutFile));
            DataTable dt = ToDataTable(ExportModelList);//===========//SOM-1722
            //string delimiter = RetrieveInterfacePropValues().Delimiter;
            string delimiter = SFTPCred.delimiter;
            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for Text file.
                if (column.ColumnName == "SOMAXREQ_DAT")
                    txt += column.ColumnName.Replace("_", "-").PadRight(15) + delimiter;
                else
                    txt += column.ColumnName + delimiter;
            }
            txt += "\n";
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                   
                    //Add the Data rows.
                    // Need to change zeros to blanks for the identified columns.
                    string colname = column.ColumnName.ToLower();
                    switch (colname)
                    {
                        case "sourcedocumentlinenumber":
                            if (Convert.ToInt32(row[column.ColumnName]) > 0)
                            {
                                txt += row[column.ColumnName].ToString() + delimiter;
                            }
                            else
                            {
                                txt += delimiter;
                            }
                            break;
                        case "oraclesourcedocumentid":
                        case "oraclesourcedocumentlineid":
                        case "oraclepartid":
                            if (Convert.ToInt64(row[column.ColumnName]) > 0)
                            {
                                txt += row[column.ColumnName].ToString() + delimiter;
                            }
                            else
                            {
                                txt += delimiter;
                            }
                            break;
                        default:
                            txt += row[column.ColumnName].ToString() + delimiter;
                            break;
                    }
                }
                txt += "\n";
            }
            txt += "999".PadLeft(15);
            txt += "\n";
            sw.Write(txt);
            sw.Close();
            rowsProcessed = dt.Rows.Count;
            RowsExtracted = rowsProcessed;
            dt.Clear();
            dt.Dispose();
            return cOutFile;
        }

        public string ExportToSFTP(string inputFile, FileTypeEnum fileType)
        {
            string path = UploadPurchaseRequestExportToSFTP(inputFile, Path.GetFileName(inputFile), fileType);
            return path;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        #endregion
        public long ExportLog(string processName, Int64 logId, string msg, int rowsExtracted)
        {
            logID = CreateUpdateExportLog(processName, logId, msg, rowsExtracted);
            return logID;
        }

    }
}