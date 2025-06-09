using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PartTransfer
{
    public class PartTransferVM: LocalisationBaseVM
    {
        public PartTransferModel parttransfermodel { get; set; }
        public PartTransferIssueModel partTransferIssueModel { get; set; }
        public PrintPartTransferModel printPartTransferModel { get; set; }
        public PartTransferReceiveModel partTransferReceiveModel { get; set; }

        public PartTransferDenyModel partTransferDenyModel { get; set; }
        public PartTransferForceCompleteModel partTransferForceCompleteModel { get; set; }
        public PartTransferCancelModel partTransferCancelModel { get; set; }
        public IEnumerable<SelectListItem> scheduleList { get; set; }
        public bool showBtnSave { get; set; }
        public bool showBtnIssue { get; set; }
        public bool showBtnReceive { get; set; }
        public bool showBtnSend { get; set; }
        public bool showCancelMenu { get; set; }
        public bool showDenyMenu { get; set; }
        public bool showForceCompleteMenu { get; set; }
        public bool showConfirmForceCompleteMenu { get; set; }
        #region V2-862
        public UserData userdata { get; set; }
        #endregion
    }
}