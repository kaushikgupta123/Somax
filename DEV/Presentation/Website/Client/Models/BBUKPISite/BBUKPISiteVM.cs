using DataContracts;

using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.BBUKPISite
{
    public class BBUKPISiteVM : LocalisationBaseVM
    {
        public IEnumerable<SelectListItem> CustomQueryDisplayList { get; set; }
        //public IEnumerable<SelectListItem> SiteList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForSiteCreatedate { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForSiteSubmitdate { get; set; }
        public List<EventLogModel> eventLogList { get; set; }
        public List<Notes> NotesList { get; set; }
        public BBUKPISiteModel BBUKPISiteModel { get; set; }
        public BBUKPISiteEditModel BBUKPISiteEditModel { get; set; }
        //public Security security { get; set; }
        public UserData udata { get; set; }
        public IEnumerable<SelectListItem> YearWeekList { get; set; }
    }
}