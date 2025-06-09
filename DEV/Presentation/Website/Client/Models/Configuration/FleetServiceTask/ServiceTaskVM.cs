using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.FleetServiceTask
{
    public class ServiceTaskVM:LocalisationBaseVM
    {
        public ServiceTaskModel ServiceTaskModel { get; set; }
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public List<ServiceTaskPrintModel> ServiceTaskPrintModel { get; set; }
    }
}