using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Configuration.UIConfiguration
{
    public class AddSectionModel
    {
        [Required(ErrorMessage = "Please enter Section Name")]
        public string SectionName { get; set; }
    }
}