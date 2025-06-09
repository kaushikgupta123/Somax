//using Admin.Models.Configuration.LookupLists;
using Admin.Models.ClientMessages;

using DataContracts;
//using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;
using static Admin.Models.Common.UserMentionDataModel;

namespace Admin.Models.Client
{
    public class ClientVM : LocalisationBaseVM
    {
        public ClientVM()
        {
            _ClientSummaryModel = new ClientSummaryModel();
            ClientData = new DataContracts.Client();

            #region V2-994
            _SiteMaintenanceModel = new SiteMaintenanceVM();
            #endregion


            #region V2-993
            _ClientMessage = new ClientMessageModel();
            #endregion
        }
        public ClientSummaryModel _ClientSummaryModel { get; set; }
        public SiteMaintenanceVM _SiteMaintenanceModel { get; set; }
        public ClientMessageModel _ClientMessage { get; set; }
        public DataContracts.Client ClientData { get; set; }
        public ClientModel ClientModel { get; set; }
        public Security security { get; set; }
        public IEnumerable<SelectListItem> InactiveFlagList { get; set; }
        public string Mode { get; set; }

        public IEnumerable<SelectListItem> LocalizationsList { get; set; }
        public IEnumerable<SelectListItem> TimeZonelist { get; set; }
        #region V2-964
        public List<ChildGridModel> listChildGridModel { get; set; }
        public IEnumerable<SelectListItem> ActiveClientList { get; set; }
        public IEnumerable<SelectListItem> ActiveSiteList { get; set; }
        public long SiteId { get; set; }
        //public List<TableHaederProp> tableHaederProps { get; set; }
        public List<ClientPDFPrintModel> ClientPDFPrintModel { get; set; }
        public CreateClientModel CreateClientModel { get; set; }
        public SiteModelView SiteModelView { get; set; }
        public SiteBillingModelView SiteBillingModelView { get; set; }
        #endregion

    }
}