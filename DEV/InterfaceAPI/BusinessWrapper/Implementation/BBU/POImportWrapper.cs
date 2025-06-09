using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using DataContracts;
using InterfaceAPI.Models.BBU.POImport;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using Common.Constants;
using System.Globalization;
using System.Text;
using Common.Enumerations;
using Presentation.Common;
using System.IO;
using RazorEngine;
using Rotativa;
using Rotativa.Options;
using System.Web.Mvc;
using Client.Models.PurchaseOrder;
using System.Web.Http.Controllers;
using Client.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using System.Web.Routing;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Common.Extensions;
using INTDataLayer.EL;
using System.Data;
using INTDataLayer.BAL;
using AzureUtil;
using System.Configuration;
using PgpCore;
using System.Web.Configuration;
using EvoPdf;
using EvoPdf.RtfToPdf;


namespace InterfaceAPI.BusinessWrapper.Implementation.BBU
{
    public class POImportWrapper : CommonWrapper, Interface.BBU.IPOImportWrapper
    {
        public Dictionary<long, Tuple<string, string>> po_email2 = new Dictionary<long, Tuple<string, string>>();
        public List<long> po_no_email = new List<long>();
        public string[] fileUrl;
        public POImportWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }


        #region Test Upload File
        public void EncryptandUploadFile(string fileName, string apiName)
        {
            EncryptandUploadFileToSFTP(fileName, apiName);
        }
        #endregion

        #region Download files in Local
        public string DownloadToLocal(string fileName, string apiName)
        {
            string filePath = string.Empty;
            filePath = DownloadFileToLocalDirectory(fileName, apiName);
            return filePath;
        }
        #endregion

        #region Decrypt File
        public string DecryptFile(string fileName, string encryptFilePath, string apiName)
        {                        
            return DecryptLocalFile(fileName, encryptFilePath, apiName);
        }
        #endregion

        #region Archive File
        public string ArchiveFileWithDBValue(string SourceFilePath, string sourceFileName, string DestinationPath)
        {
            return ArchiveFileWithDBvalue(SourceFilePath, sourceFileName, DestinationPath);
        }
        #endregion

        #region Create and Save in table
        public ProcessLogModel CreatePOImport(List<POImportJSONModel> poImportModelList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            POImport2 _poImport = new POImport2();
            POImportModel pmi = new POImportModel();
            TotalProcess = poImportModelList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            {
                Site site = new Site();
                site.ClientId = _userData.DatabaseKey.Client.ClientId;
                List<DataContracts.Site> listsite = site.RetrieveAll(_userData.DatabaseKey);
                foreach (var jItem in poImportModelList)
                {
                    pmi.ClientId = _userData.DatabaseKey.Client.ClientId;
                    Site ThisSite = listsite.FirstOrDefault(x => x.ExOrganizationId == Convert.ToString(jItem.SHIP_TO_ORGANIZATION_ID));
                    if (ThisSite == null)
                    {
                        pmi.SiteId = -1;
                    }
                    else
                    {
                        pmi.SiteId = ThisSite.SiteId;
                    }
                    pmi.PurchaseRequest = jItem.SOMAX_REQUISITION_NUMBER;
                    pmi.PurchaseRequestId = jItem.SOMAX_REQUISITION_ID == "" ? 0 : Convert.ToInt64(jItem.SOMAX_REQUISITION_ID);
                    // They (BBU) do NOT send us the work order                    
                    pmi.WorkOrder = "";// jItem.SOMAX_WORK_ORDER_REFERENCE;
                    pmi.ExRequest = jItem.ORACLE_REQUSITION_NUMBER;
                    pmi.PurchaseOrder = jItem.PURCHASE_ORDER_NUMBER;
                    pmi.ExPurchaseOrderId = jItem.PURCHASE_ORDER_ID == "" ? 0 : Convert.ToInt64(jItem.PURCHASE_ORDER_ID);
                    // RKL - 2017-Sep-17 - dates are in the format dd-mmm-yy
                    string dateformat = "dd-MMM-yy";
                    IFormatProvider culture = CultureInfo.InvariantCulture;
                    //string dateformat = "MM/dd/yyyy HH:mm:ss";
                    //IFormatProvider culture = new CultureInfo("en-US", true);
                    pmi.POCreateDate = jItem.PO_CREATION_DATE == "" ? DateTime.MinValue : DateTime.ParseExact(jItem.PO_CREATION_DATE, dateformat, culture);
                    pmi.Currency = jItem.PO_CURRENCY;
                    pmi.ExVendorId = jItem.ORACLE_VENDOR_ID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_VENDOR_ID);
                    pmi.ExVendor = jItem.ORACLE_VENDOR_NUMBER;
                    pmi.ExVendorSiteId = jItem.ORACLE_VENDOR_SITE_ID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_VENDOR_SITE_ID);
                    pmi.PRLineItemId = jItem.SOMAX_REQ_LINE_ID == "" ? 0 : Convert.ToInt64(jItem.SOMAX_REQ_LINE_ID);
                    pmi.LineNumber = jItem.ORACLE_PO_LINE_NUMBER == "" ? 0 : Convert.ToInt32(jItem.ORACLE_PO_LINE_NUMBER);
                    pmi.ExPurchaseOrderLineId = jItem.ORACLE_PO_LINE_ID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_PO_LINE_ID);
                    pmi.ExPartId = jItem.ORACLE_PART_ID == "" ? 0 : Convert.ToInt64(jItem.ORACLE_PART_ID);
                    pmi.ExPart = jItem.ORACLE_PART_NUMBER;
                    pmi.Description = jItem.ITEM_DESCRIPTION;
                    pmi.Category = jItem.UNSPSC_ID_TREE;
                    pmi.PurchaseQuantity = jItem.QUANTITY == "" ? 0 : Convert.ToDecimal(jItem.QUANTITY);
                    pmi.PurchaseUOM = jItem.PURCHASING_UNIT_OF_MEASURE;
                    pmi.UnitCost = jItem.UNIT_PRICE == "" ? 0 : Convert.ToDecimal(jItem.UNIT_PRICE);
                    //pmi.UnitOfMeasure = jItem.Primary_Unit_of_Measure; NB: Not in POImport2 Table
                    pmi.UOMConversion = jItem.UOM_CONVERSION_RATE == "" ? 0 : Convert.ToDecimal(jItem.UOM_CONVERSION_RATE);
                    pmi.LineStatus = jItem.SHIPMENT_STATUS;
                    pmi.Required = jItem.NEED_BY_DATE == "" ? DateTime.MinValue : DateTime.ParseExact(jItem.NEED_BY_DATE, dateformat, culture);
                    pmi.Revision = jItem.REVISION_NUM == "" ? 0 : Convert.ToInt32(jItem.REVISION_NUM);
                    pmi.BillToAddress1 = jItem.BILL_TO_ADDRESS1;
                    pmi.BillToAddress2 = jItem.BILL_TO_ADDRESS2;
                    pmi.BillToAddressCity = jItem.BILL_TO_CITY;
                    pmi.BillToAddressState = jItem.BILL_TO_STATE;
                    pmi.BillToAddressPostCode = jItem.BILL_TO_ZIP;
                    pmi.BillToAddressCountry = jItem.BILL_TO_COUNTRY;
                    pmi.ShipToAddress1 = jItem.SHIP_TO_ADDRESS1;
                    pmi.ShipToAddress2 = jItem.SHIP_TO_ADDRESS2;
                    pmi.ShipToAddressCity = jItem.SHIP_TO_CITY;
                    pmi.ShipToAddressState = jItem.SHIP_TO_STATE;
                    pmi.ShipToAddressPostCode = jItem.SHIP_TO_ZIP;
                    pmi.ShipToAddressCountry = jItem.SHIP_TO_COUNTRY;
                    pmi.PaymentTerms = jItem.PAYMENT_TERMS;
                    pmi.ExpenseAccount = jItem.EXPENSE_ACCOUNT;
                    if (pmi.SiteId >= 0)
                    {

                        _poImport = SavePOImport(pmi);
                        ++NewTransactions;
                        if (_poImport.ErrorMessage.Count() > 0)
                        {
                            return objProcessLogModel;
                        }
                    }

                    if (_poImport.ErrorMessages == null || _poImport.ErrorMessages.Count == 0)
                    {
                        SuccessfulProcess++;
                    }
                    else
                    {
                        FailedProcess++;
                    }
                }//end of foreach
            }
            catch (Exception ex)
            {

            }
            if (FailedProcess == 0)
            {
                logMessage = "Json data inserted into temporary table.";
            }
            else
            {
                logMessage = "Conversion of json data to temporary data tables failed.";
            }
            objProcessLogModel.NewProcess = NewTransactions;
            objProcessLogModel.TotalProcess = TotalProcess;
            objProcessLogModel.SuccessfulProcess = SuccessfulProcess;
            objProcessLogModel.FailedProcess = FailedProcess;
            objProcessLogModel.logMessage = logMessage;
            return objProcessLogModel;
        }
        private POImport2 SavePOImport(POImportModel pmi)
        {
            POImport2 pOImport2 = new POImport2();
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
                pOImport2.ClientId = _userData.DatabaseKey.Client.ClientId;
                pOImport2.SiteId = pmi.SiteId;
                pOImport2.PurchaseRequest = pmi.PurchaseRequest;
                pOImport2.PurchaseRequestId = pmi.PurchaseRequestId;
                pOImport2.WorkOrder = pmi.WorkOrder;
                pOImport2.ExRequest = pmi.ExRequest;
                pOImport2.PurchaseOrder = pmi.PurchaseOrder;
                pOImport2.ExPurchaseOrderId = pmi.ExPurchaseOrderId;
                pOImport2.POCreateDate = pmi.POCreateDate;
                pOImport2.Currency = pmi.Currency;
                pOImport2.ExVendorId = pmi.ExVendorId;
                pOImport2.ExVendor = pmi.ExVendor;
                pOImport2.ExVendorSiteId = pmi.ExVendorSiteId;
                pOImport2.PRLineItemId = pmi.PRLineItemId;
                pOImport2.LineNumber = pmi.LineNumber;
                pOImport2.ExPurchaseOrderLineId = pmi.ExPurchaseOrderLineId;
                pOImport2.ExPartId = pmi.ExPartId;
                pOImport2.ExPart = pmi.ExPart;
                pOImport2.Description = pmi.Description;
                pOImport2.Category = pmi.Category;
                pOImport2.PurchaseQuantity = pmi.PurchaseQuantity;
                pOImport2.PurchaseUOM = pmi.PurchaseUOM;
                pOImport2.UnitCost = pmi.UnitCost;
                pOImport2.UOMConversion = pmi.UOMConversion;
                pOImport2.LineStatus = pmi.LineStatus;
                pOImport2.Required = pmi.Required;

                pOImport2.Revision = pmi.Revision;
                pOImport2.BillToAddress1 = pmi.BillToAddress1;
                pOImport2.BillToAddress2 = pmi.BillToAddress2;
                pOImport2.BillToCity = pmi.BillToAddressCity;
                pOImport2.BillToState = pmi.BillToAddressState;
                pOImport2.BillToZip = pmi.BillToAddressPostCode;
                pOImport2.BillToCountry = pmi.BillToAddressCountry;
                pOImport2.ShipToAddress1 = pmi.ShipToAddress1;
                pOImport2.ShipToAddress2 = pmi.ShipToAddress2;
                pOImport2.ShipToCity = pmi.ShipToAddressCity;
                pOImport2.ShipToState = pmi.ShipToAddressState;
                pOImport2.ShipToZip = pmi.ShipToAddressPostCode;
                pOImport2.ShipToCountry = pmi.ShipToAddressCountry;
                pOImport2.PaymentTerms = pmi.PaymentTerms;
                pOImport2.ExpenseAccount = pmi.ExpenseAccount;
                // Determine if the item already exists 
                // Use ClientId, ExPurchaseOrderId, ExPurchaseOrderLineId for matching purposes
                POImport2 po2 = new POImport2();
                po2.ClientId = _userData.DatabaseKey.Client.ClientId;
                po2.ExPurchaseOrderId = pOImport2.ExPurchaseOrderId;
                po2.ExPurchaseOrderLineId = pOImport2.ExPurchaseOrderLineId;
                po2.RetrieveForImportCompare(_userData.DatabaseKey);


                try
                {
                    if (po2.POImport2Id > 0)
                    {
                        pOImport2.POImport2Id = po2.POImport2Id;
                        pOImport2.UpdateIndex = po2.UpdateIndex;
                        pOImport2.Update(_userData.DatabaseKey);
                    }
                    else
                    {
                        pOImport2.Create(_userData.DatabaseKey);
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    pOImport2.ErrorMessage = errMsg;
                }
            }
            return pOImport2;
        }
        #endregion

        #region Process Purchase Order Import
        public ProcessLogModel ProcessPOImport()
        {
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            POImport2 POImport = new POImport2();
            List<POImport2> POImlist = POImport.POImport2_RetrieveAll(_userData.DatabaseKey);
            int TotalProcess = POImlist.Count;
            int SucessfulTransaction = 0;
            int FailTransaction = 0;
            foreach (var item in POImlist)
            {
                POImport2 POImport2Interface = new POImport2();
                POImport2Interface.CallerUserInfoId = _userData.DatabaseKey.User.UserInfoId;
                POImport2Interface.CallerUserName = _userData.DatabaseKey.UserName;
                POImport2Interface.ClientId = _userData.DatabaseKey.Client.ClientId;
                POImport2Interface.POImport2Id = item.POImport2Id;
                POImport2Interface.ValidatePOImport(_userData.DatabaseKey);
                try
                {
                    // Delete is handled by the SP
                    if (POImport2Interface.ErrorMessages.Count == 0)
                    {
                        POImport2Interface.Create_POImport2ProcessInterface(_userData.DatabaseKey);
                        POImport2Interface.Retrieve(_userData.DatabaseKey);
                        if (POImport2Interface.LineStatus.ToUpper() == "OPEN")
                        {
                            long poid = POImport2Interface.PurchaseOrderId;
                            if (POImport2Interface.VendorAutoEmail)
                            {
                                if (!po_email2.ContainsKey(poid))
                                {
                                    // RKL - Need to check if the PO has already been emailed.
                                    // not done yet
                                    // Will need to make sure the revision has not been emailed
                                    po_email2.Add(poid, Tuple.Create(POImport2Interface.PurchaseOrder, POImport2Interface.VendorEmailAddress));
                                }
                            }
                            else
                            {
                                if (!po_no_email.Contains(poid))
                                {
                                    po_no_email.Add(poid);
                                }
                            }
                        }
                        POImport2Interface.Delete(_userData.DatabaseKey);
                        SucessfulTransaction++;
                    }
                    else
                    {
                        if (POImport2Interface.ErrorMessages.Count > 0)
                        {
                            StringBuilder ErrMsg = new StringBuilder();
                            foreach (var err in POImport2Interface.ErrorMessages)
                            {
                                ErrMsg.AppendLine(err.ToString());
                            }
                            POImport2Interface.Retrieve(_userData.DatabaseKey);
                            POImport2Interface.ErrorMessage = ErrMsg.ToString();
                            POImport2Interface.LastProcess = DateTime.UtcNow;
                            POImport2Interface.Update(_userData.DatabaseKey);
                            objProcessLogModel.logMessage += POImport2Interface.ErrorMessage;
                            ++FailTransaction;
                        }
                        //FailTransaction++;
                    }
                    objProcessLogModel.SuccessfulProcess = SucessfulTransaction;
                    objProcessLogModel.FailedProcess = FailTransaction;
                }
                catch (Exception ex)
                {
                    objProcessLogModel.logMessage += ex.Message.ToString();
                }
            }
            if (FailTransaction == 0)
            {
                objProcessLogModel.logMessage += " Process Complete – All items processed successfully ";
            }
            else
            {
                objProcessLogModel.logMessage += " Process Complete – Not all items were processed successfully ";
            }

            return objProcessLogModel;
        }
        #endregion
        #region Generate Events
        // V2-612
        public void GenerateEvents()
        {
          // Generate Events
          // PO Imported - Done for items auto-emailed  
          PurchasingEventLog log = new PurchasingEventLog();
          foreach (KeyValuePair<long, Tuple<string, string>> poid in po_email2)
          {
            log.Clear();
            log.ClientId = _userData.DatabaseKey.Client.ClientId;
            log.SiteId = _userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = poid.Key;
            log.Event = PurchasingEvents.Import;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "Purchase Order Imported";
            log.SourceId = 0;
            log.Create(_userData.DatabaseKey);
          }
          // PO Imported - Auto-Emailed to Vendor
          foreach (KeyValuePair<long, Tuple<string, string>> poid in po_email2)
          {
            log.Clear();
            log.ClientId = _userData.DatabaseKey.Client.ClientId;
            log.SiteId = _userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = poid.Key;
            log.Event = PurchasingEvents.EmailToVendor;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "Purchase Order Automatically Emailed to Vendor";
            log.SourceId = 0;
            log.Create(_userData.DatabaseKey);
          }
          // Imported - For those that were imported and not automatically emailed
          foreach (long poid in po_no_email)
          {
            log.Clear();
            log.ClientId = _userData.DatabaseKey.Client.ClientId;
            log.SiteId = _userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = poid;
            log.Event = PurchasingEvents.Import;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "Purchase Order Imported and NOT Automatically Emailed to Vendor";
            log.SourceId = 0;
            log.Create(_userData.DatabaseKey);
          }
          po_email2.Clear();
          po_no_email.Clear();    
        }
        #endregion Generate Events
        #region Send Alerts
        public void SendAlert()
        {
            // Send Alerts for items to be emailed 
            List<long> alertlist = new List<long>();
            foreach (KeyValuePair<long, Tuple<string, string>> poinfo in po_email2)
            {
                alertlist.Add(poinfo.Key);
            }
            if (alertlist.Count > 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this._userData);
                objAlert.CreateAlert<PurchaseOrder>(AlertTypeEnum.POEmailToVendor, alertlist);
            }
            if (po_no_email.Count > 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this._userData);
                objAlert.CreateAlert<PurchaseOrder>(AlertTypeEnum.POImportedReviewRequired, po_no_email);
            }
        }
        #endregion
        
        #region Send Emails
        // RKL - For email testing purposes 
        public void SetEmailArray(long poid, string ponumber, string emailaddress)
        {
          po_email2.Add(poid,Tuple.Create(ponumber, emailaddress));
        }
        public void SendBBUEmails()
        {
            // key = purchase order id
            // tuple item 1 = purchase order clientlookupid
            // tuple item 2 = vendor email address
           
           
            foreach (KeyValuePair<long, Tuple<string, string>> poinfo in po_email2)
            {
                long poid = poinfo.Key;
                string poclientid = poinfo.Value.Item1;
                string emailAddress = poinfo.Value.Item2;
                EmailModule emailModule = new EmailModule();               
                emailModule.ToEmailAddress = emailAddress;
                //emailModule.MailSubject = "Purchase Order" + " " + poclientid;
                emailModule.Subject = "Purchase Order" + " " + poclientid;
                CreateAttachment_BBU(poid, ref emailModule);
                //string BodyHeader = "Purchase Order" + " " + poclientid + " " + "is attached";
                // string FooterSignature = this._userData.DatabaseKey.User.FirstName + " " + this._userData.DatabaseKey.User.LastName;
                // emailModule.MailBody = "<html><body>" + BodyHeader + " <br><br> " + "Please review and process" + " <br><br> " + FooterSignature + "";
                // emailModule.CcEmail = ""; // Need to get the ccemail from 


            }
            //po_email2.Clear();
            //po_no_email.Clear();
        }
        private void CreateAttachment_BBU(long poId, ref EmailModule emailModule)
        {            
            InterfaceAPI.Models.BBU.POImport.POImportModel pOImportModel = new POImportModel();
            List<DataContracts.PurchaseOrder> listPO = new List<DataContracts.PurchaseOrder>();
            pOImportModel.PurchaseOrderId = poId;
            PopulateTemplate(poId, ref pOImportModel, ref listPO );
            string BodyContent = string.Empty;
            string emailHtmlBody = string.Empty;
            var templateFilePath = System.Web.HttpContext.Current.Server.MapPath(@"\Views\Shared\CommonEmailTemplate.cshtml");
            var templateContent = string.Empty;
            using (var reader = new StreamReader(templateFilePath))
            {
                templateContent = reader.ReadToEnd();
            }

            emailHtmlBody = ParseTemplate(templateContent);
            BodyContent = emailHtmlBody;
            if (BodyContent != null)
            {
                string resetUrl = GetHostedUrl();
                //resetUrl = "https://app.somax.com/";//--comment out after testing-- email body images does not work in Localhost //
                string output = BodyContent.
                    Replace("headerBgURL", resetUrl + SomaxAppConstants.HeaderMailTemplate).
                    Replace("somaxLogoURL", resetUrl + SomaxAppConstants.SomaxLogoForMailTemplate).
                    Replace("strtypeofrequest", "Purchase Order").
                    Replace("strrequestid", listPO[0].ClientLookupId).
                    Replace("strmailbodycomments", "").                    
                    Replace("spnloginurl", resetUrl).
                    Replace("footerBgURL", resetUrl + SomaxAppConstants.FooterMailTemplate);
                emailModule.MailBody = output;
            }
            else
            {
                // emailModule.MailBody = "<html><body>" + BodyHeader + " <br><br> " + FooterSignature + "";
            }
            string inputFilePath="";
            string PdfName = "";
            string PurchaseOrderPath = "", TermsAndCondPath = "", FinalPDFPath = "", MergedPath = "";
            inputFilePath = ExportPDF(ApiConstants.OraclePOImport, pOImportModel, ref PdfName, out PurchaseOrderPath, out MergedPath, out FinalPDFPath,  out TermsAndCondPath);
            // System.Net.Mail.Attachment pattachment = new System.Net.Mail.Attachment(stream, PdfName, "application/pdf");
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(HttpContext.Current.Server.MapPath(inputFilePath));
            attachment.Name = PdfName;            
            emailModule.MailAttachment = attachment;
            bool isSend=emailModule.SendEmail();
            emailModule.MailAttachment.Dispose();
            if(isSend)
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(HttpContext.Current.Server.MapPath(inputFilePath));
                if (!string.IsNullOrEmpty(PurchaseOrderPath))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(PurchaseOrderPath));
                }
                if (!string.IsNullOrEmpty(MergedPath))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(MergedPath));
                }
                if (!string.IsNullOrEmpty(MergedPath))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(FinalPDFPath));
                }
                if (!string.IsNullOrEmpty(TermsAndCondPath))
                {
                    File.Delete(HttpContext.Current.Server.MapPath(TermsAndCondPath));
                }

                // DeleteLocalFiles(Path.GetDirectoryName(HttpContext.Current.Server.MapPath(inputFilePath)));
            }

        }
        private static string ParseTemplate(string content)
        {
            string _mode = DateTime.Now.Ticks + new Random().Next(1000, 100000).ToString();
           // Razor.Compile(content, _mode);
            return Razor.Parse(content);
        }
         private void PopulateTemplate(Int64 poId, ref InterfaceAPI.Models.BBU.POImport.POImportModel pOImportModel, ref List<DataContracts.PurchaseOrder> listPO )
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            listPO = new List<DataContracts.PurchaseOrder>();
            List<DataContracts.PurchaseOrderLineItem> ListPOLine = new List<DataContracts.PurchaseOrderLineItem>();
            //retrieve PurchaseOrder
            DataContracts.PurchaseOrder poContract = new DataContracts.PurchaseOrder();
            poContract.PurchaseOrderId = poId;
            poContract.RetrieveByPKForeignKeys(_userData.DatabaseKey, _userData.Site.TimeZone);
            listPO.Add(poContract);

            //retrieve PurchaseOrderLineItems
            DataContracts.PurchaseOrderLineItem poLineitems = new DataContracts.PurchaseOrderLineItem()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = poContract.PurchaseOrderId
            };
            ListPOLine = DataContracts.PurchaseOrderLineItem.PurchaseOrderLineItemRetrieveByPurchaseOrderId_V2(_userData.DatabaseKey, poLineitems);

            pOImportModel = new POImportModel();
            pOImportModel.poImportLineItemsModel = new List<POImportLineItemsModel>();
            pOImportModel.ClientLookupId = poContract.ClientLookupId;
            pOImportModel.Revision = poContract.Revision;
            pOImportModel.VendorAddressCountry = poContract.VendorAddressCountry == "US" ? "United States" : poContract.VendorAddressCountry;
            pOImportModel.VendorName = poContract.VendorName;
            pOImportModel.VendorAddress1 = poContract.VendorAddress1;
            pOImportModel.MessageToVendor = poContract.MessageToVendor;//MsgVendorComment
            pOImportModel.VendorAddress2 = poContract.VendorAddress2;
            pOImportModel.VendorAddress3 = poContract.VendorAddress3;
            pOImportModel.VendorAddressCity = poContract.VendorAddressCity;
            pOImportModel.VendorAddressState = poContract.VendorAddressState;
            pOImportModel.VendorAddressPostCode = poContract.VendorAddressPostCode;
            pOImportModel.VendorPhoneNumber = poContract.VendorPhoneNumber;
            pOImportModel.BillToName = poContract.SiteBillToName;
            pOImportModel.BillToAddress1 = poContract.SiteBillToAddress1;
            pOImportModel.BillToAddress2 = poContract.SiteBillToAddress2;
            pOImportModel.BillToAddress3 = poContract.SiteBillToAddress3;
            pOImportModel.BillToAddressCity = poContract.SiteBillToAddressCity;
            pOImportModel.BillToAddressState = poContract.SiteBillToAddressState;
            pOImportModel.BillToAddressPostCode = poContract.SiteBillToAddressPostCode;
            pOImportModel.BillToAddressCountry = poContract.SiteBillToAddressCountry;
            pOImportModel.CreateDate = String.Format("{0:dd-MMM-yyyy}", poContract.CreateDate); 
            pOImportModel.PaymentTerms = poContract.PaymentTerms;
            pOImportModel.Required = poContract.Required;
            pOImportModel.FOB = poContract.FOB;
            pOImportModel.Carrier = poContract.Carrier;
            pOImportModel.Creator_PersonnelName = poContract.Creator_PersonnelName;
            pOImportModel.PersonnelEmail = poContract.PersonnelEmail;
            pOImportModel.SiteName = poContract.SiteName;
            pOImportModel.ShipToAddress1 = poContract.SiteAddress1;
            pOImportModel.ShipToAddress2 = poContract.SiteAddress2;
            pOImportModel.ShipToAddress3 = poContract.SiteAddress3;
            pOImportModel.ShipToAddressCity = poContract.SiteAddressCity;
            pOImportModel.ShipToAddressState = poContract.SiteAddressState;
            pOImportModel.ShipToAddressPostCode = poContract.SiteAddressPostCode;
            pOImportModel.ShipToAddressCountry = poContract.SiteAddressCountry;
            pOImportModel.Currency = poContract.Currency;
            pOImportModel.PrintDate = DateTime.UtcNow.ToUserTimeZone(_userData.Site.TimeZone).ToString("g");
            // RKL - Added so that empty dates are not put on the page.
            objPurchaseOrderVM.POLineItemList = new List<POLineItemModel>();
            foreach (DataContracts.PurchaseOrderLineItem PL in ListPOLine)
            {
                if (PL.EstimatedDelivery == DateTime.MinValue)
                {
                    PL.EstimatedDelivery = null;
                }
                InterfaceAPI.Models.BBU.POImport.POImportLineItemsModel poImportLineItemsModels = new InterfaceAPI.Models.BBU.POImport.POImportLineItemsModel();
                poImportLineItemsModels.PartClientLookupId = PL.PartClientLookupId;
                poImportLineItemsModels.Description = PL.Description;
                poImportLineItemsModels.PartNumber = PL.PartNumber;
                poImportLineItemsModels.PurchaseQuantity = String.Format("{0:n2}", PL.PurchaseQuantity);//VendorItem
                poImportLineItemsModels.PurchaseUOM = PL.PurchaseUOM;
                poImportLineItemsModels.PurchaseCost = String.Format("{0:n2}",PL.PurchaseCost);
                poImportLineItemsModels.LineTotal = String.Format("{0:n2}", PL.LineTotal);
                poImportLineItemsModels.EstimatedDelivery = String.Format("{0:dd-MMM-yyyy}", PL.EstimatedDelivery);
                poImportLineItemsModels.LineNumber = PL.LineNumber;
                pOImportModel.poImportLineItemsModel.Add(poImportLineItemsModels);
            }
            SetImageUrl(pOImportModel);
           

            // DownloadPurchaseTermsCondition(pOImportModel);
        }

        #endregion

        public string ExportPDF(string apiName, POImportModel po, ref string PdfName,out string POPath,out string TCPath,out string FinalPDFPath, out string MergedPath)
        {
            MemoryStream stream = new MemoryStream();
            string CurrentDateTime = DateTime.Now.Ticks.ToString();
            string AvilableRTFFileName = "";
            byte[] bytes;
            BBUPrintPDF controller = new BBUPrintPDF();
            RouteData route = new RouteData();
            route.Values.Add("action", "PDFForMailAttachment"); // ActionName
            route.Values.Add("controller", "BBUPrintPDF"); // Controller Name
            System.Web.Mvc.ControllerContext newContext = new System.Web.Mvc.ControllerContext(new HttpContextWrapper(System.Web.HttpContext.Current), route, controller);
            controller.ControllerContext = newContext;

            var ms = new MemoryStream();
            bool jsonStringExceed = false;
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            Int64 fileSizeCounter = 0;
            Int32 maxPdfSize = section.MaxRequestLength;
            string attachUrl = string.Empty;
            var doc = new iTextSharp.text.Document();
            var copy = new iTextSharp.text.pdf.PdfSmartCopy(doc, ms);
            doc.Open();            
            bytes = controller.PDFForMailAttachment(po);
            var reader = new iTextSharp.text.pdf.PdfReader(bytes);
            copy.AddDocument(reader);
            //PdfReader read = new PdfReader(HttpContext.Current.Server.MapPath(DownloadPurchaseTermsCondition(po, out AvilableRTFFileName)));
            //read.ConsolidateNamedDestinations();
            //for (int i = 1; i <= read.NumberOfPages; i++)
            //{
            //    PdfImportedPage page = copy.GetImportedPage(read, i);
            //    copy.AddPage(page);
            //}
            doc.Close();
            DownloadPurchaseTermsCondition(po, out AvilableRTFFileName);
            byte[] pdf = ms.ToArray();

            string inPathLocal = System.Configuration.ConfigurationManager.AppSettings["LocalImportDirectory"].ToString().Trim()
                 + System.Configuration.ConfigurationManager.AppSettings[apiName].ToString().Trim() + "/AttachmentTemplate";
            if (!Directory.Exists(inPathLocal))
            {
                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(inPathLocal));
            }
            PdfName = "PurchaseOrderDetails_" + CurrentDateTime + "_.pdf";
            string inputFilePath = Path.Combine(inPathLocal, PdfName);
            System.IO.File.WriteAllBytes(HttpContext.Current.Server.MapPath(inputFilePath), pdf);
            stream.Write(pdf, 0, pdf.Length);

            #region rtf to html using Rtfpipe
            string html = "";
            string rtfPathLocal = ConfigurationManager.AppSettings["LocalImportDirectory"].ToString().Trim()
                 + ConfigurationManager.AppSettings[apiName].ToString().Trim() + "/PurchaseTermsAndCond";
            string rtfFileName = AvilableRTFFileName;
            string rtfFilePath = Path.Combine(rtfPathLocal, rtfFileName);
            using (FileStream rtfFilestream = new FileStream(HttpContext.Current.Server.MapPath(rtfFilePath), FileMode.Open, FileAccess.Read))
            {
                html = RtfPipe.Rtf.ToHtml(rtfFilestream);
            }
            #endregion

            #region html to pdf
           
            string TermsAndCondFileName = "TermsAndCond_" + CurrentDateTime + "_.pdf";            
            string TermsAndCondFilePath = Path.Combine(inPathLocal, TermsAndCondFileName);
            string TermsAndCondPaged = "MergedPaged_" + CurrentDateTime + "_.pdf";
            string TermsAndCondPagedPath = Path.Combine(inPathLocal, TermsAndCondPaged);
            if (!string.IsNullOrEmpty(html))
            {
                var convertedpdf = ConvertToPdf(html.Replace("<br>", "<br />"));

                //StringReader sr = new StringReader(html.ToString());
                //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    //pdfDoc.Open();

                    //htmlparser.Parse(sr);
                    //pdfDoc.Close();

                    byte[] pdfbytes = convertedpdf;// memoryStream.ToArray();
                    memoryStream.Close();
                    
                    File.WriteAllBytes(HttpContext.Current.Server.MapPath(TermsAndCondFilePath), pdfbytes);
                }
            }
            #endregion

            #region Merge file
            string[] filenames = new string[2];
            filenames[0] = HttpContext.Current.Server.MapPath(inputFilePath);
            filenames[1] = HttpContext.Current.Server.MapPath(TermsAndCondFilePath);

            string MergedFile = "Merge_" + CurrentDateTime + "_.pdf";
            string FinalPath = Path.Combine(inPathLocal, MergedFile);
            string outputPath = HttpContext.Current.Server.MapPath(FinalPath);
            Document mergedoc = new Document();

            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                PdfCopy copywriter = new PdfCopy(mergedoc, fs);
                if (copywriter != null)
                {
                    mergedoc.Open();
                    foreach (string filename in filenames)
                    {
                        PdfReader pdfreader = new PdfReader(filename);
                        reader.ConsolidateNamedDestinations();
                        for (int i = 1; i <= pdfreader.NumberOfPages; i++)
                        {
                            PdfImportedPage page = copywriter.GetImportedPage(pdfreader, i);
                            copywriter.AddPage(page);
                        }
                       
                        reader.Close();
                    }
                    copywriter.Close();
                    mergedoc.Close();
                }
            }
            #endregion
            POPath = inputFilePath;
            MergedPath = TermsAndCondFilePath;
            TCPath = TermsAndCondPagedPath;
            FinalPDFPath = FinalPath;
            AddPageNumber(FinalPath, TermsAndCondPagedPath, po);
            return TermsAndCondPagedPath;
        }
        void AddPageNumber(string fileIn, string fileOut, POImportModel po)
        {
            byte[] bytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(fileIn));
            Font blackFont = FontFactory.GetFont("Calibri", 8, Font.NORMAL, BaseColor.BLACK);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfReader reader = new PdfReader(bytes);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    for (int i = 1; i <= pages; i++)
                    {
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_CENTER, new Phrase("Page: " + i.ToString()+" of "+ pages, blackFont), 284f, 8f, 0);
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase("Printed: " + po.PrintDate, blackFont), 20f, 8f, 0);
                    }
                }
                bytes = stream.ToArray();
            }
            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fileOut), bytes);
        }

        public void SetImageUrl(POImportModel model)
        {
            // Return the Image URL 
            string image_url = string.Empty;
            byte[] imgbyte = new byte[0];
            // Retrieve Loco Attachment Record
            Attachment attach = new Attachment()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId
            };
            attach.RetrieveLogo(_userData.DatabaseKey, _userData.Site.SiteId);
            if (string.IsNullOrEmpty(attach.AttachmentURL))
            {
                ClientLogoEL ce = new ClientLogoEL();
                ce.ClientId = _userData.DatabaseKey.Client.ClientId;
                ce.SiteId = 0;
                ce.Type = "Forms";
               
                DataTable dt = ClientLogoBL.GetLogoByType(ce, _userData.DatabaseKey.ClientConnectionString);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Image"] != null && dt.Rows[0]["Image"].ToString() != "")
                    {
                        imgbyte = (byte[])dt.Rows[0]["Image"];
                    }
                    else
                    {
                        imgbyte = new byte[0];
                    }
                }
                else
                {
                    imgbyte = new byte[0];
                }
            }
            else
            {
                image_url = attach.AttachmentURL;
                if (!attach.External)
                {
                    AzureUtil.AzureSetup azure = new AzureUtil.AzureSetup();
                    image_url = azure.GetSASUrlClientSite(_userData.DatabaseKey.Client.ClientId, _userData.Site.SiteId, image_url);
                }
               
                model.Logo = image_url;
            }
        }
        public string DownloadPurchaseTermsCondition(POImportModel model, out string rtfFileName)
        {
            string downLoadFilePath = "";
            string inputFilePath = "";
            string outputFilePath = "";
            string fileName = "";
            rtfFileName = "";
            MemoryStream mystream = null;
            byte[] tmpBytes = null;
            ClientBAL cb = new ClientBAL();
            DataTable ds = ClientBAL.RetrieveClientbyCId(_userData.DatabaseKey.User.UserInfoId, _userData.DatabaseKey.Client.ClientId, _userData.DatabaseKey.ClientConnectionString);
            {
                if (ds.Rows[0]["PurchaseTermsandConds"] != null && ds.Rows[0]["PurchaseTermsandConds"].ToString() != "")
                {
                    fileUrl = ds.Rows[0]["PurchaseTermsandConds"].ToString().Split('/');
                    if (fileUrl[4].ToString().Contains(".rtf"))
                    {
                        WebClient webClient = new WebClient();
                        // RKL - 2017-Dec-20 - Need to put this in a different location - the root of the c drive is not the place to do this.
                        try
                        {
                            downLoadFilePath = System.Configuration.ConfigurationManager.AppSettings["LocalImportDirectory"].ToString().Trim()
                 + System.Configuration.ConfigurationManager.AppSettings["OraclePurchaseTermsConditionFolderPath"].ToString().Trim() + "";
                            if (!Directory.Exists(downLoadFilePath))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(HttpContext.Current.Server.MapPath(downLoadFilePath));
                            }
                            fileName =fileUrl[4].ToString();
                            rtfFileName = fileName;
                            inputFilePath = Path.Combine(downLoadFilePath, fileName);
                            string ext = "pdf";
                            fileName = Path.GetFileNameWithoutExtension(fileUrl[4].ToString()); //fileUrl[4].ToString(); 
                            outputFilePath = Path.Combine(downLoadFilePath, fileName + ".pdf");

                            webClient.DownloadFile(ds.Rows[0]["PurchaseTermsandConds"].ToString(), HttpContext.Current.Server.MapPath(inputFilePath));
                            var templateContent = string.Empty;
                            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(inputFilePath), FileMode.Open, FileAccess.Read);
                            tmpBytes = new byte[fs.Length];
                            fs.Read(tmpBytes, 0, Convert.ToInt32(fs.Length));
                            mystream = new MemoryStream(tmpBytes);
                            //using (var reader = new StreamReader(HttpContext.Current.Server.MapPath(inputFilePath)))
                            //{
                            //    templateContent = reader.ReadToEnd();
                            //}
                            //model.PurchaseTerms = templateContent;
                            //PdfConverter pdfConverter = new PdfConverter();
                            //pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
                            //pdfConverter.PdfDocumentOptions.LeftMargin = 5;
                            //pdfConverter.PdfDocumentOptions.RightMargin = 10;
                            //pdfConverter.PdfDocumentOptions.TopMargin = 15;
                            //pdfConverter.PdfDocumentOptions.BottomMargin = 10;
                            //pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;
                            //pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
                            //pdfConverter.PdfDocumentOptions.ShowFooter = false;
                            //pdfConverter.PdfDocumentOptions.ShowHeader = false;
                            //byte[] downloadBytes = pdfConverter.GetPdfBytesFromRtfString(HttpContext.Current.Server.MapPath(inputFilePath));
                            //pdfConverter.SavePdfFromRtfFileToFile(HttpContext.Current.Server.MapPath(inputFilePath), HttpContext.Current.Server.MapPath(outputFilePath));
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                        }
                    }
                }
            }
            return outputFilePath;
        }

        [ValidateInput(false)]
        private byte[] ConvertToPdf(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 6f, 6f, 6f, 20f);
                
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                writer.SetFullCompression();                
                pdfDoc.Open();               
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return stream.ToArray();

            }
        }       
    }

    public class BBUPrintPDF : Client.Controllers.Common.SomaxBaseController
    {
        public Byte[] PDFForMailAttachment(POImportModel pvm)
        {
            
            //string customSwitches = string.Format("--header-html  \"{0}\" " +
            //                    "--header-spacing \"1\" " +
            //                    "--header-font-size \"10\" ",
            //                    null);
            //string customSwitches = string.Format("--header-html  \"{0}\" " +
            //                   //"--header-spacing \"1\" " +
            //                   //  "--footer-html \"{1}\" " +
            //                   //   "--footer-spacing \"8\" " +
            //                      "--page-offset 0 --footer-center [page]/[toPage] --footer-font-size \"8\" " +
            //                   "--header-font-size \"10\" ",
            //                   Url.Action("Header", "BBUPrintPDF", new { POImportModel = pvm }, Request.Url.Scheme)
            //                   //Url.Action("Footer", "POImport", Request.Url.Scheme)
            //                   );
            var mailpdft = new ViewAsPdf(@"~\Views\Shared\MailTemplate\POImport\POImportPrintTemplate_V1.cshtml", pvm)
            {
                PageOrientation=Orientation.Portrait,
                PageSize=Rotativa.Options.Size.A4,
                PageMargins = new Margins(15, 6, 20, 6),// it’s in millimeters
                //CustomSwitches = customSwitches
            };
            Byte[] PdfData = mailpdft.BuildPdf(this.ControllerContext);
            return PdfData;
        }
        //[AllowAnonymous]
        //public ActionResult Header(POImportModel pvm)
        //{

        //    return View(@"~\Views\Shared\MailTemplate\POImport\PrintHeader_V1.cshtml", pvm);

        //}
        //[AllowAnonymous]
        //public ActionResult Footer(POImportModel pvm)
        //{

        //    return View(@"~\Views\Shared\MailTemplate\POImport\PrintFooter_V1.cshtml", pvm);

        //}
    }
}