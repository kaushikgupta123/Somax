using Admin.BusinessWrapper;
using Admin.BusinessWrapper.Common;
using Admin.BusinessWrapper.Knowledgebase;
using Admin.Common;
using Admin.Models;
using Admin.Models.Knowledgebase;
using Common.Constants;
using DataContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Admin.Controllers.Knowledgebase
{
    public class KnowledgebaseController : BaseController
    {
        #region Property
        private readonly IKnowledgebaseWrapper _knowledgebaseWrapper;
        public KnowledgebaseController(IKnowledgebaseWrapper knowledgebaseWrapper) 
        {
            _knowledgebaseWrapper = knowledgebaseWrapper;            
        }
        #endregion
        #region Search
        public ActionResult Index()
        {
            KnowledgebaseCombined objComb = new KnowledgebaseCombined();        
            string mode = Convert.ToString(TempData["Mode"]);
            if (mode == "add")
            {
                ViewBag.IsKnowledgebaseAdd = true;
            }
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var KBCategoryList = commonWrapper.GetListFromConstVals(LookupListConstants.KB_CategoryType);
            if (KBCategoryList != null)
            {
                objComb.KBTopicsModel = new KnowledgebaseModel();
                objComb.KBTopicsModel.KBTopicsCategoryList = KBCategoryList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });

                objComb.LookupCalegoryList = KBCategoryList.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            _knowledgebaseWrapper.userData = userData;
            var totalList = _knowledgebaseWrapper.KbTopicsPersonnelList();
            objComb.KBTopicsModel.SearchPersonnellist = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.FullName.ToString() }).ToList();
            LocalizeControls(objComb, LocalizeResourceSetConstants.KnowledgebaseDetails);
            return View(objComb);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public string GetkbtopicsGridData(int? draw, int? start, int? length, int customQueryDisplayId = 1, string Title = "", string Category = "", string Description = "",
                              string Tags = "", string Folder = "", string SearchText = "", string order = "", string orderDir = "", long CategoryId = 0
      )
        {
            KnowledgebaseSearchModel eSearchModel;
            List<KnowledgebaseSearchModel> eSearchModelList = new List<KnowledgebaseSearchModel>();    
            SearchText = SearchText.Replace("%", "[%]");
            Title = Title.Replace("%", "[%]");
            Category = Category.Replace("%", "[%]");
            Description = Description.Replace("%", "[%]");
            Tags = Tags.Replace("%", "[%]");
            Folder = Folder.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            List<string> typeList = new List<string>();
            _knowledgebaseWrapper.userData = userData;
            var knowledgebaseList = _knowledgebaseWrapper.GetKnowledgebaseGridData(customQueryDisplayId, out Dictionary<string, Dictionary<string, string>> lookupLists, order, orderDir, skip, length ?? 0,Title,Category, Description,Tags,Folder, SearchText);           
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (knowledgebaseList != null && knowledgebaseList.Count > 0)
            {
                recordsFiltered = knowledgebaseList[0].TotalCount;
                totalRecords = knowledgebaseList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = knowledgebaseList
              .ToList();
            foreach (var item in filteredResult)
            {
                eSearchModel = new KnowledgebaseSearchModel();
                eSearchModel.KBTopicsId = item.KBTopicsId;
                eSearchModel.Title = item.Title;
                eSearchModel.Category = item.Category;
                eSearchModel.CategoryName = item.CategoryName;
                eSearchModel.Description = item.Description;
                eSearchModel.Tags = item.Tags;
                eSearchModel.Folder = item.Folder;
                eSearchModelList.Add(eSearchModel);
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = eSearchModelList, lookupLists = lookupLists }, JsonSerializerDateSettings);
        }
        public string GetkbtopicsPrintData( int? draw, int? start, int? length, 
                                            int customQueryDisplayId = 1, string Title = "", string Category = "", string Description = "",
                                            string Tags = "", string Folder = "", string SearchText = "", string order = "", string orderDir = ""
                                           )
        {
            KnowledgebasePrintModel objKnowledgebasePrintModel;
            List<KnowledgebasePrintModel> KnowledgebasePrintModelList = new List<KnowledgebasePrintModel>();
            if (Category == "--Select--")
            {
                Category = "";
            }
            SearchText = SearchText.Replace("%", "[%]");
            Title = Title.Replace("%", "[%]");
            Category = Category.Replace("%", "[%]");
            Description = Description.Replace("%", "[%]");
            Tags = Tags.Replace("%", "[%]");
            Folder = Folder.Replace("%", "[%]");
            start = start.HasValue
               ? start / length
               : 0;
            int skip = start * length ?? 0;
            int lengthForPrint = 100000;
            _knowledgebaseWrapper.userData = userData;
            var knowledgebaseList = _knowledgebaseWrapper.GetKnowledgebaseGridData(customQueryDisplayId, out Dictionary<string, Dictionary<string, string>> lookupLists, order, orderDir, 0, lengthForPrint, Title, Category, Description, Tags,Folder, SearchText);
            foreach (var item in knowledgebaseList)
            {
                objKnowledgebasePrintModel = new KnowledgebasePrintModel();
                objKnowledgebasePrintModel.KBTopicsId = item.KBTopicsId;
                objKnowledgebasePrintModel.Title = item.Title;
                objKnowledgebasePrintModel.Category = item.Category;
                objKnowledgebasePrintModel.Description = item.Description;
                objKnowledgebasePrintModel.Tags = item.Tags;
                objKnowledgebasePrintModel.Folder = item.Folder;
                KnowledgebasePrintModelList.Add(objKnowledgebasePrintModel);
            }
            return JsonConvert.SerializeObject(new { data = KnowledgebasePrintModelList }, JsonSerializerDateSettings);
        }
        #endregion     
        #region Add Topics
        public PartialViewResult AddKbTopics()
        {
            KnowledgebaseCombined objComb = new KnowledgebaseCombined();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            _knowledgebaseWrapper.userData = userData;
            List<DataContracts.LookupList> TypeList = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> AllLookUps = new List<DataContracts.LookupList>();
            objComb.KBTopicsModel = new KnowledgebaseModel();
            var KbCategoryType = commonWrapper.GetListFromConstVals(LookupListConstants.KB_CategoryType);
            if (KbCategoryType != null)
            {
                objComb.KBTopicsModel.KBTopicsCategoryList = KbCategoryType.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
            LocalizeControls(objComb, LocalizeResourceSetConstants.KnowledgebaseDetails);
            return PartialView("~/Views/Knowledgebase/_KnowledgebaseAdd.cshtml", objComb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KbTopicsAdd(KnowledgebaseCombined objKb, string Command)
        {
            List<string> ErrorList = new List<string>();
            _knowledgebaseWrapper.userData = userData;
            string ModelValidationFailedMessage = string.Empty;
            string Mode = string.Empty;
           
            if (ModelState.IsValid)
            {
                if (objKb.KBTopicsModel.KBTopicsId > 0)
                {
                    Mode = "Edit";
                }
                else
                {
                    Mode = "Add";
                }
                KBTopics kbTopics = new KBTopics();
                kbTopics = _knowledgebaseWrapper.AddOrEditKbTopics(objKb);
                if (kbTopics.ErrorMessages != null && kbTopics.ErrorMessages.Count > 0)
                {
                    return Json(kbTopics.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), Command = Command, Mode = Mode, KbtopicsId = kbTopics.KBTopicsId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult KbTopicsEdit(long KBTopicsId)
        {
            _knowledgebaseWrapper.userData = userData;         
            KnowledgebaseCombined KbComb = new KnowledgebaseCombined();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> VehicleTypeList = new List<DataContracts.LookupList>();
            KbComb.KBTopicsModel = new KnowledgebaseModel();
            KbComb.KBTopicsModel = _knowledgebaseWrapper.GetKnowledgebaseDetails(KBTopicsId);
            KbComb.KBTopicsModel.KbTopicsTags = KbComb.KBTopicsModel.Tags;
            KbComb._userdata = userData;
            var KbCategoryType = commonWrapper.GetListFromConstVals(LookupListConstants.KB_CategoryType);
            if (KbCategoryType != null)
            {
                KbComb.KBTopicsModel.KBTopicsCategoryList = KbCategoryType.Select(x => new SelectListItem { Text = x.text.ToString(), Value = x.value.ToString() });
            }
                      
            LocalizeControls(KbComb, LocalizeResourceSetConstants.KnowledgebaseDetails);
            return PartialView("_KnowledgebaseEdit", KbComb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateKbTopics(KnowledgebaseCombined kbComb)
        {
            string emptyValue = string.Empty;
            KnowledgebaseCombined objComb = new KnowledgebaseCombined();
            _knowledgebaseWrapper.userData = userData;
            KBTopics kbtopics = new KBTopics();
            if (ModelState.IsValid)
            {
                kbtopics = _knowledgebaseWrapper.AddOrEditKbTopics(kbComb);
                if (kbtopics.ErrorMessages != null && kbtopics.ErrorMessages.Count > 0)
                {
                    return Json(kbtopics.ErrorMessages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), KBTopicsId = kbtopics.KBTopicsId }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = string.Empty;
                ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Details
        [HttpPost]
        public PartialViewResult KnowledgebaseDetails(long KBTopicsId)
        {
            KnowledgebaseCombined objComb = new KnowledgebaseCombined();
            _knowledgebaseWrapper.userData = userData;
            var KnowledgebaseDetails = _knowledgebaseWrapper.GetKnowledgebaseDetails(KBTopicsId);
            objComb.KBTopicsModel = KnowledgebaseDetails;
           
            objComb.KBTopicsModel.Tags = objComb.KBTopicsModel.Tags;
            LocalizeControls(objComb, LocalizeResourceSetConstants.KnowledgebaseDetails);
            return PartialView("KnowledgebaseDetails", objComb);
        }
        #endregion
        #region Hover for Assigned user
        private void PopulatePopUp(out IEnumerable<SelectListItem> AssignedList, out IEnumerable<SelectListItem> PersonnelList, long KBTopicsId = 0)
        {
            _knowledgebaseWrapper.userData = userData;
            var totalList = _knowledgebaseWrapper.KbTopicsPersonnelList(Convert.ToString(KBTopicsId));
            AssignedList = totalList[1].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            PersonnelList = totalList[0].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
        }
        #endregion

        #region  Tags Name Retrieve
        public JsonResult RetrieveTagName()
        {
            _knowledgebaseWrapper.userData = userData;
            var totalList = _knowledgebaseWrapper.KbTopicsTags();
          
            return Json(totalList, JsonRequestBehavior.AllowGet);
        }


        #endregion

       
    }
}