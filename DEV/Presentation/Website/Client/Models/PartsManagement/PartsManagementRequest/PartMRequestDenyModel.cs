using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartMRequestDenyModel
    {
        public long? PartMasterRequestId { get; set; }
        [Required(ErrorMessage = "Validcommentstodenythisrequest|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string Comment { get; set; }
        public string RequestType { get; set; }

    }
}