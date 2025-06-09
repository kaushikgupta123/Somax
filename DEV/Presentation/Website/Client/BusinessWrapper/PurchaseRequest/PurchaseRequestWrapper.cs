using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models.PurchaseRequest;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using Notification;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Client.Models.PartLookup;
using Client.Models.PunchoutModel;
using System.Net;
using System.IO;
using Client.Models.PunchoutExport;
using Client.BusinessWrapper.Configuration.SiteSetUp;
using System.Reflection;
using Client.Localization;
using Client.Models.PurchaseRequest.UIConfiguration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Text;
using System.Data;

//using Client.Models;

namespace Client.BusinessWrapper
{
    public class PurchaseRequestWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        internal List<string> errorMessage = new List<string>();
        string BodyHeader = string.Empty;
        string BodyContent = string.Empty;
        string FooterSignature = string.Empty;

        public PurchaseRequestWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Get
        public List<PurchaseRequestModel> GetPurchaseRequestChunkList(int CustomQueryDisplayId, int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", DateTime? ProcessedStartDateVw = null, DateTime? ProcessedEndDateVw = null, DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CancelandDeniedStartDateVw = null, DateTime? CancelandDeniedEndDateVw = null, string PurchaseRequestClientLookUpId = "", string Reason = "", string Status = "", string Creator_PersonnelName = "", string VendorClientLookupId = "", string VendorName = "",
            string PurchaseOrderClientLookupId = "", string Processed_PersonnelName = "", string SearchText = "", DateTime? CreateStartDate = null, DateTime? CreateEndDate = null,
            DateTime? ProcessedStartDate = null, DateTime? ProcessedEndDate = null)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            PurchaseRequestModel pModel;
            List<PurchaseRequestModel> PurchaseRequestModelList = new List<PurchaseRequestModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            purchaseRequest.CustomQueryDisplayId = CustomQueryDisplayId;
            purchaseRequest.orderbyColumn = orderbycol;
            purchaseRequest.orderBy = orderDir;
            purchaseRequest.offset1 = Convert.ToString(skip);
            purchaseRequest.nextrow = Convert.ToString(length);

            purchaseRequest.ClientLookupId = PurchaseRequestClientLookUpId;
            purchaseRequest.Reason = Reason;
            purchaseRequest.Status = Status;
            purchaseRequest.Creator_PersonnelName = Creator_PersonnelName;
            purchaseRequest.VendorClientLookupId = VendorClientLookupId;
            purchaseRequest.VendorName = VendorName;
            purchaseRequest.PurchaseOrderClientLookupId = PurchaseOrderClientLookupId;
            purchaseRequest.Processed_PersonnelName = Processed_PersonnelName;
            purchaseRequest.CreateStartDate = CreateStartDate.HasValue ? CreateStartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseRequest.CreateEndDate = CreateEndDate.HasValue ? CreateEndDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseRequest.ProcessedStartDate = ProcessedStartDate.HasValue ? ProcessedStartDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseRequest.ProcessedEndDate = ProcessedEndDate.HasValue ? ProcessedEndDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseRequest.ProcessedStartDateVw = ProcessedStartDateVw.HasValue ? ProcessedStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseRequest.ProcessedEndDateVw = ProcessedEndDateVw.HasValue ? ProcessedEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            purchaseRequest.CreateStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseRequest.CreateEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseRequest.CancelandDeniedStartDateVw = CancelandDeniedStartDateVw.HasValue ? CancelandDeniedStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseRequest.CancelandDeniedEndDateVw = CancelandDeniedEndDateVw.HasValue ? CancelandDeniedEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            purchaseRequest.SearchText = SearchText;
            List<PurchaseRequest> purchaseRequestList = purchaseRequest.RetrieveChunkSearch(this.userData.DatabaseKey);
            if (purchaseRequestList != null)
            {
                foreach (var item in purchaseRequestList)
                {
                    pModel = new PurchaseRequestModel();
                    pModel.PurchaseRequestId = item.PurchaseRequestId;
                    pModel.ClientLookupId = item.ClientLookupId;
                    pModel.Reason = item.Reason;
                    pModel.Status = item.Status;
                    pModel.Creator_PersonnelName = item.Creator_PersonnelName;
                    pModel.VendorId = item.VendorId;
                    pModel.VendorClientLookupId = item.VendorClientLookupId;
                    pModel.VendorIsExternal = item.VendorIsExternal;
                    pModel.VendorName = item.VendorName;
                    if (item.CreateDate != null && item.CreateDate == default(DateTime))
                    {
                        pModel.CreateDate = null;
                    }
                    else
                    {
                        pModel.CreateDate = item.CreateDate;
                    }
                    pModel.PurchaseOrderClientLookupId = item.PurchaseOrderClientLookupId;
                    pModel.Processed_PersonnelName = item.Processed_PersonnelName;
                    if (item.ProcessedDate != null && item.ProcessedDate == default(DateTime))
                    {
                        pModel.ProcessedDate = null;
                    }
                    else
                    {
                        pModel.ProcessedDate = item.ProcessedDate;
                    }
                    pModel.ChildCount = item.ChildCount;
                    pModel.TotalCount = item.TotalCount;
                    PurchaseRequestModelList.Add(pModel);
                }
            }
            return PurchaseRequestModelList;
        }
        internal List<PurchaseRequestModel> PopulatePurchaseRequestDetails(int CustomQueryDisplayId)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            PurchaseRequestModel pModel;
            List<PurchaseRequestModel> PurchaseRequestModelList = new List<PurchaseRequestModel>();

            purchaseRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            purchaseRequest.CustomQueryDisplayId = CustomQueryDisplayId;
            List<PurchaseRequest> purchaseRequestList = purchaseRequest.RetrieveAllForSearchNew(this.userData.DatabaseKey);
            if (purchaseRequestList != null)
            {
                foreach (var item in purchaseRequestList)
                {
                    pModel = new PurchaseRequestModel();
                    pModel.PurchaseRequestId = item.PurchaseRequestId;
                    pModel.ClientLookupId = item.ClientLookupId;
                    pModel.Reason = item.Reason;
                    pModel.Status = item.Status;
                    pModel.Creator_PersonnelName = item.Creator_PersonnelName;
                    pModel.VendorId = item.VendorId;
                    pModel.VendorClientLookupId = item.VendorClientLookupId;
                    pModel.VendorName = item.VendorName;
                    if (item.CreateDate != null && item.CreateDate == default(DateTime))
                    {
                        pModel.CreateDate = null;
                    }
                    else
                    {
                        pModel.CreateDate = item.CreateDate;
                    }
                    pModel.PurchaseOrderClientLookupId = item.PurchaseOrderClientLookupId;
                    pModel.Processed_PersonnelName = item.Processed_PersonnelName;
                    if (item.ProcessedDate != null && item.ProcessedDate == default(DateTime))
                    {
                        pModel.ProcessedDate = null;
                    }
                    else
                    {
                        pModel.ProcessedDate = item.ProcessedDate;
                    }

                    PurchaseRequestModelList.Add(pModel);
                }
            }
            return PurchaseRequestModelList;
        }
        internal PurchaseRequestModel GetPurchaseRequestDetailById(long PurchaseRequestId)
        {
            PurchaseRequestModel pModel = new PurchaseRequestModel();
            PurchaseRequest purchaseRequest = new PurchaseRequest();

            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;

            purchaseRequest.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            pModel = PopulateModel(purchaseRequest);
            return pModel;
        }
        internal PurchaseRequestModel RetrieveByPKForeignKeysForReport(long PurchaseRequestId)
        {
            PurchaseRequestModel pModel = new PurchaseRequestModel();

            PurchaseRequest purchaseRequest = new PurchaseRequest();
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;

            purchaseRequest.RetrieveByPKForeignKeysForReport(userData.DatabaseKey, userData.Site.TimeZone);
            pModel = PopulateModel(purchaseRequest);
            return pModel;
        }
        private PurchaseRequestModel PopulateModel(PurchaseRequest dbObj)
        {
            PurchaseRequestModel pModel = new PurchaseRequestModel();
            pModel.VendorAddress1 = dbObj.VendorAddress1;
            pModel.VendorAddress2 = dbObj.VendorAddress2;
            pModel.VendorAddress3 = dbObj.VendorAddress3;
            pModel.VendorAddressCity = dbObj.VendorAddressCity;
            pModel.VendorAddressCityPrint = dbObj.VendorAddressCity + ',' + "  " + dbObj.VendorAddressState + "  " + dbObj.VendorAddressPostCode;
            pModel.VendorAddressState = dbObj.VendorAddressState;
            pModel.VendorAddressPostCode = dbObj.VendorAddressPostCode;
            pModel.VendorAddressCountry = dbObj.VendorAddressCountry;
            pModel.VendorPhoneNumber = dbObj.VendorPhoneNumber;


            pModel.ClientLookupId = dbObj.ClientLookupId;
            pModel.PurchaseRequestId = dbObj.PurchaseRequestId;
            pModel.Status = dbObj.Status;
            pModel.LocalizedStatus = UtilityFunction.GetMessageFromResource(dbObj.Status, LocalizeResourceSetConstants.StatusDetails);
            pModel.Reason = dbObj.Reason;
            pModel.VendorClientLookupId = dbObj.VendorClientLookupId;
            pModel.VendorIsExternal = dbObj.VendorIsExternal;
            pModel.VendorName = dbObj.VendorName;
            pModel.CountLineItem = dbObj.CountLineItem;
            pModel.TotalCost = dbObj.TotalCost;
            pModel.Creator_PersonnelName = dbObj.Creator_PersonnelName;
            pModel.CreateDate = dbObj.CreateDate;
            pModel.Approved_PersonnelName = dbObj.Approved_PersonnelName;
            pModel.Approved_Date = dbObj.Approved_Date;
            pModel.Processed_PersonnelName = dbObj.Processed_PersonnelName;
            pModel.Processed_Date = dbObj.Processed_Date;
            pModel.AutoGenerated = dbObj.AutoGenerated;
            pModel.PurchaseOrderClientLookupId = dbObj.PurchaseOrderClientLookupId;
            pModel.ToEmailId = dbObj.VendorEmailAddress;
            pModel.CcEmailId = this.userData.DatabaseKey.User.Email;
            if (dbObj.Status == PurchaseRequestStatusConstants.Denied)
            {
                pModel.Process_Comments = dbObj.Process_Comments;

            }
            else if (dbObj.Status == PurchaseRequestStatusConstants.AwaitApproval)
            {
                pModel.Process_Comments = dbObj.Process_Comments;

            }
            else if (dbObj.Status == PurchaseRequestStatusConstants.Resubmit)
            {
                pModel.Process_Comments = dbObj.Return_Comments;
            }
            pModel.SiteId = dbObj.SiteId;
            pModel.ClientId = dbObj.ClientId;
            pModel.AreaId = dbObj.AreaId;
            pModel.DepartmentId = dbObj.DepartmentId;
            pModel.StoreroomId = dbObj.StoreroomId;

            pModel.ApprovedBy_PersonnelId = dbObj.ApprovedBy_PersonnelId;
            pModel.Approved_Date = dbObj.Approved_Date;
            pModel.CreatedBy_PersonnelId = dbObj.CreatedBy_PersonnelId;
            pModel.Process_Comments = dbObj.Process_Comments;
            pModel.ProcessBy_PersonnelId = dbObj.ProcessBy_PersonnelId;

            pModel.VendorId = dbObj.VendorId;
            pModel.PurchaseOrderId = dbObj.PurchaseOrderId;

            pModel.Return_Comments = dbObj.Return_Comments;
            pModel.ExtractLogId = dbObj.ExtractLogId;
            pModel.UpdateIndex = dbObj.UpdateIndex;
            pModel.IsPunchOut = dbObj.IsPunchOut;
            #region Report element

            pModel.SiteName = dbObj.SiteName;

            pModel.SiteAddress1 = dbObj.SiteAddress1;
            pModel.SiteAddress2 = dbObj.SiteAddress2;
            pModel.SiteAddress3 = dbObj.SiteAddress3;
            pModel.SiteAddressCity = dbObj.SiteAddressCity;
            pModel.SiteAddressCityPrint = dbObj.SiteAddressCity + "," + "  " + dbObj.SiteAddressState + "  " + dbObj.SiteAddressPostCode;
            pModel.SiteAddressCountry = dbObj.SiteAddressCountry;
            pModel.SiteAddressPostCode = dbObj.SiteAddressPostCode;
            pModel.SiteAddressState = dbObj.SiteAddressState;

            pModel.SiteBillToAddress1 = dbObj.SiteBillToAddress1;
            pModel.SiteBillToAddress2 = dbObj.SiteBillToAddress2;
            pModel.SiteBillToAddress3 = dbObj.SiteBillToAddress3;
            pModel.SiteBillToAddressCity = dbObj.SiteBillToAddressCity;
            pModel.SiteBillToAddressCityPrint = dbObj.SiteBillToAddressCity + "," + "  " + dbObj.SiteBillToAddressState + "  " + dbObj.SiteBillToAddressPostCode;
            pModel.SiteBillToAddressCountry = dbObj.SiteBillToAddressCountry;
            pModel.SiteBillToAddressPostCode = dbObj.SiteBillToAddressPostCode;
            pModel.SiteBillToAddressState = dbObj.SiteBillToAddressState;


            #endregion
            return pModel;
        }
        #endregion

        #region Add/Edit
        internal PurchaseRequest EditpurchaseRequest(PurchaseRequestModel pRModel)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseRequestId = pRModel.PurchaseRequestId
            };
            purchaseRequest.Retrieve(userData.DatabaseKey);
            purchaseRequest.Reason = pRModel.Reason ?? string.Empty;
            // RKL - 2022-05-10
            if (!(pRModel.IsPunchOut))
            {
                purchaseRequest.VendorId = pRModel.VendorId ?? 0;
            }
            //if (!(pRModel.IsPunchOut  || pRModel.CountLineItem > 0))
            //{
            //    purchaseRequest.VendorId = pRModel.VendorId ?? 0;
            //}
            //else
            //{ 
            //    string Vendor_ClientLookupId = pRModel.VendorClientLookupId;
            //    Vendor vendor = new Vendor { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = Vendor_ClientLookupId };
            //    List<Vendor> vlist = vendor.RetrieveBySiteIdAndClientLookUpId(_dbKey);
            //    if (vlist != null && vlist.Count > 0)
            //    { pRModel.VendorId = vlist[0].VendorId; }

            //    purchaseRequest.Retrieve(userData.DatabaseKey);

            //    purchaseRequest.VendorId = pRModel.VendorId ?? 0;
            //    purchaseRequest.Reason = pRModel.Reason ?? string.Empty;
            //    purchaseRequest.Update(userData.DatabaseKey);
            //}
            purchaseRequest.Update(userData.DatabaseKey);
            return purchaseRequest;
        }
        internal PurchaseRequest AddPurchaseRequest(PurchaseRequestModel obj)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CustomSequentialId custid = new CustomSequentialId();
            custid.KeyList = AutoGenerateKey.PR_ANNUAL;
            var custList = custid.RetrieveByClientIdandSiteIdandKey_V2(userData.DatabaseKey);
            string PRPrefix = custList != null ? custList.Where(x => x.Key == AutoGenerateKey.PR_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : "";
            purchaseRequest.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            purchaseRequest.ClientId = this.userData.DatabaseKey.Client.ClientId;

            purchaseRequest.VendorId = obj.VendorId ?? 0;// vlist[0].VendorId;

            purchaseRequest.CreatedBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseRequest.Status = PurchaseRequestStatusConstants.Open;
            purchaseRequest.Reason = obj.Reason;

            purchaseRequest.CreateByPKForeignKeys(this.userData.DatabaseKey, true, AutoGenerateKey.PR_ANNUAL, PRPrefix);
            CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.PROpen);
            if (obj.IsAdditionalCatalogItem)
            {
                PurchaseRequestLineItem pordLineItem = new PurchaseRequestLineItem();
                pordLineItem.ClientId = _dbKey.Client.ClientId;
                pordLineItem.PurchaseRequestId = purchaseRequest.PurchaseRequestId;
                pordLineItem.Creator_PersonnelId = _dbKey.Personnel.PersonnelId;
                pordLineItem.Description = obj.Description;
                pordLineItem.LineNumber = obj.LineNumber;
                pordLineItem.PartId = obj.PartId;
                pordLineItem.PartStoreroomId = obj.PartStoreroomId;
                pordLineItem.OrderQuantity = obj.OrderQuantity;
                pordLineItem.PurchaseUOM = obj.PurchaseUOM;
                pordLineItem.UnitofMeasure = obj.UnitofMeasure;
                pordLineItem.UnitCost = obj.UnitCost;
                pordLineItem.VendorCatalogItemId = obj.VendorCatalogItemId;
                pordLineItem.CreateFromAdditionalCatalogItem(_dbKey);
            }
            return purchaseRequest;
        }

        internal PurchaseRequest AddPurchaseRequestPunchOut(long? VendorId)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();

            CustomSequentialId custid = new CustomSequentialId();
            custid.KeyList = AutoGenerateKey.PR_ANNUAL;
            var custList = custid.RetrieveByClientIdandSiteIdandKey_V2(userData.DatabaseKey);
            string PRPrefix = custList != null ? custList.Where(x => x.Key == AutoGenerateKey.PR_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : null;
            purchaseRequest.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            purchaseRequest.ClientId = this.userData.DatabaseKey.Client.ClientId;

            purchaseRequest.VendorId = VendorId ?? 0;// vlist[0].VendorId;

            purchaseRequest.CreatedBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseRequest.Status = PurchaseRequestStatusConstants.Open;
            purchaseRequest.Reason = string.Empty;
            purchaseRequest.IsPunchOut = true;
            purchaseRequest.CreateByPKForeignKeys(this.userData.DatabaseKey, true, AutoGenerateKey.PR_ANNUAL, PRPrefix);
            CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.PROpen);
            return purchaseRequest;
        }
        #endregion

        #region Notes
        internal List<NotesModel> PopulateNotes(long PurchaseRequestId)
        {
            NotesModel objNotesModel;
            List<NotesModel> NotesModelList = new List<NotesModel>();

            Notes note = new Notes()
            {
                ObjectId = PurchaseRequestId,
                TableName = "PurchaseRequest"
            };
            List<Notes> NotesList = note.RetrieveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
            if (NotesList != null)
            {
                foreach (var item in NotesList)
                {
                    objNotesModel = new NotesModel();
                    objNotesModel.NotesId = item.NotesId;
                    objNotesModel.Subject = item.Subject;
                    objNotesModel.OwnerName = item.OwnerName;
                    objNotesModel.ModifiedDate = item.ModifiedDate;
                    NotesModelList.Add(objNotesModel);
                }
            }
            return NotesModelList;
        }
        internal NotesModel EditPurchaseReqNote(long PurchaseRequestId, long NotesId)
        {
            PurchaseRequestVM objPurchaseRequestVM = new PurchaseRequestVM();
            PurchaseRequestModel objPurchaseRequestModel = new PurchaseRequestModel();
            PurchaseRequestWrapper pWrapper = new PurchaseRequestWrapper(userData);
            NotesModel objNotesModel = new NotesModel();
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
            objNotesModel.PurchaseRequestId = PurchaseRequestId;
            objNotesModel.ClientLookupId = objPurchaseRequestModel.ClientLookupId;
            return objNotesModel;
        }
        internal List<string> UpdatePurchaseReqNote(PurchaseRequestVM _PurchaseRequest)
        {
            Notes notes = new Notes()
            {
                OwnerId = userData.DatabaseKey.User.UserInfoId,
                OwnerName = userData.DatabaseKey.User.FullName,
                Subject = _PurchaseRequest.notesModel.Subject,
                Content = _PurchaseRequest.notesModel.Content,
                Type = _PurchaseRequest.notesModel.Type,
                TableName = "PurchaseRequest",
                ObjectId = _PurchaseRequest.notesModel.PurchaseRequestId,
                UpdateIndex = _PurchaseRequest.notesModel.updatedindex,
                NotesId = _PurchaseRequest.notesModel.NotesId
            };
            if (_PurchaseRequest.notesModel.NotesId == 0)
            {
                notes.Create(userData.DatabaseKey);
                return notes.ErrorMessages;
            }
            else
            {
                notes.ObjectId = _PurchaseRequest.notesModel.PurchaseRequestId;
                notes.Update(userData.DatabaseKey);
                return notes.ErrorMessages;
            }
        }
        internal bool DeletePurchaseReqNote(string _notesId)
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

        #region Line Item
        internal List<LineItemModel> PopulateLineitems(long PurchaseRequestId)
        {
            LineItemModel objLineItem;
            List<LineItemModel> LineItemList = new List<LineItemModel>();

            PurchaseRequestLineItem purchaseRequestLineItem = new PurchaseRequestLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestId = PurchaseRequestId
            };
            List<PurchaseRequestLineItem> purchaseRequestLintItemList = PurchaseRequestLineItem.PurchaseRequestLineItemRetrieveByPurchaseRequestId_Latest(this.userData.DatabaseKey, purchaseRequestLineItem);

            if (purchaseRequestLintItemList != null)
            {
                foreach (var item in purchaseRequestLintItemList)
                {
                    objLineItem = new LineItemModel();
                    objLineItem.PurchaseRequestLineItemId = item.PurchaseRequestLineItemId;
                    objLineItem.LineNumber = item.LineNumber;
                    objLineItem.PartClientLookupId = item.PartClientLookupId;
                    objLineItem.Description = item.Description;
                    objLineItem.OrderQuantity = Math.Round(item.OrderQuantity, 2);
                    objLineItem.UnitofMeasure = item.UnitofMeasure;
                    objLineItem.UnitCost = Math.Round(item.UnitCost, 2);
                    objLineItem.TotalCost = Math.Round(item.TotalCost, 2);
                    objLineItem.Account_ClientLookupId = item.Account_ClientLookupId;
                    objLineItem.PurchaseUOM = item.PurchaseUOM;
                    objLineItem.RequiredDate = item.RequiredDate;
                    objLineItem.PartId = item.PartId;
                    if (objLineItem.RequiredDate == null || objLineItem.RequiredDate == default(DateTime))
                    {
                        objLineItem.RequiredDate = null;
                    }
                    else
                    {
                        objLineItem.RequiredDate = objLineItem.RequiredDate;
                    }

                    objLineItem.ChargeToClientLookupId = item.ChargeToClientLookupId;//Added in V2-672
                    LineItemList.Add(objLineItem);
                }
            }

            return LineItemList;
        }
        internal LineItemModel GetLineItem(long PurchaseRequestLineItemId, long PurchaseRequestId)
        {
            LineItemModel objLineItem = new LineItemModel();
            PurchaseRequestLineItem purchaseRequestlineitem = new PurchaseRequestLineItem()
            {
                PurchaseRequestLineItemId = PurchaseRequestLineItemId,
                ClientId = userData.DatabaseKey.Personnel.ClientId,

            };

            purchaseRequestlineitem.PurchaseRequestLineItemRetrieveByPurchaseRequestLineItemIdV2(this.userData.DatabaseKey);

            if (purchaseRequestlineitem != null)
            {
                objLineItem.PurchaseRequestLineItemId = PurchaseRequestLineItemId;
                objLineItem.PurchaseRequestId = PurchaseRequestId;
                objLineItem.PartId = purchaseRequestlineitem.PartId;
                objLineItem.LineNumber = purchaseRequestlineitem.LineNumber;
                objLineItem.PartClientLookupId = purchaseRequestlineitem.PartClientLookupId;
                objLineItem.Description = purchaseRequestlineitem.Description;
                objLineItem.OrderQuantity = purchaseRequestlineitem.OrderQuantity;
                objLineItem.UnitofMeasure = purchaseRequestlineitem.UnitofMeasure;
                objLineItem.UnitCost = purchaseRequestlineitem.UnitCost;
                objLineItem.TotalCost = purchaseRequestlineitem.TotalCost;
                objLineItem.Account_ClientLookupId = purchaseRequestlineitem.Account_ClientLookupId;
                objLineItem.AccountId = purchaseRequestlineitem.AccountId;
                objLineItem.ChargeTo_Name = purchaseRequestlineitem.ChargeTo_Name;
                objLineItem.ChargeType = purchaseRequestlineitem.ChargeType;
                objLineItem.ChargeToClientLookupId = purchaseRequestlineitem.ChargeToClientLookupId;
                objLineItem.ChargeToID = purchaseRequestlineitem.ChargeToID;
                objLineItem.ispunchout = purchaseRequestlineitem.Ispunchout;
                objLineItem.PurchaseUOM = purchaseRequestlineitem.PurchaseUOM;
                objLineItem.UOMConvRequired = purchaseRequestlineitem.UOMConvRequired;
                objLineItem.UOMConversion = purchaseRequestlineitem.UOMConversion;
                if (purchaseRequestlineitem.RequiredDate.HasValue && purchaseRequestlineitem.RequiredDate.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                {
                    objLineItem.RequiredDate = purchaseRequestlineitem.RequiredDate;
                }
            }
            return objLineItem;
        }
        internal PurchaseRequestLineItem UpdateLineItem(LineItemModel lineItem)
        {
            PurchaseRequestLineItem purchaseRequestlineitem = new PurchaseRequestLineItem()
            {
                PurchaseRequestLineItemId = lineItem.PurchaseRequestLineItemId,
                PurchaseRequestId = lineItem.PurchaseRequestId
            };

            purchaseRequestlineitem.Retrieve(userData.DatabaseKey);

            string status = lineItem.status ?? string.Empty;
            string OldChargeType = purchaseRequestlineitem.ChargeType ?? string.Empty;
            long OldChargeToId = purchaseRequestlineitem.ChargeToID;
            string NewChargeType = lineItem.ChargeType ?? string.Empty;
            long NewChargeToId = lineItem.ChargeToID;

            if (purchaseRequestlineitem != null)
            {
                purchaseRequestlineitem.Description = lineItem.Description;
                purchaseRequestlineitem.OrderQuantity = lineItem.OrderQuantity ?? 0;
                purchaseRequestlineitem.UnitofMeasure = lineItem.UnitofMeasure == null ? string.Empty : lineItem.UnitofMeasure;
                purchaseRequestlineitem.UnitCost = lineItem.UnitCost;
                purchaseRequestlineitem.TotalCost = lineItem.TotalCost;
                purchaseRequestlineitem.AccountId = lineItem.AccountId ?? 0;
                purchaseRequestlineitem.ChargeType = lineItem.ChargeType ?? string.Empty;
                purchaseRequestlineitem.ChargeToID = lineItem.ChargeToID;
                purchaseRequestlineitem.PurchaseUOM = lineItem.PurchaseUOM == null ? string.Empty : lineItem.PurchaseUOM;
                if (userData.Site.ShoppingCart)
                {
                    purchaseRequestlineitem.RequiredDate = lineItem.RequiredDate;
                }
            }
            if (purchaseRequestlineitem.PartId == 0)
            {
                purchaseRequestlineitem.UnitofMeasure = purchaseRequestlineitem.PurchaseUOM;
            }
            purchaseRequestlineitem.UpdateByPKForeignKeys(this.userData.DatabaseKey);
            if (status == PurchaseRequestStatusConstants.Open || status == PurchaseRequestStatusConstants.Approved || status == PurchaseRequestStatusConstants.Resubmit || status == PurchaseRequestStatusConstants.AwaitApproval || status == PurchaseRequestStatusConstants.Extracted)
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

            return purchaseRequestlineitem;
        }
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
                pInventory.Quantity = p.QtyOnHand;
                pInventoryList.Add(pInventory);
            }
            return pInventoryList;
        }
        internal bool DeleteLineItem(long PurchaseRequestLineItemId, long PurchaseRequestId, string Status)
        {
            bool retValue = false;
            PurchaseRequestLineItem purchaseRequestLineItem = new PurchaseRequestLineItem()
            {
                PurchaseRequestLineItemId = PurchaseRequestLineItemId,
                PurchaseRequestId = PurchaseRequestId

            };
            purchaseRequestLineItem.Retrieve(userData.DatabaseKey);
            // V2-856 - Change the status to Open, AwaitApproval, Resubmit
            //if (Status != PurchaseOrderStatusConstants.Open)
            if (Status != PurchaseRequestStatusConstants.Open
              && Status != PurchaseRequestStatusConstants.AwaitApproval
              && Status != PurchaseRequestStatusConstants.Resubmit
              && Status != PurchaseRequestStatusConstants.Approved)
            {
                return retValue;
            }
            else
            {
                retValue = true;
                long ChargeToID = purchaseRequestLineItem.ChargeToID;
                string ChargeType = purchaseRequestLineItem.ChargeType ?? string.Empty;
                purchaseRequestLineItem.Delete(userData.DatabaseKey);
                #region Delete LineItem Records from UDF V2-653
                if (purchaseRequestLineItem.ErrorMessages == null || purchaseRequestLineItem.ErrorMessages.Count == 0)
                {
                    PRLineUDF objPRLineUDF = new PRLineUDF()
                    {
                        PurchaseRequestLineItemId = PurchaseRequestLineItemId
                    };
                    objPRLineUDF.DeleteByPurchaseRequestLineItemId(userData.DatabaseKey);
                }
                #endregion
                purchaseRequestLineItem.PRReOrderLineNumber(userData.DatabaseKey);
                if (ChargeType == AttachmentTableConstant.WorkOrder && (Status == PurchaseRequestStatusConstants.Open || Status == PurchaseRequestStatusConstants.Approved || Status == PurchaseRequestStatusConstants.Resubmit || Status == PurchaseRequestStatusConstants.AwaitApproval || Status == PurchaseRequestStatusConstants.Extracted))
                {
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(ChargeToID, "Minus"));

                }


            }
            return retValue;
        }
        #endregion

        #region Update By Status  

        internal void UpdateStatus(long PurchaseRequestId, string Status, string Process_Comments = null, int PersonnelId = 0, int lineCount = 0)

        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;

            purchaseRequest.Retrieve(userData.DatabaseKey);
            string PrevStatus = purchaseRequest.Status ?? string.Empty;
            switch (Status)
            {
                case "approve":
                    purchaseRequest.Status = PurchaseRequestStatusConstants.Approved;
                    purchaseRequest.Approved_Date = System.DateTime.UtcNow;
                    purchaseRequest.ApprovedBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    purchaseRequest.Update(userData.DatabaseKey);
                    if (purchaseRequest.ErrorMessages != null && purchaseRequest.ErrorMessages.Count > 0)
                    {
                        foreach (var vr in purchaseRequest.ErrorMessages)
                        {
                            errorMessage.Add(vr);
                        }
                    }
                    PRlist.Add(purchaseRequest.PurchaseRequestId);
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestApproved, PRlist);
                    CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.Approved);
                    break;
                case "deny":
                    purchaseRequest.Status = PurchaseRequestStatusConstants.Denied;
                    purchaseRequest.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    purchaseRequest.Processed_Date = System.DateTime.UtcNow;
                    purchaseRequest.Process_Comments = Process_Comments;
                    purchaseRequest.Update(userData.DatabaseKey);
                    if (purchaseRequest.ErrorMessages != null && purchaseRequest.ErrorMessages.Count > 0)
                    {
                        foreach (var vr in purchaseRequest.ErrorMessages)
                        {
                            errorMessage.Add(vr);
                        }
                    }
                    PRlist.Add(purchaseRequest.PurchaseRequestId);
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestDenied, PRlist);
                    CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.Denied);
                    if (lineCount > 0 && (PrevStatus == PurchaseRequestStatusConstants.Open || PrevStatus == PurchaseRequestStatusConstants.Approved || PrevStatus == PurchaseRequestStatusConstants.Resubmit || PrevStatus == PurchaseRequestStatusConstants.AwaitApproval || PrevStatus == PurchaseRequestStatusConstants.Extracted))
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdateListPartsonOrder(purchaseRequest.PurchaseRequestId, "Minus", AttachmentTableConstant.PurchaseRequest));
                    }
                    break;
                case "Cancel":
                    purchaseRequest.Status = PurchaseRequestStatusConstants.Canceled;
                    purchaseRequest.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    purchaseRequest.Processed_Date = System.DateTime.UtcNow;
                    purchaseRequest.Process_Comments = Process_Comments;
                    purchaseRequest.Update(userData.DatabaseKey);
                    PRlist.Add(purchaseRequest.PurchaseRequestId);
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestDenied, PRlist);
                    CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.Canceled);
                    if (lineCount > 0 && (PrevStatus == PurchaseRequestStatusConstants.Open || PrevStatus == PurchaseRequestStatusConstants.Approved || PrevStatus == PurchaseRequestStatusConstants.Resubmit || PrevStatus == PurchaseRequestStatusConstants.AwaitApproval || PrevStatus == PurchaseRequestStatusConstants.Extracted))
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdateListPartsonOrder(purchaseRequest.PurchaseRequestId, "Minus", AttachmentTableConstant.PurchaseRequest));
                    }
                    break;
                case "ReturnToRequester":
                    purchaseRequest.Status = PurchaseRequestStatusConstants.Resubmit;
                    purchaseRequest.Return_Comments = Process_Comments;
                    purchaseRequest.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    purchaseRequest.Processed_Date = System.DateTime.UtcNow;
                    purchaseRequest.Update(userData.DatabaseKey);
                    if (purchaseRequest.ErrorMessages != null && purchaseRequest.ErrorMessages.Count > 0)
                    {
                        foreach (var vr in purchaseRequest.ErrorMessages)
                        {
                            errorMessage.Add(vr);
                        }
                    }
                    CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.Resubmit, Process_Comments);  //V2-375
                    List<long> PrId = new List<long> { purchaseRequest.PurchaseRequestId };
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestReturned, PrId);
                    break;
                case "SendforApproval":
                    PurchaseRequest pr = new PurchaseRequest();
                    pr.ClientId = userData.DatabaseKey.Client.ClientId;
                    pr.PurchaseRequestId = PurchaseRequestId;
                    pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.DatabaseKey.Client.DefaultTimeZone);
                    pr.Status = PurchaseRequestStatusConstants.AwaitApproval;
                    // SOM-1199
                    pr.Process_Comments = Process_Comments;
                    pr.UpdateIndex = purchaseRequest.UpdateIndex;
                    pr.Update(userData.DatabaseKey);
                    CreateEventLog(pr.PurchaseRequestId, PurchasingEvents.SendForApproval);
                    // Send Notification
                    PRlist.Add(pr.PurchaseRequestId);
                    // Set the list of targets - only one in this case
                    List<long> targets = new List<long>();
                    targets.Add(PersonnelId);
                    objAlert.SetTargetList(targets);
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestApprovalNeeded, PRlist);
                    break;
            }

        }

        internal void UpdateEmailToVendorStatus(string emailHtmlBody, System.IO.MemoryStream stream, long PurchaseRequestId,
                                   int PersonnelId = 0, string ToEmail = null, string CCEmail = null, string MailBodyComments = null)

        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;
            purchaseRequest.Retrieve(userData.DatabaseKey);
            string resetUrl = GetHostedUrl();
            Presentation.Common.EmailModule emailModule = new Presentation.Common.EmailModule();
            emailModule.ToEmailAddress = ToEmail;
            emailModule.CcEmail = CCEmail;
            BodyContent = emailHtmlBody;
            emailModule.Subject = "Purchase Request" + " " + purchaseRequest.ClientLookupId;
            BodyHeader = "Purchase Request" + " " + purchaseRequest.ClientLookupId + " " + "is attached";
            FooterSignature = this.userData.DatabaseKey.User.FirstName + " " + this.userData.DatabaseKey.User.LastName;
            if (BodyContent != null)
            {
                string output = BodyContent.
                    Replace("headerBgURL", resetUrl + SomaxAppConstants.HeaderMailTemplate).
                    Replace("somaxLogoURL", resetUrl + SomaxAppConstants.SomaxLogoForMailTemplate).
                    Replace("strrequestid", purchaseRequest.ClientLookupId).
                    Replace("strmailbodycomments", MailBodyComments).
                    Replace("strtypeofrequest", "Purchase Request").
                    Replace("spnloginurl", resetUrl).
                    Replace("footerURL", resetUrl + SomaxAppConstants.FooterMailTemplate); ;
                emailModule.MailBody = output;
            }
            else
            {
                emailModule.MailBody = "<html><body>" + BodyHeader + " <br><br> " + FooterSignature + "";
            }

            #region Attachment
            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(stream, "PurchaseRequestDetails.pdf", "application/pdf");
            emailModule.MailAttachment = attachment;
            #endregion

            emailModule.SendEmail();
            CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.EmailToVendor);
        }

        internal PurchaseRequest UpdateStatusBatch(long PurchaseRequestId, PurchaseReturnStatusEnum Status, string Process_Comments = null, int lineCount = 0)

        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;
            purchaseRequest.Retrieve(userData.DatabaseKey);
            string PrevStatus = purchaseRequest.Status ?? string.Empty;
            switch (Status)
            {
                case PurchaseReturnStatusEnum.approve:
                    purchaseRequest.Status = PurchaseRequestStatusConstants.Approved;
                    purchaseRequest.Approved_Date = System.DateTime.UtcNow;
                    purchaseRequest.ApprovedBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    purchaseRequest.Update(userData.DatabaseKey);
                    PRlist.Add(purchaseRequest.PurchaseRequestId);
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestApproved, PRlist);
                    CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.Approved);
                    break;
                case PurchaseReturnStatusEnum.deny:
                    purchaseRequest.Status = PurchaseRequestStatusConstants.Denied;
                    purchaseRequest.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    purchaseRequest.Processed_Date = System.DateTime.UtcNow;
                    purchaseRequest.Process_Comments = Process_Comments;
                    purchaseRequest.Update(userData.DatabaseKey);
                    PRlist.Add(purchaseRequest.PurchaseRequestId);
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestDenied, PRlist);
                    CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.Denied);
                    if (lineCount > 0 && (PrevStatus == PurchaseRequestStatusConstants.Open || PrevStatus == PurchaseRequestStatusConstants.Approved || PrevStatus == PurchaseRequestStatusConstants.Resubmit || PrevStatus == PurchaseRequestStatusConstants.AwaitApproval || PrevStatus == PurchaseRequestStatusConstants.Extracted))
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdateListPartsonOrder(purchaseRequest.PurchaseRequestId, "Minus", AttachmentTableConstant.PurchaseRequest));
                    }
                    break;
                case PurchaseReturnStatusEnum.ReturnToRequester:
                    purchaseRequest.Status = PurchaseRequestStatusConstants.Resubmit;
                    purchaseRequest.Return_Comments = Process_Comments;
                    purchaseRequest.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    purchaseRequest.Processed_Date = System.DateTime.UtcNow;
                    purchaseRequest.Update(userData.DatabaseKey);
                    List<long> PrId = new List<long> { purchaseRequest.PurchaseRequestId };
                    CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.Resubmit, Process_Comments);  /*V2-375*/
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestReturned, PrId);
                    break;
            }

            return purchaseRequest;

        }

        private void CreateEventLog(Int64 objId, string Status, string Comments = "")
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
            log.TableName = AttachmentTableConstant.PurchaseRequest;
            log.Create(userData.DatabaseKey);
        }
        // V2-1035
        // This is the CreatePO event
        // The event is to be tied to the PurchaseOrder
        // The table name = 'PurchaseOrder'
        //  ObjectId = PurchaseOrderId
        //  SourceId = PurchaseRequestId
        private void CreateEventLog(Int64 objId, string Status, Int64 SourceId)
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
            //log.TableName = AttachmentTableConstant.PurchaseRequest;
            log.Create(userData.DatabaseKey);
        }

        #endregion

        #region Find Part in
        internal Dictionary<long, string> UpadatePartIn(List<LineItemModel> list)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();

            PurchaseRequestLineItem purchaseRequestLineitem;
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        purchaseRequestLineitem = new PurchaseRequestLineItem();

                        Part part = new Part()
                        {
                            ClientId = this.userData.DatabaseKey.User.ClientId,
                            PartId = item.PartId,
                        };

                        part.RetriveByPartId(userData.DatabaseKey);

                        purchaseRequestLineitem.ClientId = this.userData.DatabaseKey.User.ClientId;
                        purchaseRequestLineitem.PurchaseRequestId = item.PurchaseRequestId;
                        purchaseRequestLineitem.ChargeToID = 0;
                        purchaseRequestLineitem.AccountId = part.AccountId;
                        purchaseRequestLineitem.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                        purchaseRequestLineitem.Description = part.Description;
                        purchaseRequestLineitem.PartId = item.PartId;
                        purchaseRequestLineitem.PartStoreroomId = part.PartStoreroomId;
                        purchaseRequestLineitem.UnitofMeasure = part.IssueUnit;
                        purchaseRequestLineitem.PurchaseUOM = part.IssueUnit;
                        purchaseRequestLineitem.UnitCost = part.AppliedCost;
                        purchaseRequestLineitem.OrderQuantity = item.OrderQuantity ?? 0;
                        purchaseRequestLineitem.AccountId = part.AccountId;

                        purchaseRequestLineitem.CreateWithValidation(this.userData.DatabaseKey);


                        if (purchaseRequestLineitem.ErrorMessages.Count == 0)
                        {
                            purchaseRequestLineitem.PRReOrderLineNumber(userData.DatabaseKey);

                        }
                        else
                        {
                            retValue.Add(item.PartId, purchaseRequestLineitem.ErrorMessages[0]);
                        }

                    }
                }

            }
            return retValue;

        }
        internal PurchaseRequestLineItem SavePartNotInInventory(PartNotInInventoryModel partNotInInventoryModel)
        {
            PurchaseRequestLineItem purchaseRequestlineitem = new PurchaseRequestLineItem();

            purchaseRequestlineitem.PurchaseRequestId = partNotInInventoryModel.PurchaseRequestId;
            purchaseRequestlineitem.Description = partNotInInventoryModel.Description;
            purchaseRequestlineitem.OrderQuantity = partNotInInventoryModel.OrderQuantity ?? 0;
            purchaseRequestlineitem.UnitofMeasure = partNotInInventoryModel.PurchaseUOM;
            purchaseRequestlineitem.PurchaseUOM = partNotInInventoryModel.PurchaseUOM;
            purchaseRequestlineitem.UnitCost = partNotInInventoryModel.UnitCost ?? 0;
            purchaseRequestlineitem.AccountId = partNotInInventoryModel.AccountId ?? 0;
            purchaseRequestlineitem.ChargeType = partNotInInventoryModel.ChargeType ?? string.Empty;
            purchaseRequestlineitem.ChargeToID = partNotInInventoryModel.ChargeToID;
            purchaseRequestlineitem.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            if (userData.Site.ShoppingCart)
            {
                purchaseRequestlineitem.RequiredDate = partNotInInventoryModel.RequiredDate;
            }
            purchaseRequestlineitem.CreateWithValidation(this.userData.DatabaseKey);

            if (purchaseRequestlineitem.ErrorMessages.Count == 0)
            {

                purchaseRequestlineitem.PRReOrderLineNumber(userData.DatabaseKey);
                if (purchaseRequestlineitem.ChargeType == AttachmentTableConstant.WorkOrder && (partNotInInventoryModel.Status == PurchaseRequestStatusConstants.Open || partNotInInventoryModel.Status == PurchaseRequestStatusConstants.Approved || partNotInInventoryModel.Status == PurchaseRequestStatusConstants.Resubmit || partNotInInventoryModel.Status == PurchaseRequestStatusConstants.AwaitApproval || partNotInInventoryModel.Status == PurchaseRequestStatusConstants.Extracted))
                {
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(purchaseRequestlineitem.ChargeToID, "Add"));
                }

            }

            return purchaseRequestlineitem;
        }
        #endregion

        #region Convert To Purchase Order
        public List<ConvertToPOModel> populateConvertToPurchaseOrder()
        {
            ConvertToPOModel convertToPOModel;
            List<ConvertToPOModel> ConvertToPOModelList = new List<ConvertToPOModel>();
            List<PurchaseRequest> purchaseRequestitems = new List<PurchaseRequest>();
            PurchaseRequest purchaseRequest = new PurchaseRequest()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                Status = PurchaseRequestStatusConstants.Approved
            };

            purchaseRequestitems = purchaseRequest.RetrieveByStatus(userData.DatabaseKey);
            if (purchaseRequestitems != null)
            {
                foreach (var p in purchaseRequestitems)
                {
                    convertToPOModel = new ConvertToPOModel();
                    convertToPOModel.PurchaseRequestId = p.PurchaseRequestId;
                    convertToPOModel.ClientLookupId = p.ClientLookupId;
                    convertToPOModel.CountLineItem = p.CountLineItem;
                    convertToPOModel.Reason = p.Reason;
                    convertToPOModel.Creator_PersonnelName = p.Creator_PersonnelName;
                    convertToPOModel.Approved_PersonnelName = p.Approved_PersonnelName;
                    convertToPOModel.VendorId = p.VendorId;
                    convertToPOModel.VendorIsExternal = p.VendorIsExternal;
                    convertToPOModel.Status = p.Status;
                    ConvertToPOModelList.Add(convertToPOModel);
                }
            }
            return ConvertToPOModelList;

        }

        public Tuple<List<ConvertToPOModel>, bool, bool> ConvertPRToPO(long[] PuchaseRequestIds)
        {
            string newClientlookupId = string.Empty;
            List<ConvertToPOModel> PRList = new List<ConvertToPOModel>();
            String message = string.Empty;
            ConvertToPOModel objPR;
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            var customerList = sWrapper.CustomIdDetails(AutoGenerateKey.PO_ANNUAL);
            string POPrefix = customerList != null ? customerList.Where(x => x.Key == AutoGenerateKey.PO_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : null;
            #region V2-929 Vendor Insurance Checking
            // 2023-07-27 - RKL - 
            // Only need to check if Site.VendorCompliance = True
            bool VendorInsuranceChecking = false;
            bool VendorAssetMgtChecking = false;
            foreach (var items in PuchaseRequestIds)
            {
                PurchaseRequest purchaseRequestDetailsRetrieve = new PurchaseRequest
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    PurchaseRequestId = items
                };
                purchaseRequestDetailsRetrieve.Retrieve(_dbKey);
                Vendor vendor = new Vendor
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    VendorId = purchaseRequestDetailsRetrieve.VendorId
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
                        break;
                    }
                }
                //V2-933
                if (vendor.AssetMgtOverride == true)
                {
                    VendorAssetMgtChecking = false;
                }
                else
                {
                    if (userData.Site.VendorCompliance == true && vendor.AssetMgtRequired == true && (Convert.ToDateTime(vendor.AssetMgtExpireDate).Date < DateTime.UtcNow.Date || vendor.AssetMgtExpireDate == DateTime.MinValue))
                    {
                        VendorAssetMgtChecking = true;
                        break;
                    }
                }
            }

            #endregion
            foreach (var item in PuchaseRequestIds)
            {
                #region V2-929
                if (VendorInsuranceChecking == true)
                {
                    break;
                }
                #endregion
                #region V2-933
                if (VendorAssetMgtChecking == true)
                {
                    break;
                }
                #endregion
                objPR = new ConvertToPOModel();
                if (true)//  UserData.ClientUIConfiguration.AutoGeneratedIdSettings.PurchaseOrder_AutoGenerateEnabled in VI true--for new Purchase order make a bool constant = true
                {
                    newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey
                            , AutoGenerateKey.PO_ANNUAL
                            , userData.DatabaseKey.User.DefaultSiteId
                           , POPrefix);//, "P");
                }
                PurchaseRequest pr = new PurchaseRequest();
                pr.ClientId = userData.DatabaseKey.Client.ClientId;
                pr.ClientLookupId = newClientlookupId;
                pr.SiteId = userData.DatabaseKey.Personnel.SiteId;
                pr.Status = PurchaseOrderStatusConstants.Open;
                pr.PurchaseRequestId = item;
                pr.PurchaseRequestConvertV2(userData.DatabaseKey);
                List<PurchaseRequest> prlist = new List<PurchaseRequest>();
                PurchaseRequest prq = new PurchaseRequest();
                prq.ClientId = userData.DatabaseKey.Client.ClientId;
                prq.SiteId = userData.DatabaseKey.Personnel.SiteId;
                prq.PurchaseOrderId = pr.PurchaseOrderId;
                prlist = prq.RetrieveForInformation(userData.DatabaseKey);
                Int64 PRID = prlist != null && prlist.Count > 0 ? prlist[0].PurchaseRequestId : 0;
                if (pr.PurchaseOrderId != 0)
                {
                    //message = "Purchase Request " + prlist[0].ClientLookupId + " converted to Purchase Order successfully";
                    objPR.Message = Convert.ToString(JsonReturnEnum.success);
                    objPR.ClientLookupId = prlist[0].ClientLookupId;
                    PRList.Add(objPR);
                }
                else
                {
                    objPR.Message = Convert.ToString(JsonReturnEnum.failed);
                    objPR.ClientLookupId = prlist[0].ClientLookupId;
                    PRList.Add(objPR);
                }
                // V2-1035 
                // Change to make tablename = 'PurchaseOrder'
                CreateEventLog(pr.PurchaseOrderId, PurchasingEvents.POCreate, PRID);
                if (!pr.AutoGenerated)
                {
                    List<long> prList = new List<long>();
                    prList.Add(pr.PurchaseRequestId);
                    ProcessAlert objAlert = new ProcessAlert(userData);
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestConverted, prList);
                }
            }
            //return PRList;
            return new Tuple<List<ConvertToPOModel>, bool, bool>(PRList, VendorInsuranceChecking, VendorAssetMgtChecking);
        }


        #endregion

        #region PR AutoGenerate
        internal ProcessLog PReqAutoGenerate()
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            ProcessLog processLog = new ProcessLog();
            CustomSequentialId custid = new CustomSequentialId();
            custid.KeyList = AutoGenerateKey.PR_ANNUAL;
            var custList = custid.RetrieveByClientIdandSiteIdandKey_V2(userData.DatabaseKey);
            string PRPrefix = custList != null ? custList.Where(x => x.Key == AutoGenerateKey.PR_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : null;
            purchaseRequest.ClientId = this.userData.DatabaseKey.Client.ClientId;
            purchaseRequest.SiteId = this.userData.DatabaseKey.Personnel.SiteId;
            purchaseRequest.PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseRequest.Prefix = PRPrefix;// AutoGenerateKey.PurchaseRequest_AutoGeneratePrefix;
            purchaseRequest.PurchaseRequestAutoGeneration(this.userData.DatabaseKey);
            processLog.ProcessLogId = purchaseRequest.ProcessLogId;
            processLog.ClientId = this.userData.DatabaseKey.Personnel.ClientId;
            processLog.Retrieve(this.userData.DatabaseKey);
            return processLog;
        }
        #endregion

        #region V2-375
        public PRExportModel_Coupa GetApprovalListPR(long purchaseRequestId)
        {
            List<PRLineExportModel_Coupa> PRLineCoupa = new List<PRLineExportModel_Coupa>();
            DateTime comp_date = new DateTime(2000, 1, 1);
            PurchaseRequest pr = new PurchaseRequest();
            pr.PurchaseRequestId = purchaseRequestId;//prid;
            pr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pr.ClientId = userData.DatabaseKey.Client.ClientId;

            pr.RetrieveForCoupaExport(userData.DatabaseKey, userData.Site.TimeZone);

            PurchaseRequestLineItem prline = new PurchaseRequestLineItem();
            prline.PurchaseRequestId = purchaseRequestId;// prid;
            prline.ClientId = userData.DatabaseKey.Client.ClientId;
            List<PurchaseRequestLineItem> prlinelist = new List<PurchaseRequestLineItem>();
            prlinelist = PurchaseRequestLineItem.PurchaseRequestLineItemRetrieveByPurchaseRequestId(userData.DatabaseKey, prline);
            // Create a new Coupa Export Model and fill
            PRExportModel_Coupa PRCoupa = new PRExportModel_Coupa();
            PRCoupa.BuyerNote = null;
            PRCoupa.Justification = pr.Reason ?? "";
            if (pr.RequiredDate == null || pr.RequiredDate < comp_date)
            {
                PRCoupa.NeedByDate = null;
            }
            else
            {
                DateTime req_date = pr.RequiredDate ?? DateTime.UtcNow;
                DateTimeOffset dto = req_date;
                PRCoupa.NeedByDate = dto;
            }
            PRCoupa.ShipToAttention = pr.Creator_PersonnelName;
            PRCoupa.ReceivingWarehouseId = null;
            PRCoupa.LineCount = prlinelist.Count;
            PRCoupa.TotalAmount = Math.Round(pr.TotalCost, 2);
            Coupa_PR_Header_CustomFields chf = new Coupa_PR_Header_CustomFields()
            {
                PRNumber = pr.ClientLookupId,
                PurchaseRequestId = pr.PurchaseRequestId.ToString(),
                ClientId = pr.ClientId.ToString(),
                SiteId = pr.SiteId.ToString()
            };
            PRCoupa.CustomFields = chf;
            PRCoupa.Currency = new Coupa_Currency() { Code = pr.Currency_Code };
            PRCoupa.RequestedBy = new Coupa_RequestedBy() { EmailAddress = pr.EXUserId };
            PRCoupa.ShipTo = new Coupa_ShipToAddress() { ShipToName = pr.Ship_to_Code };
            PRLineExportModel_Coupa prlinecoupa;

            foreach (var data in prlinelist)
            {
                prlinecoupa = new PRLineExportModel_Coupa();
                prlinecoupa.description = data.Description.Trim();
                DateTimeOffset req_date;
                if (data.RequiredDate == null || data.RequiredDate < comp_date)
                {
                    req_date = DateTime.UtcNow.AddDays(7);
                }
                else
                {
                    req_date = data.RequiredDate ?? DateTime.UtcNow.AddDays(7);
                }
                prlinecoupa.NeedByDate = req_date.ToString("s");
                prlinecoupa.linenumber = data.LineNumber != 0 ? data.LineNumber : 0;
                prlinecoupa.PartNumber = data.PartClientLookupId;
                prlinecoupa.AuxPartNumber = null;
                prlinecoupa.OrderQuantity = data.OrderQuantity;
                prlinecoupa.TotalCost = Math.Round(data.TotalCost, 2);
                prlinecoupa.SourceType = pr.Source_Type;
                prlinecoupa.LineType = pr.Line_Type;
                prlinecoupa.UnitCost = data.UnitCost != 0 ? data.UnitCost : 0;
                prlinecoupa.CustomFields = new Coupa_PR_Line_CustomFields() { NonTaxable = null, PurchaseRequestLineItemId = data.PurchaseRequestLineItemId.ToString() };
                Coupa_Account acct = new Coupa_Account(data.Account_ClientLookupId);
                Coupa_Account_Type acct_type = new Coupa_Account_Type() { Account_Name = pr.Acct_Name };
                Coupa_Currency acct_curr = new Coupa_Currency() { Code = pr.Currency_Code };
                Coupa_UOM uom = new Coupa_UOM() { Code = data.UnitofMeasure };    // 2020-09-11
                prlinecoupa.UnitOfMeasure = uom;                                // 2020-09-11                                      
                acct_type.Account_Currency = acct_curr;
                acct.Account_Type = acct_type;
                prlinecoupa.Account = acct;
                prlinecoupa.PaymentTerm = new Coupa_Payment_Terms() { TermsCode = pr.Terms_Desc };
                prlinecoupa.Commodity = new Coupa_Commodity() { Name = pr.Commodity_Code };
                prlinecoupa.Currency = new Coupa_Currency() { Code = pr.Currency_Code };
                prlinecoupa.Vendor = new Coupa_Vendor() { VendorNumber = pr.ExVendorId.ToString() };
                PRLineCoupa.Add(prlinecoupa);
            }
            PRCoupa.requisitionlines = PRLineCoupa;

            return PRCoupa;

        }

        public List<string> UpdatePrStatus(long prId, string status, long exRequestId, string PurchasingEvents)
        {
            PurchaseRequest pr = new PurchaseRequest();
            pr.PurchaseRequestId = prId;
            pr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            pr.Status = status;
            pr.ExRequestId = exRequestId;
            pr.Update(userData.DatabaseKey);
            CreateEventLog(prId, PurchasingEvents);
            return pr.ErrorMessages;
        }


        #endregion

        #region PunchOut
        public PunchOutSetUpRequestData GetSetUpRequestData(long PurchaseRequestId, Models.VendorsModel objVen, Personnel personnel, Site site)
        {
            string appUrl = GetHostedUrl();
            //Header section
            Models.PunchoutExport.PunchOutSetUpRequestData cxml = new Models.PunchoutExport.PunchOutSetUpRequestData();
            Models.PunchoutExport.Header hdr = new Models.PunchoutExport.Header();
            Models.PunchoutExport.From frm = new Models.PunchoutExport.From();
            Models.PunchoutExport.To to = new Models.PunchoutExport.To();
            Models.PunchoutExport.Credential cred = new Models.PunchoutExport.Credential();
            Models.PunchoutExport.Sender sender = new Models.PunchoutExport.Sender();

            //Request Section
            Models.PunchoutExport.PunchOutSetupRequest POSR = new Models.PunchoutExport.PunchOutSetupRequest();
            Models.PunchoutExport.Contact con = new Models.PunchoutExport.Contact();
            Models.PunchoutExport.Name name = new Models.PunchoutExport.Name();
            Models.PunchoutExport.Extrinsic extrin = new Models.PunchoutExport.Extrinsic();
            Models.PunchoutExport.BrowserFormPost bfp = new Models.PunchoutExport.BrowserFormPost();
            Models.PunchoutExport.SupplierSetup suppSetup = new Models.PunchoutExport.SupplierSetup();
            //Initialize

            cxml.Header = new Models.PunchoutExport.Header();
            cxml.Header.From = new Models.PunchoutExport.From();
            cxml.Header.From.Credential = new Models.PunchoutExport.Credential();
            cxml.Header.To = new Models.PunchoutExport.To();
            cxml.Header.To.Credential = new Models.PunchoutExport.Credential();
            cxml.Header.Sender = new Models.PunchoutExport.Sender();
            cxml.Header.Sender.Credential = new Models.PunchoutExport.Credential();
            cxml.Request = new Models.PunchoutExport.Request();
            cxml.Request.PunchOutSetupRequest = new Models.PunchoutExport.PunchOutSetupRequest();
            cxml.Request.PunchOutSetupRequest.Extrinsic = new List<Extrinsic>();
            cxml.Request.PunchOutSetupRequest.Contact = new Models.PunchoutExport.Contact();
            cxml.Request.PunchOutSetupRequest.Contact.Name = new Models.PunchoutExport.Name();
            cxml.Request.PunchOutSetupRequest.BrowserFormPost = new Models.PunchoutExport.BrowserFormPost();
            cxml.Request.PunchOutSetupRequest.SupplierSetup = new Models.PunchoutExport.SupplierSetup();

            //--Bind data
            cxml.PayloadID = DateTime.UtcNow.Ticks.ToString() + PurchaseRequestId.ToString() + "@somax.com";
            cxml.Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            cxml.Header.From.Credential.Domain = objVen.SenderDomain;// "DUNS";
            cxml.Header.From.Credential.Identity = objVen.SenderIdentity; //"810939686";

            cxml.Header.To.Credential.Domain = objVen.VendorDomain; //"DUNS";
            cxml.Header.To.Credential.Identity = objVen.VendorIdentity;// "159148746";

            cxml.Header.Sender.Credential.Domain = objVen.SenderDomain; // "DUNS";
            cxml.Header.Sender.Credential.Identity = objVen.SenderIdentity; //"810939686";
            cxml.Header.Sender.Credential.SharedSecret = objVen.SharedSecret; //"gra1nger";
            cxml.Header.Sender.UserAgent = "SOMAXG4";//"Oracle Fusion Self Service Procurement";

            cxml.Request.PunchOutSetupRequest.Operation = "create";
            cxml.Request.PunchOutSetupRequest.BuyerCookie = userData.LoginAuditing.SessionId.ToString() + PurchaseRequestId.ToString();// ClientId.ToString() + SiteId.ToString() + PurchaseRequestId.ToString();//"1605072485189300000477103356";
            cxml.Request.PunchOutSetupRequest.Contact.Name.Text = personnel.NameFirst + ',' + personnel.NameLast; //@"GARCIA, ALVARO";
            cxml.Request.PunchOutSetupRequest.Contact.Name.Lang = "en";
            cxml.Request.PunchOutSetupRequest.Contact.Email = personnel.Email;// "alvaro.renato.garcia@oracle.com";

            List<Extrinsic> Elist = new List<Extrinsic>();
            Extrinsic extrinsic1 = new Extrinsic();
            extrinsic1.Name = "User";
            extrinsic1.Text = userData.DatabaseKey.User.UserName;// "Ora_alvaro.garcia";
            Elist.Add(extrinsic1);
            Extrinsic extrinsic2 = new Extrinsic();
            extrinsic2.Name = "UniqueName";
            extrinsic2.Text = personnel.NameFirst + ',' + personnel.NameLast; //"GARCIA, ALVARO";
            Elist.Add(extrinsic2);
            Extrinsic extrinsic3 = new Extrinsic();
            extrinsic3.Name = "UserEmail";
            extrinsic3.Text = personnel.Email;// "alvaro.renato.garcia@oracle.com";
            Elist.Add(extrinsic3);
            Extrinsic extrinsic4 = new Extrinsic();
            extrinsic4.Name = "BusinessUnit";
            extrinsic4.Text = site.Name;// "USBBU";
            Elist.Add(extrinsic4);
            cxml.Request.PunchOutSetupRequest.Extrinsic = Elist;
            cxml.Request.PunchOutSetupRequest.BrowserFormPost.URL = appUrl + SomaxAppConstants.PunchOutSetUpReturn;
            cxml.Request.PunchOutSetupRequest.SupplierSetup.URL = objVen.PunchoutURL;// "https://ca.gcom.grainger.com/punchout/cxml";
            return cxml;
        }
        public PunchoutAPIResponse postXMLData(string destinationUrl, PunchOutSetUpRequestData requestXml)
        {
            PunchoutAPIResponse ro = new PunchoutAPIResponse();
            try
            {
                string ResponseURL = string.Empty;
                string ResponseText = string.Empty;
                int statusCode = 0;

                var requestString = XmlHelper.XmlSerializeFromObject<PunchOutSetUpRequestData>(requestXml);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestString);
                request.ContentType = "application/xml; encoding='utf-8'";
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
                        var responseObject = XmlHelper.XmlDeserializeFromString<PunchOutSetUpResponse>(responseStr);
                        if (responseObject != null && responseObject.Response != null)
                        {
                            if (responseObject.Response.Status.Code == 200)
                            {
                                ResponseURL = responseObject.Response.PunchOutSetupResponse.StartPage.URL;
                                statusCode = responseObject.Response.Status.Code;
                            }
                            else
                            {
                                var errorResponseObject = XmlHelper.XmlDeserializeFromString<Client.Models.PunchOutSetUpErrorResponse.PunchOutSetUpErrorResponse>(responseStr);
                                if (errorResponseObject != null && !string.IsNullOrEmpty(errorResponseObject.Response.Status.Code) && !string.IsNullOrEmpty(errorResponseObject.Response.Status._Text))
                                {
                                    //ro.ResponseMessage = responseObject.Response.Status.__Text;
                                    statusCode = responseObject.Response.Status.Code;
                                }
                                ResponseURL = "";
                            }
                            ro.ResponseCode = statusCode;
                        }
                        ro.ResponseURL = ResponseURL;

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
                            case 500:
                                ResponseText = "An un-handled error occurred while processing this request";
                                break;
                            default:
                                ResponseText = "";
                                break;
                        }
                        ro.ResponseMessage = ResponseText;
                    }
                    else
                    {
                        ro.ResponseMessage = "An error occurred while processing your request.";
                    }
                }
                else
                {
                    ro.ResponseMessage = "An error occurred while processing your request.";
                }
            }
            catch (Exception ex)
            {
                string error_msg = ex.Message;
                ro.ResponseMessage = "An error occurred while processing your request. Please check Vendor Punch Out SetUp details.";
            }
            return ro;
        }

        public List<ShoppingCartImportDataModel> ImportShoppingCart_ToDataModel(Models.PunchoutImport.PunchOutOrderMessageResponse cxml)
        {
            Models.PunchoutImport.PunchOutOrderMessageResponse DataModel = cxml;

            List<ShoppingCartImportDataModel> shoppingList = new List<ShoppingCartImportDataModel>();

            int i = 1;
            foreach (var z in DataModel.Message.PunchOutOrderMessage.ItemIn)
            {
                ShoppingCartImportDataModel shoppingCart = new ShoppingCartImportDataModel();
                #region V2-1119 GetVendorIdFromPurchaseRequest
                long PRid = Convert.ToInt64(cxml.Message.PunchOutOrderMessage.BuyerCookie.Substring(36));
                PurchaseRequest purchaseRequest = GetVendorIdFromPurchaseRequest(PRid);
                #endregion
                shoppingCart.SupplierPartId = z.ItemID.SupplierPartID ?? "0";
                shoppingCart.SupplierPartAuxiliaryId = z.ItemID.SupplierPartAuxiliaryID ?? "";
                shoppingCart.Description = z.ItemDetail.Description.Text ?? "";
                shoppingCart.OrderQuantity = Convert.ToDecimal(z.Quantity.ToString() ?? "0.00");
                shoppingCart.UnitofMeasure = z.ItemDetail.UnitOfMeasure.ToString() ?? "";
                shoppingCart.UnitCost = Convert.ToDecimal(z.ItemDetail.UnitPrice.Money.Text.ToString() ?? "0.00");
                shoppingCart.PartId = 0;//---assign from PartLookup list after grid bind35
                shoppingCart.ChargeType = "";//---assign from Dropdown list after grid bind
                shoppingCart.ChargeToID = 0;//--From lookup, depending on ChargeType
                shoppingCart.AccountId = 0;//--assign from Account Lookup list after grid bind
                shoppingCart.PunchoutLineItemId = i++;
                shoppingCart.Classification = (z.ItemDetail.Classification.Domain.ToString() ?? "") + "-" + (z.ItemDetail.Classification.Text.ToString() ?? "");
                shoppingCart.ManufacturerName = z.ItemDetail.ManufacturerName.ToString() ?? "";
                shoppingCart.ManufacturerPartID = z.ItemDetail.ManufacturerPartID.ToString() ?? "";

                Part_Vendor_Xref part_Vendor_Xref = new Part_Vendor_Xref();
                part_Vendor_Xref.ClientId = userData.DatabaseKey.Client.ClientId;
                part_Vendor_Xref.SiteId = userData.Site.SiteId;
                part_Vendor_Xref.VendorId = purchaseRequest.VendorId;
                part_Vendor_Xref.Manufacturer = shoppingCart.ManufacturerName;
                part_Vendor_Xref.ManufacturerId = shoppingCart.ManufacturerPartID;
                // Check if the part / vendor cross - reference exists
                List<Part_Vendor_Xref> Part_Vendor_Xref = part_Vendor_Xref.RetrieveForShoppingCartDataImport(userData.DatabaseKey);
                if (Part_Vendor_Xref != null && Part_Vendor_Xref.Count > 0)
                {
                    shoppingCart.OrderUnit = Part_Vendor_Xref.FirstOrDefault().OrderUnit.ToString() ?? "";
                    shoppingCart.PartId = Part_Vendor_Xref.FirstOrDefault().PartId;
                    shoppingCart.Part_ClientLookupId = Part_Vendor_Xref.FirstOrDefault().Part_ClientLookupId.ToString() ?? "";
                }
                else
                {
                    part_Vendor_Xref.OrderUnit = GetUnitOfMeasureByVendorUnit(shoppingCart.UnitofMeasure);
                }
                shoppingList.Add(shoppingCart);//--contains all Line Items---;
            }

            return shoppingList;

        }
        public string GetUnitOfMeasureByVendorUnit(string vendorUnit)
        {
            string UOM = String.Empty;
            var AllLookUpLists = GetAllLookUpList();
            if (AllLookUpLists != null)
            {
                var unitList = AllLookUpLists.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                var unit_Of_MeasureList = unitList.Where(x => x.ListValue.ToUpper().Contains(vendorUnit.ToUpper())).FirstOrDefault();
                UOM = unit_Of_MeasureList == null ? string.Empty : unit_Of_MeasureList.ListValue.ToString();
            }
            return UOM;
        }
        internal Dictionary<long, string> InsertPartShoppingCartPurchaseRequest(long PurchaseRequestID, string Status, List<PRShoppingcartModel> list)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            PurchaseRequestLineItem purchaseRequestLineitem;
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        purchaseRequestLineitem = new PurchaseRequestLineItem();
                        purchaseRequestLineitem.ClientId = _dbKey.Client.ClientId;
                        purchaseRequestLineitem.PurchaseRequestId = PurchaseRequestID;
                        purchaseRequestLineitem.AccountId = item.AccountId;
                        purchaseRequestLineitem.Description = item.Description ?? "";
                        purchaseRequestLineitem.LineNumber = Convert.ToInt32(item.PunchoutLineItemId);
                        purchaseRequestLineitem.PartId = item.PartId;
                        purchaseRequestLineitem.OrderQuantity = item.OrderQuantity;
                        purchaseRequestLineitem.UnitCost = item.UnitCost;
                        purchaseRequestLineitem.UnitofMeasure = item.UnitofMeasure;
                        purchaseRequestLineitem.ChargeToID = item.ChargeToID;
                        purchaseRequestLineitem.ChargeType = item.ChargeType;
                        purchaseRequestLineitem.Creator_PersonnelId = _dbKey.Personnel.PersonnelId;
                        purchaseRequestLineitem.SupplierPartId = item.SupplierPartId ?? "";
                        purchaseRequestLineitem.SupplierPartAuxiliaryId = item.SupplierPartAuxiliaryId ?? "";
                        purchaseRequestLineitem.ManufacturerPartId = item.ManufacturerPartID ?? "";
                        purchaseRequestLineitem.Manufacturer = item.ManufacturerName ?? "";
                        purchaseRequestLineitem.RequiredDate = item.RequiredDate;
                        purchaseRequestLineitem.UNSPSC = item.UNSPSC;
                        purchaseRequestLineitem.CreateFromOPunchOutShoppingCart(_dbKey);
                        purchaseRequestLineitem.PRReOrderLineNumber(_dbKey);
                        #region V2-1119 Add or Update Part/Vendor Cross-Reference when Processing Shopping Cart Item
                        if (purchaseRequestLineitem.PartId > 0)
                        {
                            // Retrieve the vendor ID from the purchase request
                            PurchaseRequest purchaseRequest = GetVendorIdFromPurchaseRequest(PurchaseRequestID);

                            // create and update the part/vendor cross-reference
                            Part_Vendor_Xref objCreatePart_Vendor_Xref = new Part_Vendor_Xref();
                            objCreatePart_Vendor_Xref.ClientId = userData.DatabaseKey.Client.ClientId;
                            objCreatePart_Vendor_Xref.SiteId = userData.Site.SiteId;
                            objCreatePart_Vendor_Xref.PartId = item.PartId;
                            objCreatePart_Vendor_Xref.VendorId = purchaseRequest.VendorId;
                            objCreatePart_Vendor_Xref.CatalogNumber = item.SupplierPartId;
                            objCreatePart_Vendor_Xref.IssueOrder = 1;
                            objCreatePart_Vendor_Xref.Manufacturer = item.ManufacturerName;
                            objCreatePart_Vendor_Xref.ManufacturerId = item.ManufacturerPartID;
                            objCreatePart_Vendor_Xref.OrderQuantity = 0;
                            objCreatePart_Vendor_Xref.OrderUnit = item.UnitofMeasure;
                            objCreatePart_Vendor_Xref.Price = item.UnitCost;
                            objCreatePart_Vendor_Xref.UOMConvRequired = false;
                            objCreatePart_Vendor_Xref.Punchout = true;
                            objCreatePart_Vendor_Xref.Part_Vendor_Xref_Create_Update_Punchout(_dbKey);
                        }
                        #endregion
                        if (purchaseRequestLineitem.ChargeType == AttachmentTableConstant.WorkOrder && (Status == PurchaseRequestStatusConstants.Open || Status == PurchaseRequestStatusConstants.Approved || Status == PurchaseRequestStatusConstants.Resubmit || Status == PurchaseRequestStatusConstants.AwaitApproval || Status == PurchaseRequestStatusConstants.Extracted))
                        {
                            CommonWrapper commonWrapper = new CommonWrapper(userData);
                            Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(purchaseRequestLineitem.ChargeToID, "Add"));

                        }
                    }
                }
                CreateEventLog(PurchaseRequestID, PurchasingEvents.PunchOut);
            }
            return retValue;

        }
        private PurchaseRequest GetVendorIdFromPurchaseRequest(long PRid)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PRid;
            purchaseRequest.Retrieve(userData.DatabaseKey);
            return purchaseRequest;
        }
        #endregion

        #region V2-643 PR AutoGenerate

        public List<Models.AutoPRGenerationSearchModel> GetAutoPRGenerateChunkList(int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", string VendorIDList = "",
            string PartClientLookupId = "",
            string Description = "",
            string UnitofMeasure = "",
            string VendorClientLookupId = "",
            string VendorName = "",
            string QtyToOrder = "",
            string LastPurchaseCost = "",
            string StoreroomId = "")
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            Models.AutoPRGenerationSearchModel pModel;
            List<Models.AutoPRGenerationSearchModel> AutoPRGenerationSearchList = new List<Models.AutoPRGenerationSearchModel>();
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            purchaseRequest.orderbyColumn = orderbycol;
            purchaseRequest.orderBy = orderDir;
            purchaseRequest.offset1 = Convert.ToString(skip);
            purchaseRequest.nextrow = Convert.ToString(length);
            purchaseRequest.VendorIDList = VendorIDList;
            purchaseRequest.PartClientLookupId = PartClientLookupId;
            purchaseRequest.Description = Description;
            purchaseRequest.UnitofMeasure = UnitofMeasure;
            purchaseRequest.VendorClientLookupId = VendorClientLookupId;
            purchaseRequest.VendorName = VendorName;
            purchaseRequest.StoreroomId = string.IsNullOrEmpty(StoreroomId) ? 0:Convert.ToInt64(StoreroomId);
            if (!string.IsNullOrEmpty(QtyToOrder))
            {
                purchaseRequest.QtyToOrder = Convert.ToDecimal(QtyToOrder);
            }
            else
            {
                purchaseRequest.QtyToOrder = null;
            }
            if (!string.IsNullOrEmpty(LastPurchaseCost))
            {
                purchaseRequest.LastPurchaseCost = Convert.ToDecimal(LastPurchaseCost);
            }
            else
            {
                purchaseRequest.LastPurchaseCost = null;
            }
            List<PurchaseRequest> purchaseRequestList;
            if (userData.DatabaseKey.Client.UseMultiStoreroom)
            {
                purchaseRequestList = purchaseRequest.RetrieveChunkSearchMultiStoreroomAutoPRGeneration(this.userData.DatabaseKey);
            }
            else
            {
                purchaseRequestList = purchaseRequest.RetrieveChunkSearchAutoPRGeneration(this.userData.DatabaseKey);
            }
               
            if (purchaseRequestList != null)
            {
                foreach (var dbObj in purchaseRequestList)
                {
                    pModel = new Models.AutoPRGenerationSearchModel();
                    pModel.PartId = dbObj.PartID;
                    pModel.PartClientLookupId = dbObj.PartClientLookupId;
                    pModel.Description = dbObj.Description;
                    pModel.QtyToOrder = dbObj.QtyToOrder;
                    pModel.UnitofMeasure = dbObj.UnitofMeasure;
                    pModel.LastPurchaseCost = dbObj.LastPurchaseCost;
                    pModel.VendorClientLookupId = dbObj.VendorClientLookupId;
                    pModel.VendorName = dbObj.VendorName;
                    pModel.TotalCount = dbObj.TotalCount;
                    AutoPRGenerationSearchList.Add(pModel);
                }
            }
            return AutoPRGenerationSearchList;
        }


        #endregion

        #region Create PR AutoGenerate  V2-643
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);

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
        internal ProcessLog PReqAutoGenerate_V2(List<Models.AutoPRGeneration.PartListTableModel> partTableLists)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            ProcessLog processLog = new ProcessLog();
            CustomSequentialId custid = new CustomSequentialId();
            custid.KeyList = AutoGenerateKey.PR_ANNUAL;
            var custList = custid.RetrieveByClientIdandSiteIdandKey_V2(userData.DatabaseKey);
            string PRPrefix = custList != null ? custList.Where(x => x.Key == AutoGenerateKey.PR_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : null;
            purchaseRequest.ClientId = this.userData.DatabaseKey.Client.ClientId;
            purchaseRequest.SiteId = this.userData.DatabaseKey.Personnel.SiteId;
            purchaseRequest.PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseRequest.Prefix = PRPrefix;

            purchaseRequest.PartList = ToDataTable<Models.AutoPRGeneration.PartListTableModel>(partTableLists);// AutoGenerateKey.PurchaseRequest_AutoGeneratePrefix;
            purchaseRequest.PurchaseRequestAutoGeneration_V2(this.userData.DatabaseKey);
            processLog.ProcessLogId = purchaseRequest.ProcessLogId;
            processLog.ClientId = this.userData.DatabaseKey.Personnel.ClientId;
            processLog.Retrieve(this.userData.DatabaseKey);
            return processLog;
        }
        #endregion
        #region UI configuration for Purchase Request module V2-653
        internal ViewPurchaseRequestModelDynamic GetPurchaseRequestDetailsByIdDynamic(long PurchaseRequestId)
        {
            ViewPurchaseRequestModelDynamic viewPurchaseRequestModelDynamic = new ViewPurchaseRequestModelDynamic();
            PurchaseRequest prDetails = RetrievePRByPurchaseRequestId(PurchaseRequestId);
            PRHeaderUDF pRHeaderUDF = RetrievePRUDFByPurchaseRequestId(PurchaseRequestId);
            viewPurchaseRequestModelDynamic = MapPurchaseRequestDataForView(viewPurchaseRequestModelDynamic, prDetails);
            viewPurchaseRequestModelDynamic = MapPRHeaderUDFDataForView(viewPurchaseRequestModelDynamic, pRHeaderUDF);
            return viewPurchaseRequestModelDynamic;
        }
        public ViewPurchaseRequestModelDynamic MapPurchaseRequestDataForView(ViewPurchaseRequestModelDynamic pModel, PurchaseRequest dbObj)
        {
            pModel.ClientLookupId = dbObj.ClientLookupId;
            pModel.PurchaseRequestId = dbObj.PurchaseRequestId;
            pModel.Status = dbObj.Status;
            pModel.LocalizedStatus = UtilityFunction.GetMessageFromResource(dbObj.Status, LocalizeResourceSetConstants.StatusDetails);
            pModel.Reason = dbObj.Reason;
            pModel.VendorClientLookupId = dbObj.VendorClientLookupId;
            pModel.VendorName = dbObj.VendorName;
            pModel.CountLineItem = dbObj.CountLineItem;
            pModel.TotalCost = dbObj.TotalCost;
            pModel.Creator_PersonnelName = dbObj.Creator_PersonnelName;
            pModel.CreateDate = dbObj.CreateDate;
            pModel.Approved_PersonnelName = dbObj.Approved_PersonnelName;
            if (dbObj.Approved_Date != null && dbObj.Approved_Date != default(DateTime))
            {
                pModel.Approved_Date = dbObj.Approved_Date;
            }
            else
            {
                pModel.Approved_Date = null;
            }
            pModel.Processed_PersonnelName = dbObj.Processed_PersonnelName;
            if (dbObj.Processed_Date != null && dbObj.Processed_Date != default(DateTime))
            {
                pModel.Processed_Date = dbObj.Processed_Date;
            }
            else
            {
                pModel.Processed_Date = null;
            }
            pModel.AutoGenerated = dbObj.AutoGenerated;
            pModel.PurchaseOrderClientLookupId = dbObj.PurchaseOrderClientLookupId;
            if (dbObj.Status == PurchaseRequestStatusConstants.Denied)
            {
                pModel.Process_Comments = dbObj.Process_Comments;

            }
            else if (dbObj.Status == PurchaseRequestStatusConstants.AwaitApproval)
            {
                pModel.Process_Comments = dbObj.Process_Comments;

            }
            else if (dbObj.Status == PurchaseRequestStatusConstants.Resubmit)
            {
                pModel.Return_Comments = dbObj.Return_Comments;
            }
            pModel.ApprovedBy_PersonnelId = dbObj.ApprovedBy_PersonnelId;
            pModel.CreatedBy_PersonnelId = dbObj.CreatedBy_PersonnelId;
            pModel.Process_Comments = dbObj.Process_Comments;
            pModel.ProcessBy_PersonnelId = dbObj.ProcessBy_PersonnelId;
            pModel.VendorId = dbObj.VendorId;
            pModel.PurchaseOrderId = dbObj.PurchaseOrderId;
            pModel.Return_Comments = dbObj.Return_Comments;
            pModel.ExtractLogId = dbObj.ExtractLogId;
            pModel.StoreroomId = dbObj.StoreroomId;
            pModel.StoreroomName = dbObj.StoreroomName;
            pModel.BuyerReview = dbObj.BuyerReview;
            return pModel;
        }
        private ViewPurchaseRequestModelDynamic MapPRHeaderUDFDataForView(ViewPurchaseRequestModelDynamic viewPurchaseRequestModelDynamic, PRHeaderUDF prHeaderUDF)
        {
            if (prHeaderUDF != null)
            {
                viewPurchaseRequestModelDynamic.PRHeaderUDFId = prHeaderUDF.PRHeaderUDFId;

                viewPurchaseRequestModelDynamic.Text1 = prHeaderUDF.Text1;
                viewPurchaseRequestModelDynamic.Text2 = prHeaderUDF.Text2;
                viewPurchaseRequestModelDynamic.Text3 = prHeaderUDF.Text3;
                viewPurchaseRequestModelDynamic.Text4 = prHeaderUDF.Text4;

                if (prHeaderUDF.Date1 != null && prHeaderUDF.Date1 == DateTime.MinValue)
                {
                    viewPurchaseRequestModelDynamic.Date1 = null;
                }
                else
                {
                    viewPurchaseRequestModelDynamic.Date1 = prHeaderUDF.Date1;
                }
                if (prHeaderUDF.Date2 != null && prHeaderUDF.Date2 == DateTime.MinValue)
                {
                    viewPurchaseRequestModelDynamic.Date2 = null;
                }
                else
                {
                    viewPurchaseRequestModelDynamic.Date2 = prHeaderUDF.Date2;
                }
                if (prHeaderUDF.Date3 != null && prHeaderUDF.Date3 == DateTime.MinValue)
                {
                    viewPurchaseRequestModelDynamic.Date3 = null;
                }
                else
                {
                    viewPurchaseRequestModelDynamic.Date3 = prHeaderUDF.Date3;
                }
                if (prHeaderUDF.Date4 != null && prHeaderUDF.Date4 == DateTime.MinValue)
                {
                    viewPurchaseRequestModelDynamic.Date4 = null;
                }
                else
                {
                    viewPurchaseRequestModelDynamic.Date4 = prHeaderUDF.Date4;
                }

                viewPurchaseRequestModelDynamic.Bit1 = prHeaderUDF.Bit1;
                viewPurchaseRequestModelDynamic.Bit2 = prHeaderUDF.Bit2;
                viewPurchaseRequestModelDynamic.Bit3 = prHeaderUDF.Bit3;
                viewPurchaseRequestModelDynamic.Bit4 = prHeaderUDF.Bit4;

                viewPurchaseRequestModelDynamic.Numeric1 = prHeaderUDF.Numeric1;
                viewPurchaseRequestModelDynamic.Numeric2 = prHeaderUDF.Numeric2;
                viewPurchaseRequestModelDynamic.Numeric3 = prHeaderUDF.Numeric3;
                viewPurchaseRequestModelDynamic.Numeric4 = prHeaderUDF.Numeric4;

                viewPurchaseRequestModelDynamic.Select1 = prHeaderUDF.Select1;
                viewPurchaseRequestModelDynamic.Select2 = prHeaderUDF.Select2;
                viewPurchaseRequestModelDynamic.Select3 = prHeaderUDF.Select3;
                viewPurchaseRequestModelDynamic.Select4 = prHeaderUDF.Select4;

                viewPurchaseRequestModelDynamic.Select1Name = prHeaderUDF.Select1Name;
                viewPurchaseRequestModelDynamic.Select2Name = prHeaderUDF.Select2Name;
                viewPurchaseRequestModelDynamic.Select3Name = prHeaderUDF.Select3Name;
                viewPurchaseRequestModelDynamic.Select4Name = prHeaderUDF.Select4Name;
            }
            return viewPurchaseRequestModelDynamic;
        }

        #region Add Purchase Request dynamic
        internal Tuple<PurchaseRequest, bool, bool> AddPurchaseRequestDynamic(PurchaseRequestVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CustomSequentialId custid = new CustomSequentialId();
            custid.KeyList = AutoGenerateKey.PR_ANNUAL;
            var custList = custid.RetrieveByClientIdandSiteIdandKey_V2(userData.DatabaseKey);
            string PRPrefix = custList != null ? custList.Where(x => x.Key == AutoGenerateKey.PR_ANNUAL).Select(x => x.Prefix).FirstOrDefault() : "";

            purchaseRequest.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            purchaseRequest.ClientId = this.userData.DatabaseKey.Client.ClientId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequest, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.AddPurchaseRequest);
                getpropertyInfo = objVM.AddPurchaseRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.AddPurchaseRequest);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = purchaseRequest.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(purchaseRequest, val);
            }
            #region V2-929 Vendor Insurance Checking
            bool VendorInsuranceChecking = false;
            bool VendorAssetMgtChecking = false;
            Vendor vendor = new Vendor
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = purchaseRequest.VendorId
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
                return new Tuple<PurchaseRequest, bool, bool>(purchaseRequest, VendorInsuranceChecking, VendorAssetMgtChecking);
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
                return new Tuple<PurchaseRequest, bool, bool>(purchaseRequest, VendorInsuranceChecking, VendorAssetMgtChecking);
            }
            #endregion
            purchaseRequest.CreatedBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseRequest.Status = PurchaseRequestStatusConstants.Open;

            purchaseRequest.CreateByPKForeignKeys(this.userData.DatabaseKey, true, AutoGenerateKey.PR_ANNUAL, PRPrefix);
            if (purchaseRequest.ErrorMessages != null && purchaseRequest.ErrorMessages.Count == 0)
            {
                CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.PROpen);
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPurchaseRequestUDFDynamic(objVM.AddPurchaseRequest, purchaseRequest.PurchaseRequestId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        purchaseRequest.ErrorMessages.AddRange(errors);
                    }
                }
            }
            return new Tuple<PurchaseRequest, bool, bool>(purchaseRequest, VendorInsuranceChecking, VendorAssetMgtChecking);
        }
        public List<string> AddPurchaseRequestUDFDynamic(Models.PurchaseRequest.UIConfiguration.AddPurchaseRequestModelDynamic prRequest, long purchaseRequestId,
   List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PRHeaderUDF prHeaderUDF = new PRHeaderUDF();
            prHeaderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            prHeaderUDF.PurchaseRequestId = purchaseRequestId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, prRequest);
                getpropertyInfo = prRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prRequest);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prHeaderUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prHeaderUDF, val);
            }
            prHeaderUDF.Create(_dbKey);
            return prHeaderUDF.ErrorMessages;
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
        #region Edit Purchase Request Dynamic
        internal EditPurchaseRequestModelDynamic GetPurchaseRequestByIdDynamic(long PurchaseRequestId)
        {
            EditPurchaseRequestModelDynamic editPurchaseRequestModelDynamic = new EditPurchaseRequestModelDynamic();
            PurchaseRequest prDetails = RetrievePRByPurchaseRequestId(PurchaseRequestId);
            PRHeaderUDF pRHeaderUDF = RetrievePRUDFByPurchaseRequestId(PurchaseRequestId);
            editPurchaseRequestModelDynamic = MapPurchaseRequestDataForEdit(editPurchaseRequestModelDynamic, prDetails);
            editPurchaseRequestModelDynamic = MapPRHeaderUDFDataForEdit(editPurchaseRequestModelDynamic, pRHeaderUDF);
            return editPurchaseRequestModelDynamic;
        }
        public PurchaseRequest RetrievePRByPurchaseRequestId(long purchaseRequestId)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestId = purchaseRequestId
            };
            purchaseRequest.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            return purchaseRequest;
        }
        public PRHeaderUDF RetrievePRUDFByPurchaseRequestId(long purchaseRequestId)
        {
            PRHeaderUDF pRHeaderUDF = new PRHeaderUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseRequestId = purchaseRequestId
            };

            pRHeaderUDF = pRHeaderUDF.RetrieveByPurchaseRequestId(this.userData.DatabaseKey);
            return pRHeaderUDF;
        }
        public EditPurchaseRequestModelDynamic MapPurchaseRequestDataForEdit(EditPurchaseRequestModelDynamic pModel, PurchaseRequest dbObj)
        {
            pModel.ClientLookupId = dbObj.ClientLookupId;
            pModel.PurchaseRequestId = dbObj.PurchaseRequestId;
            pModel.Status = dbObj.Status;
            pModel.LocalizedStatus = UtilityFunction.GetMessageFromResource(dbObj.Status, LocalizeResourceSetConstants.StatusDetails);
            pModel.Reason = dbObj.Reason;
            pModel.VendorClientLookupId = dbObj.VendorClientLookupId;
            pModel.VendorName = dbObj.VendorName;
            pModel.CountLineItem = dbObj.CountLineItem;
            pModel.TotalCost = dbObj.TotalCost;
            pModel.Creator_PersonnelName = dbObj.Creator_PersonnelName;
            pModel.CreateDate = dbObj.CreateDate;
            pModel.Approved_PersonnelName = dbObj.Approved_PersonnelName;
            pModel.Processed_PersonnelName = dbObj.Processed_PersonnelName;
            if (dbObj.Approved_Date != null && dbObj.Approved_Date != default(DateTime))
            {
                pModel.Approved_Date = dbObj.Approved_Date;
            }
            else
            {
                pModel.Approved_Date = null;
            }
            if (dbObj.Processed_Date != null && dbObj.Processed_Date != default(DateTime))
            {
                pModel.Processed_Date = dbObj.Processed_Date;
            }
            else
            {
                pModel.Processed_Date = null;
            }
            pModel.AutoGenerated = dbObj.AutoGenerated;
            pModel.PurchaseOrderClientLookupId = dbObj.PurchaseOrderClientLookupId;
            if (dbObj.Status == PurchaseRequestStatusConstants.Denied)
            {
                pModel.Process_Comments = dbObj.Process_Comments;

            }
            else if (dbObj.Status == PurchaseRequestStatusConstants.AwaitApproval)
            {
                pModel.Process_Comments = dbObj.Process_Comments;

            }
            else if (dbObj.Status == PurchaseRequestStatusConstants.Resubmit)
            {
                pModel.Return_Comments = dbObj.Return_Comments;
            }
            pModel.ApprovedBy_PersonnelId = dbObj.ApprovedBy_PersonnelId;
            pModel.CreatedBy_PersonnelId = dbObj.CreatedBy_PersonnelId;
            pModel.Process_Comments = dbObj.Process_Comments;
            pModel.ProcessBy_PersonnelId = dbObj.ProcessBy_PersonnelId;
            pModel.VendorId = dbObj.VendorId;
            pModel.PurchaseOrderId = dbObj.PurchaseOrderId;
            pModel.Return_Comments = dbObj.Return_Comments;
            pModel.UpdateIndex = dbObj.UpdateIndex;
            pModel.IsPunchOut = dbObj.IsPunchOut;
            pModel.StoreroomId = dbObj.StoreroomId;
            pModel.StoreroomName = dbObj.StoreroomName;
            return pModel;
        }
        private EditPurchaseRequestModelDynamic MapPRHeaderUDFDataForEdit(EditPurchaseRequestModelDynamic editPurchaseRequestModelDynamic, PRHeaderUDF prHeaderUDF)
        {
            if (prHeaderUDF != null)
            {
                editPurchaseRequestModelDynamic.PRHeaderUDFId = prHeaderUDF.PRHeaderUDFId;

                editPurchaseRequestModelDynamic.Text1 = prHeaderUDF.Text1;
                editPurchaseRequestModelDynamic.Text2 = prHeaderUDF.Text2;
                editPurchaseRequestModelDynamic.Text3 = prHeaderUDF.Text3;
                editPurchaseRequestModelDynamic.Text4 = prHeaderUDF.Text4;

                if (prHeaderUDF.Date1 != null && prHeaderUDF.Date1 == DateTime.MinValue)
                {
                    editPurchaseRequestModelDynamic.Date1 = null;
                }
                else
                {
                    editPurchaseRequestModelDynamic.Date1 = prHeaderUDF.Date1;
                }
                if (prHeaderUDF.Date2 != null && prHeaderUDF.Date2 == DateTime.MinValue)
                {
                    editPurchaseRequestModelDynamic.Date2 = null;
                }
                else
                {
                    editPurchaseRequestModelDynamic.Date2 = prHeaderUDF.Date2;
                }
                if (prHeaderUDF.Date3 != null && prHeaderUDF.Date3 == DateTime.MinValue)
                {
                    editPurchaseRequestModelDynamic.Date3 = null;
                }
                else
                {
                    editPurchaseRequestModelDynamic.Date3 = prHeaderUDF.Date3;
                }
                if (prHeaderUDF.Date4 != null && prHeaderUDF.Date4 == DateTime.MinValue)
                {
                    editPurchaseRequestModelDynamic.Date4 = null;
                }
                else
                {
                    editPurchaseRequestModelDynamic.Date4 = prHeaderUDF.Date4;
                }

                editPurchaseRequestModelDynamic.Bit1 = prHeaderUDF.Bit1;
                editPurchaseRequestModelDynamic.Bit2 = prHeaderUDF.Bit2;
                editPurchaseRequestModelDynamic.Bit3 = prHeaderUDF.Bit3;
                editPurchaseRequestModelDynamic.Bit4 = prHeaderUDF.Bit4;

                editPurchaseRequestModelDynamic.Numeric1 = prHeaderUDF.Numeric1;
                editPurchaseRequestModelDynamic.Numeric2 = prHeaderUDF.Numeric2;
                editPurchaseRequestModelDynamic.Numeric3 = prHeaderUDF.Numeric3;
                editPurchaseRequestModelDynamic.Numeric4 = prHeaderUDF.Numeric4;

                editPurchaseRequestModelDynamic.Select1 = prHeaderUDF.Select1;
                editPurchaseRequestModelDynamic.Select2 = prHeaderUDF.Select2;
                editPurchaseRequestModelDynamic.Select3 = prHeaderUDF.Select3;
                editPurchaseRequestModelDynamic.Select4 = prHeaderUDF.Select4;
            }
            return editPurchaseRequestModelDynamic;
        }


        public PurchaseRequest EditPurchaseRequestDynamic(PurchaseRequestVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            PurchaseRequest purchaseRequest = new PurchaseRequest()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestId = Convert.ToInt64(objVM.EditPurchaseRequest.PurchaseRequestId)
            };
            purchaseRequest.Retrieve(_dbKey);

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequest, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditPurchaseRequest);
                getpropertyInfo = objVM.EditPurchaseRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPurchaseRequest);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = purchaseRequest.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "VendorId")
                {
                    // RKL - 2022-12-07 You can update a purchase request vendor if it has line items.
                    if (!(objVM.EditPurchaseRequest.IsPunchOut)) //|| objVM.EditPurchaseRequest.CountLineItem > 0))
                    {
                        setpropertyInfo.SetValue(purchaseRequest, val);
                    }
                }
                else
                {
                    setpropertyInfo.SetValue(purchaseRequest, val);
                }

            }
            purchaseRequest.Update(_dbKey);
            List<string> errors = new List<string>();
            if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                errors = EditPRHeaderUDFDynamic(objVM.EditPurchaseRequest, configDetails);

            }
            if (errors != null && errors.Count() > 0)
            {
                purchaseRequest.ErrorMessages.AddRange(errors);
            }
            return purchaseRequest;
        }
        public List<string> EditPRHeaderUDFDynamic(Models.PurchaseRequest.UIConfiguration.EditPurchaseRequestModelDynamic prRequest,
  List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PRHeaderUDF prHeaderUDF = new PRHeaderUDF();
            prHeaderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            prHeaderUDF.PurchaseRequestId = prRequest.PurchaseRequestId ?? 0;

            prHeaderUDF = prHeaderUDF.RetrieveByPurchaseRequestId(userData.DatabaseKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, prRequest);
                getpropertyInfo = prRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prRequest);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prHeaderUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prHeaderUDF, val);
            }
            if (prHeaderUDF.PurchaseRequestId > 0)
            {
                prHeaderUDF.Update(_dbKey);
            }
            else
            {
                prHeaderUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                prHeaderUDF.PurchaseRequestId = prRequest.PurchaseRequestId ?? 0;
                prHeaderUDF.Create(_dbKey);
            }

            return prHeaderUDF.ErrorMessages;
        }
        #endregion
        #region Add PR Line Item (Not In Inventory)
        internal PurchaseRequestLineItem AddPartNotInInventoryDynamic(PurchaseRequestVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PurchaseRequestLineItem purchaseRequestLineItem = new PurchaseRequestLineItem();
            purchaseRequestLineItem.ClientId = this.userData.DatabaseKey.Client.ClientId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequestLineItemPartNotInInventory, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.AddPRLineItemPartNotInInventory);
                getpropertyInfo = objVM.AddPRLineItemPartNotInInventory.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.AddPRLineItemPartNotInInventory);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = purchaseRequestLineItem.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "RequiredDate")
                {
                    if (userData.Site.ShoppingCart)
                    {
                        setpropertyInfo.SetValue(purchaseRequestLineItem, val);
                    }
                    continue;
                }

                if (item.ColumnName == "ChargeToId")
                {
                    continue;
                }
                setpropertyInfo.SetValue(purchaseRequestLineItem, val);
            }
            purchaseRequestLineItem.UnitofMeasure = objVM.AddPRLineItemPartNotInInventory.PurchaseUOM;
            purchaseRequestLineItem.PurchaseRequestId = objVM.AddPRLineItemPartNotInInventory.PurchaseRequestId ?? 0;
            purchaseRequestLineItem.ChargeToID = objVM.AddPRLineItemPartNotInInventory.ChargeToId ?? 0;
            purchaseRequestLineItem.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;

            purchaseRequestLineItem.CreateWithValidation(this.userData.DatabaseKey);
            if (purchaseRequestLineItem.ErrorMessages != null && purchaseRequestLineItem.ErrorMessages.Count == 0)
            {
                purchaseRequestLineItem.PRReOrderLineNumber(userData.DatabaseKey);
                if (purchaseRequestLineItem.ChargeType == AttachmentTableConstant.WorkOrder && (objVM.AddPRLineItemPartNotInInventory.Status == PurchaseRequestStatusConstants.Open || objVM.AddPRLineItemPartNotInInventory.Status == PurchaseRequestStatusConstants.Approved || objVM.AddPRLineItemPartNotInInventory.Status == PurchaseRequestStatusConstants.Resubmit || objVM.AddPRLineItemPartNotInInventory.Status == PurchaseRequestStatusConstants.AwaitApproval || objVM.AddPRLineItemPartNotInInventory.Status == PurchaseRequestStatusConstants.Extracted))
                {
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(purchaseRequestLineItem.ChargeToID, "Add"));
                }
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPurchaseRequestLineItemUDFDynamic(objVM.AddPRLineItemPartNotInInventory, purchaseRequestLineItem.PurchaseRequestLineItemId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        purchaseRequestLineItem.ErrorMessages.AddRange(errors);
                    }
                }
            }
            return purchaseRequestLineItem;
        }
        public List<string> AddPurchaseRequestLineItemUDFDynamic(Models.PurchaseRequest.UIConfiguration.AddPRLineItemPartNotInInventoryModelDynamic prLineItemModelDynamic, long purchaseRequestLineItemId,
   List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PRLineUDF prLineUDF = new PRLineUDF();
            prLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            prLineUDF.PurchaseRequestLineItemId = purchaseRequestLineItemId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, prLineItemModelDynamic);
                getpropertyInfo = prLineItemModelDynamic.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prLineItemModelDynamic);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prLineUDF, val);
            }
            prLineUDF.Create(_dbKey);
            return prLineUDF.ErrorMessages;
        }
        #endregion

        #region Edit Part in inventory (PR Line Item) 
        internal EditPRLineItemPartInInventoryModelDynamic GetPRLineItemInInventoryByIdDynamic(long PurchaseRequestLineItemId, long PurchaseRequestId)
        {
            EditPRLineItemPartInInventoryModelDynamic editPRLineItemPartInInventoryModel = new EditPRLineItemPartInInventoryModelDynamic();
            PurchaseRequestLineItem prLineItemDetails = RetrievePRLineItemByPurchaseRequestLineItemId(PurchaseRequestLineItemId);
            PRLineUDF pRLineUDF = RetrievePRLineItemUDFByPurchaseRequestLineItemId(PurchaseRequestLineItemId);
            editPRLineItemPartInInventoryModel = MapPRLineItemPartInInventoryDataForEdit(editPRLineItemPartInInventoryModel, prLineItemDetails, PurchaseRequestId);
            editPRLineItemPartInInventoryModel = MapPRLineUDFPartInInventoryDataForEdit(editPRLineItemPartInInventoryModel, pRLineUDF);
            return editPRLineItemPartInInventoryModel;
        }

        public EditPRLineItemPartInInventoryModelDynamic MapPRLineItemPartInInventoryDataForEdit(EditPRLineItemPartInInventoryModelDynamic objLineItem, PurchaseRequestLineItem purchaseRequestlineitem, long purchaseRequestId)
        {

            objLineItem.PurchaseRequestLineItemId = purchaseRequestlineitem.PurchaseRequestLineItemId;
            objLineItem.PurchaseRequestId = purchaseRequestId;
            if (purchaseRequestlineitem.PartId == 0)
            {
                objLineItem.PartId = null;
                objLineItem.PartClientLookupId = "";
            }
            else
            {
                objLineItem.PartId = purchaseRequestlineitem.PartId;
                objLineItem.PartClientLookupId = purchaseRequestlineitem.PartClientLookupId;
            }
            objLineItem.LineNumber = purchaseRequestlineitem.LineNumber;
            objLineItem.Description = purchaseRequestlineitem.Description;
            objLineItem.OrderQuantity = purchaseRequestlineitem.OrderQuantity;
            objLineItem.UnitofMeasure = purchaseRequestlineitem.UnitofMeasure;
            objLineItem.UnitCost = purchaseRequestlineitem.UnitCost;
            objLineItem.TotalCost = purchaseRequestlineitem.TotalCost;
            if (purchaseRequestlineitem.AccountId == 0)
            {
                objLineItem.AccountId = null;
                objLineItem.AccountClientLookupId = "";
            }
            else
            {
                objLineItem.AccountId = purchaseRequestlineitem.AccountId;
                objLineItem.AccountClientLookupId = purchaseRequestlineitem.Account_ClientLookupId;
            }
            objLineItem.IsPunchOut = purchaseRequestlineitem.Ispunchout;
            objLineItem.PurchaseUOM = purchaseRequestlineitem.PurchaseUOM;
            objLineItem.UOMConvRequired = purchaseRequestlineitem.UOMConvRequired;
            objLineItem.UOMConversion = purchaseRequestlineitem.UOMConversion;
            if (purchaseRequestlineitem.RequiredDate.HasValue && purchaseRequestlineitem.RequiredDate.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString())
            {
                objLineItem.RequiredDate = purchaseRequestlineitem.RequiredDate;
            }
            objLineItem.SupplierPartId = purchaseRequestlineitem.SupplierPartId;
            objLineItem.SupplierPartAuxiliaryId = purchaseRequestlineitem.SupplierPartAuxiliaryId;
            objLineItem.ManufacturerPartId = purchaseRequestlineitem.ManufacturerPartId;
            objLineItem.Manufacturer = purchaseRequestlineitem.Manufacturer;
            objLineItem.VendorCatalogItemId = purchaseRequestlineitem.VendorCatalogItemId;
            if (purchaseRequestlineitem.UNSPSC == 0)
            {
                objLineItem.UNSPSC = null;
                objLineItem.PartCategoryMasterClientLookupId = "";
            }
            else
            {
                objLineItem.UNSPSC = purchaseRequestlineitem.UNSPSC;
                objLineItem.PartCategoryMasterClientLookupId = purchaseRequestlineitem.PartCategoryMasterClientLookupId;
            }
            return objLineItem;
        }
        private EditPRLineItemPartInInventoryModelDynamic MapPRLineUDFPartInInventoryDataForEdit(EditPRLineItemPartInInventoryModelDynamic ediPRLineItemModel, PRLineUDF pRLineUDF)
        {
            if (pRLineUDF != null)
            {
                ediPRLineItemModel.PRLineUDFId = pRLineUDF.PRLineUDFId;

                ediPRLineItemModel.Text1 = pRLineUDF.Text1;
                ediPRLineItemModel.Text2 = pRLineUDF.Text2;
                ediPRLineItemModel.Text3 = pRLineUDF.Text3;
                ediPRLineItemModel.Text4 = pRLineUDF.Text4;

                if (pRLineUDF.Date1 != null && pRLineUDF.Date1 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date1 = null;
                }
                else
                {
                    ediPRLineItemModel.Date1 = pRLineUDF.Date1;
                }
                if (pRLineUDF.Date2 != null && pRLineUDF.Date2 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date2 = null;
                }
                else
                {
                    ediPRLineItemModel.Date2 = pRLineUDF.Date2;
                }
                if (pRLineUDF.Date3 != null && pRLineUDF.Date3 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date3 = null;
                }
                else
                {
                    ediPRLineItemModel.Date3 = pRLineUDF.Date3;
                }
                if (pRLineUDF.Date4 != null && pRLineUDF.Date4 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date4 = null;
                }
                else
                {
                    ediPRLineItemModel.Date4 = pRLineUDF.Date4;
                }

                ediPRLineItemModel.Bit1 = pRLineUDF.Bit1;
                ediPRLineItemModel.Bit2 = pRLineUDF.Bit2;
                ediPRLineItemModel.Bit3 = pRLineUDF.Bit3;
                ediPRLineItemModel.Bit4 = pRLineUDF.Bit4;

                ediPRLineItemModel.Numeric1 = pRLineUDF.Numeric1;
                ediPRLineItemModel.Numeric2 = pRLineUDF.Numeric2;
                ediPRLineItemModel.Numeric3 = pRLineUDF.Numeric3;
                ediPRLineItemModel.Numeric4 = pRLineUDF.Numeric4;

                ediPRLineItemModel.Select1 = pRLineUDF.Select1;
                ediPRLineItemModel.Select2 = pRLineUDF.Select2;
                ediPRLineItemModel.Select3 = pRLineUDF.Select3;
                ediPRLineItemModel.Select4 = pRLineUDF.Select4;
            }
            return ediPRLineItemModel;
        }
        internal PurchaseRequestLineItem UpdatePRPartInInventoryDynamic(PurchaseRequestVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            PurchaseRequestLineItem prLineItem = new PurchaseRequestLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestLineItemId = objVM.EditPRLineItemPartInInventory.PurchaseRequestLineItemId ?? 0
            };
            prLineItem.Retrieve(_dbKey);
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequestLineItemPartInInventory, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditPRLineItemPartInInventory);
                getpropertyInfo = objVM.EditPRLineItemPartInInventory.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPRLineItemPartInInventory);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prLineItem.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "ChargeToId")
                {
                    continue;
                }
                if (item.ColumnName == "RequiredDate")
                {
                    if (userData.Site.ShoppingCart)
                    {
                        setpropertyInfo.SetValue(prLineItem, val);
                    }
                    continue;
                }

                setpropertyInfo.SetValue(prLineItem, val);
            }
            string status = objVM.EditPRLineItemPartInInventory.Status ?? string.Empty;
            string OldChargeType = prLineItem.ChargeType ?? string.Empty;
            long OldChargeToId = prLineItem.ChargeToID;
            string NewChargeType = objVM.EditPRLineItemPartInInventory.ChargeType ?? string.Empty;
            long NewChargeToId = objVM.EditPRLineItemPartInInventory.ChargeToId ?? 0;
            prLineItem.UpdateByPKForeignKeys(this.userData.DatabaseKey);

            if (status == PurchaseRequestStatusConstants.Open || status == PurchaseRequestStatusConstants.Approved || status == PurchaseRequestStatusConstants.Resubmit || status == PurchaseRequestStatusConstants.AwaitApproval || status == PurchaseRequestStatusConstants.Extracted)
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
                errors = UpdatePRLineUDFPartInInventoryDynamic(objVM.EditPRLineItemPartInInventory, configDetails);

            }
            if (errors != null && errors.Count() > 0)
            {
                prLineItem.ErrorMessages.AddRange(errors);
            }
            return prLineItem;
        }
        public List<string> UpdatePRLineUDFPartInInventoryDynamic(Models.PurchaseRequest.UIConfiguration.EditPRLineItemPartInInventoryModelDynamic prLineItem,
  List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PRLineUDF prLineUDF = new PRLineUDF();
            prLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            prLineUDF.PurchaseRequestLineItemId = prLineItem.PurchaseRequestLineItemId ?? 0;

            prLineUDF = prLineUDF.RetrieveByPurchaseRequestLineItemId(userData.DatabaseKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, prLineItem);
                getpropertyInfo = prLineItem.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prLineItem);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prLineUDF, val);
            }
            if (prLineUDF.PurchaseRequestLineItemId > 0)
            {
                prLineUDF.Update(_dbKey);
            }
            else
            {
                prLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                prLineUDF.PurchaseRequestLineItemId = prLineItem.PurchaseRequestLineItemId ?? 0;
                prLineUDF.Create(_dbKey);
            }

            return prLineUDF.ErrorMessages;
        }
        #endregion
        public PurchaseRequestLineItem RetrievePRLineItemByPurchaseRequestLineItemId(long PurchaseRequestLineItemId)
        {
            PurchaseRequestLineItem purchaseRequestlineitem = new PurchaseRequestLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestLineItemId = PurchaseRequestLineItemId
            };
            purchaseRequestlineitem.PurchaseRequestLineItemRetrieveByPurchaseRequestLineItemIdV2(userData.DatabaseKey);
            return purchaseRequestlineitem;
        }
        public PRLineUDF RetrievePRLineItemUDFByPurchaseRequestLineItemId(long purchaseRequestLineItemId)
        {
            PRLineUDF pRLineUDF = new PRLineUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PurchaseRequestLineItemId = purchaseRequestLineItemId
            };

            pRLineUDF = pRLineUDF.RetrieveByPurchaseRequestLineItemId(this.userData.DatabaseKey);
            return pRLineUDF;
        }
        #region Edit Part Not in inventory (PR Line Item) 
        internal EditPRLineItemPartNotInInventoryModelDynamic GetPRLineItemNotInInventoryByIdDynamic(long PurchaseRequestLineItemId, long PurchaseRequestId)
        {
            EditPRLineItemPartNotInInventoryModelDynamic editPRLineItemPartNotInInventoryModel = new EditPRLineItemPartNotInInventoryModelDynamic();
            PurchaseRequestLineItem prLineItemDetails = RetrievePRLineItemByPurchaseRequestLineItemId(PurchaseRequestLineItemId);
            PRLineUDF pRLineUDF = RetrievePRLineItemUDFByPurchaseRequestLineItemId(PurchaseRequestLineItemId);
            editPRLineItemPartNotInInventoryModel = MapPRLineItemPartNotInInventoryDataForEdit(editPRLineItemPartNotInInventoryModel, prLineItemDetails, PurchaseRequestId);
            editPRLineItemPartNotInInventoryModel = MapPRLineUDFPartNotInInventoryDataForEdit(editPRLineItemPartNotInInventoryModel, pRLineUDF);
            return editPRLineItemPartNotInInventoryModel;
        }
        public EditPRLineItemPartNotInInventoryModelDynamic MapPRLineItemPartNotInInventoryDataForEdit(EditPRLineItemPartNotInInventoryModelDynamic objLineItem, PurchaseRequestLineItem purchaseRequestlineitem, long purchaseRequestId)
        {

            objLineItem.PurchaseRequestLineItemId = purchaseRequestlineitem.PurchaseRequestLineItemId;
            objLineItem.PurchaseRequestId = purchaseRequestId;
            objLineItem.PartId = purchaseRequestlineitem.PartId;
            objLineItem.LineNumber = purchaseRequestlineitem.LineNumber;
            objLineItem.PartClientLookupId = purchaseRequestlineitem.PartClientLookupId;
            objLineItem.Description = purchaseRequestlineitem.Description;
            objLineItem.OrderQuantity = purchaseRequestlineitem.OrderQuantity;
            objLineItem.UnitofMeasure = purchaseRequestlineitem.UnitofMeasure;
            objLineItem.UnitCost = purchaseRequestlineitem.UnitCost;
            objLineItem.TotalCost = purchaseRequestlineitem.TotalCost;
            if (purchaseRequestlineitem.AccountId == 0)
            {
                objLineItem.AccountId = null;
                objLineItem.AccountClientLookupId = "";
            }
            else
            {
                objLineItem.AccountId = purchaseRequestlineitem.AccountId;
                objLineItem.AccountClientLookupId = purchaseRequestlineitem.Account_ClientLookupId;
            }
            objLineItem.ChargeType = purchaseRequestlineitem.ChargeType;
            if (purchaseRequestlineitem.ChargeToID == 0)
            {
                objLineItem.ChargeToId = null;
                objLineItem.ChargeTo_Name = "";
                objLineItem.ChargeToClientLookupId = "";
            }
            else
            {
                objLineItem.ChargeToId = purchaseRequestlineitem.ChargeToID;
                objLineItem.ChargeTo_Name = purchaseRequestlineitem.ChargeTo_Name;
                objLineItem.ChargeToClientLookupId = purchaseRequestlineitem.ChargeToClientLookupId;
            }
            objLineItem.IsPunchOut = purchaseRequestlineitem.Ispunchout;
            objLineItem.PurchaseUOM = purchaseRequestlineitem.PurchaseUOM;
            objLineItem.UOMConvRequired = purchaseRequestlineitem.UOMConvRequired;
            objLineItem.UOMConversion = purchaseRequestlineitem.UOMConversion;
            if (purchaseRequestlineitem.RequiredDate.HasValue && purchaseRequestlineitem.RequiredDate.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString())
            {
                objLineItem.RequiredDate = purchaseRequestlineitem.RequiredDate;
            }
            objLineItem.SupplierPartId = purchaseRequestlineitem.SupplierPartId;
            objLineItem.SupplierPartAuxiliaryId = purchaseRequestlineitem.SupplierPartAuxiliaryId;
            objLineItem.ManufacturerPartId = purchaseRequestlineitem.ManufacturerPartId;
            objLineItem.Manufacturer = purchaseRequestlineitem.Manufacturer;
            objLineItem.VendorCatalogItemId = purchaseRequestlineitem.VendorCatalogItemId;
            if (purchaseRequestlineitem.UNSPSC == 0)
            {
                objLineItem.UNSPSC = null;
                objLineItem.PartCategoryMasterClientLookupId = "";
            }
            else
            {
                objLineItem.UNSPSC = purchaseRequestlineitem.UNSPSC;
                objLineItem.PartCategoryMasterClientLookupId = purchaseRequestlineitem.PartCategoryMasterClientLookupId;
            }
            return objLineItem;
        }
        private EditPRLineItemPartNotInInventoryModelDynamic MapPRLineUDFPartNotInInventoryDataForEdit(EditPRLineItemPartNotInInventoryModelDynamic ediPRLineItemModel, PRLineUDF pRLineUDF)
        {
            if (pRLineUDF != null)
            {
                ediPRLineItemModel.PRLineUDFId = pRLineUDF.PRLineUDFId;

                ediPRLineItemModel.Text1 = pRLineUDF.Text1;
                ediPRLineItemModel.Text2 = pRLineUDF.Text2;
                ediPRLineItemModel.Text3 = pRLineUDF.Text3;
                ediPRLineItemModel.Text4 = pRLineUDF.Text4;

                if (pRLineUDF.Date1 != null && pRLineUDF.Date1 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date1 = null;
                }
                else
                {
                    ediPRLineItemModel.Date1 = pRLineUDF.Date1;
                }
                if (pRLineUDF.Date2 != null && pRLineUDF.Date2 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date2 = null;
                }
                else
                {
                    ediPRLineItemModel.Date2 = pRLineUDF.Date2;
                }
                if (pRLineUDF.Date3 != null && pRLineUDF.Date3 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date3 = null;
                }
                else
                {
                    ediPRLineItemModel.Date3 = pRLineUDF.Date3;
                }
                if (pRLineUDF.Date4 != null && pRLineUDF.Date4 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date4 = null;
                }
                else
                {
                    ediPRLineItemModel.Date4 = pRLineUDF.Date4;
                }

                ediPRLineItemModel.Bit1 = pRLineUDF.Bit1;
                ediPRLineItemModel.Bit2 = pRLineUDF.Bit2;
                ediPRLineItemModel.Bit3 = pRLineUDF.Bit3;
                ediPRLineItemModel.Bit4 = pRLineUDF.Bit4;

                ediPRLineItemModel.Numeric1 = pRLineUDF.Numeric1;
                ediPRLineItemModel.Numeric2 = pRLineUDF.Numeric2;
                ediPRLineItemModel.Numeric3 = pRLineUDF.Numeric3;
                ediPRLineItemModel.Numeric4 = pRLineUDF.Numeric4;

                ediPRLineItemModel.Select1 = pRLineUDF.Select1;
                ediPRLineItemModel.Select2 = pRLineUDF.Select2;
                ediPRLineItemModel.Select3 = pRLineUDF.Select3;
                ediPRLineItemModel.Select4 = pRLineUDF.Select4;
            }
            return ediPRLineItemModel;
        }
        internal PurchaseRequestLineItem UpdatePRPartNotInInventoryDynamic(PurchaseRequestVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            PurchaseRequestLineItem prLineItem = new PurchaseRequestLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestLineItemId = objVM.EditPRLineItemPartNotInInventory.PurchaseRequestLineItemId ?? 0
            };
            prLineItem.Retrieve(_dbKey);
            // RKL - 2022-12-06 - V2-814 
            // The OldChargeType and OldChargeToId must be filled before loading the prLineItem with the 
            // information entered on the page
            string OldChargeType = prLineItem.ChargeType ?? string.Empty;
            long OldChargeToId = prLineItem.ChargeToID;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequestLineItemPartNotInInventory, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditPRLineItemPartNotInInventory);
                getpropertyInfo = objVM.EditPRLineItemPartNotInInventory.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPRLineItemPartNotInInventory);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prLineItem.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "ChargeToId")
                {
                    continue;
                }
                if (item.ColumnName == "RequiredDate")
                {
                    if (userData.Site.ShoppingCart)
                    {
                        setpropertyInfo.SetValue(prLineItem, val);
                    }
                    continue;
                }

                setpropertyInfo.SetValue(prLineItem, val);
            }
            string status = objVM.EditPRLineItemPartNotInInventory.Status ?? string.Empty;
            // RKL - 2022-12-06 - V2-814 
            // The OldChargeType and OldChargeToId must be filled before loading the prLineItem with the 
            // information entered on the page
            //string OldChargeType = prLineItem.ChargeType ?? string.Empty;
            //long OldChargeToId = prLineItem.ChargeToID;
            string NewChargeType = objVM.EditPRLineItemPartNotInInventory.ChargeType ?? string.Empty;
            long NewChargeToId = objVM.EditPRLineItemPartNotInInventory.ChargeToId ?? 0;

            prLineItem.ChargeToID = objVM.EditPRLineItemPartNotInInventory.ChargeToId ?? 0;
            prLineItem.UnitofMeasure = prLineItem.PurchaseUOM;
            prLineItem.UpdateByPKForeignKeys(this.userData.DatabaseKey);

            if (status == PurchaseRequestStatusConstants.Open || status == PurchaseRequestStatusConstants.Approved || status == PurchaseRequestStatusConstants.Resubmit || status == PurchaseRequestStatusConstants.AwaitApproval || status == PurchaseRequestStatusConstants.Extracted)
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
                errors = UpdatePRLineUDFPartNotInInventoryDynamic(objVM.EditPRLineItemPartNotInInventory, configDetails);

            }
            if (errors != null && errors.Count() > 0)
            {
                prLineItem.ErrorMessages.AddRange(errors);
            }
            return prLineItem;
        }
        public List<string> UpdatePRLineUDFPartNotInInventoryDynamic(Models.PurchaseRequest.UIConfiguration.EditPRLineItemPartNotInInventoryModelDynamic prLineItem,
 List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PRLineUDF prLineUDF = new PRLineUDF();
            prLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            prLineUDF.PurchaseRequestLineItemId = prLineItem.PurchaseRequestLineItemId ?? 0;

            prLineUDF = prLineUDF.RetrieveByPurchaseRequestLineItemId(userData.DatabaseKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, prLineItem);
                getpropertyInfo = prLineItem.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prLineItem);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prLineUDF, val);
            }
            if (prLineUDF.PurchaseRequestLineItemId > 0)
            {
                prLineUDF.Update(_dbKey);
            }
            else
            {
                prLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                prLineUDF.PurchaseRequestLineItemId = prLineItem.PurchaseRequestLineItemId ?? 0;
                prLineUDF.Create(_dbKey);
            }

            return prLineUDF.ErrorMessages;
        }

        #endregion

        #endregion

        #region V2-726
        public bool RetrieveApprovalGroupRequestStatus(string RequestType)
        {
            bool IsRequestType = false;
            ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId
            };
            var approvalGroupSettingsList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(userData.DatabaseKey).FirstOrDefault();
            if (approvalGroupSettingsList != null)
            {
                if (RequestType == "PurchaseRequests")
                {
                    if (approvalGroupSettingsList.PurchaseRequests == true)
                    {
                        IsRequestType = true;
                    }
                }
            }
            return IsRequestType;
        }

        public PurchaseRequest ApprovePR(ApprovalRouteModel arModel)
        {
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = arModel.ObjectId;

            purchaseRequest.Retrieve(userData.DatabaseKey);
            PurchaseRequest pr = new PurchaseRequest();
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.PurchaseRequestId = arModel.ObjectId;
            pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.DatabaseKey.Client.DefaultTimeZone);
            pr.Status = PurchaseRequestStatusConstants.AwaitApproval;
            //V2-832
            pr.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            pr.Processed_Date = System.DateTime.UtcNow;
            pr.Process_Comments = arModel.Comments ?? string.Empty;
            //V2-832
            pr.UpdateIndex = purchaseRequest.UpdateIndex;
            pr.Update(userData.DatabaseKey);
            //V2-820 update existing record Approval Route
            ApprovalRoute approvalRoute = new ApprovalRoute()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = arModel.ObjectId,
                RequestType = arModel.RequestType,
                ApproverId = arModel.ApproverId
            };
            List<ApprovalRoute> list = ApprovalRoute.ApprovalRoute_RetrievebyObjectId_V2(this.userData.DatabaseKey, approvalRoute);
            if (list != null && list.Count > 0)
            {
                ApprovalRoute AR = new ApprovalRoute();
                AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
                AR.ApprovalGroupId = list[0].ApprovalGroupId;
                AR.ObjectId = arModel.ObjectId;
                AR.ProcessResponse = PurchasingEvents.SendForApproval;
                AR.ApproverId = arModel.ApproverId;
                AR.UpdateByObjectId_V2(userData.DatabaseKey); //updating IsProcessed=1
            }
            //V2-820
            CreateEventLog(arModel.ApproverId, arModel.ObjectId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType);
            // Send Notification
            List<long> purchaserequestid = new List<long>() { arModel.ObjectId };
            var UserList = new List<Tuple<long, string, string>>();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var PersonnelInfo = coWrapper.GetPersonnelDetailsByPersonnelId(arModel.ApproverId);
            if (PersonnelInfo != null)
            {
                UserList.Add
                 (
                           Tuple.Create(Convert.ToInt64(PersonnelInfo.PersonnelId), PersonnelInfo.UserName, PersonnelInfo.Email)
                );
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.PurchaseRequest>(AlertTypeEnum.PurchaseRequestApprovalNeeded, purchaserequestid, UserList));
            }
            return pr;
        }
        private void CreateEventLog(Int64 ApproverId, Int64 ObjectId, string comment, long ApprovalGroupId, string RequestType)
        {
            ApprovalRoute log = new ApprovalRoute();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.ApproverId = ApproverId;
            log.ObjectId = ObjectId;
            log.Comments = comment;
            log.ApprovalGroupId = ApprovalGroupId;
            log.RequestType = RequestType;
            log.IsProcessed = false;
            log.ProcessResponse = String.Empty;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region V2-693 Send to sap
        internal PurchaseRequest RetrievePurchaseRequestByIdForExportSAP(long PurchaseRequestId)
        {
            PurchaseRequest purchaseRequest = new PurchaseRequest();

            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;

            purchaseRequest.RetrieveByIdForExportSAP(userData.DatabaseKey);
            return purchaseRequest;
        }
        internal List<PurchaseRequestLineItem> RetrievePRLineItemByIdForExportSAP(long PurchaseRequestId)
        {
            PurchaseRequestLineItem purchaseRequestLineItem = new PurchaseRequestLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestId = PurchaseRequestId
            };
            List<PurchaseRequestLineItem> purchaseRequestLintItemList = PurchaseRequestLineItem.RetrievePRLineItemByIdForExportSAP(this.userData.DatabaseKey, purchaseRequestLineItem);
            return purchaseRequestLintItemList;
        }
        #endregion

        #region V2-730
        public List<ApprovalRouteModelByObjectId> RetrieveApprovalGroupIdbyObjectId(long ApproverId, long ObjectId, string RequestType)
        {
            ApprovalRouteModelByObjectId approvalRouteModel;
            List<ApprovalRouteModelByObjectId> approvalRouteModelList = new List<ApprovalRouteModelByObjectId>();

            ApprovalRoute approvalRoute = new ApprovalRoute()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = ObjectId,
                RequestType = RequestType,
                ApproverId = ApproverId
            };
            List<ApprovalRoute> list = ApprovalRoute.ApprovalRoute_RetrievebyObjectId_V2(this.userData.DatabaseKey, approvalRoute);
            foreach (var ec in list)
            {
                approvalRouteModel = new ApprovalRouteModelByObjectId();
                approvalRouteModel.ApprovalGroupId = ec.ApprovalGroupId;
                approvalRouteModel.ApprovalRouteId = ec.ApprovalRouteId;

                approvalRouteModelList.Add(approvalRouteModel);
            }

            return approvalRouteModelList;
        }
        public PurchaseRequest MultiLevelApprovePR(ApprovalRouteModel arModel)
        {
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = arModel.ObjectId;

            purchaseRequest.Retrieve(userData.DatabaseKey);
            PurchaseRequest pr = new PurchaseRequest();
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.PurchaseRequestId = arModel.ObjectId;
            pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.DatabaseKey.Client.DefaultTimeZone);
            pr.Status = PurchaseRequestStatusConstants.AwaitApproval;
            pr.UpdateIndex = purchaseRequest.UpdateIndex;
            //V2-832
            pr.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            pr.Processed_Date = System.DateTime.UtcNow;
            pr.Process_Comments = arModel.Comments ?? string.Empty;
            //V2-832
            pr.Update(userData.DatabaseKey);
            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = arModel.ApprovalGroupId;
            AR.ObjectId = arModel.ObjectId;
            AR.ProcessResponse = PurchaseRequestStatusConstants.Approved;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);
            CreateEventLog(arModel.ApproverId, arModel.ObjectId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType);
            // Send Notification
            List<long> purchaserequestid = new List<long>() { arModel.ObjectId };
            var UserList = new List<Tuple<long, string, string>>();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var PersonnelInfo = coWrapper.GetPersonnelDetailsByPersonnelId(arModel.ApproverId);
            if (PersonnelInfo != null)
            {
                UserList.Add
                 (
                           Tuple.Create(Convert.ToInt64(PersonnelInfo.PersonnelId), PersonnelInfo.UserName, PersonnelInfo.Email)
                );
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.PurchaseRequest>(AlertTypeEnum.PurchaseRequestApprovalNeeded, purchaserequestid, UserList));
            }

            return pr;
        }
        public PurchaseRequest MultiLevelFinalApprovePR(long PurchaseRequestId, long ApprovalGroupId)
        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;

            purchaseRequest.Retrieve(userData.DatabaseKey);
            PurchaseRequest pr = new PurchaseRequest();
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.PurchaseRequestId = PurchaseRequestId;
            pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.DatabaseKey.Client.DefaultTimeZone);
            pr.Status = PurchaseRequestStatusConstants.Approved;
            pr.Approved_Date = DateTime.UtcNow;
            pr.ApprovedBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            pr.UpdateIndex = purchaseRequest.UpdateIndex;
            pr.Update(userData.DatabaseKey);
            ApprovalRoute AR = new ApprovalRoute();
            AR.ClientId = userData.DatabaseKey.Client.ClientId;
            AR.ApprovalGroupId = ApprovalGroupId;
            AR.ObjectId = PurchaseRequestId;
            AR.ProcessResponse = PurchaseRequestStatusConstants.Approved;
            AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
            AR.UpdateByObjectId_V2(userData.DatabaseKey);
            PRlist.Add(pr.PurchaseRequestId);
            objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestApproved, PRlist);

            return pr;
        }


        public PurchaseRequest MultiLevelDenyPR(long PurchaseRequestId, long ApprovalGroupId)
        {
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = PurchaseRequestId;

            purchaseRequest.Retrieve(userData.DatabaseKey);
            var PrevStatus = purchaseRequest.Status;
            purchaseRequest.Status = PurchaseRequestStatusConstants.Denied;
            purchaseRequest.Update(userData.DatabaseKey);
            if (purchaseRequest.ErrorMessages == null || purchaseRequest.ErrorMessages.Count == 0)
            {
                ApprovalRoute AR = new ApprovalRoute();
                AR.ClientId = userData.DatabaseKey.Client.ClientId;
                AR.ApprovalGroupId = ApprovalGroupId;
                AR.ObjectId = PurchaseRequestId;
                AR.ProcessResponse = PurchaseRequestStatusConstants.Denied;
                AR.ApproverId = this.userData.DatabaseKey.Personnel.PersonnelId;
                AR.UpdateByObjectId_V2(userData.DatabaseKey);
                PRlist.Add(purchaseRequest.PurchaseRequestId);
                objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestDenied, PRlist);
            }
            return purchaseRequest;
        }
        #endregion
        #region V2-820
        public PurchaseRequest ReviewAndSendApprovePR(ReviewPRSendForApprovalModel arModel)
        {

            List<long> PRlist = new List<long>();
            PurchaseRequest purchaseRequest = new PurchaseRequest();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            purchaseRequest.ClientId = userData.DatabaseKey.Client.ClientId;
            purchaseRequest.PurchaseRequestId = arModel.ObjectId;
            purchaseRequest.Retrieve(userData.DatabaseKey);
            PurchaseRequest pr = new PurchaseRequest();
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.PurchaseRequestId = arModel.ObjectId;
            pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.DatabaseKey.Client.DefaultTimeZone);
            //Update Purchase Request
            pr.Status = PurchaseRequestStatusConstants.AwaitApproval;
            if (userData.DatabaseKey.Personnel.Buyer == true)
            {
                pr.BuyerReview = true;
            }
            //V2-832
            pr.ProcessBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            pr.Processed_Date = System.DateTime.UtcNow;
            pr.Process_Comments = arModel.Comments ?? string.Empty;
            //V2-832
            pr.UpdateIndex = purchaseRequest.UpdateIndex;
            pr.Update(userData.DatabaseKey);
            //Add PurchasingEventLog Entry for Review
            CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.Reviewed, "");
            //update existing record Approval Route
            ApprovalRoute approvalRoute = new ApprovalRoute()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ObjectId = arModel.ObjectId,
                RequestType = arModel.RequestType,
                ApproverId = arModel.ApproverId
            };
            List<ApprovalRoute> list = ApprovalRoute.ApprovalRoute_RetrievebyObjectId_V2(this.userData.DatabaseKey, approvalRoute);
            if (list != null && list.Count > 0)
            {
                ApprovalRoute AR = new ApprovalRoute();
                AR.ClientId = this.userData.DatabaseKey.Client.ClientId;
                AR.ApprovalGroupId = list[0].ApprovalGroupId;
                AR.ObjectId = arModel.ObjectId;
                AR.ProcessResponse = PurchasingEvents.Reviewed;
                AR.ApproverId = arModel.ApproverId;
                AR.UpdateByObjectId_V2(userData.DatabaseKey); //updating IsProcessed=1
            }
            //adding New record ApprovalRoute
            CreateEventLog(arModel.ApproverId, arModel.ObjectId, arModel.Comments, arModel.ApprovalGroupId, arModel.RequestType);
            //Add PurchasingEventLog Entry for Send for Approval
            CreateEventLog(purchaseRequest.PurchaseRequestId, PurchasingEvents.SendForApproval, "");

            // Send Notification
            List<long> purchaserequestid = new List<long>() { arModel.ObjectId };
            var UserList = new List<Tuple<long, string, string>>();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var PersonnelInfo = coWrapper.GetPersonnelDetailsByPersonnelId(arModel.ApproverId);
            if (PersonnelInfo != null)
            {
                UserList.Add
                 (
                           Tuple.Create(Convert.ToInt64(PersonnelInfo.PersonnelId), PersonnelInfo.UserName, PersonnelInfo.Email)
                );
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                Task t2 = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.PurchaseRequest>(AlertTypeEnum.PurchaseRequestApprovalNeeded, purchaserequestid, UserList));
            }

            return pr;
        }

        public List<Client.Models.DataModel> Get_PRApprovedPersonnelListBy(bool buyer = false)
        {
            Client.Models.LookUpListModel model = new Client.Models.LookUpListModel();
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                Buyer = buyer
            };

            List<Personnel> PersonnelList = personnel.RetrieveForLookupListByMultipleSecurityItem(this.userData.DatabaseKey);

            return model.data = ReturnPRPersonnelList(PersonnelList);
        }
        private List<Client.Models.DataModel> ReturnPRPersonnelList(List<Personnel> PersonnelList)
        {
            Client.Models.LookUpListModel model = new Client.Models.LookUpListModel();
            Client.Models.DataModel dModel;
            foreach (var p in PersonnelList)
            {
                dModel = new Client.Models.DataModel();
                dModel.AssignedTo_PersonnelId = p.PersonnelId;
                dModel.AssignedTo_PersonnelClientLookupId = p.ClientLookupId;
                dModel.NameFirst = p.NameFirst;
                dModel.NameLast = p.NameLast;
                dModel.Buyer = p.Buyer;
                model.data.Add(dModel);
            }
            return model.data;
        }
        #endregion
        #region V2-852
        public bool RetrieveCoupaToken(InterfaceProp iProp, ref string coupa_token)
        {
            //string coupa_token;
            bool success;
            string URL = iProp.APIKey1;             //  https://deanfoods-test.coupahost.com/api/requisitions/submit_for_approval
            string id = iProp.APIKey2;              //  "f9112597b359640f0aed9c0bdcb61dfb";
            string secret = iProp.FTPAddress;       //  "6a0adeffc7a359c3fd5ef9d5d750d370371bfd83f89d6de2dcf5d48be74cdbfc";
            string scope = iProp.FTPFileDirectory;  //  "core.requisition.write core.user.read";
            string grant = iProp.FTPArcDirectory;   //  "client_credentials";
            string base_address = iProp.APIKey1;
            using (HttpClient client = new HttpClient())
            {
                if (URL.IndexOf("-test") > 0)
                {
                    client.BaseAddress = new Uri("https://deanfoods-test.coupahost.com/oauth2/");
                }
                else
                {
                    client.BaseAddress = new Uri("https://deanfoods.coupahost.com/oauth2/");
                }
                client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                StringContent content = new StringContent(string.Format("client_id={0}&client_secret={1}&scope={2}&grant_type={3}",
                  HttpUtility.UrlEncode(id),
                  HttpUtility.UrlEncode(secret),
                  HttpUtility.UrlEncode(scope),
                  HttpUtility.UrlEncode(grant)), Encoding.UTF8,
                  "application/x-www-form-urlencoded");
                HttpResponseMessage response = client.PostAsync("token", content).Result;
                string resultJSON = response.Content.ReadAsStringAsync().Result;

                Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(resultJSON);
                if (json.ToString().Contains("error"))
                {
                    success = false;
                    coupa_token = json["error"].ToString();
                }
                else
                {
                    success = true;
                    coupa_token = json["access_token"].ToString();
                }
            }
            return success;
        }
        #endregion

        #region V2-945
        public PurchaseRequest RetrieveAllByPurchaseRequestV2Print(List<long> PurchaseRequestIDList = null)
        {
            PurchaseRequest pr = new PurchaseRequest();
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pr.PurchaseRequestIDList = PurchaseRequestIDList != null && PurchaseRequestIDList.Count > 0 ? string.Join(",", PurchaseRequestIDList) : string.Empty;
            pr.RetrieveAllByPurchaseRequestV2Print(userData.DatabaseKey, userData.Site.TimeZone);

            return pr;
        }
        #endregion

        #region V2-1046
        public List<LineItemModel> LineitemsChunkSearchForConsolidate(long PurchaseRequestId, int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", string Description = "", string VendorClientLookupId = "", string VendorName = "")
        {
            LineItemModel objLineItem;
            List<LineItemModel> LineItemList = new List<LineItemModel>();

            PurchaseRequestLineItem purchaseRequestLineItem = new PurchaseRequestLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestId = PurchaseRequestId
            };
            purchaseRequestLineItem.OffSetVal = skip;
            purchaseRequestLineItem.NextRow = length;
            purchaseRequestLineItem.OrderBy = orderDir;
            purchaseRequestLineItem.OrderbyColumn = orderbycol;
            purchaseRequestLineItem.Description = Description;
            purchaseRequestLineItem.VendorClientLookupId = VendorClientLookupId;
            purchaseRequestLineItem.VendorName = VendorName;
            List<PurchaseRequestLineItem> purchaseRequestLintItemList = purchaseRequestLineItem.PurchaseRequestLineItemRetrieveForConsolidate(this.userData.DatabaseKey);

            if (purchaseRequestLintItemList != null)
            {
                foreach (var item in purchaseRequestLintItemList)
                {
                    objLineItem = new LineItemModel();
                    objLineItem.PurchaseRequestLineItemId = item.PurchaseRequestLineItemId;
                    objLineItem.PartClientLookupId = item.PartClientLookupId;
                    objLineItem.Description = item.Description;
                    objLineItem.OrderQuantity = Math.Round(item.OrderQuantity, 2);
                    objLineItem.UnitofMeasure = item.UnitofMeasure;
                    objLineItem.UnitCost = Math.Round(item.UnitCost, 2);
                    objLineItem.VendorClientLookupId = item.VendorClientLookupId;
                    objLineItem.VendorName = item.VendorName;
                    objLineItem.TotalCount = item.TotalCount;
                    LineItemList.Add(objLineItem);
                }
            }

            return LineItemList;
        }
        public List<string> PRLineItemConsolidateProcess(List<string> purchaseRequestLineItemIds, long PurchaseRequestId)
        {
            PurchaseRequestLineItem purchaseRequestLineItem = new PurchaseRequestLineItem()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId
            };
            purchaseRequestLineItem.PurchaseRequestId = PurchaseRequestId;
            purchaseRequestLineItem.PRLineItemIds = string.Join(",", purchaseRequestLineItemIds ?? new List<string>());
            purchaseRequestLineItem.PRConsolidateProcess(_dbKey);
            if (purchaseRequestLineItem.ErrorMessages == null || purchaseRequestLineItem.ErrorMessages.Count == 0)
            {
                purchaseRequestLineItem.PRReOrderLineNumber(_dbKey);
            }
            return purchaseRequestLineItem.ErrorMessages;
        }
        #endregion

        #region V2-1032 SingleStock LineItem
        #region Add
        internal PurchaseRequestLineItem AddPartInInventorySigleStockDynamic(PurchaseRequestVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PurchaseRequestLineItem purchaseRequestLineItem = new PurchaseRequestLineItem();
            purchaseRequestLineItem.ClientId = this.userData.DatabaseKey.Client.ClientId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequestLineItemStockSingle, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);

            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.AddPRLineItemPartInInventory);
                getpropertyInfo = objVM.AddPRLineItemPartInInventory.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.AddPRLineItemPartInInventory);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = purchaseRequestLineItem.GetType().GetProperty(item.ColumnName);
                if (item.ColumnName == "RequiredDate")
                {
                    if (userData.Site.ShoppingCart)
                    {
                        setpropertyInfo.SetValue(purchaseRequestLineItem, val);
                    }
                    continue;
                }
                if (item.ColumnName == "ChargeToId")
                {
                    continue;
                }
                if (item.ColumnName == "PartId")
                {
                    continue;
                }
                setpropertyInfo.SetValue(purchaseRequestLineItem, val);
            }
            purchaseRequestLineItem.PurchaseRequestId = objVM.AddPRLineItemPartInInventory.PurchaseRequestId ?? 0;
            purchaseRequestLineItem.ChargeToID = objVM.AddPRLineItemPartInInventory.ChargeToId ?? 0;
            purchaseRequestLineItem.PartId = objVM.AddPRLineItemPartInInventory.PartId ?? 0;
            purchaseRequestLineItem.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
            purchaseRequestLineItem.PartStoreroomId = objVM.AddPRLineItemPartInInventory.PartStoreroomId ?? 0;
            purchaseRequestLineItem.CreateWithValidation(this.userData.DatabaseKey);
            if (purchaseRequestLineItem.ErrorMessages != null && purchaseRequestLineItem.ErrorMessages.Count == 0)
            {
                purchaseRequestLineItem.PRReOrderLineNumber(userData.DatabaseKey);
                if (purchaseRequestLineItem.ChargeType == AttachmentTableConstant.WorkOrder && (objVM.AddPRLineItemPartInInventory.Status == PurchaseRequestStatusConstants.Open || objVM.AddPRLineItemPartInInventory.Status == PurchaseRequestStatusConstants.Approved || objVM.AddPRLineItemPartInInventory.Status == PurchaseRequestStatusConstants.Resubmit || objVM.AddPRLineItemPartInInventory.Status == PurchaseRequestStatusConstants.AwaitApproval || objVM.AddPRLineItemPartInInventory.Status == PurchaseRequestStatusConstants.Extracted))
                {
                    CommonWrapper commonWrapper = new CommonWrapper(userData);
                    Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdatePartsonOrder(purchaseRequestLineItem.ChargeToID, "Add"));
                }
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPurchaseRequestLineItemInInventoryUDFDynamic(objVM.AddPRLineItemPartInInventory, purchaseRequestLineItem.PurchaseRequestLineItemId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        purchaseRequestLineItem.ErrorMessages.AddRange(errors);
                    }
                }
            }
            return purchaseRequestLineItem;
        }
        public List<string> AddPurchaseRequestLineItemInInventoryUDFDynamic(AddPRLineItemPartInInventoryModelDynamic prLineItemModelDynamic, long purchaseRequestLineItemId,
   List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PRLineUDF prLineUDF = new PRLineUDF();
            prLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            prLineUDF.PurchaseRequestLineItemId = purchaseRequestLineItemId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, prLineItemModelDynamic);
                getpropertyInfo = prLineItemModelDynamic.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prLineItemModelDynamic);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prLineUDF, val);
            }
            prLineUDF.Create(_dbKey);
            return prLineUDF.ErrorMessages;
        }

        #endregion
        #region Edit
        internal EditPRLineItemPartInInventorySingleStockModelDynamic GetPRLineItemInInventorySingleStockByIdDynamic(long PurchaseRequestLineItemId, long PurchaseRequestId)
        {
            EditPRLineItemPartInInventorySingleStockModelDynamic editPRLineItemPartInInventoryModel = new EditPRLineItemPartInInventorySingleStockModelDynamic();
            PurchaseRequestLineItem prLineItemDetails = RetrievePRLineItemByPurchaseRequestLineItemId(PurchaseRequestLineItemId);
            PRLineUDF pRLineUDF = RetrievePRLineItemUDFByPurchaseRequestLineItemId(PurchaseRequestLineItemId);
            editPRLineItemPartInInventoryModel = MapPRLineItemPartInInventoryDataForSingleStockEdit(editPRLineItemPartInInventoryModel, prLineItemDetails, PurchaseRequestId);
            editPRLineItemPartInInventoryModel = MapPRLineUDFPartInInventoryDataForSingleStockEdit(editPRLineItemPartInInventoryModel, pRLineUDF);
            return editPRLineItemPartInInventoryModel;
        }

        public EditPRLineItemPartInInventorySingleStockModelDynamic MapPRLineItemPartInInventoryDataForSingleStockEdit(EditPRLineItemPartInInventorySingleStockModelDynamic objLineItem, PurchaseRequestLineItem purchaseRequestlineitem, long purchaseRequestId)
        {

            objLineItem.PurchaseRequestLineItemId = purchaseRequestlineitem.PurchaseRequestLineItemId;
            objLineItem.PurchaseRequestId = purchaseRequestId;
            if (purchaseRequestlineitem.PartId == 0)
            {
                objLineItem.PartId = null;
                objLineItem.PartClientLookupId = "";
            }
            else
            {
                objLineItem.PartId = purchaseRequestlineitem.PartId;
                objLineItem.PartClientLookupId = purchaseRequestlineitem.PartClientLookupId;
            }
            objLineItem.LineNumber = purchaseRequestlineitem.LineNumber;
            objLineItem.Description = purchaseRequestlineitem.Description;
            objLineItem.OrderQuantity = purchaseRequestlineitem.OrderQuantity;
            objLineItem.UnitofMeasure = purchaseRequestlineitem.UnitofMeasure;
            objLineItem.UnitCost = purchaseRequestlineitem.UnitCost;
            objLineItem.TotalCost = purchaseRequestlineitem.TotalCost;
            if (purchaseRequestlineitem.AccountId == 0)
            {
                objLineItem.AccountId = null;
                objLineItem.AccountClientLookupId = "";
            }
            else
            {
                objLineItem.AccountId = purchaseRequestlineitem.AccountId;
                objLineItem.AccountClientLookupId = purchaseRequestlineitem.Account_ClientLookupId;
            }
            objLineItem.IsPunchOut = purchaseRequestlineitem.Ispunchout;
            objLineItem.PurchaseUOM = purchaseRequestlineitem.PurchaseUOM;
            objLineItem.UOMConvRequired = purchaseRequestlineitem.UOMConvRequired;
            objLineItem.UOMConversion = purchaseRequestlineitem.UOMConversion;
            if (purchaseRequestlineitem.RequiredDate.HasValue && purchaseRequestlineitem.RequiredDate.Value.ToShortDateString() != DateTime.MinValue.ToShortDateString())
            {
                objLineItem.RequiredDate = purchaseRequestlineitem.RequiredDate;
            }
            objLineItem.SupplierPartId = purchaseRequestlineitem.SupplierPartId;
            objLineItem.SupplierPartAuxiliaryId = purchaseRequestlineitem.SupplierPartAuxiliaryId;
            objLineItem.ManufacturerPartId = purchaseRequestlineitem.ManufacturerPartId;
            objLineItem.Manufacturer = purchaseRequestlineitem.Manufacturer;
            objLineItem.VendorCatalogItemId = purchaseRequestlineitem.VendorCatalogItemId;
            if (purchaseRequestlineitem.UNSPSC == 0)
            {
                objLineItem.UNSPSC = null;
                objLineItem.PartCategoryMasterClientLookupId = "";
            }
            else
            {
                objLineItem.UNSPSC = purchaseRequestlineitem.UNSPSC;
                objLineItem.PartCategoryMasterClientLookupId = purchaseRequestlineitem.PartCategoryMasterClientLookupId;
            }
            return objLineItem;
        }
        private EditPRLineItemPartInInventorySingleStockModelDynamic MapPRLineUDFPartInInventoryDataForSingleStockEdit(EditPRLineItemPartInInventorySingleStockModelDynamic ediPRLineItemModel, PRLineUDF pRLineUDF)
        {
            if (pRLineUDF != null)
            {
                ediPRLineItemModel.PRLineUDFId = pRLineUDF.PRLineUDFId;

                ediPRLineItemModel.Text1 = pRLineUDF.Text1;
                ediPRLineItemModel.Text2 = pRLineUDF.Text2;
                ediPRLineItemModel.Text3 = pRLineUDF.Text3;
                ediPRLineItemModel.Text4 = pRLineUDF.Text4;

                if (pRLineUDF.Date1 != null && pRLineUDF.Date1 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date1 = null;
                }
                else
                {
                    ediPRLineItemModel.Date1 = pRLineUDF.Date1;
                }
                if (pRLineUDF.Date2 != null && pRLineUDF.Date2 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date2 = null;
                }
                else
                {
                    ediPRLineItemModel.Date2 = pRLineUDF.Date2;
                }
                if (pRLineUDF.Date3 != null && pRLineUDF.Date3 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date3 = null;
                }
                else
                {
                    ediPRLineItemModel.Date3 = pRLineUDF.Date3;
                }
                if (pRLineUDF.Date4 != null && pRLineUDF.Date4 == DateTime.MinValue)
                {
                    ediPRLineItemModel.Date4 = null;
                }
                else
                {
                    ediPRLineItemModel.Date4 = pRLineUDF.Date4;
                }

                ediPRLineItemModel.Bit1 = pRLineUDF.Bit1;
                ediPRLineItemModel.Bit2 = pRLineUDF.Bit2;
                ediPRLineItemModel.Bit3 = pRLineUDF.Bit3;
                ediPRLineItemModel.Bit4 = pRLineUDF.Bit4;

                ediPRLineItemModel.Numeric1 = pRLineUDF.Numeric1;
                ediPRLineItemModel.Numeric2 = pRLineUDF.Numeric2;
                ediPRLineItemModel.Numeric3 = pRLineUDF.Numeric3;
                ediPRLineItemModel.Numeric4 = pRLineUDF.Numeric4;

                ediPRLineItemModel.Select1 = pRLineUDF.Select1;
                ediPRLineItemModel.Select2 = pRLineUDF.Select2;
                ediPRLineItemModel.Select3 = pRLineUDF.Select3;
                ediPRLineItemModel.Select4 = pRLineUDF.Select4;
            }
            return ediPRLineItemModel;
        }
        internal PurchaseRequestLineItem UpdatePRPartInInventorySingleStockDynamic(PurchaseRequestVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            PurchaseRequestLineItem prLineItem = new PurchaseRequestLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestLineItemId = objVM.EditPRLineItemPartInInventorySingleStock.PurchaseRequestLineItemId ?? 0
            };
            prLineItem.Retrieve(_dbKey);
            // RKL-MAIL-Label Printing from Receipts
            if (prLineItem.PartId != objVM.EditPRLineItemPartInInventorySingleStock.PartId)
            {
                prLineItem.PartStoreroomId = objVM.EditPRLineItemPartInInventorySingleStock.PartStoreroomId??0;
            }
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequestLineItemStockSingle, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditPRLineItemPartInInventorySingleStock);
                getpropertyInfo = objVM.EditPRLineItemPartInInventorySingleStock.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditPRLineItemPartInInventorySingleStock);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prLineItem.GetType().GetProperty(item.ColumnName);

                if (item.ColumnName == "RequiredDate")
                {
                    setpropertyInfo.SetValue(prLineItem, val);
                    continue;
                }

                setpropertyInfo.SetValue(prLineItem, val);
            }
            string status = objVM.EditPRLineItemPartInInventorySingleStock.Status ?? string.Empty;
            string OldChargeType = prLineItem.ChargeType ?? string.Empty;
            long OldChargeToId = prLineItem.ChargeToID;
            string NewChargeType = objVM.EditPRLineItemPartInInventorySingleStock.ChargeType ?? string.Empty;
            long NewChargeToId = objVM.EditPRLineItemPartInInventorySingleStock.ChargeToId ?? 0;

            prLineItem.UpdateByPKForeignKeys(this.userData.DatabaseKey);

            if (status == PurchaseRequestStatusConstants.Open || status == PurchaseRequestStatusConstants.Approved || status == PurchaseRequestStatusConstants.Resubmit || status == PurchaseRequestStatusConstants.AwaitApproval || status == PurchaseRequestStatusConstants.Extracted)
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
                errors = UpdatePRLineUDFPartInInventorySigleStockDynamic(objVM.EditPRLineItemPartInInventorySingleStock, configDetails);

            }
            if (errors != null && errors.Count() > 0)
            {
                prLineItem.ErrorMessages.AddRange(errors);
            }
            return prLineItem;
        }
        public List<string> UpdatePRLineUDFPartInInventorySigleStockDynamic(EditPRLineItemPartInInventorySingleStockModelDynamic prLineItem,
  List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PRLineUDF prLineUDF = new PRLineUDF();
            prLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            prLineUDF.PurchaseRequestLineItemId = prLineItem.PurchaseRequestLineItemId ?? 0;

            prLineUDF = prLineUDF.RetrieveByPurchaseRequestLineItemId(userData.DatabaseKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, prLineItem);
                getpropertyInfo = prLineItem.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(prLineItem);
                Type t = getpropertyInfo.PropertyType;
                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = prLineUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(prLineUDF, val);
            }
            if (prLineUDF.PurchaseRequestLineItemId > 0)
            {
                prLineUDF.Update(_dbKey);
            }
            else
            {
                prLineUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                prLineUDF.PurchaseRequestLineItemId = prLineItem.PurchaseRequestLineItemId ?? 0;
                prLineUDF.Create(_dbKey);
            }

            return prLineUDF.ErrorMessages;
        }
        #endregion
        #endregion

        #region V2-1063
        public List<LineItemModel> LineitemsChunkSearchForMaterialRequest(long PurchaseRequestId, int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", string Description = "", string PartClientLookupId = "", string WorkOrderClientLookupId = "")
        {
            LineItemModel objLineItem;
            List<LineItemModel> LineItemList = new List<LineItemModel>();

            PurchaseRequestLineItem purchaseRequestLineItem = new PurchaseRequestLineItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PurchaseRequestId = PurchaseRequestId
            };
            purchaseRequestLineItem.OffSetVal = skip;
            purchaseRequestLineItem.NextRow = length;
            purchaseRequestLineItem.OrderBy = orderDir;
            purchaseRequestLineItem.OrderbyColumn = orderbycol;
            purchaseRequestLineItem.Description = Description;
            purchaseRequestLineItem.PartClientLookupId = PartClientLookupId;
            purchaseRequestLineItem.WorkOrderClientLookupId = WorkOrderClientLookupId;
            List<PurchaseRequestLineItem> purchaseRequestLintItemList = purchaseRequestLineItem.LineItemRetrieveForMaterialRequest(this.userData.DatabaseKey);

            if (purchaseRequestLintItemList != null)
            {
                foreach (var item in purchaseRequestLintItemList)
                {
                    objLineItem = new LineItemModel();
                    objLineItem.EstimatedCostsId = item.EstimatedCostsId;
                    objLineItem.PartClientLookupId = item.PartClientLookupId;
                    objLineItem.Description = item.Description;
                    objLineItem.OrderQuantity = Math.Round(item.OrderQuantity, 2);
                    objLineItem.UnitCost = Math.Round(item.UnitCost, 2);
                    objLineItem.UnitCostQuantity = Math.Round(item.UnitCostQuantity, 2);
                    objLineItem.WorkOrderClientLookupId = item.WorkOrderClientLookupId;
                    objLineItem.TotalCount = item.TotalCount;
                    LineItemList.Add(objLineItem);
                }
            }

            return LineItemList;
        }

        public List<string> PRLineItemMaterialRequestProcess(List<int> estimatedCostIds, long PurchaseRequestId)
        {
            PurchaseRequestLineItem purchaseRequestLineitem = new PurchaseRequestLineItem()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId
            };
            foreach (int EId in estimatedCostIds)
            {
                EstimatedCosts ec = new EstimatedCosts()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EstimatedCostsId = EId,
                };
                ec.Retrieve(userData.DatabaseKey);

                //Create purchaseRequestLineItem
                purchaseRequestLineitem = new PurchaseRequestLineItem();
                purchaseRequestLineitem.ClientId = _dbKey.Client.ClientId;
                purchaseRequestLineitem.PurchaseRequestId = PurchaseRequestId;
                purchaseRequestLineitem.AccountId = ec.AccountId;
                purchaseRequestLineitem.ChargeToID = ec.ObjectId;
                purchaseRequestLineitem.ChargeType = "WorkOrder";
                purchaseRequestLineitem.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                purchaseRequestLineitem.Description = ec.Description;
                purchaseRequestLineitem.RequiredDate = DateTime.MinValue;
                purchaseRequestLineitem.PartId = ec.CategoryId;
                purchaseRequestLineitem.PartStoreroomId = ec.PartStoreroomId;
                purchaseRequestLineitem.OrderQuantity = ec.Quantity;
                purchaseRequestLineitem.UnitofMeasure = ec.UnitOfMeasure;
                purchaseRequestLineitem.UnitCost = ec.UnitCost;
                purchaseRequestLineitem.UNSPSC = ec.UNSPSC;
                purchaseRequestLineitem.EstimatedCostsId = ec.EstimatedCostsId;
                purchaseRequestLineitem.Create(this.userData.DatabaseKey);

                if (purchaseRequestLineitem.ErrorMessages == null)
                {
                    purchaseRequestLineitem.PRReOrderLineNumber(userData.DatabaseKey);
                    //Update EstimatedCost
                    ec.Status = MaterialRequestLineStatus.OnOrder;
                    ec.PurchaseRequestId = purchaseRequestLineitem.PurchaseRequestId;
                    ec.PurchaseRequestLineItemId = purchaseRequestLineitem.PurchaseRequestLineItemId;
                    ec.Update(userData.DatabaseKey);
                }
            }
            return purchaseRequestLineitem.ErrorMessages;
        }
        #endregion

        #region V2-1112 CustomEPMConvertPRToPO
        public Tuple<ConvertToPOModel, bool, bool> ConvertCustomEPMPRToPO(PurchaseRequestVM model)
        {
            string newClientlookupId = string.Empty;
            ConvertToPOModel PR = new ConvertToPOModel();
            String message = string.Empty;
            ConvertToPOModel objPR;
            SiteSetUpWrapper sWrapper = new SiteSetUpWrapper(userData);
            string POPrefix = model.addCustomPurchaseOrder.Initials;
            string POSuffix = model.addCustomPurchaseOrder.ShiptoSuffix;
            // Only need to check if Site.VendorCompliance = True
            bool VendorInsuranceChecking = false;
            bool VendorAssetMgtChecking = false;

            PurchaseRequest purchaseRequestDetailsRetrieve = new PurchaseRequest
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                PurchaseRequestId = model.purchaseRequestModel.PurchaseRequestId
            };
            purchaseRequestDetailsRetrieve.Retrieve(_dbKey);
            Vendor vendor = new Vendor
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = purchaseRequestDetailsRetrieve.VendorId
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

            objPR = new ConvertToPOModel();
            if (true)//  UserData.ClientUIConfiguration.AutoGeneratedIdSettings.PurchaseOrder_AutoGenerateEnabled in VI true--for new Purchase order make a bool constant = true
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey
                        , AutoGenerateKey.EPM_PO_ANNUAL
                        , userData.DatabaseKey.User.DefaultSiteId
                       , POPrefix,POSuffix);
            }
            PurchaseRequest pr = new PurchaseRequest();
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.ClientLookupId = newClientlookupId;
            pr.SiteId = userData.DatabaseKey.Personnel.SiteId;
            pr.Status = PurchaseOrderStatusConstants.Open;
            pr.PurchaseRequestId = model.purchaseRequestModel.PurchaseRequestId ;
            pr.PurchaseRequestConvertV2(userData.DatabaseKey);
            List<PurchaseRequest> prlist = new List<PurchaseRequest>();
            PurchaseRequest prq = new PurchaseRequest();
            prq.ClientId = userData.DatabaseKey.Client.ClientId;
            prq.SiteId = userData.DatabaseKey.Personnel.SiteId;
            prq.PurchaseOrderId = pr.PurchaseOrderId;
            prlist = prq.RetrieveForInformation(userData.DatabaseKey);
            
            long PRID = prlist != null && prlist.Count > 0 ? prlist[0].PurchaseRequestId : 0;

            objPR.Message = pr.PurchaseOrderId != 0 ? JsonReturnEnum.success.ToString() : JsonReturnEnum.failed.ToString();
            objPR.ClientLookupId = prlist[0].ClientLookupId;
            // Change to make tablename = 'PurchaseOrder'
            CreateEventLog(pr.PurchaseOrderId, PurchasingEvents.POCreate, PRID);
            if (!pr.AutoGenerated)
            {
                var prList = new List<long> { pr.PurchaseRequestId };
                new ProcessAlert(userData).CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestConverted, prList);
            }
            return new Tuple<ConvertToPOModel, bool, bool>(PR, VendorInsuranceChecking, VendorAssetMgtChecking);
        }
        #endregion
    }
}