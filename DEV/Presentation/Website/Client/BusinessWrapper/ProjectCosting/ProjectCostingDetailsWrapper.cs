using Client.Models.ProjectCosting;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Client.Models;
using Common.Extensions;
using Client.Models.ProjectCosting.UIConfiguration;
using Common.Constants;
using System.Reflection;
using Client.Common;
using Client.Localization;

namespace Client.BusinessWrapper.ProjectsCosting
{
    public class ProjectCostingDetailsWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public ProjectCostingDetailsWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region ProjectCosting tab header
        public ProjectCostingDetailsHeaderModel GetProjectByProjectIdForProjectDetailsHeader(long projectId)
        {
            ProjectCostingDetailsHeaderModel projectDetailsHeaderModel = new ProjectCostingDetailsHeaderModel();
            Project project = new Project()
            {
                ClientId = _dbKey.Client.ClientId,
                ProjectId = projectId
            };
            var records = project.RetrieveProjectByProjectIdForHeader(this.userData.DatabaseKey);
            projectDetailsHeaderModel.ProjectId = records.ProjectId;
            projectDetailsHeaderModel.ClientlookupId = records.ClientLookupId;
            projectDetailsHeaderModel.Status = records.Status;
            projectDetailsHeaderModel.Description = records.Description;
            projectDetailsHeaderModel.Coordinator = records.Coordinator;
            projectDetailsHeaderModel.ScheduleStart = records.ScheduleStart != null && records.ScheduleStart != default(DateTime) ? Convert.ToDateTime(records.ScheduleStart).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : "";
            projectDetailsHeaderModel.ScheduleFinish = records.ScheduleFinish != null && records.ScheduleFinish != default(DateTime) ? Convert.ToDateTime(records.ScheduleFinish).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : "";
            projectDetailsHeaderModel.CompleteDate = records.CompleteDate != null && records.CompleteDate != default(DateTime) ? Convert.ToDateTime(records.CompleteDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : "";
            projectDetailsHeaderModel.Budget = records.Budget;
            return projectDetailsHeaderModel;
        }
        #endregion
        #region View Dynamic
        public Project RetrieveProjectByProjectId(long ProjectId)
        {
            Project objProject = new Project()
            {
                ClientId = _dbKey.Client.ClientId,
                ProjectId = ProjectId
            };
            objProject.RetrieveByPK_V2(this.userData.DatabaseKey);
            return objProject;
        }
        public ProjectUDF RetrieveProjectUDFByProjectId(long ProjectId)
        {
            ProjectUDF projectUDF = new ProjectUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ProjectId = ProjectId
            };

            projectUDF = projectUDF.RetrieveByProjectId(this.userData.DatabaseKey);
            return projectUDF;
        }

        public ViewProjectCostingModelDynamic MapProjectUDFDataForView(ViewProjectCostingModelDynamic viewProjectModelDynamic, ProjectUDF projectUDF)
        {
            if (projectUDF != null)
            {
                viewProjectModelDynamic.ProjectUDFId = projectUDF.ProjectUDFId;
                viewProjectModelDynamic.Text1 = projectUDF.Text1;
                viewProjectModelDynamic.Text2 = projectUDF.Text2;
                viewProjectModelDynamic.Text3 = projectUDF.Text3;
                viewProjectModelDynamic.Text4 = projectUDF.Text4;

                if (projectUDF.Date1 != null && projectUDF.Date1 == DateTime.MinValue)
                {
                    viewProjectModelDynamic.Date1 = null;
                }
                else
                {
                    viewProjectModelDynamic.Date1 = projectUDF.Date1;
                }
                if (projectUDF.Date2 != null && projectUDF.Date2 == DateTime.MinValue)
                {
                    viewProjectModelDynamic.Date2 = null;
                }
                else
                {
                    viewProjectModelDynamic.Date2 = projectUDF.Date2;
                }
                if (projectUDF.Date3 != null && projectUDF.Date3 == DateTime.MinValue)
                {
                    viewProjectModelDynamic.Date3 = null;
                }
                else
                {
                    viewProjectModelDynamic.Date3 = projectUDF.Date3;
                }
                if (projectUDF.Date4 != null && projectUDF.Date4 == DateTime.MinValue)
                {
                    viewProjectModelDynamic.Date4 = null;
                }
                else
                {
                    viewProjectModelDynamic.Date4 = projectUDF.Date4;
                }

                viewProjectModelDynamic.Bit1 = projectUDF.Bit1;
                viewProjectModelDynamic.Bit2 = projectUDF.Bit2;
                viewProjectModelDynamic.Bit3 = projectUDF.Bit3;
                viewProjectModelDynamic.Bit4 = projectUDF.Bit4;

                viewProjectModelDynamic.Numeric1 = projectUDF.Numeric1;
                viewProjectModelDynamic.Numeric2 = projectUDF.Numeric2;
                viewProjectModelDynamic.Numeric3 = projectUDF.Numeric3;
                viewProjectModelDynamic.Numeric4 = projectUDF.Numeric4;

                viewProjectModelDynamic.Select1 = projectUDF.Select1;
                viewProjectModelDynamic.Select2 = projectUDF.Select2;
                viewProjectModelDynamic.Select3 = projectUDF.Select3;
                viewProjectModelDynamic.Select4 = projectUDF.Select4;

                viewProjectModelDynamic.Select1Name = projectUDF.Select1Name;
                viewProjectModelDynamic.Select2Name = projectUDF.Select2Name;
                viewProjectModelDynamic.Select3Name = projectUDF.Select3Name;
                viewProjectModelDynamic.Select4Name = projectUDF.Select4Name;
            }
            return viewProjectModelDynamic;
        }
        public ViewProjectCostingModelDynamic MapProjectDataForView(ViewProjectCostingModelDynamic viewProjectModelDynamic, Project project)
        {
            viewProjectModelDynamic.ProjectId = project.ProjectId;
            viewProjectModelDynamic.ClientLookupId = project.ClientLookupId;
            viewProjectModelDynamic.ActualFinish = project.ActualFinish != null && project.ActualFinish != default(DateTime) ? project.ActualFinish : null;
            viewProjectModelDynamic.ActualStart = project.ActualStart != null && project.ActualStart != default(DateTime) ? project.ActualStart : null;
            viewProjectModelDynamic.Budget = project.Budget;
            viewProjectModelDynamic.CancelDate = project.CancelDate != null && project.CancelDate != default(DateTime) ? project.CancelDate : null;
            viewProjectModelDynamic.CancelBy_PersonnelId = project.CancelBy_PersonnelId;
            viewProjectModelDynamic.CancelReason = project.CancelReason;
            viewProjectModelDynamic.CloseDate = project.CloseDate != null && project.CloseDate != default(DateTime) ? project.CloseDate : null;
            viewProjectModelDynamic.CloseBy_PersonnelId = project.CloseBy_PersonnelId;
            viewProjectModelDynamic.CompleteDate = project.CompleteDate != null && project.CompleteDate != default(DateTime) ? project.CompleteDate : null;
            viewProjectModelDynamic.CompleteBy_PersonnelId = project.CompleteBy_PersonnelId;
            viewProjectModelDynamic.Coordinator_PersonnelId = project.Coordinator_PersonnelId;
            viewProjectModelDynamic.Description = project.Description;
            viewProjectModelDynamic.FiscalYear = project.FiscalYear;
            viewProjectModelDynamic.HoldDate = project.HoldDate != null && project.HoldDate != default(DateTime) ? project.HoldDate : null;
            viewProjectModelDynamic.HoldBy_PersonnelId = project.HoldBy_PersonnelId;
            viewProjectModelDynamic.Owner_PersonnelId = project.Owner_PersonnelId;
            viewProjectModelDynamic.ReturnFunds = project.ReturnFunds;
            viewProjectModelDynamic.ScheduleFinish = project.ScheduleFinish != null && project.ScheduleFinish != default(DateTime) ? project.ScheduleFinish : null;
            //viewProjectModelDynamic.ScheduleStart = project.ScheduleStart;
            viewProjectModelDynamic.ScheduleStart = project.ScheduleStart != null && project.ScheduleStart != default(DateTime) ? project.ScheduleStart : null;
            viewProjectModelDynamic.Status = project.Status;
            viewProjectModelDynamic.Type = project.Type;
            viewProjectModelDynamic.Category = project.Category;
            viewProjectModelDynamic.AssignedAssetGroup1 = project.AssignedAssetGroup1;
            viewProjectModelDynamic.AssignedAssetGroup2 = project.AssignedAssetGroup2;
            viewProjectModelDynamic.AssignedAssetGroup3 = project.AssignedAssetGroup3;
            viewProjectModelDynamic.CoordinatorFullName = project.CoordinatorFullName;
            viewProjectModelDynamic.OwnerFullName = project.OwnerFullName;
            viewProjectModelDynamic.AssignedGroup1_ClientLookupId = project.AG1ClientLookupId;
            viewProjectModelDynamic.AssignedGroup2_ClientLookupId = project.AG2ClientLookupId;
            viewProjectModelDynamic.AssignedGroup3_ClientLookupId = project.AG3ClientLookupId;

            return viewProjectModelDynamic;
        }
        #endregion

        #region Event Log
        public List<EventLogModel> PopulateEventLog(long projectId)
        {
            EventLogModel objEventLogModel;
            List<EventLogModel> EventLogModelList = new List<EventLogModel>();

            ProjectEventLog log = new ProjectEventLog();
            List<ProjectEventLog> data = new List<ProjectEventLog>();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ProjectId = projectId;
            data = log.RetriveByProjectId(userData.DatabaseKey);

            if (data != null)
            {
                foreach (var item in data)
                {
                    objEventLogModel = new EventLogModel();
                    objEventLogModel.ClientId = item.ClientId;
                    objEventLogModel.SiteId = item.SiteId;
                    objEventLogModel.EventLogId = item.ProjectEventLogId;
                    objEventLogModel.ObjectId = item.ProjectId;
                    if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                    {
                        objEventLogModel.TransactionDate = null;
                    }
                    else
                    {
                        objEventLogModel.TransactionDate = item.TransactionDate.ToUserTimeZone(userData.Site.TimeZone);
                    }
                    objEventLogModel.Event = item.Event;
                    objEventLogModel.Comments = item.Comments;
                    objEventLogModel.SourceId = item.SourceId;
                    objEventLogModel.Personnel = item.Personnel;
                    objEventLogModel.Events = item.Events;
                    objEventLogModel.PersonnelInitial = item.PersonnelInitial;
                    EventLogModelList.Add(objEventLogModel);
                }
            }
            return EventLogModelList;
        }

        #endregion

        #region  Update Project Status 
        public List<string> ProjectCostingStatusUpdating(long ProjectId, string Status)
        {
            string emptyValue = string.Empty;
            List<string> errList = new List<string>();
            Project objProject = new Project()
            {
                ClientId = _dbKey.Client.ClientId,
                ProjectId = ProjectId
            };
            objProject.Retrieve(this.userData.DatabaseKey);
            // ------Updating ------

            string Eventstatus = Status == "Open" ? "ReOpen" : Status == "Canceled" ? "Cancel" : Status == "Complete" ? "Complete" : "";
            string ProjStatus = Status == "Open" ? ProjectStatusConstants.Open : Status == "Canceled" ? ProjectStatusConstants.Canceled : Status == "Complete" ? ProjectStatusConstants.Complete : "";
            if (Status == "Complete")
            {
                objProject.CompleteDate = DateTime.UtcNow;
                objProject.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            }
            if (Status == "Canceled")
            {
                objProject.CancelDate = DateTime.UtcNow;
                objProject.CancelBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            }
            if (Status == "Open")
            {
                objProject.CancelDate = DateTime.UtcNow;
                objProject.CancelBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            }
            objProject.Status = ProjStatus;
            objProject.Update(this.userData.DatabaseKey);
            // ------creating new event logs ------
            if (Eventstatus != "")
            {
                CreatProjectEventLog(ProjectId, Eventstatus);
            }

            return errList;
        }

        private void CreatProjectEventLog(long ProjectId, string Status)
        {
            ProjectEventLog log = new ProjectEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ProjectId = ProjectId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = Status;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceTable = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }

        #endregion

        #region Edit ProjectCosting
        public EditProjectCostingModelDynamic RetrieveProjectCostingDetailsByProjectId(long ProjectId)
        {
            EditProjectCostingModelDynamic editModelDynamic = new EditProjectCostingModelDynamic();
            Project project = RetrieveProjectCostingByProjectId(ProjectId);
            ProjectUDF projectUDF = RetrieveProjectCostingUDFByProjectId(ProjectId);

            editModelDynamic = MapProjectUDFDataForEdit(editModelDynamic, projectUDF);
            editModelDynamic = MapProjectDataForEdit(editModelDynamic, project);
            return editModelDynamic;
        }
        public Project RetrieveProjectCostingByProjectId(long ProjectId)
        {
            Project project = new Project()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ProjectId = ProjectId
            };
            project.Retrieve(_dbKey);

            return project;
        }
        public ProjectUDF RetrieveProjectCostingUDFByProjectId(long ProjectId)
        {
            ProjectUDF projectUDF = new ProjectUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ProjectId = ProjectId
            };

            projectUDF = projectUDF.RetrieveByProjectId(this.userData.DatabaseKey);
            return projectUDF;
        }
        private EditProjectCostingModelDynamic MapProjectUDFDataForEdit(EditProjectCostingModelDynamic editModelDynamic, ProjectUDF projectUDF)
        {
            if (projectUDF != null)
            {
                editModelDynamic.ProjectUDFId = projectUDF.ProjectUDFId;

                editModelDynamic.Text1 = projectUDF.Text1;
                editModelDynamic.Text2 = projectUDF.Text2;
                editModelDynamic.Text3 = projectUDF.Text3;
                editModelDynamic.Text4 = projectUDF.Text4;

                if (projectUDF.Date1 != null && projectUDF.Date1 == DateTime.MinValue)
                {
                    editModelDynamic.Date1 = null;
                }
                else
                {
                    editModelDynamic.Date1 = projectUDF.Date1;
                }
                if (projectUDF.Date2 != null && projectUDF.Date2 == DateTime.MinValue)
                {
                    editModelDynamic.Date2 = null;
                }
                else
                {
                    editModelDynamic.Date2 = projectUDF.Date2;
                }
                if (projectUDF.Date3 != null && projectUDF.Date3 == DateTime.MinValue)
                {
                    editModelDynamic.Date3 = null;
                }
                else
                {
                    editModelDynamic.Date3 = projectUDF.Date3;
                }
                if (projectUDF.Date4 != null && projectUDF.Date4 == DateTime.MinValue)
                {
                    editModelDynamic.Date4 = null;
                }
                else
                {
                    editModelDynamic.Date4 = projectUDF.Date4;
                }

                editModelDynamic.Bit1 = projectUDF.Bit1;
                editModelDynamic.Bit2 = projectUDF.Bit2;
                editModelDynamic.Bit3 = projectUDF.Bit3;
                editModelDynamic.Bit4 = projectUDF.Bit4;

                editModelDynamic.Numeric1 = projectUDF.Numeric1;
                editModelDynamic.Numeric2 = projectUDF.Numeric2;
                editModelDynamic.Numeric3 = projectUDF.Numeric3;
                editModelDynamic.Numeric4 = projectUDF.Numeric4;

                editModelDynamic.Select1 = projectUDF.Select1;
                editModelDynamic.Select2 = projectUDF.Select2;
                editModelDynamic.Select3 = projectUDF.Select3;
                editModelDynamic.Select4 = projectUDF.Select4;
            }
            return editModelDynamic;
        }
        public EditProjectCostingModelDynamic MapProjectDataForEdit(EditProjectCostingModelDynamic editModelDynamic, Project project)
        {

            editModelDynamic.ProjectId = project.ProjectId;
            editModelDynamic.ClientLookupId = project.ClientLookupId;
            editModelDynamic.ActualFinish = project.ActualFinish != null && project.ActualFinish != default(DateTime) ? project.ActualFinish : null;
            editModelDynamic.ActualStart = project.ActualStart != null && project.ActualStart != default(DateTime) ? project.ActualStart : null;
            editModelDynamic.Budget = project.Budget;
            editModelDynamic.CancelDate = project.CancelDate != null && project.CancelDate != default(DateTime) ? project.CancelDate : null;
            editModelDynamic.CancelBy_PersonnelId = project.CancelBy_PersonnelId;
            editModelDynamic.CancelReason = project.CancelReason;
            editModelDynamic.CloseDate = project.CloseDate != null && project.CloseDate != default(DateTime) ? project.CloseDate : null;
            editModelDynamic.CloseBy_PersonnelId = project.CloseBy_PersonnelId;
            editModelDynamic.CompleteDate = project.CompleteDate != null && project.CompleteDate != default(DateTime) ? project.CompleteDate : null;
            editModelDynamic.CompleteBy_PersonnelId = project.CompleteBy_PersonnelId;
            editModelDynamic.Coordinator_PersonnelId = project.Coordinator_PersonnelId;
            editModelDynamic.Description = project.Description;
            editModelDynamic.FiscalYear = project.FiscalYear;
            editModelDynamic.HoldDate = project.HoldDate != null && project.HoldDate != default(DateTime) ? project.HoldDate : null;
            editModelDynamic.HoldBy_PersonnelId = project.HoldBy_PersonnelId;
            editModelDynamic.Owner_PersonnelId = project.Owner_PersonnelId;
            editModelDynamic.ReturnFunds = project.ReturnFunds;
            editModelDynamic.ScheduleFinish = project.ScheduleFinish != null && project.ScheduleFinish != default(DateTime) ? project.ScheduleFinish : null;
            editModelDynamic.ScheduleStart = project.ScheduleStart != null && project.ScheduleStart != default(DateTime) ? project.ScheduleStart : null;
            editModelDynamic.Status = project.Status;
            editModelDynamic.Type = project.Type;
            editModelDynamic.Category = project.Category;
            editModelDynamic.AssignedAssetGroup1 = project.AssignedAssetGroup1;
            editModelDynamic.AssignedAssetGroup2 = project.AssignedAssetGroup2;
            editModelDynamic.AssignedAssetGroup3 = project.AssignedAssetGroup3;
            return editModelDynamic;
        }

        public Project EditProjectCostingDynamic(ProjectCostingVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            Project project = new Project()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ProjectId = Convert.ToInt64(objVM.EditProject.ProjectId)
            };
            project.Retrieve(_dbKey);

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditProject, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditProject);
                getpropertyInfo = objVM.EditProject.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditProject);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = project.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(project, val);
            }
            project.Update(_dbKey);
            List<string> errors = new List<string>();
            if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                errors = EditProjectUDFDynamic(objVM.EditProject, configDetails);

            }
            if (errors != null && errors.Count() > 0)
            {
                project.ErrorMessages.AddRange(errors);
            }
            return project;
        }
        public List<string> EditProjectUDFDynamic(EditProjectCostingModelDynamic project, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            ProjectUDF projectUDF = new ProjectUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ProjectId = project.ProjectId
            };
            projectUDF = projectUDF.RetrieveByProjectId(_dbKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, project);
                getpropertyInfo = project.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(project);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = projectUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(projectUDF, val);
            }
            if (projectUDF.ProjectUDFId > 0)
            {
                projectUDF.Update(_dbKey);
            }
            else
            {
                projectUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                projectUDF.ProjectId = project.ProjectId;
                projectUDF.Create(_dbKey);
            }

            return projectUDF.ErrorMessages;
        }

        private void AssignDefaultOrNullValue(ref object val, Type t)
        {
            if (t.Equals(typeof(long?)))
            {
                val = val ?? 0;
            }
            else if (t.Equals(typeof(DateTime?)))
            {
                //val = val ?? null;
            }
            else if (t.Equals(typeof(decimal?)))
            {
                val = val ?? 0M;
            }
            else if (t.Name == "String")
            {
                val = val ?? string.Empty;
            }
        }
        #endregion

        #region Add ProjectCosting

        public Project AddProjectCostingDynamic(ProjectCostingVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            Project project = new Project();

            project.ClientId = userData.DatabaseKey.Client.ClientId;
            project.SiteId = userData.DatabaseKey.User.DefaultSiteId;

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddProject, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.AddProject);
                getpropertyInfo = objVM.AddProject.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.AddProject);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = project.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(project, val);
            }
            project.CheckDuplicateProject(this.userData.DatabaseKey);
            if (project.ErrorMessages == null || project.ErrorMessages.Count == 0)
            {
                project.Status = ProjectStatusConstants.Open;
                project.Create(this.userData.DatabaseKey);

                //Add in ProjectEventLog table
                CreateProjectEventLog(project.ProjectId, ProjectStatusConstants.Open);
            }
            if (project.ErrorMessages != null && project.ErrorMessages.Count == 0 && configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
            {
                IEnumerable<string> errors = AddProjectUDFDynamic(objVM.AddProject, project.ProjectId, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    project.ErrorMessages.AddRange(errors);
                }
            }

            return project;
        }

        private void CreateProjectEventLog(Int64 ProjectId, string Status)
        {
            ProjectEventLog log = new ProjectEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ProjectId = ProjectId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = Status;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceTable = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        public List<string> AddProjectUDFDynamic(AddProjectCostingModelDynamic project, long ProjectId, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            ProjectUDF projectUDF = new ProjectUDF();
            projectUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            projectUDF.ProjectId = ProjectId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, project);
                getpropertyInfo = project.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(project);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = projectUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(projectUDF, val);
            }

            projectUDF.Create(_dbKey);
            return projectUDF.ErrorMessages;
        }

        public List<AssetGroup1Model> GetAssetGroup1Dropdowndata()
        {
            AssetGroup1 assetGroup1 = new AssetGroup1()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup1.RetrieveAssetGroup1ByByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup1Model assetGroup1Model;
            List<AssetGroup1Model> AssetGroup1ModelList = new List<AssetGroup1Model>();
            foreach (var item in retData)
            {
                assetGroup1Model = new AssetGroup1Model();
                assetGroup1Model.AssetGroup1Id = item.AssetGroup1Id;
                assetGroup1Model.AssetGroup1DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup1ModelList.Add(assetGroup1Model);
            }
            return AssetGroup1ModelList;
        }
        public List<AssetGroup2Model> GetAssetGroup2Dropdowndata()
        {
            AssetGroup2 assetGroup2 = new AssetGroup2()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup2.RetrieveAssetGroup2ByByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup2Model assetGroup2Model;
            List<AssetGroup2Model> AssetGroup2ModelList = new List<AssetGroup2Model>();
            foreach (var item in retData)
            {
                assetGroup2Model = new AssetGroup2Model();
                assetGroup2Model.AssetGroup2Id = item.AssetGroup2Id;
                assetGroup2Model.AssetGroup2DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup2ModelList.Add(assetGroup2Model);
            }
            return AssetGroup2ModelList;
        }
        public List<AssetGroup3Model> GetAssetGroup3Dropdowndata()
        {
            AssetGroup3 assetGroup3 = new AssetGroup3()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup3.RetrieveAssetGroup3ByByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup3Model assetGroup3Model;
            List<AssetGroup3Model> AssetGroup3ModelList = new List<AssetGroup3Model>();
            foreach (var item in retData)
            {
                assetGroup3Model = new AssetGroup3Model();
                assetGroup3Model.AssetGroup3Id = item.AssetGroup3Id;
                assetGroup3Model.AssetGroup3DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup3ModelList.Add(assetGroup3Model);
            }
            return AssetGroup3ModelList;
        }

        #endregion

        #region Get WorkOrder ProjectCostingWOLookupList Grid Data
        public List<ProjetCostingWorkOrderSearchModel> GetProjectCostingWorkOrderSearchGridData(
            string orderbycol = "",
            string orderDir = "",
            int skip = 0,
            int length = 0,
            long ProjectId = 0,
            string clientLookupId = "",
            string Description = "",
            string Status = "",
            string Planner = "",
            string _CompleteDate = "",
            decimal MaterialCost = 0,
            decimal LaborCost = 0,
            decimal TotalCost = 0)
        {
            var projectList = new Project
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                OffSetVal = skip,
                NextRow = length,
                ProjectId = ProjectId,
                WorkOrderClientLookupId = clientLookupId,
                Description = Description,
                Status = Status,
                Planner = Planner,
                PC_WO_CompleteDate = _CompleteDate,
                MaterialCost = MaterialCost,
                LaborCost = LaborCost,
                TotalCost = TotalCost
            }.RetrieveProject_ProjectCostingWorkOrderSearchCriteria(userData.DatabaseKey);

            return projectList.Select(item => new ProjetCostingWorkOrderSearchModel
            {
                ProjectId = item.WorkOrderId,
                ClientLookupId = item.WorkOrderClientLookupId,
                Description = item.Description,
                Status = item.Status,
                Planner = item.Planner,
                CompleteDate = item.CompleteDate != null && item.CompleteDate != default(DateTime)
                    ? item.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                    : null,
                MaterialCost = item.MaterialCost,
                LaborCost = item.LaborCost,
                TotalCost = item.TotalCost,
                TotalCount = item.TotalCount
            }).ToList();
        }
        #endregion

        #region Get WorkOrder ProjectCostingWOLookupList Grid Data
        public List<ProjetCostingPurchasingSearchModel> GetProjectCostingPurchasingSearchGridData(
            string orderbycol = "",
            string orderDir = "",
            int skip = 0,
            int length = 0,
            long ProjectId = 0,
            string clientLookupId = "",
            int Line = 0,
            string PartID = "",
            string Description = "",
            decimal Quantity = 0,
            decimal UnitCost = 0,
            decimal TotalCost = 0,
            string Status = "",
            string Buyer = "",
            string _CompleteDate = "")
        {
            var projectList = new Project
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                OrderbyColumn = orderbycol,
                OrderBy = orderDir,
                OffSetVal = skip,
                NextRow = length,
                ProjectId = ProjectId,
                PurchasOrderClientLookupId = clientLookupId,
                Line = Line,
                PartID = PartID,
                Description = Description,
                Quantity = Quantity,
                UnitCost = UnitCost,
                TotalCost = TotalCost,
                Status = Status,
                Buyer = Buyer,
                PC_PO_CompleteDate = _CompleteDate
            }.RetrieveProject_ProjectCostingPurchaseOrderSearchCriteria(userData.DatabaseKey);

            return projectList.Select(item => new ProjetCostingPurchasingSearchModel
            {
                ProjectId = item.ProjectId,
                ClientLookupId = item.ClientLookupId,
                Line = item.Line,
                PartID = item.PartID,
                Description = item.Description,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                TotalCost = item.TotalCost,
                Status = item.Status,
                Buyer = item.Buyer,
                CompleteDate = item.CompleteDate != null && item.CompleteDate != default(DateTime)
                    ? item.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                    : null,
                TotalCount = item.TotalCount
            }).ToList();
        }
        #endregion

    }
}