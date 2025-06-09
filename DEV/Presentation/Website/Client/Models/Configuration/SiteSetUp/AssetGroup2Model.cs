using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.SiteSetUp
{
    public class AssetGroup2Model
    {
        public long AssetGroup2Id { get; set; }
        public string AssetGroupLabel { get; set; }
        [Required(ErrorMessage = "ValidationAssetGroup2ID|" + LocalizeResourceSetConstants.SetUpDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "AssetGroup2IDRegErrMsg|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string ClientLookupId { get; set; }
        [Required(ErrorMessage = "validationenterDescription|" + LocalizeResourceSetConstants.Global)]
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
        public int UpdateIndex { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
    }
}