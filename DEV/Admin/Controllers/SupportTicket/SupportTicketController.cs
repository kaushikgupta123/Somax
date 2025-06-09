using Admin.ActionFilters;
using Admin.BusinessWrapper;
using Admin.BusinessWrapper.Common;
using Admin.Common;
using Admin.Controllers.Common;
using Admin.Models;
using Admin.Models.Client;
using Admin.Models.SupportTicket;

using Common.Constants;

using DataContracts;

using Newtonsoft.Json;

//using Rotativa;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using static Admin.Models.Common.UserMentionDataModel;

namespace Client.Controllers.SupportTicket
{
    public class SupportTicketController : SomaxBaseController
    {
        #region Search
        public ActionResult Index()
        {
            SupportTicketVM supportTicketVM = new SupportTicketVM();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            supportTicketVM.CustomQueryDisplayList = commonWrapper.PopulateCustomQueryDisplay(AttachmentTableConstant.SupportTicket);
            LocalizeControls(supportTicketVM, LocalizeResourceSetConstants.SupportTicketDetails);
            return View(supportTicketVM);
        }
        public string GetSupportTicketGridData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, long? SupportTicketId = null, string Subject = "", string Status = "", string Order = "0", string Contact = "", string Agent = "", DateTime? CreateDate = null, DateTime? CompleteDate = null, string SearchText = "")
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            SupportTicketWrapper mrWrapper = new SupportTicketWrapper(userData);
            List<SupportTicketModel> mRList = mrWrapper.GetSupportTicketGridData(CustomQueryDisplayId, skip, length ?? 0, Order, orderDir, SupportTicketId,Status,Contact,Subject, Agent,CreateDate,CompleteDate, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (mRList != null && mRList.Count > 0)
            {
                recordsFiltered = mRList[0].TotalCount;
                totalRecords = mRList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = mRList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        #region Print Grid
        public string GetSupportTicketPrintData(int? draw, int? start, int? length, int CustomQueryDisplayId = 0, long? SupportTicketId = null, string Subject = "", string Status = "", string Contact = "", string Agent = "", DateTime? CreateDate = null, DateTime? CompleteDate = null, string SearchText = "", string Order = "0", string coldir = "asc")
        {
            //string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            var filter = CustomQueryDisplayId;
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            SupportTicketWrapper stWrapper = new SupportTicketWrapper(userData);
            SupportTicketPrintModel objSupportTicketPrintModel;
            List<SupportTicketPrintModel> supportTicketPrintModelsList= new List<SupportTicketPrintModel>();
            List<SupportTicketModel> STList = stWrapper.GetSupportTicketGridData(CustomQueryDisplayId, 0, 100000, Order, coldir, SupportTicketId, Status, Contact, Subject, Agent, CreateDate, CompleteDate, SearchText);
            foreach (var item in STList)
            {
                objSupportTicketPrintModel = new SupportTicketPrintModel();
                objSupportTicketPrintModel.SupportTicketId = item.SupportTicketId;
                objSupportTicketPrintModel.Subject = item.Subject;
                objSupportTicketPrintModel.Contact = item.Contact;
                objSupportTicketPrintModel.Status = item.Status;
                objSupportTicketPrintModel.Agent = item.Agent;
                objSupportTicketPrintModel.CompleteDate = item.CompleteDate;
                objSupportTicketPrintModel.CreateDate = item.CreateDate;
                objSupportTicketPrintModel.TotalCount = item.TotalCount;
                supportTicketPrintModelsList.Add(objSupportTicketPrintModel);
            }
            return JsonConvert.SerializeObject(new { data = supportTicketPrintModelsList }, JsonSerializerDateSettings);
        }
        #endregion
        #endregion

        #region Common
        public List<ClientModel> GetAllActiveClient()
        {
            DataContracts.Client client = new DataContracts.Client();
            var retData = client.RetrieveAllActiveClient(this.userData.DatabaseKey);
            ClientModel clientModel;
            List<ClientModel> ClientModelList = new List<ClientModel>();
            foreach (var item in retData)
            {
                clientModel = new ClientModel();
                clientModel.ClientId = item.ClientId;
                clientModel.Name = item.CompanyName;
                ClientModelList.Add(clientModel);
            }
            return ClientModelList;
        }
        [HttpPost]
        public JsonResult GetSiteByClientId(long ClientId)
        {
            List<DropDownWithIdModel> values = new List<DropDownWithIdModel>();
            DropDownWithIdModel multiSelectModel;

            Site personnel = new Site()
            {
                ClientId = ClientId
            };
            DataContracts.Client client = new DataContracts.Client()
            {
                CreatedClientId = ClientId,
            };
            client.RetrieveBySomaxAdmin_V2(this.userData.DatabaseKey);
            List<Site> PersonnelList = personnel.RetrieveByClientIdForLookupList(this.userData.DatabaseKey,client.ConnectionString);

            foreach (var item in PersonnelList)
            {
                multiSelectModel = new DropDownWithIdModel();
                multiSelectModel.value = item.SiteId;
                multiSelectModel.text = item.Name;
                values.Add(multiSelectModel);
            }
            return Json(new { data = JsonReturnEnum.success.ToString(), SiteList = values }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllActiveList_Personnel(long ClientId,long SiteId)
        {
            List<DropDownWithIdModel> values = new List<DropDownWithIdModel>();
            DropDownWithIdModel multiSelectModel;
            Personnel personnel = new Personnel()
            {
                ClientId = ClientId,
                SiteId = SiteId
            };

            List<Personnel> PersonnelList = personnel.RetrieveAllActiveForLookupListForAdmin(this.userData.DatabaseKey);
            foreach (var item in PersonnelList)
            {
                multiSelectModel = new DropDownWithIdModel();
                multiSelectModel.value = item.PersonnelId;
                multiSelectModel.text = item.NameFirst+" "+item.NameLast;
                values.Add(multiSelectModel);
            }

            return Json(new { data = JsonReturnEnum.success.ToString(), ContactList = values }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Details
        public PartialViewResult TicketDetails(long SupportTicketId,long ClientId)
        {
            SupportTicketVM supportTicketVM = new SupportTicketVM();
            SupportTicketWrapper supportTicketWrapper = new SupportTicketWrapper(userData);
            var SupportTicketDetails = supportTicketWrapper.GetDetailsById(SupportTicketId,ClientId);
            supportTicketVM.SupportTicketModel = SupportTicketDetails;
            LocalizeControls(supportTicketVM, LocalizeResourceSetConstants.SupportTicketDetails);
            return PartialView("_SupportTicketDetails", supportTicketVM);
        }
        #endregion

        #region Add/Edit
        public PartialViewResult AddSupportTicket()
        {
            SupportTicketVM objSTVM = new SupportTicketVM();
            SupportTicketWrapper mrWrapper = new SupportTicketWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            SupportTicketModel stModel = new SupportTicketModel();
            objSTVM.security = this.userData.Security;

            var ClientList = GetAllActiveClient();
            if(ClientList!=null)
            {
                stModel.ClientList = ClientList.Select(x => new SelectListItem { Text = x.Name.ToString(), Value = x.ClientId.ToString() });
            }
            var STTypeList = commonWrapper.GetListFromConstVals(LookupListConstants.STType);
            if(STTypeList!=null)
            {
                stModel.TypeList=STTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            objSTVM.SupportTicketModel = stModel;
            LocalizeControls(objSTVM, LocalizeResourceSetConstants.SupportTicketDetails);
            return PartialView("~/Views/SupportTicket/_SupportTicketAdd.cshtml", objSTVM);
        }
        public PartialViewResult EditSupportTicket(long SupportTicketId, long ClientId)
        {
            SupportTicketVM supportTicketVM = new SupportTicketVM();
            SupportTicketWrapper supportTicketWrapper = new SupportTicketWrapper(userData);
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var SupportTicketDetails = supportTicketWrapper.GetDetailsById(SupportTicketId, ClientId);
            var ClientList = GetAllActiveClient();
            if (ClientList != null)
            {
                SupportTicketDetails.ClientList = ClientList.Select(x => new SelectListItem { Text = x.Name.ToString(), Value = x.ClientId.ToString() });
            }
            var STTypeList = commonWrapper.GetListFromConstVals(LookupListConstants.STType);
            if (STTypeList != null)
            {
                SupportTicketDetails.TypeList = STTypeList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            supportTicketVM.SupportTicketModel = SupportTicketDetails;
            LocalizeControls(supportTicketVM, LocalizeResourceSetConstants.SupportTicketDetails);
            return PartialView("~/Views/SupportTicket/_SupportTicketAdd.cshtml", supportTicketVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddSupportTicket(SupportTicketVM stVM, string Command)
        {
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
            if (ModelState.IsValid)
            {
                SupportTicketWrapper stWrapper = new SupportTicketWrapper(userData);
                if (stVM.SupportTicketModel.SupportTicketId == 0)
                {
                    Mode = "add";
                }
                var objSupportTicket = stWrapper.SaveSupportTicket(stVM.SupportTicketModel);

                if (objSupportTicket.ErrorMessages != null && objSupportTicket.ErrorMessages.Count > 0)
                {
                    return Json(objSupportTicket.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, SupportTicketId = objSupportTicket.SupportTicketId, ClientId=objSupportTicket.ClientId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
            return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ticket Response     
        [HttpPost]
        public PartialViewResult LoadTicketResponses(long SupportTicketId/*, long ClientId = 0, long SiteId = 0*/)
        {
            SupportTicketVM supportTicketVM = new SupportTicketVM();
            SupportTicketWrapper STWrapper = new SupportTicketWrapper(userData);
            List<STNotes> NotesList = new List<STNotes>();
            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() => NotesList = STWrapper.PopulateTicketResponse(SupportTicketId));
            Task.WaitAll(tasks);
            if (!tasks[0].IsFaulted && tasks[0].IsCompleted)
            {
                supportTicketVM.STNotesList = NotesList;
            }
            LocalizeControls(supportTicketVM, LocalizeResourceSetConstants.SupportTicketDetails);
            return PartialView("_TicketResponsesList", supportTicketVM);
        }
                
        #region Add Update Response
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddUpdateResponse(long SupportTicketId, string content, long noteId = 0, string Type = "")
        {
            SupportTicketWrapper stWrapper = new SupportTicketWrapper(userData);
            STNotesModel notesModel = new STNotesModel();
            notesModel.SupportTicketId = SupportTicketId;
            notesModel.Content = content;
            notesModel.STNotesId = noteId;
            notesModel.Type = Type;
            if (ModelState.IsValid)
            {
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                errorList = stWrapper.SaveResponse(notesModel, ref Mode);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), SupportTicketId = SupportTicketId, mode = Mode }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Delete Response
        [HttpPost]
        public ActionResult DeleteResponse(long _notesId)
        {
            SupportTicketWrapper stWrapper = new SupportTicketWrapper(userData);
            var deleteResult = stWrapper.DeleteResponse(_notesId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion

        #region  Tags Name Retrieve
        public JsonResult RetrieveTagName()
        {
            SupportTicketWrapper stWrapper = new SupportTicketWrapper(userData);
            var totalList = stWrapper.SupportTicketTags();

            return Json(totalList, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Functions
        public PartialViewResult AddSTNotes(long SupportTicketId,long ClientId,long SiteId, string Type)
        {
            SupportTicketVM objMaterialRequestVM = new SupportTicketVM();
            STNotesModel sTNotesModel = new STNotesModel();
            sTNotesModel.SupportTicketId = SupportTicketId;
            sTNotesModel.ClientId = ClientId;
            sTNotesModel.SiteId = SiteId;
            if (Type == "Reply")
            {
                sTNotesModel.Type = STNotesTypesConstants.Reply;
            }
            else if (Type == "Note")
            {
                sTNotesModel.Type= STNotesTypesConstants.Note;
            }

            objMaterialRequestVM.STNotesModel = sTNotesModel;
            ViewBag.IsMaterialRequestDetails = false;
            LocalizeControls(objMaterialRequestVM, LocalizeResourceSetConstants.SupportTicketDetails);
            return PartialView("_AddReply", objMaterialRequestVM);
        }
        public JsonResult ChangeStatus(long ClientId,long SupportTicketId,string Status)
        {
            SupportTicketWrapper stWrapper = new SupportTicketWrapper(userData);
            var deleteResult = stWrapper.ChangeStatus(ClientId,SupportTicketId,Status);
            if (deleteResult.ErrorMessages == null || deleteResult.ErrorMessages.Count == 0)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion        
    }

}

