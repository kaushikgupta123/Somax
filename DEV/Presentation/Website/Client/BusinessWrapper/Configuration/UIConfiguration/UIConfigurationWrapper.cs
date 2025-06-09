using Client.Models.Configuration.UIConfiguration;

using DataContracts;

using INTDataLayer.BAL;
using INTDataLayer.EL;

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Client.BusinessWrapper.Configuration.UIConfiguration
{
    public class UIConfigurationWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public UIConfigurationWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Get all ui view name
        public List<UIViewNameModel> GetAllCustomViewName()
        {
            UIView uiView = new UIView();
            List<UIViewNameModel> UiViewNameList = new List<UIViewNameModel>();
            UIViewNameModel uiViewnameModel;
            List<UIView> dashboardlisting = uiView.RetrieveAllCustom(userData.DatabaseKey);
            foreach (var item in dashboardlisting)
            {
                uiViewnameModel = new UIViewNameModel();
                uiViewnameModel.Name = item.ViewDescription;
                uiViewnameModel.Value = item.UIViewId.ToString();
                UiViewNameList.Add(uiViewnameModel);
            }
            return UiViewNameList;
        }
        #endregion

        #region Get Available Ui Configuration list
        public List<AvailableUIConfigurationModel> GetAvailableUilist(long ViewId)
        {
            List<AvailableUIConfigurationModel> AvailableUiList = new List<AvailableUIConfigurationModel>();

            DataContracts.UIConfiguration UIC = new DataContracts.UIConfiguration()
            {
                UIViewId = ViewId,
                ClientId = this.userData.DatabaseKey.Client.ClientId,
            };
            List<DataContracts.UIConfiguration> UIconf = UIC.UIConfigRetrieveHiddenByViewOrTable(this.userData.DatabaseKey);

            if (UIconf != null)
            {
                AvailableUIConfigurationModel AvailableUi;
                foreach (var v in UIconf)
                {
                    AvailableUi = new AvailableUIConfigurationModel();
                    AvailableUi.UIConfigurationId = v.UIConfigurationId;
                    AvailableUi.ColumnName = v.ColumnName;
                    if (v.TableName == "Equipment" && v.ColumnName == "AssetGroup1")
                    {
                        AvailableUi.ColumnLabel = userData.Site.AssetGroup1Name;
                    }
                    else if (v.TableName == "Equipment" && v.ColumnName == "AssetGroup2")
                    {
                        AvailableUi.ColumnLabel = userData.Site.AssetGroup2Name;
                    }
                    else if (v.TableName == "Equipment" && v.ColumnName == "AssetGroup3")
                    {
                        AvailableUi.ColumnLabel = userData.Site.AssetGroup3Name;
                    }
                    else
                    {
                        AvailableUi.ColumnLabel = v.ColumnLabel;
                    }
                    AvailableUi.Order = v.Order;
                    AvailableUi.Required = v.Required;
                    AvailableUi.SystemRequired = v.SystemRequired;
                    AvailableUi.DataDictionaryId = v.DataDictionaryId;
                    AvailableUi.Section = v.Section;
                    AvailableUi.SectionName = v.SectionName;
                    AvailableUi.UDF = v.UDF;
                    AvailableUiList.Add(AvailableUi);
                }
            }

            return AvailableUiList;
        }
        #endregion

        #region Get Selected Ui Configuration list
        public List<SelectedUIConfigurationMedel> GetSelectedUilist(long ViewId)
        {
            List<SelectedUIConfigurationMedel> SelectedUiList = new List<SelectedUIConfigurationMedel>();

            DataContracts.UIConfiguration UIC = new DataContracts.UIConfiguration()
            {
                UIViewId = ViewId,
                ClientId = this.userData.DatabaseKey.Client.ClientId,
            };
            List<DataContracts.UIConfiguration> UIconf = UIC.RetrieveSelectedListByViewId(this.userData.DatabaseKey);

            if (UIconf != null)
            {
                SelectedUIConfigurationMedel SelectedUi;
                foreach (var v in UIconf)
                {
                    SelectedUi = new SelectedUIConfigurationMedel();
                    SelectedUi.UIConfigurationId = v.UIConfigurationId;
                    SelectedUi.ColumnName = v.ColumnName;
                    if (v.TableName == "Equipment" && v.ColumnName == "AssetGroup1")
                    {
                        SelectedUi.ColumnLabel = userData.Site.AssetGroup1Name;
                    }
                    else if (v.TableName == "Equipment" && v.ColumnName == "AssetGroup2")
                    {
                        SelectedUi.ColumnLabel = userData.Site.AssetGroup2Name;
                    }
                    else if (v.TableName == "Equipment" && v.ColumnName == "AssetGroup3")
                    {
                        SelectedUi.ColumnLabel = userData.Site.AssetGroup3Name;
                    }
                    else
                    {
                        SelectedUi.ColumnLabel = v.ColumnLabel;
                    }
                    SelectedUi.Order = v.Order;
                    SelectedUi.Required = v.Required;
                    SelectedUi.SystemRequired = v.SystemRequired;
                    SelectedUi.DataDictionaryId = v.DataDictionaryId;
                    SelectedUi.Section = v.Section;
                    SelectedUi.SectionName = v.SectionName;
                    SelectedUi.UDF = v.UDF;
                    SelectedUi.ColumnType = v.ColumnType;
                    SelectedUi.LookupType = v.LookupType;
                    SelectedUi.LookupName = v.LookupName;
                    SelectedUi.DisplayonForm = v.DisplayonForm;
                    SelectedUiList.Add(SelectedUi);
                }
            }

            return SelectedUiList;
        }
        #endregion

        #region Update Selected and Available List
        public List<string> UpdateSelectedandAvailableList(UiConfigurationVM objUic)
        {
            DataTable dt = new DataTable();
            DataColumn dcol1 = new DataColumn("ConfigurationId", typeof(long));
            dt.Columns.Add(dcol1);
            DataColumn dcol2 = new DataColumn("Order", typeof(int));
            dt.Columns.Add(dcol2);
            DataColumn dcol3 = new DataColumn("Required", typeof(int));
            dt.Columns.Add(dcol3);

            foreach (var item in objUic.selectedListParam)
            {
                DataRow dr = dt.NewRow();
                dr["ConfigurationId"] = item.ItemId;
                dr["Order"] = item.OrderId + 1;
                dr["Required"] = false;
                dt.Rows.Add(dr);
            }

            DataContracts.UIConfiguration uic = new DataContracts.UIConfiguration()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                UIViewId = objUic.UIViewId,
                UICSelectedListData = dt
            };
            uic.AvailableConfigurationId = objUic.hiddenAvailableList;
            uic.UpdateAvailableandSelectedListCustom(this.userData.DatabaseKey);
            return uic.ErrorMessages;

        }
        #endregion

        #region Get UI View details
        public UiViewDetails GetUiViewDetails(long ViewId)
        {
            UiViewDetails uiViewdetails = new UiViewDetails();
            UIView uiView = new UIView()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                UIViewId = ViewId
            };
            uiView.Retrieve(this.userData.DatabaseKey);
            if (uiView != null)
            {
                uiViewdetails.UiViewId = uiView.UIViewId;
                uiViewdetails.ViewName = uiView.ViewName;
                uiViewdetails.Description = uiView.ViewDescription;
                uiViewdetails.ViewType = uiView.ViewType;
            }
            return uiViewdetails;
        }
        #endregion

        #region Get Selected Column Details
        public DataDictionary updateColumnSetting(long DataDictionaryId, long UIConfigurationId, bool isRequired, string description, bool displayonForm)
        {
            DataContracts.DataDictionary objDataDictionary = new DataContracts.DataDictionary();
            objDataDictionary.ClientId = this.userData.DatabaseKey.Client.ClientId;
            objDataDictionary.DataDictionaryId = DataDictionaryId;
            objDataDictionary.UIConfigurationId = UIConfigurationId;
            objDataDictionary.Required = isRequired;
            objDataDictionary.ColumnLabel = description;
            objDataDictionary.DisplayonForm = displayonForm;
            objDataDictionary.UpdateColumnSettingByDataDictionaryId(this.userData.DatabaseKey);
            return objDataDictionary;
        }
        //public List<string> updateColumnSettingold(long DataDictionaryId, bool isRequired, string description)
        //{
        //    DataContracts.DataDictionary objDataDictionary = new DataContracts.DataDictionary();
        //    objDataDictionary.ClientId = this.userData.DatabaseKey.Client.ClientId;
        //    objDataDictionary.DataDictionaryId = DataDictionaryId;
        //    objDataDictionary.Required = isRequired;
        //    objDataDictionary.ColumnLabel = description;
        //    objDataDictionary.UpdateColumnSettingByDataDictionaryId(this.userData.DatabaseKey);
        //    return objDataDictionary.ErrorObj;
        //}
        public List<string> removeColumnfromSelectedCardUI(long UIConfigId)
        {
            List<string> errList = new List<string>();
            DataContracts.UIConfiguration objUIConfig = new DataContracts.UIConfiguration()
            {
                ClientId = _dbKey.Client.ClientId,
                UIConfigurationId = UIConfigId
            };
            objUIConfig.Retrieve(this.userData.DatabaseKey);
            objUIConfig.Display = false;
            objUIConfig.Order = 0;
            if (objUIConfig.Required == false)
            {
                objUIConfig.Required = false;
                objUIConfig.Update(this.userData.DatabaseKey);
            }
            else
            {
                objUIConfig.Required = false;
                objUIConfig.UpdateColumnSettingByDataDictionaryIdWhileRemove(this.userData.DatabaseKey);
            }
            errList = objUIConfig.ErrorObj;

            return errList;
        }
        #endregion

        #region Get Data Dictionary  By ClientId
        public List<DataDictionaryModel> GeDataDictionaryRetrieveDetailsByClientId(long UiViewId)
        {
            List<DataDictionaryModel> DDModelList = new List<DataDictionaryModel>();

            DataContracts.DataDictionary DD = new DataContracts.DataDictionary()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                UiViewId = UiViewId
            };
            List<DataContracts.DataDictionary> DDlist = DD.DataDictionaryRetrieveDetailsByClientId(this.userData.DatabaseKey);

            if (DDlist != null)
            {
                DataDictionaryModel DDModel;
                foreach (var v in DDlist)
                {
                    DDModel = new DataDictionaryModel();
                    DDModel.DataDictionaryId = v.DataDictionaryId;
                    DDModel.ColumnName = v.ColumnName;
                    DDModelList.Add(DDModel);
                }
            }

            return DDModelList;
        }
        #endregion

        #region Add User Define Field
        public void AddUDF(string listofIds)
        {
            if (!String.IsNullOrEmpty(listofIds))
            {
                string[] list = listofIds.Split(',');
                foreach (var id in list)
                {
                    DataDictionary DD = new DataDictionary();
                    DD.ClientId = userData.DatabaseKey.Client.ClientId;
                    DD.DataDictionaryId = Convert.ToInt64(id);
                    DD.Retrieve(userData.DatabaseKey);
                    DD.Enabled = true;
                    DD.Update(userData.DatabaseKey);
                }
            }
        }
        #endregion

        #region Add Section
        public DataContracts.UIConfiguration AddSection(UiConfigurationVM objUICVM)
        {

            #region Insert into Ui Configuration Table
            DataContracts.UIConfiguration UIC = new DataContracts.UIConfiguration();
            UIC.ClientId = this.userData.DatabaseKey.User.ClientId;
            UIC.UIViewId = objUICVM.UIViewId;
            UIC.DataDictionaryId = 0;
            UIC.Display = false;
            UIC.Order = 0;
            UIC.ViewOnly = false;
            UIC.Section = true;
            UIC.SectionName = objUICVM.addSectiomModel.SectionName;
            UIC.CheckDuplicateSectionName(this.userData.DatabaseKey);
            if (UIC.ErrorMessages == null || UIC.ErrorMessages.Count == 0)
            {
                UIC.Create(this.userData.DatabaseKey);
            }

            #endregion

            return UIC;
        }
        #endregion

        #region Populate Lookuplist
        public List<UDFLookupListModel> populateUDFLookUpList(string DescriptionLookUpName)
        {
            UDFLookupListModel lookUpListsModel;
            List<UDFLookupListModel> LookUpListsModelList = new List<UDFLookupListModel>();
            DataTable dtLookUp = new DataTable();
            dtLookUp = LookUpListBAL.GetLookUpListList(DescriptionLookUpName, this.userData.DatabaseKey.User.ClientId, userData.DatabaseKey.Client.ConnectionString, userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            var dtLookUpList = ConvertDataTable<UDFLookupListModel>(dtLookUp);
            foreach (var p in dtLookUpList)
            {
                lookUpListsModel = new UDFLookupListModel();
                lookUpListsModel.LookupListId = p.LookupListId;
                lookUpListsModel.Description = p.Description;
                lookUpListsModel.ListValue = p.ListValue;
                lookUpListsModel.InactiveFlag = p.InactiveFlag;
                lookUpListsModel.ListName = p.ListName;
                lookUpListsModel.UpdateIndex = p.UpdateIndex;
                LookUpListsModelList.Add(lookUpListsModel);
            }
            return LookUpListsModelList;
        }


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
        #endregion

        #region Add Edit Lookuplist

        public UDFLookupListModel AddLookUpLists(UDFLookupListModel _LookUpListsModel)
        {
            UDFLookupListModel objLookUpListsModel = new UDFLookupListModel();
            LookupList ll = new LookupList();
            ll.Create(this.userData.DatabaseKey);
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
            objLookupListEL.ListName = Convert.ToString(_LookUpListsModel.ListName);
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
        public UDFLookupListModel EditLookUpLists(UDFLookupListModel _LookUpListsModel)
        {
            LookupList lu = new LookupList();
            lu.ClientId = this.userData.DatabaseKey.User.ClientId;
            lu.LookupListId = _LookUpListsModel.LookupListId;
            UDFLookupListModel objLookUpListsModel = new UDFLookupListModel();
            LookupListEL objLookupListEL = new LookupListEL();
            objLookupListEL.Description = _LookUpListsModel.Description;
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
            Int64 Deletenum = 1;
            LookupList lookupList = new LookupList()
            {
                ClientId = this.userData.DatabaseKey.User.ClientId,
                LookupListId = LookupListId
            };
            Deletenum = lookupList.DeleteByLookupListId(userData.DatabaseKey);
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