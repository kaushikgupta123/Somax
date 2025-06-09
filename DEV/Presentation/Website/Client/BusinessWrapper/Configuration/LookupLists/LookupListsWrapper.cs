using System;
using System.Collections.Generic;
using DataContracts;
using Client.BusinessWrapper.Common;
using Client.Models.Configuration.LookupLists;
using System.Data;
using INTDataLayer.BAL;
using INTDataLayer.EL;
using System.Reflection;
using System.Linq;
namespace Client.BusinessWrapper.Configuration.LookUpLists
{
    public class LookUpListsWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public LookUpListsWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public List<LookUpListsModel> populateLookUpList(string DescriptionLookUpName)
        {
            LookUpListsModel lookUpListsModel;
            List<LookUpListsModel> LookUpListsModelList = new List<LookUpListsModel>();
            DataTable dtLookUp = new DataTable();
            dtLookUp = LookUpListBAL.GetLookUpListListMaint(DescriptionLookUpName, this.userData.DatabaseKey.User.ClientId, userData.DatabaseKey.Client.ConnectionString, userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            var dtLookUpList = ConvertDataTable<LookUpListsModel>(dtLookUp);
            foreach (var p in dtLookUpList)
            {
                lookUpListsModel = new LookUpListsModel();
                lookUpListsModel.LookupListId = p.LookupListId;
                lookUpListsModel.Description = p.Description;
                lookUpListsModel.ListValue = p.ListValue;
                lookUpListsModel.InactiveFlag = p.InactiveFlag;
                lookUpListsModel.IsReadOnly = p.IsReadOnly;
                LookUpListsModelList.Add(lookUpListsModel);
            }
            return LookUpListsModelList;
        }
        public List<LookUpListDescription> GetAllLookUpList()
        {
            LookUpListDescription lookUpListDescription;
            List<LookUpListDescription> lookUpListDescriptionList = new List<LookUpListDescription>();
            LocalizedList local_list = new LocalizedList()
            {
                Language = userData.Site.LocalizationLanguage,
                Culture = userData.Site.LocalizationCulture,
                SiteId = userData.Site.SiteId,
                APM= userData.Site.APM,
                CMMS= userData.Site.CMMS,
                Sanit = userData.Site.Sanitation,
                Fleet = userData.Site.Fleet,      // V2-475
                UsePartMaster=userData.Site.UsePartMaster,
                UseShoppingCart=userData.Site.ShoppingCart,
                PackageLevel=userData.DatabaseKey.Client.PackageLevel
            };
            List<LocalizedList> local_lists = local_list.RetrieveLanguageSpecificList(userData.DatabaseKey);
            //--V2-173--//
            //if((userData.Site.APM==false && userData.Site.CMMS==true) || (userData.Site.APM == true && userData.Site.CMMS == false) || (userData.Site.APM == true && userData.Site.CMMS == true))
            //{
            //    bool IsExistEQUIPTYPE = local_lists.Any(x => x.ListName == "EQUIP_TYPE");
            //    if(!IsExistEQUIPTYPE)
            //    {
            //        LocalizedList data = new LocalizedList();
            //        data.ListName = "EQUIP_TYPE";
            //        data.Description = "Equipment Types";
            //        local_lists.Add(data);
            //    }
            //    bool IsExistEVENTFAULTS= local_lists.Any(x => x.ListName == "EVENT_FAULTS");
            //    if (!IsExistEVENTFAULTS)
            //    {
            //        LocalizedList data = new LocalizedList();
            //        data.ListName = "EVENT_FAULTS";
            //        data.Description = "Event Faults";
            //        local_lists.Add(data);
            //    }
            //}
            List<LocalizedList> local_lists_sorted = local_lists.AsQueryable().OrderBy(x => x.Description).ToList();
            foreach (var p in local_lists_sorted)
            {
                lookUpListDescription = new LookUpListDescription();
                lookUpListDescription.Value = p.ListName;
                lookUpListDescription.Name = p.Description;
                lookUpListDescriptionList.Add(lookUpListDescription);
            }
            return lookUpListDescriptionList;
        }
        public LookUpListsModel populateLookUpDetails(long LookupListId)
        {
            LookUpListsModel lookUpListsModel = new LookUpListsModel();
            List<LookUpListsModel> LookUpListsModelList = new List<LookUpListsModel>();
            DataTable dtLookUp = new DataTable();
            dtLookUp = LookUpListBAL.GetLookUpListbyPKId(LookupListId, this.userData.DatabaseKey.User.ClientId, userData.DatabaseKey.Client.ConnectionString);
            var dtLookUpList = ConvertDataTable<LookUpListsModel>(dtLookUp);
            foreach (var p in dtLookUpList)
            {
                lookUpListsModel = new LookUpListsModel();
                lookUpListsModel.LookupListId = p.LookupListId;
                lookUpListsModel.Description = p.Description;
                lookUpListsModel.ListValue = p.ListValue;
                lookUpListsModel.InactiveFlag = p.InactiveFlag;
                lookUpListsModel.UpdateIndex = p.UpdateIndex;
            }
            return lookUpListsModel;
        }
        #endregion
        #region Add-Edit-Delete Look Up
        public LookUpListsModel AddLookUpLists(LookUpListsModel _LookUpListsModel)
        {
            LookUpListsModel objLookUpListsModel = new LookUpListsModel();
            LookupListEL objLookupListEL = new LookupListEL();
            objLookupListEL.Description = _LookUpListsModel.Description;
            objLookupListEL.InactiveFlag = Convert.ToBoolean(_LookUpListsModel.InactiveFlag);
            objLookupListEL.UpdateIndex = Convert.ToInt64(0);
            objLookupListEL.LookupListId = Convert.ToInt64(0);
            objLookupListEL.SiteID = 0;
            objLookupListEL.AreaId = Convert.ToInt64(0);
            objLookupListEL.StoreRoomId = Convert.ToInt64(0);
            objLookupListEL.DepartmentId = Convert.ToInt64(0);
            objLookupListEL.Filter = "";
            objLookupListEL.ListName = Convert.ToString(_LookUpListsModel.DescriptionLookUp);
            objLookupListEL.ListValue = Convert.ToString(_LookUpListsModel.ListValue).Trim();
            objLookupListEL.Language = userData.Site.LocalizationLanguage;
            objLookupListEL.Culture = userData.Site.LocalizationCulture;
            UserEL Uel = new UserEL();
            Uel.ClientId = this.userData.DatabaseKey.User.ClientId;
            Uel.UserFullName = this.userData.DatabaseKey.User.FullName;
            Uel.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            bool duplicatestatus = LookUpListBAL.CheckduplicateEntry(Uel, objLookupListEL, userData.DatabaseKey.Client.ConnectionString);
            if (duplicatestatus)
            {
                objLookUpListsModel.ErrorMessage = "duplicate";
            }
            else
            {
                Int64 insetid = LookUpListBAL.CreateLookUpList(Uel, objLookupListEL, userData.DatabaseKey.Client.ConnectionString);
                objLookUpListsModel.LookupListId = insetid;
            }
            return objLookUpListsModel;
        }
        public LookUpListsModel EditLookUpLists(LookUpListsModel _LookUpListsModel)
        {
            LookupList lu = new LookupList();
            lu.ClientId= this.userData.DatabaseKey.User.ClientId;
            lu.LookupListId = _LookUpListsModel.LookupListId;
            LookUpListsModel objLookUpListsModel = new LookUpListsModel();
            LookupListEL objLookupListEL = new LookupListEL();
            if(_LookUpListsModel.IsReadOnly)
            {
                lu.Retrieve(userData.DatabaseKey);
                objLookupListEL.Description = lu.Description;

            }
            else
            {
                objLookupListEL.Description = _LookUpListsModel.Description;
            }
           
            objLookupListEL.InactiveFlag = Convert.ToBoolean(_LookUpListsModel.InactiveFlag);
            objLookupListEL.UpdateIndex = _LookUpListsModel.UpdateIndex;
            objLookupListEL.LookupListId = _LookUpListsModel.LookupListId;
            UserEL Uel = new UserEL();
            Uel.ClientId = this.userData.DatabaseKey.User.ClientId;
            Uel.UserFullName = this.userData.DatabaseKey.User.FullName;
            Uel.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            Int64 insetid = LookUpListBAL.UpdateLookUpList(Uel, objLookupListEL, userData.DatabaseKey.Client.ConnectionString);
            objLookUpListsModel.LookupListId = insetid;
            return objLookUpListsModel;
        }
        internal bool DeleteLookUpLists(long LookupListId)
        {
            LookupListEL objLookupListEL = new LookupListEL();
            objLookupListEL.LookupListId = LookupListId;
            UserEL Uel = new UserEL();
            Uel.ClientId = this.userData.DatabaseKey.User.ClientId;
            Uel.UserFullName = this.userData.DatabaseKey.User.FullName;
            Uel.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            Int64 Deletenum = LookUpListBAL.DeleteLookUpListByPKId(Uel, objLookupListEL, userData.DatabaseKey.Client.ConnectionString);
            if (Deletenum <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
