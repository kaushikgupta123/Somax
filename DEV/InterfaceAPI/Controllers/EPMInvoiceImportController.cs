using System;
using System.Collections.Generic;
using System.Web;
using InterfaceAPI.BusinessWrapper.Interface;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Http;
using Common.Constants;
using InterfaceAPI.Models.Common;
using InterfaceAPI.CommonUtility;
using InterfaceAPI.BusinessWrapper.Implementation;
using InterfaceAPI.Models.MonnitIoTReading;
using InterfaceAPI.Models.EPMInvoiceImport;
using DevExpress.Xpo.Logger.Transport;
using Renci.SshNet;
using System.Linq;
using System.Configuration;
using Client.Controllers.Devices;

namespace InterfaceAPI.Controllers
{
    public class EPMInvoiceImportController : BaseApiController
    {
        readonly IImportCSVWrapper _ImportCSVWrapper;
        readonly IEPMInvoiceImportWrapper _EPMInvoiceImportWrapper;
        public EPMInvoiceImportController(IImportCSVWrapper ImportCSVWrapper, IEPMInvoiceImportWrapper EPMInvoiceImportWrapper)
        {
            _ImportCSVWrapper = ImportCSVWrapper;
            _EPMInvoiceImportWrapper = EPMInvoiceImportWrapper;
        }
        public long logID { get; set; }
        [HttpPost]
        public HttpResponseMessage EPMInvoiceImport([FromBody] SchedulerAPICredentials credentials)
        {
            #region Authentication
            if (!_EPMInvoiceImportWrapper.CheckAuthentication(credentials.UserName, credentials.Password))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ErrorMessageConstants.Authentication_Failed);
            }
            if (_EPMInvoiceImportWrapper.IsMaintenanceGoingOn())
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ErrorMessageConstants.ServiceUnavailable);
            }
            #endregion

            #region Initiate SFTP Values
            _EPMInvoiceImportWrapper.RetrieveInterfacePropValues(ApiConstants.EPMInvoiceImport);
            #endregion

            #region Interface Setup Properties Check 
            if (!_EPMInvoiceImportWrapper.CheckIsActiveInterface(ApiConstants.EPMInvoiceImport))
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ErrorMessageConstants.The_Process_Is_Not_Enabled_For_The_Client);
            }
            #endregion

            #region Retrieve Interface Prop values for SFTP File Download
            var sftpDirPath = SFTPCred.ftpFileDirectory;
            var sftpArchiveDirPath = SFTPCred.ftpArcDirectory;
            string result = string.Empty;
            string errCsv = string.Empty;
            var fileList = _EPMInvoiceImportWrapper.GetFileNames(sftpDirPath);
            #endregion

            #region Read files fron SFTP
            EPMInvoiceImportResponseModel ePMInvoiceImportResponseModel;
            List<EPMInvoiceImportResponseModel> ePMInvoiceImportResponseModelList = new List<EPMInvoiceImportResponseModel>();
            if (fileList != null && fileList.Count == 0)
            {
                ePMInvoiceImportResponseModel = new EPMInvoiceImportResponseModel();
                ePMInvoiceImportResponseModel.status = StatusEnum.failed.ToString();
                ePMInvoiceImportResponseModel.errMessage = ErrorMessageConstants.No_file_found_source_directory;
                ePMInvoiceImportResponseModelList.Add(ePMInvoiceImportResponseModel);
                var respFile = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(ePMInvoiceImportResponseModelList),
                                             System.Text.Encoding.UTF8, "application/json")
                };
                return respFile;
            }

            //---Get json from csv---
            foreach (var fileName in fileList)
            {
                string thisExtension = Utility.GetFileExtension(fileName);
                if (!(thisExtension.Contains(FileExtensionEnum.txt.ToString()) || thisExtension.Contains(FileExtensionEnum.csv.ToString())))
                {
                    ePMInvoiceImportResponseModel = new EPMInvoiceImportResponseModel();
                    ePMInvoiceImportResponseModel.fileName = fileName;
                    ePMInvoiceImportResponseModel.status = StatusEnum.failed.ToString();
                    ePMInvoiceImportResponseModel.errMessage = "File type " + thisExtension + " is not supported.";
                    ePMInvoiceImportResponseModelList.Add(ePMInvoiceImportResponseModel);
                }
                else
                {
                    var EPMInvoiceImport = ParseInvoiceFromSFTPFile(sftpDirPath, fileName, FileTypeEnum.EPMInvoiceImport);
                  
                    result = JsonConvert.SerializeObject(EPMInvoiceImport, Formatting.Indented);

                    if (result.Equals(StatusEnum.ErrorConvertingJson.ToString()))
                    {
                        ePMInvoiceImportResponseModel = new EPMInvoiceImportResponseModel();
                        ePMInvoiceImportResponseModel.fileName = fileName;
                        ePMInvoiceImportResponseModel.status = StatusEnum.failed.ToString();
                        ePMInvoiceImportResponseModel.errMessage = ErrorMessageConstants.File_Read_failure;
                        ePMInvoiceImportResponseModelList.Add(ePMInvoiceImportResponseModel);
                        continue;
                    }
                    //--- serialize JSON directly to a file
                    EPMInvoiceImportModel aList = new EPMInvoiceImportModel();
                    aList = JsonConvert.DeserializeObject<EPMInvoiceImportModel>(result.ToString());
                    string firstName = Utility.GetFileName(fileName);
                    string jsonPath = HttpContext.Current.Server.MapPath("~/JsonSchema/EPMInvoiceImport/" + firstName + ".json");
                    // Generate schema based on the data model
                    JsonSchemaGenerator generator = new JsonSchemaGenerator();
                    JsonSchema schema = generator.Generate(typeof(EPMInvoiceImportModel));
                    using (StreamWriter file = File.CreateText(jsonPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, aList);
                    }
                    //---compare the JSON with schema
                    JsonSchema jschema = new JsonSchema();

                    String schemaPath = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["EPMInvoiceImportSchemaPath"]);
                    ////---check if schema file exists. If not return relevent response---
                    if (!File.Exists(schemaPath))
                    {
                        ePMInvoiceImportResponseModel = new EPMInvoiceImportResponseModel();
                        ePMInvoiceImportResponseModel.fileName = fileName;
                        ePMInvoiceImportResponseModel.status = StatusEnum.failed.ToString();
                        ePMInvoiceImportResponseModel.errMessage = ErrorMessageConstants.Refence_schema_requred_json_validation_not_found;
                        ePMInvoiceImportResponseModelList.Add(ePMInvoiceImportResponseModel);
                        var respFile = new HttpResponseMessage
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(ePMInvoiceImportResponseModelList),
                                            System.Text.Encoding.UTF8, "application/json")
                        };
                        return respFile;
                    }
                    jschema = JsonSchema.Parse(File.ReadAllText(schemaPath));
                    JObject jo = new JObject();
                    JObject ja = JObject.Parse(File.ReadAllText(jsonPath));
                    bool schemaMatch = false;
                    string archiveErr = string.Empty;

                    
                    schemaMatch = IsSchemasEqual(jschema,schema);

                    
                        if (schemaMatch == false)
                        {
                            ePMInvoiceImportResponseModel = new EPMInvoiceImportResponseModel();
                            ePMInvoiceImportResponseModel.fileName = fileName;
                            ePMInvoiceImportResponseModel.status = StatusEnum.failed.ToString();
                            ePMInvoiceImportResponseModel.errMessage = ErrorMessageConstants.Json_file_not_match_required_schema;
                            ePMInvoiceImportResponseModelList.Add(ePMInvoiceImportResponseModel);
                            var respJson = new HttpResponseMessage
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(ePMInvoiceImportResponseModelList),
                                              System.Text.Encoding.UTF8, "application/json")
                            };
                            return respJson;
                        }
                   

                    archiveErr = _EPMInvoiceImportWrapper.ArchiveFileWithDBvalue(sftpDirPath, fileName, sftpArchiveDirPath);
                    if (String.IsNullOrEmpty(archiveErr))
                    {
                        ePMInvoiceImportResponseModel = new EPMInvoiceImportResponseModel();
                        ePMInvoiceImportResponseModel.fileName = fileName;
                        ePMInvoiceImportResponseModel.status = StatusEnum.success.ToString();
                        ePMInvoiceImportResponseModel.errMessage = string.Empty;
                        //---process json
                        var respMessage = ProcessLocalEPMInvoiceImportJsonFile(EPMInvoiceImport);
                        ePMInvoiceImportResponseModel.EPMInvoiceImportResponseModelList = respMessage;
                        ePMInvoiceImportResponseModelList.Add(ePMInvoiceImportResponseModel);
                    }
                    else
                    {
                        ePMInvoiceImportResponseModel = new EPMInvoiceImportResponseModel();
                        ePMInvoiceImportResponseModel.fileName = fileName;
                        ePMInvoiceImportResponseModel.status = StatusEnum.failed.ToString();
                        ePMInvoiceImportResponseModel.errMessage = archiveErr;
                        ePMInvoiceImportResponseModelList.Add(ePMInvoiceImportResponseModel);
                    }
                }
            }
            #region Send Mail
            if (SFTPCred.isMailSend == true)
            {
                _EPMInvoiceImportWrapper.SendEMail(credentials.UserName, (JsonConvert.SerializeObject(ePMInvoiceImportResponseModelList)));
            }

            #endregion
            var resp = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(ePMInvoiceImportResponseModelList),
                                             System.Text.Encoding.UTF8, "application/json")
            };
            return resp;
            #endregion
        }


        private bool IsSchemasEqual(JsonSchema schema1, JsonSchema schema2)
        {
            // Check for reference equality
            if (ReferenceEquals(schema1, schema2))
            {
                return true;
            }

            // If either is null, they are not equal
            if (schema1 == null || schema2 == null)
            {
                return false;
            }

            // Convert schemas to JSON for comparison
            var json1 = JObject.FromObject(schema1);
            var json2 = JObject.FromObject(schema2);

            // Compare the two JSON objects
            return JToken.DeepEquals(json1, json2);
        }
        private List<EPMInvoiceImportResponseModel> ProcessLocalEPMInvoiceImportJsonFile(EPMInvoiceImportModel ePMInvoiceImport)
        {
            String FilePath = HttpContext.Current.Server.MapPath("~/JsonSchema/EPMInvoiceImport/");

            string[] file = Directory.GetFiles(FilePath, "*.json");
            List<EPMInvoiceImportResponseModel> vmrList = new List<EPMInvoiceImportResponseModel>();
            EPMInvoiceImportResponseModel ePMInvoiceImportResponseModel = new EPMInvoiceImportResponseModel();
           
                if (ePMInvoiceImport == null)
                {
                    ePMInvoiceImportResponseModel.status = StatusEnum.failed.ToString();
                    ePMInvoiceImportResponseModel.errMessage =ErrorMessageConstants.Data_Is_null;
                    vmrList.Add(ePMInvoiceImportResponseModel);
                    return (vmrList);
                }
                #region ePMInvoiceImport objects converted to data tables
                var result = _EPMInvoiceImportWrapper.CreateEPMInvoiceImport(ePMInvoiceImport);
                #endregion

                #region create log
                var logid = _EPMInvoiceImportWrapper.CreateLog(ApiConstants.EPMInvoiceImport);
                #endregion

                #region validate validation & process(import)
                var vResult = _EPMInvoiceImportWrapper.EPMInvoiceImportValidate(logid);
                #endregion

                if (vResult != null && vResult.Count > 0)
                {
                    ePMInvoiceImportResponseModel.status = StatusEnum.failed.ToString();
                    foreach (var vr in vResult)
                    {
                        ePMInvoiceImportResponseModel.errMessageList.Add(vr);
                    }
                }
                else
                {
                    ePMInvoiceImportResponseModel.status = StatusEnum.success.ToString();
                    ePMInvoiceImportResponseModel.errMessage = "";
                }
                vmrList.Add(ePMInvoiceImportResponseModel);
           
            //delete all json files from local directory
            _EPMInvoiceImportWrapper.DeleteJsonFiles("EPMInvoiceImport");
            return vmrList;
        }
        #region  Parse the model
        private EPMInvoiceImportModel ParseInvoiceFromSFTPFile(string directoryPath, string fileName, FileTypeEnum fileType)
        {
            var invoiceModel = new EPMInvoiceImportModel();
            string fileExtension = Path.GetExtension(fileName);
            string Host = ConfigurationManager.AppSettings["SFTPHost"];
            string ftpUserId = ConfigurationManager.AppSettings["SFTPuserid"];
            string ftpPassWord = ConfigurationManager.AppSettings["SFTPpassword"];
            try
            {
                using (SftpClient sftp = new SftpClient(Host, ftpUserId, ftpPassWord))
                {
                    sftp.Connect();
                    Stream fileStream = sftp.OpenRead(Path.Combine(directoryPath, fileName));
                    using (var sr = new StreamReader(fileStream, true))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] dataFields = line.Split('|');
                            
                            switch (dataFields[0])
                            {
                                // Header Section (H)
                                case "H":
                                    BindHeaderRecord(invoiceModel, dataFields);

                                    break;

                                // Header Comment Section (HC)
                                case "HC":
                                    
                                    var headerComment = new HeaderComment
                                    {
                                        PONumber = invoiceModel.PONumber,
                                        Notes = dataFields.Length > 1 ? dataFields[1] : string.Empty
                                    };
                                    invoiceModel.HeaderComment.Add(headerComment);
                                    break;

                                // Item Section (I)
                                case "I":
                                   
                                    var item = new Item
                                    {
                                        PONumber = invoiceModel.PONumber,
                                        LineNumber = dataFields.Length > 1 ? int.Parse(dataFields[1]) : 0,
                                        Quantity = dataFields.Length > 2 ? decimal.Parse(dataFields[2]) : 0,
                                        UnitOfMeasurement = dataFields.Length > 3 ? dataFields[3] : string.Empty,
                                        UnitPrice = dataFields.Length > 4 ? decimal.Parse(dataFields[4]) : 0,
                                        BuyerPartNumber = dataFields.Length > 5 ? dataFields[5] : string.Empty,
                                        VendorPartNumber = dataFields.Length > 6 ? dataFields[6] : string.Empty,
                                       
                                    };
                                    invoiceModel.Items.Add(item);
                                    break;

                                // Item Comment Section (IC)
                                case "IC":
                                  
                                    var itemComment = new ItemComment
                                    {
                                        PONumber = invoiceModel.PONumber,
                                        Description = dataFields.Length > 1 ? dataFields[1] : string.Empty
                                    };
                                    if (invoiceModel.Items.Count > 0)
                                    {
                                        invoiceModel.Items.Last().ItemsComments.Add(itemComment);
                                    }
                                    break;
                            }
                        }
                    }
                    sftp.Disconnect();
                }
            }
            catch (Exception e)
            {
                // Handle exceptions (logging or rethrowing)
                throw new Exception(ErrorMessageConstants.Error_parsing_file, e);
            }

            return invoiceModel;
        }

        private static void BindHeaderRecord(EPMInvoiceImportModel invoiceModel, string[] dataFields)
        {
            invoiceModel.HeaderQualifier = dataFields.Length > 0 ? dataFields[0] : invoiceModel.HeaderQualifier;
            invoiceModel.InvoiceDate = dataFields.Length > 1 ? dataFields[1] : invoiceModel.InvoiceDate;
            invoiceModel.InvoiceNumber = dataFields.Length > 2 ? dataFields[2] : invoiceModel.InvoiceNumber;
            invoiceModel.PODate = dataFields.Length > 3 ? dataFields[3] : invoiceModel.PODate;
            invoiceModel.PONumber = dataFields.Length > 4 ? dataFields[4] : invoiceModel.PONumber;
            invoiceModel.TranType = dataFields.Length > 5 ? dataFields[5] : invoiceModel.TranType;
            invoiceModel.CarrierDetails = dataFields.Length > 6 ? dataFields[6] : invoiceModel.CarrierDetails;
            invoiceModel.CarrierName = dataFields.Length > 7 ? dataFields[7] : invoiceModel.CarrierName;
            // ShipTo properties added here
            invoiceModel.ShipToAttn = dataFields.Length > 8 ? dataFields[8] : invoiceModel.ShipToAttn;
            invoiceModel.ShipToLocation = dataFields.Length > 9 ? dataFields[9] : invoiceModel.ShipToLocation;
            invoiceModel.ShipToStreet1 = dataFields.Length > 10 ? dataFields[10] : invoiceModel.ShipToStreet1;
            invoiceModel.ShipToStreet2 = dataFields.Length > 11 ? dataFields[11] : invoiceModel.ShipToStreet2;
            invoiceModel.ShipToStreet3 = dataFields.Length > 12 ? dataFields[12] : invoiceModel.ShipToStreet3;
            invoiceModel.ShipToCity = dataFields.Length > 13 ? dataFields[13] : invoiceModel.ShipToCity;
            invoiceModel.ShipToState = dataFields.Length > 14 ? dataFields[14] : invoiceModel.ShipToState;
            invoiceModel.ShipToPostalCode = dataFields.Length > 15 ? dataFields[15] : invoiceModel.ShipToPostalCode;
            invoiceModel.ShipToCountry = dataFields.Length > 16 ? dataFields[16] : invoiceModel.ShipToCountry;
            //vendor properties added here
            invoiceModel.VendorNumber = dataFields.Length > 17 ? dataFields[17] : invoiceModel.VendorNumber;
            invoiceModel.VendorName = dataFields.Length > 18 ? dataFields[18] : invoiceModel.VendorName;
            invoiceModel.VendorStreet1 = dataFields.Length > 19 ? dataFields[19] : invoiceModel.VendorStreet1;
            invoiceModel.VendorStreet2 = dataFields.Length > 20 ? dataFields[20] : invoiceModel.VendorStreet2;
            invoiceModel.VendorStreet3 = dataFields.Length > 21 ? dataFields[21] : invoiceModel.VendorStreet3;
            invoiceModel.VendorCity = dataFields.Length > 22 ? dataFields[22] : invoiceModel.VendorCity;
            invoiceModel.VendorState = dataFields.Length > 23 ? dataFields[23] : invoiceModel.VendorState;
            invoiceModel.VendorPostalCode = dataFields.Length > 24 ? dataFields[24] : invoiceModel.VendorPostalCode;
            invoiceModel.VendorCountry = dataFields.Length > 25 ? dataFields[25] : invoiceModel.VendorCountry;

            // Remit properties added here
            invoiceModel.RemitName = dataFields.Length > 26 ? dataFields[26] : invoiceModel.RemitName;
            invoiceModel.RemitAddress1 = dataFields.Length > 27 ? dataFields[27] : invoiceModel.RemitAddress1;
            invoiceModel.RemitAddress2 = dataFields.Length > 28 ? dataFields[28] : invoiceModel.RemitAddress2;
            invoiceModel.RemitAddress3 = dataFields.Length > 29 ? dataFields[29] : invoiceModel.RemitAddress3;
            invoiceModel.RemitCity = dataFields.Length > 30 ? dataFields[30] : invoiceModel.RemitCity;
            invoiceModel.RemitState = dataFields.Length > 31 ? dataFields[31] : invoiceModel.RemitState;
            invoiceModel.RemitZip = dataFields.Length > 32 ? dataFields[32] : invoiceModel.RemitZip;
            invoiceModel.RemitCountry = dataFields.Length > 33 ? dataFields[33] : invoiceModel.RemitCountry;

            // Buyer properties start here
            invoiceModel.BuyerName = dataFields.Length > 34 ? dataFields[34] : invoiceModel.BuyerName;
            invoiceModel.BuyerPhoneNumber = dataFields.Length > 35 ? dataFields[35] : invoiceModel.BuyerPhoneNumber;
            invoiceModel.BuyerFaxNumber = dataFields.Length > 36 ? dataFields[36] : invoiceModel.BuyerFaxNumber;

            // Additional fields
            invoiceModel.ShipDate = dataFields.Length > 37 ? dataFields[37] : invoiceModel.ShipDate;
            invoiceModel.TermsNetDueDate = dataFields.Length > 38 ? dataFields[38] : invoiceModel.TermsNetDueDate;
            invoiceModel.POTotalCost = dataFields.Length > 39 ? decimal.Parse(dataFields[39]) : invoiceModel.POTotalCost;
        }
        #endregion


    }
}