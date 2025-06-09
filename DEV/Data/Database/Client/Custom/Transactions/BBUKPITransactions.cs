using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    #region Enterprise
    public class BBUKPI_RetrieveChunkEnterpriseSearch : BBUKPI_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_BBUKPI tmpList = null;
            BBUKPI.RetrieveChunkEnterpriseSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class BBUKPI_RetrieveForEnterpriseSiteFilter : BBUKPI_TransactionBaseClass
    {

        public List<b_BBUKPI> SiteList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_BBUKPI> tmpList = null;
            BBUKPI.RetrieveForEnterpriseSiteFilter(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            SiteList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class BBUKPI_RetrieveEnterpriseDetailsByBBUKPIId : BBUKPI_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (BBUKPI.BBUKPIId == 0)
            {
                string message = "BBUKPI has an invalid ID.";
                throw new Exception(message);
            }
            base.UseTransaction = false;
        }

        public override void PerformWorkItem()
        {
            BBUKPI.BBUKPIRetrieveEnterpriseDetailsByBBUKPIId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    #endregion

    #region Site
    public class BBUKPI_RetrieveChunkSiteSearch : BBUKPI_TransactionBaseClass
    {
        public override void PerformWorkItem()
        {
            b_BBUKPI tmpList = null;
            BBUKPI.RetrieveChunkSiteSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }
    }
    public class BBUKPI_RetrieveSiteDetailsByBBUKPIId : BBUKPI_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (BBUKPI.BBUKPIId == 0)
            {
                string message = "BBUKPI has an invalid ID.";
                throw new Exception(message);
            }
            base.UseTransaction = false;
        }

        public override void PerformWorkItem()
        {
            // This is too late to set this - the transaction has already been created
            //base.UseTransaction = false;
            BBUKPI.BBUKPIRetrieveSiteDetailsByBBUKPIId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class BBUKPI_RetrieveYearWeekForFilter : BBUKPI_TransactionBaseClass
    {

        public List<b_BBUKPI> YearWeekList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_BBUKPI> tmpList = null;
            BBUKPI.RetrieveYearWeekForFilter(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            YearWeekList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion
}
