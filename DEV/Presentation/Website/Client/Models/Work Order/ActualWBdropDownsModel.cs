using Client.Models.Common;
using System.Collections.Generic;

namespace Client.Models.Work_Order
{
    public class ActualWBdropDownsModel
    {
        public ActualWBdropDownsModel()
        {
            WorkAssigned = new List<DataTableDropdownModel>();
            ShiftList = new List<DataTableDropdownModel>();
            FailReasonList = new List<DataTableDropdownModel>();
        }
        public List<DataTableDropdownModel> WorkAssigned { get; set; }
        public List<DataTableDropdownModel> ShiftList { get; set; }
        public List<DataTableDropdownModel> FailReasonList { get; set; }
    }
}