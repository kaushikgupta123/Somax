using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers.Common;
using Client.Models;
using Client.Models.Equipment;
using Client.Models.MultiStoreroomPart;
using Client.Models.Personnel;

using Common.Constants;

using DataContracts;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.SomaxPersonnel
{
    [CheckUserSecurity(securityType = SecurityConstants.Personnel)]
    public class PersonnelController : SomaxBaseController
    {
        #region Search
        public ActionResult Index()
        {
            PersonnelVM personnelVM = new PersonnelVM();
            var StatusList = UtilityFunction.InactiveActiveStatusTypes();
            if (StatusList != null)
            {
                personnelVM.InactiveFlagList = StatusList.Select(x => new SelectListItem { Text = x.text, Value = x.value.ToString() });
            }
            #region 1108
            personnelVM.personnelModel = new PersonnelModel();
           AssetGroupMasterQuery assetGroupMasterQuery = new AssetGroupMasterQuery();
            personnelVM = BindAssetGroupMasterQuery(personnelVM, assetGroupMasterQuery);
            #endregion
            LocalizeControls(personnelVM, LocalizeResourceSetConstants.PersonnelDetails);
            return View(personnelVM);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public ActionResult GetAdvanSearchContent()
        {
            PersonnelVM personnelVM = new PersonnelVM();
            PersonnelModel personnelModel = new PersonnelModel();
            List<DataContracts.LookupList> shiftList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> schedulegroupList = new List<DataContracts.LookupList>();

            Parallel.Invoke(
                () => shiftList = GetLookUpListByListName(LookupListConstants.Shift),
                () => schedulegroupList = GetLookUpListByListName(LookupListConstants.ScheduleGroup)
                );

            if (shiftList != null)
            {
                personnelModel.ShiftList = shiftList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });
            }
            if (schedulegroupList != null)
            {
                personnelModel.ScheduleGroupList = schedulegroupList.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue });
            }
            personnelVM.personnelModel = personnelModel;
            #region 1108
            AssetGroupMasterQuery assetGroupMasterQuery = new AssetGroupMasterQuery();
            personnelVM = BindAssetGroupMasterQuery(personnelVM, assetGroupMasterQuery);
            #endregion
            LocalizeControls(personnelVM, LocalizeResourceSetConstants.PersonnelDetails);
            return PartialView("_PersonnelAdvancedSearch", personnelVM);
        }
        public string GetPersonnelGrid(int? draw, int? start, int? length, string Order = "1",
            string ClientLookUpId = "", string Name = "", string Shift = "",string searchText = "",int ActiveStatus = 1, int AssignedAssetGroup1=0,int AssignedAssetGroup2=0,int AssignedAssetGroup3=0)
        {
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int totalRecords = 0;
            int recordsFiltered = 0;
            List<PersonnelModel> filteredResult = new List<PersonnelModel>();

            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);

            start = start.HasValue
                ? start / length
                : 0;
            int skip = start * length ?? 0;

            List<PersonnelModel> list = _personalWrapper.GetPersonnelGridData(skip, length ?? 0, Order, orderDir, ClientLookUpId, Name, Shift, searchText, ActiveStatus, AssignedAssetGroup1, AssignedAssetGroup2, AssignedAssetGroup3);

            if (list != null && list.Count > 0)
            {
                recordsFiltered = list.Select(o => o.TotalCount).FirstOrDefault();
                totalRecords = list.Select(o => o.TotalCount).FirstOrDefault();
                filteredResult = list.ToList();
            }
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }

        public string GetPersonnelPrintData(string _colname, string _coldir, int? draw, int? start, int? length, string Order = "1",
            string _ClientLookUpId = "", string _Name = "", string _Shift = "", string _searchText = "",int _activeStatus=1, int _assignedAssetGroup1 = 0, int _assignedAssetGroup2 = 0, int _assignedAssetGroup3 = 0)
        {
            List<PersonnelPrintModel> filteredResult = new List<PersonnelPrintModel>();
            PersonnelPrintModel objPersonnelPrintModel;

            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);

            start = start.HasValue
                ? start / length
                : 0;

            int skip = start * length ?? 0;
            int lengthForPrint = 100000;

            List<PersonnelModel> list = _personalWrapper.GetPersonnelGridData(skip, lengthForPrint, _colname, _coldir, _ClientLookUpId, _Name, _Shift, _searchText, _activeStatus, _assignedAssetGroup1, _assignedAssetGroup2, _assignedAssetGroup3);

            if (list != null)
            {
                foreach (var item in list)
                {
                    objPersonnelPrintModel = new PersonnelPrintModel();
                    objPersonnelPrintModel.ClientLookupId = item.ClientLookupId;
                    objPersonnelPrintModel.Name = item.Name;                  
                    objPersonnelPrintModel.ShiftDescription = item.ShiftDescription;                    
                    objPersonnelPrintModel.CraftClientLookupId = item.CraftClientLookupId;
                    #region 1108
                    objPersonnelPrintModel.AssetGroup1Names = item.AssetGroup1Names;
                    objPersonnelPrintModel.AssetGroup2Names = item.AssetGroup2Names;
                    objPersonnelPrintModel.AssetGroup3Names = item.AssetGroup3Names;
                    #endregion
                    filteredResult.Add(objPersonnelPrintModel);
                }
            }
            return JsonConvert.SerializeObject(new { data = filteredResult }, JsonSerializerDateSettings);
        }

        #endregion

        #region Details
        [HttpPost]
        public ActionResult PersonnelDetails(long PersonnelId)
        {
            PersonnelVM personnelVM = new PersonnelVM();
            AuxiliaryInformationModel auxiliaryInformationModel = new AuxiliaryInformationModel();
            AssetGroupMasterQuery assetGroupMasterQuery = new AssetGroupMasterQuery();
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
            personnelVM.personnelModel = _personalWrapper.getPersonnelDetailsById(PersonnelId, out auxiliaryInformationModel, out assetGroupMasterQuery);
            personnelVM.auxiliaryInformation = auxiliaryInformationModel;
            personnelVM.security = userData.Security;
            personnelVM.UseAssetGroupMasterQuery =  userData.DatabaseKey.Client.UseAssetGroupMasterQuery;
            personnelVM = BindAssetGroupMasterQuery(personnelVM, assetGroupMasterQuery);
            personnelVM.udata = userData;
            LocalizeControls(personnelVM, LocalizeResourceSetConstants.PersonnelDetails);
            return PartialView("PersonnelDetails", personnelVM);
        }
        private PersonnelVM BindAssetGroupMasterQuery(PersonnelVM personnelVM, AssetGroupMasterQuery assetGroupMasterQuery)
        {
            EquipmentWrapper eWrapper = new EquipmentWrapper(userData);

            assetGroupMasterQuery.PersonnelId = personnelVM.personnelModel.PersonnelId;
            personnelVM.AssetGroup1Label = userData.Site.AssetGroup1Name;
            personnelVM.AssetGroup2Label = userData.Site.AssetGroup2Name;
            personnelVM.AssetGroup3Label = userData.Site.AssetGroup3Name;

            Task[] tasks = new Task[3];
            List<AssetGroup1Model> ast1 = new List<AssetGroup1Model>();
            List<AssetGroup2Model> ast2 = new List<AssetGroup2Model>();
            List<AssetGroup3Model> ast3 = new List<AssetGroup3Model>();

            tasks[0] = Task.Factory.StartNew(() => ast1 = eWrapper.GetAllAssetGroup1Dropdowndata());
            tasks[1] = Task.Factory.StartNew(() => ast2 = eWrapper.GetAllAssetGroup2Dropdowndata());
            tasks[2] = Task.Factory.StartNew(() => ast3 = eWrapper.GetAllAssetGroup3Dropdowndata());
            Task.WaitAll(tasks);
            if (ast1 != null && ast1.Count > 0)
            {
                personnelVM.AssetGroup1List = ast1.Select(x => new SelectListItem { Text = x.AssetGroup1DescWithClientLookupId, Value = x.AssetGroup1Id.ToString() });
            }
            if (ast2 != null && ast2.Count > 0)
            {
                personnelVM.AssetGroup2List = ast2.Select(x => new SelectListItem { Text = x.AssetGroup2DescWithClientLookupId, Value = x.AssetGroup2Id.ToString() });
            }
            if (ast3 != null && ast3.Count > 0)
            {
                personnelVM.AssetGroup3List = ast3.Select(x => new SelectListItem { Text = x.AssetGroup3DescWithClientLookupId, Value = x.AssetGroup3Id.ToString() });
            }

            personnelVM.assetGroupMasterQuery = assetGroupMasterQuery;

            return personnelVM;
        }
        #endregion

        #region Edit
        public ActionResult EditPersonnel(long? PersonnelId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PersonnelVM personnelVM = new PersonnelVM();
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
            List<DataContracts.LookupList> allLookUps = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> shiftlist = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> scheduleGrouplist = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> crewlist = new List<DataContracts.LookupList>();
            List<DropDownModel> deptList = new List<DropDownModel>();
            List<DataModel> craftList = new List<DataModel>();
            List<DataModel> DeptlookUpList = new List<DataModel>();        
            PersonnelModel obj = new PersonnelModel();
            AuxiliaryInformationModel auxiliaryInformation = new AuxiliaryInformationModel();
            AssetGroupMasterQuery assetGroupMasterQuery = new AssetGroupMasterQuery();          

            #region task
            Parallel.Invoke(
                    () => obj = _personalWrapper.getPersonnelDetailsById(PersonnelId ?? 0, out auxiliaryInformation, out assetGroupMasterQuery),
                    () => allLookUps = commonWrapper.GetAllLookUpList(),
                    () => craftList = GetLookUpList_Craft(),
                    () => deptList = GetDepartmenttByInActiveFlag(false)
                    
                );

            #endregion

            if (allLookUps != null)
            {
                shiftlist = allLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (shiftlist != null)
                {
                    obj.ShiftList = shiftlist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
                crewlist = allLookUps.Where(x => x.ListName.ToUpper() == LookupListConstants.crew).ToList();
                if (crewlist != null)
                {
                    obj.CrewList = crewlist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
                scheduleGrouplist = allLookUps.Where(x => x.ListName == LookupListConstants.ScheduleGroup).ToList();
                if (scheduleGrouplist != null)
                {
                    obj.ScheduleGroupList = scheduleGrouplist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }
            }
            if (craftList != null)
            {
                obj.CraftList = craftList.Select(x => new SelectListItem { Text = x.Description.ToString(), Value = x.CraftId.ToString() }).ToList();
            }
            if (deptList != null && deptList.Count > 0)
            {
                obj.DeptList = deptList.Select(x => new SelectListItem { Text = x.text, Value = x.value }).ToList();
            }
            personnelVM.personnelModel = obj;
            #region 1108
            personnelVM = BindAssetGroupMasterQuery(personnelVM, assetGroupMasterQuery);
            #endregion
            #region V2-1178
            var StoreroomLookUplist = commonWrapper.GetStoreroomListByClientIdSiteId();
            if (StoreroomLookUplist != null)
            {
                personnelVM.personnelModel.StoreroomList = StoreroomLookUplist.Select(x => new SelectListItem { Text = x.Name+" "+x.Description, Value = x.StoreroomId.ToString() }).ToList();
            }
            personnelVM.udata = userData;
            #endregion
            LocalizeControls(personnelVM, LocalizeResourceSetConstants.PersonnelDetails);            
            return PartialView("_PersonnelEdit", personnelVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePersonnel(PersonnelVM personnelVM, string Command)
        {
            if (ModelState.IsValid)
            {
                List<String> errorList = new List<string>();
                PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData); ;
                string Mode = string.Empty;
                long PersonnelId = personnelVM.personnelModel.PersonnelId;

                errorList = _personalWrapper.UpdatePersonnel(personnelVM.personnelModel);

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PersonnelId = PersonnelId, mode = Mode, Command = Command }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Attachment
        [HttpPost]
        public string PopulateAttachment(int? draw, int? start, int? length, long PersonnelId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            var AttachmentList = objCommonWrapper.PopulateAttachments(PersonnelId, AttachmentTableConstant.Personnel, userData.Security.Personnel.Edit);
            if (AttachmentList != null)
            {
                AttachmentList = GetAllAttachmentsSortByColumnWithOrder(order, orderDir, AttachmentList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AttachmentList.Count();
            totalRecords = AttachmentList.Count();
            int initialPage = start.Value;
            var filteredResult = AttachmentList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);
        }

        [HttpGet]
        public ActionResult AddAttachment(long PersonnelId, string ClientLookupId)
        {
            PersonnelVM personnelVM = new PersonnelVM();
            PersonnelModel personnelModel = new PersonnelModel();
            AttachmentModel Attachment = new AttachmentModel();

            personnelModel.PersonnelId = PersonnelId;
            personnelModel.ClientLookupId = ClientLookupId;
            personnelVM.personnelModel = personnelModel;

            Attachment.PersonnelId = PersonnelId;
            Attachment.ClientLookupId = ClientLookupId;
            personnelVM.attachmentModel = Attachment;

            LocalizeControls(personnelVM, LocalizeResourceSetConstants.PersonnelDetails);
            return PartialView("_AttachmentAdd", personnelVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttachment()
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                Stream stream = Request.Files[0].InputStream;
                var fileName = Request.Files[0].FileName;
                AttachmentModel attachmentModel = new AttachmentModel();
                CommonWrapper objCommonWrapper = new CommonWrapper(userData);
                Attachment fileAtt = new Attachment();

                attachmentModel.FileName = System.IO.Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
                string fileExt = System.IO.Path.GetExtension(Request.Files[0].FileName);
                attachmentModel.FileType = fileExt.Substring(1);

                attachmentModel.FileSize = Request.Files[0].ContentLength;
                attachmentModel.ContentType = Request.Files[0].ContentType;
                attachmentModel.ObjectId = Convert.ToInt64(Request.Form["attachmentModel.PersonnelId"]);
                attachmentModel.Subject = String.IsNullOrEmpty(Request.Form["attachmentModel.Subject"]) ? "No Subject" : Request.Form["attachmentModel.Subject"];
                attachmentModel.TableName = AttachmentTableConstant.Personnel;
                bool attachStatus = false;
                bool OnPremise = userData.DatabaseKey.Client.OnPremise;
                if (OnPremise)
                {
                    fileAtt = objCommonWrapper.AddAttachmentOnPremise(attachmentModel, stream, ref attachStatus, userData.Security.Personnel.Edit);
                }
                else
                {
                    fileAtt = objCommonWrapper.AddAttachment(attachmentModel, stream, ref attachStatus, userData.Security.Personnel.Edit);
                }

                if (attachStatus)
                {
                    if (fileAtt.ErrorMessages != null && fileAtt.ErrorMessages.Count > 0)
                    {
                        return Json(fileAtt.ErrorMessages, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = JsonReturnEnum.success.ToString(), PersonnelId = Convert.ToInt64(Request.Form["attachmentModel.PersonnelId"]) }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var fileTypeMessage = UtilityFunction.GetMessageFromResource("spnInvalidFileType", LocalizeResourceSetConstants.Global);
                    return Json(fileTypeMessage, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadAttachment(long _fileinfoId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            DataContracts.Attachment fileInfo = new DataContracts.Attachment();
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                fileInfo = objCommonWrapper.DownloadAttachmentOnPremise(_fileinfoId);
                string contentType = MimeMapping.GetMimeMapping(fileInfo.AttachmentURL);
                return File(fileInfo.AttachmentURL, contentType, fileInfo.FileName + '.' + fileInfo.FileType);
            }
            else
            {
                fileInfo = objCommonWrapper.DownloadAttachment(_fileinfoId);
                return Redirect(fileInfo.AttachmentURL);
            }

        }
     
        [HttpPost]
        public ActionResult DeleteAttachment(long fileAttachmentId)
        {
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            bool deleteResult = false;
            string Message = string.Empty;
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            if (OnPremise)
            {
                deleteResult = objCommonWrapper.DeleteAttachmentOnPremise(fileAttachmentId, ref Message);
            }
            else
            {
                deleteResult = objCommonWrapper.DeleteAttachment(fileAttachmentId);

            }
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString(), Message = Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Events

        [HttpPost]
        public string PopulateEvents(int? draw, int? start, int? length, long PersonnelId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
            var eventsList = _personalWrapper.PopulateEventsList(PersonnelId);
            eventsList = this.GetAllEventsSortByColumnWithOrder(order, orderDir, eventsList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = eventsList.Count();
            totalRecords = eventsList.Count();
            int initialPage = start.Value;
            var filteredResult = eventsList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, security = userData.Security.Personnel_Events }, JsonSerializerDateSettings);
        }

        private List<EventsModel> GetAllEventsSortByColumnWithOrder(string order, string orderDir, List<EventsModel> data)
        {
            List<EventsModel> lst = new List<EventsModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CompleteDate).ToList() : data.OrderBy(p => p.CompleteDate).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ExpireDate).ToList() : data.OrderBy(p => p.ExpireDate).ToList();
                    break;
            }
            return lst;
        }

        public ActionResult AddOrEditEvents(long? EventId, long PersonnelId, string ClientLookupId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
            PersonnelVM objpersonnelVM = new PersonnelVM();
            PersonnelModel personnelModel = new PersonnelModel();
            List<DataContracts.LookupList> eventslist = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> allLookUps = new List<DataContracts.LookupList>();

            allLookUps = commonWrapper.GetAllLookUpList();
            if (allLookUps != null)
            {
                eventslist = allLookUps.Where(x => x.ListName == LookupListConstants.Event_Type).ToList();
                if (eventslist != null)
                {
                    objpersonnelVM.EventTypeList = eventslist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

            }
            EventsModel eventmodel = new EventsModel();

            if (EventId != null)
            {
                eventmodel = _personalWrapper.RetriveEventById(EventId, PersonnelId)
               ;
            }
            else
            {
                eventmodel.PersonnelId = PersonnelId;
                eventmodel.PersonnelClientLookupId = ClientLookupId;
                eventmodel.EventsId = 0;
                eventmodel.CompleteDate = DateTime.Now;
            }
            objpersonnelVM.eventmodel = eventmodel;
            LocalizeControls(objpersonnelVM, LocalizeResourceSetConstants.PersonnelDetails);
            return PartialView("_EventsAddEdit", objpersonnelVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEditEvent(PersonnelVM objperson)
        {
            if (ModelState.IsValid)
            {
                PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                if (objperson.eventmodel.EventsId > 0)
                {
                    Mode = "update";
                    errorList = _personalWrapper.UpdateEvent(objperson.eventmodel);
                }
                else
                {
                    Mode = "add";
                    errorList = _personalWrapper.AddEvent(objperson.eventmodel);
                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode, PersonnelId = objperson.eventmodel.PersonnelId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteEvent(long eventId)
        {
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
            var deleteResult = _personalWrapper.DeleteEvent(eventId);
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

        #region Availability
        [HttpPost]
        public string PopulateAvailability(int? draw, int? start, int? length, long PersonnelId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);

            var shifts = GetLookUpListByListName(LookupListConstants.Shift);
            var personnelAvailabilities = _personalWrapper.GetPersonnelAvailabilityGridData(PersonnelId);

            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = personnelAvailabilities.Count();
            totalRecords = personnelAvailabilities.Count();
            int initialPage = start.Value;

            var filteredResult = personnelAvailabilities.OrderBy(x => x.PersonnelAvailabilityDate);
            switch (order)
            {
                case "0":
                    filteredResult = orderDir == "desc" ?
                        personnelAvailabilities.OrderByDescending(x => x.PersonnelAvailabilityDate) : personnelAvailabilities.OrderBy(x => x.PersonnelAvailabilityDate);
                    break;
                case "1":
                    filteredResult = orderDir == "desc" ?
                        personnelAvailabilities.OrderByDescending(x => x.PAHours) : personnelAvailabilities.OrderBy(x => x.PAHours);
                    break;
                case "2":
                    filteredResult = orderDir == "desc" ?
                        personnelAvailabilities.OrderByDescending(x => x.PAShift) : personnelAvailabilities.OrderBy(x => x.PAShift);
                    break;
            }


            var result = filteredResult.Skip(initialPage * length ?? 0)
                   .Take(length ?? 0).Select((x) =>
                   {
                       x.PAShift = shifts?.FirstOrDefault(y => y.ListValue == x.PAShift)?.Description;
                       return x;
                   }).ToList();

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = result,
                security = userData.Security.Personnel_Availability
            }, JsonSerializerDateSettings);
        }

        [HttpGet]
        public ActionResult AddAvailability(long PersonnelId, long? PersonnelAvailabilityId, string clientLookUpId)
        {
            PersonnelVM personnelVM = new PersonnelVM();
            PersonnelModel personnelModel = new PersonnelModel();
            PersonnelAvailabilityModel personnelAvailabilityModel = new PersonnelAvailabilityModel();

            personnelModel.PersonnelId = PersonnelId;
            personnelVM.personnelModel = personnelModel;
            personnelModel.ClientLookupId = clientLookUpId;

            personnelAvailabilityModel.PersonnelId = PersonnelId;

            if (PersonnelAvailabilityId.HasValue)
            {
                personnelAvailabilityModel.PersonnelAvailabilityId = PersonnelAvailabilityId.Value;
                PersonnelWrapper personnelWrapper = new PersonnelWrapper(userData);
                var personnelAvailability = personnelWrapper.RetrievePersonnelAvailability(personnelAvailabilityModel);
                personnelAvailabilityModel.PAHours = personnelAvailability.Hours;
                personnelAvailabilityModel.PAShift = personnelAvailability.Shift;
                personnelAvailabilityModel.PersonnelAvailabilityDate = personnelAvailability.Date;
                personnelAvailabilityModel.PersonnelAvailabilityId = personnelAvailability.PersonnelAvailabilityId;
                personnelAvailabilityModel.PersonnelId = PersonnelId;
                personnelAvailabilityModel.ClientLookupId = clientLookUpId;
            }

            personnelVM.personnelAvailabilityModel = personnelAvailabilityModel;

            personnelVM.personnelModel.ShiftList = GetLookUpListByListName(LookupListConstants.Shift)?.Select(x => new SelectListItem { Text = x.Description, Value = x.ListValue.ToString() });

            LocalizeControls(personnelVM, LocalizeResourceSetConstants.PersonnelDetails);
            return PartialView("_AvailibilityAdd", personnelVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAvailability(PersonnelAvailabilityModel personnelAvailabilityModel)
        {
            string ModelValidationFailedMessage = string.Empty;
            if (ModelState.IsValid)
            {
                string modeOfOrperation = string.Empty;
                List<string> errorList = null;
                PersonnelWrapper personnelWrapper = new PersonnelWrapper(userData);
                if (personnelAvailabilityModel.PersonnelAvailabilityId > 0)
                {
                    modeOfOrperation = "Update";
                    errorList = personnelWrapper.UpdatePersonnelAvailability(personnelAvailabilityModel);
                }
                else
                {
                    modeOfOrperation = "Add";
                    errorList = personnelWrapper.AddPersonnelAvailability(personnelAvailabilityModel);
                }

                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Mode = modeOfOrperation, PersonnelId = personnelAvailabilityModel.PersonnelId }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteAvailability(long PersonnelAvailabilityId)
        {
            PersonnelWrapper personnelWrapper = new PersonnelWrapper(userData);
            PersonnelAvailabilityModel personnelAvailabilityModel = new PersonnelAvailabilityModel();
            personnelAvailabilityModel.PersonnelAvailabilityId = PersonnelAvailabilityId;
            personnelWrapper.DeletePersonnelAvailability(personnelAvailabilityModel);
            return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Labor
        [HttpPost]
        public string PopulateLabor(int? draw, int? start, int? length, long PersonnelId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
            var laborList = _personalWrapper.PopulateLaborList(PersonnelId);
            laborList = this.GetAllLaborsSortByColumnWithOrder(order, orderDir, laborList);
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = laborList.Count();
            totalRecords = laborList.Count();
            int initialPage = start.Value;
            var filteredResult = laborList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializerDateSettings);
        }
        private List<LaborModel> GetAllLaborsSortByColumnWithOrder(string order, string orderDir, List<LaborModel> data)
        {
            List<LaborModel> lst = new List<LaborModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo).ToList() : data.OrderBy(p => p.ChargeTo).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Date).ToList() : data.OrderBy(p => p.Date).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Hours).ToList() : data.OrderBy(p => p.Hours).ToList();
                    break;
                case "3":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cost).ToList() : data.OrderBy(p => p.Cost).ToList();
                    break;
            }
            return lst;
        }

        #endregion

        #region Attendance
        [HttpPost]
        public string PopulateAttendance(int? draw, int? start, int? length, long PersonnelId)
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
            var AttendanceList = _personalWrapper.PopulateAttendance(PersonnelId);
            if (AttendanceList != null)
            {
                AttendanceList = GetAllAttendanceSortByColumnWithOrder(order, orderDir, AttendanceList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = AttendanceList.Count();
            totalRecords = AttendanceList.Count();
            int initialPage = start.Value;
            var filteredResult = AttendanceList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult, security = userData.Security.Personnel_Attendance }, JsonSerializerDateSettings);
        }
        private List<PersonnelAttendanceModel> GetAllAttendanceSortByColumnWithOrder(string order, string orderDir, List<PersonnelAttendanceModel> data)
        {
            List<PersonnelAttendanceModel> lst = new List<PersonnelAttendanceModel>();
            switch (order)
            {
                case "0":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Date).ToList() : data.OrderBy(p => p.Date).ToList();
                    break;
                case "1":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Hours).ToList() : data.OrderBy(p => p.Hours).ToList();
                    break;
                case "2":
                    lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ShiftDescription).ToList() : data.OrderBy(p => p.ShiftDescription).ToList();
                    break;
            }
            return lst;
        }
        public ActionResult AddOrEditAttendance(long? PersonnelAttendanceId, long PersonnelId, string ClientLookupId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
            PersonnelVM objpersonnelVM = new PersonnelVM();
            List<DataContracts.LookupList> shiftslist = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> allLookUps = new List<DataContracts.LookupList>();

            allLookUps = commonWrapper.GetAllLookUpList();
            if (allLookUps != null)
            {
                shiftslist = allLookUps.Where(x => x.ListName == LookupListConstants.Shift).ToList();
                if (shiftslist != null)
                {
                    objpersonnelVM.ShiftList = shiftslist.Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.ListValue }).ToList();
                }

            }

            PersonnelAttendanceModel personnelModel = new PersonnelAttendanceModel();

            if (PersonnelAttendanceId != null)
            {
                personnelModel = _personalWrapper.RetrivePersonnelAttendaceById(PersonnelAttendanceId, PersonnelId);
            }
            else
            {
                personnelModel.PersonnelId = PersonnelId;
                personnelModel.PersonnelClientLookupId = ClientLookupId;
                personnelModel.PersonnelAttendanceId = 0;
            }
            objpersonnelVM.personnelAttendanceModel = personnelModel;
            LocalizeControls(objpersonnelVM, LocalizeResourceSetConstants.PersonnelDetails);
            return PartialView("_AttendaceAddEdit", objpersonnelVM);
        }
        [ValidateAntiForgeryToken()]
        [HttpPost]
        public ActionResult AddOrEditAttendance(PersonnelVM objpersonnel)
        {
            if (ModelState.IsValid)
            {
                PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
                string Mode = string.Empty;
                List<String> errorList = new List<string>();
                if (objpersonnel.personnelAttendanceModel.PersonnelAttendanceId > 0)
                {
                    Mode = "update";
                    errorList = _personalWrapper.UpdateAttendance(objpersonnel.personnelAttendanceModel);
                }
                else
                {
                    Mode = "add";
                    errorList = _personalWrapper.AddAttendance(objpersonnel.personnelAttendanceModel);
                }
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), mode = Mode, PersonnelId = objpersonnel.personnelAttendanceModel.PersonnelId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteAttendance(long PersonnelAttendanceId)
        {
            PersonnelWrapper _personalWrapper = new PersonnelWrapper(userData);
            var deleteResult = _personalWrapper.DeleteAttendance(PersonnelAttendanceId);
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

        #region Auxiliary-Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateAuxiliaryInformation(AuxiliaryInformationModel auxiliaryInformation)
        {
            if (ModelState.IsValid)
            {
                PersonnelWrapper personnelWrapper = new PersonnelWrapper(userData);
                var errorList = personnelWrapper.UpdateAuxiliaryInformation(auxiliaryInformation);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PersonnelId = auxiliaryInformation.PersonnelId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Asset group master query
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateAssetGroupMasterQuery(AssetGroupMasterQuery assetGroupMasterQuery)
        {
            if (ModelState.IsValid)
            {
                PersonnelWrapper personnelWrapper = new PersonnelWrapper(userData);
                var errorList = personnelWrapper.UpdateAssetGroupMasterQuery(assetGroupMasterQuery);
                if (errorList != null && errorList.Count > 0)
                {
                    return Json(errorList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), PersonnelId = assetGroupMasterQuery.PersonnelId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
