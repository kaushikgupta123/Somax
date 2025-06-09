using Admin.BusinessWrapper.Knowledgebase;
using Admin.Models;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Admin.BusinessWrapper
{
    public class KnowledgebaseWrapper: IKnowledgebaseWrapper
    {
        private DatabaseKey _dbKey;
        public readonly UserData _userData;

        List<string> errorMessage = new List<string>();
        public KnowledgebaseWrapper(UserData userData)
        {
            this._userData = userData;
            _dbKey = _userData.DatabaseKey;
        }
        public UserData userData { get; set; }

        #region Search
        public List<KBTopics> GetKnowledgebaseGridData(int CustomQueryDisplayId, out Dictionary<string, Dictionary<string, string>> lookupLists, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0,
            string Title = "", string Category="", string Description = "", string Tags = "",string Folder="", string searchText = "")
        {
            KBTopics kbtopics = new KBTopics();
            kbtopics.CustomQueryDisplayId = CustomQueryDisplayId;
            kbtopics.OrderbyColumn = orderbycol;
            kbtopics.OrderBy = orderDir;
            kbtopics.OffSetVal = skip;
            kbtopics.NextRow = length;
            kbtopics.Title = Title;
            kbtopics.Category = Category;
            if (Tags != "")
            {
                kbtopics.Tags = deserializeobj(Tags);
               // kbtopics.Tags = Tags;
            }
            else
                kbtopics.Tags = Tags;

            //kbtopics.Tags = Tags;
            kbtopics.Folder = Folder;
            kbtopics.SearchText = searchText;
            kbtopics.locallang= userData.DatabaseKey.Localization;
            kbtopics.KBTopicRetrieveChunkSearchV2(userData.DatabaseKey);
            lookupLists = new Dictionary<string, Dictionary<string, string>>();        
            return kbtopics.listOfKBTopics;
        }
        #endregion
        #region Details 
        public KnowledgebaseModel GetKnowledgebaseDetails(long kBTopicsId)
        {
            KnowledgebaseModel objKno = new KnowledgebaseModel();
            KBTopics kbtopics = new KBTopics()
            {
                KBTopicsId = kBTopicsId    
            };
            kbtopics.locallang = userData.DatabaseKey.Localization;
            kbtopics.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
            objKno = initializeControls(kbtopics);
            if (objKno != null && objKno.KBTopicsId > 0)
            {

                objKno.KBTopicsId = kbtopics.KBTopicsId;
                objKno.Title = kbtopics.Title;
                objKno.Category = kbtopics.Category;
                objKno.CategoryName = kbtopics.CategoryName;
                objKno.Description = kbtopics.Description;
                objKno.Tags = kbtopics.Tags;
                objKno.Folder = kbtopics.Folder;
            }
            return objKno;
        }

        public KnowledgebaseModel initializeControls(KBTopics obj)
        {
            KnowledgebaseModel objKno = new KnowledgebaseModel();

            objKno.KBTopicsId = obj?.KBTopicsId ?? 0;
            objKno.Title = obj?.Title ?? string.Empty;
            objKno.Category = obj?.Category ?? string.Empty;
            objKno.CategoryName = obj?.CategoryName ?? string.Empty;
            objKno.Description = obj?.Description ?? string.Empty;
            objKno.Tags = obj?.Tags ?? string.Empty;
            objKno.Folder = obj?.Folder ?? string.Empty;
            return objKno;
        }

        #endregion

        #region Add or edit Kb Topics
        public KBTopics AddOrEditKbTopics(KnowledgebaseCombined objKbcomb)
        {
           
            string emptyValue = string.Empty;
            string PersonnelList = String.Empty;
            List<string> errList = new List<string>();
            KBTopics kbTopics = new KBTopics();
            
            if (objKbcomb.KBTopicsModel.KBTopicsId == 0)
            {
                //Add in KbTopics Table
                kbTopics.Title = objKbcomb.KBTopicsModel.Title;
                kbTopics.Category = objKbcomb.KBTopicsModel.Category;
                kbTopics.Description = objKbcomb.KBTopicsModel.Description;
                kbTopics.Folder = objKbcomb.KBTopicsModel.Folder;
                if (objKbcomb.KBTopicsModel.KbTopicsTags != null)
                    kbTopics.Tags = deserializeobj(objKbcomb.KBTopicsModel.KbTopicsTags);
                else
                    kbTopics.Tags = "";
                kbTopics.Create(userData.DatabaseKey);
                
            }

            else
            {
                kbTopics.KBTopicsId = objKbcomb.KBTopicsModel.KBTopicsId;
                kbTopics.Retrieve(this.userData.DatabaseKey);
                kbTopics.Title = objKbcomb.KBTopicsModel.Title ?? "";
                kbTopics.Category = objKbcomb.KBTopicsModel.Category;
                kbTopics.Folder = objKbcomb.KBTopicsModel.Folder;
                kbTopics.Description = !string.IsNullOrEmpty(objKbcomb.KBTopicsModel.Description) ? objKbcomb.KBTopicsModel.Description.Trim() : emptyValue;

                if (objKbcomb.KBTopicsModel.KbTopicsTags != null)
                    kbTopics.Tags = deserializeobj(objKbcomb.KBTopicsModel.KbTopicsTags);
                else
                    kbTopics.Tags = "";



                kbTopics.Update(this.userData.DatabaseKey);

            }

            return kbTopics;
        }

        #endregion

      
        public List<List<KBTopics>> KbTopicsPersonnelList(string KBTopicsId = "")
        {
            KBTopics SO = new KBTopics();
            SO.KBTopicsId = string.IsNullOrEmpty(KBTopicsId) ? 0 : Convert.ToInt64(KBTopicsId);
            SO.Retrievetags(userData.DatabaseKey);
            return SO.TotalRecords;
        }
        public List<string> KbTopicsTags()
        {
            List<string> tags = new List<string>();
            KBTopics SO = new KBTopics();
            SO.Retrievetags(userData.DatabaseKey);
            foreach(var item in SO.TotalRecords[0])
            {
                tags.Add(item.FullName);
            }
            return tags;
        }
        public string RetrieveTagValue(long KBTopicsId = 0)
        {
            string PersonnelList = String.Empty;
            KBTopics SO = new KBTopics();
            SO.KBTopicsId = KBTopicsId;
            SO.Retrievetags(userData.DatabaseKey);
            var AssignedList = SO.TotalRecords[1].Select(x => new SelectListItem { Text = x.FullName, Value = x.PersonnelId.ToString() }).ToList();
            if (AssignedList.Count > 0)
            {
                foreach(var item in AssignedList)
                {
                    PersonnelList += item.Text + ",";
                }
                PersonnelList= PersonnelList.TrimEnd(',');
            }
            return PersonnelList;
        }

        public string deserializeobj(string val)
        {
            // Deserializing json data to object  
            JavaScriptSerializer js = new JavaScriptSerializer();

            //string val = objKb.KBTopicsModel.KbTopicsTags;
            var TName = "";
            dynamic blogObject = js.Deserialize<dynamic>(val);
            foreach (var item in blogObject)
            {
                TName += item["value"] + ',';
            }
            TName = TName.TrimEnd(',');
            return TName;
        }
    }
}