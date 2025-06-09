using Client.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.NewLaborScheduling
{
    public class NewLaborSchedulingVM : LocalisationBaseVM
    {
        public NewLaborSchedulingVM()
        {
            WorkOrderList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> ScheduledDateList { get; set; }
        public IEnumerable<SelectListItem> RequiredDateList { get; set; }
        public IEnumerable<SelectListItem> Personnellist { get; set; }
       
        public WoRescheduleModel woRescheduleModel { get; set; }
        public AvailableWorkAssignModel availableWorkAssignModel { get; set; }
        public AddSchedlingCalendarModal AddSchedlingCalendarModal { get; set; }
        public IEnumerable<SelectListItem> WorkOrderList { get; set; }
        public EditSchedlingCalendarModal EditSchedlingCalendarModal { get; set; }
        public IEnumerable<SelectListItem> ScheduledGroupingList { get; set; }
        public List<NewLaborSchedulingPrintModel> NewLaborSchedulingPrintModel { get; set; }
        public List<NewLaborSchedulingPdfPrintModel> NewLaborSchedulingPdfPrintModel { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public AvailableWorkAssignCalendarModel availableWorkAssignCalendarModel { get; set; }
        public AvailableWoScheduleModel availableWOModel { get; set; } //V2-984
        public ReassignModel reassignModel { get; set; } //V2-1060
    }
}