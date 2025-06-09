using Client.Models.PurchaseApproval;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using Client.BusinessWrapper.Common;
using System.Web;
using System.Text;

namespace Client.BusinessWrapper.PurchaseApproval
{
    public class PurchaseApprovalWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public PurchaseApprovalWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public List<PRLineItemModel> GetLineItems(long RequestId)
        {
            PRLineItemModel PLineModel;
            List<PRLineItemModel> PAModelList = new List<PRLineItemModel>();
            PurchaseRequestLineItem pRLineItem = new PurchaseRequestLineItem
            {
                PurchaseRequestId = RequestId,
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            List<PurchaseRequestLineItem> PRList = pRLineItem.RetrieveByPurchaseOrderLineItemId(userData.DatabaseKey);
            foreach (var p in PRList)
            {
                PLineModel = new PRLineItemModel();
                PLineModel.PurchaseRequestId = p.PurchaseRequestId;
                PLineModel.LineNumber = p.LineNumber;
                PLineModel.PartClientLookupId = p.PartClientLookupId;
                PLineModel.Description = p.Description;
                PLineModel.OrderQuantity = p.OrderQuantity;
                PLineModel.UnitofMeasure = p.UnitofMeasure;
                PLineModel.UnitCost = p.UnitCost;
                PLineModel.TotalCost = p.TotalCost;
                PLineModel.Account_ClientLookupId = p.Account_ClientLookupId;
                PLineModel.PartId = p.PartId;
                PAModelList.Add(PLineModel);
            }
            return PAModelList;
        }
        public List<PurchaseApprovalModel> RetrievePurchaseApprovalData(int StatusTypeId, int CreatedDateId)
        {
            PurchaseRequest pr = new PurchaseRequest()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                StatusDrop = StatusTypeId.ToString(),
                Created = CreatedDateId.ToString(),
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId
            };
            PurchaseApprovalModel PAModel;
            List<PurchaseApprovalModel> PAModelList = new List<PurchaseApprovalModel>();
            List<PurchaseRequest> PRList = pr.RetrieveWorkBenchForSearchNew(userData.DatabaseKey, userData.Site.TimeZone);
            foreach (var p in PRList)
            {
                PAModel = new PurchaseApprovalModel();
                PAModel.PurchaseRequestId = p.PurchaseRequestId;
                PAModel.PRClientLookupId = p.PRClientLookupId;
                PAModel.Reason = p.Reason;
                PAModel.CreatedBy = p.CreatedBy;
                PAModel.CreateDate = p.CreateDate;
                PAModel.VendorClientLookupId = p.VendorClientLookupId;
                PAModel.VendorName = p.VendorName;
                PAModel.TotalCost = p.TotalCost;
                PAModelList.Add(PAModel);
            }
            return PAModelList;
        }
        public bool UpdateApprovalList(List<long> list)
        {
            bool valid = false;
            List<long> PRlist = new List<long>();
            foreach (var item in list)
            {
                PurchaseRequest pr = new PurchaseRequest();
                long prid = Convert.ToInt64(item);
                pr.PurchaseRequestId = prid;
                pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                pr.SiteId = userData.Site.SiteId;
                pr.Status = PurchaseRequestStatusConstants.Approved;
                pr.UpdateIndex = pr.UpdateIndex;
                pr.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                pr.Flag = "1";
                try
                {
                    pr.UpdateByForeignKeys(userData.DatabaseKey);
                    PRlist.Add(prid);
                    CreateEventLog(pr.PurchaseRequestId, PurchasingEvents.Approved);
                    valid = true;
                }
                catch (Exception ex)
                {
                    valid = false;
                }

            }
            ProcessAlert objAlert = new ProcessAlert(userData);
            objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestApproved, PRlist);
            return valid;
        }
        private void CreateEventLog(Int64 objId, string Status)
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
            log.TableName= AttachmentTableConstant.PurchaseRequest;
            log.Create(userData.DatabaseKey);
        }
        private void CreateEventLog(Int64 objId, string Status, string Comments)
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
        public bool UpdateDenyList(List<long> list)
        {
            bool valid = false;
            List<long> PrId = new List<long>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            foreach (var item in list)
            {
                PurchaseRequest pr = new PurchaseRequest();
                long prid = Convert.ToInt64(item);
                pr.PurchaseRequestId = prid;
                pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                pr.SiteId = userData.Site.SiteId;
                pr.Status = PurchaseRequestStatusConstants.Denied;
                pr.UpdateIndex = pr.UpdateIndex;
                pr.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                pr.Flag = "2";
                try
                {
                    pr.UpdateByForeignKeys(userData.DatabaseKey);
                    PrId.Add(pr.PurchaseRequestId);
                    CreateEventLog(pr.PurchaseRequestId, PurchasingEvents.Denied);
                    if (pr.CountLineItem > 0)
                    {
                        Task task1 = Task.Factory.StartNew(() => commonWrapper.UpdateListPartsonOrder(pr.PurchaseRequestId, "Minus", AttachmentTableConstant.PurchaseRequest));
                       }
                    valid = true;
                }
                catch (Exception ex)
                {
                    valid = false;
                }
            }
            ProcessAlert objAlert = new ProcessAlert(this.userData);
            objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestDenied, PrId);
            return valid;
        }
        public bool UpdateReturnToRequesterList(string[] PurchaseRequestid, string ReturnComments)
        {
            List<long> PrId = new List<long>();
            bool valid = false;
            try
            {
                foreach (var Pid in PurchaseRequestid)
                {
                    PurchaseRequest pr = new PurchaseRequest()
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        PurchaseRequestId = Convert.ToInt64(Pid)
                    };
                    pr.Retrieve(userData.DatabaseKey);
                    pr.Return_Comments = ReturnComments;
                    pr.Status = PurchaseRequestStatusConstants.Resubmit;
                    pr.ProcessBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    pr.Processed_Date = System.DateTime.UtcNow;
                    pr.Update(userData.DatabaseKey);
                    PrId.Add(pr.PurchaseRequestId);
                    CreateEventLog(pr.PurchaseRequestId, PurchasingEvents.Resubmit, ReturnComments);
                    valid = true;
                }
                if (PrId.Count > 0)
                {
                    ProcessAlert objAlert = new ProcessAlert(this.userData);
                    objAlert.CreateAlert<PurchaseRequest>(AlertTypeEnum.PurchaseRequestReturned, PrId);
                }
            }
            catch (Exception ex)
            {
                valid = false;
            }

            return valid;
        }

        #region PurchaseExport

        public bool CheckIsActiveInterface()
        {
            bool IsActive = false;
            InterfaceProp iprop = new InterfaceProp();
            iprop.InterfaceType = ApiConstants.PurchaseRequestExport;
            iprop.ClientId = userData.DatabaseKey.Client.ClientId;
            iprop.CheckIsActive(userData.DatabaseKey);
            if (iprop.InterfacePropId > 0)
            {
                IsActive = true;
            }
            return IsActive;
        }

        public InterfaceProp RetrieveInterfaceProperties()
        {
            InterfaceProp iprop = new InterfaceProp();
            iprop.ClientId = userData.DatabaseKey.Client.ClientId;
            iprop.SiteId = userData.Site.SiteId;
            iprop.InterfaceType = ApiConstants.PurchaseRequestExport;
            iprop.RetrieveInterfaceProperties(userData.DatabaseKey);
            return iprop;
        }

        public List<PRExportModel_Coupa> GetApprovalList(List<long> list)
        {
            List<PRExportModel_Coupa> PrCoupa = new List<PRExportModel_Coupa>();
            List<PRLineExportModel_Coupa> PRLineCoupa = new List<PRLineExportModel_Coupa>();
            foreach (var item in list)
            {
                long prid = Convert.ToInt64(item);
                DateTime comp_date = new DateTime(2000, 1, 1);
                PurchaseRequest pr = new PurchaseRequest();
                pr.PurchaseRequestId = prid;
                pr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                pr.ClientId = userData.DatabaseKey.Client.ClientId;
                // Added this method to retrieve the data we need using the sp.
                // Need the following: 
                //  Coupa Header Information
                //    Approver's Email Address (UserInfo Record of the Approver)
                //    Ship To Address (Site Record)
                //    External Vendor ID (Vendor and Vendor Master)
                //  Coupa Detail Information (Retrieved from pr as they are "defaults" - doing this so the value can be changed without a code change)
                //    Terms Description
                //    Currency Code 
                //    Source Type
                //    Line Type
                //    Account Name 
                //    Commodity Code 
                pr.RetrieveForCoupaExport(userData.DatabaseKey,userData.Site.TimeZone);
                //pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                // RKL - We could change the RetrieveByPKForeignKeys to retrieve the External Vendor Id
                //Vendor ve = new Vendor() {ClientId=pr.ClientId,VendorId=pr.VendorId};
                //ve.Retrieve(userData.DatabaseKey);
                //VendorMaster vm = new VendorMaster() { ClientId = pr.ClientId, VendorMasterId = ve.VendorMasterId };
                //vm.Retrieve(userData.DatabaseKey);
                PurchaseRequestLineItem prline = new PurchaseRequestLineItem();
                prline.PurchaseRequestId = prid;
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
                  PRCoupa.NeedByDate = dto;//.ToString("O");
                }
                PRCoupa.ShipToAttention = pr.Creator_PersonnelName; //--to be chaecked from sp
                PRCoupa.ReceivingWarehouseId = null;
                PRCoupa.LineCount = prlinelist.Count;
                PRCoupa.TotalAmount = Math.Round(pr.TotalCost,2);  // (from x in prlinelist select x.OrderQuantity*x.UnitCost).Sum();  
                Coupa_PR_Header_CustomFields chf = new Coupa_PR_Header_CustomFields()
                { PRNumber = pr.ClientLookupId,
                  PurchaseRequestId =pr.PurchaseRequestId.ToString(),
                  ClientId =pr.ClientId.ToString(),
                  SiteId =pr.SiteId.ToString()
                };
                PRCoupa.CustomFields = chf;
                PRCoupa.Currency = new Coupa_Currency() { Code = pr.Currency_Code};
                PRCoupa.RequestedBy = new Coupa_RequestedBy() { EmailAddress = pr.EXUserId };
                //PRCoupa.RequestedBy = new Coupa_RequestedBy() { EmailAddress = "John_Burns@deanfoods.com" }; // MUST BE CHANGED
                PRCoupa.ShipTo = new Coupa_ShipToAddress() {ShipToName= pr.Ship_to_Code }; // Must be stored
                //PRCoupa.ShipTo = new Coupa_ShipToAddress() {ShipToName= "TX:Dallas-2711 N. Haskell Avenue (Dean Headquarters)" }; // Must be stored
                // Are these needed?
                //PRCoupa.approver = Convert.ToString(pr.ApprovedBy_PersonnelId) ?? "";
                //PRCoupa.createdby = Convert.ToString(pr.ApprovedBy_PersonnelId);
                //PRCoupa.requester = Convert.ToString(pr.ApprovedBy_PersonnelId) ?? "";
                //PRCoupa.shiptoaddress = pr.SiteAddress1 ?? "";//--to be verified
                //PRCoupa.somax_client_id = Convert.ToInt32(pr.ClientId) == 0 ? 0 : Convert.ToInt32(pr.ClientId);
                //PRCoupa.somax_site_id = Convert.ToInt32(pr.SiteId) == 0 ? 0 : Convert.ToInt32(pr.SiteId);
                //PRCoupa.somax_pr = pr.ClientLookupId ?? "";
                //PRCoupa.somax_document_id = pr.PurchaseRequestLineItemId;                
                //pr.Status = PurchaseRequestStatusConstants.Approved;
                //pr.UpdateIndex = pr.UpdateIndex;
                //pr.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                //pr.Flag = "1";
                // Requisitions Lines
                //---add to line--
                PRLineExportModel_Coupa prlinecoupa;

                foreach (var data in prlinelist)
                {
                    prlinecoupa = new PRLineExportModel_Coupa();
                    prlinecoupa.description = data.Description.Trim();
                    DateTimeOffset req_date;
                    if (data.RequiredDate == null || data.RequiredDate < comp_date)
                    {
                      req_date = DateTime.UtcNow.AddDays(7);
                      //prlinecoupa.NeedByDate = req_date.ToString("s");
                    }
                    else
                    {
                      req_date = data.RequiredDate ?? DateTime.UtcNow.AddDays(7);
                      //prlinecoupa.NeedByDate = req_date;//.ToString("O");
                    }
                    prlinecoupa.NeedByDate = req_date.ToString("s");
                    prlinecoupa.linenumber = data.LineNumber != 0 ? data.LineNumber : 0;
                    prlinecoupa.PartNumber = data.PartClientLookupId;
                    prlinecoupa.AuxPartNumber = null;
                    prlinecoupa.OrderQuantity = data.OrderQuantity;
                    prlinecoupa.TotalCost = Math.Round(data.TotalCost,2);  // data.UnitCost * data.OrderQuantity;
                    prlinecoupa.SourceType = pr.Source_Type;                // Required by Coupa?
                    prlinecoupa.LineType = pr.Line_Type;                    // Required by Coupa?
                    //prlinecoupa.SourceType = "Non-Catalog Request";       // Required by Coupa?
                    //prlinecoupa.LineType = "RequisitionQuantityLine";     // Required by Coupa?
                    prlinecoupa.UnitCost = data.UnitCost != 0 ? data.UnitCost : 0;
                    prlinecoupa.CustomFields = new Coupa_PR_Line_CustomFields() { NonTaxable = null, PurchaseRequestLineItemId = data.PurchaseRequestLineItemId.ToString() };
                    Coupa_Account acct = new Coupa_Account(data.Account_ClientLookupId);
                    Coupa_Account_Type acct_type = new Coupa_Account_Type() {Account_Name=pr.Acct_Name};
                    Coupa_Currency acct_curr = new Coupa_Currency() { Code=pr.Currency_Code};
                    acct_type.Account_Currency = acct_curr;
                    acct.Account_Type = acct_type;                    
                    prlinecoupa.Account = acct;
                    prlinecoupa.PaymentTerm = new Coupa_Payment_Terms() { TermsCode = pr.Terms_Desc};
                    prlinecoupa.Commodity = new Coupa_Commodity() { Name= pr.Commodity_Code };  // Required by Coupa? 
                    prlinecoupa.Currency = new Coupa_Currency() { Code = pr.Currency_Code};
                    prlinecoupa.Vendor = new Coupa_Vendor() {VendorNumber=pr.ExVendorId.ToString()};
                    //prlinecoupa.PaymentTerm = new Coupa_Payment_Terms() { Te
                    //prlinecoupa.Commodity = new Coupa_Commodity() { Name= "Industrial machinery components and accessories" };  // Required by Coupa? 
                    //prlinecoupa.Currency = new Coupa_Currency() { Code="USD"};
                    //prlinecoupa.Vendor = new Coupa_Vendor() { VendorName=pr.VendorName.Trim(), VendorNumber=vm.ExVendorId.ToString()};rmsCode = ve.Terms };
                    // RKL - Are these needed?                                         
                    //prlinecoupa.item = "";
                    //prlinecoupa.quantity = data.OrderQuantity != 0 ? data.OrderQuantity : 0;
                    //prlinecoupa.sourcepartnum = ;  // --might be the local (plant level) part number
                    //prlinecoupa.supplier = Convert.ToString(pr.VendorId) ?? "";
                    //prlinecoupa.uom = data.UnitofMeasure ?? "";
                    //prlinecoupa.PurchaseRequestLineItemId = Convert.ToString(data.PurchaseRequestLineItemId);
                    PRLineCoupa.Add(prlinecoupa);
                }
                PRCoupa.requisitionlines = PRLineCoupa;
                PrCoupa.Add(PRCoupa);

                //try
                //{
                // pr.UpdateByForeignKeys(userData.DatabaseKey); //----to be verified
                //CreateEventLog(pr.PurchaseRequestId, PurchasingEvents.Approved); //---not mentioned in spec

                //}
                //catch (Exception ex)
                //{
                //}

            }//end of foreach

            return PrCoupa;
        }
        public async Task CreateProduct(List<PRExportModel_Coupa> PrCoupa)
        {
            HttpClient client = new HttpClient();
            //------------------------
            //HttpResponseMessage response = await client.PostAsJsonAsync(
            //    $"https://deanfoods-test.coupahost.com/API", PrCoupa);
            //response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            //return response.Headers.Location;
            //return null;
        }
       
        public async Task<string> CallApiMethod(List<PRExportModel_Coupa> PrCoupa)
        {
            string str = string.Empty;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://deanfoods-test.coupahost.com/API");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var uri = "https://deanfoods-test.coupahost.com/API";
            //var response = await client.PostAsJsonAsync(uri, PrCoupa); 
           // string s = JsonConvert.SerializeObject(new { PrCoupa }, JsonSerializer12HoursDateAndTimeSettings);
            var response = await client.PostAsync(uri, null); 

            //HttpResponseMessage response = new HttpResponseMessage();
            //try
            //{
            //    response = await client.PostAsJsonAsync($"https://deanfoods-test.coupahost.com/API", PrCoupa);
            //    //response.EnsureSuccessStatusCode();
            //    // return URI of the created resource.
            //    str = Convert.ToString(response.Headers.Location) ?? "header unknown";
            //}
            //catch (Exception ex)
            //{
            //    str = ex.ToString();
            //}
            str = Convert.ToString(response);
            return str;
            
        }

        public List<string> UpdatePrStatus(long prId,string status)
        {
            PurchaseRequest pr = new PurchaseRequest();
            pr.PurchaseRequestId = prId;
            pr.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pr.ClientId = userData.DatabaseKey.Client.ClientId;
            pr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
            pr.Status = status;
            pr.Update(userData.DatabaseKey);
            return pr.ErrorMessages;
        }
        #endregion

        #region V2-820

        public List<PurchaseApprovalModel> RetrievePurchaseApprovalData_V2(int StatusTypeId, int CreatedDateId)
        {
            PurchaseRequest pr = new PurchaseRequest()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                StatusDrop = StatusTypeId.ToString(),
                Created = CreatedDateId.ToString(),
                PersonnelId = userData.DatabaseKey.Personnel.PersonnelId
            };
            PurchaseApprovalModel PAModel;
            List<PurchaseApprovalModel> PAModelList = new List<PurchaseApprovalModel>();
            //List<PurchaseRequest> PRList = pr.RetrieveWorkBenchForSearchNew(userData.DatabaseKey, userData.Site.TimeZone);
            List<PurchaseRequest> PRList = pr.RetrieveForPurchaseRequestWorkBenchRetrieveAll_V2(userData.DatabaseKey, userData.Site.TimeZone);
            foreach (var p in PRList)
            {
                PAModel = new PurchaseApprovalModel();
                PAModel.PurchaseRequestId = p.PurchaseRequestId;
                PAModel.PRClientLookupId = p.PRClientLookupId;
                PAModel.Reason = p.Reason;
                PAModel.CreatedBy = p.CreatedBy;
                PAModel.CreateDate = p.CreateDate;
                PAModel.BuyerReview = p.BuyerReview;
                PAModel.VendorClientLookupId = p.VendorClientLookupId;
                PAModel.VendorName = p.VendorName;
                PAModel.TotalCost = p.TotalCost;
                PAModelList.Add(PAModel);
            }
            return PAModelList;
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

  }
}