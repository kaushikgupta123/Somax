using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts
{
    public class AlertForward
    {

        public long SendToId { get; set; }

        public string Comment { get; set; }

        public long  CurrentUserId { get; set; }


        public void ForwardAlert(DatabaseKey dbKey)
        {
            long _AlertId = 0;

            DataContracts.AlertUser alertuser = new AlertUser 
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName 
            };
            alertuser.AlertUserId = this.CurrentUserId;
            alertuser.Retrieve(dbKey);

            //Get the alert Id
            _AlertId = alertuser.AlertsId;

            alertuser.AlertUserId = 0;
            alertuser.UserId = SendToId;
            alertuser.UpdateIndex = 0;
            alertuser.IsRead = false;
            alertuser.IsDeleted = false;
            alertuser.Create(dbKey);

            DataContracts.Alerts alerts = new Alerts();
            alerts.AlertsId = _AlertId;
            alerts.Retrieve(dbKey);

            if(string.IsNullOrWhiteSpace(alerts.Notes))
            {
                alerts.Notes = "";
            }
            alerts.Notes += "<br/><br/>" + Comment;
            //System.Web.HttpUtility.HtmlEncode
            alerts.Update(dbKey);
        }

    }
}
