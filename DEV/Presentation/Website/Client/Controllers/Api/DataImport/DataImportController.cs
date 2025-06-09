using Business.Authentication;
using Common.Enumerations;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Client.Controllers.Api.DataImport
{
    public class DataImportController : ApiController
    {
        public UserData UserData { get; set; }
        #region Equipment ImportDatabaseKey
        [HttpPost]
        [Route("api/DataImport/ImportEquipment")]
        public HttpResponseMessage ImportEquipment([FromUri] string LoginSessionId, [FromUri]string Filename, object Jdata)
        {
            String ErrMsg = String.Empty;
            if (LoginSessionId == null)
            {
                return Request.CreateErrorResponse((HttpStatusCode)422, "LoginSessionId Cannot be null");
            }
            string Response = string.Empty;
            string Error = string.Empty;
            int Transactions = 0;
            int Success = 0;

            if (CheckLoginSession(LoginSessionId) == true)
            {
                EquipmentFlatModel fm = new EquipmentFlatModel();
                DataImportLog dlog = new DataImportLog();
                EquipmentImportLog elog = new EquipmentImportLog();
                var fm1 = JsonConvert.DeserializeObject<List<EquipmentFlatModel>>(JsonConvert.SerializeObject(Jdata));
                string pattern = "^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$";
                List<EquipmentFlatModel> itemFiltered = new List<EquipmentFlatModel>();
                foreach (var item in fm1)
                {
                    System.Text.RegularExpressions.Match result = System.Text.RegularExpressions.Regex.Match(item.ClientLookupId, pattern);
                    if (result.Success)
                    {
                        itemFiltered.Add(item);
                    }
                }
                DataContracts.Equipment eq = new DataContracts.Equipment();
                List<EquipmentErrorModel> erList = new List<EquipmentErrorModel>();
                EquipmentErrorModel er = new EquipmentErrorModel();
                foreach (var obj in itemFiltered)
                {
                    er = new EquipmentErrorModel();
                    eq.Clear();
                    eq.ClientId = UserData.DatabaseKey.Client.ClientId;
                    eq.SiteId = UserData.DatabaseKey.User.DefaultSiteId;
                    eq.ClientLookupId = obj.ClientLookupId;
                    eq.Name = obj.Name;
                    eq.Location = obj.Location;
                    eq.Make = obj.Make;
                    eq.Model = obj.Model;
                    eq.SerialNumber = obj.SerialNumber;
                    eq.Type = obj.Type;
                    eq.AssetNumber = obj.AssetNumber;
                    eq.AssetCategory = obj.AssetCategory;                   // V2-356
                    eq.CheckDuplicateEquipment(UserData.DatabaseKey);
                    eq.CheckAssetCategory();
                    Transactions += 1;
                    if (eq.ErrorMessages != null && eq.ErrorMessages.Count > 0)
                    {

                        er.ClientLookupId = obj.ClientLookupId;
                        //er.Error = "Error in Import: " + eq.ErrorMessages.FirstOrDefault().Replace("Unable to add new Equipment.", "");
                        er.Error = "Error in Import: " + string.Join(",", eq.ErrorMessages).Replace("Unable to add new Equipment.", "");
                        erList.Add(er);
                        //Error += eq.ErrorMessages.FirstOrDefault() + "<br/>";
                        Error += string.Join(",", eq.ErrorMessages) + "<br/>";
                    }
                }
                if (erList != null && erList.Count > 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(erList));
                }
                else if (erList == null || erList.Count == 0)
                {
                    List<EquipmentFlatModel> efList = new List<EquipmentFlatModel>();
                    foreach (var obj in itemFiltered)
                    {
                        EquipmentFlatModel ef = new EquipmentFlatModel();
                        eq.Clear();
                        eq.ClientId = UserData.DatabaseKey.Client.ClientId;
                        eq.SiteId = UserData.DatabaseKey.User.DefaultSiteId;
                        eq.ClientLookupId = obj.ClientLookupId;
                        eq.Name = obj.Name;
                        eq.Location = obj.Location;
                        eq.Make = obj.Make;
                        eq.Model = obj.Model;
                        eq.SerialNumber = obj.SerialNumber;
                        eq.Type = obj.Type;
                        eq.AssetNumber = obj.AssetNumber;
                        eq.AssetCategory = obj.AssetCategory;               // V2-356
                        eq.ImportEquipment(UserData.DatabaseKey);
                        Success += 1;
                        ef.EquipmentId = eq.EquipmentId;
                        ef.ClientLookupId = eq.ClientLookupId;
                        ef.AssetNumber = eq.AssetNumber;
                        ef.AssetCategory = eq.AssetCategory;
                        ef.Location = eq.Location;
                        ef.Make = eq.Make;
                        ef.Model = eq.Model;
                        ef.Name = eq.Name;
                        ef.SerialNumber = eq.SerialNumber;
                        ef.Type = eq.Type;
                        efList.Add(ef);
                        eq.EquipmentId = 0;
                    }
                    dlog.ClientId = UserData.DatabaseKey.Client.ClientId;
                    dlog.SiteId = UserData.DatabaseKey.User.DefaultSiteId;
                    dlog.RunBy_PersonnelId = UserData.DatabaseKey.Personnel.PersonnelId;
                    dlog.Type = "Equipment";
                    dlog.SuccessfulTransactions = Success;
                    dlog.Filename = Filename;
                    dlog.RunDate = DateTime.UtcNow;
                    dlog.Create(UserData.DatabaseKey);
                    foreach (var obj in efList)
                    {
                        elog.Clear();
                        elog.ClientId = UserData.DatabaseKey.Client.ClientId;
                        elog.SiteId = UserData.DatabaseKey.User.DefaultSiteId;
                        elog.DataImportLogId = dlog.DataImportLogId;
                        elog.EquipmentId = obj.EquipmentId;
                        elog.ClientLookupId = obj.ClientLookupId;
                        elog.Location = obj.Location;
                        elog.Make = obj.Make;
                        elog.Model = obj.Model;
                        elog.Name = obj.Name;
                        elog.SerialNumber = obj.SerialNumber;
                        elog.Type = obj.Type;
                        elog.AssetNumber = obj.AssetNumber;
                        elog.Create(UserData.DatabaseKey);
                        //elog.EquipmentImportLogId = 0;
                    }
                    Response = string.Format("Successfully added {0} Equipment records", Success);
                    erList = new List<EquipmentErrorModel>();
                    er = new EquipmentErrorModel();
                    er.Error = Response;
                    erList.Add(er);
                    return Request.CreateErrorResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(erList));
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.OK, Response);
        }
        #endregion

        #region Part Import
        public HttpResponseMessage ImportPart([FromUri] string LoginSessionId, [FromUri]string Filename, object Jdata)
        {
            String ErrMsg = String.Empty;
            if (LoginSessionId == null)
            {
                return Request.CreateErrorResponse((HttpStatusCode)422, "LoginSessionId Cannot be null");
            }
            string Response = string.Empty;
            string Error = string.Empty;
            int Transactions = 0;
            int Success = 0;
            if (CheckLoginSession(LoginSessionId) == true)
            {
                PartFlatModel fm = new PartFlatModel();
                DataImportLog dlog = new DataImportLog();
                PartImportLog plog = new PartImportLog();
                var fm1 = JsonConvert.DeserializeObject<List<PartFlatModel>>(JsonConvert.SerializeObject(Jdata));
                DataContracts.Part prt = new DataContracts.Part();
                DataContracts.PartStoreroom ps = new DataContracts.PartStoreroom();
                List<PartErrorModel> erList = new List<PartErrorModel>();
                PartErrorModel er = new PartErrorModel();
                foreach (var obj in fm1)
                {
                    er = new PartErrorModel();
                    prt.Clear();
                    prt.ClientId = UserData.DatabaseKey.Client.ClientId;
                    prt.SiteId = UserData.DatabaseKey.User.DefaultSiteId;
                    prt.ClientLookupId = obj.ClientLookupId;
                    prt.Description = obj.Description;
                    prt.Manufacturer = obj.Manufacturer;
                    prt.ManufacturerId = obj.ManufacturerId;
                    prt.AverageCost = obj.AverageCost;
                    prt.QtyOnHand = obj.QtyOnHand;
                    prt.QtyMaximum = obj.QtyMaximum;
                    prt.QtyReorderLevel = obj.QtyReorderLevel;
                    prt.ValidateAdd(UserData.DatabaseKey);
                    //prt.PartId = 0;
                    Transactions += 1;
                    if (prt.ErrorMessages != null && prt.ErrorMessages.Count > 0)
                    {

                        er.ClientLookupId = obj.ClientLookupId;
                        er.Error = "Error in Import: " + string.Format(prt.ErrorMessages.FirstOrDefault().ToString(), prt.ClientLookupId);
                        erList.Add(er);
                        Error += prt.ErrorMessages.FirstOrDefault() + "<br/>";
                    }
                }
                if (erList != null && erList.Count > 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(erList));
                }
                else if (erList == null || erList.Count == 0)
                {
                    List<PartFlatModel> efList = new List<PartFlatModel>();
                    foreach (var obj in fm1)
                    {
                        PartFlatModel pf = new PartFlatModel();
                        prt.Clear();
                        prt.ClientId = UserData.DatabaseKey.Client.ClientId;
                        prt.SiteId = UserData.DatabaseKey.User.DefaultSiteId;
                        prt.ClientLookupId = obj.ClientLookupId;
                        prt.Description = obj.Description;
                        prt.Manufacturer = obj.Manufacturer;
                        prt.ManufacturerId = obj.ManufacturerId;
                        prt.AverageCost = obj.AverageCost;
                        prt.Create(UserData.DatabaseKey);
                        Success += 1;
                        ps.Clear();
                        ps.ClientId = UserData.DatabaseKey.Client.ClientId;
                        ps.SiteId = UserData.DatabaseKey.User.DefaultSiteId;
                        ps.PartId = prt.PartId;
                        ps.QtyOnHand = obj.QtyOnHand;
                        ps.QtyMaximum = obj.QtyMaximum;
                        ps.QtyReorderLevel = obj.QtyReorderLevel;
                        ps.Create(UserData.DatabaseKey);
                        pf.PartId = prt.PartId;
                        pf.ClientLookupId = prt.ClientLookupId;
                        pf.Description = prt.Description;
                        pf.Manufacturer = prt.Manufacturer;
                        pf.ManufacturerId = prt.ManufacturerId;
                        pf.AverageCost = prt.AverageCost;
                        pf.QtyOnHand = prt.QtyOnHand;
                        pf.QtyMaximum = prt.QtyMaximum;
                        pf.QtyReorderLevel = prt.QtyReorderLevel;
                        efList.Add(pf);
                        //prt.PartId = 0;
                        //ps.PartStoreroomId = 0;
                    }
                    dlog.ClientId = UserData.DatabaseKey.Client.ClientId;
                    dlog.SiteId = UserData.DatabaseKey.User.DefaultSiteId;
                    dlog.RunBy_PersonnelId = UserData.DatabaseKey.Personnel.PersonnelId;
                    dlog.Type = "Part";
                    dlog.SuccessfulTransactions = Success;
                    dlog.Filename = Filename;
                    dlog.RunDate = DateTime.UtcNow;
                    dlog.Create(UserData.DatabaseKey);
                    foreach (var obj in efList)
                    {
                        plog.Clear();
                        plog.ClientId = UserData.DatabaseKey.Client.ClientId;
                        plog.SiteId = UserData.DatabaseKey.User.DefaultSiteId;
                        plog.DataImportLogId = dlog.DataImportLogId;
                        plog.PartId = obj.PartId;
                        plog.ClientLookupId = obj.ClientLookupId;
                        plog.Description = obj.Description;
                        plog.Manufacturer = obj.Manufacturer;
                        plog.ManufacturerId = obj.ManufacturerId;
                        plog.AverageCost = obj.AverageCost;
                        plog.QtyOnHand = obj.QtyOnHand;
                        plog.QtyMaximum = obj.QtyMaximum;
                        plog.QtyReorderLevel = obj.QtyReorderLevel;
                        plog.Create(UserData.DatabaseKey);
                        //plog.PartImportLogId = 0;
                    }
                    Response = string.Format("Successfully added {0} Part records", Success);
                    erList = new List<PartErrorModel>();
                    er = new PartErrorModel();
                    er.Error = Response;
                    erList.Add(er);
                    return Request.CreateErrorResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(erList));
                }
            }


            return Request.CreateErrorResponse(HttpStatusCode.OK, Response);
        }
        #endregion

        #region Authentication       
        private bool CheckAuthentication(string userName, string password)
        {
            Authentication auth = new Authentication()
            {
                UserName = userName,
                Password = password,
                website = WebSiteEnum.Service,
                BrowserInfo = HttpContext.Current.Request.Browser.Type + " " + HttpContext.Current.Request.Browser.Version,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };
            auth.VerifyLogin();
            if (auth.IsAuthenticated)
            {
                DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
                this.UserData = new UserData() { SessionId = auth.SessionId, WebSite = WebSiteEnum.Client };
                this.UserData.Retrieve(dbKey);

            }
            return auth.IsAuthenticated;
        }
        private bool CheckLoginSession(string LoginSessionID)
        {

            Guid LogsessionId = Guid.Empty;
            LogsessionId = new Guid(LoginSessionID);
            DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            this.UserData = new UserData() { SessionId = LogsessionId, WebSite = WebSiteEnum.Client };
            this.UserData.Retrieve(dbKey);

            Authentication auth = new Authentication() { UserData = this.UserData };
            auth.UserData.LoginAuditing.CreateDate = DateTime.Now;
            auth.VerifyCurrentUser();
            return auth.IsAuthenticated;
        }
        #endregion
    }

    #region New Model Class
    #region EquipmentModel
    [Serializable]
    public class EquipmentFlatModel
    {
        [JsonProperty]
        public string ClientLookupId { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string Location { get; set; }
        [JsonProperty]
        public string Make { get; set; }
        [JsonProperty]
        public string Model { get; set; }
        [JsonProperty]
        public string SerialNumber { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]                                // V2-356
        public string AssetCategory { get; set; }     // V2-356
        [JsonProperty]
        public string AssetNumber { get; set; }
        [JsonProperty]
        public Int64 EquipmentId { get; set; }
        [JsonProperty]
        public List<EquipmentFlatModel> EquipmentFlatList { get; set; }
    }
    public class EquipmentErrorModel
    {
        [JsonProperty]
        public string EquipmentId { get; set; }
        [JsonProperty]
        public string ClientLookupId { get; set; }
        [JsonProperty]
        public string Error { get; set; }

        public List<EquipmentErrorModel> EquipmentErrorList { get; set; }
    }

    #endregion

    #region PartModel
    public class PartFlatModel
    {
        [JsonProperty]
        public string ClientLookupId { get; set; }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public string Manufacturer { get; set; }
        [JsonProperty]
        public string ManufacturerId { get; set; }
        [JsonProperty]
        public decimal AverageCost { get; set; }
        [JsonProperty]
        public decimal QtyOnHand { get; set; }
        [JsonProperty]
        public decimal QtyMaximum { get; set; }
        [JsonProperty]
        public decimal QtyReorderLevel { get; set; }
        [JsonProperty]
        public Int64 PartId { get; set; }
        [JsonProperty]
        public List<PartFlatModel> PartFlatList { get; set; }
    }
    public class PartErrorModel
    {
        [JsonProperty]
        public string PartId { get; set; }
        [JsonProperty]
        public string ClientLookupId { get; set; }
        [JsonProperty]
        public string Error { get; set; }

        public List<EquipmentErrorModel> PartErrorList { get; set; }
    }
    #endregion
    #endregion
}