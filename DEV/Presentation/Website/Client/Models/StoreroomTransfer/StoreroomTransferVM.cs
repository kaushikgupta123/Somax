using Client.Models.Common;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.StoreroomTransfer
{
    public class StoreroomTransferVM : LocalisationBaseVM
    {
        public StoreroomTransferVM()
        {

        }
        public Security security { get; set; }
        public UserData udata { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public StoreroomTransferModel StoreroomTransferModel { get; set; }
        public AddTransferRequest addTransferRequest { get; set; }
    }
}