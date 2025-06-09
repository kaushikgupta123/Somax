using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Constants;
using Client.CustomValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MaterialRequest
{
    public class MaterialRequestMultiStoreroomModel
    {
        [Required(ErrorMessage = "GlobalStoreroomSelect|" + LocalizeResourceSetConstants.Global)]
        public long? StoreroomId { get; set; }
    }
}