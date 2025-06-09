using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Devices
{
    public class DeviceCategoryModel
    {
        [Required(ErrorMessage = "ValidationDeviceCategoryErrorMessage|" + LocalizeResourceSetConstants.DeviceDetails)]
        public string DeviceCategory { get; set; }
        public IEnumerable<SelectListItem> DeviceCategoryList { get; set; }
    }
}