using Database.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database
{
    public class UIConfiguration_RetrieveSelectedListByViewId_V2 : UIConfiguration_TransactionBaseClass
    {
        public List<b_UIConfiguration> UIConfigurationList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_UIConfiguration> tmpList = null;

            UIConfiguration.RetrieveSelectedListByViewId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            UIConfigurationList = new List<b_UIConfiguration>();
            foreach (b_UIConfiguration tmpObj in tmpList)
            {
                UIConfigurationList.Add(tmpObj);
            }
        }
    }
    public class UIConfiguration_RetrieveAvailableListByViewId_V2 : UIConfiguration_TransactionBaseClass
    {
        public List<b_UIConfiguration> UIConfigurationList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_UIConfiguration> tempList = null;
            UIConfiguration.RetrieveAvailableListByViewId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            this.UIConfigurationList = tempList;
        }
    }

    public class UIConfiguration_RetrieveDetailsByUIViewId_V2 : UIConfiguration_TransactionBaseClass
    {
        public List<b_UIConfiguration> UIConfigurationList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_UIConfiguration> tempList = null;
            UIConfiguration.RetrieveDetailsByUIViewId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            this.UIConfigurationList = tempList;
        }
    }

    #region Update Available and selected list
    public class UICOnfiguration_UpdateAvailableandSelectedListCustom : UIConfiguration_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }
        public override void PerformWorkItem()
        {
            UIConfiguration.UiConfigurationUpdateForAvailableandSelectedListInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion

    #region Check Duplicate Section Name
    public class UIConfiguration_ValidateSectionNameByUIViewId : UIConfiguration_TransactionBaseClass
    {
        public UIConfiguration_ValidateSectionNameByUIViewId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                UIConfiguration.ValidateSectionNameByUIViewId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Get List of Columns From Table
    public class UIConfiguration_RetrieveListColumnsFromTable_V2 : UIConfiguration_TransactionBaseClass
    {
        public List<b_UIConfiguration> UIConfigurationList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_UIConfiguration> tempList = null;
            UIConfiguration.RetrieveListColumnsFromTable_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            this.UIConfigurationList = tempList;
        }
    }
    #endregion

    #region Update columns while remove from selected list
    public class UIConfiguration_UpdateColumnSettingByDataDictionaryId_V2 : UIConfiguration_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            UIConfiguration.UpdateColumnSettingByDataDictionaryIdWhileRemove_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion
}
