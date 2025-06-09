using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class ProjectTask : DataContractBase
    {
        #region Property
        public string WorkOrderClientlookupId { get; set; }
        public string WorkOrderDescription { get; set; }
        public long SiteId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }

        #endregion

        #region Project Task Inner grid item
        public List<ProjectTask> RetrieveProjectTask_ByProjectId(DatabaseKey dbKey)
        {
            ProjectTask_RetrieveByProjectId trans = new ProjectTask_RetrieveByProjectId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ProjectTask = ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ProjectTask> ProjectTasklist = new List<ProjectTask>();

            foreach (b_ProjectTask Ptask in trans.ProjectTaskList)
            {
                ProjectTask tmpPTask = new ProjectTask();
                tmpPTask.UpdateFromDatabaseObjectForRetriveAllbyProjectId(Ptask);
                ProjectTasklist.Add(tmpPTask);
            }
            return ProjectTasklist;
        }
      
        public void UpdateFromDatabaseObjectForRetriveAllbyProjectId(b_ProjectTask dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.WorkOrderClientlookupId = dbObj.WorkOrderClientlookupId;
            this.WorkOrderDescription = dbObj.WorkOrderDescription;
        }

        #endregion

        #region Project Task for chunk search
        public List<ProjectTask> RetrieveProjectTask_ByProjectIdForChunkSearch(DatabaseKey dbKey)
        {
            ProjectTask_RetrieveByProjectIdForChunksearch trans = new ProjectTask_RetrieveByProjectIdForChunksearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ProjectTask = ToDatabaseObjectForRetrieveProjectTaskByProjectIdForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ProjectTask> ProjectTasklist = new List<ProjectTask>();

            foreach (b_ProjectTask Ptask in trans.ProjectTaskList)
            {
                ProjectTask tmpPTask = new ProjectTask();
                tmpPTask.UpdateFromDatabaseObjectForRetrieveProjectTaskByProjectIdForChunkSearch(Ptask);
                ProjectTasklist.Add(tmpPTask);
            }
            return ProjectTasklist;
        }

        public b_ProjectTask ToDatabaseObjectForRetrieveProjectTaskByProjectIdForChunkSearch()
        {
            b_ProjectTask dbObj = this.ToDatabaseObject();
            dbObj.SiteId = this.SiteId;
            dbObj.WorkOrderClientlookupId = this.WorkOrderClientlookupId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.WorkOrderDescription = this.WorkOrderDescription;
            dbObj.TotalCount = this.TotalCount;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetrieveProjectTaskByProjectIdForChunkSearch(b_ProjectTask dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.SiteId = dbObj.SiteId;
            this.WorkOrderClientlookupId = dbObj.WorkOrderClientlookupId;
            this.WorkOrderDescription = dbObj.WorkOrderDescription;
            this.OrderbyColumn = dbObj.OrderbyColumn;
            this.OrderBy = dbObj.OrderBy;
            this.OffSetVal = dbObj.OffSetVal;
            this.NextRow = dbObj.NextRow;
            this.TotalCount = dbObj.TotalCount;
        }

        #endregion

        #region Dashboard
        public List<KeyValuePair<string, long>> ProjectTaskDashboardStatuses(DatabaseKey dbKey)
        {
            ProjectTaskDashboardStatusesChart trans = new ProjectTaskDashboardStatusesChart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ProjectTask = ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return trans.Entries;
        }
        public List<KeyValuePair<string, long>> ProjectTaskDashboardScheduleComplianceStatuses(DatabaseKey dbKey)
        {
            ProjectTaskScheduleComplianceStatusesChart trans = new ProjectTaskScheduleComplianceStatusesChart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ProjectTask = ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return trans.Entries;
        }
        public b_ProjectTask ToDatabaseObjectExtended()
        {
            b_ProjectTask dbObj = ToDatabaseObject();
            dbObj.SiteId = this.SiteId;
            return dbObj;
        }
        #endregion
    }
}
