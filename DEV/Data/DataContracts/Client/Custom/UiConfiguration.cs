using Database;
using Database.Business;
using System;
using System.Collections.Generic;

namespace DataContracts
{
    public partial class UIConfiguration : DataContractBase, IStoredProcedureValidation
    {
        #region Property
        public String ColumnName { get; set; }
        public String ColumnLabel { get; set; }
        //public bool Required { get; set; }
        //public bool SystemRequired { get; set; }
        public String TableName { get; set; }
        public String ColumnType { get; set; }
        public String LookupType { get; set; }
        public String LookupName { get; set; }
        public bool UDF { get; set; }
        public bool Enabled { get; set; }
        public bool DisplayonForm { get; set; } //V2-944
        public string AvailableConfigurationId { get; set; }
        public System.Data.DataTable UICSelectedListData = new System.Data.DataTable();
        public string ValidateFor = string.Empty;
        #endregion

        public List<UIConfiguration> RetrieveSelectedListByViewId(DatabaseKey dbKey)
        {
            UIConfiguration_RetrieveSelectedListByViewId_V2 trans = new UIConfiguration_RetrieveSelectedListByViewId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.UIConfiguration = this.ToDatabaseObjectRetrieveSelectedListByViewId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UIConfiguration.UpdateFromDatabaseObjectList(trans.UIConfigurationList);
        }
        public b_UIConfiguration ToDatabaseObjectRetrieveSelectedListByViewId()
        {
            b_UIConfiguration dbObj = new b_UIConfiguration();
            dbObj.ClientId = this.ClientId;
            dbObj.UIViewId = this.UIViewId;

            return dbObj;
        }
        public b_UIConfiguration ToDatabaseObjectRetrieveSelectedList()
        {
            b_UIConfiguration dbObj = new b_UIConfiguration();
            dbObj.ClientId = this.ClientId;
            dbObj.UIViewId = this.UIViewId;
            return dbObj;
        }

        public static List<UIConfiguration> UpdateFromDatabaseObjectList(List<b_UIConfiguration> dbObjs)
        {
            List<UIConfiguration> result = new List<UIConfiguration>();

            foreach (b_UIConfiguration dbObj in dbObjs)
            {
                UIConfiguration tmp = new UIConfiguration();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_UIConfiguration dbObj)
        {
            this.UIConfigurationId = dbObj.UIConfigurationId;
            this.ColumnName = dbObj.ColumnName;
            this.ColumnLabel = dbObj.ColumnLabel;
            this.Order = dbObj.Order;
            this.Required = dbObj.Required;
            this.SystemRequired = dbObj.SystemRequired;
            this.DataDictionaryId = dbObj.DataDictionaryId;
            this.Section = dbObj.Section;
            this.SectionName = dbObj.SectionName;
            this.UDF = dbObj.UDF;
            this.ColumnType = dbObj.ColumnType;
            this.LookupType = dbObj.LookupType;
            this.LookupName = dbObj.LookupName;
            this.TableName = dbObj.TableName;
            this.DisplayonForm = dbObj.DisplayonForm; //V2-944
        }
        public List<UIConfiguration> UIConfigRetrieveHiddenByViewOrTable(DatabaseKey dbKey)
        {
            UIConfiguration_RetrieveAvailableListByViewId_V2 trans = new UIConfiguration_RetrieveAvailableListByViewId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UIConfiguration = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.UIConfigurationList);
        }


        public List<UIConfiguration> RetrieveDetailsByUIViewId(DatabaseKey dbKey)
        {
            UIConfiguration_RetrieveDetailsByUIViewId_V2 trans = new UIConfiguration_RetrieveDetailsByUIViewId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.UIConfiguration = this.ToDatabaseObjectRetrieveDetailsByUIViewId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UIConfiguration.UpdateFromDatabaseObjectRetrieveDetailsByUIViewIdList(trans.UIConfigurationList);
        }
        public b_UIConfiguration ToDatabaseObjectRetrieveDetailsByUIViewId()
        {
            b_UIConfiguration dbObj = new b_UIConfiguration();
            dbObj.ClientId = this.ClientId;
            dbObj.UIViewId = this.UIViewId;

            return dbObj;
        }
        public static List<UIConfiguration> UpdateFromDatabaseObjectRetrieveDetailsByUIViewIdList(List<b_UIConfiguration> dbObjs)
        {
            List<UIConfiguration> result = new List<UIConfiguration>();

            foreach (b_UIConfiguration dbObj in dbObjs)
            {
                UIConfiguration tmp = new UIConfiguration();
                tmp.UpdateFromExtendedRetrieveDetailsByUIViewIdDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedRetrieveDetailsByUIViewIdDatabaseObject(b_UIConfiguration dbObj)
        {
            this.UIConfigurationId = dbObj.UIConfigurationId;
            this.TableName = dbObj.TableName;
            this.ColumnName = dbObj.ColumnName;
            this.ColumnLabel = dbObj.ColumnLabel;
            this.ColumnType = dbObj.ColumnType;
            this.Required = dbObj.Required;
            this.LookupType = dbObj.LookupType;
            this.LookupName = dbObj.LookupName;
            this.UDF = dbObj.UDF;
            this.Enabled = dbObj.Enabled;
            this.SystemRequired = dbObj.SystemRequired;
            this.Order = dbObj.Order;
            this.Display = dbObj.Display;
            this.ViewOnly = dbObj.ViewOnly;
            this.Section = dbObj.Section;
            this.SectionName = dbObj.SectionName;
        }

        public void UpdateAvailableandSelectedListCustom(DatabaseKey dbKey)
        {
            UICOnfiguration_UpdateAvailableandSelectedListCustom trans = new UICOnfiguration_UpdateAvailableandSelectedListCustom()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UIConfiguration = this.ToDatabaseObjectForUiConfigurationList();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure changed the Update Index.
            //UpdateFromDatabaseObject(trans.UIConfiguration);
        }
        public b_UIConfiguration ToDatabaseObjectForUiConfigurationList()
        {
            b_UIConfiguration dbObj = new b_UIConfiguration();
            dbObj.ClientId = this.ClientId;
            dbObj.UIViewId = this.UIViewId;
            dbObj.UICSelectedListData.Dispose();
            dbObj.UICSelectedListData = new System.Data.DataTable();
            dbObj.UICSelectedListData = this.UICSelectedListData;
            dbObj.AvailableConfigurationId = this.AvailableConfigurationId;
            //dbObj.SelectedConfigurationId = this.SelectedConfigurationId;
            return dbObj;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            //--------Check For Section Name already exists-------------------------------------------------
            if (ValidateFor == "CheckDuplicate")
            {
                UIConfiguration_ValidateSectionNameByUIViewId trans = new UIConfiguration_ValidateSectionNameByUIViewId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName  
                };
                trans.UIConfiguration = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }
            
            return errors;
        }

        public void CheckDuplicateSectionName(DatabaseKey dbKey)
        {
            ValidateFor = "CheckDuplicate";
            Validate<UIConfiguration>(dbKey);
        }

        #region Get List of Columns From Table
        public List<UIConfiguration> RetrieveListColumnsFromTable(DatabaseKey dbKey)
        {
            UIConfiguration_RetrieveListColumnsFromTable_V2 trans = new UIConfiguration_RetrieveListColumnsFromTable_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.UIConfiguration = this.ToDatabaseObjectRetrieveListColumnsFromTable();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UIConfiguration.UpdateFromDatabaseObjectRetrieveRetrieveListColumnsFromTable(trans.UIConfigurationList);
        }
        public b_UIConfiguration ToDatabaseObjectRetrieveListColumnsFromTable()
        {
            b_UIConfiguration dbObj = new b_UIConfiguration();
            dbObj.ClientId = this.ClientId;
            dbObj.TableName = this.TableName;
            return dbObj;
        }
        public static List<UIConfiguration> UpdateFromDatabaseObjectRetrieveRetrieveListColumnsFromTable(List<b_UIConfiguration> dbObjs)
        {
            List<UIConfiguration> result = new List<UIConfiguration>();

            foreach (b_UIConfiguration dbObj in dbObjs)
            {
                UIConfiguration tmp = new UIConfiguration();
                tmp.ColumnName = dbObj.ColumnName;
                result.Add(tmp);
            }
            return result;
        }

        #endregion

        #region Update columns while remove from selected list
        public void UpdateColumnSettingByDataDictionaryIdWhileRemove(DatabaseKey dbKey)
        {
            UIConfiguration_UpdateColumnSettingByDataDictionaryId_V2 trans = new UIConfiguration_UpdateColumnSettingByDataDictionaryId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UIConfiguration = this.ToDatabaseObjectForUpdateColumnSettingByDataDictionaryIdWhileRemove();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

        }
        public b_UIConfiguration ToDatabaseObjectForUpdateColumnSettingByDataDictionaryIdWhileRemove()
        {
            b_UIConfiguration dbObj = new b_UIConfiguration();
            dbObj.ClientId = this.ClientId;
            dbObj.DataDictionaryId = this.DataDictionaryId;
            dbObj.Required = this.Required; 
            return dbObj;
        }
        #endregion
    }
}
