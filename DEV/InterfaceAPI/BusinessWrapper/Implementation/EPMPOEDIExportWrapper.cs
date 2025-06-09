using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Web;
using InterfaceAPI.Models.Common;
using DataContracts;
using InterfaceAPI.Models.EPMPOEDIExport;
using Common.Constants;
using System.IO;
using Business.Authentication;
using Common.Enumerations;
using System.Text;
using Common.Extensions;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
    public class EPMPOEDIExportWrapper : CommonWrapper, IEPMPOEDIExportWrapper
    {
        Int64 logID;
        public static string dirpath;
        public EPMPOEDIExportWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Convert data row to model
        public EPMPOEDIExportModel ConvertDataRowToModel(long PurchaseOrderId)
        {
            // Temporary
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            purchaseOrder.ClientId = _userData.DatabaseKey.Client.ClientId;
            purchaseOrder.SiteId = _userData.Site.SiteId;
            purchaseOrder.PurchaseOrderId = PurchaseOrderId;
            purchaseOrder.RetrieveForEDIExport_V2(_userData.DatabaseKey, _userData.Site.TimeZone);
            List<EPMPOEDIExportModel> PurchaseExportModelList = new List<EPMPOEDIExportModel>();
            EPMPOEDIExportModel purchaseExportModel = new EPMPOEDIExportModel();
            purchaseExportModel.PONumber = purchaseOrder.ClientLookupId;
            purchaseExportModel.PODate = purchaseOrder.CreateDate.ToString("yyyyMMdd");
            purchaseExportModel.TermsDescription = purchaseOrder.TermDescription;
            purchaseExportModel.CarrierName = purchaseOrder.Carrier;
            purchaseExportModel.ShipToAttn = purchaseOrder.Attention;
            purchaseExportModel.ShipToLocation = purchaseOrder.Shipto_ClientLookUpId;
            purchaseExportModel.ShipToStreet1 = purchaseOrder.ShipToAddress1;
            purchaseExportModel.ShipToStreet2 = purchaseOrder.ShipToAddress2;
            purchaseExportModel.ShipToStreet3 = purchaseOrder.ShipToAddress3;
            purchaseExportModel.ShipToCity = purchaseOrder.ShipToAddressCity;
            purchaseExportModel.ShipToState = purchaseOrder.ShipToAddressState;
            purchaseExportModel.ShipToPostalCode = purchaseOrder.ShipToAddressPostCode;
            purchaseExportModel.ShipToCountry = purchaseOrder.ShipToAddressCountry;
            purchaseExportModel.VendorNumber = purchaseOrder.VendorClientLookupId;
            purchaseExportModel.VendorName = purchaseOrder.VendorName;
            purchaseExportModel.VendorStreet1 = purchaseOrder.VendorAddress1;
            purchaseExportModel.VendorStreet2 = purchaseOrder.VendorAddress2;
            purchaseExportModel.VendorStreet3 = purchaseOrder.VendorAddress3;
            purchaseExportModel.VendorCity = purchaseOrder.VendorAddressCity;
            purchaseExportModel.VendorState = purchaseOrder.VendorAddressState;
            purchaseExportModel.VendorPostalCode = purchaseOrder.VendorAddressPostCode;
            purchaseExportModel.VendorCountry = purchaseOrder.VendorAddressCountry;
            purchaseExportModel.BuyerName = purchaseOrder.Buyer_PersonnelName;
            purchaseExportModel.BuyerPhoneNumber = purchaseOrder.Buyer_Phone;
            purchaseExportModel.POTotalCost = purchaseOrder.TotalCost;
            Notes Header = new Notes()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                ObjectId = PurchaseOrderId,
                TableName = AttachmentTableConstant.PurchaseOrder
            };
            List<Notes> HeaderList = Header.RetrieveByObjectId(_userData.DatabaseKey, _userData.Site.TimeZone);
            foreach (var item in HeaderList)
            {
                HeaderComment itemModel = new HeaderComment()
                {
                    PONumber = purchaseOrder.ClientLookupId,
                    Notes = item.Content
                };
                purchaseExportModel.HeaderComment.Add(itemModel);
            }
            PurchaseOrderLineItem purchaseOrderLineItem = new PurchaseOrderLineItem()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.DatabaseKey.Client.SiteId,
                PurchaseOrderId = purchaseOrder.PurchaseOrderId
            };
            List<PurchaseOrderLineItem> plist = PurchaseOrderLineItem.PurchaseOrderLineItemRetrieveByPurchaseOrderId_V2(_userData.DatabaseKey, purchaseOrderLineItem);
            // Add Items
            foreach (var lineItem in plist)
            {
                Item itemModel = new Item
                {
                    PONumber = purchaseOrder.ClientLookupId,
                    LineNumber = lineItem.LineNumber,
                    Quantity = lineItem.OrderQuantity,
                    UnitOfMeasurement = lineItem.UnitOfMeasure,
                    UnitPrice = lineItem.UnitCost,
                    BuyerPartNumber = lineItem.PartClientLookupId,
                    VendorPartNumber = ""
                };
                if (lineItem.EstimatedDelivery != null && lineItem.EstimatedDelivery != DateTime.MinValue)
                {
                    itemModel.DeliveryDate = lineItem.EstimatedDelivery?.ToUserTimeZone(_userData.Site.TimeZone).ToString("yyyyMMdd");
                }
                else
                {
                    itemModel.DeliveryDate = string.Empty;
                }
                if (lineItem.RequiredDate != null && lineItem.RequiredDate != DateTime.MinValue.ToString())
                {
                    itemModel.RequiredDate = lineItem.RequiredDate.ToDateTime().ToUserTimeZone(_userData.Site.TimeZone).ToString("yyyyMMdd");
                }
                else
                {
                    itemModel.RequiredDate = string.Empty;
                }
                // Add Item Comments
                ItemComment itemComment = new ItemComment
                {
                    PONumber = purchaseOrder.ClientLookupId,
                    Description = lineItem.Description
                };
                itemModel.ItemsComments.Add(itemComment);
                // Add Item Summaries
                ItemSummary itemSummary = new ItemSummary
                {
                    PONumber = purchaseOrder.ClientLookupId,
                    LineNumber = lineItem.LineNumber,
                    Quantity = lineItem.OrderQuantity,
                    UnitOfMeasurement = lineItem.UnitOfMeasure
                };
                if (lineItem.EstimatedDelivery != null && lineItem.EstimatedDelivery != DateTime.MinValue)
                {
                    itemSummary.DeliveryDate = lineItem.EstimatedDelivery?.ToUserTimeZone(_userData.Site.TimeZone).ToString("yyyyMMdd");
                }
                else
                {
                    itemSummary.DeliveryDate = string.Empty;
                }
                itemModel.ItemsSummaries = itemSummary;
                purchaseExportModel.Items.Add(itemModel);
            }
            return purchaseExportModel;
        }
        public string ConvertToDataFile(EPMPOEDIExportModel ExportModel)
        {
            string txt = string.Empty;
            string filePath = string.Empty;
            dirpath = System.Configuration.ConfigurationManager.AppSettings["LocalExportDirectory"].ToString().Trim()
                    + System.Configuration.ConfigurationManager.AppSettings[ApiConstants.EPMEDIPOExport].ToString().Trim();
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(dirpath));
            }
            string cFileExt = (SFTPCred.filesEncrypted == true) ? ".txt.pgp" : ".txt";
            string cFileName = "PO_" + ExportModel.PONumber + cFileExt;
            string cOutFile = Path.Combine(dirpath, cFileName);
            TextWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(cOutFile));
            string delimiter = SFTPCred.delimiter;
            StringBuilder sb = new StringBuilder();

            // Convert Header Section
            sb.AppendLine($"{ExportModel.HeaderQualifier}{delimiter}{ExportModel.PONumber}{delimiter}{ExportModel.PODate}{delimiter}{ExportModel.TermsDescription}{delimiter}{ExportModel.CarrierDetails}{delimiter}{ExportModel.CarrierName}{delimiter}{ExportModel.ShipToAttn}{delimiter}{ExportModel.ShipToLocation}{delimiter}{ExportModel.ShipToStreet1}{delimiter}{ExportModel.ShipToStreet2}{delimiter}{ExportModel.ShipToStreet3}{delimiter}{ExportModel.ShipToCity}{delimiter}{ExportModel.ShipToState}{delimiter}{ExportModel.ShipToPostalCode}{delimiter}{ExportModel.ShipToCountry}{delimiter}{ExportModel.VendorNumber}{delimiter}{ExportModel.VendorName}{delimiter}{ExportModel.VendorStreet1}{delimiter}{ExportModel.VendorStreet2}{delimiter}{ExportModel.VendorStreet3}{delimiter}{ExportModel.VendorCity}{delimiter}{ExportModel.VendorState}{delimiter}{ExportModel.VendorPostalCode}{delimiter}{ExportModel.VendorCountry}{delimiter}{ExportModel.BuyerName}{delimiter}{ExportModel.BuyerPhoneNumber}{delimiter}{ExportModel.BuyerFaxNumber}{delimiter}{ExportModel.POTotalCost}{delimiter}");

            // Convert Header Comment Section
            foreach (var headerComment in ExportModel.HeaderComment)
            {
                sb.AppendLine($"{headerComment.Qualifier}{delimiter}{headerComment.PONumber}{delimiter}{headerComment.Notes}{delimiter}");
            }
            sb.AppendLine($"{"HC"}{delimiter}{ExportModel.PONumber}{delimiter}{"PACKING LIST MUST BE ATTACHED TO LAST SKID OR CARTON OF SHIPMENT"}{delimiter}");

            // Convert Items Section
            foreach (var item in ExportModel.Items)
            {
                sb.AppendLine($"{item.Qualifier}{delimiter}{item.PONumber}{delimiter}{item.LineNumber}{delimiter}{item.Quantity}{delimiter}{item.UnitOfMeasurement}{delimiter}{item.UnitPrice}{delimiter}{item.BuyerPartNumber}{delimiter}{item.VendorPartNumber}{delimiter}{item.DeliveryDate}{delimiter}{item.RequiredDate}{delimiter}");

                // Convert Items Comment Section
                foreach (var itemComment in item.ItemsComments)
                {
                    sb.AppendLine($"{itemComment.Qualifier}{delimiter}{itemComment.PONumber}{delimiter}{itemComment.Description}{delimiter}");
                }
                // Convert Items Summary 
                sb.AppendLine($"{item.ItemsSummaries.Qualifier}{delimiter}{item.ItemsSummaries.PONumber}{delimiter}{item.ItemsSummaries.LineNumber}{delimiter}{item.ItemsSummaries.Quantity}{delimiter}{item.ItemsSummaries.UnitOfMeasurement}{delimiter}{item.ItemsSummaries.DeliveryDate}{delimiter}");
            }

            txt = sb.ToString();
            sw.Write(txt);
            sw.Close();
            return cOutFile;
        }
        public string ExportToSFTP(string inputFile, FileTypeEnum fileType)
        {
            string path = UploadToSFTP(inputFile, Path.GetFileName(inputFile), fileType, ApiConstants.EPMEDIPOExport);
            return path;
        }
        #endregion
        public bool CheckLoginSession(string LoginSessionID)
        {
            Guid LogsessionId = Guid.Empty;
            LogsessionId = new Guid(LoginSessionID);
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            this._userData = new UserData() { SessionId = LogsessionId, WebSite = WebSiteEnum.Client };
            this._userData.Retrieve(dbKey);

            Authentication auth = new Authentication() { UserData = this._userData };
            auth.UserData.LoginAuditing.CreateDate = DateTime.Now;
            auth.VerifyCurrentUser();
            return auth.IsAuthenticated;
        }
    }
}