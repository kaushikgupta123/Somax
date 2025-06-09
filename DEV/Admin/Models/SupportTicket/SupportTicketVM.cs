//using Admin.Models.Configuration.LookupLists;
using DataContracts;
//using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;
using static Admin.Models.Common.UserMentionDataModel;

namespace Admin.Models.SupportTicket
{
    public class SupportTicketVM : LocalisationBaseVM
    {
        public SupportTicketVM()
        {

        }
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public IEnumerable<SelectListItem> CustomQueryDisplayList { get; set; } 
        public SupportTicketModel SupportTicketModel { get; set; }
        public List<UserMentionData> userMentionDatas { get; set; }
        public List<STNotes> STNotesList { get; set; }
        public STNotesModel STNotesModel { get; set; }
    }
}