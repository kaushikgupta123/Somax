using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class DataDictionary : DataContractBase
    {
        #region Property
        public long UiViewId { get; set; }
        public long UIConfigurationId { get; set; }
        public bool Required { get; set; } //temporarry added because this field was removed from db but used in a SP. So need to remove from SP and the code.

        #endregion
        public List<DataDictionary> DataDictionaryRetrieveDetailsByClientId(DatabaseKey dbKey)
        {
            DataDictionary_RetrieveDetailsByClientId_V2 trans = new DataDictionary_RetrieveDetailsByClientId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.DataDictionary = this.ToDatabaseObjectRetrieveDetailsByClientId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectRetrieveDetailsByClientId(trans.DataDictionaryList);
        }
        public b_DataDictionary ToDatabaseObjectRetrieveDetailsByClientId()
        {
            b_DataDictionary dbObj = new b_DataDictionary();
            dbObj.ClientId = this.ClientId;
            dbObj.UiViewId = this.UiViewId;          
            return dbObj;
        }
        public static List<DataDictionary> UpdateFromDatabaseObjectRetrieveDetailsByClientId(List<b_DataDictionary> dbObjs)
        {
            List<DataDictionary> result = new List<DataDictionary>();

            foreach (b_DataDictionary dbObj in dbObjs)
            {
                DataDictionary tmp = new DataDictionary();
                tmp.UpdateFromExtendedRetrieveDetailsByClientId(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedRetrieveDetailsByClientId(b_DataDictionary dbObj)
        {
            this.DataDictionaryId = dbObj.DataDictionaryId;
            this.ColumnName = dbObj.ColumnName;          
        }

        public void UpdateColumnSettingByDataDictionaryId(DatabaseKey dbKey)
        {
            DataDictionary_UpdateColumnSettingByDataDictionaryId_V2 trans = new DataDictionary_UpdateColumnSettingByDataDictionaryId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.DataDictionary = this.ToDatabaseObjectForUpdateColumnSettingByDataDictionaryId();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
        }
        public b_DataDictionary ToDatabaseObjectForUpdateColumnSettingByDataDictionaryId()
        {
            b_DataDictionary dbObj = new b_DataDictionary();
            dbObj.ClientId = this.ClientId;
            dbObj.DataDictionaryId = this.DataDictionaryId;
            dbObj.UIConfigurationId = this.UIConfigurationId;
            dbObj.Required = this.Required; ////temporarry added because this field was removed from db but used in a SP. So need to remove from SP and the code.
            dbObj.ColumnLabel = this.ColumnLabel;
            dbObj.DisplayonForm = this.DisplayonForm;
            return dbObj;
        }
    }
}
