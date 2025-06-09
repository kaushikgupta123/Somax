using Admin.Models.Common;
using DataContracts;
using Admin.Models.Knowledgebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models
{
    public class KnowledgebaseCombined : LocalisationBaseVM
    {
        public KnowledgebaseCombined()
        {
            KBTopicsData = new DataContracts.KBTopics();
        }
        public DataContracts.KBTopics KBTopicsData { get; set; }
        public KnowledgebaseModel KBTopicsModel { get; set; }
        public IEnumerable<SelectListItem> LookupCalegoryList { get; set; }
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public string Mode { get; set; }
        public IEnumerable<SelectListItem> AssignedList { get; set; }
        public IEnumerable<SelectListItem> PersonnelList { get; set; }

    }
}