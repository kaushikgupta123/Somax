using Database.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    #region Search Project
    public class Project_RetrieveChunkSearch : Project_TransactionBaseClass
    {

        public List<b_Project> ProjectList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Project.ProjectId > 0)
            {
                string message = "Project has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Project> tmpList = null;
            Project.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ProjectList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Project tab header
    public class Project_RetrieveByProjectIdForHeader : Project_TransactionBaseClass
    {

        public b_Project projectObj { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Project tmp = null;
            Project.RetrieveProjectByProjectIdForHeader(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmp);
            projectObj = new b_Project();
            projectObj = tmp;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region WorkOrder project details LookupList By Search Criteria
    public class WorkOrder_ProjectDetailsLookupListBySearchCriteria : Project_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_Project> workOrderList { get; set; }
        public override void PerformWorkItem()
        {

            List<b_Project> tmpList = null;
            Project.RetrieveWorkOrder_ProjectDetailsLookupListBySearchCriteria(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            workOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Validate ClientLookupId
    public class Project_ValidateClientLookupId : Project_TransactionBaseClass
    {
        public Project_ValidateClientLookupId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                Project.ValidateClientLookupId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }
    #endregion

    #region Project Chunk Search Lookuplist

    public class Project_RetrieveChunkSearchLookupListV2 : Project_TransactionBaseClass
    {
        public List<b_Project> ProjectList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Project.ProjectId > 0)
            {
                string message = "Project has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Project> tmpList = null;
            Project.RetrieveProjectLookuplistChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ProjectList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    #endregion
    #region V2-1135
    public class Project_RetrieveWorkoderProjectLookupList_V2 : Project_TransactionBaseClass
    {

        public List<b_Project> ProjectList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_Project> tmpList = null;
            Project.RetrieveWorkoderProjectLookupList_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ProjectList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region 1087 Project Costing
    public class Project_RetrieveByPk_V2 : Project_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Project.ProjectId == 0)
            {
                string message = "Project has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Project.RetrieveByPK_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Project_RetrieveByProjectIdForPCDashboard_V2 : Project_TransactionBaseClass
    {

        public b_Project projectObj { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Project tmp = null;
            Project.RetrieveForProjectCostByProjectIdForDashboard_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmp);
            projectObj = new b_Project();
            projectObj = tmp;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    #region Search Project Costing
    public class Project_ProjectCostingRetrieveChunkSearch : Project_TransactionBaseClass
    {

        public List<b_Project> ProjectList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Project.ProjectId > 0)
            {
                string message = "Project has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Project> tmpList = null;
            Project.RetrieveProjectCostingChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ProjectList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Dashboard tab
    public class Project_RetrieveProjectCostingWorkOrderLookupListBySearchCriteria_V2 : Project_TransactionBaseClass
    {
        public List<b_Project> ProjectList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_Project> tmpList = null;
            Project.RetrieveProjectCostingWorkOrderTabLookupListBySearchCriteria(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ProjectList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Project_RetrieveProjectCostingPurchaseOrderSearchCriteria_V2 : Project_TransactionBaseClass
    {
        public List<b_Project> ProjectList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_Project> tmpList = null;
            Project.RetrieveProjectCostingPurchasingTabSearchCriteria(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ProjectList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
    #endregion

}
