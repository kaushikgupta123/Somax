using Database.Business;
using System.Collections.Generic;

namespace Database
{
 public class DataDictionary_RetrieveDetailsByClientId_V2 : DataDictionary_TransactionBaseClass
    {
            public List<b_DataDictionary> DataDictionaryList { get; set; }
            public override void PerformWorkItem()
            {
                List<b_DataDictionary> tempList = null;
                DataDictionary.RetrieveDetailsByClientId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
                this.DataDictionaryList = tempList;
            }
        }
    #region Update Column Setting UI
    public class DataDictionary_UpdateColumnSettingByDataDictionaryId_V2 : DataDictionary_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            DataDictionary.UpdateColumnSettingByDataDictionaryId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion

}
