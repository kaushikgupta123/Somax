using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.Site
{
    public class CreatedLastUpdatedModel
    {
        public string Create { get; set; }
        public string LastUpdated { get; set; }
        public string CreatedUser { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyDate { get; set; }
        public string ModifyUser { get; set; }

        public string CreatedUserValue { get; set; }
        public string CreatedDateValue { get; set; }
        public string ModifyDatevalue { get; set; }
        public string ModifyUserValue { get; set; }
    }
}