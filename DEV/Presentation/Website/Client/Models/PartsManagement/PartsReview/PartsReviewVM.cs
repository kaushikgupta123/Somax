using Client.Models.Parts;
using System.Collections.Generic;

namespace Client.Models.PartsManagement.PartsReview
{
    public class PartsReviewVM : LocalisationBaseVM
    {
        public PartsReviewVM()
        {
            reviewSiteModelList = new List<ReviewSiteModel>();
        }
        public List<ReviewSiteModel> reviewSiteModelList { get; set; }
    }
}