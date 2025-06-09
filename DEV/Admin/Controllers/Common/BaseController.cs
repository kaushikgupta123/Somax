using Admin.ActionFilters;
using Admin.BusinessWrapper;
using Admin.BusinessWrapper.Common;
using Admin.Common;
using Admin.Models;
using Admin.Models.Common;
using AzureUtil;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Utility;

namespace Admin.Controllers
{
    [CustomAuthorize(Roles = new string[] { "*" })]
    [SessionExpired]
    [NoCacheActionFilter]
    [SiteUnderMaintennce]
    [HandleError]
    public class BaseController : Controller
    {              
        #region Initialization
        internal UserData userData { get; set; }
        internal long objectId { get; set; }
        protected JsonSerializerSettings JsonSerializerDateSettings { get; set; }
        protected JsonSerializerSettings JsonSerializer24HoursDateAndTimeSettings { get; set; }
        protected JsonSerializerSettings JsonSerializer12HoursDateAndTimeSettings { get; set; }
        protected JsonSerializerSettings JsonSerializer12HoursDateAndTimeUptoMinuteSettings { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var SessionData = Session["AdminUserData"];
            if (SessionData != null)
            {
                userData = (UserData)Session["AdminUserData"];
            }
            if (Session[SessionConstants.OBJECT_ID] != null)
            {
                objectId = Convert.ToInt64(Session[SessionConstants.OBJECT_ID]);
                Session[SessionConstants.OBJECT_ID] = null;
            }
            else
            {
                objectId = objectId;
            }
            this.JsonSerializerDateSettings = new JsonSerializerSettings
            {
                DateFormatString = "MM/dd/yyyy"
            };
            this.JsonSerializer24HoursDateAndTimeSettings = new JsonSerializerSettings
            {
                DateFormatString = "MM/dd/yyyy HH:mm:ss"
            };

            this.JsonSerializer12HoursDateAndTimeSettings = new JsonSerializerSettings
            {
                DateFormatString = "MM/dd/yyyy hh:mm:ss tt"
            };
            this.JsonSerializer12HoursDateAndTimeUptoMinuteSettings = new JsonSerializerSettings
            {
                DateFormatString = "MM/dd/yyyy hh:mm tt"
            };
        }
        #endregion
        #region Localisations
        internal void LocalizeControls(LocalisationBaseVM objComb, string ResourceType)
        {
            LoginCacheSet _logCache = new LoginCacheSet();
            var connstring = ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString();
            //------------Retrieve by Specific Resource Type Localization--------------//
            List<Localizations> locSpecificPageCache = _logCache.GetLocalizationCommon(connstring, ResourceType, userData.Site.Localization);
            //------------Retrieve Global Localization--------------------//
            List<Localizations> locGlobalCache = _logCache.GetLocalizationCommon(connstring, LocalizeResourceSetConstants.Global, userData.Site.Localization);
            if (locSpecificPageCache != null && locSpecificPageCache.Count > 0)
            {
                objComb.Loc = locSpecificPageCache;
            }
            else
            {
                //------Retrieve "en-us" data if other lang fail----------//
                locSpecificPageCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), ResourceType, "en-us");
                objComb.Loc = locSpecificPageCache;
            }
            if (locGlobalCache != null && locGlobalCache.Count > 0)
            {
                objComb.Loc.AddRange(locGlobalCache);
            }
            else
            {
                //------Retrieve "en-us" data if other lang fail----------//
                locGlobalCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Global, "en-us");
                objComb.Loc.AddRange(locGlobalCache);
            }
        }
        public List<Localizations> MsgLocalizeDetailsBase(UserData userData)
        {
            LoginCacheSet _logCache = new LoginCacheSet();
            List<Localizations> jsAlertmsglist = new List<Localizations>();
            jsAlertmsglist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.JsAlerts, userData.Site.Localization);
            if (jsAlertmsglist != null && jsAlertmsglist.Count > 0)
            {
                return jsAlertmsglist;
            }
            else
            {
                jsAlertmsglist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.JsAlerts, "en-us");
                return jsAlertmsglist;
            }
        }
        public List<Localizations> MsgLocalizeStatusDetails(UserData userData)
        {
            LoginCacheSet _logCache = new LoginCacheSet();
            List<Localizations> jsAlertmsglist = new List<Localizations>();
            jsAlertmsglist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.StatusDetails, userData.Site.Localization);
            if (jsAlertmsglist != null && jsAlertmsglist.Count > 0)
            {
                return jsAlertmsglist;
            }
            else
            {
                jsAlertmsglist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.StatusDetails, "en-us");
                return jsAlertmsglist;
            }
        }
        public JsonResult GetDataTableLanguageJson(bool InnerGrid = false, bool nGrid = false)
        {
            DataTableLanguageRoot dtlanguageDetails = new DataTableLanguageRoot();
            string sLengthMenu = "<select class='searchdt-menu select2picker'>" +
                                              "<option value='10'>10</option>" +
                                              "<option value='20'>20</option>" +
                                              "<option value='30'>30</option>" +
                                              "<option value='40'>40</option>" +
                                              "<option value='50'>50</option>" +
                                              "</select>";

            dtlanguageDetails.sEmptyTable = UtilityFunction.GetMessageFromResource("sEmptyTable", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfo = UtilityFunction.GetMessageFromResource("sInfo", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoEmpty = UtilityFunction.GetMessageFromResource("sInfoEmpty", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoFiltered = UtilityFunction.GetMessageFromResource("sInfoFiltered", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoPostFix = UtilityFunction.GetMessageFromResource("sInfoPostFix", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoThousands = UtilityFunction.GetMessageFromResource("sInfoThousands", LocalizeResourceSetConstants.DataTableEntry);
            if (nGrid)
            {
                dtlanguageDetails.sLengthMenu = sLengthMenu;
            }
            else if (InnerGrid)
            {
                dtlanguageDetails.sLengthMenu = "<select class='innergrid-searchdt-menu form-control search'>" +
                                              "<option value='10'>10</option>" +
                                              "<option value='20'>20</option>" +
                                              "</select>";
            }
            else
            {
                dtlanguageDetails.sLengthMenu = "Page size :&nbsp;" + sLengthMenu;
            }
            dtlanguageDetails.sLoadingRecords = UtilityFunction.GetMessageFromResource("sLoadingRecords", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sProcessing = "<img src='../Content/Images/image_1197421.gif'>";
            dtlanguageDetails.sSearch = UtilityFunction.GetMessageFromResource("sSearch", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sZeroRecords = UtilityFunction.GetMessageFromResource("sZeroRecords", LocalizeResourceSetConstants.DataTableEntry);
            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sFirst = "<img src='../images/drop-grid-first.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sFirst = UtilityFunction.GetMessageFromResource("sFirst", LocalizeResourceSetConstants.DataTableEntry);
            }
            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sPrevious = "<img src='../images/drop-grid-prev.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sPrevious = UtilityFunction.GetMessageFromResource("sPrevious", LocalizeResourceSetConstants.DataTableEntry);
            }

            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sNext = "<img src='../images/drop-grid-next.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sNext = UtilityFunction.GetMessageFromResource("sNext", LocalizeResourceSetConstants.DataTableEntry);
            }
            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sLast = "<img src='../images/drop-grid-last.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sLast = UtilityFunction.GetMessageFromResource("sLast", LocalizeResourceSetConstants.DataTableEntry);
            }
            dtlanguageDetails.oAria.sSortAscending = UtilityFunction.GetMessageFromResource("sSortAscending", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.oAria.sSortDescending = UtilityFunction.GetMessageFromResource("sSortDescending", LocalizeResourceSetConstants.DataTableEntry);
            return Json(dtlanguageDetails, JsonRequestBehavior.AllowGet);
        }

        public List<Localizations> GetTimeZoneList()
        {
            LoginCacheSet _logCache = new LoginCacheSet();
            List<Localizations> objlist = new List<Localizations>();
            var connstring = ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString();
            objlist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.TimeZoneDetails, "en-us");
            return objlist;
        }
        #endregion       
        #region GridLayout
        public JsonResult CreateUpdateState(string GridName, string LayOutInfo, string FilterInfo = "")
        {
            GridStateWrapper gridStateWrapper = new GridStateWrapper(userData);
            gridStateWrapper.CreateUpdateState(GridName, LayOutInfo, FilterInfo);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetState(string GridName)
        {
            GridStateWrapper gridStateWrapper = new GridStateWrapper(userData);
            string currentState = gridStateWrapper.GetState(GridName);
            return Json(currentState, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLayout(string GridName)
        {
            GridStateWrapper gridStateWrapper = new GridStateWrapper(userData);
            var currentState = gridStateWrapper.GetLayout(GridName);
            return Json(currentState, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Common Text Search
        public JsonResult PopulateNewSearchList(string tableName)
        {
            List<string> searchOptionList = new List<string>();
            string tabName = string.Empty;

            switch (tableName)
            {
                case "Client":
                    tabName = AttachmentTableConstant.Client;
                    break;
                case "Site":
                    tabName = AttachmentTableConstant.Site;
                    break;
                case "KBTopics":
                    tabName = AttachmentTableConstant.KBTopics;
                    break;
                case "AdminUserManagement": //V2-962
                    tabName = AttachmentTableConstant.AdminUserManagement;
                    break;
            }
                    CommonWrapper coWrapper = new CommonWrapper(userData);
            var optList = coWrapper.GetSearchOptionList(tabName);

            if (optList != null)
            {
                foreach (var item in optList)
                {
                    searchOptionList.Add(item.SearchText);
                }
                var returnOjb = new { success = true, searchOptionList = searchOptionList };
                var jsonResult = Json(returnOjb, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            else
            {
                var jsonResult = Json("failed", JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
        }

        [HttpPost]
        public JsonResult ModifyNewSearchList(string tableName, string searchText = "", bool isClear = false)
        {
            List<string> searchOptionList = new List<string>();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var optList = coWrapper.ModifySearchOptionList(tableName, searchText, isClear);
            foreach (var item in optList)
            {
                searchOptionList.Add(item.SearchText);
            }
            var returnOjb = new { success = true, searchOptionList = searchOptionList };
            var jsonResult = Json(returnOjb, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion
        #region  System Unavailable Message
        public JsonResult GetSiteMaintenanceMessage()
        {
            SiteMaintenanceWrapper objSiteMainWrapper = new SiteMaintenanceWrapper(userData);
            var mydata = objSiteMainWrapper.GetNextSitemaintenance("y");
            string maintenanceMessage = string.Empty;
            if (Session["SitetenanceMessage"] == null)
            {
                if (mydata.SiteMaintenanceId != 0)
                {
                    mydata.EasternEndTime = UtilityFunction.GetAMPMWithSpace(mydata.EasternEndTime);
                    mydata.EasternStartTime = UtilityFunction.GetAMPMWithSpace(mydata.EasternStartTime);
                    if (mydata.DowntimeStart.Date == mydata.DowntimeEnd.Date && mydata.DowntimeStart.Date != DateTime.Now.Date)
                    {
                        maintenanceMessage = string.Format("{0} from {1} to {2} on {3}",
                        mydata.DashboardMessage, mydata.EasternStartTime, mydata.EasternEndTime, mydata.DowntimeStart.ToString("dd-MM-yyyy"));
                    }
                    else if (mydata.DowntimeStart.Date == mydata.DowntimeEnd.Date && mydata.DowntimeStart.Date == DateTime.Now.Date)
                    {
                        maintenanceMessage = string.Format("{0} today from {1} to {2}",
                        mydata.DashboardMessage, mydata.EasternStartTime, mydata.EasternEndTime);
                    }
                    else
                    {
                        mydata = objSiteMainWrapper.GetNextSitemaintenance("n");
                        maintenanceMessage = string.Format("{0} from {1} {2} to {3} {4}",
                        mydata.DashboardMessage, mydata.DowntimeStart.ToString("dd-MM-yyyy"), mydata.EasternStartTime, mydata.DowntimeEnd.ToString("dd-MM-yyyy"), mydata.EasternEndTime);
                    }
                }

            }
            else
            {
                maintenanceMessage = string.Empty;
            }
            Session["SitetenanceMessage"] = maintenanceMessage;
            var jsonResult = Json(maintenanceMessage, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion
        #region Menu State
        public JsonResult SetMenuOpenState(string state)
        {
            Session["AdminMenuState"] = state;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FileUpload
        [HttpPost]
        public ActionResult SaveUploadedFileToServer(HttpPostedFileBase file)
        {
            string imageurl = "";
            if (file != null && file.ContentLength > 0)
            {
                 
                var originalDirectory = new DirectoryInfo(string.Format("{0}UploadedImages\\images", Server.MapPath(@"\write\")));
                string pathString = System.IO.Path.Combine(originalDirectory.ToString(), userData.SessionId.ToString());
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);
                var path = string.Format("{0}\\{1}", pathString, file.FileName);
                file.SaveAs(path);
                imageurl = UploadNewImageToAzure(file.FileName, 000, "KBTopics", "KBTopics");
                var originalDirectorynew = new DirectoryInfo(string.Format("{0}UploadedImages\\images\\{1}", Server.MapPath(@"\write\"), userData.SessionId.ToString()));
                System.IO.FileInfo[] files = originalDirectorynew.GetFiles();
                foreach (var filenew in files)
                {
                    if(filenew.Name== file.FileName)
                    filenew.Delete();
                }

            }
            //string url = "https://somaxclientstorage.blob.core.windows.net/0001-00001/Equipment/107180/404Icon.png";
            string url = imageurl;
            return Json(url, JsonRequestBehavior.AllowGet);
        }
       
      
        private string UploadNewImageToAzure(string fileName, long objectId, string TableName, string AttachObjectName)
        {
            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
            string uploadedURL = string.Empty;
            string imageurl = string.Empty;
            string imagePath = string.Empty;
            string content_type = string.Empty;
            var originalDirectory = new DirectoryInfo(string.Format("{0}UploadedImages\\images\\{1}", Server.MapPath(@"\write\"), userData.SessionId.ToString()));
            System.IO.FileInfo[] files = originalDirectory.GetFiles();
            foreach (var file in files)
            {
                if (file.Name == fileName)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(file.FullName);
                    content_type = MimeMapping.GetMimeMapping(file.FullName);
                    imagePath = aset.CreateFileNamebyObject(TableName, objectId.ToString(), fileName);
                    uploadedURL = aset.UploadToAzureBlob(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, imagePath, bytes, content_type);
                    //SaveToDatabase(file, objectId, AttachObjectName, uploadedURL);
                    imageurl = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, uploadedURL);
                }
                
            }
            return imageurl;
        }
      
        private void SavePartMasterRequest(PartMasterRequest pmr, string imageUrl)
        {
            pmr.ImageURL = imageUrl == null ? "" : imageUrl;
            pmr.Update(this.userData.DatabaseKey);
        }
        private string UploadImageToAzure(byte[] uploadedFile, long objectId, string TableName)
        {
            string rtrData = string.Empty;
            AzureBlob ablob = new AzureBlob();
            AzureSetup aset = new AzureSetup();

            if (uploadedFile.Length > 1)
            {
                Int64 Clientid = userData.DatabaseKey.Client.ClientId;
                Int64 Siteid = userData.DatabaseKey.User.DefaultSiteId;
                string imgName = TableName + "_" + DateTime.Now.Ticks.ToString() + "." + "jpg";
                string fileName = aset.CreateFileNamebyObject(TableName, objectId.ToString(), imgName);
                rtrData = aset.ConnectToAzureBlob(Clientid, Siteid, fileName, uploadedFile);
            }

            return rtrData;
        }
        #endregion
    }
}