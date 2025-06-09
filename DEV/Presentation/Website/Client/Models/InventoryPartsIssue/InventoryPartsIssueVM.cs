using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.InventoryPartsIssue
{
    public class InventoryPartsIssueVM :LocalisationBaseVM
    {
        public PartsIssue partsIssue { get; set; }
        public UserData userData { get; set; }
    }
}
