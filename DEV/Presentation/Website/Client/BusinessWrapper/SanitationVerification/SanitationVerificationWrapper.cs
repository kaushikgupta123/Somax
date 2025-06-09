using DataContracts;
using System;
using System.Collections.Generic;
using Client.Models.Sanitation;
using Common.Constants;
namespace Client.BusinessWrapper.SanitationVerification
{
    public class SanitationVerificationWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public SanitationVerificationWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<SanitationVerificationModel> RetrieveSanitationVerificationdata(int StatusTypeId, int CreatedDateId)
        {
            SanitationJob SV = new SanitationJob()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                ApproveStatusDrop = StatusTypeId.ToString(),
                VerificationCompleteDate = CreatedDateId.ToString()
            };

            SanitationVerificationModel SVModel;
            List<SanitationVerificationModel> SVModelList = new List<SanitationVerificationModel>();

            List<SanitationJob> SVList = SV.Retrieve_SanitationVerificationWorkBench_ByFilterCriteria(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var p in SVList)
            {
                SVModel = new SanitationVerificationModel();

                #region Model Bind
                SVModel.SanitationJobId = p.SanitationJobId;//Hidden Field To fetch Details
                SVModel.ClientLookupId = p.ClientLookupId;
                SVModel.Description = p.Description;
                SVModel.Status = p.Status;
                SVModel.ChargeTo_ClientLookupId = p.ChargeTo_ClientLookupId;
                SVModel.ChargeTo_Name = p.ChargeTo_Name;
                SVModel.CompleteDate = p.CompleteDate;
                SVModel.FailReason = p.FailReason;
                SVModel.FailComment = p.FailComment;
                SVModel.Assigned_PersonnelClientLookupId = p.Assigned_PersonnelClientLookupId;
                SVModel.AssignedTo_PersonnelId = p.AssignedTo_PersonnelId;
                SVModel.Shift = p.Shift;
                #endregion

                SVModelList.Add(SVModel);
            }

            return SVModelList;
        }

        public Dictionary<long, string> UpdatePassedSVListGrid(List<SVGridModel> list)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            foreach (var item in list)
            {
                SanitationJob objSJob = new SanitationJob()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId,
                    CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                    SanitationJobId = Convert.ToInt64(item.SanitationJobId)
                };
                objSJob.Retrieve(userData.DatabaseKey);

                objSJob.Status = SanitationJobConstant.Pass;
                objSJob.PassDate = DateTime.UtcNow;
                objSJob.PassBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                objSJob.ClientId = this.userData.DatabaseKey.Client.ClientId;


                objSJob.Update(this.userData.DatabaseKey);
                CreateEventLog(objSJob.SanitationJobId, SanitationEvents.Passed);


                Alerts alerts = new Alerts()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                    ObjectId = objSJob.SanitationJobId,
                    ObjectName = "SanitationJob",
                    IsCleared = true
                };

                alerts.AlertClear(this.userData.DatabaseKey);

            }
            return retValue;
        }
        public Dictionary<long, string> UpdateFailedSVListGrid(List<SVGridModel> list)
        {
            if (userData.Security.SanitationJob.CreateRequest)
            {
                Dictionary<long, string> retValue = new Dictionary<long, string>();
                foreach (var item in list)
                {
                    SanitationJob objSJob = new SanitationJob()
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        SiteId = userData.DatabaseKey.User.DefaultSiteId,
                        CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                        SanitationJobId = Convert.ToInt64(item.SanitationJobId)
                    };
                    objSJob.Retrieve(userData.DatabaseKey);

                    objSJob.Status = SanitationJobConstant.Fail;
                    objSJob.FailDate = DateTime.UtcNow;
                    objSJob.FailBy_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    if (item.FailReason != null)
                    {
                        objSJob.FailReason = item.FailReason;
                    }
                    else
                        objSJob.FailReason = "";

                    if (item.FailComment != null)
                    {
                        objSJob.FailComment = Convert.ToString(item.FailComment);
                    }
                    else
                        objSJob.FailComment = "";

                    objSJob.Update(this.userData.DatabaseKey);

                    CreateEventLog(objSJob.SanitationJobId, SanitationEvents.Fail);

                    Alerts alerts = new Alerts()
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                        ObjectId = objSJob.SanitationJobId,
                        ObjectName = "SanitationJob",
                        IsCleared = true
                    };

                    alerts.AlertClear(this.userData.DatabaseKey);

                    CreateSanitationRequest((item.SanitationJobId).ToString(), SanitationJobConstant.SourceType_FailedValidation);

                }
                return retValue;
            }
            else
            {
                return null;
            }
        }
        #region Create Event Log
        private void CreateEventLog(Int64 sanId, string Status)
        {
            SanitationEventLog log = new SanitationEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = sanId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion


        private void CreateSanitationRequest(string SanitJobId, string SourceType)
        {

            string errmsg = string.Empty;
            try
            {
                if (userData.Security.SanitationJob.CreateRequest)
                {
                    // SOM-1459 
                    // Need to retrieve the Failed Job and load some data from that job.
                    SanitationJob failedjob = new SanitationJob
                    {
                        ClientId = userData.DatabaseKey.Client.ClientId,
                        SanitationJobId = Convert.ToInt64(SanitJobId)
                    };
                    failedjob.Retrieve(userData.DatabaseKey);
                    string newClientlookupId = "";

                    SanitationRequest SanitationRequest = new SanitationRequest()
                    {
                        ClientId = this.userData.DatabaseKey.Client.ClientId
                    };

                    if (SanitationJobConstant.SanitaionJob_AutoGenerateEnabled)
                    {
                        newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.SANIT_ANNUAL, userData.DatabaseKey.User.DefaultSiteId, "");
                    }
                    SanitationRequest.ClientLookupId = newClientlookupId;
                    SanitationRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    SanitationRequest.ChargeToId = failedjob.ChargeToId;
                    SanitationRequest.ChargeTo_Name = failedjob.ChargeTo_Name;
                    SanitationRequest.ChargeType = failedjob.ChargeType;
                    SanitationRequest.Description = failedjob.Description;
                    SanitationRequest.SourceId = failedjob.SanitationJobId;
                    SanitationRequest.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
                    SanitationRequest.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    SanitationRequest.SourceType = SourceType;
                    SanitationRequest.Status = SanitationJobConstant.JobRequest;
                    SanitationRequest.FlagSourceType = 2;

                    SanitationRequest.Add_SanitationRequest(this.userData.DatabaseKey);

                    if (SanitationRequest.ErrorMessages.Count != 0)
                    {
                        //lblError.Visible = true;
                        //string txt = "";
                        //if (SanitationRequest.ErrorMessages != null)
                        //{
                        //    foreach (string s in SanitationRequest.ErrorMessages)
                        //    {

                        //        txt += s + "<br />";
                        //    }
                        //}

                        //lblError.Text = txt;
                        //lblError.ForeColor = Color.Red;
                        //return;
                    }

                    if (SanitationRequest.SanitationJobId > 0)
                    {
                        SanitationJobTask task = new SanitationJobTask()
                        {
                            SanitationJobId = Convert.ToInt64(SanitJobId),
                            ClientId = this.userData.DatabaseKey.Client.ClientId,
                            SiteId = userData.DatabaseKey.User.DefaultSiteId
                        };

                        List<SanitationJobTask> SaniatationTasks = task.SanitationJobTask_RetrieveBy_SanitationJobId(userData.DatabaseKey);

                        foreach (SanitationJobTask JobTask in SaniatationTasks)
                        {
                            SanitationJobTask Task = new SanitationJobTask()
                            {
                                SanitationJobTaskId = JobTask.SanitationJobTaskId,
                                ClientId = this.userData.DatabaseKey.Client.ClientId,
                                SiteId = userData.DatabaseKey.User.DefaultSiteId
                            };
                            Task.Retrieve(userData.DatabaseKey);

                            task.SanitationJobId = SanitationRequest.SanitationJobId;
                            task.ClientId = userData.DatabaseKey.Personnel.ClientId;
                            task.TaskId = Task.TaskId;
                            task.Status = SanitationJobConstant.Open;
                            task.Description = Task.Description;
                            task.SanOnDemandMasterTaskId = Task.SanOnDemandMasterTaskId;

                            task.CreateNew_SanitationJobTask(userData.DatabaseKey, userData.Site.TimeZone);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }
        }
    }
}