using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.ClientSetUp
{
    public class ClientSetUpModel 
    {       
        [Required(ErrorMessage = "ClientNameRequiredMessage|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string CompanyName { get; set; }      
        [Required(ErrorMessage = "PrimaryContactRequiredMessage|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string PrimaryContact { get; set; }
        public string OfficerPhone { get; set; }

        [EmailAddress(ErrorMessage = "validEmailRequiredMessage|" + LocalizeResourceSetConstants.SetUpDetails)]
        [Required(ErrorMessage = "EmailRequiredMessage|" + LocalizeResourceSetConstants.SetUpDetails)]       
        public string Email { get; set; }
        public string WOPrintMessage { get; set; }
        public bool AssetTree { get; set; }
        public string ImageUrl { get; set; }
        public bool IsImageDelete { get; set; }
        public long UpdateIndex { get; set; }

        public long ClientId { get; set; }

        public bool ClientOnPremise { get; set; }
        public UserData _userdata { get; set; }
        public string PMWOGenerateMethod { get; set; }
        public string MasterSanGenerateMethod { get; set; }
    }
}