using Database.Business;
using Database.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    #region  Project task Details
    public class ProjectTask_RetrieveByProjectId : ProjectTask_TransactionBaseClass
    {
        public List<b_ProjectTask> ProjectTaskList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ProjectTask.ProjectId == 0)
            {
                string message = "ProjectTask has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_ProjectTask> tmpList = null;
            ProjectTask.RetrieveProjectTaskRetrieveByProjectId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ProjectTaskList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region  Project task for chunk search
    public class ProjectTask_RetrieveByProjectIdForChunksearch : ProjectTask_TransactionBaseClass
    {
        public List<b_ProjectTask> ProjectTaskList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ProjectTask.ProjectId == 0)
            {
                string message = "ProjectTask has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_ProjectTask> tmpList = null;
            ProjectTask.RetrieveProjectTaskRetrieveByProjectIdForChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            ProjectTaskList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion


    #region Dashboard tab
    public class ProjectTaskDashboardStatusesChart : PieChartBaseTransaction
    {
        public b_ProjectTask ProjectTask { get; set; }

        public override void PerformWorkItem()
        {
            List<KeyValuePair<string, long>> tmpArray = null;

            ProjectTask.ProjectTaskDashboardStatusesChart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            Entries = new List<KeyValuePair<string, long>>();
            for (int i = 0; i < tmpArray.Count; i++)
            {
                Entries.Add(tmpArray[i]);
            }

        }
    }

    public class ProjectTaskScheduleComplianceStatusesChart : PieChartBaseTransaction
    {
        public b_ProjectTask ProjectTask{ get; set; }
        
        public override void PerformWorkItem()
        {
            List<KeyValuePair<string, long>> tmpArray = null;

            ProjectTask.ProjectTaskDashboardScheduleComplianceStatuses(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            Entries = new List<KeyValuePair<string, long>>();
            for (int i = 0; i < tmpArray.Count; i++)
            {
                Entries.Add(tmpArray[i]);
            }

        }
    }
    #endregion
}
