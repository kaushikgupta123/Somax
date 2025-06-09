using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using DataContracts;
using InterfaceAPI.Models.Account;
using InterfaceAPI.Models.BBU.PartMasterRequestExport;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using Common.Constants;
using System.IO;
using System.Data;
using System.Reflection;

namespace InterfaceAPI.BusinessWrapper.Implementation.BBU
{
    public class PartMasterRequestExportWrapper : CommonWrapper, IPartMasterRequestExportWrapper
    {
        Int64 logID;
        public static string dirpath;
        public static int rowsProcessed;
        public PartMasterRequestExportWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Convert data row to model
        public List<PartMasterRequestExportModel> ConvertDataRowToModel(Int64 LogId)
        {
            List<PartMasterRequestExportModel> partMasterRequestExportModelList = new List<PartMasterRequestExportModel>();
            PartMasterRequest pmrExtract = new PartMasterRequest();
            pmrExtract.ClientId = _userData.DatabaseKey.Client.ClientId;
            pmrExtract.ExportLogId = LogId;

            List<PartMasterRequest> pmrExtractList = pmrExtract.Retrieve_PartMasterRequest_ExtractData(_userData.DatabaseKey);
            if (pmrExtractList.Count > 0)
            {
                foreach (var item in pmrExtractList)
                {
                    PartMasterRequestExportModel pmrExportModel = new PartMasterRequestExportModel();
                    pmrExportModel.ECO_REQUEST_NUM = item.PartMasterRequestId.ToString().Trim();
                    if (item.RequestType == Common.Constants.PartMasterRequestTypeConstants.ECO_SX_Replace)
                    {
                        pmrExportModel.REQUEST_TYPE = "ECO_SX_REPLACEMENT";
                    }
                    else
                    {
                        pmrExportModel.REQUEST_TYPE = item.RequestType.ToUpper();
                    }
                    pmrExportModel.ORACLE_PLANT_ID = item.ExOrganizationId;
                    pmrExportModel.SOMAX_PART_CLIENT_LOOKUP_ID = item.Part_ClientLookupId;
                    pmrExportModel.ORACLE_PART_NUM = item.PartMaster_ClientLookupId;
                    pmrExportModel.ORACLE_PART_ID = item.EXPartId.ToString().Trim();
                    pmrExportModel.ORACLE_REPLACE_PART_NUM = item.PartMasterClientLookUpId_Replace;
                    pmrExportModel.ORACLE_REPLACE_PART_ID = (item.EXPartId_Replace == 0 ? "" : item.EXPartId_Replace.ToString().Trim());
                    partMasterRequestExportModelList.Add(pmrExportModel);//==========
                }
            }
            return partMasterRequestExportModelList;
        }
        #endregion

        #region Convert To data file
        public string ConvertToDataFile(List<PartMasterRequestExportModel> ExportModelList, out int RowsExtracted)
        {
            string txt = string.Empty;
            string filePath = string.Empty;
            dirpath = System.Configuration.ConfigurationManager.AppSettings["LocalExportDirectory"].ToString().Trim()
                    + System.Configuration.ConfigurationManager.AppSettings[ApiConstants.PartMasterRequestExport].ToString().Trim();
            if (!Directory.Exists(dirpath))
            {
                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dirpath));
            }
            string cFilePrefix = System.Configuration.ConfigurationManager.AppSettings[ApiConstants.PartMasterRequestExport].ToString().Trim();     // SOMAXREQ
            // Per Request from BBU/Oracle
            string cFileExt = (SFTPCred.filesEncrypted == true) ? ".txt.pgp" :".txt";
            //string cFileExt = (SFTPCred.filesEncrypted == true) ? ".pgp" : ".txt";
            string cFileName = cFilePrefix + "-DAT-BBU" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + cFileExt;
            //string cFileName = cFilePrefix + "-DAT-BBU" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";
            // SOMAXREQ-DAT-BBUYYYY-MM-DD-HH-mm-ss.TXT 
            //string cOutFile = dirpath + "\\" + cFileName;
            string cOutFile =Path.Combine(dirpath, cFileName);
            TextWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(cOutFile));
            DataTable dt = ToDataTable(ExportModelList);//===========//SOM-1722
            //string delimiter = RetrieveInterfacePropValues().Delimiter;
            string delimiter = SFTPCred.delimiter;
            foreach (DataColumn column in dt.Columns)
            {
                //Add the Header row for Text file.
                if (column.ColumnName == "SOMAXECOREQ_DAT")
                    txt += column.ColumnName.Replace("_", "-").PadRight(15) + delimiter;
                else
                    txt += column.ColumnName + delimiter;
            }
            txt += "\n";
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    txt += row[column.ColumnName].ToString() + delimiter;
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
            string path = UploadPartMasterRequestExportToSFTP(inputFile, Path.GetFileName(inputFile), fileType);           
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