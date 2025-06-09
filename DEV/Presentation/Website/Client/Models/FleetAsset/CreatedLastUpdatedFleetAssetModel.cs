using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.FleetAsset
{
    public class CreatedLastUpdatedFleetAssetModel
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