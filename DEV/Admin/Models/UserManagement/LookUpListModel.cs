using System.Collections.Generic;

namespace Admin.Models
{
    public class LookUpListModel
    {
        public LookUpListModel()
        {
            data = new List<DataModel>();
        }
        public string msg { get; set; }
        public long count { get; set; }
        public List<DataModel> data { get; set; }
    }
    public class DataModel
    {

        #region Craft
        public string Description { get; set; }
        public string ClientLookUpId { get; set; }
     
        public long CraftId { get; set; }
        public string Craft { get; set; }
        public decimal ChargeRate { get; set; }
        #endregion

      
    }
}