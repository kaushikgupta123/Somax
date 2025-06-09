using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DataContracts;
namespace Client.Models.Configuration.ClientSetUp
{
    public class ClientSetUpVM: LocalisationBaseVM
    {
        public ClientSetUpVM()
        {
        }
        public Security security { get; set; }
        public ClientSetUpModel clientSetUpModel { get; set; }
        public PasswordSettingsModel passwordSettingsModel { get; set; }
        public WoCompletionSettingsModel woCompletionSettingsModel { get; set; }
        public List<SelectListItem> PMWOGenerationMethodList { get; set; }
        public WoCompletionCriteriaSetupModel woCompletionCriteriaSetupModel { get; set; }
        public WoFormSettingsSetupModel woFormSettingsSetupModel { get; set; } //V2-944
        public List<SelectListItem> MasterSanGenerateMethodList { get; set; }//V2-992
    }
}