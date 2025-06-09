using System;
using System.Collections.Generic;

namespace Client.Models.Common
{
    public class NotificationModelVM
    {
        public List<NotificationModel> NotificationList { get; set; }
        public int PageNumber { get; set; }
        public int NewNotificationCount { get; set; }
        public int NewNotificationSelectedtabCount { get; set; }
    }
    public class NotificationModel
    {
        public long ClientId { get; set; }
        public long AlertsId { get; set; }
        public string Summary { get; set; }
        public string Details { get; set; }
        public string From { get; set; }
        public string AlertType { get; set; }
        public DateTime? ActiveDate { get; set; }
        public long ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string Action { get; set; }
        public string ObjectState { get; set; }
        public bool IsRead { get; set; }
        public int UpdateIndex { get; set; }
        public string HeadLine { get; set; }
        public long AlertUserId { get; set; }
        public string Notes { get; set; }
        public long AlertDefineId { get; set; }
        public string NotificationType { get; set; }

    }
}