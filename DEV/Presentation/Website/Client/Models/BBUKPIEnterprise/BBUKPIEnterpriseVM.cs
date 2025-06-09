using DataContracts;

using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.BBUKPIEnterprise
{
    public class BBUKPIEnterpriseVM : LocalisationBaseVM
    {
        public BBUKPIEnterpriseVM()
        {

        }
        public Security security { get; set; }
        public UserData udata { get; set; }
        public BBUKPIEnterpriseSearchGridModel BBUKPIEnterpriseSearchGridModel { get; set; }
        public BBUKPIEnterpriseModel BBUKPIEnterpriseModel { get; set; }
        public IEnumerable<SelectListItem> CustomQueryDisplayList { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForEnterpriseCreatedate { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForEnterpriseSubmitdate { get; set; }
        public List<EventLogModel> eventLogList { get; set; }
        public List<Notes> NotesList { get; set; }
        public IEnumerable<SelectListItem> YearWeekList { get; set; }
    }
}