using System;

namespace Database
{
    public class DashboardUserSettings_RetrieveByDashboardIdandUserinfoId : DashboardUserSettings_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            DashboardUserSettings.RetrieveByDashboardIdandUserinfoidFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class DashboardUserSettings_RetrieveDefaultDashboardListingId : DashboardUserSettings_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            DashboardUserSettings.RetrieveDefaultDashboardListingId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class DashboardUserSettings_UpdateFromDatabase_V2 : DashboardUserSettings_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (DashboardUserSettings.DashboardUserSettingsId == 0)
            {
                string message = "DashboardUserSettings has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            DashboardUserSettings.UpdateFromDashboard_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class DashboardUserSettings_CreateFromDatabase_V2 : DashboardUserSettings_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (DashboardUserSettings.DashboardUserSettingsId > 0)
            {
                string message = "DashboardUserSettings has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            DashboardUserSettings.InsertFromDashboard_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
}
