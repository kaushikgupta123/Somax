using Client.Models;
using Client.Models.Invoice;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Client.Common;
using System.Globalization;
using INTDataLayer.EL;
using System.Windows.Interop;
using Database.Business;


namespace Client.BusinessWrapper.Invoice
{
    public class InvoiceWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public string newClientlookupId { get; set; }
        public InvoiceWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        internal List<KeyValuePair<string, string>> populateListDetails()
        {
            List<KeyValuePair<string, string>> customList = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> customList1 = new List<KeyValuePair<string, string>>();
            customList = CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, "InvoiceMatchHeader", userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            if (customList.Count > 0)
            {
                customList.Insert(0, new KeyValuePair<string, string>("0", "-- Select All --"));
            }
            return customList;
        }
        public List<InvoiceMatchHeaderModel> RetrieveAllForSearchNew(int displayId)
        {
            InvoiceMatchHeader inv = new InvoiceMatchHeader()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                CustomQueryDisplayId = displayId
            };
            InvoiceMatchHeaderModel InvoiceModel;
            List<InvoiceMatchHeaderModel> InvoiceModelList = new List<InvoiceMatchHeaderModel>();
            List<InvoiceMatchHeader> InvoiceList = inv.RetrieveAllForSearchNew(userData.DatabaseKey);
            if (InvoiceList != null && InvoiceList.Count > 0)
            {
                foreach (var p in InvoiceList)
                {
                    InvoiceModel = new InvoiceMatchHeaderModel();
                    #region Model Bind
                    InvoiceModel.InvoiceMatchHeaderId = p.InvoiceMatchHeaderId;
                    InvoiceModel.ClientLookupId = p.ClientLookupId;
                    InvoiceModel.InvoiceMatchHeaderId = p.InvoiceMatchHeaderId;
                    InvoiceModel.Status = p.Status;
                    InvoiceModel.Type = p.Type;
                    InvoiceModel.VendorClientLookupId = p.VendorClientLookupId;
                    InvoiceModel.VendorName = p.VendorName;
                    if (p.ReceiptDate != null && p.ReceiptDate == default(DateTime))
                    {
                        InvoiceModel.ReceiptDate = null;
                    }
                    else
                    {
                        InvoiceModel.ReceiptDate = p.ReceiptDate;
                    }
                    InvoiceModel.POClientLookUpId = p.POClientLookupId;
                    InvoiceModel.InvoiceDate = p.InvoiceDate;
                    if (p.InvoiceDate != null && p.InvoiceDate == default(DateTime))
                    {
                        InvoiceModel.InvoiceDate = null;
                    }
                    else
                    {
                        InvoiceModel.InvoiceDate = p.InvoiceDate;
                    }
                    #endregion
                    InvoiceModelList.Add(InvoiceModel);
                }
            }
            return InvoiceModelList;
        }
        //V2-373
        public List<InvoiceMatchHeaderModel> RetrieveINVGridChunkSearch(int CustomQueryDisplayId, DateTime? CompleteATPStartDateVw = null,
          DateTime? CompleteATPEndDateVw = null, DateTime? CompletePStartDateVw = null, DateTime? CompletePEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, int skip = 0, int length = 0, string orderbycol = "",
          string orderDir = "", string invoice = "", string status = "", string vendor = "", string vendorname = "",
          DateTime? receiptdate = null, string purchaseorder = "", DateTime? invoicedate = null, string txtSearchval = "")
        {
            InvoiceMatchHeader inv = new InvoiceMatchHeader()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                CustomQueryDisplayId = CustomQueryDisplayId,
                CompleteATPStartDateVw = CompleteATPStartDateVw.HasValue ? CompleteATPStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty,//V2-373
                CompleteATPEndDateVw = CompleteATPEndDateVw.HasValue ? CompleteATPEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty,//V2-373
                CompletePStartDateVw = CompletePStartDateVw.HasValue ? CompletePStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty,//V2-373
                CompletePEndDateVw = CompletePEndDateVw.HasValue ? CompletePEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty,//V2-373
                CreateStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty,//V2-1061
                CreateEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty,//V2-1061
                orderBy = orderDir,
                orderbyColumn = orderbycol,
                offset1 = Convert.ToString(skip),
                nextrow = Convert.ToString(length),
                ClientLookupId = invoice,
                Status = status,
                VendorClientLookupId = vendor,
                VendorName = vendorname,
                ReceiptDate = receiptdate,
                POClientLookupId = purchaseorder,
                InvoiceDate = invoicedate,
                SearchText = txtSearchval
            };
            InvoiceMatchHeaderModel InvoiceModel;
            List<string> StatusList = new List<string>();
            List<InvoiceMatchHeaderModel> InvoiceModelList = new List<InvoiceMatchHeaderModel>();
            List<InvoiceMatchHeader> InvoiceList = inv.RetrieveChunkSearch(userData.DatabaseKey);
            if (InvoiceList != null && InvoiceList.Count > 0)
            {
                foreach (var p in InvoiceList)
                {
                    InvoiceModel = new InvoiceMatchHeaderModel();
                    #region Model Bind
                    InvoiceModel.InvoiceMatchHeaderId = p.InvoiceMatchHeaderId;
                    InvoiceModel.ClientLookupId = p.ClientLookupId;
                    InvoiceModel.InvoiceMatchHeaderId = p.InvoiceMatchHeaderId;
                    InvoiceModel.Status = p.Status;
                    InvoiceModel.VendorClientLookupId = p.VendorClientLookupId;
                    InvoiceModel.VendorName = p.VendorName;
                    if (p.ReceiptDate != null && p.ReceiptDate == default(DateTime))
                    {
                        InvoiceModel.ReceiptDate = null;
                    }
                    else
                    {
                        InvoiceModel.ReceiptDate = p.ReceiptDate;
                    }
                    InvoiceModel.POClientLookUpId = p.POClientLookupId;
                    InvoiceModel.InvoiceDate = p.InvoiceDate;
                    if (p.InvoiceDate != null && p.InvoiceDate == default(DateTime))
                    {
                        InvoiceModel.InvoiceDate = null;
                    }
                    else
                    {
                        InvoiceModel.InvoiceDate = p.InvoiceDate;
                    }
                    InvoiceModel.TotalCount = p.TotalCount;
                    InvoiceModel.ChildCount = p.ChildCount;
                    #endregion
                    InvoiceModelList.Add(InvoiceModel);
                }
            }
            return InvoiceModelList;
        }
        public InvoiceMatchHeaderModel initializeControls(InvoiceMatchHeader obj)
        {

            string OpenLocValue = UtilityFunction.GetMessageFromResource("spnInvStatusOpen", LocalizeResourceSetConstants.InvoiceDetails);
            string AuthorizedToPayLocValue = UtilityFunction.GetMessageFromResource("spnInvStatusAuthorizedToPay", LocalizeResourceSetConstants.InvoiceDetails);
            string PaidLocValue = UtilityFunction.GetMessageFromResource("spnInvStatusPaid", LocalizeResourceSetConstants.InvoiceDetails);
            InvoiceMatchHeaderModel objInvoice = new InvoiceMatchHeaderModel();
            objInvoice.Status = obj.Status;
            objInvoice.VendorClientLookupId = obj.VendorClientLookupId;
            objInvoice.VendorName = obj.VendorName;
            objInvoice.POClientLookUpId = obj.POClientLookupId;
            objInvoice.PurchaseOrderId = obj?.PurchaseOrderId ?? 0;
            objInvoice.DateRange = obj.DateRange;
            objInvoice.Assigned = obj.Assigned;
            if (obj.Status == InvoiceMatchStatus.Open)
            {
                objInvoice.Status_Display = OpenLocValue;
            }
            else if (obj.Status == InvoiceMatchStatus.Paid)
            {
                objInvoice.Status_Display = PaidLocValue;
                objInvoice.PaidBy = obj.PaidBy;
                objInvoice.AuthorizedToPayBy = obj?.AuthorizedToPayBy ?? string.Empty;
            }
            else if (obj.Status == InvoiceMatchStatus.AuthorizedToPay)
            {
                objInvoice.Status_Display = AuthorizedToPayLocValue;
                objInvoice.AuthorizedToPayBy = obj.AuthorizedToPayBy;
            }
            else
            {
                objInvoice.Status_Display = string.Empty;
            }
            objInvoice.PersonnelId = obj.PersonnelId;
            objInvoice.Variance = obj.Variance;
            objInvoice.CustomQueryDisplayId = obj.CustomQueryDisplayId;
            objInvoice.NumberOfLineItems = obj.NumberOfLineItems;
            objInvoice.Flag = obj.Flag;
            objInvoice.ClientLookupId = obj.ClientLookupId;
            objInvoice.InvoiceMatchHeaderId = obj.InvoiceMatchHeaderId;
            if (obj.InvoiceDate != null && obj.InvoiceDate == default(DateTime))
            {
                objInvoice.InvoiceDate = null;
            }
            else
            {
                objInvoice.InvoiceDate = obj.InvoiceDate;
            }
            objInvoice.ItemTotal = obj.ItemTotal;
            objInvoice.ShipAmount = obj.ShipAmount;
            objInvoice.Total = obj.Total;
            objInvoice.ClientId = obj.ClientId;
            objInvoice.SiteId = obj.SiteId;
            objInvoice.AreaId = obj.AreaId;
            objInvoice.StoreroomId = obj.StoreroomId;
            objInvoice.AuthorizedToPay = obj.AuthorizedToPay;
            objInvoice.TaxAmount = obj.TaxAmount;
            objInvoice.AuthorizedToPayDate = obj?.AuthorizedToPayDate;
            objInvoice.AuthorizedToPay_PersonnelId = obj.AuthorizedToPay_PersonnelId;
            objInvoice.Creator_PersonnelId = this.userData.DatabaseKey.Client.CreatedClientId;
            objInvoice.AuthorizedToPay = obj.AuthorizedToPay;
            if (obj.DueDate != null && obj.DueDate == default(DateTime))
            {
                objInvoice.DueDate = null;
            }
            else
            {
                objInvoice.DueDate = obj.DueDate;
            }
            objInvoice.OverrideCode = obj.OverrideCode;
            objInvoice.OverrideComments = obj.OverrideComments;
            objInvoice.Responsible_PersonnelId = obj.Responsible_PersonnelId;
            objInvoice.OverrideCode = obj.OverrideCode;
            objInvoice.TotalInput = obj.TotalInput;
            objInvoice.Type = obj.Type;
            objInvoice.VendorId = obj?.VendorId ?? 0;
            objInvoice.PaidDate = obj?.PaidDate;
            objInvoice.Paid = obj.Paid;
            objInvoice.Paid_PersonnelId = obj.Paid_PersonnelId;
            objInvoice.UpdateIndex = obj.UpdateIndex;
            if (obj.Status == InvoiceMatchStatus.Open)
            {
                objInvoice.OpenStatusSecurity = true;
            }
            if (obj.Status == InvoiceMatchStatus.AuthorizedToPay)
            {
                objInvoice.AuthorisedToPayStatusSecurity = true;
            }
            objInvoice.SecurityInvoiceEdit = userData.Security.InvoiceMatching.Edit;
            objInvoice.SecurityInvoicePaid = userData.Security.InvoiceMatching.InvoicePaid;

            if (obj.CreateDate != null && obj.CreateDate == default(DateTime))
            {
                objInvoice.CreateDate = null;
            }
            else
            {
                objInvoice.CreateDate = obj.CreateDate;
            }
            objInvoice.CreateBy = obj.CreateBy;

            if (obj.ModifyDate != null && obj.ModifyDate == default(DateTime))
            {
                objInvoice.ModifyDate = null;
            }
            else
            {
                objInvoice.ModifyDate = obj.ModifyDate;
            }
            objInvoice.ModifyBy = obj.ModifyBy;
            objInvoice.Responsible = obj.Responsible;
            objInvoice.ResponsibleWithClientLookupId = obj.ResponsibleWithClientLookupId;

            if (obj.ReceiptDate != null && obj.ReceiptDate == default(DateTime))
            {
                objInvoice.ReceiptDate = null;
            }
            else
            {
                objInvoice.ReceiptDate = obj.ReceiptDate;
            }
            return objInvoice;
        }
        public InvoiceMatchHeaderModel getInvoiceDetailsById(long InvoiceMatchHeaderId)
        {
            InvoiceMatchHeaderModel InvoiceModel = new InvoiceMatchHeaderModel();
            InvoiceMatchHeader Inv = new InvoiceMatchHeader()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                InvoiceMatchHeaderId = InvoiceMatchHeaderId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            Inv.RetrieveByPKForeignKeys(this.userData.DatabaseKey);
            InvoiceModel = initializeControls(Inv);
            return InvoiceModel;
        }
        public List<InvoiceMatchItemModel> RetrieveMatchItemList(int ObjectId)
        {
            InvoiceMatchItem inv = new InvoiceMatchItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                InvoiceMatchHeaderId = ObjectId
            };
            InvoiceMatchItemModel InvoiceModel;
            List<InvoiceMatchItemModel> InvoiceModelList = new List<InvoiceMatchItemModel>();
            List<InvoiceMatchItem> MatchItemList = inv.RetrieveByPKForeignKeys(userData.DatabaseKey);
            foreach (var p in MatchItemList)
            {
                InvoiceModel = new InvoiceMatchItemModel();
                #region Model_Bind
                InvoiceModel.LineNumber = p.LineNumber;
                InvoiceModel.Description = p.Description;
                InvoiceModel.Quantity = p.Quantity;
                InvoiceModel.UnitOfMeasure = p.UnitOfMeasure;
                InvoiceModel.UnitCost = p.UnitCost;
                InvoiceModel.TotalCost = p.TotalCost;
                InvoiceModel.PurchaseOrder = p.PurchaseOrder;
                InvoiceModel.Account = p.Account;
                InvoiceModel.ClientId = p.ClientId;
                InvoiceModel.Creator_PersonnelId = p.Creator_PersonnelId;
                InvoiceModel.InvoiceMatchHeaderId = p.InvoiceMatchHeaderId;
                InvoiceModel.InvoiceMatchItemId = p.InvoiceMatchItemId;
                InvoiceModel.AccountId = p.AccountId;
                InvoiceModel.POReceiptItemID = p.POReceiptItemID;
                InvoiceModel.IsValid = p.IsValid;
                InvoiceModel.Status_Display = p.Status_Display;
                InvoiceModel.StoreroomId = p.StoreroomId;
                #endregion
                InvoiceModelList.Add(InvoiceModel);
            }
            return InvoiceModelList;
        }
        public InvoiceMatchItemModel RetrieveMatchItemDetails(long invoiceMatchItemId)
        {
            InvoiceMatchItemModel ojDetails = new InvoiceMatchItemModel();
            InvoiceMatchItem p = new InvoiceMatchItem
            {
                InvoiceMatchItemId = invoiceMatchItemId,
                ClientId = _dbKey.Client.ClientId
            };
            p.RetrieveByPrimaryKey(this.userData.DatabaseKey);
            ojDetails.LineNumber = p.LineNumber;
            ojDetails.Description = p.Description;
            ojDetails.Quantity = p.Quantity;
            ojDetails.UnitOfMeasure = p.UnitOfMeasure;
            ojDetails.UnitCost = p.UnitCost;
            ojDetails.TotalCost = p.TotalCost;
            ojDetails.PurchaseOrder = p.PurchaseOrder;
            ojDetails.Account = p.Account;
            ojDetails.ClientId = p.ClientId;
            ojDetails.Creator_PersonnelId = p.Creator_PersonnelId;
            ojDetails.InvoiceMatchHeaderId = p.InvoiceMatchHeaderId;
            ojDetails.InvoiceMatchItemId = p.InvoiceMatchItemId;
            ojDetails.AccountId = p.AccountId;
            ojDetails.POReceiptItemID = p.POReceiptItemID;
            ojDetails.IsValid = p.IsValid;
            ojDetails.Status_Display = p.Status_Display;
            ojDetails.StoreroomId = p.StoreroomId;
            return ojDetails;
        }
        public List<DataModel> GetAccountList()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            Account account = new Account
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };
            List<Account> acc = account.RetrieveAllTemplatesWithClient(this.userData.DatabaseKey);
            foreach (var ac in acc)
            {
                dModel = new DataModel();
                dModel.AccountId = ac.AccountId;
                dModel.Account = ac.ClientLookupId;
                dModel.Name = ac.Name;
                model.data.Add(dModel);
            }
            return model.data;

        }
        public InvoiceMatchHeader EditInvoiceHeader(InvoiceMatchHeaderModel InvoiceMatchHeaderModel)
        {
            InvoiceMatchHeader objInvoice = new InvoiceMatchHeader()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                InvoiceMatchHeaderId = InvoiceMatchHeaderModel.InvoiceMatchHeaderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            objInvoice.RetrieveByPKForeignKeys(userData.DatabaseKey);
            objInvoice.CallerUserName = this.userData.DatabaseKey.Client.CallerUserName;
            objInvoice.CallerUserInfoId = this.userData.DatabaseKey.Client.CallerUserInfoId;

            // Update properties
            objInvoice.DueDate = InvoiceMatchHeaderModel.DueDate;
            objInvoice.PurchaseOrderId = InvoiceMatchHeaderModel?.PurchaseOrderId ?? 0;
            objInvoice.ReceiptDate = InvoiceMatchHeaderModel.ReceiptDate;
            objInvoice.Responsible_PersonnelId = InvoiceMatchHeaderModel?.Responsible_PersonnelId ?? 0;
            objInvoice.ShipAmount = InvoiceMatchHeaderModel?.ShipAmount ?? 0;
            objInvoice.TaxAmount = InvoiceMatchHeaderModel?.TaxAmount ?? 0;
            objInvoice.TotalInput = InvoiceMatchHeaderModel?.TotalInput ?? 0;
            objInvoice.VendorId = InvoiceMatchHeaderModel?.VendorId ?? 0;

            objInvoice.Update(userData.DatabaseKey);
            return objInvoice;
        }

        public List<string> UpdateClientLookupId(InvoiceMatchHeaderModel InvoiceMatchHeaderModel)
        {
            InvoiceMatchHeader objInvoice = new InvoiceMatchHeader()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                InvoiceMatchHeaderId = InvoiceMatchHeaderModel.InvoiceMatchHeaderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            objInvoice.ClientLookupId = InvoiceMatchHeaderModel.ClientLookupId.Trim();
            objInvoice.Flag = InvoiceMatchHeaderModel.Flag;
            objInvoice.CallerUserName = this.userData.DatabaseKey.Client.CallerUserName;
            objInvoice.CallerUserInfoId = this.userData.DatabaseKey.Client.CallerUserInfoId;
            objInvoice.Creator_PersonnelId = this.userData.DatabaseKey.Client.CreatedClientId;
            objInvoice.UpdateIndex = InvoiceMatchHeaderModel.UpdateIndex;
            objInvoice.ChangeClientLookupId(userData.DatabaseKey);
            return objInvoice.ErrorMessages;
        }
        public InvoiceMatchHeader AddInvoiceMatchHeader(InvoiceMatchHeaderModel InvoiceMatchHeaderModel)
        {
            InvoiceMatchHeader objInvoice = new InvoiceMatchHeader()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                InvoiceMatchHeaderId = InvoiceMatchHeaderModel.InvoiceMatchHeaderId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            objInvoice.AreaId = InvoiceMatchHeaderModel.AreaId;
            objInvoice.DepartmentId = InvoiceMatchHeaderModel?.DepartmentId ?? 0;
            objInvoice.StoreroomId = InvoiceMatchHeaderModel.StoreroomId;
            objInvoice.ClientLookupId = InvoiceMatchHeaderModel.ClientLookupId.Trim();
            objInvoice.AuthorizedToPay = InvoiceMatchHeaderModel.AuthorizedToPay;
            objInvoice.AuthorizedToPayDate = InvoiceMatchHeaderModel?.AuthorizedToPayDate;
            objInvoice.AuthorizedToPay_PersonnelId = InvoiceMatchHeaderModel.AuthorizedToPay_PersonnelId;
            objInvoice.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            objInvoice.DueDate = InvoiceMatchHeaderModel.DueDate;
            objInvoice.OverrideCode = InvoiceMatchHeaderModel?.OverrideCode ?? string.Empty;
            objInvoice.OverrideComments = InvoiceMatchHeaderModel?.OverrideComments ?? string.Empty;
            objInvoice.PurchaseOrderId = InvoiceMatchHeaderModel?.PurchaseOrderId ?? 0;
            objInvoice.ReceiptDate = InvoiceMatchHeaderModel.ReceiptDate;
            objInvoice.Responsible_PersonnelId = InvoiceMatchHeaderModel?.Responsible_PersonnelId ?? 0;
            objInvoice.ShipAmount = InvoiceMatchHeaderModel?.ShipAmount ?? 0;
            objInvoice.Status = InvoiceMatchStatus.Open;
            objInvoice.TaxAmount = InvoiceMatchHeaderModel?.TaxAmount ?? 0;
            // objInvoice.Type = "PURCH";
            objInvoice.Type = InvoiceMatchHeaderModel?.Type ?? string.Empty;
            objInvoice.InvoiceDate = InvoiceMatchHeaderModel?.InvoiceDate;
            objInvoice.VendorId = InvoiceMatchHeaderModel?.VendorId ?? 0;
            objInvoice.Paid = InvoiceMatchHeaderModel.Paid;
            objInvoice.PaidDate = InvoiceMatchHeaderModel.PaidDate;
            objInvoice.Paid_PersonnelId = InvoiceMatchHeaderModel?.Paid_PersonnelId ?? 0;
            objInvoice.TotalInput = InvoiceMatchHeaderModel?.TotalInput ?? 0;
            objInvoice.CreateTo(userData.DatabaseKey);
            return objInvoice;
        }


        public InvoiceMatchItem LineItemRowUpdating(InvoiceMatchItemModel Invo)
        {
            InvoiceMatchItem invoiceMatchItem = new InvoiceMatchItem
            {
                InvoiceMatchItemId = Invo.InvoiceMatchItemId,
                ClientId = this.userData.DatabaseKey.Client.ClientId,
            };
            invoiceMatchItem.InvoiceMatchHeaderId = Invo.InvoiceMatchHeaderId;
            invoiceMatchItem.RetrieveByPrimaryKey(this.userData.DatabaseKey);
            invoiceMatchItem.Description = Invo?.Description ?? string.Empty;
            invoiceMatchItem.Quantity = (Invo?.Quantity ?? 0);
            invoiceMatchItem.UnitOfMeasure = Invo?.UnitOfMeasure ?? string.Empty;
            invoiceMatchItem.UnitCost = (Invo?.UnitCost ?? 0);
            invoiceMatchItem.AccountId = Invo?.AccountId ?? 0;
            invoiceMatchItem.Update(this.userData.DatabaseKey);
            return invoiceMatchItem;
        }
        public List<string> DeleteRowItem(long InvoiceMatchHeaderId, long InvoiceMatchId)
        {
            List<string> EMsg = new List<string>();
            InvoiceMatchHeader invoiceMatchHeader = new InvoiceMatchHeader()
            {
                InvoiceMatchHeaderId = InvoiceMatchHeaderId,
                ClientId = this.userData.DatabaseKey.Client.ClientId,
            };
            InvoiceMatchItem invoiceMatchItem = new InvoiceMatchItem()
            {
                InvoiceMatchItemId = InvoiceMatchId,
                ClientId = this.userData.DatabaseKey.Client.ClientId,
            };
            invoiceMatchHeader.Retrieve(this.userData.DatabaseKey);
            invoiceMatchItem.Retrieve(this.userData.DatabaseKey);
            if (invoiceMatchHeader.Status.ToLower() != "open")
            {
                invoiceMatchItem.ErrorMessages = new List<string> { "spnCandeltNotOpenItem" };
                EMsg = invoiceMatchItem.ErrorMessages;
            }
            else
            {
                invoiceMatchItem.DeleteItem(this.userData.DatabaseKey);
                EMsg = invoiceMatchItem.ErrorMessages;
            }
            return EMsg;
        }
        public List<PopupGridViewModel> ReceiptPopUpPopulate(long VendorId)
        {
            List<POReceipt> poReceiptList = new List<POReceipt>();
            PopupGridViewModel pMod;
            List<PopupGridViewModel> pModList = new List<PopupGridViewModel>();
            POReceipt poReceipt = new POReceipt();
            poReceipt.POReceiptItem.ClientId = this.userData.DatabaseKey.Client.ClientId;
            poReceipt.SiteId = this.userData.DatabaseKey.Personnel.SiteId;
            poReceipt.VendorId = VendorId;
            poReceiptList = poReceipt.RetrieveAllNonInvoiced(this.userData.DatabaseKey);
            foreach (var item in poReceiptList)
            {
                pMod = new PopupGridViewModel();
                pMod.POReceiptItemId = item.POReceiptItemId;
                pMod.POClientLookupId = item.POClientLookupId;
                pMod.ReceivedDate = item.ReceivedDate;
                pMod.PartClientLookupId = item.PartClientLookupId;
                pMod.Description = item.Description;
                pMod.QuantityReceived = item.QuantityReceived;
                pMod.UnitOfMeasure = item.UnitOfMeasure;
                pMod.UnitCost = item.UnitCost;
                pMod.TotalCost = item.TotalCost;
                pMod.AccountId = item.AccountId;
                pModList.Add(pMod);
            }
            return pModList;
        }
        public InvoiceMatchItem AddReceipt(InvoiceMatchItemModel Obj)
        {
            InvoiceMatchItem objInvoice = new InvoiceMatchItem();
            objInvoice.Description = Obj.Description;
            objInvoice.Quantity = Obj.Quantity ?? 0;
            objInvoice.UnitOfMeasure = Obj.UnitOfMeasure;
            objInvoice.UnitCost = Obj.UnitCost ?? 0;
            objInvoice.AccountId = Obj.AccountId ?? 0;
            objInvoice.InvoiceMatchHeaderId = Obj.InvoiceMatchHeaderId;
            objInvoice.ClientId = this.userData.DatabaseKey.Client.ClientId;
            objInvoice.POReceiptItemID = Obj.POReceiptItemID;
            objInvoice.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            objInvoice.InvoiceMatchItemId = 0;
            objInvoice.CreateTo(this.userData.DatabaseKey);
            return objInvoice;
        }
        public bool UpdatePopupReceiptListGrid(List<InvoiceGridListModel> list)
        {
            try
            {
                foreach (var item in list)
                {
                    InvoiceMatchItem objInvoice = new InvoiceMatchItem();
                    InvoiceMatchItemModel model = new InvoiceMatchItemModel();
                    objInvoice.POReceiptItemID = item.POReceiptItemID;
                    objInvoice.Quantity = item.QuantityReceived;
                    objInvoice.UnitCost = item.UnitCost;
                    objInvoice.UnitOfMeasure = item.UnitOfMeasure;
                    objInvoice.Description = item.Description;
                    objInvoice.AccountId = item.AccountId;
                    objInvoice.ClientId = this.userData.DatabaseKey.Client.ClientId;
                    objInvoice.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    objInvoice.InvoiceMatchHeaderId = item.InvoiceMatchHeaderId;
                    objInvoice.AccountId = item.AccountId;
                    objInvoice.InvoiceMatchItemId = 0;
                    objInvoice.Create(this.userData.DatabaseKey);
                    if (objInvoice.InvoiceMatchItemId > 0)
                    {
                        POReceiptItem pri = new POReceiptItem();
                        pri.ClientId = this.userData.DatabaseKey.Client.ClientId;
                        pri.POReceiptItemId = item.POReceiptItemID;
                        pri.Retrieve(this.userData.DatabaseKey);
                        pri.Invoiced = true;
                        pri.Update(this.userData.DatabaseKey);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region Notes
        public List<Client.Models.Invoice.InvoiceNoteModel> PopulateNotes(long InvoiceId)
        {
            Client.Models.Invoice.InvoiceNoteModel objNotesModel;
            List<Client.Models.Invoice.InvoiceNoteModel> NotesModelList = new List<Client.Models.Invoice.InvoiceNoteModel>();
            Notes note = new Notes()
            {
                ObjectId = InvoiceId,
                TableName = "InvoiceMatch"
            };
            List<Notes> NotesList = note.RetrieveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
            if (NotesList != null)
            {
                foreach (var item in NotesList)
                {
                    objNotesModel = new Client.Models.Invoice.InvoiceNoteModel();
                    objNotesModel.NotesId = item.NotesId;
                    objNotesModel.Subject = item.Subject;
                    objNotesModel.OwnerName = item.OwnerName;
                    objNotesModel.ModifiedDate = item.ModifiedDate;
                    NotesModelList.Add(objNotesModel);
                }
            }
            return NotesModelList;
        }
        public List<string> AddNote(Client.Models.Invoice.InvoiceNoteModel objNote)
        {
            Notes notes = new Notes();
            notes.OwnerId = userData.DatabaseKey.User.UserInfoId;
            notes.OwnerName = userData.DatabaseKey.User.FullName;
            notes.Subject = objNote.Subject;
            notes.Content = objNote.Content;
            notes.Type = objNote.Type;
            notes.TableName = objNote.TableName;
            notes.ObjectId = objNote.ObjectId;
            notes.UpdateIndex = objNote.updatedindex;
            notes.NotesId = objNote.NotesId;
            notes.Create(this.userData.DatabaseKey);
            return notes.ErrorMessages;
        }
        public List<string> UpdateNote(Client.Models.Invoice.InvoiceNoteModel objNote)
        {
            Notes notes = new Notes();
            notes.OwnerId = userData.DatabaseKey.User.UserInfoId;
            notes.OwnerName = userData.DatabaseKey.User.FullName;
            notes.Subject = objNote.Subject;
            notes.Content = objNote.Content;
            notes.Type = objNote.Type;
            notes.TableName = objNote.TableName;
            notes.ObjectId = objNote.ObjectId;
            notes.UpdateIndex = objNote.updatedindex;
            notes.NotesId = objNote.NotesId;
            notes.Update(this.userData.DatabaseKey);
            return notes.ErrorMessages;
        }
        public Client.Models.Invoice.InvoiceNoteModel getNoteById(long NotesId)
        {
            Client.Models.Invoice.InvoiceNoteModel objNotesModel = new Client.Models.Invoice.InvoiceNoteModel();
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
            return objNotesModel;
        }
        public bool DeleteNote(long _notesId)
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

        #region Event Log
        public void CreateEventLog(long objId, string eventVal)
        {
            InvoiceMatchEventLog log = new InvoiceMatchEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            log.ObjectId = objId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = eventVal;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = string.Empty;
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region V2-981
        public void ChangeInvoiceModel(long invoiceId, InvoiceVM objInvVM)
        {
            objInvVM.changeInvoiceModel.invoiceId = invoiceId;
            objInvVM.changeInvoiceModel.oldClientLookupId = objInvVM.InvoiceMatchHeaderModel.ClientLookupId;
            objInvVM.security = userData.Security;
            objInvVM.udata = userData;
        }
        public InvoiceMatchHeader ChangeOptions(string mode, long invoiceId, out string msg)
        {
            InvoiceMatchHeader objInvoice = new InvoiceMatchHeader()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                InvoiceMatchHeaderId = invoiceId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            objInvoice.RetrieveByPKForeignKeys(userData.DatabaseKey);
            objInvoice.Status = mode;
            bool InvoiceVarianceCheck = false;

            switch (mode)
            {
                case InvoiceMatchStatus.AuthorizedToPay:
                    Site site = new Site()
                    {
                        ClientId = this.userData.DatabaseKey.Client.ClientId,
                        SiteId = this.userData.DatabaseKey.Personnel.SiteId
                    };
                    site.Retrieve(userData.DatabaseKey);
                    if (site.InvoiceVarianceCheck)
                    {
                        InvoiceVarianceCheck = true;
                        objInvoice.InvoiceVariance = site.InvoiceVariance;
                    }
                    objInvoice.AuthorizedToPay = true;
                    objInvoice.AuthorizedToPayDate = DateTime.UtcNow;
                    objInvoice.AuthorizedToPay_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    objInvoice.AuthorizedToPayBy = $"{userData?.DatabaseKey?.Personnel?.NameFirst} {userData?.DatabaseKey?.Personnel?.NameLast}";
                    break;

                case InvoiceMatchStatus.Paid:
                    objInvoice.Paid = true;
                    objInvoice.PaidDate = DateTime.UtcNow;
                    objInvoice.Paid_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    objInvoice.PaidBy = $"{userData?.DatabaseKey?.Personnel?.NameFirst} {userData?.DatabaseKey?.Personnel?.NameLast}";
                    break;

                case InvoiceMatchStatus.Reopen:
                    objInvoice.Status = InvoiceMatchStatus.Open;
                    objInvoice.AuthorizedToPay = false;
                    objInvoice.AuthorizedToPayDate = null;
                    objInvoice.AuthorizedToPay_PersonnelId = 0;
                    objInvoice.AuthorizedToPayBy = "";
                    break;
            }
            if(InvoiceVarianceCheck)
            {
                // Check if the item total is  within the specified variance of the Invoice Total Input
                objInvoice.ValidateVarianceCheck(this.userData.DatabaseKey);
                if (objInvoice.ErrorMessages != null && objInvoice.ErrorMessages.Count>0)
                {
                    msg = objInvoice.ErrorMessages.FirstOrDefault();
                    return objInvoice;
                }
               
            }
            objInvoice.Update(userData.DatabaseKey);
            msg = JsonReturnEnum.success.ToString();
            return objInvoice;
        }     

        public List<String> DeleteInvoice(long invoiceId)
        {
            List<string> EMsg = new List<string>();
            if (invoiceId > 0)
            {
                InvoiceMatchHeader objInvoice = new InvoiceMatchHeader();
                objInvoice.InvoiceMatchHeaderId = invoiceId;
                objInvoice.ClientId = userData.DatabaseKey.Personnel.ClientId;
                objInvoice.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                objInvoice.DeleteInvoiceMatchHeaderAndInvoiceMatchItemsId(userData.DatabaseKey);
                if (objInvoice.ErrorMessages != null && objInvoice.ErrorMessages.Count > 0)
                {
                    EMsg = objInvoice.ErrorMessages;
                }
            }
            return EMsg;
        }
        public void ChangeInvoiceOptions(ChangeInvoiceModel changeInvoiceModel, out List<string> errorMsg)
        {
            var objInvoiceVM = new InvoiceVM
            {
                InvoiceMatchHeaderModel = getInvoiceDetailsById(changeInvoiceModel.invoiceId)
            };

            objInvoiceVM.InvoiceMatchHeaderModel.ClientLookupId = changeInvoiceModel.ClientLookupId;
            objInvoiceVM.InvoiceMatchHeaderModel.Flag = "Check";
            errorMsg = UpdateClientLookupId(objInvoiceVM.InvoiceMatchHeaderModel);
        }


        #endregion

    }
}