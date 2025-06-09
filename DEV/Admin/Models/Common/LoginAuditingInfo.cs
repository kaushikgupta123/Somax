using System;

namespace Admin.Models
{
    public class LoginAuditingInfo
    {
        public long ClientId { get; set; }   
        public long LoginAuditingId { get; set; }
        public long LoginInfoId { get; set; }
        public long UserInfoId { get; set; }
        public Guid SessionId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Browser { get; set; }
        public string IPAddress { get; set; }
        public int TotalCount { get; set; }
    }
}