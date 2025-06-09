using Database;
using Database.Business;
using Database.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;        // V2-XXXX RKL 
using System.Web.Caching;

namespace DataContracts
{
    [Serializable]
    public partial class AlertComposite : DataContractBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AlertComposite()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        #region Properties
        public string AlertName { get; set; }
        public long AlertDefineId { get; set; }
        public string ObjectClientLookupId { get; set; } //V2-1136
        #endregion

        /// <summary>
        /// The AlertComposite object combines elements from both the b_AlertUser 
        /// Fill from db object when a timezone is sent
        /// </summary>
        /// <param name="dbAlert"></param>
        /// <param name="dbUser"></param>
        public void UpdateFromDatabaseObject(b_Alerts dbAlert, b_AlertUser dbUser, string blankSummaryText, string blankDetailsText,string timezone)
        {
            // Set the appropriate items from the Alerts table
            this.ClientId = dbAlert.ClientId;
            this.AlertsId = dbAlert.AlertsId;
            this.Summary = (string.IsNullOrWhiteSpace(dbAlert.Summary.Trim()) ? blankSummaryText : dbAlert.Summary);
            this.Details = (string.IsNullOrWhiteSpace(dbAlert.Details.Trim()) ? blankDetailsText : dbAlert.Details);
            this.From = dbAlert.From;
            this.AlertType = dbAlert.AlertType;
            this.ObjectId = dbAlert.ObjectId;
            this.ObjectName = dbAlert.ObjectName;
            this.Action = dbAlert.Action;
            this.ObjectState = dbAlert.ObjectState;
            this.HeadLine = dbAlert.Headline;
            this.Notes = dbAlert.Notes;

            // Set the appropriate items from the AlertUser table
            this.IsRead = dbUser.IsRead;
            this.ActiveDate = dbUser.ActiveDate.ToUserTimeZone(timezone);
            this.AlertUserId = dbUser.AlertUserId;
            this.AlertName = dbUser.AlertName;
            this.AlertDefineId = dbAlert.AlertDefineId;
            this.ObjectClientLookupId = dbUser.ObjectClientLookupId;
        }
        /// <summary>
        /// The AlertComposite object combines elements from both the b_AlertUser 
        /// </summary>
        /// <param name="dbAlert"></param>
        /// <param name="dbUser"></param>
        public void UpdateFromDatabaseObject(b_Alerts dbAlert, b_AlertUser dbUser, string blankSummaryText, string blankDetailsText)
        {
            // Set the appropriate items from the Alerts table
            this.ClientId = dbAlert.ClientId;
            this.AlertsId = dbAlert.AlertsId;
            this.Summary = (string.IsNullOrWhiteSpace(dbAlert.Summary.Trim()) ? blankSummaryText : dbAlert.Summary);
            this.Details = (string.IsNullOrWhiteSpace(dbAlert.Details.Trim()) ? blankDetailsText : dbAlert.Details);
            this.From = dbAlert.From;
            this.AlertType = dbAlert.AlertType;
            this.ObjectId = dbAlert.ObjectId;
            this.ObjectName = dbAlert.ObjectName;
            this.Action = dbAlert.Action;
            this.ObjectState = dbAlert.ObjectState;
            this.HeadLine = dbAlert.Headline;
            this.Notes = dbAlert.Notes;

            // Set the appropriate items from the AlertUser table
            this.IsRead = dbUser.IsRead;
            this.ActiveDate = dbUser.ActiveDate;
            this.AlertUserId = dbUser.AlertUserId;
            this.AlertName = dbUser.AlertName;
            this.AlertDefineId = dbAlert.AlertDefineId;
        }

        private void Initialize()
        {
            b_Alerts dbAlert = new b_Alerts();
            b_AlertUser dbUser = new b_AlertUser();
            UpdateFromDatabaseObject(dbAlert, dbUser, "", "");  
        }

        public void ToDatabaseObject(b_Alerts dbAlert, b_AlertUser dbUser)
        {
            dbAlert.ClientId = this.ClientId;
            dbAlert.AlertsId = this.AlertsId;
            dbAlert.Summary = this.Summary;
            dbAlert.Details = this.Details;
            dbAlert.From = this.From;
            dbAlert.ObjectId = this.ObjectId;
            dbAlert.ObjectName = this.ObjectName;
            dbAlert.Action = this.Action;
            dbAlert.ObjectState = this.ObjectState;
            dbAlert.UpdateIndex = this.UpdateIndex;

            dbUser.ActiveDate = this.ActiveDate.Value;
            dbUser.IsRead = this.IsRead;
        }

        #endregion

        #region Transaction Methods

        //public static List<AlertComposite> RetrieveAlertCompositeByFilterCriteria(DatabaseKey dbKey)
        //{
        //    List<AlertComposite> results = new List<AlertComposite>();
        //    return results;
        //}
        // V2-XXXX - RKL 2020-05-09
        public List<AlertComposite> RetrieveAlertCompositeByFilterCriteria(DatabaseKey dbKey, string timezone)
        {
            List<AlertComposite> results = new List<AlertComposite>();
            AlertComposite_RetrieveBySearchCriteria trans = new AlertComposite_RetrieveBySearchCriteria();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            if (trans.AlertData != null)
            {
                List<KeyValuePair<b_Alerts, b_AlertUser>> AlertData = trans.AlertData;
                AlertData.ForEach(x =>
                {
                    AlertComposite AlertComposite = new DataContracts.AlertComposite();
                    AlertComposite.UpdateFromDatabaseObject(x.Key, x.Value, "", "",timezone);
                    results.Add(AlertComposite);
                });
            }
            return results;
        }

        public int RetrieveAlertCompositeByCount(DatabaseKey dbKey, out int resultMaintenanceCount, out int resultInventoryCount, out int resultProcurementCount, out int resultSystemCount )
        {
            AlertComposite_RetrieveCountBySearchCriteria trans = new AlertComposite_RetrieveCountBySearchCriteria();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            resultMaintenanceCount= trans.ResultMaintenanceCount;
            resultInventoryCount = trans.ResultInventoryCount;
            resultProcurementCount = trans.ResultProcurementCount;
            resultSystemCount = trans.ResultSystemCount;
            return trans.AlertData;
        }

        /// <summary>
        /// This method indicates that the user has read the full message.
        /// </summary>
        /// <param name="dbKey"></param>
        public void MarkAsRead(DatabaseKey dbKey)
        {
            AlertUser_MarkAsRead trans = new AlertUser_MarkAsRead();
            trans.AlertUser = new b_AlertUser() { AlertsId = this.AlertsId, UserId = dbKey.User.UserInfoId, ClientId = dbKey.User.ClientId };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // We're only updating the IsRead bit (or creating a new entry), so nothing to return.
        }

        /// <summary>
        /// This method marks the object as deleted, so it will no longer appear in future searches.
        /// </summary>
        /// <param name="dbKey"></param>
        public void Delete(DatabaseKey dbKey)
        {
            AlertUser_MarkAsDeleted trans = new AlertUser_MarkAsDeleted();
            trans.AlertUser = new b_AlertUser() { AlertsId = this.AlertsId, UserId = dbKey.User.UserInfoId, ClientId = dbKey.User.ClientId };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }

        #endregion

        #region Properties


        /// <summary>
        /// ClientId property
        /// </summary>
        [DataMember]
        public long ClientId { get; set; }

        /// <summary>
        /// AlertsId property
        /// </summary>
        [DataMember]
        public long AlertsId { get; set; }

        /// <summary>
        /// Summary property
        /// </summary>
        [DataMember]
        public string Summary { get; set; }

        /// <summary>
        /// Details property
        /// </summary>
        [DataMember]
        public string Details { get; set; }

        /// <summary>
        /// From property
        /// </summary>
        [DataMember]
        public string From { get; set; }

        /// <summary>
        /// AlertType property
        /// </summary>
        [DataMember]
        public string AlertType { get; set; }

        /// <summary>
        /// ActiveDate property
        /// </summary>
        [DataMember]
        public DateTime? ActiveDate { get; set; }

        /// <summary>
        /// ObjectID property
        /// </summary>
        [DataMember]
        public long ObjectId { get; set; }

        /// <summary>
        /// ObjectName property
        /// </summary>
        [DataMember]
        public string ObjectName { get; set; }

        /// <summary>
        /// Action property
        /// </summary>
        [DataMember]
        public string Action { get; set; }

        /// <summary>
        /// ObjectState property
        /// </summary>
        [DataMember]
        public string ObjectState { get; set; }

        /// <summary>
        /// IsRead property
        /// </summary>
        [DataMember]
        public bool IsRead { get; set; }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        [DataMember]
        public int UpdateIndex { get; set; }

        /// <summary>
        /// HeadLine Property
        /// </summary>
        [DataMember]
        public string HeadLine { get; set; }

        /// <summary>
        /// AlertUserId Property
        /// </summary>
        [DataMember]
        public long AlertUserId { get; set; }

        /// <summary>
        /// Notes Property
        /// </summary>
        [DataMember]
        public string Notes { get; set; }
        #endregion

        public static string GetRedirectURL(string objectName)
        {
            //
            // Note: Create case statements to handle unusual cases or in case the naming convention was not followed.
            // 
            switch (objectName)
            {
                default:
                    return objectName + "Edit.aspx";
            }


        }
    }
}
