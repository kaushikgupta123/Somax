using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models;
using Client.Models.Parts;
using Client.Models.PunchoutModel;
using Client.Models.PunchoutOrderExport;
using Client.Models.PunchoutOrderMessageResponse;
using Client.Models.PurchaseOrder;

using Common.Constants;
using Common.Enumerations;
using Common.Extensions;

using DataContracts;

using Notification;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Client.Localization;
using Client.Models.PurchaseOrder.UIConfiguration;
using DocumentFormat.OpenXml.EMMA;
using Rotativa;
using System.Web.Mvc;
using Database.Business;
using DevExpress.Xpo.DB.Helpers;
using Org.BouncyCastle.Asn1.X509;
using Pipelines.Sockets.Unofficial.Buffers;

namespace Client.BusinessWrapper.Purchase_Order
{
    public class PurchaseOrderWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        string BodyHeader = string.Empty;
        string BodyContent = string.Empty;
        string FooterSignature = string.Empty;
        public POPrintReceiptModel PrintTotalModel { get; set; }
        public PurchaseOrderWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Get
        public List<PurchaseOrderModel> GetPurchaseOrderChunkList(int CustomQueryDisplayId, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null,
            int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string ClientLookupId = "", string Status = "", string VendorClientLookupId = "", string VendorName = "", DateTime? StartCreateDate = null, DateTime? EndCreateDate = null,
            string Attention = "", string VendorPhoneNumber = "", DateTime? StartCompleteDate = null, DateTime? EndCompleteDate = null, string Reason = "", string Buyer_PersonnelName = "", string TotalCost = "", string FilterValue = "", string SearchText = "", DateTime? Required = null)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            PurchaseOrderModel poModel;
            List<PurchaseOrderModel> PurchaseRequestModelList = new List<PurchaseOrderModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<string> StatusList = new List<string>();
            List<string> VendorList = new List<string>();
            purchaseOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            purchaseOrder.CustomQueryDisplayId = CustomQueryDisplayId;
            //V2-347
            purchaseOrder.CompleteStartDateVw = CompleteStartDateVw.HasValue ? CompleteStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseOrder.CompleteEndDateVw = CompleteEndDateVw.HasValue ? CompleteEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            //V2-347

            //v2-364
            purchaseOrder.CreateStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseOrder.CreateEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            //v2-364
            purchaseOrder.orderbyColumn = orderbycol;
            purchaseOrder.orderBy = orderDir;
            purchaseOrder.offset1 = Convert.ToString(skip);
            purchaseOrder.nextrow = Convert.ToString(length);

            purchaseOrder.ClientLookupId = ClientLookupId;
            purchaseOrder.Status = Status;
            purchaseOrder.VendorClientLookupId = VendorClientLookupId;
            purchaseOrder.VendorName = VendorName;
            //V2-347
            //purchaseOrder.CreatedDate = CreateDate.HasValue? CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseOrder.StartCreateDate = StartCreateDate.HasValue ? StartCreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseOrder.EndCreateDate = EndCreateDate.HasValue ? EndCreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            //V2-347

            purchaseOrder.Attention = Attention;
            purchaseOrder.VendorPhoneNumber = VendorPhoneNumber;
            //V2-347
            //purchaseOrder.CompleteDT = CompleteDate.HasValue ? CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseOrder.StartCompleteDate = StartCompleteDate.HasValue ? StartCompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseOrder.EndCompleteDate = EndCompleteDate.HasValue ? EndCompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            //V2-347
            purchaseOrder.Reason = Reason;
            purchaseOrder.Buyer_PersonnelName = Buyer_PersonnelName;
            purchaseOrder.TtlCost = (string.IsNullOrEmpty(TotalCost)) ? string.Empty : TotalCost;
            purchaseOrder.FilterValue = Convert.ToInt64(FilterValue);
            purchaseOrder.SearchText = SearchText;
            purchaseOrder.Reason = Reason;
            purchaseOrder.Status = Status;
            purchaseOrder.Required = Required; //V2-1171
            List<PurchaseOrder> purchaseOrderList = purchaseOrder.RetrieveChunkSearch(this.userData.DatabaseKey);
            if (purchaseOrderList != null)
            {
                foreach (var po in purchaseOrderList)
                {
                    poModel = new PurchaseOrderModel();
                    poModel.PurchaseOrderId = po.PurchaseOrderId;
                    poModel.ClientLookupId = po.ClientLookupId;
                    poModel.Status = po.Status;
                    poModel.VendorClientLookupId = po.VendorClientLookupId;
                    poModel.VendorName = po.VendorName;
                    if (po.CreateDate != null && po.CreateDate == default(DateTime))
                    {
                        poModel.CreateDate = null;
                    }
                    else
                    {
                        poModel.CreateDate = po.CreateDate;
                    }
                    poModel.Attention = po.Attention;
                    poModel.VendorPhoneNumber = po.VendorPhoneNumber;
                    if (po.CompleteDate != null && po.CompleteDate == default(DateTime))
                    {
                        poModel.CompleteDate = null;
                    }
                    else
                    {
                        poModel.CompleteDate = po.CompleteDate;
                    }
                    poModel.Reason = po.Reason;
                    poModel.Buyer_PersonnelName = po.Buyer_PersonnelName;
                    poModel.TotalCost = Math.Round(po.TotalCost, 2);
                    poModel.ChildCount = po.ChildCount;
                    poModel.TotalCount = po.TotalCount;
                    #region V2-1171
                    if (po.Required != null && po.Required == default(DateTime))
                    {
                        poModel.Required = null;
                    }
                    else
                    {
                        poModel.Required = po.Required;
                    }
                    #endregion
                    PurchaseRequestModelList.Add(poModel);
                }
            }
            return PurchaseRequestModelList;
        }
        public List<PurchaseOrderModel> PopulatePODetails(int CustomQueryDisplayId)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            PurchaseOrderModel poModel;
            List<PurchaseOrderModel> poModelList = new List<PurchaseOrderModel>();
            purchaseOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            purchaseOrder.CustomQueryDisplayId = CustomQueryDisplayId;
            List<PurchaseOrder> poList = purchaseOrder.RetrieveAllForSearchNew(this.userData.DatabaseKey, 0);
            foreach (var po in poList)
            {
                poModel = new PurchaseOrderModel();
                poModel.PurchaseOrderId = po.PurchaseOrderId;
                poModel.ClientLookupId = po.ClientLookupId;
                poModel.Status = po.Status;
                poModel.VendorClientLookupId = po.VendorClientLookupId;
                poModel.VendorName = po.VendorName;
                if (po.CreateDate != null && po.CreateDate == default(DateTime))
                {
                    poModel.CreateDate = null;
                }
                else
                {
                    poModel.CreateDate = po.CreateDate;
                }
                poModel.Attention = po.Attention;
                poModel.VendorPhoneNumber = po.VendorPhoneNumber;
                if (po.CompleteDate != null && po.CompleteDate == default(DateTime))
                {
                    poModel.CompleteDate = null;
                }
                else
                {
                    poModel.CompleteDate = po.CompleteDate;
                }
                poModel.Reason = po.Reason;
                poModel.Buyer_PersonnelName = po.Buyer_PersonnelName;
                poModel.TotalCost = Math.Round(po.TotalCost, 2);
                poModelList.Add(poModel);
            }
            return poModelList;
        }



        #region V2-331
        public List<PurchaseOrderModel> GetPORChunkList(int CustomQueryDisplayId,
           int skip = 0, int length = 0, string orderbycol = "", string orderDir = "",
           string ClientLookupId = "", string Status = "",
           string VendorClientLookupId = "", string VendorName = "", DateTime? CreateDate = null,
           string Attention = "", string SearchText = "")
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            PurchaseOrderModel poModel;
            List<PurchaseOrderModel> PurchaseRequestModelList = new List<PurchaseOrderModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<string> StatusList = new List<string>();
            List<string> VendorList = new List<string>();
            purchaseOrder.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseOrder.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            purchaseOrder.CustomQueryDisplayId = CustomQueryDisplayId;
            purchaseOrder.orderbyColumn = orderbycol;
            purchaseOrder.orderBy = orderDir;
            purchaseOrder.offset1 = Convert.ToString(skip);
            purchaseOrder.nextrow = Convert.ToString(length);
            purchaseOrder.ClientLookupId = ClientLookupId;
            purchaseOrder.Status = Status;
            purchaseOrder.VendorClientLookupId = VendorClientLookupId;
            purchaseOrder.VendorName = VendorName;
            purchaseOrder.strCreatedDate = CreateDate.HasValue ? CreateDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseOrder.Attention = Attention;
            purchaseOrder.SearchText = SearchText;
            purchaseOrder.Status = Status;
            List<PurchaseOrder> purchaseOrderList = purchaseOrder.RetrievePOReceiptChunkSearch(this.userData.DatabaseKey);

            if (purchaseOrderList != null)
            {
                foreach (var po in purchaseOrderList)
                {
                    poModel = new PurchaseOrderModel();
                    poModel.PurchaseOrderId = po.PurchaseOrderId;
                    poModel.ClientLookupId = po.ClientLookupId;
                    poModel.Status = po.Status;
                    poModel.VendorClientLookupId = po.VendorClientLookupId;
                    poModel.VendorName = po.VendorName;
                    if (po.CreateDate != null && po.CreateDate == default(DateTime))
                    {
                        poModel.CreateDate = null;
                    }
                    else
                    {
                        poModel.CreateDate = po.CreateDate;
                    }
                    poModel.Attention = po.Attention;
                    poModel.VendorPhoneNumber = po.VendorPhoneNumber;
                    if (po.CompleteDate != null && po.CompleteDate == default(DateTime))
                    {
                        poModel.CompleteDate = null;
                    }
                    else
                    {
                        poModel.CompleteDate = po.CompleteDate;
                    }
                    poModel.Reason = po.Reason;
                    poModel.Buyer_PersonnelName = po.Buyer_PersonnelName;
                    poModel.TotalCost = Math.Round(po.TotalCost, 2);
                    poModel.TotalCount = po.TotalCount;
                    PurchaseRequestModelList.Add(poModel);
                }
            }
            return PurchaseRequestModelList;
        }

        #endregion

        public PurchaseOrderModel getPurchaseOderDetailsById(long _purchaseOrderId)
        {
            PurchaseOrderModel purchaseOrderModel = new PurchaseOrderModel();
            PurchaseOrder po = new PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = _purchaseOrderId
            };
            po.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            purchaseOrderModel = initializeControls(po);
            return purchaseOrderModel;
        }
        public PurchaseOrderModel getPurchaseOderPrintDetailsById(long _purchaseOrderId)
        {
            PurchaseOrderModel purchaseOrderModel = new PurchaseOrderModel();
            PurchaseOrder po = new PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = _purchaseOrderId
            };
            po.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            purchaseOrderModel = initializeControls(po);
            if (string.IsNullOrEmpty(purchaseOrderModel.VendorAddress2))
            {
                if (string.IsNullOrEmpty(purchaseOrderModel.VendorAddress3))
                {
                    purchaseOrderModel.VendorAddress2 = purchaseOrderModel.VendorAddressCity + ", " + purchaseOrderModel.VendorAddressState + "  " + purchaseOrderModel.VendorAddressPostCode;
                    if (!string.IsNullOrEmpty(purchaseOrderModel.VendorAddressCountry))
                    {
                        purchaseOrderModel.VendorAddress3 = purchaseOrderModel.VendorAddressCountry;
                        purchaseOrderModel.VendorAddressCity = purchaseOrderModel.VendorPhoneNumber;
                        purchaseOrderModel.VendorAddressCountry = string.Empty;
                        purchaseOrderModel.VendorPhoneNumber = string.Empty;
                    }
                    else
                    {
                        purchaseOrderModel.VendorAddress3 = purchaseOrderModel.VendorPhoneNumber;
                        purchaseOrderModel.VendorAddressCity = string.Empty;
                        purchaseOrderModel.VendorAddressCountry = string.Empty;
                        purchaseOrderModel.VendorPhoneNumber = string.Empty;
                    }
                }
                else
                {
                    purchaseOrderModel.VendorAddress2 = purchaseOrderModel.VendorAddress3;
                    purchaseOrderModel.VendorAddress3 = purchaseOrderModel.VendorAddressCity + ", " + purchaseOrderModel.VendorAddressState + "  " + purchaseOrderModel.VendorAddressPostCode;
                    purchaseOrderModel.VendorAddressCity = purchaseOrderModel.VendorAddressCountry;
                    purchaseOrderModel.VendorAddressCountry = purchaseOrderModel.VendorPhoneNumber;
                    purchaseOrderModel.VendorPhoneNumber = string.Empty;
                }
            }
            else
            {
                purchaseOrderModel.VendorAddress2 = purchaseOrderModel.VendorAddress2;
                if (string.IsNullOrEmpty(purchaseOrderModel.VendorAddress3))
                {
                    purchaseOrderModel.VendorAddress3 = purchaseOrderModel.VendorAddressCity + ", " + purchaseOrderModel.VendorAddressState + "  " + purchaseOrderModel.VendorAddressPostCode;
                    if (string.IsNullOrEmpty(purchaseOrderModel.VendorAddressCountry))
                    {
                        purchaseOrderModel.VendorAddressCity = purchaseOrderModel.VendorPhoneNumber;
                        purchaseOrderModel.VendorAddressCountry = string.Empty;
                        purchaseOrderModel.VendorPhoneNumber = string.Empty;
                    }
                    else
                    {
                        purchaseOrderModel.VendorAddressCity = purchaseOrderModel.VendorAddressCountry;
                        purchaseOrderModel.VendorAddressCountry = purchaseOrderModel.VendorPhoneNumber;
                        purchaseOrderModel.VendorPhoneNumber = string.Empty;
                    }
                }
                else
                {
                    purchaseOrderModel.VendorAddress3 = purchaseOrderModel.VendorAddress3;
                    purchaseOrderModel.VendorAddressCity = purchaseOrderModel.VendorAddressCity + ", " + purchaseOrderModel.VendorAddressState + "  " + purchaseOrderModel.VendorAddressPostCode;
                    if (string.IsNullOrEmpty(purchaseOrderModel.VendorAddressCountry))
                    {
                        purchaseOrderModel.VendorAddressCountry = purchaseOrderModel.VendorPhoneNumber;
                        purchaseOrderModel.VendorPhoneNumber = string.Empty;
                    }
                    else
                    {
                        purchaseOrderModel.VendorAddressCountry = purchaseOrderModel.VendorAddressCountry;
                        purchaseOrderModel.VendorPhoneNumber = purchaseOrderModel.VendorPhoneNumber;
                    }
                }
            }
            if (string.IsNullOrEmpty(purchaseOrderModel.SiteAddress2))
            {
                if (string.IsNullOrEmpty(purchaseOrderModel.SiteAddress3))
                {
                    purchaseOrderModel.SiteAddress2 = purchaseOrderModel.SiteAddressCity + ", " + purchaseOrderModel.SiteAddressState + "  " + purchaseOrderModel.SiteAddressPostCode;
                    purchaseOrderModel.SiteAddress3 = purchaseOrderModel.SiteAddressCountry;
                    purchaseOrderModel.SiteAddressCity = string.Empty;
                    purchaseOrderModel.SiteAddressCountry = string.Empty;
                }
                else
                {
                    purchaseOrderModel.SiteAddress2 = purchaseOrderModel.SiteAddress3;
                    purchaseOrderModel.SiteAddress3 = purchaseOrderModel.SiteAddressCity + ", " + purchaseOrderModel.SiteAddressState + "  " + purchaseOrderModel.SiteAddressPostCode;
                    purchaseOrderModel.SiteAddressCity = purchaseOrderModel.SiteAddressCountry;
                    purchaseOrderModel.SiteAddressCountry = string.Empty;
                }
            }
            else
            {
                purchaseOrderModel.SiteAddress2 = purchaseOrderModel.SiteAddress2;
                if (string.IsNullOrEmpty(purchaseOrderModel.SiteAddress3))
                {
                    purchaseOrderModel.SiteAddress3 = purchaseOrderModel.SiteAddressCity + ", " + purchaseOrderModel.SiteAddressState + "  " + purchaseOrderModel.SiteAddressPostCode;
                    purchaseOrderModel.SiteAddressCity = purchaseOrderModel.SiteAddressCountry;
                    purchaseOrderModel.SiteAddressCountry = string.Empty;
                }
                else
                {
                    purchaseOrderModel.SiteAddress3 = purchaseOrderModel.SiteAddress3;
                    purchaseOrderModel.SiteAddressCity = purchaseOrderModel.SiteAddressCity + ", " + purchaseOrderModel.SiteAddressState + "  " + purchaseOrderModel.SiteAddressPostCode;
                    purchaseOrderModel.SiteAddressCountry = purchaseOrderModel.SiteAddressCountry;
                }
            }
            if (string.IsNullOrEmpty(purchaseOrderModel.SiteBillToAddress2))
            {
                if (string.IsNullOrEmpty(purchaseOrderModel.SiteBillToAddress3))
                {
                    purchaseOrderModel.SiteBillToAddress2 = purchaseOrderModel.SiteBillToAddressCity + ", " + purchaseOrderModel.SiteBillToAddressState + "  " + purchaseOrderModel.SiteBillToAddressPostCode;
                    purchaseOrderModel.SiteBillToAddress3 = purchaseOrderModel.SiteBillToAddressCountry;
                    purchaseOrderModel.SiteBillToAddressCity = string.Empty;
                    purchaseOrderModel.SiteBillToAddressCountry = string.Empty;
                }
                else
                {
                    purchaseOrderModel.SiteBillToAddress2 = purchaseOrderModel.SiteBillToAddress3;
                    purchaseOrderModel.SiteBillToAddress3 = purchaseOrderModel.SiteBillToAddressCity + ", " + purchaseOrderModel.SiteBillToAddressState + "  " + purchaseOrderModel.SiteBillToAddressPostCode;
                    purchaseOrderModel.SiteBillToAddressCity = purchaseOrderModel.SiteBillToAddressCountry;
                    purchaseOrderModel.SiteBillToAddressCountry = string.Empty;
                }
            }
            else
            {
                purchaseOrderModel.SiteBillToAddress2 = purchaseOrderModel.SiteBillToAddress2;
                if (string.IsNullOrEmpty(purchaseOrderModel.SiteBillToAddress3))
                {
                    purchaseOrderModel.SiteBillToAddress3 = purchaseOrderModel.SiteBillToAddressCity + ", " + purchaseOrderModel.SiteBillToAddressState + "  " + purchaseOrderModel.SiteBillToAddressPostCode;
                    purchaseOrderModel.SiteBillToAddressCity = purchaseOrderModel.SiteBillToAddressCountry;
                    purchaseOrderModel.SiteBillToAddressCountry = string.Empty;
                }
                else
                {
                    purchaseOrderModel.SiteBillToAddress3 = purchaseOrderModel.SiteBillToAddress3;
                    purchaseOrderModel.SiteBillToAddressCity = purchaseOrderModel.SiteBillToAddressCity + ", " + purchaseOrderModel.SiteBillToAddressState + "  " + purchaseOrderModel.SiteBillToAddressPostCode;
                    purchaseOrderModel.SiteBillToAddressCountry = purchaseOrderModel.SiteBillToAddressCountry;
                }
            }
            return purchaseOrderModel;
        }
        public PurchaseOrderModel initializeControls(PurchaseOrder po)
        {
            PurchaseOrderModel purchaseOrderModel = new PurchaseOrderModel();
            purchaseOrderModel.PurchaseOrderId = po.PurchaseOrderId;
            purchaseOrderModel.ClientLookupId = po.ClientLookupId;
            purchaseOrderModel.Attention = po.Attention;
            purchaseOrderModel.Buyer_PersonnelId = po.Buyer_PersonnelId;
            purchaseOrderModel.Carrier = po.Carrier;
            purchaseOrderModel.CompleteBy_PersonnelId = po.CompleteBy_PersonnelId;
            purchaseOrderModel.Required = po.Required;
            purchaseOrderModel.CompleteDate = po.CompleteDate;
            purchaseOrderModel.Creator_PersonnelId = po.Creator_PersonnelId;
            purchaseOrderModel.FOB = po.FOB;
            purchaseOrderModel.Status = po.Status;
            purchaseOrderModel.Terms = po.Terms;
            purchaseOrderModel.VendorId = po.VendorId;
            purchaseOrderModel.VoidBy_PersonnelId = po.VoidBy_PersonnelId;
            purchaseOrderModel.VoidDate = po.VoidDate;
            purchaseOrderModel.VoidReason = po.VoidReason;
            purchaseOrderModel.Reason = po.Reason;
            purchaseOrderModel.MessageToVendor = po.MessageToVendor;
            purchaseOrderModel.ExPurchaseOrderId = po.ExPurchaseOrderId;
            purchaseOrderModel.ExPurchaseRequest = po.ExPurchaseRequest;
            purchaseOrderModel.Currency = po.Currency;
            purchaseOrderModel.Revision = po.Revision;
            purchaseOrderModel.PaymentTerms = po.PaymentTerms;
            purchaseOrderModel.UpdateIndex = po.UpdateIndex;
            purchaseOrderModel.CreateDate = po.CreateDate;
            purchaseOrderModel.VendorName = po.VendorName;
            purchaseOrderModel.VendorPhoneNumber = po.VendorPhoneNumber;
            purchaseOrderModel.VendorCustomerAccount = po.VendorCustomerAccount;
            purchaseOrderModel.VendorEmailAddress = po.VendorEmailAddress;
            purchaseOrderModel.VendorClientLookupId = po.VendorClientLookupId;
            purchaseOrderModel.Creator_PersonnelName = po.Creator_PersonnelName;
            purchaseOrderModel.Completed_PersonnelName = po.Completed_PersonnelName;
            purchaseOrderModel.Buyer_PersonnelName = po.Buyer_PersonnelName;
            purchaseOrderModel.CountLineItem = po.CountLineItem;
            purchaseOrderModel.TotalCost = po.TotalCost;
            purchaseOrderModel.VendorAddress1 = po.VendorAddress1;
            purchaseOrderModel.VendorAddress2 = po.VendorAddress2;
            purchaseOrderModel.VendorAddress3 = po.VendorAddress3;
            purchaseOrderModel.VendorAddressCity = po.VendorAddressCity;
            purchaseOrderModel.VendorAddressCityForPrint = po.VendorAddressCity + ", " + po.VendorAddressState + "  " + po.VendorAddressPostCode;
            purchaseOrderModel.VendorAddressCountry = po.VendorAddressCountry;
            purchaseOrderModel.VendorAddressPostCode = po.VendorAddressPostCode;
            purchaseOrderModel.VendorAddressState = po.VendorAddressState;
            purchaseOrderModel.VendorCustomerAccount = po.VendorCustomerAccount;
            purchaseOrderModel.SiteName = po.SiteName;
            purchaseOrderModel.SiteAddress1 = po.SiteAddress1;
            purchaseOrderModel.SiteAddress2 = po.SiteAddress2;
            purchaseOrderModel.SiteAddress3 = po.SiteAddress3;
            purchaseOrderModel.SiteAddressCity = po.SiteAddressCity;
            purchaseOrderModel.SiteAddressCityForPrint = po.SiteAddressCity + ", " + po.SiteAddressState + "  " + po.SiteAddressPostCode;
            purchaseOrderModel.SiteAddressCountry = po.SiteAddressCountry;
            purchaseOrderModel.SiteAddressPostCode = po.SiteAddressPostCode;
            purchaseOrderModel.SiteAddressState = po.SiteAddressState;
            purchaseOrderModel.SiteBillToName = po.SiteBillToName;
            purchaseOrderModel.SiteBillToAddress1 = po.SiteBillToAddress1;
            purchaseOrderModel.SiteBillToAddress2 = po.SiteBillToAddress2;
            purchaseOrderModel.SiteBillToAddress3 = po.SiteBillToAddress3;
            purchaseOrderModel.SiteBillToAddressCity = po.SiteBillToAddressCity;
            purchaseOrderModel.SiteBillToAddressCityForPrint = po.SiteBillToAddressCity + ", " + po.SiteBillToAddressState + "  " + po.SiteBillToAddressPostCode;
            purchaseOrderModel.SiteBillToAddressCountry = po.SiteBillToAddressCountry;
            purchaseOrderModel.SiteBillToAddressPostCode = po.SiteBillToAddressPostCode;
            purchaseOrderModel.SiteBillToAddressState = po.SiteBillToAddressState;
            purchaseOrderModel.SiteBillToComment = po.SiteBillToComment;
            purchaseOrderModel.ModifyBy = po.ModifyBy;
            purchaseOrderModel.ModifyDate = po.ModifyDate;
            purchaseOrderModel.CreateBy = po.CreateBy;
            purchaseOrderModel.PersonnelEmail = po.PersonnelEmail;
            purchaseOrderModel.ToEmailId = po.VendorEmailAddress;
            purchaseOrderModel.CcEmailId = this.userData.DatabaseKey.User.Email;
            purchaseOrderModel.IsExternal = po.IsExternal;
            purchaseOrderModel.IsPunchout = po.IsPunchout;
            purchaseOrderModel.SentOrderRequest = po.SentOrderRequest;
            purchaseOrderModel.SiteId = po.SiteId;
            purchaseOrderModel.ClientId = po.ClientId;
            purchaseOrderModel.PrintDate = DateTime.UtcNow.ToUserTimeZone(userData.Site.TimeZone).ToString("g");
            purchaseOrderModel.StoreroomId = po.StoreroomId;
            return purchaseOrderModel;
        }
        public bool PoVendorNoEmail(long VendorId)
        {
            bool result = false;
            Vendor ve = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = VendorId
            };
            ve.Retrieve(userData.DatabaseKey);
            if (ve.VendorMasterId > 0)
            {
                Int64 vendormasterid = ve.VendorMasterId;
                if (vendormasterid > 0)
                {
                    VendorMaster vm = new VendorMaster()
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        VendorMasterId = vendormasterid
                    };
                    vm.Retrieve(userData.DatabaseKey);
                    List<long> noemaillist = new List<long>() { 329, 586, 3335, 4760, 5436 };
                    if (!noemaillist.Contains(vm.ExVendorId))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public long PoListCount(long PurchaseOrderId)
        {
            DataContracts.PurchaseRequest pr = new DataContracts.PurchaseRequest();
            List<DataContracts.PurchaseRequest> list_pr = new List<DataContracts.PurchaseRequest>();
            pr.ClientId = this.userData.DatabaseKey.Client.ClientId;
            pr.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            pr.PurchaseOrderId = PurchaseOrderId;
            list_pr = pr.RetrieveForInformation(userData.DatabaseKey);
            return list_pr.Count;
        }
        public PurchaseRequestModel PopulateRequestDetails(long PurchaseOrderId, ref int prCount)
        {
            PurchaseRequestModel prModel = new PurchaseRequestModel();
            PurchaseRequest pr = new PurchaseRequest();
            List<PurchaseRequest> list_pr = new List<PurchaseRequest>();
            pr.ClientId = this.userData.DatabaseKey.Client.ClientId;
            pr.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            pr.PurchaseOrderId = PurchaseOrderId;
            list_pr = pr.RetrieveForInformation(userData.DatabaseKey);
            prCount = list_pr.Count;
            foreach (var PR in list_pr)
            {
                prModel.PurchaseRequestId = PR.PurchaseRequestId;
                prModel.PurchaseOrderId = PR.PurchaseOrderId;
                prModel.ClientLookupId = PR.ClientLookupId;
                prModel.Creator_PersonnelName = PR.Creator_PersonnelName;
                prModel.Approved_PersonnelName = PR.Approved_PersonnelName;
                prModel.Processed_PersonnelName = PR.Processed_PersonnelName;
                if (PR.CreateDate != null && PR.CreateDate == default(DateTime))
                {
                    prModel.CreateDates = string.Empty;
                }
                else
                {
                    prModel.CreateDates = PR.CreateDate.ConvertFromUTCToUser(userData.DatabaseKey.User.TimeZone).ToShortDateString();
                }
                if (PR.Approved_Date != null && PR.Approved_Date == default(DateTime))
                {
                    prModel.ApprovedDate = string.Empty;
                }
                else
                {
                    prModel.ApprovedDate = PR.Approved_Date.ToUserTimeZone(userData.DatabaseKey.User.TimeZone).ToShortDate();
                }
                if (PR.Processed_Date != null && PR.Processed_Date == default(DateTime))
                {
                    prModel.ProcessedDate = string.Empty;
                }
                else
                {
                    prModel.ProcessedDate = PR.Processed_Date.ConvertFromUTCToUser(userData.DatabaseKey.User.TimeZone).ToShortDateString();
                }

                prModel.AutoGenerated = PR.AutoGenerated;
            }
            return prModel;
        }
        #endregion      
        #region Add-Edit
        public PurchaseOrder AddpurchaseOrder(PurchaseOrderModel pOModel)
        {
            PurchaseOrder purchaseorder = new PurchaseOrder();
            WorkFlowLog workflowlog = new WorkFlowLog();
            CustomSequentialId custid = new CustomSequentialId();
            custid.KeyList = AutoGenerateKey.PO_ANNUAL;
            var custList = custid.RetrieveByClientIdandSiteIdandKey_V2(userData.DatabaseKey);
            string POPrefix = custList != null ? custList.Where(x => x.Key == AutoGenerateKey.PO_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : "";
            //string Vendor_ClientLookupId = pOModel.VendorClientLookupId;
            //Vendor vendor = new Vendor { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = Vendor_ClientLookupId };
            //List<Vendor> vlist = vendor.RetrieveBySiteIdAndClientLookUpId(_dbKey);
            purchaseorder.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            purchaseorder.ClientId = this.userData.DatabaseKey.Client.ClientId;
            purchaseorder.VendorClientLookupId = pOModel.VendorClientLookupId;
            purchaseorder.Attention = pOModel.Attention ?? "";    //---------V2-178
            purchaseorder.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseorder.Status = PurchaseOrderStatusConstants.Open;
            purchaseorder.FOB = pOModel.FOB ?? "";
            purchaseorder.Terms = pOModel.Terms ?? "";
            purchaseorder.VendorId = pOModel.VendorId;// vlist.First().VendorId;
            workflowlog.ClientId = this.userData.DatabaseKey.Personnel.ClientId;
            workflowlog.ObjectName = "PurchaseOrder";
            workflowlog.Message = "Purchase Order Add";
            workflowlog.UserName = this.userData.DatabaseKey.UserName;
            purchaseorder.workflowlog = workflowlog;
            purchaseorder.Buyer_PersonnelId = pOModel.Buyer_PersonnelId ?? 0;
            purchaseorder.CreateByPKForeignKeys(this.userData.DatabaseKey, true, AutoGenerateKey.PO_ANNUAL, POPrefix);
            CreateEventLog(purchaseorder.PurchaseOrderId, PurchasingEvents.POOpen);
            return purchaseorder;
        }
        public PurchaseOrder EditpurchaseOrder(PurchaseOrderModel pOModel)
        {
            PurchaseOrder purchaseorder = new DataContracts.PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = pOModel.PurchaseOrderId
            };
            purchaseorder.Retrieve(userData.DatabaseKey);
            //Vendor vendor = new Vendor();
            //vendor.ClientId = userData.DatabaseKey.Client.ClientId;
            //vendor.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            //vendor.ClientLookupId = pOModel.VendorClientLookupId;
            //List<Vendor> vlist = vendor.RetrieveBySiteIdAndClientLookUpId(userData.DatabaseKey);
            if (!(pOModel.IsPunchout || pOModel.CountLineItem > 0))
            {
                purchaseorder.VendorId = pOModel.VendorId;// vlist[0].VendorId;
            }
            purchaseorder.Attention = pOModel.Attention ?? "";
            purchaseorder.Carrier = pOModel.Carrier ?? "";
            purchaseorder.FOB = pOModel.FOB ?? "";
            purchaseorder.Terms = pOModel.Terms ?? ""; ;
            purchaseorder.Buyer_PersonnelId = pOModel.Buyer_PersonnelId ?? 0;
            purchaseorder.Reason = pOModel.Reason ?? "";
            purchaseorder.MessageToVendor = pOModel.MessageToVendor ?? "";
            purchaseorder.Required = pOModel.Required;
            purchaseorder.UpdateIndex = purchaseorder.UpdateIndex;
            purchaseorder.Update(userData.DatabaseKey);
            return purchaseorder;
        }

        public string UpdateStatus(string emailHtmlBody, System.IO.MemoryStream stream, long PurchaseOrderId, string Status, string VoidComntValue = null,
                              string ToEmail = null, string CCEmail = null, string MailBodyComments = null)
        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> Polist = new List<long>();
            string UpdateMsg = string.Empty;
            Int64 PRID = 0;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<PurchaseRequest> prlist = new List<PurchaseRequest>();
            PurchaseOrder purchaseorder = new PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = PurchaseOrderId
            };
            WorkFlowLog workflowlog = new WorkFlowLog();
            purchaseorder.Retrieve(userData.DatabaseKey);
            string PrevStatus = purchaseorder.Status ?? string.Empty;
            switch (Status)
            {
                case "void":
                    purchaseorder.VoidReason = VoidComntValue;
                    purchaseorder.Status = PurchaseOrderStatusConstants.Void;
                    purchaseorder.CompleteDate = System.DateTime.UtcNow;
                    purchaseorder.VoidDate = System.DateTime.UtcNow;
                    purchaseorder.CompleteBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    purchaseorder.VoidBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    workflowlog.ClientId = this.userData.DatabaseKey.Personnel.ClientId;
                    workflowlog.ObjectName = "PurchaseOrder";
                    workflowlog.ObjectId = PurchaseOrderId;
                    workflowlog.UserName = this.userData.DatabaseKey.UserName;
                    workflowlog.Message = "Void";
                    PurchaseOrderLineItem p = new PurchaseOrderLineItem();
                    p.ClientId = userData.DatabaseKey.Client.ClientId;
                    p.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    p.PurchaseOrderId = PurchaseOrderId;
                    List<PurchaseOrderLineItem> plist = PurchaseOrderLineItem.PurchaseOrderLineItemRetrieveByPurchaseOrderId_V2(userData.DatabaseKey, p);
                    foreach (var item in plist)
                    {
                        PurchaseOrderLineItem purchaselineitem = new PurchaseOrderLineItem()
                        {
                            ClientId = this.userData.DatabaseKey.User.ClientId,
                            PurchaseOrderId = PurchaseOrderId,
                            PurchaseOrderLineItemId = item.PurchaseOrderLineItemId

                        };
                        purchaselineitem.Retrieve(userData.DatabaseKey);
                        purchaselineitem.Status = PurchaseOrderStatusConstants.Void;
                        purchaselineitem.Update(this.userData.DatabaseKey);
                    }
                    UpdateMsg = "success";
                    purchaseorder.Update(userData.DatabaseKey);
                    PurchaseRequest pr = new PurchaseRequest();
                    pr.ClientId = userData.DatabaseKey.Client.ClientId;
                    pr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    pr.PurchaseOrderId = purchaseorder.PurchaseOrderId;
                    prlist = pr.RetrieveForInformation(userData.DatabaseKey);
                    PRID = prlist != null && prlist.Count > 0 ? prlist[0].PurchaseRequestId : 0;
                    CreateEventLog(purchaseorder.PurchaseOrderId, PurchasingEvents.Void, PRID);
                    if (plist != null && plist.Count > 0 && (PrevStatus == PurchaseOrderStatusConstants.Open || PrevStatus == PurchaseOrderStatusConstants.Partial))
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdateListPartsonOrder(purchaseorder.PurchaseOrderId, "Minus", AttachmentTableConstant.PurchaseOrder));
                    }
                    break;
                case "EmailToVendor":
                    string resetUrl = GetHostedUrl();
                    //EmailModule emailModule = new EmailModule();
                    Presentation.Common.EmailModule emailModule = new Presentation.Common.EmailModule();
                    emailModule.ToEmailAddress = ToEmail;
                    emailModule.CcEmail = CCEmail;
                    BodyContent = emailHtmlBody;
                    //emailModule.MailSubject = "Purchase Order" + " " + purchaseorder.ClientLookupId;
                    emailModule.Subject = "Purchase Order" + " " + purchaseorder.ClientLookupId;
                    BodyHeader = "Purchase Order" + " " + purchaseorder.ClientLookupId + " " + "is attached";
                    FooterSignature = this.userData.DatabaseKey.User.FirstName + " " + this.userData.DatabaseKey.User.LastName;
                    if (BodyContent != null)
                    {
                        string output = BodyContent.
                            Replace("headerBgURL", resetUrl + SomaxAppConstants.HeaderMailTemplate).
                            Replace("somaxLogoURL", resetUrl + SomaxAppConstants.SomaxLogoForMailTemplate).
                            Replace("strrequestid", purchaseorder.ClientLookupId).
                            Replace("strmailbodycomments", MailBodyComments).
                            Replace("strtypeofrequest", "Purchase Order").
                            Replace("spnloginurl", resetUrl).
                            Replace("footerURL", resetUrl + SomaxAppConstants.FooterMailTemplate);

                        emailModule.MailBody = output;
                    }
                    else
                    {
                        emailModule.MailBody = "<html><body>" + BodyHeader + " <br><br> " + FooterSignature + "";
                    }

                    #region Attachment
                    System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(stream, "PurchaseOrderDetails.pdf", "application/pdf");
                    emailModule.MailAttachment = attachment;
                    #endregion

                    bool IsSent = emailModule.SendEmail();
                    if (IsSent == true)
                    {
                        ReviewLog rlog = new ReviewLog()
                        {
                            ClientId = userData.DatabaseKey.Client.ClientId,
                            TableName = "PurchaseOrder",
                            ObjectId = PurchaseOrderId,
                            Function = ReviewLogConstants.Email,
                            PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                            Comments = "Purchase Order Emailed to Vendor",
                            ReviewDate = DateTime.UtcNow
                        };
                        rlog.Create(userData.DatabaseKey);
                        Polist.Add(PurchaseOrderId);
                        objAlert.CreateAlert<PurchaseOrder>(AlertTypeEnum.POEmailToVendor, Polist);
                        Alerts alerts = new Alerts()
                        {
                            ClientId = userData.DatabaseKey.Client.ClientId,
                            ObjectId = PurchaseOrderId,
                            ObjectName = "PurchaseOrder",
                            AlertName = AlertTypeEnum.POImportedReviewRequired.ToString(),
                            PersonnelId = 0
                        };
                        alerts.ClearAlert(userData.DatabaseKey);
                    }
                    PurchaseRequest prq = new PurchaseRequest();
                    prq.ClientId = userData.DatabaseKey.Client.ClientId;
                    prq.SiteId = userData.DatabaseKey.Personnel.SiteId;
                    prq.PurchaseOrderId = PurchaseOrderId;
                    prq.RetrieveForInformation(userData.DatabaseKey);
                    prlist = prq.RetrieveForInformation(userData.DatabaseKey);
                    PRID = prlist != null && prlist.Count > 0 ? prlist[0].PurchaseRequestId : 0;
                    CreateEventLog(PurchaseOrderId, PurchasingEvents.EmailToVendor, PRID);
                    UpdateMsg = "success";
                    break;
            }
            return UpdateMsg;
        }


        public string UpdateForceCompleteStatus(long PurchaseOrderId, int lineitemcount)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string UpdateMsg = string.Empty;
            PurchaseOrder purchaseorder = new PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = PurchaseOrderId
            };
            WorkFlowLog workflowlog = new WorkFlowLog();
            purchaseorder.Retrieve(userData.DatabaseKey);
            string PrevStatus = purchaseorder.Status ?? string.Empty;
            purchaseorder.Status = PurchaseOrderStatusConstants.Complete;
            purchaseorder.CompleteDate = DateTime.UtcNow;
            purchaseorder.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            purchaseorder.WorkFlowMessageForceComplete = "Force Comeplete";
            purchaseorder.UpdateByPurchaseOrderForceComplete(userData.DatabaseKey);
            if (lineitemcount > 0 && (PrevStatus == PurchaseOrderStatusConstants.Open || PrevStatus == PurchaseOrderStatusConstants.Partial))
            {
                Task task2 = Task.Factory.StartNew(() => commonWrapper.UpdateListPartsonOrder(PurchaseOrderId, "Minus", AttachmentTableConstant.PurchaseOrder));
            }
            UpdateMsg = "success";
            return UpdateMsg;
        }
        #endregion

        #region Purchase Line Item
        public bool DeleteLineItem(long _PurchaseOrderLineItemId, long _PurchaseOrderId)
        {
            try
            {
                bool retValue = false;
                PurchaseOrderLineItem purchaseOrderLineItem = new PurchaseOrderLineItem()
                {
                    PurchaseOrderLineItemId = _PurchaseOrderLineItemId,
                    PurchaseOrderId = _PurchaseOrderId
                };
                purchaseOrderLineItem.Retrieve(userData.DatabaseKey);
                string Status = string.Empty;
                Status = purchaseOrderLineItem.Status;
                if (purchaseOrderLineItem.Status != PurchaseOrderStatusConstants.Open)
                {
                    return retValue;
                }
                else
                {
                    retValue = true;
                    long ChargeToID = purchaseOrderLineItem.ChargeToId;
                    string ChargeType = purchaseOrderLineItem.ChargeType ?? string.Empty;
                    purchaseOrderLineItem.Delete(userData.DatabaseKey);
                    #region Delete LineItem Records from UDF V2-653
                    if (purchaseOrderLineItem.ErrorMessages == null || purchaseOrderLineItem.ErrorMessages.Count == 0)
                    {

                        POLineUDF objPOLineUDF = new POLineUDF()
                        {
                            PurchaseOrderLineItemId = _PurchaseOrderLineItemId
                        };
                        objPOLineUDF.DeleteByPurchaseOrderLineItemId(userData.DatabaseKey);
                    }
                    #endregion
                    purchaseOrderLineItem.ReOrderLineNumber(userData.DatabaseKey);
                    if (ChargeType == AttachmentTableConstant.WorkOrder && (Status == PurchaseOrderStatusConstants.Open || Status == PurchaseOrderStatusConstants.Partial))
                    {
                        CommonWrapper commonWrapper = new CommonWrapper(userData);
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(ChargeToID, "Minus"));

                    }



                }
                return retValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<POLineItemModel> PopulatePOLineItem(long PurchaseOrderId)
        {
            POLineItemModel polineModel;
            List<POLineItemModel> POLineItemList = new List<POLineItemModel>();
            PurchaseOrderLineItem purchaseorder = new PurchaseOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = PurchaseOrderId
            };
            List<PurchaseOrderLineItem> purchaseorderitemlist = PurchaseOrderLineItem.PurchaseOrderLineItemRetrieveByPurchaseOrderId_V2(this.userData.DatabaseKey, purchaseorder);
            foreach (var po in purchaseorderitemlist)
            {
                polineModel = new POLineItemModel();
                polineModel.PurchaseOrderId = po.PurchaseOrderId;
                polineModel.PurchaseOrderLineItemId = po.PurchaseOrderLineItemId;
                polineModel.Status = po.Status;
                polineModel.LineNumber = po.LineNumber;
                polineModel.PartClientLookupId = po.PartClientLookupId;
                polineModel.Description = po.Description;
                polineModel.OrderQuantity = Math.Round(po.OrderQuantity, 2);
                polineModel.UnitOfMeasure = po.UnitOfMeasure;
                polineModel.UnitCost = Math.Round(po.UnitCost, 2);
                polineModel.TotalCost = Math.Round(po.TotalCost, 2);
                polineModel.Status_Display = po.Status_Display;
                polineModel.PartId = po.PartId;
                polineModel.QuantityBackOrdered = po.QuantityBackOrdered;
                polineModel.QuantityReceived = po.QuantityReceived;
                polineModel.QuantityToDate = po.QuantityToDate;
                polineModel.StoreroomId = po.StoreroomId;
                polineModel.CurrentAverageCost = po.CurrentAverageCost;
                polineModel.CurrentAppliedCost = po.CurrentAppliedCost;
                polineModel.CurrentOnHandQuantity = po.CurrentOnHandQuantity;
                polineModel.AccountId = po.AccountId;
                polineModel.Creator_PersonnelId = po.Creator_PersonnelId;
                polineModel.PartStoreroomId = po.PartStoreroomId;
                polineModel.Description = po.Description;
                polineModel.StockType = po.StockType;
                polineModel.UnitOfMeasure = po.UnitOfMeasure;
                polineModel.ChargeToId = po.ChargeToId;
                polineModel.ChargeType = po.ChargeType;
                polineModel.Part_Manufacturer = po.Part_Manufacturer;
                polineModel.Part_ManufacturerID = po.Part_ManufacturerID;
                polineModel.SupplierPartId = po.SupplierPartId;
                polineModel.SupplierPartAuxiliaryId = po.SupplierPartAuxiliaryId;
                //polineModel.Classification = po.Classification; V2-717
                polineModel.AccountClientLookupId = po.AccountClientLookupId;
                polineModel.UOMConversion = po.UOMConversion;
                polineModel.PurchaseUOM = po.PurchaseUOM;
                polineModel.PurchaseQuantity = po.PurchaseQuantity;
                polineModel.PurchaseCost = po.PurchaseCost;
                polineModel.LineTotal = po.LineTotal;
                polineModel.PartNumber = po.PartNumber;
                polineModel.EstimatedDelivery = DateTime.MinValue == po.EstimatedDelivery ? null : po.EstimatedDelivery;
                polineModel.ChargeToClientLookupId = po.ChargeToClientLookupId;//Added in V2-672
                polineModel.RequestorName = po.RequestorName;//Added in V2-1115
                polineModel.EstimatedCostsId = po.EstimatedCostsId;//Added in V2-1124
                POLineItemList.Add(polineModel);
            }
            return POLineItemList;
        }
        internal LineItem GetLineItem(long PurchaseOrderLineItemId, long PurchaseOrderId)
        {
            LineItem objLineItem = new LineItem();
            PurchaseOrderLineItem purchaseOrderlineitem = new PurchaseOrderLineItem()
            {
                PurchaseOrderLineItemId = PurchaseOrderLineItemId,
                ClientId = userData.DatabaseKey.Personnel.ClientId,
            };
            purchaseOrderlineitem.PurchaseOrderLineItemRetrieveByPurchaseOrderLineItemId(this.userData.DatabaseKey);
            if (purchaseOrderlineitem != null)
            {
                objLineItem.PurchaseOrderLineItemId = PurchaseOrderLineItemId;
                objLineItem.PurchaseOrderId = PurchaseOrderId;
                objLineItem.PartId = purchaseOrderlineitem.PartId;
                objLineItem.LineNumber = purchaseOrderlineitem.LineNumber;
                objLineItem.PartClientLookupId = purchaseOrderlineitem.PartClientLookupId;
                objLineItem.Description = purchaseOrderlineitem.Description;
                objLineItem.OrderQuantity = purchaseOrderlineitem.OrderQuantity;
                objLineItem.UnitOfMeasure = purchaseOrderlineitem.UnitOfMeasure;
                objLineItem.UnitCost = purchaseOrderlineitem.UnitCost;
                objLineItem.TotalCost = purchaseOrderlineitem.TotalCost;
                objLineItem.AccountClientLookupId = purchaseOrderlineitem.AccountClientLookupId;
                objLineItem.AccountId = purchaseOrderlineitem.AccountId;
                objLineItem.EstimatedDelivery = purchaseOrderlineitem.EstimatedDelivery;
                objLineItem.Taxable = purchaseOrderlineitem.Taxable;
                objLineItem.ChargeType = purchaseOrderlineitem.ChargeType;
                objLineItem.ChargeToId = purchaseOrderlineitem.ChargeToId;
                objLineItem.ChargeTo_Name = purchaseOrderlineitem.ChargeTo_Name;
                objLineItem.Status = purchaseOrderlineitem.Status;
                objLineItem.ChargeToClientLookupIdToShow = purchaseOrderlineitem.ChargeToClientLookupId;
                objLineItem.PurchaseUOM = purchaseOrderlineitem.PurchaseUOM;
            }
            return objLineItem;
        }
        internal PurchaseOrderLineItem UpdateLineItem(LineItem lineItem)
        {
            PurchaseOrderLineItem purchaseOrderlineitem = new PurchaseOrderLineItem()
            {
                PurchaseOrderLineItemId = lineItem.PurchaseOrderLineItemId,
                PurchaseOrderId = lineItem.PurchaseOrderId,
            };
            purchaseOrderlineitem.Retrieve(userData.DatabaseKey);
            string status = purchaseOrderlineitem.Status ?? string.Empty;
            string OldChargeType = purchaseOrderlineitem.ChargeType ?? string.Empty;
            long OldChargeToId = purchaseOrderlineitem.ChargeToId;
            string NewChargeType = lineItem.ChargeType ?? string.Empty;
            long NewChargeToId = lineItem.ChargeToId ?? 0;
            if (purchaseOrderlineitem != null)
            {
                purchaseOrderlineitem.Description = lineItem.Description ?? "";
                purchaseOrderlineitem.OrderQuantity = lineItem.OrderQuantity;
                //purchaseOrderlineitem.UnitOfMeasure = lineItem.UnitOfMeasure ?? "";                
                purchaseOrderlineitem.AccountId = lineItem.AccountId ?? 0;
                purchaseOrderlineitem.UnitCost = lineItem.UnitCost;
                purchaseOrderlineitem.Taxable = lineItem.Taxable;
                if (lineItem.EstimatedDelivery != null && lineItem.EstimatedDelivery.Value == default(DateTime))
                {
                    purchaseOrderlineitem.EstimatedDelivery = null;
                }
                else
                {
                    purchaseOrderlineitem.EstimatedDelivery = lineItem.EstimatedDelivery;
                }
                if (purchaseOrderlineitem.PartId == 0)
                {
                    purchaseOrderlineitem.ChargeType = lineItem.ChargeType;
                    purchaseOrderlineitem.ChargeToId = lineItem.ChargeToId ?? 0;
                    purchaseOrderlineitem.ChargeTo_Name = lineItem.ChargeTo_Name;
                    purchaseOrderlineitem.UnitOfMeasure = lineItem.PurchaseUOM;
                }
                else
                {
                    purchaseOrderlineitem.PartId = purchaseOrderlineitem.PartId;
                }
                purchaseOrderlineitem.PurchaseUOM = lineItem.PurchaseUOM;
            }
            purchaseOrderlineitem.UpdateByPKForeignKeys(this.userData.DatabaseKey);

            if (status == PurchaseOrderStatusConstants.Open || status == PurchaseOrderStatusConstants.Partial)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                if (OldChargeType == AttachmentTableConstant.WorkOrder)
                {
                    if (NewChargeType != AttachmentTableConstant.WorkOrder)
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(OldChargeToId, "Minus"));
                    }
                    else
                    {
                        if (OldChargeToId != NewChargeToId)
                        {
                            Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(OldChargeToId, "Minus"));
                            Task task2 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(NewChargeToId, "Add"));
                            Task.WaitAll(task1, task2);
                        }
                    }
                }
                else
                {

                    if (NewChargeType == AttachmentTableConstant.WorkOrder)
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(NewChargeToId, "Add"));
                    }
                }

            }

            return purchaseOrderlineitem;
        }
        internal List<string> InsertLineItem(PurchaseOrderVM _PurchaseOrder)
        {
            LineItem lineItem = new LineItem()
            {
                PurchaseOrderId = _PurchaseOrder.lineItem.PurchaseOrderId,
                Description = _PurchaseOrder.lineItem.Description ?? "",
                OrderQuantity = _PurchaseOrder.lineItem.OrderQuantity,
                UnitOfMeasure = _PurchaseOrder.lineItem.PurchaseUOM,
                AccountId = _PurchaseOrder.lineItem.AccountId,
                UnitCost = _PurchaseOrder.lineItem.UnitCost,
                ChargeType = _PurchaseOrder.lineItem.ChargeType,
                ChargeToId = _PurchaseOrder.lineItem.ChargeToId,
                Status = PurchaseOrderStatusConstants.Open,
                PurchaseUOM = _PurchaseOrder.lineItem.PurchaseUOM
            };
            PurchaseOrderLineItem purchaseOrderlineitem = new PurchaseOrderLineItem()
            {
                PurchaseOrderId = lineItem.PurchaseOrderId,
                Description = lineItem.Description ?? "",
                OrderQuantity = lineItem.OrderQuantity,
                UnitOfMeasure = lineItem.UnitOfMeasure,
                AccountId = lineItem.AccountId ?? 0,
                UnitCost = lineItem.UnitCost,
                ChargeType = lineItem.ChargeType,
                ChargeToId = lineItem.ChargeToId ?? 0,
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId,
                Status = lineItem.Status,
                PurchaseUOM = lineItem.PurchaseUOM
            };
            purchaseOrderlineitem.CreateWithValidation(userData.DatabaseKey);
            if (purchaseOrderlineitem.ErrorMessages.Count == 0)
            {
                purchaseOrderlineitem.ReOrderLineNumber(userData.DatabaseKey);

                if (purchaseOrderlineitem.ChargeType == AttachmentTableConstant.WorkOrder && (purchaseOrderlineitem.Status == PurchaseOrderStatusConstants.Open || purchaseOrderlineitem.Status == PurchaseOrderStatusConstants.Partial))
                {
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(purchaseOrderlineitem.ChargeToId, "Add"));

                }
            }
            return purchaseOrderlineitem.ErrorMessages;
        }
        #endregion      
        #region Notes
        public List<Client.Models.PurchaseOrder.NotesModel> PopulateNotes(long PurchaseRequestId)
        {
            Client.Models.PurchaseOrder.NotesModel objNotesModel;
            List<Client.Models.PurchaseOrder.NotesModel> NotesModelList = new List<Client.Models.PurchaseOrder.NotesModel>();
            Notes note = new Notes()
            {
                ObjectId = PurchaseRequestId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<Notes> NotesList = note.RetrieveByObjectIdForPurchaseOrder_V2(userData.DatabaseKey, userData.Site.TimeZone);
            if (NotesList != null)
            {
                foreach (var item in NotesList)
                {
                    objNotesModel = new Client.Models.PurchaseOrder.NotesModel();
                    objNotesModel.NotesId = item.NotesId;
                    objNotesModel.Subject = item.Subject;
                    objNotesModel.OwnerName = item.OwnerName;
                    objNotesModel.ModifiedDate = item.ModifiedDate;
                    NotesModelList.Add(objNotesModel);
                }
            }
            return NotesModelList;
        }
        public List<string> AddPoNotes(PurchaseOrderVM _PurchaseOrder)
        {
            Notes notes = new Notes();
            if (_PurchaseOrder.notesModel.NotesId == 0)
            {
                notes.OwnerId = userData.DatabaseKey.User.UserInfoId;
                notes.OwnerName = userData.DatabaseKey.User.FullName;
                notes.Type = _PurchaseOrder.notesModel.Type;
                notes.TableName = "PurchaseOrder";
                notes.ObjectId = _PurchaseOrder.notesModel.PurchaseOrderId;
                // RKL - 2024-Nov-07
                // Added the following two lines - New PO Notes - Subject and Content were empty
                notes.Subject = _PurchaseOrder.notesModel.Subject;
                notes.Content = _PurchaseOrder.notesModel.Content;
                notes.UpdateIndex = _PurchaseOrder.notesModel.updatedindex;
                notes.NotesId = _PurchaseOrder.notesModel.NotesId;
                notes.Create(userData.DatabaseKey);
            }
            else
            {
                notes.ClientId = userData.DatabaseKey.Client.ClientId;
                notes.NotesId = _PurchaseOrder.notesModel.NotesId;
                notes.Retrieve(userData.DatabaseKey);
                notes.Subject = _PurchaseOrder.notesModel.Subject;
                notes.Content = _PurchaseOrder.notesModel.Content;
                notes.OwnerId = userData.DatabaseKey.User.UserInfoId;
                notes.OwnerName = userData.DatabaseKey.User.FullName;
                notes.Update(userData.DatabaseKey);
            }
            return notes.ErrorMessages;
        }
        public Models.PurchaseOrder.NotesModel EditPoNotes(long PurchaseOrderId, long NotesId)
        {
            Models.PurchaseOrder.NotesModel objNotesModel = new Models.PurchaseOrder.NotesModel();
            Notes note = new Notes()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                NotesId = NotesId,
            };
            note.Retrieve(userData.DatabaseKey);
            objNotesModel.NotesId = note.NotesId;
            objNotesModel.updatedindex = note.UpdateIndex;
            objNotesModel.Subject = note.Subject;
            objNotesModel.Content = note.Content;
            objNotesModel.PurchaseOrderId = PurchaseOrderId;
            return objNotesModel;
        }

        public bool DeletePoNotes(string _notesId)
        {
            try
            {
                Notes notes = new Notes()
                {
                    NotesId = Convert.ToInt64(_notesId)
                };
                notes.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Attachment
        public Models.PurchaseOrder.AttachmentModel EditPoAttachment(long PurchaseOrderId, long FileAttachmentId)
        {
            Models.PurchaseOrder.AttachmentModel objAttachmentModel = new Models.PurchaseOrder.AttachmentModel();
            Attachment attachment = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                AttachmentId = FileAttachmentId,
            };
            attachment.Retrieve(userData.DatabaseKey);
            objAttachmentModel.AttachmentId = attachment.AttachmentId;
            objAttachmentModel.Subject = attachment.Description;
            objAttachmentModel.PrintwithForm = attachment.PrintwithForm;
            objAttachmentModel.PurchaseOrderId = PurchaseOrderId;
            #region V2-1006
            objAttachmentModel.FileType = attachment.FileType;
            #endregion
            return objAttachmentModel;
        }


        #endregion
        #region Event Log
        public List<EventLogModel> PopulateEventLog(long PurchaseOrderId)
        {
            EventLogModel objEventLogModel;
            List<EventLogModel> EventLogModelList = new List<EventLogModel>();
            PurchasingEventLog log = new PurchasingEventLog();
            List<PurchasingEventLog> data = new List<PurchasingEventLog>();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = PurchaseOrderId;
            data = log.RetriveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
            if (data != null)
            {
                foreach (var item in data)
                {
                    objEventLogModel = new EventLogModel();
                    objEventLogModel.ClientId = item.ClientId;
                    objEventLogModel.SiteId = item.SiteId;
                    objEventLogModel.EventLogId = item.EventLogId;
                    objEventLogModel.ObjectId = item.ObjectId;
                    if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                    {
                        objEventLogModel.TransactionDate = null;
                    }
                    else
                    {
                        objEventLogModel.TransactionDate = item.TransactionDate;
                    }
                    objEventLogModel.Event = item.Event;
                    objEventLogModel.Comments = item.Comments;
                    objEventLogModel.SourceId = item.SourceId;
                    objEventLogModel.Personnel = item.Personnel;
                    objEventLogModel.Events = UtilityFunction.GetMessageFromResource("PurchaseEvent" + item.Event, LocalizeResourceSetConstants.PurchaseEventStatus);
                    if (string.IsNullOrEmpty(objEventLogModel.Events))
                    {
                        objEventLogModel.Events = item.Event;
                    }
                    EventLogModelList.Add(objEventLogModel);
                }
            }
            return EventLogModelList;
        }
        #endregion
        #region Part In Inventory
        internal List<PartInInventoryModel> PopulateSelectParts()
        {
            PartInInventoryModel pInventory;
            List<PartInInventoryModel> pInventoryList = new List<PartInInventoryModel>();
            Part part = new Part()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<Part> partList = part.RetrieveForSearchBySiteId(this.userData.DatabaseKey);
            foreach (var p in partList)
            {
                pInventory = new PartInInventoryModel();
                pInventory.PartId = p.PartId;
                pInventory.ClientLookupId = p.ClientLookupId;
                pInventory.Description = p.Description;
                pInventory.Manufacturer = p.Manufacturer;
                pInventory.ManufacturerId = p.ManufacturerId;
                pInventory.Quantity = p.QtyOnHand;
                pInventoryList.Add(pInventory);
            }
            return pInventoryList;
        }
        internal Dictionary<long, string> UpadatePartIn(List<LineItem> list)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            PurchaseOrderLineItem purchaseOrderLineItem;
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        purchaseOrderLineItem = new PurchaseOrderLineItem();
                        Part part = new Part()
                        {
                            ClientId = this.userData.DatabaseKey.User.ClientId,
                            PartId = item.PartId,
                        };
                        part.RetriveByPartId(userData.DatabaseKey);
                        purchaseOrderLineItem.ClientId = this.userData.DatabaseKey.User.ClientId;
                        purchaseOrderLineItem.PurchaseOrderId = item.PurchaseOrderId;
                        purchaseOrderLineItem.DepartmentId = 0;
                        purchaseOrderLineItem.StoreroomId = 0;
                        purchaseOrderLineItem.AccountId = part.AccountId;
                        purchaseOrderLineItem.ChargeToId = 0;
                        purchaseOrderLineItem.CompleteBy_PersonnelId = 0;
                        purchaseOrderLineItem.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                        purchaseOrderLineItem.Description = part.Description;
                        purchaseOrderLineItem.PartId = item.PartId;
                        purchaseOrderLineItem.PartStoreroomId = part.PartStoreroomId;
                        purchaseOrderLineItem.PRCreator_PersonnelId = 0;
                        purchaseOrderLineItem.Status = PurchaseOrderStatusConstants.Open;
                        purchaseOrderLineItem.Taxable = part.Taxable;
                        purchaseOrderLineItem.UnitOfMeasure = part.IssueUnit;
                        purchaseOrderLineItem.UnitCost = part.AppliedCost;
                        purchaseOrderLineItem.OrderQuantity = item.OrderQuantity;
                        purchaseOrderLineItem.CreateWithValidation(this.userData.DatabaseKey);
                        if (purchaseOrderLineItem.ErrorMessages.Count == 0)
                        {
                            purchaseOrderLineItem.ReOrderLineNumber(userData.DatabaseKey);
                        }
                        else
                        {
                            retValue.Add(item.PartId, purchaseOrderLineItem.ErrorMessages[0]);
                        }

                    }
                }
            }
            return retValue;
        }
        #endregion
        #region AccountInfo
        public DataContracts.Account RetrieveaccountDetails(long AccountId)
        {
            DataContracts.Account account = new DataContracts.Account()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AccountId = AccountId
            };
            account.Retrieve(_dbKey);
            return account;
        }
        #endregion
        #region Purchase Order Receipt
        internal List<KeyValuePair<string, string>> DisplayIdList()
        {
            List<KeyValuePair<string, string>> customFinalList = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> customList = CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, "PurchaseOrder", userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            if (customList.Count > 0)
            {
                customList.Insert(0, new KeyValuePair<string, string>("0", "-- Select All --"));
            }
            return customList;
        }
        public string GetBuyerName(long personnelId)
        {
            string BuyerName = string.Empty;
            Personnel objPersonnel = new Personnel();
            objPersonnel.PersonnelId = personnelId;
            objPersonnel.ClientId = this.userData.DatabaseKey.User.ClientId;
            objPersonnel.Retrieve(userData.DatabaseKey);
            BuyerName = objPersonnel.ClientLookupId + " - " + objPersonnel.NameFirst + " " + objPersonnel.NameLast;
            return BuyerName;
        }
        public PurchaseOrderReceiptModel initializeControls(Part obj)
        {
            PurchaseOrderReceiptModel objPurchaseOrderReceipt = new PurchaseOrderReceiptModel();
            objPurchaseOrderReceipt.ClientLookupId = obj.ClientLookupId;
            objPurchaseOrderReceipt.Description = obj?.Description ?? string.Empty;
            objPurchaseOrderReceipt.UOM = obj.IssueUnit;
            // RKL-MAIL-Label Printing from Receipts
            objPurchaseOrderReceipt.Location1_5 = obj.Location1_5;
            objPurchaseOrderReceipt.Location1_1 = obj.Location1_1;
            objPurchaseOrderReceipt.Location1_2 = obj.Location1_2;
            objPurchaseOrderReceipt.Location1_3 = obj.Location1_3;
            objPurchaseOrderReceipt.Location1_4 = obj.Location1_4;
            objPurchaseOrderReceipt.Minimum = obj.QtyReorderLevel.ToString("N0");
            objPurchaseOrderReceipt.Maximum = obj.QtyMaximum.ToString("N0");
            objPurchaseOrderReceipt.Manufacturer = Convert.ToString(obj.Manufacturer);
            objPurchaseOrderReceipt.ManufacturerId = Convert.ToString(obj.ManufacturerId);
            return objPurchaseOrderReceipt;
        }
        public PurchaseOrderReceiptModel populatePartDetails(long PartId)
        {
            PurchaseOrderReceiptModel objPurchaseOrderReceipt = new PurchaseOrderReceiptModel();
            PartModel objPart = new PartModel();
            Part obj = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId
            };
            obj.RetriveByPartId(userData.DatabaseKey);
            objPurchaseOrderReceipt = initializeControls(obj);
            return objPurchaseOrderReceipt;
        }
        #region RKL MAIL -Label Printing from Receipts
        public PurchaseOrderReceiptModel populatePartDetailsMultiStoreroom(long PartId, long PartStoreroomId)
        {
            PurchaseOrderReceiptModel objPurchaseOrderReceipt = new PurchaseOrderReceiptModel();
            PartModel objPart = new PartModel();
            Part obj = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId,
                PartStoreroomId = PartStoreroomId
            };
            obj.RetrieveByPartIdAndPartStoreroomId_V2(userData.DatabaseKey);
            objPurchaseOrderReceipt = initializeControls(obj);
            return objPurchaseOrderReceipt;
        }
        #endregion
        public PurchaseOrderReceiptModel initializePartStoreControls(PartStoreroom obj)
        {
            PurchaseOrderReceiptModel objPurchaseOrderReceipt = new PurchaseOrderReceiptModel();
            PartModel objPart = new PartModel();
            Part objp = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = obj.PartId
            };
            objp.RetriveByPartId(userData.DatabaseKey);
            PartStoreroom partStoreroom = new PartStoreroom();
            objPurchaseOrderReceipt.Location1_5 = obj.Location1_5;
            objPurchaseOrderReceipt.Location1_1 = obj.Location1_1;
            objPurchaseOrderReceipt.Location1_2 = obj.Location1_2;
            objPurchaseOrderReceipt.Location1_3 = obj.Location1_3;
            objPurchaseOrderReceipt.Location1_4 = obj.Location1_4;
            objPurchaseOrderReceipt.Minimum = obj.QtyReorderLevel.ToString("N0");
            objPurchaseOrderReceipt.Maximum = obj.QtyMaximum.ToString("N0");
            objPurchaseOrderReceipt.Manufacturer = Convert.ToString(objp.Manufacturer);
            objPurchaseOrderReceipt.ManufacturerId = Convert.ToString(objp.ManufacturerId);
            return objPurchaseOrderReceipt;
        }
        public PurchaseOrderReceiptModel populatePartStoreDetails(long PartId)
        {
            PurchaseOrderReceiptModel objPurchaseOrderReceipt = new PurchaseOrderReceiptModel();
            PartStoreroom pstoreroom = new PartStoreroom
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartId = Convert.ToInt64(PartId)
            };
            PartStoreroom partStoreroom = PartStoreroom.RetrieveByPartId(userData.DatabaseKey, pstoreroom)[0];
            objPurchaseOrderReceipt = initializePartStoreControls(partStoreroom);
            return objPurchaseOrderReceipt;
        }
        public PurchaseOrderReceiptModel GetPurchaseOrderReceiptDetails()
        {
            int NoOfItems = 0;
            int ItemsReceived = 0;
            int ItemsIssued = 0;
            PurchaseOrderReceiptModel objPurchaseOrderReceiptModel = new PurchaseOrderReceiptModel();
            objPurchaseOrderReceiptModel.NoOfItems = Convert.ToInt32(NoOfItems.ToString());
            objPurchaseOrderReceiptModel.ItemsReceived = Convert.ToInt32(ItemsReceived.ToString());
            objPurchaseOrderReceiptModel.ItemsIssued = Convert.ToInt32(ItemsIssued.ToString());
            return objPurchaseOrderReceiptModel;
        }
        public POReceipt AddPurchaseOrderReceipt(PurchaseOrderReceiptModel pORModel)
        {
            PurchaseOrderReceiptModel objPurchaseOrderReceiptModel = new PurchaseOrderReceiptModel();
            POReceipt receiptitems = new POReceipt();
            try
            {
                receiptitems.POReceiptHeader.ClientId = this.userData.DatabaseKey.Client.ClientId;
                receiptitems.POReceiptHeader.ReceiveDate = DateTime.UtcNow;
                receiptitems.POReceiptHeader.Comments = pORModel.Comments;
                receiptitems.POReceiptHeader.Carrier = pORModel.Carrier;
                receiptitems.POReceiptHeader.PackingSlip = pORModel.PackingSlip;
                receiptitems.POReceiptHeader.FreightBill = pORModel.FreightBill;
                decimal fa = 0;
                receiptitems.POReceiptHeader.FreightAmount = decimal.TryParse(pORModel.FreightAmount, out fa) == true ? fa : 0;
                receiptitems.POReceiptHeader.PurchaseOrderId = pORModel.PurchaseOrderId;
                receiptitems.POReceiptHeader.ReceiptNumber = 0;
                receiptitems.POReceiptHeader.ReceiveBy_PersonnelID = this.userData.DatabaseKey.Personnel.PersonnelId;
                receiptitems.PO_Receipt_CreateHeader(userData.DatabaseKey);
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
            }
            return receiptitems;
        }
        internal POReceipt AddPOReceiptItems(long PurchaseOrderId, List<PurchaseOrderReceiptLineItemModel> PORData)
        {
            PurchaseOrderReceiptModel pORModel = new PurchaseOrderReceiptModel();
            POReceipt receiptitems = new POReceipt();
            int CompleteCount = 0;
            int PartialCount = 0;
            int OpenCount = 0;
            foreach (var ri in PORData)
            {
                long ChargeToId = ri.ChargeToId;
                string ChargeType = ri.ChargeType ?? string.Empty;
                ReceiptParams r = new ReceiptParams();
                receiptitems.POPurchaseOrder.ClientId = userData.DatabaseKey.Client.ClientId;
                receiptitems.POPurchaseOrder.PurchaseOrderId = PurchaseOrderId;
                receiptitems.POPurchaseOrder.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                #region V2-1124
                if (ri.EstimatedCostsId > 0)
                {
                    receiptitems.POEstimatedCosts.ClientId = userData.DatabaseKey.Client.ClientId;
                    receiptitems.POEstimatedCosts.EstimatedCostsId = ri.EstimatedCostsId;
                    receiptitems.POEstimatedCosts.Retrieve(userData.DatabaseKey);
                }
                #endregion
                r.PurchaseOrderLineItemId = Convert.ToInt64(ri.PurchaseOrderLineItemId);
                // We do NOT want to convert to an integer - RKL - 2021-Feb-16
                //r.QuantityReceived = Convert.ToInt64(ri.QuantityReceived);  // z;
                r.QuantityReceived = Convert.ToDecimal(ri.QuantityReceived);  // z;
                r.POLinePartStoreroomId = Convert.ToInt64(ri.PartStoreroomId);
                r.OrderQuantity = Convert.ToDecimal(ri.OrderQuantity);
                r.QuantityToDate = Convert.ToDecimal(ri.QuantityToDate);
                r.CurrentAvgCost = Convert.ToDecimal(ri.CurrentAverageCost);
                r.CurrentAppCost = Convert.ToDecimal(ri.CurrentAppliedCost);
                r.CurrentonHandQty = Convert.ToDecimal(ri.CurrentOnHandQuantity);
                r.UnitCost = Convert.ToDecimal(ri.UnitCost);
                r.POLineAccountId = Convert.ToInt64(ri.AccountId);
                r.POLIneItem_Creator = Convert.ToInt64(ri.Creator_PersonnelId);
                r.PartId = Convert.ToInt64(ri.PartId);
                r.POLineStoreroomId = Convert.ToInt64(ri.StoreroomId);
                r.POLineDescription = Convert.ToString(ri.Description);
                r.Stocktype = Convert.ToString(ri.StockType);
                r.POLineUnitofMeasure = Convert.ToString(ri.UnitOfMeasure);
                r.POLineChargeToId = Convert.ToInt64(ri.ChargeToId);
                r.POLineChargeType = Convert.ToString(ri.ChargeType);
                r.UOMConversion = (Convert.ToDecimal(ri.UOMConversion) == Convert.ToDecimal("0")) ? Convert.ToDecimal("1.0") : (Convert.ToDecimal(ri.UOMConversion)); //553
                r.OverReceive = r.OrderQuantity - ((r.QuantityToDate / r.UOMConversion) + r.QuantityReceived);
                r.PurchaseUOM = ri.PurchaseUOM;//553
                receiptitems.POlineItem.UnitCost = Convert.ToDecimal(ri.UnitCost);
                receiptitems.POlineItem.UnitOfMeasure = Convert.ToString(ri.UnitOfMeasure);
                receiptitems.POlineItem.PartId = Convert.ToInt64(ri.PartId);
                receiptitems.POlineItem.ChargeToId = Convert.ToInt64(ri.ChargeToId);
                receiptitems.POReceiptHeaderId = Convert.ToInt64(ri.POReceiptHeaderId);
                receiptitems.PurchaseUOM = ri.PurchaseUOM;
                if (r.QuantityReceived > 0)
                {
                    receiptitems.POPartHistory.AverageCostBefore = r.CurrentAvgCost;
                    receiptitems.POPartHistory.CostBefore = r.CurrentAppCost;
                    if (r.OrderQuantity <= (r.QuantityReceived + (r.QuantityToDate / r.UOMConversion)))
                    {
                        r.Status = PurchaseOrderStatusConstants.Complete;
                        CompleteCount++;
                    }
                    else
                    {
                        r.Status = PurchaseOrderStatusConstants.Partial;
                        PartialCount++;
                    }
                    if (r.OverReceive >= 0)
                    {
                        // SOM-1700
                        // RKL - 2019-Jun-25 - Begin
                        // If current on hand is negative and the same as received
                        //  Then the new average cost is the cost of the received part 
                        //  Otherwise used the "standard" average cost calculation
                        decimal newquan = (r.CurrentonHandQty + (r.QuantityReceived * r.UOMConversion));   //553
                        if (newquan > (decimal)0.0)
                        {
                            r.CalculatedAvgCost = ((r.CurrentAvgCost * r.CurrentonHandQty) + (r.UnitCost * r.QuantityReceived)) / newquan;
                        }
                        else
                        {
                            r.CalculatedAvgCost = r.UnitCost;
                        }
                        // RKL - 2019-Jun-25 - End

                        // r.CalculatedAvgCost = ((r.CurrentAvgCost * r.CurrentonHandQty) + (r.UnitCost * r.QuantityReceived)) / (r.CurrentonHandQty + r.QuantityReceived);

                        switch (PurchaseOrderReceiptConstant.PartCostCalc.ToLower())
                        {
                            case "average":
                                r.CurrentAppCost = r.CalculatedAvgCost;
                                break;
                            case "lastpurchase":
                                r.CurrentAppCost = r.UnitCost;
                                break;
                            case "user":
                                r.CurrentAppCost = r.CurrentAppCost;
                                break;
                        }
                        r.CurrentAppCost = r.CalculatedAvgCost;
                        RetrievePageControl(receiptitems, r, pORModel);
                        receiptitems.PO_Receipt(userData.DatabaseKey);
                        try
                        {
                            List<PurchaseRequest> prlist = new List<PurchaseRequest>();
                            PurchaseRequest prq = new PurchaseRequest();
                            prq.ClientId = userData.DatabaseKey.Client.ClientId;
                            prq.SiteId = userData.DatabaseKey.Personnel.SiteId;
                            prq.PurchaseOrderId = PurchaseOrderId;
                            prlist = prq.RetrieveForInformation(userData.DatabaseKey);
                            Int64 PRID = prlist != null && prlist.Count > 0 ? prlist[0].PurchaseRequestId : 0;
                            ProcessAlert objAlert = new ProcessAlert(this.userData);
                            objAlert.CreateAlert(AlertTypeEnum.PurchaseOrderReceipt, receiptitems.PurchaseOrderLineItemId, receiptitems.QuantityReceived);
                            if (ChargeType == AttachmentTableConstant.WorkOrder)  //v2 576
                            {
                                CommonWrapper commonWrapper = new CommonWrapper(userData);
                                //RKL - 2022-APR-21 - This should be a MINUS not an ADD
                                Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(ChargeToId, "Minus"));
                                //Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(ChargeToId, "Add"));

                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        if (receiptitems != null && receiptitems.ErrorMessages != null)
                        { receiptitems.ErrorMessages.Add("You cannot receive more than was ordered"); }

                    }
                }
                else
                {
                    if (r.OrderQuantity <= r.QuantityToDate)
                    {
                        CompleteCount++;
                    }
                    if (r.OrderQuantity > r.QuantityToDate && r.QuantityToDate > 0)
                    {
                        PartialCount++;
                    }
                    if (r.QuantityToDate == 0)
                    {
                        OpenCount++;
                    }
                }

            }
            return receiptitems;
        }
        public void CreateEventLog(Int64 objId, string Status, Int64 SourceId)
        {
            PurchasingEventLog log = new PurchasingEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = objId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = SourceId;
            log.TableName = AttachmentTableConstant.PurchaseOrder;
            log.Create(userData.DatabaseKey);
        }
        public void CreateEventLog(Int64 objId, string Status)
        {
            PurchasingEventLog log = new PurchasingEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = objId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = 0;
            log.TableName = AttachmentTableConstant.PurchaseOrder;
            log.Create(userData.DatabaseKey);
        }
        public void CreateEventLog(Int64 objId, string Status, string Comments)
        {
            PurchasingEventLog log = new PurchasingEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = objId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = Comments;
            log.SourceId = 0;
            log.TableName = AttachmentTableConstant.PurchaseOrder;
            log.Create(userData.DatabaseKey);
        }
        #endregion
        #region Retrieve Control
        private void RetrievePageControl(DataContracts.POReceipt poReceipt, ReceiptParams r, PurchaseOrderReceiptModel poRM)
        {
            poReceipt.ClientId = userData.DatabaseKey.Client.ClientId;
            poReceipt.PurchaseOrderLineItemId = r.PurchaseOrderLineItemId;
            // poReceipt.QuantityReceived = r.QuantityReceived;
            // poReceipt.UnitCost = r.UnitCost;//553 
            poReceipt.UnitOfMeasure = r.POLineUnitofMeasure;
            poReceipt.POlineItem.Status = r.Status;
            poReceipt.POPart.ClientId = userData.DatabaseKey.Client.ClientId;
            poReceipt.POPart.PartId = r.PartId;
            poReceipt.POPart.AverageCost = r.CalculatedAvgCost;
            poReceipt.POPart.AppliedCost = r.CurrentAppCost;
            poReceipt.POPartStoreRoom.ClientId = userData.DatabaseKey.Client.ClientId;
            poReceipt.POPartStoreRoom.PartStoreroomId = r.POLinePartStoreroomId;
            poReceipt.POPartStoreRoom.QtyOnHand = (r.CurrentonHandQty + (r.QuantityReceived * r.UOMConversion));  //v2 553
            poReceipt.UOMConversion = r.UOMConversion;  //553
            poReceipt.QuantityReceived = r.QuantityReceived * r.UOMConversion;  //553
            poReceipt.PurchaseQuantityReceived = r.QuantityReceived;//553
            poReceipt.PurchaseCost = r.UnitCost;  //553
            poReceipt.UnitCost = r.UnitCost / r.UOMConversion;
            poReceipt.PurchaseUOM = r.PurchaseUOM;  //553
            if (r.PartId == 0 || (r.PartId != 0 && poReceipt.POlineItem.ChargeToId != 0)) // if condition modified for V2-1124 from (POPart.PartId == 0) to (r.PartId == 0 || (r.PartId != 0 && poReceipt.POlineItem.ChargeToId != 0))
            {
                poReceipt.POPartHistory.ClientId = userData.DatabaseKey.Client.ClientId;
                poReceipt.POPartHistory.PartId = r.PartId;
                poReceipt.POPartHistory.PartStoreroomId = r.POLinePartStoreroomId;
                poReceipt.POPartHistory.AccountId = r.POLineAccountId;
                poReceipt.POPartHistory.AverageCostBefore = r.UnitCost;
                poReceipt.POPartHistory.AverageCostAfter = r.UnitCost;
                poReceipt.POPartHistory.ChargeToId_Primary = r.POLineChargeToId;
                poReceipt.POPartHistory.ChargeType_Primary = r.POLineChargeType;
                poReceipt.POPartHistory.Comments = poRM.Comments;
                poReceipt.POPartHistory.Cost = r.UnitCost;
                poReceipt.POPartHistory.CostAfter = r.UnitCost;
                poReceipt.POPartHistory.CostBefore = r.UnitCost;
                poReceipt.POPartHistory.Description = r.POLineDescription;
                poReceipt.POPartHistory.PerformedById = userData.DatabaseKey.Personnel.PersonnelId;
                poReceipt.POPartHistory.RequestorId = r.POLIneItem_Creator;
                poReceipt.POPartHistory.QtyAfter = r.QuantityReceived;
                poReceipt.POPartHistory.QtyBefore = 0;
                poReceipt.POPartHistory.StockType = "";
                poReceipt.POPartHistory.StoreroomId = r.POLinePartStoreroomId;
                poReceipt.POPartHistory.TransactionDate = DateTime.UtcNow;
                poReceipt.POPartHistory.TransactionQuantity = r.QuantityReceived;
                poReceipt.POPartHistory.TransactionType = PartHistoryTranTypes.Receipt;
                poReceipt.POPartHistory.UnitofMeasure = r.POLineUnitofMeasure;
                poReceipt.POPartHistory.CreatedBy = this.userData.DatabaseKey.UserName;
                poReceipt.POPartHistory.CreatedDate = DateTime.UtcNow;
            }
            else if (r.PartId != 0)
            {
                // RKL - Support Multiple Storerooms
                // V2-968 - Begin
                Part part = new Part()
                {
                    ClientId = this.userData.DatabaseKey.User.ClientId,
                    PartId = r.PartId,
                    StoreroomId = poReceipt.POPurchaseOrder.StoreroomId
                };
                //part.RetriveByPartId(userData.DatabaseKey); 
                part.Retrieve(userData.DatabaseKey);  //V2-968 need not to use part.RetriveByPartId(userData.DatabaseKey) as only part.IssueUnit is need to fetch
                poReceipt.POPartHistory.ClientId = userData.DatabaseKey.Client.ClientId;
                poReceipt.POPartHistory.PartId = r.PartId;
                poReceipt.POPartHistory.PartStoreroomId = r.POLinePartStoreroomId;
                poReceipt.POPartHistory.AccountId = r.POLineAccountId;
                poReceipt.POPartHistory.AverageCostAfter = r.CalculatedAvgCost;
                poReceipt.POPartHistory.Comments = poRM.Comments;
                poReceipt.POPartHistory.Cost = r.UnitCost / r.UOMConversion;  //v2 - 553
                poReceipt.POPartHistory.CostAfter = r.CurrentAppCost;
                poReceipt.POPartHistory.Description = r.POLineDescription;
                poReceipt.POPartHistory.PerformedById = userData.DatabaseKey.Personnel.PersonnelId;
                poReceipt.POPartHistory.RequestorId = r.POLIneItem_Creator;
                poReceipt.POPartHistory.QtyAfter = r.CurrentonHandQty + (r.QuantityReceived * r.UOMConversion);   //v2 - 553
                poReceipt.POPartHistory.QtyBefore = r.CurrentonHandQty;
                poReceipt.POPartHistory.StockType = r.Stocktype;
                poReceipt.POPartHistory.StoreroomId = r.POLinePartStoreroomId;
                poReceipt.POPartHistory.TransactionDate = DateTime.UtcNow;
                poReceipt.POPartHistory.TransactionQuantity = r.QuantityReceived * r.UOMConversion;  //v2 553
                poReceipt.POPartHistory.TransactionType = PartHistoryTranTypes.Receipt;
                poReceipt.POPartHistory.UnitofMeasure = part.IssueUnit;// r.POLineUnitofMeasure;  v2  553
                poReceipt.POPartHistory.CreatedBy = this.userData.DatabaseKey.UserName;
                poReceipt.POPartHistory.CreatedDate = DateTime.UtcNow;
            }
        }
        public struct ReceiptParams
        {
            public Int64 PurchaseOrderLineItemId { get; set; }
            public int LineNo { get; set; }
            public decimal QuantityReceived { get; set; }
            public decimal OrderQuantity { get; set; }
            public decimal QuantityToDate { get; set; }
            public decimal OverReceive { get; set; }
            public decimal CurrentAvgCost { get; set; }
            public decimal CurrentAppCost { get; set; }
            public decimal CurrentonHandQty { get; set; }
            public decimal CalculatedAvgCost { get; set; }
            public decimal CalculatedAppCost { get; set; }
            public decimal UnitCost { get; set; }
            public string Status { get; set; }
            public Int64 POLinePartStoreroomId { get; set; }
            public Int64 PartId { get; set; }
            public Int64 POLineAccountId { get; set; }
            public string POLineDescription { get; set; }
            public string Stocktype { get; set; }
            public Int64 POLineStoreroomId { get; set; }
            public string POLineUnitofMeasure { get; set; }
            public Int64 POLineChargeToId { get; set; }
            public string POLineChargeType { get; set; }
            public Int64 POLIneItem_Creator { get; set; }

            public decimal UOMConversion { get; set; }

            public string PurchaseUOM { get; set; }
        }
        #endregion
        #region Receipt History
        internal List<InnerGridLineItemModel> InnerGridReceiptHistory(long PurchaseOrderLineItemId)
        {
            List<InnerGridLineItemModel> retObj = new List<InnerGridLineItemModel>();
            List<POReceiptHeader> rObj = new List<POReceiptHeader>();
            POReceiptHeader poReceiptHeader = new POReceiptHeader
            {
                PurchaseOrderLineItemId = PurchaseOrderLineItemId,
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            var PoHeaderListItem = poReceiptHeader.RetrieveByPurchaseOrderLineItemId(userData.DatabaseKey);
            foreach (var item in PoHeaderListItem)
            {
                InnerGridLineItemModel obj = new InnerGridLineItemModel();
                obj.PurchaseOrderLineItemId = PurchaseOrderLineItemId;
                obj.ReceiptNumber = item.ReceiptNumber;
                obj.ReceiveBy_PersonnelName = item.ReceiveBy_PersonnelName;
                if (item.ReceiveDate.HasValue)
                {
                    obj.strReceiveDate = item.ReceiveDate.Value.ToShortDateString();
                }
                obj.ReceiveDate = item.ReceiveDate;
                obj.QuantityReceived = item.QuantityReceived;
                obj.Comments = item.Comments;
                obj.Reversed = item.Reversed;
                obj.ReversedComments = item.ReversedComments;
                if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster)
                {
                    obj.ExReceiptNo = item.ExReceiptNo;
                    obj.ExReceiptTxnId = item.ExReceiptTxnId;
                }
                obj.POReceiptItemId = item.POReceiptItemId;
                obj.POReceiptHeaderId = item.POReceiptHeaderId;
                obj.ReceiveBy_PersonnelID = item.ReceiveBy_PersonnelID;
                obj.IsPurchasingEdit = userData.Security.Purchasing.Edit;
                obj.UOMConversion = (Convert.ToDecimal(item.UOMConversion) == Convert.ToDecimal("0")) ? Convert.ToDecimal("1.0") : (Convert.ToDecimal(item.UOMConversion)); //553
                obj.ChargeType = item.ChargeType;
                obj.ChargeToId = item.ChargeToId;
                #region V2-878
                obj.PackingSlip = item.PackingSlip;
                obj.FreightAmount = item.FreightAmount;
                obj.FreightBill = item.FreightBill;
                #endregion
                retObj.Add(obj);
            }
            return retObj;
        }
        internal POReceipt UpdateRevised(List<InnerGridLineItemModel> list)
        {
            POReceipt poReceiptUpdate = new POReceipt();
            RetrievePageControls(poReceiptUpdate, list[0].POReceiptItemId);
            poReceiptUpdate.ClientId = _dbKey.Client.ClientId;
            poReceiptUpdate.POReceiptItemId = list[0].POReceiptItemId;
            poReceiptUpdate.POReceiptHeaderId = list[0].POReceiptHeaderId;
            poReceiptUpdate.PurchaseOrderLineItemId = list[0].PurchaseOrderLineItemId;
            poReceiptUpdate.QuantityReceived = list[0].QuantityReceived;
            poReceiptUpdate.Reversed = true;
            poReceiptUpdate.ReversedBy_PersonnelId = list[0].ReceiveBy_PersonnelID;
            poReceiptUpdate.ReversedComments = list[0].Comments;
            poReceiptUpdate.ReversedDate = System.DateTime.UtcNow;
            poReceiptUpdate.UOMConversion = list[0].UOMConversion;
            poReceiptUpdate.PO_ReverseReceipt(userData.DatabaseKey);
            string ChargeType = list[0].ChargeType ?? string.Empty;

            if (ChargeType == AttachmentTableConstant.WorkOrder)  //v2 576
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                // RKL - 2022-Apr-21 - RKL when a receipt is reversed - we need to "Add"
                Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(list[0].ChargeToId, "Add"));
                //Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(list[0].ChargeToId, "Minus"));

            }
            return poReceiptUpdate;
        }
        protected void RetrievePageControls(POReceipt poReceipt, long POReceiptItemId)
        {
            string partCostType = PurchaseOrderReceiptConstant.PartCostCalc;
            POReceiptItem poReceiptItem = new POReceiptItem();
            poReceiptItem.ClientId = this.userData.DatabaseKey.Client.ClientId;
            poReceiptItem.POReceiptItemId = POReceiptItemId;
            poReceiptItem.Retrieve(this.userData.DatabaseKey);
            POReceiptHeader poReceiptHeader = new POReceiptHeader();
            poReceiptHeader.ClientId = poReceiptItem.ClientId;
            poReceiptHeader.POReceiptHeaderId = poReceiptItem.POReceiptHeaderId;
            poReceiptHeader.Retrieve(this.userData.DatabaseKey);
            PurchaseOrderLineItem POLineItem = new PurchaseOrderLineItem();
            POLineItem.ClientId = poReceiptItem.ClientId;
            POLineItem.PurchaseOrderLineItemId = poReceiptItem.PurchaseOrderLineItemId;
            POLineItem.PurchaseOrderLineItemRetrieveByPurchaseOrderLineItemId(this.userData.DatabaseKey);
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            purchaseOrder.ClientId = poReceiptItem.ClientId;
            purchaseOrder.PurchaseOrderId = POLineItem.PurchaseOrderId;
            purchaseOrder.Retrieve(this.userData.DatabaseKey);
            poReceipt.POReceiptItemId = poReceiptItem.POReceiptItemId;
            poReceipt.UpdateIndex = poReceiptItem.UpdateIndex;
            poReceipt.POReceiptHeaderId = poReceiptItem.POReceiptHeaderId;
            poReceipt.PurchaseOrderLineItemId = poReceiptItem.PurchaseOrderLineItemId;
            poReceipt.QuantityReceived = poReceiptItem.QuantityReceived;
            poReceipt.POPurchaseOrder = purchaseOrder;
            poReceipt.POlineItem = POLineItem;
            poReceipt.Reversed = true;
            poReceipt.ReversedBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            poReceipt.ReversedDate = DateTime.UtcNow;
            decimal newQtyToDate = POLineItem.QuantityToDate - poReceiptItem.QuantityReceived;
            if (newQtyToDate <= 0)
            {
                POLineItem.Status = PurchaseOrderStatusConstants.Open;
            }
            else if (newQtyToDate >= POLineItem.OrderQuantity)
            {
                POLineItem.Status = PurchaseOrderStatusConstants.Complete;
            }
            else if (newQtyToDate < POLineItem.OrderQuantity)
            {
                POLineItem.Status = PurchaseOrderStatusConstants.Partial;
            }
            if (poReceipt.POlineItem.PartId > 0)
            {
                poReceipt.POPart.ClientId = poReceiptItem.ClientId;
                poReceipt.POPart.PartId = POLineItem.PartId;
                poReceipt.POPart.RetriveByPartId(userData.DatabaseKey);
                poReceipt.POPartStoreRoom.ClientId = poReceiptItem.ClientId;
                poReceipt.POPartStoreRoom.PartStoreroomId = POLineItem.PartStoreroomId;

                poReceipt.POPartStoreRoom.QtyOnHand = POLineItem.CurrentOnHandQuantity - poReceiptItem.QuantityReceived;  //v2 553 As per the spec(Page no 18): poReceiptItem.QuantityReceived=(Received Qty * UOM Conversion Factor);
                decimal newOnHand = POLineItem.CurrentOnHandQuantity - poReceiptItem.QuantityReceived;  //v2 553 As per the spec(Page no 18): poReceiptItem.QuantityReceived=(Received Qty * UOM Conversion Factor);
                if (newOnHand > 0)
                {
                    poReceipt.POPart.AverageCost = ((POLineItem.CurrentAverageCost * POLineItem.CurrentOnHandQuantity) - (poReceiptItem.UnitCost * poReceiptItem.QuantityReceived)) / newOnHand;

                }
                switch (partCostType.ToLower())
                {
                    case "average":
                        poReceipt.POPart.AppliedCost = POLineItem.CurrentAverageCost;
                        break;
                    case "lastpurchase":
                        poReceipt.POPart.AppliedCost = poReceiptItem.UnitCost;
                        break;
                    case "user":
                        poReceipt.POPart.AppliedCost = poReceipt.POPart.AppliedCost;
                        break;
                }
            }
            poReceipt.POPart.AverageCost = poReceipt.POPart.AverageCost;
            poReceipt.POPart.AppliedCost = poReceipt.POPart.AverageCost;
            poReceipt.POPartHistory.ClientId = this.userData.DatabaseKey.Client.ClientId;
            poReceipt.POPartHistory.AccountId = POLineItem.AccountId;
            poReceipt.POPartHistory.ChargeType_Primary = POLineItem.ChargeType;
            poReceipt.POPartHistory.ChargeToId_Primary = POLineItem.ChargeToId;
            poReceipt.POPartHistory.Comments = poReceiptHeader.Comments;
            poReceipt.POPartHistory.Description = POLineItem.Description;
            poReceipt.UOMConversion = (Convert.ToDecimal(poReceiptItem.UOMConversion) == Convert.ToDecimal("0")) ? Convert.ToDecimal("1.0") : (Convert.ToDecimal(poReceiptItem.UOMConversion)); //553
            // V2-342 - Part History Cost is UNIT COST 
            poReceipt.POPartHistory.Cost = poReceiptItem.UnitCost; //v2 553;
            //poReceipt.POPartHistory.Cost = poReceiptItem.QuantityReceived * poReceiptItem.UnitCost;
            poReceipt.POPartHistory.PerformedById = this.userData.DatabaseKey.Personnel.PersonnelId;
            poReceipt.POPartHistory.StockType = POLineItem.StockType;
            poReceipt.POPartHistory.StoreroomId = POLineItem.StoreroomId;
            poReceipt.POPartHistory.TransactionDate = DateTime.UtcNow;
            poReceipt.POPartHistory.TransactionQuantity = poReceiptItem.QuantityReceived;  //v2-553 Here (poReceiptItem.QuantityReceived = Receipt Quantity * UOM Conversion Factor) as per page no 20 ;
            poReceipt.POPartHistory.UnitofMeasure = poReceipt.POPart.IssueUnit; //poReceiptItem.UnitOfMeasure;  //v2 553;
            // V2-342 - 2020-Apr-09 - RKL
            poReceipt.POPartHistory.CreatedBy = this.userData.DatabaseKey.UserName;
            poReceipt.POPartHistory.CreatedDate = DateTime.UtcNow;

            if (POLineItem.PartId != 0)
            {
                poReceipt.POPartHistory.PartId = POLineItem.PartId;
                poReceipt.POPartHistory.PartStoreroomId = POLineItem.PartStoreroomId;
                poReceipt.POPartHistory.AverageCostBefore = POLineItem.CurrentAverageCost;
                poReceipt.POPartHistory.AverageCostAfter = poReceipt.POPart.AverageCost;  //v2 - 553;
                poReceipt.POPartHistory.CostAfter = poReceipt.POPart.AppliedCost;
                poReceipt.POPartHistory.CostBefore = POLineItem.CurrentAppliedCost;
                poReceipt.POPartHistory.QtyAfter = poReceipt.POPartStoreRoom.QtyOnHand;  //v2 - 553;
                poReceipt.POPartHistory.QtyBefore = POLineItem.CurrentOnHandQuantity;
            }
            else
            {
                poReceipt.POPartHistory.AverageCostBefore = poReceiptItem.UnitCost;
                poReceipt.POPartHistory.AverageCostAfter = poReceiptItem.UnitCost;
                // RKL - V2-342 - cost calculation is incorrect
                //poReceipt.POPartHistory.CostAfter = poReceiptItem.QuantityReceived * poReceiptItem.UnitCost;
                //poReceipt.POPartHistory.CostBefore = poReceiptItem.QuantityReceived * poReceiptItem.UnitCost;
                poReceipt.POPartHistory.CostAfter = poReceiptItem.UnitCost;
                poReceipt.POPartHistory.CostBefore = poReceiptItem.UnitCost;
                poReceipt.POPartHistory.QtyAfter = 0;
                poReceipt.POPartHistory.QtyBefore = poReceiptItem.QuantityReceived;
            }
        }
        #endregion

        #region Print PO Receipt
        public POPrintReceiptModel POReceiptPrint(long POReceiptHeaderId)
        {
            System.Data.DataTable dtPOReceipt = new System.Data.DataTable();
            System.Data.DataTable dtPOReceiptVendor = new System.Data.DataTable();
            System.Data.DataTable dtPOLineitems = new System.Data.DataTable();
            INTDataLayer.BAL.PurchaseOrderBAL POReceiptBal = new INTDataLayer.BAL.PurchaseOrderBAL();
            dtPOReceipt = POReceiptBal.GetReceiptNumberbyPurchaseOrderId(userData.DatabaseKey.User.CallerUserInfoId, userData.DatabaseKey.User.CallerUserName, userData.DatabaseKey.Client.ClientId, Convert.ToInt64(POReceiptHeaderId), userData.DatabaseKey.AdminConnectionString);
            Int64 POID = (Int64)dtPOReceipt.Rows[0]["PurchaseOrderId"];
            dtPOReceiptVendor = POReceiptBal.GetVendorDetailsbyPurchaseOrderIdandReceiptNumber(userData.DatabaseKey.User.CallerUserInfoId, userData.DatabaseKey.User.CallerUserName, userData.DatabaseKey.Client.ClientId, POID, POReceiptHeaderId, userData.DatabaseKey.AdminConnectionString);
            dtPOLineitems = POReceiptBal.GetDetailByPurchaseOrderId(userData.DatabaseKey.User.CallerUserInfoId, userData.DatabaseKey.User.CallerUserName, userData.DatabaseKey.Client.ClientId, POID, POReceiptHeaderId, userData.DatabaseKey.AdminConnectionString, userData.DatabaseKey.User.TimeZone);
            PrintTotalModel = new POPrintReceiptModel();
            POPrintReceiptModel PrintPOReceiptModel = new POPrintReceiptModel();
            POPrintReceiptModel PrintVendorModel = new POPrintReceiptModel();
            POPrintReceiptModel PrintLineItemModel = new POPrintReceiptModel();
            PrintTotalModel.poLineItemList = new List<POPrintReceiptModel>();

            PrintPOReceiptModel = ConvertToList<POPrintReceiptModel>(dtPOReceipt);
            MergePOReceipt(PrintPOReceiptModel);

            PrintVendorModel = ConvertToList<POPrintReceiptModel>(dtPOReceiptVendor);
            MergePOReceipt(PrintVendorModel);

            PrintLineItemModel.poLineItemList = ConvertDataTable<POPrintReceiptModel>(dtPOLineitems);
            PrintTotalModel.poLineItemList = PrintLineItemModel.poLineItemList;

            return PrintTotalModel;
        }

        public POPrintReceiptModel POReceiptHeaderPrint(long POReceiptHeaderId)
        {
            System.Data.DataTable dtPOReceipt = new System.Data.DataTable();
            System.Data.DataTable dtPOReceiptVendor = new System.Data.DataTable();
            INTDataLayer.BAL.PurchaseOrderBAL POReceiptBal = new INTDataLayer.BAL.PurchaseOrderBAL();
            dtPOReceipt = POReceiptBal.GetReceiptNumberbyPurchaseOrderId(userData.DatabaseKey.User.CallerUserInfoId, userData.DatabaseKey.User.CallerUserName, userData.DatabaseKey.Client.ClientId, Convert.ToInt64(POReceiptHeaderId), userData.DatabaseKey.AdminConnectionString);
            Int64 POID = (Int64)dtPOReceipt.Rows[0]["PurchaseOrderId"];
            dtPOReceiptVendor = POReceiptBal.GetVendorDetailsbyPurchaseOrderIdandReceiptNumber(userData.DatabaseKey.User.CallerUserInfoId, userData.DatabaseKey.User.CallerUserName, userData.DatabaseKey.Client.ClientId, POID, POReceiptHeaderId, userData.DatabaseKey.AdminConnectionString);
            PrintTotalModel = new POPrintReceiptModel();
            POPrintReceiptModel PrintPOReceiptModel = new POPrintReceiptModel();
            POPrintReceiptModel PrintVendorModel = new POPrintReceiptModel();
            PrintTotalModel.poLineItemList = new List<POPrintReceiptModel>();

            PrintPOReceiptModel = ConvertToList<POPrintReceiptModel>(dtPOReceipt);
            MergePOReceipt(PrintPOReceiptModel);

            PrintVendorModel = ConvertToList<POPrintReceiptModel>(dtPOReceiptVendor);
            MergePOReceipt(PrintVendorModel);

            return PrintTotalModel;
        }
        private void MergePOReceipt(POPrintReceiptModel m)
        {
            PrintTotalModel.Address1 = string.IsNullOrEmpty(m.Address1) ? PrintTotalModel.Address1 : m.Address1;
            PrintTotalModel.Address2 = string.IsNullOrEmpty(m.Address2) ? PrintTotalModel.Address2 : m.Address2;
            PrintTotalModel.Address3 = string.IsNullOrEmpty(m.Address1) ? PrintTotalModel.Address3 : m.Address3;
            PrintTotalModel.AddressCity = string.IsNullOrEmpty(m.AddressCity) ? PrintTotalModel.AddressCity : m.AddressCity;
            PrintTotalModel.AddressCountry = string.IsNullOrEmpty(m.AddressCountry) ? PrintTotalModel.AddressCountry : m.AddressCountry;
            PrintTotalModel.AddressPostCode = string.IsNullOrEmpty(m.AddressPostCode) ? PrintTotalModel.AddressPostCode : m.AddressPostCode;
            PrintTotalModel.AddressState = string.IsNullOrEmpty(m.AddressState) ? PrintTotalModel.AddressState : m.AddressState;
            PrintTotalModel.ClientLookupId = string.IsNullOrEmpty(m.ClientLookupId) ? PrintTotalModel.ClientLookupId : m.ClientLookupId;
            PrintTotalModel.Comments = string.IsNullOrEmpty(m.Comments) ? PrintTotalModel.Comments : m.Comments;
            PrintTotalModel.Description = string.IsNullOrEmpty(m.Description) ? PrintTotalModel.Description : m.Description;
            PrintTotalModel.LineNumber = m.LineNumber;
            PrintTotalModel.Location = string.IsNullOrEmpty(m.Location) ? PrintTotalModel.Location : m.Location;
            PrintTotalModel.ManufacturerId = string.IsNullOrEmpty(m.ManufacturerId) ? PrintTotalModel.ManufacturerId : m.ManufacturerId;
            PrintTotalModel.POReceiptHeaderId = m.POReceiptHeaderId == 0 ? PrintTotalModel.POReceiptHeaderId : m.POReceiptHeaderId;
            PrintTotalModel.PartClientLookUpId = string.IsNullOrEmpty(m.PartClientLookUpId) ? PrintTotalModel.PartClientLookUpId : m.PartClientLookUpId;
            PrintTotalModel.PersonnelName = string.IsNullOrEmpty(m.PersonnelName) ? PrintTotalModel.PersonnelName : m.PersonnelName;
            PrintTotalModel.PurchaseOrderId = m.PurchaseOrderId == 0 ? PrintTotalModel.PurchaseOrderId : m.PurchaseOrderId;
            PrintTotalModel.QuantityReceived = m.QuantityReceived == 0 ? PrintTotalModel.QuantityReceived : m.QuantityReceived;
            PrintTotalModel.ReceiptNumber = Convert.ToInt32(m.POReceiptHeaderId) == 0 ? PrintTotalModel.ReceiptNumber : Convert.ToInt32(m.POReceiptHeaderId);
            PrintTotalModel.ReceiveDate = string.IsNullOrEmpty(m.ReceiveDate.ToString()) ? PrintTotalModel.ReceiveDate : m.ReceiveDate;
            PrintTotalModel.Total = m.Total == 0 ? PrintTotalModel.Total : m.Total;
            PrintTotalModel.UnitCost = m.UnitCost == 0 ? PrintTotalModel.UnitCost : m.UnitCost;
            PrintTotalModel.UnitofMeasure = string.IsNullOrEmpty(m.UnitofMeasure) ? PrintTotalModel.UnitofMeasure : m.UnitofMeasure;
            PrintTotalModel.VendorName = string.IsNullOrEmpty(m.VendorName) ? PrintTotalModel.VendorName : m.VendorName;
        }
        public static T ConvertToList<T>(System.Data.DataTable dt)
        {
            var columnNames = dt.Columns.Cast<System.Data.DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList().FirstOrDefault();
        }
        //--Added for multiple rows to list--//
        private static List<T> ConvertDataTable<T>(System.Data.DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (var pro in properties)
                {
                    //if (pro.Name == column.ColumnName)
                    //    pro.SetValue(obj, dr[column.ColumnName], null);
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        catch (Exception ex) { }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        #endregion

        #region PunchOut
        public PunchoutOrderMessageData GetPunchoutOrderMessageData(long PurchaseOrderId, Models.VendorsModel objVen, Personnel personnel, Site site)
        {
            return SetPunchoutOrder(PurchaseOrderId, objVen, personnel, site);
        }
        public PunchoutOrderMessageData GetPunchoutOrderMessageData(long PurchaseOrderId, Models.VendorsModel objVen, Personnel personnel, Site site, long SiteId)
        {
            return SetPunchoutOrder(PurchaseOrderId, objVen, personnel, site, SiteId);
        }
        private PunchoutOrderMessageData SetPunchoutOrder(long PurchaseOrderId, Models.VendorsModel objVen, Personnel personnel, Site site, long SiteId = 0)
        {
            PartsWrapper partsWrapper = new PartsWrapper(userData);

            Models.PunchoutOrderExport.PunchoutOrderMessageData cxml = new Models.PunchoutOrderExport.PunchoutOrderMessageData();
            Models.PunchoutOrderExport.Header hdr = new Models.PunchoutOrderExport.Header();
            Models.PunchoutOrderExport.From frm = new Models.PunchoutOrderExport.From();
            Models.PunchoutOrderExport.To to = new Models.PunchoutOrderExport.To();
            Models.PunchoutOrderExport.Credential cred = new Models.PunchoutOrderExport.Credential();
            Models.PunchoutOrderExport.Sender sender = new Models.PunchoutOrderExport.Sender();
            //Request Section
            Models.PunchoutOrderExport.Request req = new Models.PunchoutOrderExport.Request();
            Models.PunchoutOrderExport.OrderRequest oreq = new Models.PunchoutOrderExport.OrderRequest();
            Models.PunchoutOrderExport.OrderRequestHeader oreqH = new Models.PunchoutOrderExport.OrderRequestHeader();
            Models.PunchoutOrderExport.Contact con = new Models.PunchoutOrderExport.Contact();
            Models.PunchoutOrderExport.Name name = new Models.PunchoutOrderExport.Name();
            Models.PunchoutOrderExport.Extrinsic extrin = new Models.PunchoutOrderExport.Extrinsic();
            //Initialize
            cxml.Header = new Models.PunchoutOrderExport.Header();
            cxml.Header.From = new Models.PunchoutOrderExport.From();
            cxml.Header.From.Credential = new Models.PunchoutOrderExport.Credential();
            cxml.Header.To = new Models.PunchoutOrderExport.To();
            cxml.Header.To.Credential = new Models.PunchoutOrderExport.Credential();
            cxml.Header.Sender = new Models.PunchoutOrderExport.Sender();
            cxml.Header.Sender.Credential = new Models.PunchoutOrderExport.Credential();
            cxml.Request = new Models.PunchoutOrderExport.Request();
            cxml.Request.OrderRequest = new Models.PunchoutOrderExport.OrderRequest();
            cxml.Request.OrderRequest.OrderRequestHeader = new OrderRequestHeader();
            cxml.Request.OrderRequest.OrderRequestHeader.Total = new Models.PunchoutOrderExport.Total();
            cxml.Request.OrderRequest.OrderRequestHeader.Total.Money = new Models.PunchoutOrderExport.Money();
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo = new Models.PunchoutOrderExport.ShipTo();
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address = new Models.PunchoutOrderExport.Address();
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.Name = new Models.PunchoutOrderExport.Name();
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress = new Models.PunchoutOrderExport.PostalAddress();
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.Email = new Models.PunchoutOrderExport.Email();
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.Country = new Models.PunchoutOrderExport.Country();
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo = new Models.PunchoutOrderExport.BillTo();
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address = new Models.PunchoutOrderExport.Address();
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.Name = new Models.PunchoutOrderExport.Name();
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress = new Models.PunchoutOrderExport.PostalAddress();
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.Email = new Models.PunchoutOrderExport.Email();
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.Country = new Models.PunchoutOrderExport.Country();

            cxml.Request.OrderRequest.ItemOut = new List<Models.PunchoutOrderExport.ItemOut>();
            //--Bind data
            cxml.PayloadID = DateTime.UtcNow.Ticks.ToString() + PurchaseOrderId.ToString() + "@somax.com";
            cxml.Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");

            cxml.Header.From.Credential.Domain = objVen.SenderDomain;// "DUNS";
            cxml.Header.From.Credential.Identity = objVen.SenderIdentity; //"810939686";

            cxml.Header.To.Credential.Domain = objVen.VendorDomain; //"DUNS";
            cxml.Header.To.Credential.Identity = objVen.VendorIdentity;// "159148746";

            cxml.Header.Sender.Credential.Domain = objVen.SenderDomain; // "DUNS";
            cxml.Header.Sender.Credential.Identity = objVen.SenderIdentity; //"810939686";
            cxml.Header.Sender.Credential.SharedSecret = objVen.SharedSecret; //"gra1nger";
            cxml.Header.Sender.UserAgent = "SOMAXG4";//"Oracle Fusion Self Service Procurement";

            cxml.Request.OrderRequest.OrderRequestHeader.OrderID = PurchaseOrderId.ToString();
            cxml.Request.OrderRequest.OrderRequestHeader.OrderDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            cxml.Request.OrderRequest.OrderRequestHeader.Type = "new";
            cxml.Request.OrderRequest.OrderRequestHeader.OrderVersion = "1";// need to check
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.IsoCountryCode = site.AddressISOCountryCode;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.AddressID = SiteId > 0 ? SiteId.ToString() : site.SiteId.ToString();
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.Name.Text = site.Name;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.Name = "default";
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.DeliverTo = personnel.NameLast + ',' + personnel.NameFirst;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.Street1 = site.Address1;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.Street2 = site.Address2;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.City = site.AddressCity;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.State = site.AddressState;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.PostalCode = site.AddressPostCode;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.Country.IsoCountryCode = site.AddressISOCountryCode;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.PostalAddress.Country.Text = site.AddressCountry;
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.Email.Name = "default";
            cxml.Request.OrderRequest.OrderRequestHeader.ShipTo.Address.Email.Text = string.IsNullOrEmpty(personnel.Email) ? objVen.SendPunchoutPOEmail : personnel.Email;

            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.Name.Text = site.Name;
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.Name = "default";
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.IsoCountryCode = site.AddressISOCountryCode;
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.DeliverTo = personnel.NameLast + ',' + personnel.NameFirst;
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.Street1 = site.BillToAddress1;
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.Street2 = site.BillToAddress2;
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.City = site.BillToAddressCity;
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.State = site.BillToAddressState;
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.PostalCode = site.BillToAddressPostCode;
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.Country.IsoCountryCode = site.BillToAddressISOCountryCode;
            cxml.Request.OrderRequest.OrderRequestHeader.BillTo.Address.PostalAddress.Country.Text = site.BillToAddressCountry;
            //PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);

            List<ItemOut> itemOutList = new List<ItemOut>();
            decimal lineCost = 0;
            List<POLineItemModel> lineItems = new List<POLineItemModel>();
            string PartClientlookupId = string.Empty;
            lineItems = PopulatePOLineItem(PurchaseOrderId);

            if (lineItems != null && lineItems.Count > 0)
            {
                foreach (var item in lineItems)
                {
                    PartClientlookupId = string.Empty;
                    if (item.PartId > 0)
                    {
                        PartModel partModel = partsWrapper.PopulatePartDetails(item.PartId);
                        if (partModel != null)
                        {
                            PartClientlookupId = partModel.ClientLookupId ?? "";
                        }
                    }

                    ItemOut itemOut1 = new Models.PunchoutOrderExport.ItemOut();
                    itemOut1.ItemID = new Models.PunchoutOrderExport.ItemID();
                    itemOut1.ItemDetail = new Models.PunchoutOrderExport.ItemDetail();
                    itemOut1.ItemDetail.UnitPrice = new Models.PunchoutOrderExport.UnitPrice();
                    itemOut1.ItemDetail.UnitPrice.Money = new Models.PunchoutOrderExport.Money();
                    itemOut1.ItemDetail.Description = new Models.PunchoutOrderExport.Description();
                    itemOut1.ItemDetail.Classification = new Models.PunchoutOrderExport.Classification();
                    itemOut1.ItemDetail.ManufacturerPartID = new Models.PunchoutOrderExport.ManufacturerPartID();
                    itemOut1.ItemDetail.ManufacturerName = new Models.PunchoutOrderExport.ManufacturerName();

                    itemOut1.LineNumber = item.LineNumber.ToString();
                    itemOut1.Quantity = item.OrderQuantity.ToString();
                    itemOut1.ItemID.SupplierPartID = item.SupplierPartId;
                    itemOut1.ItemID.SupplierPartAuxiliaryID = PartClientlookupId ?? "";
                    itemOut1.ItemDetail.UnitPrice.Money.Text = item.UnitCost.ToString() ?? string.Empty;
                    itemOut1.ItemDetail.Description.Text = item.Description ?? string.Empty;
                    itemOut1.ItemDetail.UnitOfMeasure = item.UnitOfMeasure;
                    if (!string.IsNullOrEmpty(item.Classification) && item.Classification.Length > 0)
                    {
                        itemOut1.ItemDetail.Classification.Text = item.Classification.Split('-')[1] ?? "";
                        itemOut1.ItemDetail.Classification.Domain = item.Classification.Split('-')[0] ?? ""; //"UNSPSC";//--as per example document: Sampler cXML Test PO.xml
                    }
                    else
                    {
                        itemOut1.ItemDetail.Classification.Text = string.Empty;
                        itemOut1.ItemDetail.Classification.Domain = string.Empty;
                    }
                    itemOut1.ItemDetail.ManufacturerPartID.Text = item.Part_ManufacturerID.ToString();
                    itemOut1.ItemDetail.ManufacturerName.Text = item.Part_Manufacturer.ToString();//

                    lineCost += (item.OrderQuantity * item.UnitCost);
                    itemOutList.Add(itemOut1);
                }
            }
            cxml.Request.OrderRequest.OrderRequestHeader.Total.Money.Text = lineCost.ToString();
            cxml.Request.OrderRequest.ItemOut = itemOutList;
            return cxml;

        }
        public PunchoutAPIResponse postXMLData(string destinationUrl, PunchoutOrderMessageData requestXml)
        {
            PunchoutAPIResponse ro = new PunchoutAPIResponse();
            try
            {
                string ResponseText = string.Empty;
                int statusCode = 0;

                var requestString = XmlHelper.XmlSerializeFromObject<PunchoutOrderMessageData>(requestXml);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.Unicode.GetBytes(requestString);
                request.ContentType = "application/xml; encoding='utf-16'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();

                if (response != null && response.StatusDescription != null)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();


                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObject = XmlHelper.XmlDeserializeFromString<PunchoutOrderMessageResponse>(responseStr);
                        if (responseObject != null && !string.IsNullOrEmpty(responseObject.Response.Status.Code) && !string.IsNullOrEmpty(responseObject.Response.Status._Text))
                        {
                            statusCode = Convert.ToInt32(responseObject.Response.Status.Code);

                            //ro.ResponseText = responseObject.Response.Status.__Text;

                            switch (statusCode)
                            {
                                case 200:
                                    ResponseText = "OK";
                                    break;
                                case 400:
                                    ResponseText = "No XML in POST body/POST XML is not valid";
                                    break;
                                case 401:
                                    ResponseText = "Sender identity or shared secret is invalid";
                                    break;
                                case 406:
                                    ResponseText = "POST XML is not valid: ADDITIONAL_INFO";
                                    break;
                                case 500:
                                    ResponseText = "An un-handled error occurred while processing this request";
                                    break;
                                default:
                                    ResponseText = "";
                                    break;
                            }
                            ro.ResponseCode = statusCode;
                            ro.ResponseMessage = ResponseText;
                        }
                    }
                    else
                    {
                        ro.ResponseText = "An error occurred while processing your request.";
                    }

                }
                else
                {
                    ro.ResponseText = "An error occurred while processing your request.";
                }
            }
            catch
            {
                ro.ResponseText = "An error occurred while processing your request. Please check Vendor Punch Out SetUp details.";
            }
            return ro;
        }
        public void UpdatePOOnOrderSetupResponse(long purchaseOrderId, long ClientId)
        {
            PurchaseOrder purchaseorder = new DataContracts.PurchaseOrder()
            {
                ClientId = ClientId,
                PurchaseOrderId = purchaseOrderId
            };
            purchaseorder.Retrieve(userData.DatabaseKey);
            purchaseorder.SentOrderRequest = true;
            purchaseorder.Update(userData.DatabaseKey);
        }
        public void UpdatePOEventLogOnOrderSetupResponse(long purchaseOrderId, long ClientId, long SiteId)
        {
            PurchasingEventLog log = new PurchasingEventLog();
            log.ClientId = ClientId;
            log.SiteId = SiteId;
            log.ObjectId = purchaseOrderId;
            log.Event = PurchasingEvents.OrderRequestSent;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "Order Request Sent";
            log.SourceId = 0;
            log.TableName = AttachmentTableConstant.PurchaseOrder;
            log.Create(userData.DatabaseKey);
        }
        #endregion
        #region V2-653
        #region Add Purchase Order dynamic V2-653
        internal Tuple<PurchaseOrder, bool, bool> AddPurchaseOrderDynamic(PurchaseOrderVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PurchaseOrder purchaseorder = new PurchaseOrder();
            WorkFlowLog workflowlog = new WorkFlowLog();
            CustomSequentialId custid = new CustomSequentialId();
            custid.KeyList = AutoGenerateKey.PO_ANNUAL;
            var custList = custid.RetrieveByClientIdandSiteIdandKey_V2(userData.DatabaseKey);
            string POPrefix = custList != null ? custList.Where(x => x.Key == AutoGenerateKey.PO_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : "";

            purchaseorder.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            purchaseorder.ClientId = this.userData.DatabaseKey.Client.ClientId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrder, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.addPurchaseOrder);
                getpropertyInfo = objVM.addPurchaseOrder.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.addPurchaseOrder);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = purchaseorder.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(purchaseorder, val);
            }
            #region V2-929 Vendor Insurance Checking
            // 2023-07-27 - RKL - 
            // Only need to check if Site.VendorCompliance = True
            bool VendorInsuranceChecking = false;
            bool VendorAssetMgtChecking = false;
            Vendor vendor = new Vendor
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = purchaseorder.VendorId
            };
            vendor.Retrieve(_dbKey);
            if (vendor.InsuranceOverride == true)
            {
                VendorInsuranceChecking = false;
            }
            else
            {
                if (userData.Site.VendorCompliance == true && vendor.InsuranceRequired == true && (Convert.ToDateTime(vendor.InsuranceExpireDate).Date < DateTime.UtcNow.Date || vendor.InsuranceExpireDate == DateTime.MinValue))
                {
                    VendorInsuranceChecking = true;
                }
            }
            if (VendorInsuranceChecking == true)
            {
                return new Tuple<PurchaseOrder, bool, bool>(purchaseorder, VendorInsuranceChecking, VendorAssetMgtChecking);
            }
            #endregion
            #region V2-933 Vendor Asset Mgt Checking

            if (vendor.AssetMgtOverride == true)
            {
                VendorAssetMgtChecking = false;
            }
            else
            {
                if (userData.Site.VendorCompliance == true && vendor.AssetMgtRequired == true && (Convert.ToDateTime(vendor.AssetMgtExpireDate).Date < DateTime.UtcNow.Date || vendor.AssetMgtExpireDate == DateTime.MinValue))
                {
                    VendorAssetMgtChecking = true;
                }
            }
            if (VendorAssetMgtChecking == true)
            {
                return new Tuple<PurchaseOrder, bool, bool>(purchaseorder, VendorInsuranceChecking, VendorAssetMgtChecking);
            }
            #endregion
            purchaseorder.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseorder.VendorClientLookupId = objVM.addPurchaseOrder.VendorClientLookupId;
            purchaseorder.Status = PurchaseOrderStatusConstants.Open;
            purchaseorder.FOB = objVM.addPurchaseOrder.FOB ?? "";
            purchaseorder.Terms = objVM.addPurchaseOrder.Terms ?? "";
            workflowlog.ClientId = this.userData.DatabaseKey.Personnel.ClientId;
            workflowlog.ObjectName = "PurchaseOrder";
            workflowlog.Message = "Purchase Order Add";
            workflowlog.UserName = this.userData.DatabaseKey.UserName;
            purchaseorder.workflowlog = workflowlog;
            purchaseorder.CreateByPKForeignKeys_V2(this.userData.DatabaseKey, true, AutoGenerateKey.PO_ANNUAL, POPrefix);
            if (purchaseorder.ErrorMessages != null && purchaseorder.ErrorMessages.Count == 0)
            {
                CreateEventLog(purchaseorder.PurchaseOrderId, PurchasingEvents.POOpen);
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPurchaseOrderUDFDynamic(objVM.addPurchaseOrder, purchaseorder.PurchaseOrderId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        purchaseorder.ErrorMessages.AddRange(errors);
                    }
                }
            }
            //return purchaseorder;
            return new Tuple<PurchaseOrder, bool, bool>(purchaseorder, VendorInsuranceChecking, VendorAssetMgtChecking);
        }
        public List<string> AddPurchaseOrderUDFDynamic(Models.PurchaseOrder.UIConfiguration.AddPurchaseOrderModelDynamic PurchaseOrderObj, long PurchaseOrderId,
   List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            POHeaderUDF poHeaderUDF = new POHeaderUDF();
            poHeaderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            poHeaderUDF.PurchaseOrderId = PurchaseOrderId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, PurchaseOrderObj);
                getpropertyInfo = PurchaseOrderObj.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(PurchaseOrderObj);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poHeaderUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(poHeaderUDF, val);
            }
            poHeaderUDF.Create(_dbKey);
            return poHeaderUDF.ErrorMessages;
        }
        private void AssignDefaultOrNullValue(ref object val, Type t)
        {
            if (t.Equals(typeof(long?)))
            {
                val = val ?? 0;
            }
            else if (t.Equals(typeof(DateTime?)))
            {
                //val = val ?? null;
            }
            else if (t.Equals(typeof(decimal?)))
            {
                val = val ?? 0M;
            }
            else if (t.Name == "String")
            {
                val = val ?? string.Empty;
            }
        }
        #endregion
        #region View Purchase Order
        internal ViewPurchaseOrderModelDynamic GetPurchaseOrderDetailsByIdDynamic(long PurchaseOrderId)
        {
            ViewPurchaseOrderModelDynamic viewPurchaseOrderModelDynamic = new ViewPurchaseOrderModelDynamic();
            PurchaseOrder poDetails = RetrievePOByPurchaseOrderId(PurchaseOrderId);
            POHeaderUDF poHeaderUDF = RetrievePOUDFByPurchaseOrderId(PurchaseOrderId);
            viewPurchaseOrderModelDynamic = MapPurchaseOrderDataForView(viewPurchaseOrderModelDynamic, poDetails);
            viewPurchaseOrderModelDynamic = MapPOHeaderUDFDataForView(viewPurchaseOrderModelDynamic, poHeaderUDF);
            return viewPurchaseOrderModelDynamic;
        }
        public ViewPurchaseOrderModelDynamic MapPurchaseOrderDataForView(ViewPurchaseOrderModelDynamic objPurchaseOrderModelDynamic, PurchaseOrder ObjPurchaseOrder)
        {
            objPurchaseOrderModelDynamic.PurchaseOrderId = ObjPurchaseOrder.PurchaseOrderId;
            objPurchaseOrderModelDynamic.ClientLookupId = ObjPurchaseOrder.ClientLookupId;
            objPurchaseOrderModelDynamic.Attention = ObjPurchaseOrder.Attention;
            objPurchaseOrderModelDynamic.Buyer_PersonnelId = ObjPurchaseOrder.Buyer_PersonnelId;
            objPurchaseOrderModelDynamic.Carrier = ObjPurchaseOrder.Carrier;
            objPurchaseOrderModelDynamic.CompleteBy_PersonnelId = ObjPurchaseOrder.CompleteBy_PersonnelId;
            if (ObjPurchaseOrder.Required != null && ObjPurchaseOrder.Required != default(DateTime))
            {
                objPurchaseOrderModelDynamic.Required = ObjPurchaseOrder.Required;
            }
            else
            {
                objPurchaseOrderModelDynamic.Required = null;
            }
            if (ObjPurchaseOrder.CompleteDate != null && ObjPurchaseOrder.CompleteDate != default(DateTime))
            {
                objPurchaseOrderModelDynamic.CompleteDate = ObjPurchaseOrder.CompleteDate;
            }
            else
            {
                objPurchaseOrderModelDynamic.CompleteDate = null;
            }
            objPurchaseOrderModelDynamic.Creator_PersonnelId = ObjPurchaseOrder.Creator_PersonnelId;
            objPurchaseOrderModelDynamic.FOB = ObjPurchaseOrder.FOB;
            objPurchaseOrderModelDynamic.Status = ObjPurchaseOrder.Status;
            objPurchaseOrderModelDynamic.Terms = ObjPurchaseOrder.Terms;
            objPurchaseOrderModelDynamic.VendorId = ObjPurchaseOrder.VendorId;
            objPurchaseOrderModelDynamic.VoidBy_PersonnelId = ObjPurchaseOrder.VoidBy_PersonnelId;
            if (ObjPurchaseOrder.VoidDate != null && ObjPurchaseOrder.VoidDate != default(DateTime))
            {
                objPurchaseOrderModelDynamic.VoidDate = ObjPurchaseOrder.VoidDate;
            }
            else
            {
                objPurchaseOrderModelDynamic.VoidDate = null;
            }
            objPurchaseOrderModelDynamic.VoidReason = ObjPurchaseOrder.VoidReason;
            objPurchaseOrderModelDynamic.Reason = ObjPurchaseOrder.Reason;
            objPurchaseOrderModelDynamic.MessageToVendor = ObjPurchaseOrder.MessageToVendor;
            objPurchaseOrderModelDynamic.ExPurchaseOrderId = ObjPurchaseOrder.ExPurchaseOrderId;
            objPurchaseOrderModelDynamic.ExPurchaseRequest = ObjPurchaseOrder.ExPurchaseRequest;
            objPurchaseOrderModelDynamic.Currency = ObjPurchaseOrder.Currency;
            objPurchaseOrderModelDynamic.Revision = ObjPurchaseOrder.Revision;
            objPurchaseOrderModelDynamic.PaymentTerms = ObjPurchaseOrder.PaymentTerms;
            objPurchaseOrderModelDynamic.VendorName = ObjPurchaseOrder.VendorName;
            objPurchaseOrderModelDynamic.VendorClientLookupId = ObjPurchaseOrder.VendorClientLookupId;
            objPurchaseOrderModelDynamic.Creator_PersonnelName = ObjPurchaseOrder.Creator_PersonnelName;
            objPurchaseOrderModelDynamic.Completed_PersonnelName = ObjPurchaseOrder.Completed_PersonnelName;
            objPurchaseOrderModelDynamic.Buyer_PersonnelName = ObjPurchaseOrder.Buyer_PersonnelName;
            objPurchaseOrderModelDynamic.CountLineItem = ObjPurchaseOrder.CountLineItem;
            objPurchaseOrderModelDynamic.IsExternal = ObjPurchaseOrder.IsExternal;
            objPurchaseOrderModelDynamic.IsPunchOut = ObjPurchaseOrder.IsPunchout;
            objPurchaseOrderModelDynamic.TotalCost = ObjPurchaseOrder.TotalCost;
            objPurchaseOrderModelDynamic.SentOrderRequest = ObjPurchaseOrder.SentOrderRequest;
            //V2-738
            objPurchaseOrderModelDynamic.StoreroomName = ObjPurchaseOrder.StoreroomName;
            objPurchaseOrderModelDynamic.StoreroomId = ObjPurchaseOrder.StoreroomId;
            //V2-1086
            objPurchaseOrderModelDynamic.Shipto = ObjPurchaseOrder.Shipto;
            objPurchaseOrderModelDynamic.Shipto_ClientLookUpId = ObjPurchaseOrder.Shipto_ClientLookUpId;
            return objPurchaseOrderModelDynamic;
        }
        public ViewPurchaseOrderModelDynamic MapPOHeaderUDFDataForView(ViewPurchaseOrderModelDynamic viewPurchaseOrderModelDynamic, POHeaderUDF poHeaderUDF)
        {
            if (poHeaderUDF != null)
            {
                viewPurchaseOrderModelDynamic.POHeaderUDFId = poHeaderUDF.POHeaderUDFId;
                viewPurchaseOrderModelDynamic.Text1 = poHeaderUDF.Text1;
                viewPurchaseOrderModelDynamic.Text2 = poHeaderUDF.Text2;
                viewPurchaseOrderModelDynamic.Text3 = poHeaderUDF.Text3;
                viewPurchaseOrderModelDynamic.Text4 = poHeaderUDF.Text4;

                if (poHeaderUDF.Date1 != null && poHeaderUDF.Date1 == DateTime.MinValue)
                {
                    viewPurchaseOrderModelDynamic.Date1 = null;
                }
                else
                {
                    viewPurchaseOrderModelDynamic.Date1 = poHeaderUDF.Date1;
                }
                if (poHeaderUDF.Date2 != null && poHeaderUDF.Date2 == DateTime.MinValue)
                {
                    viewPurchaseOrderModelDynamic.Date2 = null;
                }
                else
                {
                    viewPurchaseOrderModelDynamic.Date2 = poHeaderUDF.Date2;
                }
                if (poHeaderUDF.Date3 != null && poHeaderUDF.Date3 == DateTime.MinValue)
                {
                    viewPurchaseOrderModelDynamic.Date3 = null;
                }
                else
                {
                    viewPurchaseOrderModelDynamic.Date3 = poHeaderUDF.Date3;
                }
                if (poHeaderUDF.Date4 != null && poHeaderUDF.Date4 == DateTime.MinValue)
                {
                    viewPurchaseOrderModelDynamic.Date4 = null;
                }
                else
                {
                    viewPurchaseOrderModelDynamic.Date4 = poHeaderUDF.Date4;
                }

                viewPurchaseOrderModelDynamic.Bit1 = poHeaderUDF.Bit1;
                viewPurchaseOrderModelDynamic.Bit2 = poHeaderUDF.Bit2;
                viewPurchaseOrderModelDynamic.Bit3 = poHeaderUDF.Bit3;
                viewPurchaseOrderModelDynamic.Bit4 = poHeaderUDF.Bit4;

                viewPurchaseOrderModelDynamic.Numeric1 = poHeaderUDF.Numeric1;
                viewPurchaseOrderModelDynamic.Numeric2 = poHeaderUDF.Numeric2;
                viewPurchaseOrderModelDynamic.Numeric3 = poHeaderUDF.Numeric3;
                viewPurchaseOrderModelDynamic.Numeric4 = poHeaderUDF.Numeric4;

                viewPurchaseOrderModelDynamic.Select1 = poHeaderUDF.Select1;
                viewPurchaseOrderModelDynamic.Select2 = poHeaderUDF.Select2;
                viewPurchaseOrderModelDynamic.Select3 = poHeaderUDF.Select3;
                viewPurchaseOrderModelDynamic.Select4 = poHeaderUDF.Select4;

                viewPurchaseOrderModelDynamic.Select1Name = poHeaderUDF.Select1Name;
                viewPurchaseOrderModelDynamic.Select2Name = poHeaderUDF.Select2Name;
                viewPurchaseOrderModelDynamic.Select3Name = poHeaderUDF.Select3Name;
                viewPurchaseOrderModelDynamic.Select4Name = poHeaderUDF.Select4Name;
            }
            return viewPurchaseOrderModelDynamic;
        }
        public PurchaseOrder RetrievePOByPurchaseOrderId(long PurchaseOrderId)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = PurchaseOrderId
            };
            purchaseOrder.RetrieveByPKForeignKeys_V2(userData.DatabaseKey, userData.Site.TimeZone);
            return purchaseOrder;
        }
        public POHeaderUDF RetrievePOUDFByPurchaseOrderId(long PurchaseOrderId)
        {
            POHeaderUDF poHeaderUDF = new POHeaderUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = PurchaseOrderId
            };

            poHeaderUDF = poHeaderUDF.RetrieveByPurchaseOrderId(this.userData.DatabaseKey);
            return poHeaderUDF;
        }
        #endregion
        #region Edit Purchase Order Dynamic
        internal EditPurchaseOrderModelDynamic GetPurchaseOrderByIdDynamic(long PurchaseOrderId)
        {
            EditPurchaseOrderModelDynamic editPurchaseOrderModelDynamic = new EditPurchaseOrderModelDynamic();
            PurchaseOrder poDetails = RetrievePOByPurchaseOrderId(PurchaseOrderId);
            POHeaderUDF poHeaderUDF = RetrievePOUDFByPurchaseOrderId(PurchaseOrderId);
            editPurchaseOrderModelDynamic = MapPurchaseOrderDataForEdit(editPurchaseOrderModelDynamic, poDetails);
            editPurchaseOrderModelDynamic = MapPOHeaderUDFDataForEdit(editPurchaseOrderModelDynamic, poHeaderUDF);
            return editPurchaseOrderModelDynamic;
        }
        public EditPurchaseOrderModelDynamic MapPurchaseOrderDataForEdit(EditPurchaseOrderModelDynamic objEditPurchaseOrderModelDynamic, PurchaseOrder ObjPurchaseOrder)
        {
            objEditPurchaseOrderModelDynamic.PurchaseOrderId = ObjPurchaseOrder.PurchaseOrderId;
            objEditPurchaseOrderModelDynamic.ClientLookupId = ObjPurchaseOrder.ClientLookupId;
            objEditPurchaseOrderModelDynamic.Attention = ObjPurchaseOrder.Attention;
            objEditPurchaseOrderModelDynamic.Buyer_PersonnelId = ObjPurchaseOrder.Buyer_PersonnelId;
            objEditPurchaseOrderModelDynamic.Carrier = ObjPurchaseOrder.Carrier;
            objEditPurchaseOrderModelDynamic.CompleteBy_PersonnelId = ObjPurchaseOrder.CompleteBy_PersonnelId;
            if (ObjPurchaseOrder.Required != null && ObjPurchaseOrder.Required != default(DateTime))
            {
                objEditPurchaseOrderModelDynamic.Required = ObjPurchaseOrder.Required;
            }
            else
            {
                objEditPurchaseOrderModelDynamic.Required = null;
            }
            if (ObjPurchaseOrder.CompleteDate != null && ObjPurchaseOrder.CompleteDate != default(DateTime))
            {
                objEditPurchaseOrderModelDynamic.CompleteDate = ObjPurchaseOrder.CompleteDate;
            }
            else
            {
                objEditPurchaseOrderModelDynamic.CompleteDate = null;
            }
            objEditPurchaseOrderModelDynamic.Creator_PersonnelId = ObjPurchaseOrder.Creator_PersonnelId;
            objEditPurchaseOrderModelDynamic.FOB = ObjPurchaseOrder.FOB;
            objEditPurchaseOrderModelDynamic.Status = ObjPurchaseOrder.Status;
            objEditPurchaseOrderModelDynamic.Terms = ObjPurchaseOrder.Terms;
            objEditPurchaseOrderModelDynamic.VendorId = ObjPurchaseOrder.VendorId;
            objEditPurchaseOrderModelDynamic.VoidBy_PersonnelId = ObjPurchaseOrder.VoidBy_PersonnelId;
            if (ObjPurchaseOrder.VoidDate != null && ObjPurchaseOrder.VoidDate != default(DateTime))
            {
                objEditPurchaseOrderModelDynamic.VoidDate = ObjPurchaseOrder.VoidDate;
            }
            else
            {
                objEditPurchaseOrderModelDynamic.VoidDate = null;
            }
            objEditPurchaseOrderModelDynamic.VoidReason = ObjPurchaseOrder.VoidReason;
            objEditPurchaseOrderModelDynamic.Reason = ObjPurchaseOrder.Reason;
            objEditPurchaseOrderModelDynamic.MessageToVendor = ObjPurchaseOrder.MessageToVendor;
            objEditPurchaseOrderModelDynamic.ExPurchaseOrderId = ObjPurchaseOrder.ExPurchaseOrderId;
            objEditPurchaseOrderModelDynamic.ExPurchaseRequest = ObjPurchaseOrder.ExPurchaseRequest;
            objEditPurchaseOrderModelDynamic.Currency = ObjPurchaseOrder.Currency;
            objEditPurchaseOrderModelDynamic.Revision = ObjPurchaseOrder.Revision;
            objEditPurchaseOrderModelDynamic.PaymentTerms = ObjPurchaseOrder.PaymentTerms;
            objEditPurchaseOrderModelDynamic.VendorName = ObjPurchaseOrder.VendorName;
            objEditPurchaseOrderModelDynamic.VendorClientLookupId = ObjPurchaseOrder.VendorClientLookupId;
            objEditPurchaseOrderModelDynamic.CountLineItem = ObjPurchaseOrder.CountLineItem;
            objEditPurchaseOrderModelDynamic.IsExternal = ObjPurchaseOrder.IsExternal;
            objEditPurchaseOrderModelDynamic.IsPunchOut = ObjPurchaseOrder.IsPunchout;
            objEditPurchaseOrderModelDynamic.TotalCost = ObjPurchaseOrder.TotalCost;
            objEditPurchaseOrderModelDynamic.SentOrderRequest = ObjPurchaseOrder.SentOrderRequest;
            //V2-738
            objEditPurchaseOrderModelDynamic.StoreroomName = ObjPurchaseOrder.StoreroomName;
            objEditPurchaseOrderModelDynamic.StoreroomId = ObjPurchaseOrder.StoreroomId;
            //V2-1086
            objEditPurchaseOrderModelDynamic.Shipto = ObjPurchaseOrder.Shipto;
            objEditPurchaseOrderModelDynamic.Shipto_ClientLookUpId = ObjPurchaseOrder.Shipto_ClientLookUpId;
            return objEditPurchaseOrderModelDynamic;
        }
        public EditPurchaseOrderModelDynamic MapPOHeaderUDFDataForEdit(EditPurchaseOrderModelDynamic objEditPurchaseOrderModelDynamic, POHeaderUDF poHeaderUDF)
        {
            if (poHeaderUDF != null)
            {
                objEditPurchaseOrderModelDynamic.POHeaderUDFId = poHeaderUDF.POHeaderUDFId;
                objEditPurchaseOrderModelDynamic.Text1 = poHeaderUDF.Text1;
                objEditPurchaseOrderModelDynamic.Text2 = poHeaderUDF.Text2;
                objEditPurchaseOrderModelDynamic.Text3 = poHeaderUDF.Text3;
                objEditPurchaseOrderModelDynamic.Text4 = poHeaderUDF.Text4;

                if (poHeaderUDF.Date1 != null && poHeaderUDF.Date1 == DateTime.MinValue)
                {
                    objEditPurchaseOrderModelDynamic.Date1 = null;
                }
                else
                {
                    objEditPurchaseOrderModelDynamic.Date1 = poHeaderUDF.Date1;
                }
                if (poHeaderUDF.Date2 != null && poHeaderUDF.Date2 == DateTime.MinValue)
                {
                    objEditPurchaseOrderModelDynamic.Date2 = null;
                }
                else
                {
                    objEditPurchaseOrderModelDynamic.Date2 = poHeaderUDF.Date2;
                }
                if (poHeaderUDF.Date3 != null && poHeaderUDF.Date3 == DateTime.MinValue)
                {
                    objEditPurchaseOrderModelDynamic.Date3 = null;
                }
                else
                {
                    objEditPurchaseOrderModelDynamic.Date3 = poHeaderUDF.Date3;
                }
                if (poHeaderUDF.Date4 != null && poHeaderUDF.Date4 == DateTime.MinValue)
                {
                    objEditPurchaseOrderModelDynamic.Date4 = null;
                }
                else
                {
                    objEditPurchaseOrderModelDynamic.Date4 = poHeaderUDF.Date4;
                }

                objEditPurchaseOrderModelDynamic.Bit1 = poHeaderUDF.Bit1;
                objEditPurchaseOrderModelDynamic.Bit2 = poHeaderUDF.Bit2;
                objEditPurchaseOrderModelDynamic.Bit3 = poHeaderUDF.Bit3;
                objEditPurchaseOrderModelDynamic.Bit4 = poHeaderUDF.Bit4;

                objEditPurchaseOrderModelDynamic.Numeric1 = poHeaderUDF.Numeric1;
                objEditPurchaseOrderModelDynamic.Numeric2 = poHeaderUDF.Numeric2;
                objEditPurchaseOrderModelDynamic.Numeric3 = poHeaderUDF.Numeric3;
                objEditPurchaseOrderModelDynamic.Numeric4 = poHeaderUDF.Numeric4;

                objEditPurchaseOrderModelDynamic.Select1 = poHeaderUDF.Select1;
                objEditPurchaseOrderModelDynamic.Select2 = poHeaderUDF.Select2;
                objEditPurchaseOrderModelDynamic.Select3 = poHeaderUDF.Select3;
                objEditPurchaseOrderModelDynamic.Select4 = poHeaderUDF.Select4;

            }
            return objEditPurchaseOrderModelDynamic;
        }

        public PurchaseOrder updatePurchaseOrderDynamic(PurchaseOrderVM objVM)
        {

            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            PurchaseOrder purchaseorder = new DataContracts.PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = objVM.EditPurchaseOrder.PurchaseOrderId
            };
            purchaseorder.Retrieve(userData.DatabaseKey);

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrder, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditPurchaseOrder);
                getpropertyInfo = objVM.EditPurchaseOrder.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPurchaseOrder);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = purchaseorder.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "VendorId")
                {
                    // 2022-12-07 - V2-815 - Can update vendor if line items exist
                    if (!(objVM.EditPurchaseOrder.IsPunchOut))// || objVM.EditPurchaseOrder.CountLineItem > 0))
                    {
                        setpropertyInfo.SetValue(purchaseorder, val);
                    }
                }
                else
                {
                    setpropertyInfo.SetValue(purchaseorder, val);
                }

            }
            purchaseorder.Update(userData.DatabaseKey);
            List<string> errors = new List<string>();
            if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                errors = EditPOHeaderUDFDynamic(objVM.EditPurchaseOrder, configDetails);

            }
            if (errors != null && errors.Count() > 0)
            {
                purchaseorder.ErrorMessages.AddRange(errors);
            }
            return purchaseorder;
        }
        public List<string> EditPOHeaderUDFDynamic(Models.PurchaseOrder.UIConfiguration.EditPurchaseOrderModelDynamic EditPO,
  List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            POHeaderUDF poHeaderUDF = new POHeaderUDF();
            poHeaderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            poHeaderUDF.PurchaseOrderId = EditPO.PurchaseOrderId;

            poHeaderUDF = poHeaderUDF.RetrieveByPurchaseOrderId(userData.DatabaseKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, EditPO);
                getpropertyInfo = EditPO.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(EditPO);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poHeaderUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(poHeaderUDF, val);
            }
            if (poHeaderUDF.PurchaseOrderId > 0)
            {
                poHeaderUDF.Update(_dbKey);
            }
            else
            {
                poHeaderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                poHeaderUDF.PurchaseOrderId = EditPO.PurchaseOrderId;
                poHeaderUDF.Create(_dbKey);
            }

            return poHeaderUDF.ErrorMessages;
        }
        #endregion
        #region Add PO Line Item (Not In Inventory)
        internal List<string> AddPartNotInInventoryDynamic(PurchaseOrderVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PurchaseOrderLineItem purchaseOrderLineItem = new PurchaseOrderLineItem();
            purchaseOrderLineItem.ClientId = this.userData.DatabaseKey.Client.ClientId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrderLineItemPartNotInInventory, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.AddPOLineItemPartNotInInventory);
                getpropertyInfo = objVM.AddPOLineItemPartNotInInventory.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.AddPOLineItemPartNotInInventory);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = purchaseOrderLineItem.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "ChargeToId")
                {
                    continue;
                }

                setpropertyInfo.SetValue(purchaseOrderLineItem, val);
            }
            purchaseOrderLineItem.UnitOfMeasure = objVM.AddPOLineItemPartNotInInventory.PurchaseUOM;
            purchaseOrderLineItem.PurchaseRequestId = objVM.AddPOLineItemPartNotInInventory.PurchaseRequestId ?? 0;
            purchaseOrderLineItem.ChargeToId = objVM.AddPOLineItemPartNotInInventory.ChargeToId ?? 0;
            purchaseOrderLineItem.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseOrderLineItem.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseOrderLineItem.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            purchaseOrderLineItem.PurchaseOrderId = objVM.AddPOLineItemPartNotInInventory.PurchaseOrderId ?? 0;
            // V2-787 - RKL - 2022-11-07
            purchaseOrderLineItem.Status = PurchaseOrderStatusConstants.Open;
            purchaseOrderLineItem.CreateWithValidation_V2(userData.DatabaseKey);
            if (purchaseOrderLineItem.ErrorMessages.Count == 0)
            {
                purchaseOrderLineItem.ReOrderLineNumber(userData.DatabaseKey);

                if (purchaseOrderLineItem.ChargeType == AttachmentTableConstant.WorkOrder && (purchaseOrderLineItem.Status == PurchaseOrderStatusConstants.Open || purchaseOrderLineItem.Status == PurchaseOrderStatusConstants.Partial))
                {
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(purchaseOrderLineItem.ChargeToId, "Add"));
                }
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPurchaseOrderLineItemUDFDynamic(objVM.AddPOLineItemPartNotInInventory, purchaseOrderLineItem.PurchaseOrderLineItemId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        purchaseOrderLineItem.ErrorMessages.AddRange(errors);
                    }
                }
            }
            return purchaseOrderLineItem.ErrorMessages;
        }
        public List<string> AddPurchaseOrderLineItemUDFDynamic(Models.PurchaseOrder.UIConfiguration.AddPOLineItemPartNotInInventoryDynamic poLineItemModelDynamic, long purchaseOrderLineItemId,
   List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            POLineUDF poLineUDF = new POLineUDF();
            poLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            poLineUDF.PurchaseOrderLineItemId = purchaseOrderLineItemId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, poLineItemModelDynamic);
                getpropertyInfo = poLineItemModelDynamic.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(poLineItemModelDynamic);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(poLineUDF, val);
            }
            poLineUDF.Create(_dbKey);
            return poLineUDF.ErrorMessages;
        }
        #endregion
        public PurchaseOrderLineItem RetrievePOLineItemByPurchaseOrderLineItemId(long PurchaseOrderLineItemId)
        {
            PurchaseOrderLineItem purchaseOrderlineitem = new PurchaseOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseOrderLineItemId = PurchaseOrderLineItemId
            };
            purchaseOrderlineitem.PurchaseOrderLineItemRetrieveByPurchaseOrderLineItemId_V2(userData.DatabaseKey);
            return purchaseOrderlineitem;
        }
        public POLineUDF RetrievePOLineItemUDFByPurchaseOrderLineItemId(long purchaseOrderLineItemId)
        {
            POLineUDF pOLineUDF = new POLineUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderLineItemId = purchaseOrderLineItemId
            };

            pOLineUDF = pOLineUDF.RetrieveByPurchaseOrderLineItemId(this.userData.DatabaseKey);
            return pOLineUDF;
        }
        #region Edit PO Line Item (Not In Inventory)
        internal EditPOLineItemPartNotInInventoryModelDynamic GetPOLineItemNotInInventoryByIdDynamic(long PurchaseOrderLineItemId, long PurchaseOrderId)
        {
            EditPOLineItemPartNotInInventoryModelDynamic editPOLineItemPartNotInInventoryModel = new EditPOLineItemPartNotInInventoryModelDynamic();
            PurchaseOrderLineItem poLineItemDetails = RetrievePOLineItemByPurchaseOrderLineItemId(PurchaseOrderLineItemId);
            POLineUDF pOLineUDF = RetrievePOLineItemUDFByPurchaseOrderLineItemId(PurchaseOrderLineItemId);
            editPOLineItemPartNotInInventoryModel = MapPOLineItemPartNotInInventoryDataForEdit(editPOLineItemPartNotInInventoryModel, poLineItemDetails, PurchaseOrderId);
            editPOLineItemPartNotInInventoryModel = MapPOLineUDFPartNotInInventoryDataForEdit(editPOLineItemPartNotInInventoryModel, pOLineUDF);
            return editPOLineItemPartNotInInventoryModel;
        }
        public EditPOLineItemPartNotInInventoryModelDynamic MapPOLineItemPartNotInInventoryDataForEdit(EditPOLineItemPartNotInInventoryModelDynamic objLineItem, PurchaseOrderLineItem purchaseOrderlineitem, long PurchaseOrderId)
        {
            objLineItem.PurchaseOrderLineItemId = purchaseOrderlineitem.PurchaseOrderLineItemId;
            objLineItem.PurchaseOrderId = PurchaseOrderId;
            objLineItem.PartId = purchaseOrderlineitem.PartId;
            objLineItem.LineNumber = purchaseOrderlineitem.LineNumber;
            objLineItem.PartClientLookupId = purchaseOrderlineitem.PartClientLookupId;
            objLineItem.Description = purchaseOrderlineitem.Description;
            objLineItem.OrderQuantity = purchaseOrderlineitem.OrderQuantity;
            objLineItem.UnitOfMeasure = purchaseOrderlineitem.UnitOfMeasure;
            objLineItem.UnitCost = purchaseOrderlineitem.UnitCost;
            objLineItem.TotalCost = purchaseOrderlineitem.TotalCost;
            if (purchaseOrderlineitem.AccountId == 0)
            {
                objLineItem.AccountId = null;
                objLineItem.AccountClientLookupId = "";
            }
            else
            {
                objLineItem.AccountId = purchaseOrderlineitem.AccountId;
                objLineItem.AccountClientLookupId = purchaseOrderlineitem.AccountClientLookupId;
            }
            objLineItem.EstimatedDelivery = purchaseOrderlineitem.EstimatedDelivery;
            objLineItem.Taxable = purchaseOrderlineitem.Taxable;
            objLineItem.ChargeType = purchaseOrderlineitem.ChargeType;
            if (purchaseOrderlineitem.ChargeToId == 0)
            {
                objLineItem.ChargeToId = null;
                objLineItem.ChargeTo_Name = "";
                objLineItem.ChargeToClientLookupId = "";
            }
            else
            {
                objLineItem.ChargeToId = purchaseOrderlineitem.ChargeToId;
                objLineItem.ChargeTo_Name = purchaseOrderlineitem.ChargeTo_Name;
                objLineItem.ChargeToClientLookupId = purchaseOrderlineitem.ChargeToClientLookupId;
            }
            objLineItem.Status = purchaseOrderlineitem.Status;
            objLineItem.PurchaseUOM = purchaseOrderlineitem.PurchaseUOM;
            objLineItem.Manufacturer = purchaseOrderlineitem.Manufacturer;
            objLineItem.SupplierPartId = purchaseOrderlineitem.SupplierPartId;
            objLineItem.SupplierPartAuxiliaryId = purchaseOrderlineitem.SupplierPartAuxiliaryId;
            objLineItem.ManufacturerPartId = purchaseOrderlineitem.ManufacturerPartId;
            objLineItem.VendorCatalogItemId = purchaseOrderlineitem.VendorCatalogItemId;
            //objLineItem.Classification = purchaseOrderlineitem.Classification; v2-717
            objLineItem.UOMConversion = purchaseOrderlineitem.UOMConversion;
            if (purchaseOrderlineitem.EstimatedDelivery != null && purchaseOrderlineitem.EstimatedDelivery.Value == default(DateTime))
            {
                objLineItem.EstimatedDelivery = null;
            }
            //V2-717
            if (purchaseOrderlineitem.UNSPSC == 0)
            {
                objLineItem.UNSPSC = null;
                objLineItem.PartCategoryMasterClientLookupId = "";
            }
            else
            {
                objLineItem.UNSPSC = purchaseOrderlineitem.UNSPSC;
                objLineItem.PartCategoryMasterClientLookupId = purchaseOrderlineitem.PartCategoryMasterClientLookupId;
            }


            return objLineItem;
        }
        private EditPOLineItemPartNotInInventoryModelDynamic MapPOLineUDFPartNotInInventoryDataForEdit(EditPOLineItemPartNotInInventoryModelDynamic ediPOLineItemModel, POLineUDF pOLineUDF)
        {
            if (pOLineUDF != null)
            {
                ediPOLineItemModel.POLineUDFId = pOLineUDF.POLineUDFId;

                ediPOLineItemModel.Text1 = pOLineUDF.Text1;
                ediPOLineItemModel.Text2 = pOLineUDF.Text2;
                ediPOLineItemModel.Text3 = pOLineUDF.Text3;
                ediPOLineItemModel.Text4 = pOLineUDF.Text4;

                if (pOLineUDF.Date1 != null && pOLineUDF.Date1 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date1 = null;
                }
                else
                {
                    ediPOLineItemModel.Date1 = pOLineUDF.Date1;
                }
                if (pOLineUDF.Date2 != null && pOLineUDF.Date2 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date2 = null;
                }
                else
                {
                    ediPOLineItemModel.Date2 = pOLineUDF.Date2;
                }
                if (pOLineUDF.Date3 != null && pOLineUDF.Date3 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date3 = null;
                }
                else
                {
                    ediPOLineItemModel.Date3 = pOLineUDF.Date3;
                }
                if (pOLineUDF.Date4 != null && pOLineUDF.Date4 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date4 = null;
                }
                else
                {
                    ediPOLineItemModel.Date4 = pOLineUDF.Date4;
                }

                ediPOLineItemModel.Bit1 = pOLineUDF.Bit1;
                ediPOLineItemModel.Bit2 = pOLineUDF.Bit2;
                ediPOLineItemModel.Bit3 = pOLineUDF.Bit3;
                ediPOLineItemModel.Bit4 = pOLineUDF.Bit4;

                ediPOLineItemModel.Numeric1 = pOLineUDF.Numeric1;
                ediPOLineItemModel.Numeric2 = pOLineUDF.Numeric2;
                ediPOLineItemModel.Numeric3 = pOLineUDF.Numeric3;
                ediPOLineItemModel.Numeric4 = pOLineUDF.Numeric4;

                ediPOLineItemModel.Select1 = pOLineUDF.Select1;
                ediPOLineItemModel.Select2 = pOLineUDF.Select2;
                ediPOLineItemModel.Select3 = pOLineUDF.Select3;
                ediPOLineItemModel.Select4 = pOLineUDF.Select4;
            }
            return ediPOLineItemModel;
        }
        internal PurchaseOrderLineItem UpdatePOPartNotInInventoryDynamic(PurchaseOrderVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            PurchaseOrderLineItem poLineItem = new PurchaseOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseOrderLineItemId = objVM.EditPOLineItemPartNotInInventory.PurchaseOrderLineItemId ?? 0
            };
            poLineItem.Retrieve(_dbKey);
            // RKL - 2022-Dec-06 - V2-814
            // Old Charge Type and OldChargToId must be set before the POLineItem datacontract 
            // is update using the data entered on the page
            string OldChargeType = poLineItem.ChargeType ?? string.Empty;
            long OldChargeToId = poLineItem.ChargeToId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrderLineItemPartNotInInventory, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditPOLineItemPartNotInInventory);
                getpropertyInfo = objVM.EditPOLineItemPartNotInInventory.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPOLineItemPartNotInInventory);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poLineItem.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "ChargeToId")
                {
                    continue;
                }
                if (item.ColumnName == "EstimatedDelivery")
                {
                    continue;
                }

                setpropertyInfo.SetValue(poLineItem, val);
            }
            string status = poLineItem.Status ?? string.Empty;
            // RKL - 2022-Dec-06 - V2-814
            // Old Charge Type and OldChargToId must be set before the POLineItem datacontract 
            // is update using the data entered on the page
            //string OldChargeType = poLineItem.ChargeType ?? string.Empty;
            //long OldChargeToId = poLineItem.ChargeToId;
            string NewChargeType = objVM.EditPOLineItemPartNotInInventory.ChargeType ?? string.Empty;
            long NewChargeToId = objVM.EditPOLineItemPartNotInInventory.ChargeToId ?? 0;
            poLineItem.ChargeToId = objVM.EditPOLineItemPartNotInInventory.ChargeToId ?? 0;
            //poLineItem.PurchaseUOM = objVM.EditPOLineItemPartNotInInventory.PurchaseUOM ?? "";
            if (objVM.EditPOLineItemPartNotInInventory.EstimatedDelivery != null && objVM.EditPOLineItemPartNotInInventory.EstimatedDelivery.Value == default(DateTime))
            {
                poLineItem.EstimatedDelivery = null;
            }
            else
            {
                poLineItem.EstimatedDelivery = objVM.EditPOLineItemPartNotInInventory.EstimatedDelivery;
            }
            poLineItem.UnitOfMeasure = objVM.EditPOLineItemPartNotInInventory.PurchaseUOM;
            poLineItem.UpdateByPKForeignKeys(this.userData.DatabaseKey);

            if (status == PurchaseOrderStatusConstants.Open || status == PurchaseOrderStatusConstants.Partial)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                if (OldChargeType == AttachmentTableConstant.WorkOrder)
                {
                    if (NewChargeType != AttachmentTableConstant.WorkOrder)
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(OldChargeToId, "Minus"));
                    }
                    else
                    {
                        if (OldChargeToId != NewChargeToId)
                        {
                            Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(OldChargeToId, "Minus"));
                            Task task2 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(NewChargeToId, "Add"));
                            Task.WaitAll(task1, task2);
                        }
                    }
                }
                else
                {

                    if (NewChargeType == AttachmentTableConstant.WorkOrder)
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(NewChargeToId, "Add"));
                    }
                }
            }
            List<string> errors = new List<string>();
            if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                errors = UpdatePOLineUDFPartNotInInventoryDynamic(objVM.EditPOLineItemPartNotInInventory, configDetails);

            }
            if (errors != null && errors.Count() > 0)
            {
                poLineItem.ErrorMessages.AddRange(errors);
            }
            return poLineItem;
        }
        public List<string> UpdatePOLineUDFPartNotInInventoryDynamic(Models.PurchaseOrder.UIConfiguration.EditPOLineItemPartNotInInventoryModelDynamic poLineItem,
 List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            POLineUDF poLineUDF = new POLineUDF();
            poLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            poLineUDF.PurchaseOrderLineItemId = poLineItem.PurchaseOrderLineItemId ?? 0;

            poLineUDF = poLineUDF.RetrieveByPurchaseOrderLineItemId(userData.DatabaseKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, poLineItem);
                getpropertyInfo = poLineItem.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(poLineItem);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(poLineUDF, val);
            }
            if (poLineUDF.PurchaseOrderLineItemId > 0)
            {
                poLineUDF.Update(_dbKey);
            }
            else
            {
                poLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                poLineUDF.PurchaseOrderLineItemId = poLineItem.PurchaseOrderLineItemId ?? 0;
                poLineUDF.Create(_dbKey);
            }

            return poLineUDF.ErrorMessages;
        }

        #endregion
        #region Edit PO Line Item (In Inventory)
        internal EditPOLineItemPartInInventoryModelDynamic GetPOLineItemInInventoryByIdDynamic(long PurchaseOrderLineItemId, long PurchaseOrderId)
        {
            EditPOLineItemPartInInventoryModelDynamic editPOLineItemPartInInventoryModel = new EditPOLineItemPartInInventoryModelDynamic();
            PurchaseOrderLineItem poLineItemDetails = RetrievePOLineItemByPurchaseOrderLineItemId(PurchaseOrderLineItemId);
            POLineUDF pOLineUDF = RetrievePOLineItemUDFByPurchaseOrderLineItemId(PurchaseOrderLineItemId);
            editPOLineItemPartInInventoryModel = MapPOLineItemPartInInventoryDataForEdit(editPOLineItemPartInInventoryModel, poLineItemDetails, PurchaseOrderId);
            editPOLineItemPartInInventoryModel = MapPOLineUDFPartInInventoryDataForEdit(editPOLineItemPartInInventoryModel, pOLineUDF);
            return editPOLineItemPartInInventoryModel;
        }
        public EditPOLineItemPartInInventoryModelDynamic MapPOLineItemPartInInventoryDataForEdit(EditPOLineItemPartInInventoryModelDynamic objLineItem, PurchaseOrderLineItem purchaseOrderlineitem, long PurchaseOrderId)
        {
            objLineItem.PurchaseOrderLineItemId = purchaseOrderlineitem.PurchaseOrderLineItemId;
            objLineItem.PurchaseOrderId = PurchaseOrderId;
            if (purchaseOrderlineitem.PartId == 0)
            {
                objLineItem.PartId = null;
                objLineItem.PartClientLookupId = "";
            }
            else
            {
                objLineItem.PartId = purchaseOrderlineitem.PartId;
                objLineItem.PartClientLookupId = purchaseOrderlineitem.PartClientLookupId;
            }
            objLineItem.LineNumber = purchaseOrderlineitem.LineNumber;
            objLineItem.Description = purchaseOrderlineitem.Description;
            objLineItem.OrderQuantity = purchaseOrderlineitem.OrderQuantity;
            objLineItem.UnitOfMeasure = purchaseOrderlineitem.UnitOfMeasure;
            objLineItem.UnitCost = purchaseOrderlineitem.UnitCost;
            objLineItem.TotalCost = purchaseOrderlineitem.TotalCost;
            if (purchaseOrderlineitem.AccountId == 0)
            {
                objLineItem.AccountId = null;
                objLineItem.AccountClientLookupId = "";
            }
            else
            {
                objLineItem.AccountId = purchaseOrderlineitem.AccountId;
                objLineItem.AccountClientLookupId = purchaseOrderlineitem.AccountClientLookupId;
            }
            objLineItem.EstimatedDelivery = purchaseOrderlineitem.EstimatedDelivery;
            objLineItem.Taxable = purchaseOrderlineitem.Taxable;
            objLineItem.Status = purchaseOrderlineitem.Status;
            objLineItem.PurchaseUOM = purchaseOrderlineitem.PurchaseUOM;
            objLineItem.Manufacturer = purchaseOrderlineitem.Manufacturer;
            objLineItem.SupplierPartId = purchaseOrderlineitem.SupplierPartId;
            objLineItem.SupplierPartAuxiliaryId = purchaseOrderlineitem.SupplierPartAuxiliaryId;
            objLineItem.ManufacturerPartId = purchaseOrderlineitem.ManufacturerPartId;
            objLineItem.VendorCatalogItemId = purchaseOrderlineitem.VendorCatalogItemId;
            //objLineItem.Classification = purchaseOrderlineitem.Classification; v2-717
            objLineItem.UOMConversion = purchaseOrderlineitem.UOMConversion;
            if (purchaseOrderlineitem.EstimatedDelivery != null && purchaseOrderlineitem.EstimatedDelivery.Value == default(DateTime))
            {
                objLineItem.EstimatedDelivery = null;
            }
            //V2-717
            if (purchaseOrderlineitem.UNSPSC == 0)
            {
                objLineItem.UNSPSC = null;
                objLineItem.PartCategoryMasterClientLookupId = "";
            }
            else
            {
                objLineItem.UNSPSC = purchaseOrderlineitem.UNSPSC;
                objLineItem.PartCategoryMasterClientLookupId = purchaseOrderlineitem.PartCategoryMasterClientLookupId;
            }

            return objLineItem;
        }
        private EditPOLineItemPartInInventoryModelDynamic MapPOLineUDFPartInInventoryDataForEdit(EditPOLineItemPartInInventoryModelDynamic ediPOLineItemModel, POLineUDF pOLineUDF)
        {
            if (pOLineUDF != null)
            {
                ediPOLineItemModel.POLineUDFId = pOLineUDF.POLineUDFId;

                ediPOLineItemModel.Text1 = pOLineUDF.Text1;
                ediPOLineItemModel.Text2 = pOLineUDF.Text2;
                ediPOLineItemModel.Text3 = pOLineUDF.Text3;
                ediPOLineItemModel.Text4 = pOLineUDF.Text4;

                if (pOLineUDF.Date1 != null && pOLineUDF.Date1 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date1 = null;
                }
                else
                {
                    ediPOLineItemModel.Date1 = pOLineUDF.Date1;
                }
                if (pOLineUDF.Date2 != null && pOLineUDF.Date2 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date2 = null;
                }
                else
                {
                    ediPOLineItemModel.Date2 = pOLineUDF.Date2;
                }
                if (pOLineUDF.Date3 != null && pOLineUDF.Date3 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date3 = null;
                }
                else
                {
                    ediPOLineItemModel.Date3 = pOLineUDF.Date3;
                }
                if (pOLineUDF.Date4 != null && pOLineUDF.Date4 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date4 = null;
                }
                else
                {
                    ediPOLineItemModel.Date4 = pOLineUDF.Date4;
                }

                ediPOLineItemModel.Bit1 = pOLineUDF.Bit1;
                ediPOLineItemModel.Bit2 = pOLineUDF.Bit2;
                ediPOLineItemModel.Bit3 = pOLineUDF.Bit3;
                ediPOLineItemModel.Bit4 = pOLineUDF.Bit4;

                ediPOLineItemModel.Numeric1 = pOLineUDF.Numeric1;
                ediPOLineItemModel.Numeric2 = pOLineUDF.Numeric2;
                ediPOLineItemModel.Numeric3 = pOLineUDF.Numeric3;
                ediPOLineItemModel.Numeric4 = pOLineUDF.Numeric4;

                ediPOLineItemModel.Select1 = pOLineUDF.Select1;
                ediPOLineItemModel.Select2 = pOLineUDF.Select2;
                ediPOLineItemModel.Select3 = pOLineUDF.Select3;
                ediPOLineItemModel.Select4 = pOLineUDF.Select4;
            }
            return ediPOLineItemModel;
        }
        internal PurchaseOrderLineItem UpdatePOPartInInventoryDynamic(PurchaseOrderVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            PurchaseOrderLineItem poLineItem = new PurchaseOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseOrderLineItemId = objVM.EditPOLineItemPartInInventory.PurchaseOrderLineItemId ?? 0
            };
            poLineItem.Retrieve(_dbKey);
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrderLineItemPartInInventory, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditPOLineItemPartInInventory);
                getpropertyInfo = objVM.EditPOLineItemPartInInventory.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPOLineItemPartInInventory);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poLineItem.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "EstimatedDelivery")
                {
                    continue;
                }

                setpropertyInfo.SetValue(poLineItem, val);
            }
            string status = poLineItem.Status ?? string.Empty;
            string OldChargeType = poLineItem.ChargeType ?? string.Empty;
            long OldChargeToId = poLineItem.ChargeToId;
            string NewChargeType = objVM.EditPOLineItemPartInInventory.ChargeType ?? string.Empty;
            long NewChargeToId = objVM.EditPOLineItemPartInInventory.ChargeToId ?? 0;
            if (objVM.EditPOLineItemPartInInventory.EstimatedDelivery != null && objVM.EditPOLineItemPartInInventory.EstimatedDelivery.Value == default(DateTime))
            {
                poLineItem.EstimatedDelivery = null;
            }
            else
            {
                poLineItem.EstimatedDelivery = objVM.EditPOLineItemPartInInventory.EstimatedDelivery;
            }
            poLineItem.UnitOfMeasure = objVM.EditPOLineItemPartInInventory.PurchaseUOM;
            poLineItem.UpdateByPKForeignKeys(this.userData.DatabaseKey);

            if (status == PurchaseOrderStatusConstants.Open || status == PurchaseOrderStatusConstants.Partial)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                if (OldChargeType == AttachmentTableConstant.WorkOrder)
                {
                    if (NewChargeType != AttachmentTableConstant.WorkOrder)
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(OldChargeToId, "Minus"));
                    }
                    else
                    {
                        if (OldChargeToId != NewChargeToId)
                        {
                            Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(OldChargeToId, "Minus"));
                            Task task2 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(NewChargeToId, "Add"));
                            Task.WaitAll(task1, task2);
                        }
                    }
                }
                else
                {

                    if (NewChargeType == AttachmentTableConstant.WorkOrder)
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(NewChargeToId, "Add"));
                    }
                }
            }
            List<string> errors = new List<string>();
            if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                errors = UpdatePOLineUDFPartInInventoryDynamic(objVM.EditPOLineItemPartInInventory, configDetails);

            }
            if (errors != null && errors.Count() > 0)
            {
                poLineItem.ErrorMessages.AddRange(errors);
            }
            return poLineItem;
        }
        public List<string> UpdatePOLineUDFPartInInventoryDynamic(Models.PurchaseOrder.UIConfiguration.EditPOLineItemPartInInventoryModelDynamic poLineItem,
 List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            POLineUDF poLineUDF = new POLineUDF();
            poLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            poLineUDF.PurchaseOrderLineItemId = poLineItem.PurchaseOrderLineItemId ?? 0;

            poLineUDF = poLineUDF.RetrieveByPurchaseOrderLineItemId(userData.DatabaseKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, poLineItem);
                getpropertyInfo = poLineItem.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(poLineItem);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(poLineUDF, val);
            }
            if (poLineUDF.PurchaseOrderLineItemId > 0)
            {
                poLineUDF.Update(_dbKey);
            }
            else
            {
                poLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                poLineUDF.PurchaseOrderLineItemId = poLineItem.PurchaseOrderLineItemId ?? 0;
                poLineUDF.Create(_dbKey);
            }

            return poLineUDF.ErrorMessages;
        }
        #endregion

        #endregion

        #region V2-738
        public List<POLineItemModel> PopulatePOLineItemForMultiStoreroom(long PurchaseOrderId, long StoreroomId = 0)
        {
            POLineItemModel polineModel;
            List<POLineItemModel> POLineItemList = new List<POLineItemModel>();
            PurchaseOrderLineItem purchaseorder = new PurchaseOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = PurchaseOrderId,
                StoreroomId = StoreroomId
            };
            List<PurchaseOrderLineItem> purchaseorderitemlist = PurchaseOrderLineItem.PurchaseOrderLineItemRetrieveByPurchaseOrderIdForMultiStoreroom_V2(this.userData.DatabaseKey, purchaseorder);
            foreach (var po in purchaseorderitemlist)
            {
                polineModel = new POLineItemModel();
                polineModel.PurchaseOrderId = po.PurchaseOrderId;
                polineModel.PurchaseOrderLineItemId = po.PurchaseOrderLineItemId;
                polineModel.Status = po.Status;
                polineModel.LineNumber = po.LineNumber;
                polineModel.PartClientLookupId = po.PartClientLookupId;
                polineModel.Description = po.Description;
                polineModel.OrderQuantity = Math.Round(po.OrderQuantity, 2);
                polineModel.UnitOfMeasure = po.UnitOfMeasure;
                polineModel.UnitCost = Math.Round(po.UnitCost, 2);
                polineModel.TotalCost = Math.Round(po.TotalCost, 2);
                polineModel.Status_Display = po.Status_Display;
                polineModel.PartId = po.PartId;
                polineModel.QuantityBackOrdered = po.QuantityBackOrdered;
                polineModel.QuantityReceived = po.QuantityReceived;
                polineModel.QuantityToDate = po.QuantityToDate;
                polineModel.StoreroomId = po.StoreroomId;
                polineModel.CurrentAverageCost = po.CurrentAverageCost;
                polineModel.CurrentAppliedCost = po.CurrentAppliedCost;
                polineModel.CurrentOnHandQuantity = po.CurrentOnHandQuantity;
                polineModel.AccountId = po.AccountId;
                polineModel.Creator_PersonnelId = po.Creator_PersonnelId;
                polineModel.PartStoreroomId = po.PartStoreroomId;
                polineModel.Description = po.Description;
                polineModel.StockType = po.StockType;
                polineModel.UnitOfMeasure = po.UnitOfMeasure;
                polineModel.ChargeToId = po.ChargeToId;
                polineModel.ChargeType = po.ChargeType;
                polineModel.Part_Manufacturer = po.Part_Manufacturer;
                polineModel.Part_ManufacturerID = po.Part_ManufacturerID;
                polineModel.SupplierPartId = po.SupplierPartId;
                polineModel.SupplierPartAuxiliaryId = po.SupplierPartAuxiliaryId;
                //polineModel.Classification = po.Classification; V2-717
                polineModel.AccountClientLookupId = po.AccountClientLookupId;
                polineModel.UOMConversion = po.UOMConversion;
                polineModel.PurchaseUOM = po.PurchaseUOM;
                polineModel.PurchaseQuantity = po.PurchaseQuantity;
                polineModel.PurchaseCost = po.PurchaseCost;
                polineModel.LineTotal = po.LineTotal;
                polineModel.PartNumber = po.PartNumber;
                polineModel.EstimatedDelivery = DateTime.MinValue == po.EstimatedDelivery ? null : po.EstimatedDelivery;
                polineModel.ChargeToClientLookupId = po.ChargeToClientLookupId;//Added in V2-672
                POLineItemList.Add(polineModel);
            }
            return POLineItemList;
        }
        #endregion

        #region V2-796
        public PurchaseOrderUpdateModel getPurchaseOderDetailsByIdForUpdateDetails(long _purchaseOrderId)
        {
            PurchaseOrderUpdateModel purchaseOrderModel = new PurchaseOrderUpdateModel();
            PurchaseOrder po = new PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = _purchaseOrderId
            };
            po.RetrieveByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);
            purchaseOrderModel = initializeControlsForUpdateDetails(po);
            return purchaseOrderModel;
        }
        public PurchaseOrderUpdateModel initializeControlsForUpdateDetails(PurchaseOrder po)
        {
            PurchaseOrderUpdateModel purchaseOrderModel = new PurchaseOrderUpdateModel();
            purchaseOrderModel.PurchaseOrderId = po.PurchaseOrderId;
            purchaseOrderModel.ClientLookupId = po.ClientLookupId;
            purchaseOrderModel.Attention = po.Attention;
            purchaseOrderModel.Buyer_PersonnelId = po.Buyer_PersonnelId;
            purchaseOrderModel.Carrier = po.Carrier;
            purchaseOrderModel.Required = po.Required;
            purchaseOrderModel.FOB = po.FOB;
            purchaseOrderModel.Terms = po.Terms;
            purchaseOrderModel.Reason = po.Reason;
            purchaseOrderModel.MessageToVendor = po.MessageToVendor;
            purchaseOrderModel.SiteId = po.SiteId;
            purchaseOrderModel.StoreroomId = po.StoreroomId;
            return purchaseOrderModel;
        }
        public PurchaseOrder UpdatepurchaseOrderDetails(PurchaseOrderUpdateModel pOModel)
        {
            PurchaseOrder purchaseorder = new DataContracts.PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = pOModel.PurchaseOrderId
            };
            purchaseorder.Retrieve(userData.DatabaseKey);
            purchaseorder.Attention = pOModel.Attention ?? "";
            purchaseorder.Carrier = pOModel.Carrier ?? "";
            purchaseorder.FOB = pOModel.FOB ?? "";
            purchaseorder.Terms = pOModel.Terms ?? ""; ;
            purchaseorder.Buyer_PersonnelId = pOModel.Buyer_PersonnelId ?? 0;
            purchaseorder.Reason = pOModel.Reason ?? "";
            purchaseorder.MessageToVendor = pOModel.MessageToVendor ?? "";
            purchaseorder.Required = pOModel.Required;
            purchaseorder.UpdateIndex = purchaseorder.UpdateIndex;
            purchaseorder.Update(userData.DatabaseKey);
            return purchaseorder;
        }
        #endregion

        #region V2-884 UnVoid
        public string UpdateUnvoidStatus(long PurchaseOrderId)
        {
            string UpdateMsg = string.Empty;
            #region Update PO
            PurchaseOrder purchaseorder = new PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = PurchaseOrderId
            };
            purchaseorder.Retrieve(userData.DatabaseKey);
            purchaseorder.Status = PurchaseOrderStatusConstants.Open;
            purchaseorder.VoidBy_PersonnelId = 0;
            purchaseorder.VoidDate = System.DateTime.UtcNow;
            purchaseorder.VoidReason = " ";
            purchaseorder.Update(userData.DatabaseKey);
            #endregion
            #region Update lineItemPO
            PurchaseOrderLineItem p = new PurchaseOrderLineItem();
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            p.PurchaseOrderId = PurchaseOrderId;
            List<PurchaseOrderLineItem> plist = PurchaseOrderLineItem.PurchaseOrderLineItemRetrieveByPurchaseOrderId_V2(userData.DatabaseKey, p);
            foreach (var item in plist)
            {
                PurchaseOrderLineItem purchaselineitem = new PurchaseOrderLineItem()
                {
                    ClientId = this.userData.DatabaseKey.User.ClientId,
                    PurchaseOrderId = PurchaseOrderId,
                    PurchaseOrderLineItemId = item.PurchaseOrderLineItemId

                };
                purchaselineitem.Retrieve(userData.DatabaseKey);
                purchaselineitem.OrderQuantity = purchaselineitem.OrderQuantityOriginal;
                purchaselineitem.Update(this.userData.DatabaseKey);
            }
            #endregion
            #region Add Purchasing EventLog
            PurchasingEventLog log = new PurchasingEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = PurchaseOrderId;
            log.Event = "UnVoid";
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "UnVoid Purchase Order";
            log.SourceId = 0;
            log.TableName = AttachmentTableConstant.PurchaseOrder;
            log.Create(userData.DatabaseKey);
            #endregion
            UpdateMsg = "success";
            return UpdateMsg;
        }


        #endregion

        public PurchaseOrderVM RetrieveAllByPurchaseOrderV2Print(List<long> PurchaseOrderIDList = null)
        {
            PurchaseOrderVM objPurchaseOrderVM = new PurchaseOrderVM();
            foreach (var item in PurchaseOrderIDList)
            {
                long PurchaseOrderId = item;

                PurchaseOrderWrapper pWrapper = new PurchaseOrderWrapper(userData);
                CommonWrapper comWrapper = new CommonWrapper(userData);
                PurchaseOrderModel objPOModel = new PurchaseOrderModel();
                objPOModel = pWrapper.getPurchaseOderDetailsById(PurchaseOrderId);
                objPOModel.AzureImageURL = comWrapper.GetClientLogoUrl();

                if (objPOModel.Required != null && objPOModel.Required.Value == default(DateTime))
                {
                    objPOModel.Required = null;
                }
                if (objPOModel != null)
                {
                    objPurchaseOrderVM.PurchaseOrderModel = objPOModel;
                }
                #region LineItems
                List<POLineItemModel> lst = new List<POLineItemModel>();
                if (userData.DatabaseKey.Client.UseMultiStoreroom) /*738*/
                {
                    lst = pWrapper.PopulatePOLineItemForMultiStoreroom(PurchaseOrderId, objPOModel.StoreroomId ?? 0);
                }
                else
                {
                    lst = pWrapper.PopulatePOLineItem(PurchaseOrderId);
                }
                if (lst != null)
                {
                    objPurchaseOrderVM.POLineItemList = lst;
                }
                #endregion

            }

            return objPurchaseOrderVM;
        }

        public PurchaseOrder RetrieveAllByPOV2Print(List<long> PurchaseOrderIDList = null)
        {
            PurchaseOrder po = new PurchaseOrder();
            po.ClientId = userData.DatabaseKey.Client.ClientId;
            po.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            po.PurchaseOrderIDList = PurchaseOrderIDList != null && PurchaseOrderIDList.Count > 0 ? string.Join(",", PurchaseOrderIDList) : string.Empty;
            po.RetrieveAllByPurchaseOrdeV2Print(userData.DatabaseKey, userData.Site.TimeZone);
            return po;
        }
        #region V2-947
        public POReceipt RetrieveAllPrintForPOreceipt(long PurchaseOrderId, long POReceiptHeaderId)
        {
            POReceipt por = new POReceipt();
            por.ClientId = userData.DatabaseKey.Client.ClientId;
            por.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            por.PurchaseOrderId = PurchaseOrderId;
            por.POReceiptHeaderId = POReceiptHeaderId;
            var result = por.RetrievePORPrintV2(userData.DatabaseKey, userData.Site.TimeZone);
            return result;
        }
        #endregion
        #region V2-1047 PO Model
        public Tuple<string, long> ConvertPOtoPR(long PurchaseOrderId, PurchaseOrderVM objPurchaseOrderVM)
        {
            string SuccessMsg = string.Empty;
            #region Retrieve PurchaseOrder Record 
            PurchaseOrder purchaseorder = new PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = PurchaseOrderId
            };
            purchaseorder.Retrieve(userData.DatabaseKey);
            long purchaseRequestId = 0;
            long poClientId = purchaseorder.ClientId;
            long poSiteId = purchaseorder.SiteId;
            if (purchaseorder != null && purchaseorder.PurchaseOrderId > 0)
            {
                //Creating New Record In PurchaseRequest Table
                PurchaseRequest purchaseRequest = CreatePurchaseRequest(purchaseorder);
                purchaseRequestId = purchaseRequest.PurchaseRequestId;
            }
            #endregion
            #region  retrieve the Record from  POHeaderUDF
            POHeaderUDF POHeaderUDFobj = RetrievePOUDFByPurchaseOrderId(PurchaseOrderId);
            if (POHeaderUDFobj != null && POHeaderUDFobj.POHeaderUDFId > 0)
            {
                //Creating New Record In PRHeaderUDF Table
                CreatePRHeaderUDF(purchaseRequestId, poClientId, POHeaderUDFobj);
            }
            #endregion
            #region Retrieve lineItemPO

            PurchaseOrderLineItem p = new PurchaseOrderLineItem();
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            p.PurchaseOrderId = PurchaseOrderId;
            p.StockType = "all";
            List<PurchaseOrderLineItem> plist = PurchaseOrderLineItem.PurchaseOrderLineItemRetrieveByPurchaseOrderIdForDirectLineItems(userData.DatabaseKey, p);
            foreach (var item in plist)
            {
                long AccountID = item.AccountId;
                long ChargeToId = item.ChargeToId;
                string ChargeType = item.ChargeType;

                #region  DirectBuyItems(NonStock having partId=0)
                if (objPurchaseOrderVM.POLineItemList != null && objPurchaseOrderVM.POLineItemList.Count > 0)
                {
                    var DirectBuyItems = objPurchaseOrderVM.POLineItemList.Where(m => m.PurchaseOrderLineItemId == item.PurchaseOrderLineItemId && m.PurchaseOrderId == item.PurchaseOrderId).FirstOrDefault();
                    if (DirectBuyItems != null)
                    {
                        AccountID = DirectBuyItems.AccountId ?? 0;
                        ChargeToId = DirectBuyItems.ChargeToId ?? 0;
                        ChargeType = DirectBuyItems.ChargeType;
                    }
                }
                #endregion

                var PurchaseRequestLineItem = CreatePRLineItem(item, AccountID, ChargeToId, ChargeType, purchaseRequestId);
                #region Retrieve the PoLineUDF Record
                POLineUDF POLineUDFobj = RetrievePOLineItemUDFByPurchaseOrderLineItemId(item.PurchaseOrderLineItemId);
                if (POLineUDFobj != null && POLineUDFobj.POLineUDFId > 0)
                {
                    //Save in PRLineUDF
                    CreatePRLineUDF(poClientId, POLineUDFobj, PurchaseRequestLineItem);

                }
                #endregion
            }
            #endregion
            #region Add Purchase Request EventLog
            CreatePurchasingEventLog(PurchaseOrderId, purchaseRequestId, poClientId, poSiteId);
            #endregion
            SuccessMsg = "success";
            return Tuple.Create(SuccessMsg, purchaseRequestId);


        }


        public List<POLineItemModel> PopulatePOLineItemForDirectLineItems(long PurchaseOrderId, string StockType)
        {
            POLineItemModel polineModel;
            List<POLineItemModel> POLineItemList = new List<POLineItemModel>();
            PurchaseOrderLineItem p = new PurchaseOrderLineItem();
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            p.PurchaseOrderId = PurchaseOrderId;
            p.StockType = StockType;
            List<PurchaseOrderLineItem> plist = PurchaseOrderLineItem.PurchaseOrderLineItemRetrieveByPurchaseOrderIdForDirectLineItems(userData.DatabaseKey, p);
            foreach (var po in plist)
            {
                polineModel = new POLineItemModel();
                polineModel.PurchaseOrderId = po.PurchaseOrderId;
                polineModel.PurchaseOrderLineItemId = po.PurchaseOrderLineItemId;
                polineModel.Status = po.Status;
                polineModel.LineNumber = po.LineNumber;
                polineModel.PartClientLookupId = po.PartClientLookupId;
                polineModel.Description = po.Description;
                polineModel.OrderQuantity = Math.Round(po.OrderQuantity, 2);
                polineModel.UnitOfMeasure = po.UnitOfMeasure;
                polineModel.UnitCost = Math.Round(po.UnitCost, 2);
                polineModel.TotalCost = Math.Round(po.TotalCost, 2);
                polineModel.Status_Display = po.Status_Display;
                polineModel.PartId = po.PartId;
                polineModel.QuantityBackOrdered = po.QuantityBackOrdered;
                polineModel.QuantityReceived = po.QuantityReceived;
                polineModel.QuantityToDate = po.QuantityToDate;
                polineModel.StoreroomId = po.StoreroomId;
                polineModel.CurrentAverageCost = po.CurrentAverageCost;
                polineModel.CurrentAppliedCost = po.CurrentAppliedCost;
                polineModel.CurrentOnHandQuantity = po.CurrentOnHandQuantity;
                polineModel.AccountId = po.AccountId;
                polineModel.Creator_PersonnelId = po.Creator_PersonnelId;
                polineModel.PartStoreroomId = po.PartStoreroomId;
                polineModel.Description = po.Description;
                polineModel.StockType = po.StockType;
                polineModel.UnitOfMeasure = po.UnitOfMeasure;
                polineModel.ChargeToId = po.ChargeToId;
                polineModel.ChargeType = po.ChargeType;
                polineModel.Part_Manufacturer = po.Part_Manufacturer;
                polineModel.Part_ManufacturerID = po.Part_ManufacturerID;
                polineModel.SupplierPartId = po.SupplierPartId;
                polineModel.SupplierPartAuxiliaryId = po.SupplierPartAuxiliaryId;
                //polineModel.Classification = po.Classification; V2-717
                polineModel.AccountClientLookupId = po.AccountClientLookupId;
                polineModel.UOMConversion = po.UOMConversion;
                polineModel.PurchaseUOM = po.PurchaseUOM;
                polineModel.PurchaseQuantity = po.PurchaseQuantity;
                polineModel.PurchaseCost = po.PurchaseCost;
                polineModel.LineTotal = po.LineTotal;
                polineModel.PartNumber = po.PartNumber;
                polineModel.EstimatedDelivery = DateTime.MinValue == po.EstimatedDelivery ? null : po.EstimatedDelivery;
                polineModel.ChargeToClientLookupId = po.ChargeToClientLookupId;//Added in V2-672
                POLineItemList.Add(polineModel);
            }
            return POLineItemList;
        }

        private PurchaseRequest CreatePurchaseRequest(PurchaseOrder purchaseorder)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CustomSequentialId custid = new CustomSequentialId();
            custid.KeyList = AutoGenerateKey.PR_ANNUAL;
            var custList = custid.RetrieveByClientIdandSiteIdandKey_V2(userData.DatabaseKey);
            string PRPrefix = custList != null ? custList.Where(x => x.Key == AutoGenerateKey.PR_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : "";

            purchaseRequest.SiteId = purchaseorder.SiteId;
            purchaseRequest.ClientId = purchaseorder.ClientId;
            purchaseRequest.AreaId = 0;
            purchaseRequest.DepartmentId = 0;
            purchaseRequest.StoreroomId = purchaseorder.StoreroomId;
            purchaseRequest.VendorId = purchaseorder.VendorId;
            purchaseRequest.CreatedBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseRequest.Status = PurchaseRequestStatusConstants.Open;
            purchaseRequest.Reason = "";
            purchaseRequest.CreateByPKForeignKeys(this.userData.DatabaseKey, true, AutoGenerateKey.PR_ANNUAL, PRPrefix);
            return purchaseRequest;
        }
        private void CreatePRHeaderUDF(long purchaseRequestId, long poClientId, POHeaderUDF POHeaderUDFobj)
        {
            PRHeaderUDF prHeaderUDF = new PRHeaderUDF();
            prHeaderUDF.ClientId = poClientId;
            prHeaderUDF.PurchaseRequestId = purchaseRequestId;
            prHeaderUDF.Text1 = POHeaderUDFobj.Text1;
            prHeaderUDF.Text2 = POHeaderUDFobj.Text2;
            prHeaderUDF.Text3 = POHeaderUDFobj.Text3;
            prHeaderUDF.Text4 = POHeaderUDFobj.Text4;
            if (POHeaderUDFobj.Date1 != null && POHeaderUDFobj.Date1.Value == default(DateTime))
            {
                prHeaderUDF.Date1 = null;
            }
            else
            {
                prHeaderUDF.Date1 = POHeaderUDFobj.Date1;
            }

            if (POHeaderUDFobj.Date2 != null && POHeaderUDFobj.Date2.Value == default(DateTime))
            {
                prHeaderUDF.Date2 = null;
            }
            else
            {
                prHeaderUDF.Date2 = POHeaderUDFobj.Date2;
            }
            if (POHeaderUDFobj.Date3 != null && POHeaderUDFobj.Date3.Value == default(DateTime))
            {
                prHeaderUDF.Date3 = null;
            }
            else
            {
                prHeaderUDF.Date3 = POHeaderUDFobj.Date3;
            }
            if (POHeaderUDFobj.Date4 != null && POHeaderUDFobj.Date4.Value == default(DateTime))
            {
                prHeaderUDF.Date4 = null;
            }
            else
            {
                prHeaderUDF.Date4 = POHeaderUDFobj.Date4;
            }

            prHeaderUDF.Bit1 = POHeaderUDFobj.Bit1;
            prHeaderUDF.Bit2 = POHeaderUDFobj.Bit2;
            prHeaderUDF.Bit3 = POHeaderUDFobj.Bit3;
            prHeaderUDF.Bit4 = POHeaderUDFobj.Bit4;
            prHeaderUDF.Numeric1 = POHeaderUDFobj.Numeric1;
            prHeaderUDF.Numeric2 = POHeaderUDFobj.Numeric2;
            prHeaderUDF.Numeric3 = POHeaderUDFobj.Numeric3;
            prHeaderUDF.Numeric4 = POHeaderUDFobj.Numeric4;
            prHeaderUDF.Select1 = POHeaderUDFobj.Select1;
            prHeaderUDF.Select2 = POHeaderUDFobj.Select2;
            prHeaderUDF.Select3 = POHeaderUDFobj.Select3;
            prHeaderUDF.Select4 = POHeaderUDFobj.Select4;
            prHeaderUDF.Create(this.userData.DatabaseKey);
        }
        private PurchaseRequestLineItem CreatePRLineItem(PurchaseOrderLineItem item, long AccountID, long ChargeToId, string ChargeType, long purchaseRequestId)
        {
            PurchaseRequestLineItem PRLobj = new PurchaseRequestLineItem();
            PRLobj.ClientId = item.ClientId;
            PRLobj.PurchaseRequestId = purchaseRequestId;
            PRLobj.AccountId = AccountID;
            PRLobj.ChargeToID = ChargeToId;
            PRLobj.ChargeType = ChargeType;
            PRLobj.Creator_PersonnelId = item.Creator_PersonnelId;
            PRLobj.Description = item.Description;
            //PRLobj.RequiredDate = item.RequiredDate;
            PRLobj.LineNumber = item.LineNumber;
            PRLobj.PartId = item.PartId;
            PRLobj.PartStoreroomId = item.PartStoreroomId;
            PRLobj.PurchaseOrderLineItemId = 0;
            PRLobj.OrderQuantity = item.OrderQuantity;
            PRLobj.UnitofMeasure = item.UnitOfMeasure;
            PRLobj.UnitCost = item.UnitCost;
            PRLobj.AutoGenerated = false;
            PRLobj.ExtractLogId = 0;
            PRLobj.VendorCatalogItemId = 0;
            PRLobj.SupplierPartId = item.SupplierPartId;
            PRLobj.SupplierPartAuxiliaryId = item.SupplierPartAuxiliaryId;
            PRLobj.ManufacturerPartId = item.ManufacturerPartId;
            PRLobj.Manufacturer = item.Manufacturer;
            PRLobj.PurchaseQuantity = item.PurchaseQuantity;
            PRLobj.UOMConversion = item.UOMConversion;
            PRLobj.PurchaseUOM = item.PurchaseUOM;
            PRLobj.UNSPSC = item.UNSPSC;
            PRLobj.Create(this.userData.DatabaseKey);
            return PRLobj;
        }
        private void CreatePRLineUDF(long poClientId, POLineUDF POLineUDFobj, PurchaseRequestLineItem PRlineitem)
        {
            PRLineUDF pRLineUDF = new PRLineUDF();
            pRLineUDF.ClientId = poClientId;
            pRLineUDF.PurchaseRequestLineItemId = PRlineitem.PurchaseRequestLineItemId;
            pRLineUDF.Text1 = POLineUDFobj.Text1;
            pRLineUDF.Text2 = POLineUDFobj.Text2;
            pRLineUDF.Text3 = POLineUDFobj.Text3;
            pRLineUDF.Text4 = POLineUDFobj.Text4;
            if (POLineUDFobj.Date1 != null && POLineUDFobj.Date1.Value == default(DateTime))
            {
                pRLineUDF.Date1 = null;
            }
            else
            {
                pRLineUDF.Date1 = POLineUDFobj.Date1;
            }

            if (POLineUDFobj.Date2 != null && POLineUDFobj.Date2.Value == default(DateTime))
            {
                pRLineUDF.Date2 = null;
            }
            else
            {
                pRLineUDF.Date2 = POLineUDFobj.Date2;
            }
            if (POLineUDFobj.Date3 != null && POLineUDFobj.Date3.Value == default(DateTime))
            {
                pRLineUDF.Date3 = null;
            }
            else
            {
                pRLineUDF.Date3 = POLineUDFobj.Date3;
            }
            if (POLineUDFobj.Date4 != null && POLineUDFobj.Date4.Value == default(DateTime))
            {
                pRLineUDF.Date4 = null;
            }
            else
            {
                pRLineUDF.Date4 = POLineUDFobj.Date4;
            }

            pRLineUDF.Bit1 = POLineUDFobj.Bit1;
            pRLineUDF.Bit2 = POLineUDFobj.Bit2;
            pRLineUDF.Bit3 = POLineUDFobj.Bit3;
            pRLineUDF.Bit4 = POLineUDFobj.Bit4;
            pRLineUDF.Numeric1 = POLineUDFobj.Numeric1;
            pRLineUDF.Numeric2 = POLineUDFobj.Numeric2;
            pRLineUDF.Numeric3 = POLineUDFobj.Numeric3;
            pRLineUDF.Numeric4 = POLineUDFobj.Numeric4;
            pRLineUDF.Select1 = POLineUDFobj.Select1;
            pRLineUDF.Select2 = POLineUDFobj.Select2;
            pRLineUDF.Select3 = POLineUDFobj.Select3;
            pRLineUDF.Select4 = POLineUDFobj.Select4;
            pRLineUDF.Create(this.userData.DatabaseKey);
        }
        private void CreatePurchasingEventLog(long PurchaseOrderId, long purchaseRequestId, long ClientId, long SiteId)
        {

            PurchasingEventLog log = new PurchasingEventLog();
            log.ClientId = ClientId;
            log.SiteId = SiteId;
            log.ObjectId = purchaseRequestId;
            log.Event = PurchasingEvents.PROpen;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "Created from Purchase Order Model";
            log.SourceId = PurchaseOrderId;
            log.TableName = AttachmentTableConstant.PurchaseRequest;
            log.Create(userData.DatabaseKey);

        }
        #endregion

        #region V2-1032 SingleStock LineItem

        #region Add
        internal List<string> AddPartInInventorySingleStockDynamic(PurchaseOrderVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PurchaseOrderLineItem purchaseOrderLineItem = new PurchaseOrderLineItem();
            purchaseOrderLineItem.ClientId = this.userData.DatabaseKey.Client.ClientId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrderLineItemStockSingle, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.AddPOLineItemPartInInventory);
                getpropertyInfo = objVM.AddPOLineItemPartInInventory.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.AddPOLineItemPartInInventory);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = purchaseOrderLineItem.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "ChargeToId")
                {
                    continue;
                }
                if (item.ColumnName == "PartId")
                {
                    continue;
                }

                setpropertyInfo.SetValue(purchaseOrderLineItem, val);
            }
            purchaseOrderLineItem.UnitOfMeasure = objVM.AddPOLineItemPartInInventory.UnitOfMeasure ?? string.Empty;
            purchaseOrderLineItem.PurchaseRequestId = objVM.AddPOLineItemPartInInventory.PurchaseRequestId ?? 0;
            purchaseOrderLineItem.ChargeToId = objVM.AddPOLineItemPartInInventory.ChargeToId ?? 0;
            purchaseOrderLineItem.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseOrderLineItem.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseOrderLineItem.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            purchaseOrderLineItem.PurchaseOrderId = objVM.AddPOLineItemPartInInventory.PurchaseOrderId ?? 0;
            purchaseOrderLineItem.PartId = objVM.AddPOLineItemPartInInventory.PartId ?? 0;
            purchaseOrderLineItem.Status = PurchaseOrderStatusConstants.Open;
            purchaseOrderLineItem.PartStoreroomId = objVM.AddPOLineItemPartInInventory.PartStoreroomId ?? 0;//RKL MAIL -Label Printing from Receipts
            purchaseOrderLineItem.CreateWithValidation_V2(userData.DatabaseKey);
            if (purchaseOrderLineItem.ErrorMessages.Count == 0)
            {
                purchaseOrderLineItem.ReOrderLineNumber(userData.DatabaseKey);

                if (purchaseOrderLineItem.ChargeType == AttachmentTableConstant.WorkOrder && (purchaseOrderLineItem.Status == PurchaseOrderStatusConstants.Open || purchaseOrderLineItem.Status == PurchaseOrderStatusConstants.Partial))
                {
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(purchaseOrderLineItem.ChargeToId, "Add"));
                }
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPurchaseOrderLineItemUDFForSingleStockDynamic(objVM.AddPOLineItemPartInInventory, purchaseOrderLineItem.PurchaseOrderLineItemId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        purchaseOrderLineItem.ErrorMessages.AddRange(errors);
                    }
                }
            }
            return purchaseOrderLineItem.ErrorMessages;
        }
        public List<string> AddPurchaseOrderLineItemUDFForSingleStockDynamic(AddPOLineItemPartInInventoryModelDynamic poLineItemModelDynamic, long purchaseOrderLineItemId,
   List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            POLineUDF poLineUDF = new POLineUDF();
            poLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            poLineUDF.PurchaseOrderLineItemId = purchaseOrderLineItemId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, poLineItemModelDynamic);
                getpropertyInfo = poLineItemModelDynamic.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(poLineItemModelDynamic);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(poLineUDF, val);
            }
            poLineUDF.Create(_dbKey);
            return poLineUDF.ErrorMessages;
        }
        #endregion
        #region Edit
        internal EditPOLineItemPartInInventorySingleStockModelDynamic GetPOLineItemInInventorySingleStockByIdDynamic(long PurchaseOrderLineItemId, long PurchaseOrderId)
        {
            EditPOLineItemPartInInventorySingleStockModelDynamic editPOLineItemPartInInventoryModel = new EditPOLineItemPartInInventorySingleStockModelDynamic();
            PurchaseOrderLineItem poLineItemDetails = RetrievePOLineItemByPurchaseOrderLineItemId(PurchaseOrderLineItemId);
            POLineUDF pOLineUDF = RetrievePOLineItemUDFByPurchaseOrderLineItemId(PurchaseOrderLineItemId);
            editPOLineItemPartInInventoryModel = MapPOLineItemPartInInventorySingleStockDataForEdit(editPOLineItemPartInInventoryModel, poLineItemDetails, PurchaseOrderId);
            editPOLineItemPartInInventoryModel = MapPOLineUDFPartInInventorySingleStockDataForEdit(editPOLineItemPartInInventoryModel, pOLineUDF);
            return editPOLineItemPartInInventoryModel;
        }
        public EditPOLineItemPartInInventorySingleStockModelDynamic MapPOLineItemPartInInventorySingleStockDataForEdit(EditPOLineItemPartInInventorySingleStockModelDynamic objLineItem, PurchaseOrderLineItem purchaseOrderlineitem, long PurchaseOrderId)
        {
            objLineItem.PurchaseOrderLineItemId = purchaseOrderlineitem.PurchaseOrderLineItemId;
            objLineItem.PurchaseOrderId = PurchaseOrderId;
            if (purchaseOrderlineitem.PartId == 0)
            {
                objLineItem.PartId = null;
                objLineItem.PartClientLookupId = "";
            }
            else
            {
                objLineItem.PartId = purchaseOrderlineitem.PartId;
                objLineItem.PartClientLookupId = purchaseOrderlineitem.PartClientLookupId;
            }
            objLineItem.LineNumber = purchaseOrderlineitem.LineNumber;
            objLineItem.Description = purchaseOrderlineitem.Description;
            objLineItem.OrderQuantity = purchaseOrderlineitem.OrderQuantity;
            objLineItem.UnitOfMeasure = purchaseOrderlineitem.UnitOfMeasure;
            objLineItem.UnitCost = purchaseOrderlineitem.UnitCost;
            objLineItem.TotalCost = purchaseOrderlineitem.TotalCost;
            if (purchaseOrderlineitem.AccountId == 0)
            {
                objLineItem.AccountId = null;
                objLineItem.AccountClientLookupId = "";
            }
            else
            {
                objLineItem.AccountId = purchaseOrderlineitem.AccountId;
                objLineItem.AccountClientLookupId = purchaseOrderlineitem.AccountClientLookupId;
            }
            objLineItem.EstimatedDelivery = purchaseOrderlineitem.EstimatedDelivery;
            objLineItem.Taxable = purchaseOrderlineitem.Taxable;
            objLineItem.Status = purchaseOrderlineitem.Status;
            objLineItem.PurchaseUOM = purchaseOrderlineitem.PurchaseUOM;
            objLineItem.Manufacturer = purchaseOrderlineitem.Manufacturer;
            objLineItem.SupplierPartId = purchaseOrderlineitem.SupplierPartId;
            objLineItem.SupplierPartAuxiliaryId = purchaseOrderlineitem.SupplierPartAuxiliaryId;
            objLineItem.ManufacturerPartId = purchaseOrderlineitem.ManufacturerPartId;
            objLineItem.VendorCatalogItemId = purchaseOrderlineitem.VendorCatalogItemId;
            objLineItem.UOMConversion = purchaseOrderlineitem.UOMConversion;
            if (purchaseOrderlineitem.EstimatedDelivery != null && purchaseOrderlineitem.EstimatedDelivery.Value == default(DateTime))
            {
                objLineItem.EstimatedDelivery = null;
            }

            if (purchaseOrderlineitem.UNSPSC == 0)
            {
                objLineItem.UNSPSC = null;
                objLineItem.PartCategoryMasterClientLookupId = "";
            }
            else
            {
                objLineItem.UNSPSC = purchaseOrderlineitem.UNSPSC;
                objLineItem.PartCategoryMasterClientLookupId = purchaseOrderlineitem.PartCategoryMasterClientLookupId;
            }

            return objLineItem;
        }
        private EditPOLineItemPartInInventorySingleStockModelDynamic MapPOLineUDFPartInInventorySingleStockDataForEdit(EditPOLineItemPartInInventorySingleStockModelDynamic ediPOLineItemModel, POLineUDF pOLineUDF)
        {
            if (pOLineUDF != null)
            {
                ediPOLineItemModel.POLineUDFId = pOLineUDF.POLineUDFId;

                ediPOLineItemModel.Text1 = pOLineUDF.Text1;
                ediPOLineItemModel.Text2 = pOLineUDF.Text2;
                ediPOLineItemModel.Text3 = pOLineUDF.Text3;
                ediPOLineItemModel.Text4 = pOLineUDF.Text4;

                if (pOLineUDF.Date1 != null && pOLineUDF.Date1 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date1 = null;
                }
                else
                {
                    ediPOLineItemModel.Date1 = pOLineUDF.Date1;
                }
                if (pOLineUDF.Date2 != null && pOLineUDF.Date2 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date2 = null;
                }
                else
                {
                    ediPOLineItemModel.Date2 = pOLineUDF.Date2;
                }
                if (pOLineUDF.Date3 != null && pOLineUDF.Date3 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date3 = null;
                }
                else
                {
                    ediPOLineItemModel.Date3 = pOLineUDF.Date3;
                }
                if (pOLineUDF.Date4 != null && pOLineUDF.Date4 == DateTime.MinValue)
                {
                    ediPOLineItemModel.Date4 = null;
                }
                else
                {
                    ediPOLineItemModel.Date4 = pOLineUDF.Date4;
                }

                ediPOLineItemModel.Bit1 = pOLineUDF.Bit1;
                ediPOLineItemModel.Bit2 = pOLineUDF.Bit2;
                ediPOLineItemModel.Bit3 = pOLineUDF.Bit3;
                ediPOLineItemModel.Bit4 = pOLineUDF.Bit4;

                ediPOLineItemModel.Numeric1 = pOLineUDF.Numeric1;
                ediPOLineItemModel.Numeric2 = pOLineUDF.Numeric2;
                ediPOLineItemModel.Numeric3 = pOLineUDF.Numeric3;
                ediPOLineItemModel.Numeric4 = pOLineUDF.Numeric4;

                ediPOLineItemModel.Select1 = pOLineUDF.Select1;
                ediPOLineItemModel.Select2 = pOLineUDF.Select2;
                ediPOLineItemModel.Select3 = pOLineUDF.Select3;
                ediPOLineItemModel.Select4 = pOLineUDF.Select4;
            }
            return ediPOLineItemModel;
        }
        internal PurchaseOrderLineItem UpdatePOPartInInventorySingleStockDynamic(PurchaseOrderVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            PurchaseOrderLineItem poLineItem = new PurchaseOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseOrderLineItemId = objVM.EditPOLineItemPartInInventorySingleStock.PurchaseOrderLineItemId ?? 0
            };
            poLineItem.Retrieve(_dbKey);
            if (poLineItem.PartId != objVM.EditPOLineItemPartInInventorySingleStock.PartId)
            {
                poLineItem.PartStoreroomId = objVM.EditPOLineItemPartInInventorySingleStock.PartStoreroomId ?? 0;
            }
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrderLineItemStockSingle, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditPOLineItemPartInInventorySingleStock);
                getpropertyInfo = objVM.EditPOLineItemPartInInventorySingleStock.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPOLineItemPartInInventorySingleStock);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poLineItem.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "EstimatedDelivery")
                {
                    continue;
                }

                setpropertyInfo.SetValue(poLineItem, val);
            }
            string status = poLineItem.Status ?? string.Empty;
            string OldChargeType = poLineItem.ChargeType ?? string.Empty;
            long OldChargeToId = poLineItem.ChargeToId;
            string NewChargeType = objVM.EditPOLineItemPartInInventorySingleStock.ChargeType ?? string.Empty;
            long NewChargeToId = objVM.EditPOLineItemPartInInventorySingleStock.ChargeToId ?? 0;
            if (objVM.EditPOLineItemPartInInventorySingleStock.EstimatedDelivery != null && objVM.EditPOLineItemPartInInventorySingleStock.EstimatedDelivery.Value == default(DateTime))
            {
                poLineItem.EstimatedDelivery = null;
            }
            else
            {
                poLineItem.EstimatedDelivery = objVM.EditPOLineItemPartInInventorySingleStock.EstimatedDelivery;
            }
            
            poLineItem.UpdateByPKForeignKeys(this.userData.DatabaseKey);

            if (status == PurchaseOrderStatusConstants.Open || status == PurchaseOrderStatusConstants.Partial)
            {
                CommonWrapper commonWrapper = new CommonWrapper(userData);
                if (OldChargeType == AttachmentTableConstant.WorkOrder)
                {
                    if (NewChargeType != AttachmentTableConstant.WorkOrder)
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(OldChargeToId, "Minus"));
                    }
                    else
                    {
                        if (OldChargeToId != NewChargeToId)
                        {
                            Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(OldChargeToId, "Minus"));
                            Task task2 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(NewChargeToId, "Add"));
                            Task.WaitAll(task1, task2);
                        }
                    }
                }
                else
                {

                    if (NewChargeType == AttachmentTableConstant.WorkOrder)
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(NewChargeToId, "Add"));
                    }
                }
            }
            List<string> errors = new List<string>();
            if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                errors = UpdatePOLineUDFPartInInventorySingleStockDynamic(objVM.EditPOLineItemPartInInventorySingleStock, configDetails);

            }
            if (errors != null && errors.Count() > 0)
            {
                poLineItem.ErrorMessages.AddRange(errors);
            }
            return poLineItem;
        }
        public List<string> UpdatePOLineUDFPartInInventorySingleStockDynamic(EditPOLineItemPartInInventorySingleStockModelDynamic poLineItem,
 List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            POLineUDF poLineUDF = new POLineUDF();
            poLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            poLineUDF.PurchaseOrderLineItemId = poLineItem.PurchaseOrderLineItemId ?? 0;

            poLineUDF = poLineUDF.RetrieveByPurchaseOrderLineItemId(userData.DatabaseKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, poLineItem);
                getpropertyInfo = poLineItem.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(poLineItem);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = poLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(poLineUDF, val);
            }
            if (poLineUDF.PurchaseOrderLineItemId > 0)
            {
                poLineUDF.Update(_dbKey);
            }
            else
            {
                poLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                poLineUDF.PurchaseOrderLineItemId = poLineItem.PurchaseOrderLineItemId ?? 0;
                poLineUDF.Create(_dbKey);
            }

            return poLineUDF.ErrorMessages;
        }
        #endregion
        #endregion

        #region V2-1079
        public PurchaseOrder UpdatePurchaseOrderForEDIExport(long PurchaseOrderId)
        {
            PurchaseOrder purchaseorder = new DataContracts.PurchaseOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseOrderId = PurchaseOrderId
            };
            purchaseorder.Retrieve(userData.DatabaseKey);
            purchaseorder.SentOrderRequest = true;
            purchaseorder.Update(userData.DatabaseKey);
            CreateEventLog(PurchaseOrderId, PurchasingEvents.EDIPOSent, "EDI Purchase Order Sent");
            return purchaseorder;
        }
        #endregion

        #region V2-1112 Add CustomEPMPO
        internal Tuple<PurchaseOrder, bool, bool> AddCustomEPMPurchaseOrderDynamic(PurchaseOrderVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PurchaseOrder purchaseorder = new PurchaseOrder();
            WorkFlowLog workflowlog = new WorkFlowLog();
            CustomSequentialId custid = new CustomSequentialId();
            custid.KeyList = AutoGenerateKey.EPM_PO_ANNUAL;
            string POPrefix = objVM.addCustomPurchaseOrder.Initials;
            string POSuffix = objVM.addCustomPurchaseOrder.ShiptoSuffix;

            purchaseorder.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            purchaseorder.ClientId = this.userData.DatabaseKey.Client.ClientId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrder, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.addPurchaseOrder);
                getpropertyInfo = objVM.addPurchaseOrder.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.addPurchaseOrder);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = purchaseorder.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(purchaseorder, val);
            }
            // Only need to check if Site.VendorCompliance = True
            bool VendorInsuranceChecking = false;
            bool VendorAssetMgtChecking = false;
            Vendor vendor = new Vendor
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = purchaseorder.VendorId
            };
            vendor.Retrieve(_dbKey);
            if (vendor.InsuranceOverride == true)
            {
                VendorInsuranceChecking = false;
            }
            else
            {
                if (userData.Site.VendorCompliance == true && vendor.InsuranceRequired == true && (Convert.ToDateTime(vendor.InsuranceExpireDate).Date < DateTime.UtcNow.Date || vendor.InsuranceExpireDate == DateTime.MinValue))
                {
                    VendorInsuranceChecking = true;
                }
            }
            if (VendorInsuranceChecking == true)
            {
                return new Tuple<PurchaseOrder, bool, bool>(purchaseorder, VendorInsuranceChecking, VendorAssetMgtChecking);
            }

            if (vendor.AssetMgtOverride == true)
            {
                VendorAssetMgtChecking = false;
            }
            else
            {
                if (userData.Site.VendorCompliance == true && vendor.AssetMgtRequired == true && (Convert.ToDateTime(vendor.AssetMgtExpireDate).Date < DateTime.UtcNow.Date || vendor.AssetMgtExpireDate == DateTime.MinValue))
                {
                    VendorAssetMgtChecking = true;
                }
            }
            if (VendorAssetMgtChecking == true)
            {
                return new Tuple<PurchaseOrder, bool, bool>(purchaseorder, VendorInsuranceChecking, VendorAssetMgtChecking);
            }
            purchaseorder.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseorder.VendorClientLookupId = objVM.addPurchaseOrder.VendorClientLookupId;
            purchaseorder.Status = PurchaseOrderStatusConstants.Open;
            purchaseorder.FOB = objVM.addPurchaseOrder.FOB ?? "";
            purchaseorder.Terms = objVM.addPurchaseOrder.Terms ?? "";
            workflowlog.ClientId = this.userData.DatabaseKey.Personnel.ClientId;
            workflowlog.ObjectName = AttachmentTableConstant.PurchaseOrder;
            workflowlog.Message = GlobalConstants.PurchaseOrderAdd;
            workflowlog.UserName = this.userData.DatabaseKey.UserName;
            purchaseorder.workflowlog = workflowlog;
            purchaseorder.CreateByPKForeignKeys_V2(this.userData.DatabaseKey, true, AutoGenerateKey.EPM_PO_ANNUAL, POPrefix,POSuffix);
            if (purchaseorder.ErrorMessages != null && purchaseorder.ErrorMessages.Count == 0)
            {
                CreateEventLog(purchaseorder.PurchaseOrderId, PurchasingEvents.POOpen);
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPurchaseOrderUDFDynamic(objVM.addPurchaseOrder, purchaseorder.PurchaseOrderId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        purchaseorder.ErrorMessages.AddRange(errors);
                    }
                }
            }
            return new Tuple<PurchaseOrder, bool, bool>(purchaseorder, VendorInsuranceChecking, VendorAssetMgtChecking);
        }
        #endregion
        #region V2-1112 RetrieveByEPM_POPrintV2
        public PurchaseOrder RetrieveByEPMPOPrintV2(List<long> PurchaseOrderIDList = null)
        {
            PurchaseOrder po = new PurchaseOrder();
            po.ClientId = userData.DatabaseKey.Client.ClientId;
            po.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            po.PurchaseOrderIDList = PurchaseOrderIDList != null && PurchaseOrderIDList.Count > 0 ? string.Join(",", PurchaseOrderIDList) : string.Empty;
            po.RetrieveByEPMPurchaseOrdePrintV2(userData.DatabaseKey, userData.Site.TimeZone);
            return po;
        }
        #endregion
        #region V2-1147
        public long GetPurchaseOrderIdFromPurchaseOrderLineItemId(long PurchaseOrderLineItemId)
        {
            PurchaseOrderLineItem purchaselineitem = new PurchaseOrderLineItem()
            {
                ClientId = this.userData.DatabaseKey.User.ClientId,
                PurchaseOrderLineItemId = PurchaseOrderLineItemId

            };
            purchaselineitem.Retrieve(userData.DatabaseKey);
            return purchaselineitem.PurchaseOrderId;
        }
        #endregion
    }

}