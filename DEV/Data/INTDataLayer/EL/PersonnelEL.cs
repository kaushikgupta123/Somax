using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTDataLayer.EL
{
    public class PersonnelEL
    {
        public long ClientId{ set; get; }
        public long PersonnelId{ set; get; }
        public long UserInfoId{ set; get; }
        public long SiteId{ set; get; }
        public long AreaId{ set; get; }
        public long DepartmentId{ set; get; }
        public long StoreroomId{ set; get; }
        public string ClientLookupId{ set; get; }
        public string Address1{ set; get; }
        public string Address2{ set; get; }
        public string Address3{ set; get; }
        public string AddressCity{ set; get; }
        public string AddressCountry{ set; get; }
        public string AddressPostCode{ set; get; }
        public string AddressState{ set; get; }
        public decimal ApprovalLimitPO{ set; get; }
        public decimal ApprovalLimitWO{ set; get; }
        public decimal BasePay{ set; get; }
        public string Craft{ set; get; }
        public string Crew{ set; get; }
        public string CurrentLevel{ set; get; }
        public DateTime? DateofBirth{ set; get; }
        public long DefaultStoreroomId{ set; get; }
        public decimal DistancefromWork{ set; get; }
        public string Email{ set; get; }
        public bool Floater{ set; get; }
        public string Gender{ set; get; }
        public bool InactiveFlag{ set; get; }
        public string InitialLevel{ set; get; }
        public decimal InitialPay{ set; get; }
        public DateTime? LastSalaryReview{ set; get; }
        public string MaritalStatus{ set; get; }
        public string NameFirst{ set; get; }
        public string NameLast{ set; get; }
        public string NameMiddle{ set; get; }
        public string Phone1{ set; get; }
        public string Phone2{ set; get; }
        public bool Planner{ set; get; }
        public bool Scheduler{ set; get; }
        public bool ScheduleEmployee{ set; get; }
        public string Section{ set; get; }
        public string Shift{ set; get; }
        public string SocialSecurityNumber{ set; get; }
        public DateTime? StartDate{ set; get; }
        public string Status{ set; get; }
        public long Supervisor_PersonnelId{ set; get; }
        public DateTime? TerminationDate{ set; get; }
        public string TerminationReason{ set; get; }
        public int UpdateIndex{ set; get; }
    }
}
