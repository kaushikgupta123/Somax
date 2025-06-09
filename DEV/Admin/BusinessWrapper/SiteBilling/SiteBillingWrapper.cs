using Admin.Common;
using Admin.Models.Client;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;

namespace Admin.BusinessWrapper
{
    public class SiteBillingWrapper
    {
        private DatabaseKey _dbKey;
        public readonly UserData _userData;

        List<string> errorMessage = new List<string>();
        public SiteBillingWrapper(UserData userData)
        {
            this._userData = userData;
            _dbKey = _userData.DatabaseKey;
        }
        public UserData userData { get; set; }

        #region V2-964
        public SiteBillingModelView GetSiteBillingDetailsByClientIdSiteId(long ClientId = 0, long SiteId = 0)
        {
            SiteBillingModelView siteBillingModelView = new SiteBillingModelView();
            DataContracts.SiteBilling sitebilling = new DataContracts.SiteBilling()
            {
                CustomClientId = ClientId,
                SiteId = SiteId,
            };
            sitebilling.RetrieveSiteBillingByClientIdSiteId_V2(_dbKey);
            siteBillingModelView.SiteId = sitebilling.SiteId;
            siteBillingModelView.ClientId = sitebilling.ClientId;
            siteBillingModelView.SiteBillingId = sitebilling.SiteBillingId;
            if (sitebilling.AnniversaryDate != null && sitebilling.AnniversaryDate == default(DateTime))
            {
                siteBillingModelView.AnniversaryDate = null;
            }
            else
            {
                siteBillingModelView.AnniversaryDate = sitebilling.AnniversaryDate;
            }
            
            siteBillingModelView.InvoiceFreq = sitebilling.InvoiceFreq;
            siteBillingModelView.Terms = sitebilling.Terms;
            siteBillingModelView.CurrentInvoice = sitebilling.CurrentInvoice;
            
            if (sitebilling.InvoiceDate != null && sitebilling.InvoiceDate == default(DateTime))
            {
                siteBillingModelView.InvoiceDate = null;
            }
            else
            {
                siteBillingModelView.InvoiceDate = sitebilling.InvoiceDate;
            }
            
            if (sitebilling.NextInvoiceDate != null && sitebilling.NextInvoiceDate == default(DateTime))
            {
                siteBillingModelView.NextInvoiceDate = null;
            }
            else
            {
                siteBillingModelView.NextInvoiceDate = sitebilling.NextInvoiceDate;
            }
            siteBillingModelView.QuoteRequired = sitebilling.QuoteRequired;
            
            return siteBillingModelView;
        }
        #endregion
    }
}