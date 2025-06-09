using Client.CustomValidation;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Configuration.ClientSetUp
{
    public class WoFormSettingsSetupModel
    {      
        public long FormSettingsId { get; set; }
        public bool WOLaborRecording { get; set; }
        public bool WOUIC { get; set; }
        public bool WOScheduling { get; set; }
        public bool WOSummary { get; set; }
        public bool WOPhotos { get; set; }
        public bool WOComments { get; set; }
        //public UserData _userdata { get; set; }
        //V2-945
        public bool PRUIC { get; set; }
        public bool PRLine2 { get; set; }
        public bool PRLIUIC { get; set; }
        public bool PRComments { get; set; }

        //V2-946
        public bool POUIC { get; set; }
        public bool POLine2 { get; set; }
        public bool POLIUIC { get; set; }
        public bool POComments { get; set; } 
        public bool POTandC { get; set; }
        public string POTandCURL { get; set; }
        public bool IsFileRequired { get; set; }
        //[RequiredIf("POTandC", true, ErrorMessage = "VendorAttachmentFileExists|" + LocalizeResourceSetConstants.Global)]
        [RequiredIf("IsFileRequired", true, ErrorMessage = "VendorAttachmentFileExists|" + LocalizeResourceSetConstants.VendorDetails)]
        //[NotRequiredIfValueExists("POTandCURL", ErrorMessage = "VendorAttachmentFileExists|" + LocalizeResourceSetConstants.Global)]
        public HttpPostedFile FileContent { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        //V2-947
        public bool PORHeader { get; set; }
        public bool PORLine2 { get; set; }
        public bool PORPrint { get; set; }

        //V2-1011
        public bool PORUIC { get; set; }
        public bool PORComments { get; set; }
    }
}