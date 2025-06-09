using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Sanitation;

using Common.Constants;
using Common.Enumerations;
using Common.Extensions;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Client.BusinessWrapper
{
    public class SanitationJobWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        string BodyHeader = string.Empty;
        string BodyContent = string.Empty;
        string FooterSignature = string.Empty;

        public SanitationJobWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Sanitation Approval WB
        public List<SanitationJobModel> GetWOApprovalWorkBenchDetails(string status, string createdates)
        {
            SanitationJob objSan = new SanitationJob();
            List<SanitationJob> sanList = new List<SanitationJob>();
            SanitationJobModel sanitationJobModel;
            List<SanitationJobModel> Joblist = new List<SanitationJobModel>();

            objSan.ClientId = this.userData.DatabaseKey.User.ClientId;
            objSan.SiteId = this.userData.DatabaseKey.Personnel.SiteId;
            objSan.ApproveStatusDrop = status;
            objSan.ApproveCreatedDate = createdates;
            List<SanitationJob> ApproveJobList = objSan.Retrieve_SanitationJobApproveWorkBench_ByFilterCriteria(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (ApproveJobList != null && ApproveJobList.Count > 0)
            {
                ApproveJobList.ForEach(x =>
                {
                    if (x.Description.Length > 100)
                    {
                        x.Description = x.Description.Substring(0, 99) + "..";
                    }
                });
            }

            if (ApproveJobList.Count == 0)
            {
                sanList = ApproveJobList;

            }
            else
            {
                sanList = ApproveJobList.Where(x => x.Status != SanitationJobConstant.Canceled && x.Status != SanitationJobConstant.Complete).ToList();

            }
            foreach (var sa in sanList)
            {
                sanitationJobModel = new SanitationJobModel();
                sanitationJobModel.SanitationJobId = sa.SanitationJobId;
                sanitationJobModel.ClientLookupId = sa.ClientLookupId;
                sanitationJobModel.Description = sa.Description;
                sanitationJobModel.ChargeTo_Name = sa.ChargeTo_Name;
                sanitationJobModel.ChargeTo_ClientLookupId = sa.ChargeTo_ClientLookupId;
                sanitationJobModel.AssignedTo_PersonnelId = sa.AssignedTo_PersonnelId;
                sanitationJobModel.Assigned_PersonnelClientLookupId = sa.Assigned_PersonnelClientLookupId;
                sanitationJobModel.Shift = sa.Shift;
                if (sa.CreateDate != null && sa.CreateDate == default(DateTime))
                {
                    sanitationJobModel.CreateDate = null;
                }
                else
                {
                    sanitationJobModel.CreateDate = sa.CreateDate;
                }
                if (sa.ScheduledDate != null && sa.ScheduledDate == default(DateTime))
                {
                    sanitationJobModel.ScheduledDate = null;
                }
                else
                {
                    sanitationJobModel.ScheduledDate = sa.ScheduledDate;
                }
                if (sa.CreateBy_PersonnelId != null && sa.CreateBy_PersonnelId != "")
                {
                    sanitationJobModel.CreateBy_PersonnelId = sa.CreateBy_PersonnelId;

                }
                else
                {
                    sanitationJobModel.CreateBy_PersonnelId = "";

                }
                sanitationJobModel.CreateBy = sa.CreateBy;
                sanitationJobModel.ScheduledDuration = sa.ScheduledDuration;
                Joblist.Add(sanitationJobModel);
            }
            return Joblist;
        }
        internal SanitationJob ApproveSanitationWBDetails(List<SanitationApproveWBModel> sanData)
        {
            var todaysDate = DateTime.UtcNow;
            SanitationJob sanJob = new SanitationJob();
            foreach (var sa in sanData)
            {

                sanJob.ClientId = this.userData.DatabaseKey.User.ClientId;
                sanJob.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                sanJob.SanitationJobId = Convert.ToInt64(sa.SanitationJobId);
                sanJob.Retrieve(userData.DatabaseKey);

                sanJob.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                sanJob.ApproveFlag = "Yes";
                sanJob.DeniedFlag = "No";
                sanJob.ScheduleFlag = "No";
                sanJob.Status = SanitationJobConstant.Approved;

                sanJob.ScheduledDuration = Convert.ToDecimal(sa.duration);
                sanJob.AssignedTo_PersonnelId = Convert.ToInt64(sa.workassignedval);
                sanJob.Shift = sa.shiftval == null ? "" : sa.shiftval;

                if (!string.IsNullOrEmpty(sa.scheduledate))
                {
                    sanJob.ScheduledDate = DateTime.ParseExact(sa.scheduledate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    if (sanJob.ScheduledDate < todaysDate)
                    {
                        sanJob.ScheduledDate = todaysDate;
                    }
                }

                if (sanJob.ScheduledDate != null && (sanJob.ScheduledDate.Value.Year > DateTime.Now.Year - 1))
                {
                    sanJob.ScheduleFlag = "Yes";
                    sanJob.Status = SanitationJobConstant.Scheduled;
                }
                sanJob.SanitationJob_UpdateFor_ApproveWorkBench(userData.DatabaseKey);

                if (sanJob.ScheduleFlag == "No")
                {
                    ApproveWODetails(sanJob.SanitationJobId);
                    CreateEventLog(sanJob.SanitationJobId, SanitationEvents.Approved);

                }
                if (sanJob.ScheduleFlag == "Yes")
                {
                    CreateEventLog(sanJob.SanitationJobId, SanitationEvents.Approved);
                    CreateEventLog(sanJob.SanitationJobId, SanitationEvents.Scheduled);
                }
            }
            return sanJob;

        }
        private void ApproveWODetails(long sanitationJobId)
        {
            SanitationJob SanitJobBench = new SanitationJob()
            {
                ClientId = this.userData.DatabaseKey.User.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                SanitationJobId = sanitationJobId
            };
            SanitJobBench.Retrieve(userData.DatabaseKey);

            SanitJobBench.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            SanitJobBench.ApproveFlag = "Yes";
            SanitJobBench.DeniedFlag = "No";
            SanitJobBench.ScheduleFlag = "No";
            SanitJobBench.Status = SanitationJobConstant.Approved;
            SanitJobBench.SanitationJob_UpdateFor_ApproveWorkBench(userData.DatabaseKey);
        }
        public bool DenySanitationWB(string[] wOIds, string DeniedReason, string DeniedComments)
        {
            try
            {
                foreach (var sanId in wOIds)
                {
                    SanitationJob SanitJobBench = new SanitationJob()
                    {
                        ClientId = this.userData.DatabaseKey.User.ClientId,
                        SiteId = userData.DatabaseKey.User.DefaultSiteId,
                        SanitationJobId = Convert.ToInt64(sanId)
                    };

                    SanitJobBench.Retrieve(userData.DatabaseKey);

                    SanitJobBench.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    SanitJobBench.DeniedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    SanitJobBench.DeniedDate = DateTime.UtcNow;
                    SanitJobBench.DeniedReason = DeniedReason;
                    SanitJobBench.DeniedComment = DeniedComments;
                    SanitJobBench.DeniedFlag = "Yes";
                    SanitJobBench.Status = SanitationJobConstant.Denied;
                    SanitJobBench.ScheduledDate = null;
                    if (!string.IsNullOrEmpty(DeniedReason))
                    {
                        SanitJobBench.SanitationJob_UpdateFor_ApproveWorkBench(userData.DatabaseKey);
                        CreateEventLog(SanitJobBench.SanitationJobId, SanitationEvents.Denied);
                    }

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        #endregion

        #region Photos
        //public void DeleteImage(long SanitationJobId, string TableName, bool Profile, bool Image, ref string rtrMsg)
        //{ // Check if there is a profile image attachment record for the object 
        //    Attachment attach = new Attachment()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        ObjectName = "Sanitation",
        //        ObjectId = SanitationJobId,
        //        Profile = Profile,
        //        Image = Image
        //    };
        //    attach.ClientId = userData.DatabaseKey.Client.ClientId;
        //    List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
        //    if (AList.Count > 0)
        //    {
        //        // Profile Image Attachment Record Exists
        //        string image_url = AList.First().AttachmentURL;
        //        bool external = AList.First().External;
        //        attach.AttachmentId = AList.First().AttachmentId;
        //        attach.Delete(userData.DatabaseKey);
        //        // If the image is NOT external then we delete the image 
        //        if (!external)
        //        {
        //            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
        //            aset.DeleteBlobByURL(image_url);
        //            rtrMsg = "Success";
        //        }
        //        else
        //        {
        //            rtrMsg = "External";
        //        }
        //    }
        //    else
        //    {
        //        // We still may have URL refrences in the Equipment. 
        //        // If so we need to delete the URL from the Equipment
        //        SanitationJob sanitationJob = new SanitationJob();
        //        sanitationJob.ClientId = userData.DatabaseKey.Client.ClientId;
        //        sanitationJob.SiteId = userData.DatabaseKey.User.DefaultSiteId;
        //        sanitationJob.SanitationJobId = SanitationJobId;
        //        sanitationJob.Retrieve(userData.DatabaseKey);
        //        if (sanitationJob.ImageURI != "")
        //        {
        //            sanitationJob.ImageURI = ""; //Garima
        //            sanitationJob.Update(userData.DatabaseKey);
        //            rtrMsg = "Success";
        //        }
        //    }

        //}
        #endregion

        #region Search
        internal List<SanitationJobSearchModel> SanitationSearch(int CustomQueryDisplayId)
        {
            SanitationJob sRequest = new SanitationJob();
            SanitationJobSearchModel pModel;
            List<SanitationJobSearchModel> SanitationJobSearchModelList = new List<SanitationJobSearchModel>();

            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            var AllLookUpList = GetAllLookUpList();
            if (AllLookUpList != null)
            {
                Shift = AllLookUpList.Where(x => x.ListName == LookupListConstants.Shift).ToList(); //GetShiftList();
            }

            sRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            sRequest.CustomQueryDisplayId = CustomQueryDisplayId;
            List<SanitationJob> sList = sRequest.Retrieve_SanitationJobSearch_ByFilterCriteria(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            //  List<SanitationJob> sList = sRequest.SanitationJob_WRDashboardRetrieveByFilter(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            if (sList != null)
            {
                foreach (var item in sList)
                {
                    pModel = new SanitationJobSearchModel();
                    pModel.SanitationJobId = item.SanitationJobId;
                    pModel.ClientId = item.ClientId;
                    pModel.ClientLookupId = item.ClientLookupId;
                    pModel.Description = item.Description;
                    pModel.ChargeTo_ClientLookupId = item.ChargeTo_ClientLookupId;
                    pModel.ChargeTo_Name = item.ChargeTo_Name;
                    pModel.Status = item.Status;
                    pModel.Shift = item.Shift;
                    if (Shift != null && Shift.Any(cus => cus.ListValue == item.Shift))
                    {
                        pModel.ShiftDescription = Shift.Where(x => x.ListValue == item.Shift).Select(x => x.Description).First();
                    }
                    if (item.CreateDate != null && item.CreateDate != default(DateTime))
                    {
                        pModel.CreateDate = item.CreateDate;
                    }
                    else
                    {
                        pModel.CreateDate = null;
                    }
                    pModel.CreateByName = item.CreateByName;
                    pModel.Assigned = item.Assigned;
                    if (item.CompleteDate != null && item.CompleteDate != default(DateTime))
                    {
                        pModel.CompleteDate = item.CompleteDate;
                    }
                    else
                    {
                        pModel.CompleteDate = null;
                    }
                    if (item.VerifiedDate != null && item.VerifiedDate != default(DateTime))
                    {
                        pModel.VerifiedDate = item.VerifiedDate;
                    }
                    else
                    {
                        pModel.VerifiedDate = null;
                    }
                    pModel.VerifiedBy = item.VerifiedBy;
                    pModel.Extracted = item.Extracted;
                    if (item.ScheduledDate != null && item.ScheduledDate != default(DateTime))
                    {
                        pModel.ScheduledDate = item.ScheduledDate;
                    }
                    else
                    {
                        pModel.ScheduledDate = null;
                    }
                    SanitationJobSearchModelList.Add(pModel);
                }
            }
            return SanitationJobSearchModelList;
        }


        #region ChunkSearch


        public List<SanitationJobSearchModel> GetSanitationChunkList(int CustomQueryDisplayId, int skip = 0, int length = 0, string orderbycol = "", string orderDir = "",
            string ClientLookupId = "", string Description = "", string ChargeTo_ClientLookupId = "", string ChargeTo_Name = "", string AssetLocation = "", string Status = "", string shift = "", string AssetGroup1_ClientLookUpId = "", string AssetGroup2_ClientLookUpId = "",
            string AssetGroup3_ClientLookUpId = "", string CreateDate = "", string CreateBy = "", string Assigned = "", string CompleteDate = "", string VerifiedBy = "", string VerifiedDate = "", bool Extracted = false, string ScheduledDate = "", string SearchText = "",
            DateTime? CreateStartDateVw = null, DateTime? CreateEndDateVw = null, DateTime? CompleteStartDateVw = null, DateTime? CompleteEndDateVw = null, DateTime? FailedStartDateVw = null, DateTime? FailedEndDateVw = null, DateTime? PassedStartDateVw = null, DateTime? PassedEndDateVw = null)
        {
            SanitationJob sanitationJob = new SanitationJob();
            List<SanitationJob> sanitationJobList = new List<SanitationJob>();
            SanitationJobSearchModel sanitationJobSearchModel;
            List<SanitationJobSearchModel> SanitationJobSearchModelList = new List<SanitationJobSearchModel>();

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            sanitationJob.ClientId = userData.DatabaseKey.Client.ClientId;
            sanitationJob.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            sanitationJob.CustomQueryDisplayId = CustomQueryDisplayId;
            sanitationJob.orderbyColumn = orderbycol;
            sanitationJob.orderBy = orderDir;
            sanitationJob.offset1 = skip;
            sanitationJob.nextrow = length;
            sanitationJob.ClientLookupId = ClientLookupId;
            sanitationJob.Description = Description;
            sanitationJob.ChargeTo_ClientLookupId = ChargeTo_ClientLookupId;
            sanitationJob.ChargeTo_Name = ChargeTo_Name;
            sanitationJob.AssetLocation = AssetLocation;
            sanitationJob.ChargeTo = ChargeTo_ClientLookupId;//added on 13/07/2020 for advance filter
            sanitationJob.ChargeToName = ChargeTo_Name;//added on 13/07/2020 for advance filter
            sanitationJob.Status = Status;
            sanitationJob.CreatedDate = CreateDate;
            sanitationJob.CreateBy = CreateBy;
            sanitationJob.Assigned = Assigned;
            sanitationJob.CompletedDate = CompleteDate;
            sanitationJob.Shift = shift;
            sanitationJob.VerifiedBy = VerifiedBy;
            sanitationJob.VerifyDate = VerifiedDate;
            sanitationJob.Extracted = Extracted;
            sanitationJob.ScheduleDate = ScheduledDate;
            sanitationJob.SearchText = SearchText;
            sanitationJob.AssetGroup1_ClientLookUpId = AssetGroup1_ClientLookUpId;
            sanitationJob.AssetGroup2_ClientLookUpId = AssetGroup2_ClientLookUpId;
            sanitationJob.AssetGroup3_ClientLookUpId = AssetGroup3_ClientLookUpId;
            sanitationJob.CreateStartDateVw = CreateStartDateVw.HasValue ? CreateStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            sanitationJob.CreateEndDateVw = CreateEndDateVw.HasValue ? CreateEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            sanitationJob.CompleteStartDateVw = CompleteStartDateVw.HasValue ? CompleteStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            sanitationJob.CompleteEndDateVw = CompleteEndDateVw.HasValue ? CompleteEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            sanitationJob.FailedStartDateVw = FailedStartDateVw.HasValue ? FailedStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            sanitationJob.FailedEndDateVw = FailedEndDateVw.HasValue ? FailedEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            sanitationJob.PassedStartDateVw = PassedStartDateVw.HasValue ? PassedStartDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            sanitationJob.PassedEndDateVw = PassedEndDateVw.HasValue ? PassedEndDateVw.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            sanitationJob.LoggedInUserPEID = userData.DatabaseKey.Personnel.PersonnelId;

            sanitationJobList = sanitationJob.Retrieve_ChunkSearch(this.userData.DatabaseKey, userData.Site.TimeZone);


            List<DataContracts.LookupList> ShiftDescription = new List<DataContracts.LookupList>();
            var AllLookUpList = GetAllLookUpList();
            if (AllLookUpList != null)
            {
                ShiftDescription = AllLookUpList.Where(x => x.ListName == LookupListConstants.Shift).ToList(); //GetShiftList();
            }


            foreach (var sanjob in sanitationJobList)
            {
                sanitationJobSearchModel = new SanitationJobSearchModel();
                sanitationJobSearchModel.SanitationJobId = sanjob.SanitationJobId;
                sanitationJobSearchModel.ClientId = sanjob.ClientId;
                sanitationJobSearchModel.ClientLookupId = sanjob.ClientLookupId;
                sanitationJobSearchModel.Description = sanjob.Description;
                sanitationJobSearchModel.ChargeTo_ClientLookupId = sanjob.ChargeTo_ClientLookupId;
                sanitationJobSearchModel.ChargeTo_Name = sanjob.ChargeTo_Name;
                sanitationJobSearchModel.AssetLocation = sanjob.AssetLocation;
                sanitationJobSearchModel.Status = sanjob.Status;
                sanitationJobSearchModel.Shift = sanjob.Shift;

                sanitationJobSearchModel.AssetGroup1_ClientLookUpId = sanjob.AssetGroup1_ClientLookUpId;
                sanitationJobSearchModel.AssetGroup2_ClientLookUpId = sanjob.AssetGroup2_ClientLookUpId;
                sanitationJobSearchModel.AssetGroup3_ClientLookUpId = sanjob.AssetGroup3_ClientLookUpId;

                if (ShiftDescription != null && ShiftDescription.Any(cus => cus.ListValue == sanjob.Shift))
                {
                    sanitationJobSearchModel.ShiftDescription = ShiftDescription.Where(x => x.ListValue == sanjob.Shift).Select(x => x.Description).First();
                }
                if (sanjob.CreateDate != null && sanjob.CreateDate != default(DateTime))
                {
                    sanitationJobSearchModel.CreateDate = sanjob.CreateDate;
                }
                else
                {
                    sanitationJobSearchModel.CreateDate = null;
                }
                sanitationJobSearchModel.CreateByName = sanjob.CreateByName;
                sanitationJobSearchModel.Assigned = sanjob.Assigned;
                if (sanjob.CompleteDate != null && sanjob.CompleteDate != default(DateTime))
                {
                    sanitationJobSearchModel.CompleteDate = sanjob.CompleteDate;
                }
                else
                {
                    sanitationJobSearchModel.CompleteDate = null;
                }
                if (sanjob.VerifiedDate != null && sanjob.VerifiedDate != default(DateTime))
                {
                    sanitationJobSearchModel.VerifiedDate = sanjob.VerifiedDate;
                }
                else
                {
                    sanitationJobSearchModel.VerifiedDate = null;
                }
                sanitationJobSearchModel.VerifiedBy = sanjob.VerifiedBy;
                sanitationJobSearchModel.Extracted = sanjob.Extracted;
                if (sanjob.ScheduledDate != null && sanjob.ScheduledDate != default(DateTime))
                {
                    sanitationJobSearchModel.ScheduledDate = sanjob.ScheduledDate;
                }
                else
                {
                    sanitationJobSearchModel.ScheduledDate = null;
                }

                sanitationJobSearchModel.TotalCount = sanjob.TotalCount;

                //V2-910
                if (sanjob.PassDate != null && sanjob.PassDate != default(DateTime))
                {
                    sanitationJobSearchModel.PassDate = sanjob.PassDate;
                }
                else
                {
                    sanitationJobSearchModel.PassDate = null;
                }
                if (sanjob.FailDate != null && sanjob.FailDate != default(DateTime))
                {
                    sanitationJobSearchModel.FailDate = sanjob.FailDate;
                }
                else
                {
                    sanitationJobSearchModel.FailDate = null;
                }

                SanitationJobSearchModelList.Add(sanitationJobSearchModel);
            }
            return SanitationJobSearchModelList;
        }


        #endregion

        internal List<SanitationJobSearchModel> SanitationWRSearch(int CustomQueryDisplayId)
        {
            SanitationJob sRequest = new SanitationJob();
            SanitationJobSearchModel pModel;
            List<SanitationJobSearchModel> SanitationJobSearchModelList = new List<SanitationJobSearchModel>();

            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            var AllLookUpList = GetAllLookUpList();
            if (AllLookUpList != null)
            {
                Shift = AllLookUpList.Where(x => x.ListName == LookupListConstants.Shift).ToList(); //GetShiftList();
            }

            sRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            sRequest.CustomQueryDisplayId = CustomQueryDisplayId;
            // List<SanitationJob> sList = sRequest.Retrieve_SanitationJobSearch_ByFilterCriteria(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            List<SanitationJob> sList = sRequest.SanitationJob_WRDashboardRetrieveBy_Filter_V2(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            if (sList != null)
            {
                foreach (var item in sList)
                {
                    pModel = new SanitationJobSearchModel();
                    pModel.SanitationJobId = item.SanitationJobId;
                    pModel.ClientId = item.ClientId;
                    pModel.ClientLookupId = item.ClientLookupId;
                    pModel.Description = item.Description;
                    pModel.ChargeTo_ClientLookupId = item.ChargeTo_ClientLookupId;
                    pModel.ChargeTo_Name = item.ChargeTo_Name;
                    pModel.Status = item.Status;
                    pModel.Shift = item.Shift;
                    if (Shift != null && Shift.Any(cus => cus.ListValue == item.Shift))
                    {
                        pModel.ShiftDescription = Shift.Where(x => x.ListValue == item.Shift).Select(x => x.Description).First();
                    }
                    if (item.CreateDate != null && item.CreateDate != default(DateTime))
                    {
                        pModel.CreateDate = item.CreateDate;
                    }
                    else
                    {
                        pModel.CreateDate = null;
                    }
                    pModel.CreateByName = item.CreateByName;
                    pModel.Assigned = item.Assigned;
                    if (item.CompleteDate != null && item.CompleteDate != default(DateTime))
                    {
                        pModel.CompleteDate = item.CompleteDate;
                    }
                    else
                    {
                        pModel.CompleteDate = null;
                    }
                    if (item.VerifiedDate != null && item.VerifiedDate != default(DateTime))
                    {
                        pModel.VerifiedDate = item.VerifiedDate;
                    }
                    else
                    {
                        pModel.VerifiedDate = null;
                    }
                    pModel.VerifiedBy = item.VerifiedBy;
                    pModel.Extracted = item.Extracted;
                    if (item.ScheduledDate != null && item.ScheduledDate != default(DateTime))
                    {
                        pModel.ScheduledDate = item.ScheduledDate;
                    }
                    else
                    {
                        pModel.ScheduledDate = null;
                    }
                    SanitationJobSearchModelList.Add(pModel);
                }
            }
            return SanitationJobSearchModelList;
        }
        internal List<KeyValuePair<string, string>> DisplayIdList()
        {

            List<KeyValuePair<string, string>> customFinalList = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> customList = CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, "SanitationJob", userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);

            if (customList.Count > 0)
            {
                customList.Insert(0, new KeyValuePair<string, string>("0", "--Select All--"));
            }
            return customList;
        }

        public SanitationJobDetailsModel RetrieveBy_SanitationJobId(long ObjectId)
        {
            SanitationJobDetailsModel JobDetails = new SanitationJobDetailsModel();
            SanitationJob SanDetail = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = ObjectId
            };
            SanDetail.RetrieveByV2(this.userData.DatabaseKey);
            JobDetails = initializeDetailsControls(SanDetail);

            return JobDetails;
        }

        //public List<DataContracts.LookupList> GetShiftList()
        //{
        //    List<DataContracts.LookupList> objshift = new Models.LookupList().RetrieveAll(userData.DatabaseKey).Where(x => x.ListName == LookupListConstants.Shift).ToList();
        //    return objshift;
        //}
        public SanitationJobDetailsModel initializeDetailsControls(SanitationJob obj)
        {
            SanitationJobDetailsModel objdetails = new SanitationJobDetailsModel();
            List<DataContracts.LookupList> Shift = new List<DataContracts.LookupList>();
            var AllLookUpList = GetAllLookUpList();
            if (AllLookUpList != null)
            {
                Shift = AllLookUpList.Where(x => x.ListName == LookupListConstants.Shift).ToList(); //GetShiftList();
            }
            //var Shift = GetShiftList();
            objdetails.SanitationJobId = obj.SanitationJobId;
            objdetails.ClientLookupId = obj.ClientLookupId;
            objdetails.Status = obj?.Status ?? string.Empty;
            if (Shift != null && Shift.Any(cus => cus.ListValue == obj.Shift))
            {
                objdetails.ShiftDesc = Shift.Where(x => x.ListValue == obj.Shift).Select(x => x.Description).First();
            }
            objdetails.Shift = obj.Shift;
            objdetails.DownRequired = obj.DownRequired;
            objdetails.Description = obj?.Description ?? string.Empty;
            objdetails.AssignedTo_PersonnelId = obj.AssignedTo_PersonnelId;

            objdetails.Assigned = (obj.AssignedTo_PersonnelId) > 0 ? obj.Assigned : "";
            objdetails.CreateBy_Name = obj?.CreateByName ?? string.Empty;
            objdetails.CreateBy = obj?.CreateBy ?? string.Empty;
            objdetails.ChargeToId_string = string.IsNullOrEmpty(obj.ChargeTo_ClientLookupId) ? "" : obj.ChargeTo_ClientLookupId;
            objdetails.ChargeTo_Name = obj?.ChargeTo_Name ?? string.Empty;
            objdetails.AssetLocation = obj?.AssetLocation ?? string.Empty;
            objdetails.ChargeType = obj?.ChargeType ?? string.Empty;//hidden

            objdetails.ChargeTo_ClientLookupId = string.IsNullOrEmpty(obj.ChargeTo_ClientLookupId) ? "" : obj.ChargeTo_ClientLookupId;

            objdetails.ChargeToId = obj.ChargeToId;

            objdetails.ScheduledDuration = obj.ScheduledDuration;
            if (obj.ScheduledDate != null && obj.ScheduledDate == default(DateTime))
            {
                objdetails.ScheduledDate = null;
            }
            else
            {
                objdetails.ScheduledDate = obj.ScheduledDate;
            }
            if (obj.CompleteDate != null && obj.CompleteDate == default(DateTime))
            {
                objdetails.CompleteDate = null;
            }
            else
            {
                objdetails.CompleteDate = obj.CompleteDate;
            }
            objdetails.CompleteBy = obj?.CompleteBy ?? string.Empty;
            objdetails.ActualDuration = obj.ActualDuration;
            objdetails.CompleteComments = obj?.CompleteComments ?? string.Empty;
            objdetails.PlantLocationId = obj.PlantLocationId;
            objdetails.CreateDate = obj?.CreateDate ?? DateTime.MinValue;
            objdetails.Status_Display = obj.Status_Display;


            //if (obj.PassDate != null && DateTime.Parse(obj.PassDate.ToString()) != null && DateTime.Parse(obj.PassDate.ToString()) != DateTime.MinValue)
            if (obj.PassDate != null && obj.PassDate != default(DateTime))
            {
                objdetails.VerificationStatus = "Pass";
                objdetails.VerificationDate = obj.PassDate;
                objdetails.VerificationBy = obj.PassBy;
                objdetails.VerificationCommentsVisible = false;
                objdetails.VerificationReasonVisible = false;
                objdetails.VerificationReason = "";
                objdetails.VerificationComments = "";
            }
            //else if (obj.FailDate != null && DateTime.Parse(obj.FailDate.ToString()) != null && DateTime.Parse(obj.FailDate.ToString()) != DateTime.MinValue)
            else if (obj.FailDate != null && obj.FailDate != default(DateTime))
            {
                objdetails.VerificationStatus = "Fail";
                objdetails.VerificationDate = obj.FailDate;
                objdetails.VerificationBy = obj.FailBy;
                objdetails.VerificationCommentsVisible = true;
                objdetails.VerificationReasonVisible = true;
                objdetails.VerificationReason = obj.FailReason;
                objdetails.VerificationComments = obj.FailComment;
            }


            objdetails.SourceType = string.IsNullOrEmpty(obj.SourceType) ? "" : obj.SourceType;
            objdetails.SourceIDClientLookUpId = string.IsNullOrEmpty(obj.SourceIDClientLookUpId) ? "" : obj.SourceIDClientLookUpId;
            objdetails.SanitationMasterId = obj.SanitationMasterId;
            objdetails.PassBy = string.IsNullOrEmpty(obj.PassBy) ? "" : obj.PassBy;
            if (obj.PassDate != null && obj.PassDate == default(DateTime))
            {
                objdetails.PassDate = null;
            }
            else
            {
                objdetails.PassDate = obj.PassDate;
            }
            objdetails.FailBy = string.IsNullOrEmpty(obj.FailBy) ? "" : obj.FailBy;
            if (obj.FailDate != null && obj.FailDate == default(DateTime))
            {
                objdetails.FailDate = null;
            }
            else
            {
                objdetails.FailDate = obj.FailDate;
            }

            objdetails.FailReason = string.IsNullOrEmpty(obj.FailReason) ? "" : obj.FailReason;
            objdetails.FailComment = string.IsNullOrEmpty(obj.FailComment) ? "" : obj.FailComment; //V2-827
            objdetails.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            return objdetails;
        }

        public SanitationJob SaveEditSanitationJob(SanitationJobDetailsModel jobObj)
        {
            SanitationJob sanitationjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = jobObj.SanitationJobId.Value
            };
            sanitationjob.RetrieveByPKForeignKeys(this.userData.DatabaseKey);


            sanitationjob.ClientLookupId = jobObj.ClientLookupId;
            sanitationjob.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            sanitationjob.Status = jobObj.Status;
            sanitationjob.Shift = (jobObj.Shift != null) ? jobObj.Shift : "";
            sanitationjob.DownRequired = jobObj.DownRequired;
            sanitationjob.Description = jobObj.Description;
            sanitationjob.ActualDuration = jobObj.ActualDuration ?? 0;
            sanitationjob.ScheduledDuration = jobObj.ScheduledDuration ?? 0;
            sanitationjob.ChargeType = jobObj.ChargeType;
            sanitationjob.PlantLocationId = jobObj.PlantLocationId ?? 0;
            sanitationjob.CompleteComments = jobObj.CompleteComments ?? "";


            sanitationjob.Creator_PersonnelClientLookupId = this.userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            sanitationjob.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            sanitationjob.ChargeTo_ClientLookupId = jobObj.ChargeTo_ClientLookupId;
            sanitationjob.CheckStatus = "Update";

            sanitationjob.UpdateByPK_ForeignKeys(this.userData.DatabaseKey);


            return sanitationjob;
        }
        public SanitationJob SaveEditSanitationJobMobile(SanitationJobDetailsModel jobObj)
        {
            SanitationJob sanitationjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = jobObj.SanitationJobId.Value
            };
            sanitationjob.RetrieveByPKForeignKeys(this.userData.DatabaseKey);
            sanitationjob.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            sanitationjob.Shift = (jobObj.Shift != null) ? jobObj.Shift : "";
            sanitationjob.DownRequired = jobObj.DownRequired;
            sanitationjob.Description = jobObj.Description;
            sanitationjob.ActualDuration = jobObj.ActualDuration ?? 0;
            sanitationjob.ScheduledDuration = jobObj.ScheduledDuration ?? 0;
            sanitationjob.CompleteComments = jobObj.CompleteComments ?? "";
            sanitationjob.Creator_PersonnelClientLookupId = this.userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            sanitationjob.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            sanitationjob.ChargeTo_ClientLookupId = jobObj.ChargeTo_ClientLookupId;
            sanitationjob.CheckStatus = "Update";
            sanitationjob.UpdateByPK_ForeignKeys(this.userData.DatabaseKey);
            return sanitationjob;
        }
        public SanitationJob CompleteSanitationJob(SanitationJobDetailsModel jobObj)
        {
            SanitationJob sanitationjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = jobObj.SanitationJobId.Value

            };
            sanitationjob.RetrieveBy_SanitationJobId(this.userData.DatabaseKey);

            if (sanitationjob.Status_Display != SanitationJobConstant.Complete && sanitationjob.Status_Display != SanitationJobConstant.Canceled)
            {
                sanitationjob.Status = SanitationJobConstant.Complete;
                sanitationjob.CompleteDate = DateTime.UtcNow;
                sanitationjob.CheckStatus = SanitationJobConstant.Complete;
                sanitationjob.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                sanitationjob.Creator_PersonnelClientLookupId = this.userData.DatabaseKey.Personnel.ClientLookupId.ToString();
                sanitationjob.Shift = (jobObj.Shift != null) ? jobObj.Shift : "";
                sanitationjob.DownRequired = jobObj.DownRequired;
                sanitationjob.Description = jobObj.Description ?? "";
                sanitationjob.ActualDuration = jobObj.ActualDuration ?? 0;
                sanitationjob.ScheduledDuration = jobObj.ScheduledDuration ?? 0;
                sanitationjob.ChargeType = jobObj.ChargeType;
                sanitationjob.PlantLocationId = jobObj.PlantLocationId ?? 0;
                sanitationjob.ChargeTo_ClientLookupId = jobObj.ChargeTo_ClientLookupId;
                sanitationjob.CompleteComments = jobObj.CompleteComments ?? "";
                var hdnUpdateIndex = Convert.ToString(sanitationjob.UpdateIndex);

                sanitationjob.UpdateByPK_ForeignKeys(this.userData.DatabaseKey);

                if (sanitationjob.ErrorMessages.Count != 0)
                {

                    string txt = "";
                    if (sanitationjob.ErrorMessages != null)
                    {
                        foreach (string s in sanitationjob.ErrorMessages)
                        {
                            txt += s + "<br />";
                        }
                    }
                }
                else
                {
                    CreateEventLog(sanitationjob.SanitationJobId, SanitationEvents.Complete);
                    DataContracts.SanitationJobTask task = new DataContracts.SanitationJobTask()
                    {
                        SanitationJobId = jobObj.SanitationJobId.Value,
                        SiteId = userData.DatabaseKey.User.DefaultSiteId
                    };

                    List<SanitationJobTask> TaskList = task.SanitationJobTask_RetrieveBy_SanitationJobId(this.userData.DatabaseKey);
                    if (TaskList != null && TaskList.Count > 0)
                    {
                        foreach (SanitationJobTask SanitationTask in TaskList)
                        {
                            if (SanitationTask.Status != SanitationJobConstant.TaskComplete &&
                                SanitationTask.Status != SanitationJobConstant.TaskCancel)
                            {

                                SanitationTask.Retrieve(userData.DatabaseKey);

                                SanitationTask.ClientId = userData.DatabaseKey.Personnel.ClientId;
                                SanitationTask.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                                SanitationTask.Status = SanitationJobConstant.Complete;
                                SanitationTask.CompleteDate = System.DateTime.UtcNow;
                                SanitationTask.Update_SanitationJobTask(userData.DatabaseKey);
                            }
                        }
                    }
                }
            }
            return sanitationjob;
        }

        public SanitationJob CompleteSanitationJob(long SanitationJobId, string CompleteComments)
        {
            SanitationJob sanitationjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = SanitationJobId

            };
            sanitationjob.RetrieveBy_SanitationJobId(this.userData.DatabaseKey);
            if (sanitationjob.Status_Display != SanitationJobConstant.Complete && sanitationjob.Status_Display != SanitationJobConstant.Canceled)
            {
                sanitationjob.Status = SanitationJobConstant.Complete;
                sanitationjob.CompleteDate = DateTime.UtcNow;
                sanitationjob.CompleteComments = CompleteComments;
                sanitationjob.CheckStatus = SanitationJobConstant.Complete;
                sanitationjob.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                sanitationjob.Creator_PersonnelClientLookupId = this.userData.DatabaseKey.Personnel.ClientLookupId.ToString();

                sanitationjob.UpdateByPK_ForeignKeys(this.userData.DatabaseKey);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count != 0)
                {
                    return sanitationjob;
                }
                else
                {
                    CreateEventLog(sanitationjob.SanitationJobId, SanitationEvents.Complete);
                    DataContracts.SanitationJobTask task = new DataContracts.SanitationJobTask()
                    {
                        SanitationJobId = SanitationJobId,
                        SiteId = userData.DatabaseKey.User.DefaultSiteId
                    };

                    List<SanitationJobTask> TaskList = task.SanitationJobTask_RetrieveBy_SanitationJobId(this.userData.DatabaseKey);
                    if (TaskList != null && TaskList.Count > 0)
                    {
                        foreach (SanitationJobTask SanitationTask in TaskList)
                        {
                            if (SanitationTask.Status != SanitationJobConstant.TaskComplete &&
                                SanitationTask.Status != SanitationJobConstant.TaskCancel)
                            {

                                SanitationTask.Retrieve(userData.DatabaseKey);

                                SanitationTask.ClientId = userData.DatabaseKey.Personnel.ClientId;
                                SanitationTask.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                                SanitationTask.Status = SanitationJobConstant.Complete;
                                SanitationTask.CompleteDate = DateTime.UtcNow;
                                SanitationTask.Update_SanitationJobTask(userData.DatabaseKey);
                            }
                        }
                    }
                }
            }
            return sanitationjob;
        }
        public List<SanOnDemandMaster> SanOnDemandMaster()
        {
            SanOnDemandMaster SanOnDemandMaster = new SanOnDemandMaster();
            SanOnDemandMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            SanOnDemandMaster.InactiveFlag = false;
            List<SanOnDemandMaster> obj_Lookuplist = SanOnDemandMaster.Retrieve_SanOnDemandMaster_ByInActiveFlag(this.userData.DatabaseKey, this.userData.Site.TimeZone);
            return obj_Lookuplist;
        }
        public SanitationRequest AddSanitationJobRequestandDemand(AddODemandModel obj, string SaveType, bool IsSavedFromDashboard)
        {
            SanitationRequest SanitationRequest = new SanitationRequest
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            string newClientlookupId = "";

            if (obj.ClientLookupId == null && SanitationJobConstant.SanitaionJob_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.SANIT_ANNUAL, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            SanitationRequest.SiteId = userData.DatabaseKey.Personnel.SiteId;
            SanitationRequest.ChargeType = obj?.ChargeType ?? string.Empty;
            SanitationRequest.ChargeToClientLookupId = obj?.ChargeToClientLookupId ?? string.Empty;
            SanitationRequest.PlantLocationId = obj?.PlantLocationId ?? 0;
            SanitationRequest.SanOnDemandMasterId = obj.OnDemandId ?? 0;
            SanitationRequest.Description = obj?.Description ?? string.Empty;



            SanitationRequest.ClientLookupId = newClientlookupId;
            SanitationRequest.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            SanitationRequest.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            SanitationRequest.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;

            #region Logic For Status type Save
            var Type = string.Concat(SaveType.TakeWhile((c) => c != '_'));
            var Operation = SaveType.Substring(SaveType.LastIndexOf('_') + 1);
            if (Type == "Request")
            {
                if (Operation == "Demand")
                {
                    SanitationRequest.Status = SanitationJobConstant.JobRequest;
                    if (!IsSavedFromDashboard)
                    {
                        SanitationRequest.SourceType = SanitationJobConstant.SourceType_NewJob;
                        SanitationRequest.FlagSourceType = 0;
                    }
                    else
                    {
                        SanitationRequest.RequiredDate = obj.RequiredDate;
                        SanitationRequest.ChargeToClientLookupId = obj.ChargeType == "PlantLocation" ? "" : obj.ChargeToClientLookupId;
                        SanitationRequest.FlagSourceType = obj.ChargeType == "PlantLocation" ? 0 : 2;

                        SanitationRequest.AddEXSanitationRequest(this.userData.DatabaseKey);
                        CreateEventLog(SanitationRequest.SanitationJobId, SanitationEvents.Create);//-----------SOM-1635-------------//
                        CreateEventLog(SanitationRequest.SanitationJobId, SanitationEvents.JobRequest);//-----------SOM-1635-------------//
                        return SanitationRequest;
                    }
                }
                else if (Operation == "Describe")
                {
                    SanitationRequest.Status = SanitationJobConstant.JobRequest;
                    SanitationRequest.FlagSourceType = 0;

                }
            }
            else if (Type == "Job")
            {
                if (Operation == "Demand")
                {
                    SanitationRequest.Status = SanitationJobConstant.Approved;
                    SanitationRequest.SourceType = SanitationJobConstant.SourceType_Request;
                    SanitationRequest.FlagSourceType = 1;
                }
                else if (Operation == "Describe")
                {
                    SanitationRequest.Status = SanitationJobConstant.Approved;
                    SanitationRequest.FlagSourceType = 1;
                }

            }
            #endregion


            if (Operation == "Demand")
            {
                SanitationRequest.Add_SanitationJobOnDemandjobsAndRequests(this.userData.DatabaseKey);

            }
            else if (Operation == "Describe")
            {
                SanitationRequest.Add_SanitationRequest(this.userData.DatabaseKey);
            }

            if (Type == "Request")
            {
                CreateEventLog(SanitationRequest.SanitationJobId, SanitationEvents.Create);
                CreateEventLog(SanitationRequest.SanitationJobId, SanitationEvents.JobRequest);

            }
            else if (Type == "Job")
            {
                CreateEventLog(SanitationRequest.SanitationJobId, SanitationEvents.Create);
                CreateEventLog(SanitationRequest.SanitationJobId, SanitationEvents.Approved);
            }


            return SanitationRequest;
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

        #region Create Work Order Event Log
        private void CreateWorkOrderEventLog(Int64 WOId, string Status)
        {
            WorkOrderEventLog log = new WorkOrderEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.WorkOrderId = WOId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion
        public SanitationJob CancelJob(long SanitationJobId, string CancelReason, string Comments)
        {
            SanitationJob sanitationjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = SanitationJobId
            };

            sanitationjob.RetrieveByPKForeignKeys(userData.DatabaseKey);


            if (sanitationjob.Status_Display != SanitationJobConstant.Complete && sanitationjob.Status_Display != SanitationJobConstant.Canceled)
            {
                {
                    sanitationjob.Status = SanitationJobConstant.Canceled;
                    sanitationjob.CompleteDate = DateTime.UtcNow;
                    sanitationjob.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    sanitationjob.CompleteComments = (Comments != null) ? Comments.Trim() : string.Empty;
                    sanitationjob.CancelReason = CancelReason;
                    sanitationjob.CheckStatus = SanitationJobConstant.Complete;
                    sanitationjob.Creator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId;
                    sanitationjob.Creator_PersonnelClientLookupId = this.userData.DatabaseKey.Personnel.ClientLookupId.ToString();
                    sanitationjob.UpdateByPK_ForeignKeys(this.userData.DatabaseKey);
                    if (sanitationjob.ErrorMessages.Count != 0)
                    {
                        return sanitationjob;
                    }
                    else
                    {
                        CreateEventLog(sanitationjob.SanitationJobId, SanitationEvents.Canceled);

                        //Cancel All Tasks
                        SanitationJobTask task = new SanitationJobTask()
                        {
                            SanitationJobId = SanitationJobId,
                            SiteId = userData.DatabaseKey.User.DefaultSiteId
                        };
                        List<SanitationJobTask> TaskList = task.SanitationJobTask_RetrieveBy_SanitationJobId(userData.DatabaseKey);
                        if (TaskList != null && TaskList.Count > 0)
                        {
                            foreach (SanitationJobTask SanitationTask in TaskList)
                            {
                                if (SanitationTask.Status != SanitationJobConstant.TaskComplete &&
                                    SanitationTask.Status != SanitationJobConstant.TaskCancel)
                                {

                                    SanitationTask.Retrieve(userData.DatabaseKey);

                                    SanitationTask.ClientId = userData.DatabaseKey.Personnel.ClientId;
                                    SanitationTask.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                                    SanitationTask.Status = SanitationJobConstant.TaskCancel;
                                    SanitationTask.CompleteDate = System.DateTime.UtcNow;
                                    SanitationTask.Update_SanitationJobTask(userData.DatabaseKey);

                                }
                            }
                        }
                    }
                }
            }
            return sanitationjob;
        }


        #endregion

        #region Task
        internal List<SanitationJobTaskModel> SanitationJobTaskPopulate(long sanitationJobId, string ChargeToClientLookupId = null)
        {
            string TaskCompleteDate = string.Empty;
            SanitationJobTask sjTask = new SanitationJobTask()
            {
                SanitationJobId = sanitationJobId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };
            List<SanitationJobTask> sjTaskList = sjTask.SanitationJobTask_RetrieveBy_SanitationJobId(this.userData.DatabaseKey);
            List<SanitationJobTaskModel> sjTaskList2 = new List<SanitationJobTaskModel>();
            SanitationJobTaskModel objSanitationJobTaskModel;
            foreach (var item in sjTaskList)
            {
                objSanitationJobTaskModel = new SanitationJobTaskModel();
                objSanitationJobTaskModel.ClientId = item.ClientId;
                objSanitationJobTaskModel.SanitationJobId = item.SanitationJobId;
                objSanitationJobTaskModel.SanitationJobTaskId = item.SanitationJobTaskId;
                objSanitationJobTaskModel.SanitationMasterTaskId = item.SanitationMasterTaskId;
                objSanitationJobTaskModel.CancelReason = item.CancelReason;
                objSanitationJobTaskModel.CompleteBy_PersonnelId = item.CompleteBy_PersonnelId;
                objSanitationJobTaskModel.CompleteBy = item.CompleteBy;
                objSanitationJobTaskModel.CompleteComments = item.CompleteComments;

                if (item.CompleteDate != null && item.CompleteDate == default(DateTime))
                {
                    objSanitationJobTaskModel.CompleteDate = null;
                    TaskCompleteDate = "";
                }
                else
                {
                    objSanitationJobTaskModel.CompleteDate = item.CompleteDate;
                    TaskCompleteDate = item.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                objSanitationJobTaskModel.Description = item.Description;
                objSanitationJobTaskModel.Status = item.Status;
                objSanitationJobTaskModel.TaskId = item.TaskId;
                objSanitationJobTaskModel.RecordedValue = item.RecordedValue;
                objSanitationJobTaskModel.SanOnDemandMasterTaskId = item.SanOnDemandMasterTaskId;
                objSanitationJobTaskModel.UpdateIndex = item.UpdateIndex;
                objSanitationJobTaskModel.ChargeToClientLookupId = ChargeToClientLookupId;
                //objSanitationJobTaskModel.ChargeType = item.ChargeType;
                if (TaskCompleteDate != "" && TaskCompleteDate != null)
                {
                    if (item.CompleteDate.HasValue && item.CompleteDate.Value != DateTime.MinValue)  //SOM-1416
                    {
                        objSanitationJobTaskModel.CompleteStatus = 1;
                    }
                    else
                    {
                        objSanitationJobTaskModel.CompleteStatus = 0;
                    }
                }
                sjTaskList2.Add(objSanitationJobTaskModel);
            }
            return sjTaskList2;
        }
        internal List<DataContracts.LookupList> GetSanitTaskCancelReason()
        {
            List<DataContracts.LookupList> objCancel = new Models.LookupList().RetrieveAll(userData.DatabaseKey).Where(x => x.ListName == LookupListConstants.SANIT_CAN_REASN).ToList();
            return objCancel;
        }
        internal SanitationJobTaskModel SanitationJobTaskRetrieveSingleBySanitationJobId(long sanitationJobId, long taskId, string ClientLookupId)
        {
            SanitationJobTask sanitTask = new SanitationJobTask() { SanitationJobTaskId = taskId };
            sanitTask.SanitationJobId = sanitationJobId;
            sanitTask.SiteId = userData.DatabaseKey.Personnel.SiteId;

            var sjDetails = RetrieveBy_SanitationJobId(sanitationJobId);
            sanitTask.SanitationJobTask_RetrieveSingleBy_SanitationJobId(userData.DatabaseKey);

            SanitationJobTaskModel objSanitationJobTaskModel = new SanitationJobTaskModel();

            objSanitationJobTaskModel.SanitationJobId = sanitationJobId;
            objSanitationJobTaskModel.SanitationJobTaskId = taskId;
            objSanitationJobTaskModel.TaskId = sanitTask.TaskId;
            objSanitationJobTaskModel.Description = sanitTask.Description;
            objSanitationJobTaskModel.ChargeType = sjDetails.ChargeType;
            objSanitationJobTaskModel.ChargeToClientLookupId = sjDetails.ChargeTo_ClientLookupId;
            objSanitationJobTaskModel.Status = sanitTask.Status;
            objSanitationJobTaskModel.CompleteBy_PersonnelId = sanitTask.CompleteBy_PersonnelId;
            objSanitationJobTaskModel.CompleteBy = sanitTask.CompleteBy;
            objSanitationJobTaskModel.CompleteDate = sanitTask.CompleteDate;
            objSanitationJobTaskModel.CancelReason = sanitTask.CancelReason;
            objSanitationJobTaskModel.UpdateIndex = sanitTask.UpdateIndex;
            sanitTask.ClientId = userData.DatabaseKey.Personnel.ClientId;
            objSanitationJobTaskModel.ClientLookupId = ClientLookupId;
            return objSanitationJobTaskModel;
        }
        internal SanitationJobTask SanitationJobTaskRetrieve(long sanitationJobId, long taskId)
        {
            SanitationJobTask sjTask = new SanitationJobTask()
            { SanitationJobTaskId = taskId };
            sjTask.SanitationJobId = sanitationJobId;
            sjTask.Retrieve(userData.DatabaseKey);
            return sjTask;
        }
        internal void UpdateSanitJobTask(SanitationJobTask sjTask)
        {
            sjTask.Update_SanitationJobTask(userData.DatabaseKey);
        }




        internal List<string> UpdateSanitJobTask(SanitationVM sjVM)
        {
            SanitationJobTask sjTask = new SanitationJobTask()
            {
                SanitationJobTaskId = sjVM.sanitationJobTaskModel.SanitationJobTaskId
            };
            sjTask = SanitationJobTaskRetrieve(sjVM.sanitationJobTaskModel.SanitationJobId, sjVM.sanitationJobTaskModel.SanitationJobTaskId);
            sjTask.ClientId = userData.DatabaseKey.Personnel.ClientId;
            sjTask.TaskId = sjVM.sanitationJobTaskModel.TaskId;
            sjTask.Description = sjVM.sanitationJobTaskModel.Description;
            sjTask.Update_SanitationJobTask(userData.DatabaseKey);
            return sjTask.ErrorMessages;
        }
        internal List<string> CreateSanitJobTask(SanitationVM sjVM)
        {
            SanitationJobTask sjTask = new SanitationJobTask();

            sjTask.SanitationJobId = sjVM.sanitationJobTaskModel.SanitationJobId;
            sjTask.ClientId = userData.DatabaseKey.Personnel.ClientId;
            sjTask.TaskId = sjVM.sanitationJobTaskModel.TaskId;
            sjTask.Status = SanitationJobConstant.Open;
            sjTask.CancelReason = string.Empty;
            sjTask.CompleteBy_PersonnelId = 0;
            sjTask.Description = sjVM.sanitationJobTaskModel.Description;
            sjTask.CreateNew_SanitationJobTask(userData.DatabaseKey, userData.Site.TimeZone);
            return sjTask.ErrorMessages;
        }
        internal bool DeleteSanitJobTask(long taskNumber)
        {
            try
            {
                SanitationJobTask tsk = new SanitationJobTask()
                {
                    SanitationJobTaskId = taskNumber
                };
                tsk.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal int CancelSanitJobTask(long sanitationJobId, long sanitationJobTaskId, string cancelReason, string taskStatus)
        {
            int retVal = 0;
            SanitationJobTask task = new SanitationJobTask()
            {
                SanitationJobTaskId = sanitationJobTaskId
            };
            task.SanitationJobId = sanitationJobId;

            if (taskStatus != SanitationJobConstant.TaskCancel)
            {
                task = SanitationJobTaskRetrieve(sanitationJobId, sanitationJobTaskId);

                task.ClientId = userData.DatabaseKey.Personnel.ClientId;
                task.CompleteBy_PersonnelId = 0;
                task.CancelReason = cancelReason;
                task.Status = SanitationJobConstant.TaskCancel;
                UpdateSanitJobTask(task);
                retVal = 1;
            }
            return retVal;
        }
        internal int ReOpenSanitJobTask(long sanitationJobId, long sanitationJobTaskId, string taskStatus)
        {
            int retVal = 0;
            SanitationJobTask task = new SanitationJobTask()
            {
                SanitationJobTaskId = sanitationJobTaskId
            };
            task.SanitationJobId = sanitationJobId;

            if (taskStatus == SanitationJobConstant.TaskComplete || taskStatus == SanitationJobConstant.TaskCancel)
            {
                task = SanitationJobTaskRetrieve(sanitationJobId, sanitationJobTaskId);

                task.ClientId = userData.DatabaseKey.Personnel.ClientId;
                task.CompleteBy_PersonnelId = 0;
                task.CompleteDate = null;
                task.CancelReason = string.Empty;
                task.Status = SanitationJobConstant.Open;
                UpdateSanitJobTask(task);
                retVal = 1;
            }
            return retVal;
        }
        internal int CompleteSanitJobTask(long sanitationJobId, long sanitationJobTaskId)
        {
            int retVal = 0;
            SanitationJobTask task = new SanitationJobTask()
            {
                SanitationJobTaskId = sanitationJobTaskId
            };

            task.SanitationJobId = sanitationJobId;

            task = SanitationJobTaskRetrieve(sanitationJobId, sanitationJobTaskId);

            task.ClientId = userData.DatabaseKey.Personnel.ClientId;
            task.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            task.Status = SanitationJobConstant.TaskComplete;
            task.CompleteBy = userData.DatabaseKey.Personnel.NameFull;
            task.CompleteDate = System.DateTime.UtcNow;
            UpdateSanitJobTask(task);
            retVal = 1;

            return retVal;
        }
        #endregion

        #region Notes
        public List<Client.Models.Sanitation.NotesModel> PopulateNotes(long SanitationJobId)
        {
            Client.Models.Sanitation.NotesModel objNotesModel;
            List<Client.Models.Sanitation.NotesModel> NotesModelList = new List<Client.Models.Sanitation.NotesModel>();
            Notes note = new Notes()
            {
                ObjectId = SanitationJobId,
                TableName = "SanitationJob"
            };
            List<Notes> NotesList = note.RetrieveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
            if (NotesList != null)
            {
                foreach (var item in NotesList)
                {
                    objNotesModel = new Client.Models.Sanitation.NotesModel();
                    objNotesModel.NotesId = item.NotesId;
                    objNotesModel.Subject = item.Subject;
                    objNotesModel.Content = item.Content;
                    objNotesModel.updatedindex = item.UpdateIndex;
                    objNotesModel.OwnerName = item.OwnerName;
                    if (item.ModifiedDate != null && item.ModifiedDate == default(DateTime))
                    {
                        objNotesModel.ModifiedDate = null;
                    }
                    else
                    {
                        objNotesModel.ModifiedDate = item.ModifiedDate;
                    }
                    objNotesModel.ModifiedDate = item.ModifiedDate;
                    NotesModelList.Add(objNotesModel);
                }
            }
            return NotesModelList;
        }
        public List<String> AddOrUpdateNote(SanitationVM sanVM, ref string Mode)
        {
            Notes notes = new Notes()
            {
                OwnerId = userData.DatabaseKey.User.UserInfoId,
                OwnerName = userData.DatabaseKey.User.FullName,
                Subject = sanVM.notesModel.Subject,
                Content = sanVM.notesModel.Content,
                Type = sanVM.notesModel.Type,
                TableName = "SanitationJob",
                ObjectId = sanVM.notesModel.SanitationJobId,
                UpdateIndex = sanVM.notesModel.updatedindex,
                NotesId = sanVM.notesModel.NotesId,
            };
            if (sanVM.notesModel.NotesId == 0)
            {
                Mode = "add";
                notes.Create(this.userData.DatabaseKey);
            }
            else
            {
                Mode = "update";
                notes.Update(this.userData.DatabaseKey);
            }
            return notes.ErrorMessages;

            #region Extra Code

            //Notes notes = new Notes();
            //notes.OwnerId = userData.DatabaseKey.User.UserInfoId;
            //notes.OwnerName = userData.DatabaseKey.User.FullName;
            //notes.Subject = objNote.Subject;
            //notes.Content = objNote.Content;
            //notes.Type = objNote.Type;
            //notes.TableName = "SanitationJob";
            //notes.ObjectId = objNote.ObjectId;
            //notes.UpdateIndex = objNote.updatedindex;
            //notes.NotesId = objNote.NotesId;
            //notes.Create(this.userData.DatabaseKey);



            //notes.OwnerId = userData.DatabaseKey.User.UserInfoId;
            //notes.OwnerName = userData.DatabaseKey.User.FullName;
            //notes.Subject = objNote.Subject;
            //notes.Content = objNote.Content;
            //notes.Type = objNote.Type;
            //notes.TableName = "SanitationJob";
            //notes.ObjectId = objNote.ObjectId;
            //notes.UpdateIndex = objNote.updatedindex;
            //notes.NotesId = objNote.NotesId;
            //notes.Update(this.userData.DatabaseKey);
            //return notes;
            #endregion
        }
        public Notes UpdateNote(Client.Models.Sanitation.NotesModel objNote)
        {
            Notes notes = new Notes();


            return notes;
        }

        public Client.Models.Sanitation.NotesModel getNoteById(long NotesId)
        {
            Client.Models.Sanitation.NotesModel objNotesModel = new Client.Models.Sanitation.NotesModel();
            Notes note = new Notes()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                NotesId = NotesId,
            };
            note.Retrieve(userData.DatabaseKey);
            objNotesModel.NotesId = note.NotesId;
            objNotesModel.updatedindex = note.UpdateIndex;
            objNotesModel.Subject = note.Subject;
            objNotesModel.Content = note.Content;

            return objNotesModel;


        }
        public bool DeleteNote(long _notesId)
        {
            try
            {
                Notes notes = new Notes()
                {
                    NotesId = Convert.ToInt64(_notesId)
                };
                notes.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Event Log
        public List<EventLogModel> PopulateEventLog(long SanitationJobId)
        {
            EventLogModel objEventLogModel;
            List<EventLogModel> EventLogModelList = new List<EventLogModel>();

            SanitationEventLog log = new SanitationEventLog();
            List<SanitationEventLog> data = new List<SanitationEventLog>();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = SanitationJobId;
            data = log.RetriveBySanitationJobId(userData.DatabaseKey);

            if (data != null)
            {
                foreach (var item in data)
                {
                    objEventLogModel = new EventLogModel();
                    objEventLogModel.ClientId = item.ClientId;
                    objEventLogModel.SiteId = item.SiteId;
                    objEventLogModel.EventLogId = item.EventLogId;
                    objEventLogModel.ObjectId = item.ObjectId;
                    if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                    {
                        objEventLogModel.TransactionDate = null;
                    }
                    else
                    {
                        objEventLogModel.TransactionDate = item.TransactionDate;
                    }
                    objEventLogModel.Event = item.Event;
                    objEventLogModel.Comments = item.Comments;
                    objEventLogModel.SourceId = item.SourceId;
                    objEventLogModel.Personnel = item.Personnel;
                    objEventLogModel.Events = item.Events;
                    EventLogModelList.Add(objEventLogModel);
                }
            }
            return EventLogModelList;
        }
        #endregion

        #region Labors   

        public List<Client.Models.Sanitation.LaborModel> PopulateLabors(long SanitationJobId)
        {
            Client.Models.Sanitation.LaborModel objLaborModel;
            List<Client.Models.Sanitation.LaborModel> LaborModelList = new List<Client.Models.Sanitation.LaborModel>();
            Timecard timecard = new Timecard();

            timecard.ChargeToId_Primary = SanitationJobId;
            timecard.ChargeType_Primary = SanitationJobConstant.SanitationLabor_ChargeType_Primary;
            timecard.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            timecard.ClientId = userData.DatabaseKey.User.ClientId;

            List<Timecard> LaborList = timecard.RetriveBy_SanitationJobId(userData.DatabaseKey);
            if (LaborList != null)
            {
                foreach (var item in LaborList)
                {
                    objLaborModel = new Client.Models.Sanitation.LaborModel();
                    objLaborModel.PersonnelId = item.PersonnelId;
                    objLaborModel.Value = item.Value;
                    objLaborModel.NameFull = item.FullName;
                    objLaborModel.PersonnelClientLookupId = item.PersonnelClientLookupId;

                    objLaborModel.ChargeToId_Primary = item.ChargeToId_Primary;
                    objLaborModel.Hours = item.Hours;
                    if (item.StartDate != null && item.StartDate == default(DateTime))
                    {
                        objLaborModel.StartDate = null;
                    }
                    else
                    {
                        objLaborModel.StartDate = item.StartDate;
                    }
                    objLaborModel.PersonnelId = item.PersonnelId;
                    objLaborModel.UpdateIndex = item.UpdateIndex;
                    objLaborModel.TimecardId = item.TimecardId;

                    LaborModelList.Add(objLaborModel);
                }
            }
            return LaborModelList;
        }

        public List<string> AddOrUpdateLabor(SanitationVM sanVM, ref string Mode)
        {
            Timecard TC = new Timecard()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };

            if (sanVM.laborModel.TimecardId == 0)
            {
                Mode = "add";
                TC.ChargeToId_Primary = sanVM.laborModel.ChargeToId_Primary;
                TC.ChargeType_Primary = SanitationJobConstant.SanitationLabor_ChargeType_Primary;
                TC.PersonnelId = sanVM.laborModel.PersonnelId;
                TC.Hours = sanVM.laborModel.Hours ?? 0;
                TC.StartDate = sanVM.laborModel.StartDate ?? default(DateTime);
                TC.CreateByPKForeignKeys(userData.DatabaseKey);

            }
            else
            {
                Mode = "update";
                TC.TimecardId = sanVM.laborModel.TimecardId;
                TC.Retrieve(userData.DatabaseKey);//*
                TC.ChargeToId_Primary = sanVM.laborModel.ChargeToId_Primary;
                TC.ChargeType_Primary = SanitationJobConstant.SanitationLabor_ChargeType_Primary;
                TC.PersonnelId = sanVM.laborModel.PersonnelId;
                TC.Hours = sanVM.laborModel.Hours ?? 0;
                TC.StartDate = sanVM.laborModel.StartDate ?? default(DateTime);
                TC.UpdateIndex = sanVM.laborModel.UpdateIndex;
                TC.UpdateByPKForeignKeys(userData.DatabaseKey);
            }
            return TC.ErrorMessages;

            #region Extra Code
            //Timecard TC = new Timecard();

            //if (objLabor.TimecardId == 0)
            //{
            //    TC.ClientId = userData.DatabaseKey.Client.ClientId;
            //    TC.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            //    TC.ChargeToId_Primary = objLabor.ChargeToId_Primary;
            //    TC.ChargeType_Primary = SanitationJobConstant.SanitationLabor_ChargeType_Primary;
            //    TC.PersonnelId = objLabor.PersonnelId;
            //    TC.Hours = objLabor.Hours ?? 0;               
            //    TC.StartDate = objLabor.StartDate ?? default(DateTime);
            //    TC.CreateByPKForeignKeys(userData.DatabaseKey);
            //}
            //else
            //{

            //    TC.ClientId = userData.DatabaseKey.Client.ClientId;
            //    TC.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            //    TC.TimecardId = objLabor.TimecardId;
            //    TC.Retrieve(userData.DatabaseKey);
            //    TC.ChargeToId_Primary = objLabor.ChargeToId_Primary;
            //    TC.ChargeType_Primary = SanitationJobConstant.SanitationLabor_ChargeType_Primary;
            //    TC.PersonnelId = objLabor.PersonnelId;
            //    TC.Hours = objLabor.Hours ?? 0;
            //    TC.StartDate = objLabor.StartDate ?? default(DateTime);               
            //    TC.UpdateIndex = objLabor.UpdateIndex;         
            //    TC.UpdateByPKForeignKeys(userData.DatabaseKey);
            //}
            //return TC;
            #endregion
        }
        public bool DeleteLabor(long timecardId)
        {
            try
            {
                Timecard Timecards = new Timecard()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    TimecardId = Convert.ToInt64(timecardId)
                };
                Timecards.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public LaborModel getLaborById(long _TimecardId)
        {
            LaborModel objLaborModel = new LaborModel();
            Timecard Timecards = new Timecard()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                TimecardId = _TimecardId
            };
            Timecards.Retrieve(userData.DatabaseKey);
            objLaborModel.ChargeToId_Primary = Timecards.ChargeToId_Primary;
            objLaborModel.Hours = Timecards.Hours;
            objLaborModel.StartDate = Timecards.StartDate;
            objLaborModel.PersonnelId = Timecards.PersonnelId;
            objLaborModel.UpdateIndex = Timecards.UpdateIndex;
            objLaborModel.TimecardId = _TimecardId;
            return objLaborModel;
        }
        #endregion

        #region Assignments
        public List<Client.Models.Sanitation.AssignmentModel> PopulateAssignments(long SanitationJobId)
        {
            Client.Models.Sanitation.AssignmentModel objAssignmentModel;
            List<Client.Models.Sanitation.AssignmentModel> AssignmentModelList = new List<Client.Models.Sanitation.AssignmentModel>();
            SanitationJobSchedule sanitationJobSchedule = new SanitationJobSchedule();

            sanitationJobSchedule.ClientId = userData.DatabaseKey.Client.ClientId;
            sanitationJobSchedule.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            sanitationJobSchedule.SanitationJobId = SanitationJobId;
            List<SanitationJobSchedule> AssignmentList = sanitationJobSchedule.RetrieveAllBy_SanitationJobId(userData.DatabaseKey);
            if (AssignmentList != null)
            {
                foreach (var item in AssignmentList)
                {
                    objAssignmentModel = new Client.Models.Sanitation.AssignmentModel();
                    objAssignmentModel.SanitationJobScheduleId = item.SanitationJobScheduleId;
                    objAssignmentModel.PersonnelId = item.PersonnelId;
                    if (item.ScheduledStartDate != null && item.ScheduledStartDate == default(DateTime))
                    {
                        objAssignmentModel.ScheduledStartDate = null;
                    }
                    else
                    {
                        objAssignmentModel.ScheduledStartDate = item.ScheduledStartDate;
                    }
                    objAssignmentModel.ScheduledHours = item.ScheduledHours;
                    objAssignmentModel.Name = item.Name;
                    objAssignmentModel.ClientLookupId = item.ClientLookupId;
                    objAssignmentModel.UpdateIndex = item.UpdateIndex;
                    objAssignmentModel.SanitationJobScheduleId = item.SanitationJobScheduleId;
                    AssignmentModelList.Add(objAssignmentModel);
                }
            }
            return AssignmentModelList;
        }
        public SanitationVM EditAssignment(long SanitationJobId, long _SanitationJobScheduledId, string ClientLookupId)
        {
            SanitationVM objSanitationJobVM = new SanitationVM();
            AssignmentModel objAssignmentModel = new AssignmentModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            SanitationJobSchedule SJC = new SanitationJobSchedule()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SanitationJobScheduleId = _SanitationJobScheduledId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                SanitationJobId = SanitationJobId
            };
            SJC.RetrieveSingleBy_SanitationJobId(userData.DatabaseKey);
            objAssignmentModel.SanitationJobId = SJC.SanitationJobId;
            objAssignmentModel.ScheduledHours = SJC.ScheduledHours;
            objAssignmentModel.ScheduledStartDate = SJC.ScheduledStartDate;
            objAssignmentModel.PersonnelId = SJC.PersonnelId;
            objAssignmentModel.UpdateIndex = SJC.UpdateIndex;
            objAssignmentModel.SanitationJobScheduleId = _SanitationJobScheduledId;
            objAssignmentModel.ClientLookupId = ClientLookupId;
            objSanitationJobVM.assignmentModel = objAssignmentModel;
            SanitationJobModel SJM = new SanitationJobModel();
            //SJM.ImageURI = GetSanitationJobImageUrl(SanitationJobId);
            SJM.ImageURI = comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation);
            objSanitationJobVM.sanitationJobModel = SJM;
            return objSanitationJobVM;

        }

        public List<string> AddOrUpdateAssignment(SanitationVM sanVM, ref string Mode)
        {
            SanitationJobSchedule SJob = new SanitationJobSchedule()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                SanitationJobId = sanVM.assignmentModel.SanitationJobId
            };

            if (sanVM.assignmentModel.SanitationJobScheduleId == 0)
            {
                Mode = "add";
                SJob.PersonnelId = sanVM.assignmentModel.PersonnelId;
                SJob.ScheduledHours = sanVM.assignmentModel.ScheduledHours ?? 0;
                SJob.ScheduledStartDate = sanVM.assignmentModel.ScheduledStartDate ?? default(DateTime);
                SJob.CreateForSanitationJob(userData.DatabaseKey);
                CreateEventLog(SJob.SanitationJobId, SanitationEvents.Scheduled);

            }
            else
            {
                Mode = "update";
                SJob.SanitationJobScheduleId = sanVM.assignmentModel.SanitationJobScheduleId;
                SJob.RetrieveSingleBy_SanitationJobId(userData.DatabaseKey);
                SJob.PersonnelId = sanVM.assignmentModel.PersonnelId;
                SJob.ScheduledHours = sanVM.assignmentModel.ScheduledHours ?? 0;
                SJob.ScheduledStartDate = sanVM.assignmentModel.ScheduledStartDate ?? default(DateTime);
                SJob.UpdateIndex = sanVM.assignmentModel.UpdateIndex;
                SJob.UpdateForSanitationJob(userData.DatabaseKey);
            }
            return SJob.ErrorMessages;

            #region Extra Code

            //SanitationJobSchedule SJC = new SanitationJobSchedule();

            //if (_Sanitation.assignmentModel.SanitationJobScheduleId == 0)
            //{
            //    SJC.ClientId = userData.DatabaseKey.Client.ClientId;
            //    SJC.SanitationJobId = _Sanitation.assignmentModel.SanitationJobId;
            //    SJC.PersonnelId = _Sanitation.assignmentModel.PersonnelId;
            //    SJC.ScheduledHours = _Sanitation.assignmentModel.ScheduledHours ?? 0;
            //    SJC.ScheduledStartDate = _Sanitation.assignmentModel.ScheduledStartDate ?? default(DateTime);
            //    SJC.CreateForSanitationJob(userData.DatabaseKey);
            //    CreateEventLog(SJC.SanitationJobId, SanitationEvents.Scheduled);
            //}
            //else
            //{
            //    SJC.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            //    SJC.ClientId = userData.DatabaseKey.Client.ClientId;
            //    SJC.SanitationJobId = _Sanitation.assignmentModel.SanitationJobId;
            //    SJC.SanitationJobScheduleId = _Sanitation.assignmentModel.SanitationJobScheduleId;
            //    SJC.RetrieveSingleBy_SanitationJobId(userData.DatabaseKey);

            //    SJC.PersonnelId = _Sanitation.assignmentModel.PersonnelId;
            //    SJC.ScheduledHours = _Sanitation.assignmentModel.ScheduledHours ?? 0;
            //    SJC.ScheduledStartDate = _Sanitation.assignmentModel.ScheduledStartDate ?? default(DateTime);

            //    SJC.UpdateIndex = _Sanitation.assignmentModel.UpdateIndex;

            //    SJC.UpdateForSanitationJob(userData.DatabaseKey);
            //}
            //return SJC;

            #endregion
        }
        public bool DeleteAssignment(long sanitationJobScheduledId, long SanitationJobId)
        {
            try
            {
                SanitationJobSchedule sanitationJobSchedule = new SanitationJobSchedule()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SanitationJobScheduleId = sanitationJobScheduledId,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId,
                    SanitationJobId = SanitationJobId
                };
                sanitationJobSchedule.RetrieveSingleBy_SanitationJobId(this.userData.DatabaseKey);
                sanitationJobSchedule.Del = true;
                sanitationJobSchedule.DeleteForSanitationJob(this.userData.DatabaseKey);
                int RowCount = GetAssignmentListCount(SanitationJobId);
                if (RowCount <= 0)
                {
                    CreateEventLog(sanitationJobSchedule.SanitationJobId, SanitationEvents.Approved);//-----------SOM-1633-------------// 
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int GetAssignmentListCount(long _SanitationJobId)
        {
            int RowCount = 0;
            if (Convert.ToInt64(_SanitationJobId) > -1)
            {
                SanitationJobSchedule sanitationJobSchedule = new SanitationJobSchedule();
                sanitationJobSchedule.ClientId = userData.DatabaseKey.Client.ClientId;
                sanitationJobSchedule.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                sanitationJobSchedule.SanitationJobId = _SanitationJobId;
                List<SanitationJobSchedule> x = sanitationJobSchedule.RetrieveAllBy_SanitationJobId(userData.DatabaseKey);
                if (x != null && x.Count > 0)
                {
                    RowCount = x.Count;
                }
            }
            return RowCount;
        }
        #endregion

        #region Tools
        public List<Client.Models.Sanitation.ToolsModel> PopulateTools(long SanitationJobId)
        {
            Client.Models.Sanitation.ToolsModel objtoolsModel;
            List<Client.Models.Sanitation.ToolsModel> ToolsModelList = new List<Client.Models.Sanitation.ToolsModel>();
            SanitationPlanning sanitationPlanning = new SanitationPlanning();

            sanitationPlanning.ClientId = userData.DatabaseKey.Client.ClientId;
            sanitationPlanning.Category = SanitationJobConstant.SanitationPlanning_CategoryTool;
            sanitationPlanning.SanitationJobId = SanitationJobId;
            List<SanitationPlanning> ToolsList = sanitationPlanning.SanitationPlanningRetrieveBy_SanitationJobID(userData.DatabaseKey);
            if (ToolsList != null)
            {
                foreach (var item in ToolsList)
                {
                    objtoolsModel = new Client.Models.Sanitation.ToolsModel();
                    objtoolsModel.SanitationPlanningId = item.SanitationPlanningId;
                    objtoolsModel.Quantity = item.Quantity;
                    objtoolsModel.Description = item.Description;
                    objtoolsModel.Instructions = item.Instructions;
                    objtoolsModel.CategoryValue = item.CategoryValue;
                    ToolsModelList.Add(objtoolsModel);
                }
            }
            return ToolsModelList;
        }
        public SanitationVM EditTools(long SanitationJobId, long _SanitationPlanningId, string ClientLookupId)
        {
            SanitationVM objSanitationJobVM = new SanitationVM();
            ToolsModel objToolsModel = new ToolsModel();
            CommonWrapper comWrapper = new CommonWrapper(userData);
            objToolsModel.ClientLookupId = ClientLookupId;

            SanitationPlanning SP = new SanitationPlanning()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SanitationPlanningId = _SanitationPlanningId,
                SanitationJobId = SanitationJobId
            };
            SP.Retrieve(userData.DatabaseKey);
            objToolsModel.SanitationJobId = SP.SanitationJobId;
            objToolsModel.CategoryValue = SP.CategoryValue;
            objToolsModel.Description = SP.Description;
            objToolsModel.Instructions = SP.Instructions;
            objToolsModel.Quantity = SP.Quantity;
            objToolsModel.SanitationPlanningId = _SanitationPlanningId;
            objSanitationJobVM.toolModel = objToolsModel;

            SanitationJobModel SJM = new SanitationJobModel();
            //SJM.ImageURI = GetSanitationJobImageUrl(SanitationJobId);
            SJM.ImageURI = comWrapper.GetAzureImageUrl(SanitationJobId, AttachmentTableConstant.Sanitation);
            objSanitationJobVM.sanitationJobModel = SJM;
            return objSanitationJobVM;
        }

        public List<String> AddOrUpdateTools(SanitationVM sanVM, ref string Mode)
        {
            SanitationPlanning Splan = new SanitationPlanning()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SanitationMasterId = 0,
                SanitationJobId = sanVM.toolModel.SanitationJobId,
                Category = SanitationJobConstant.SanitationPlanning_CategoryTool,
                CategoryId = 0,
                CategoryValue = sanVM.toolModel.CategoryValue,
                Description = sanVM.toolModel.Description,
                Dilution = string.Empty,
                Instructions = sanVM.toolModel.Instructions,
                Quantity = sanVM.toolModel.Quantity ?? 0,
            };
            if (sanVM.toolModel.SanitationPlanningId == 0)
            {
                Mode = "add";
                if (sanVM.toolModel.hdnDropdownDescription != null)
                {
                    #region String-split 
                    string temp = sanVM.toolModel.hdnDropdownDescription;
                    var index = temp.IndexOf('|');
                    if (index != -1)
                    {
                        string[] test = temp.Split('|');
                        for (int i = 0; i < test.Length; i++)
                        {
                            if (i == test.Length - 1)
                            {
                                Splan.Description = test[test.Length - 1];
                            }
                        }
                    }
                    #endregion
                }
                Splan.Create_SanitationPlanning(userData.DatabaseKey);
            }
            else
            {
                Mode = "update";
                Splan.Description = sanVM.toolModel.Description;
                Splan.SanitationPlanningId = sanVM.toolModel.SanitationPlanningId;
                Splan.Update_SanitationPlanning(userData.DatabaseKey);
            }
            return Splan.ErrorMessages;

            #region Extra code
            //SanitationPlanning SP = new SanitationPlanning();

            //if (_Sanitation.toolModel.SanitationPlanningId == 0)
            //{
            //    var selectedItem = ToolLookUplist.Find(p => p.ListName == _Sanitation.toolModel.CategoryValue.ToString());
            //    SP.ClientId = userData.DatabaseKey.Client.ClientId;
            //    SP.SanitationMasterId = 0;
            //    SP.SanitationJobId = _Sanitation.toolModel.SanitationJobId;
            //    SP.Category = SanitationJobConstant.SanitationPlanning_CategoryTool;
            //    SP.CategoryId = 0;
            //    SP.CategoryValue = _Sanitation.toolModel.CategoryValue;
            //    SP.Description = selectedItem.Description;
            //    SP.Dilution = string.Empty;
            //    SP.Instructions = _Sanitation.toolModel.Instructions;
            //    SP.Quantity = _Sanitation.toolModel.Quantity ?? 0;
            //    SP.Create_SanitationPlanning(userData.DatabaseKey);
            //}
            //else
            //{
            //    SP.ClientId = userData.DatabaseKey.Client.ClientId;
            //    SP.SanitationMasterId = 0;
            //    SP.SanitationJobId = _Sanitation.toolModel.SanitationJobId;
            //    SP.Category = SanitationJobConstant.SanitationPlanning_CategoryTool;
            //    SP.CategoryId = 0;
            //    SP.CategoryValue = _Sanitation.toolModel.CategoryValue;
            //    SP.Description = _Sanitation.toolModel.Description;
            //    SP.Dilution = string.Empty;
            //    SP.Instructions = _Sanitation.toolModel.Instructions;
            //    SP.Quantity = _Sanitation.toolModel.Quantity ?? 0;
            //    SP.SanitationPlanningId = _Sanitation.toolModel.SanitationPlanningId;
            //    SP.Update_SanitationPlanning(userData.DatabaseKey);
            //}
            //return SP;
            #endregion
        }
        public bool DeleteTools(string _SanitationPlanningId)
        {
            try
            {
                SanitationPlanning sanitationPlanning = new SanitationPlanning()
                {
                    SanitationPlanningId = Convert.ToInt64(_SanitationPlanningId)
                };
                sanitationPlanning.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ChemicalSupplies
        public List<Client.Models.Sanitation.ChemicalSuppliesModel> PopulateChemicalSupplies(long SanitationJobId)
        {
            Client.Models.Sanitation.ChemicalSuppliesModel objChemicalSuppliesModel;
            List<Client.Models.Sanitation.ChemicalSuppliesModel> ChemicalSuppliesModelList = new List<Client.Models.Sanitation.ChemicalSuppliesModel>();
            SanitationPlanning sanitationPlanning = new SanitationPlanning();
            sanitationPlanning.ClientId = userData.DatabaseKey.Client.ClientId;
            sanitationPlanning.Category = SanitationJobConstant.SanitationPlanning_CategoryChemical;
            sanitationPlanning.SanitationJobId = SanitationJobId;
            List<SanitationPlanning> ChemicalSuppliesList = sanitationPlanning.SanitationPlanningRetrieveBy_SanitationJobID(userData.DatabaseKey);
            if (ChemicalSuppliesList != null)
            {
                foreach (var item in ChemicalSuppliesList)
                {
                    objChemicalSuppliesModel = new Client.Models.Sanitation.ChemicalSuppliesModel();
                    objChemicalSuppliesModel.SanitationPlanningId = item.SanitationPlanningId;
                    objChemicalSuppliesModel.Quantity = item.Quantity;
                    objChemicalSuppliesModel.Description = item.Description;
                    objChemicalSuppliesModel.Instructions = item.Instructions;
                    objChemicalSuppliesModel.CategoryValue = item.CategoryValue;
                    objChemicalSuppliesModel.Dilution = item.Dilution;
                    ChemicalSuppliesModelList.Add(objChemicalSuppliesModel);
                }
            }
            return ChemicalSuppliesModelList;
        }
        public ChemicalSuppliesModel getChemicalSuppliesById(long _SanitationPlanningId)
        {
            ChemicalSuppliesModel objChemicalSuppliesModel = new ChemicalSuppliesModel();
            SanitationPlanning SP = new SanitationPlanning()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SanitationPlanningId = _SanitationPlanningId,
            };
            SP.Retrieve(userData.DatabaseKey);

            objChemicalSuppliesModel.SanitationJobId = SP.SanitationJobId;
            objChemicalSuppliesModel.CategoryValue = SP.CategoryValue;
            objChemicalSuppliesModel.Description = SP.Description;
            objChemicalSuppliesModel.Instructions = SP.Instructions;
            objChemicalSuppliesModel.Dilution = SP.Dilution;
            objChemicalSuppliesModel.Quantity = SP.Quantity;

            return objChemicalSuppliesModel;
        }
        public bool DeleteChemicalSupplies(long _SanitationPlanningId)
        {
            try
            {
                SanitationPlanning sanitationPlanning = new SanitationPlanning()
                {
                    SanitationPlanningId = Convert.ToInt64(_SanitationPlanningId)
                };
                sanitationPlanning.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<String> AddOrUpadateChemical(SanitationVM sanVM, ref string Mode)  //public SanitationPlanning AddOrUpadateChemicalSupplies(SanitationVM _Sanitation, List<DataContracts.LookupList> ChemicalLookUplist)    
        {

            SanitationPlanning Splan = new SanitationPlanning()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SanitationMasterId = 0,
                SanitationJobId = sanVM.chemicalSuppliesModel.SanitationJobId,
                Category = SanitationJobConstant.SanitationPlanning_CategoryChemical,
                CategoryId = 0,
                CategoryValue = sanVM.chemicalSuppliesModel.CategoryValue,
                Description = sanVM.chemicalSuppliesModel.Description,
                Dilution = sanVM.chemicalSuppliesModel.Dilution,
                Instructions = sanVM.chemicalSuppliesModel.Instructions,
                Quantity = sanVM.chemicalSuppliesModel.Quantity ?? 0
            };

            if (sanVM.chemicalSuppliesModel.SanitationPlanningId == 0)
            {
                Mode = "add";
                if (sanVM.chemicalSuppliesModel.hdnDropdownDescription != null)
                {
                    #region String-split 
                    string temp = sanVM.chemicalSuppliesModel.hdnDropdownDescription;
                    var index = temp.IndexOf('|');
                    if (index != -1)
                    {
                        string[] test = temp.Split('|');
                        for (int i = 0; i < test.Length; i++)
                        {
                            if (i == test.Length - 1)
                            {
                                Splan.Description = test[test.Length - 1].Trim();
                            }
                        }
                    }
                    #endregion
                }
                Splan.Create_SanitationPlanning(userData.DatabaseKey);
            }
            else
            {
                Mode = "update";
                Splan.Description = sanVM.chemicalSuppliesModel.Description;
                Splan.SanitationPlanningId = sanVM.chemicalSuppliesModel.SanitationPlanningId;
                Splan.Update_SanitationPlanning(userData.DatabaseKey);
            }
            return Splan.ErrorMessages;

            #region Extra Code
            //SanitationPlanning SP = new SanitationPlanning();
            //if (_Sanitation.chemicalSuppliesModel.SanitationPlanningId == 0)
            //{
            //    var selectedItem = ChemicalLookUplist.Find(p => p.ListName == _Sanitation.chemicalSuppliesModel.CategoryValue.ToString());
            //    SP.ClientId = userData.DatabaseKey.Client.ClientId;
            //    SP.SanitationMasterId = 0;
            //    SP.SanitationJobId = _Sanitation.chemicalSuppliesModel.SanitationJobId;
            //    SP.Category = SanitationJobConstant.SanitationPlanning_CategoryChemical;
            //    SP.CategoryId = 0;
            //    SP.CategoryValue = _Sanitation.chemicalSuppliesModel.CategoryValue;
            //    SP.Description = selectedItem.Description;
            //    SP.Dilution = _Sanitation.chemicalSuppliesModel.Dilution;
            //    SP.Instructions = _Sanitation.chemicalSuppliesModel.Instructions;
            //    SP.Quantity = _Sanitation.chemicalSuppliesModel.Quantity ?? 0;

            //    SP.Create_SanitationPlanning(userData.DatabaseKey);
            //}
            //else
            //{
            //    SP.ClientId = userData.DatabaseKey.Client.ClientId;
            //    SP.SanitationMasterId = 0;
            //    SP.SanitationJobId = _Sanitation.chemicalSuppliesModel.SanitationJobId;
            //    SP.Category = SanitationJobConstant.SanitationPlanning_CategoryChemical;
            //    SP.CategoryId = 0;
            //    SP.CategoryValue = _Sanitation.chemicalSuppliesModel.CategoryValue;
            //    SP.Description = _Sanitation.chemicalSuppliesModel.Description;
            //    SP.Dilution = _Sanitation.chemicalSuppliesModel.Dilution;
            //    SP.Instructions = _Sanitation.chemicalSuppliesModel.Instructions;
            //    SP.Quantity = _Sanitation.chemicalSuppliesModel.Quantity ?? 0;

            //    SP.SanitationPlanningId = _Sanitation.chemicalSuppliesModel.SanitationPlanningId;
            //    SP.Update_SanitationPlanning(userData.DatabaseKey);
            //}
            //return SP;
            #endregion
        }
        #endregion

        public SanitationJobModel getSanitationJobDetailsById(long _SanitationJobId)
        {
            SanitationJobModel sanitationJobModel = new SanitationJobModel();
            SanitationJob sjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SanitationJobId = _SanitationJobId
            };
            sjob.Retrieve(this.userData.DatabaseKey);
            sanitationJobModel = initializeControls(sjob);
            return sanitationJobModel;
        }
        public SanitationJobModel initializeControls(SanitationJob SJ)
        {
            SanitationJobModel sanitationJobModel = new SanitationJobModel();

            sanitationJobModel.ClientLookupId = SJ.ClientLookupId;

            return sanitationJobModel;
        }
        //public string GetSanitationJobImageUrl(long SanitationJobId)
        //{
        //    string imageurl = string.Empty;
        //    bool lExternal = false;
        //    string sasToken = string.Empty;
        //    SanitationJob SanitJob = new SanitationJob()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        SiteId = userData.DatabaseKey.User.DefaultSiteId,
        //        CallerUserName = userData.DatabaseKey.Client.CallerUserName,
        //        SanitationJobId = SanitationJobId
        //    };

        //    Attachment attach = new Attachment()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        ObjectName = "Sanitation",
        //        ObjectId = SanitationJobId,
        //        Profile = true,
        //        Image = true
        //    };
        //    List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
        //    if (AList.Count > 0)
        //    {
        //        lExternal = AList.First().External;
        //        imageurl = AList.First().AttachmentURL;
        //    }

        //    else
        //    {
        //        imageurl = SanitJob.ImageURI;
        //        if (imageurl == null || imageurl == "")
        //        {
        //            imageurl = "No Image";
        //            lExternal = true;
        //        }
        //        else if (imageurl.Contains("somaxclientstorage"))
        //        {
        //            lExternal = false;
        //        }
        //        else
        //        {
        //            lExternal = true;
        //        }
        //    }
        //    if (lExternal)
        //    {
        //        if (imageurl == "No Image")
        //        {
        //            const string UploadDirectory = "../Images/DisplayImg/";
        //            const string ThumbnailFileName = "NoImage.jpg";
        //            imageurl = UploadDirectory + ThumbnailFileName;
        //        }
        //        else
        //        {
        //            imageurl = imageurl;
        //        }
        //    }
        //    else // SOMAX Storage 
        //    {
        //        AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
        //        sasToken = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, imageurl);
        //        if (sasToken == null || sasToken == "" || imageurl.Contains("No Image"))
        //        {
        //            const string UploadDirectory = "../Images/DisplayImg/";
        //            const string ThumbnailFileName = "NoImage.jpg";
        //            imageurl = UploadDirectory + ThumbnailFileName;
        //        }
        //        else
        //        {
        //            imageurl = sasToken;
        //        }
        //    }
        //    return imageurl;
        //}

        #region SanitationOnlyDashBoard
        public int GetCount(string ChartType)
        {
            int count = 0;
            SanitationJob sjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                ChartType = ChartType,
                TooDay = DateTime.UtcNow.ConvertFromUTCToUser(userData.Site.TimeZone)
            };
            var sanitationJobs = sjob.RetrieveDashboardChart(userData.DatabaseKey, sjob);
            if (sanitationJobs != null && sanitationJobs.Count > 0)
            {
                count = sanitationJobs[0].SanitationJobCount;
            }
            return count;
        }
        public Chart GetBarChartOfJobsByStatusData(int QueryId)
        {
            SanitationJob sjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                TimeFrame = QueryId,
                ChartType = "JobsBystatus",

            };
            List<string> ColorList = new List<string>();
            Chart _chart = new Chart();
            var chartData = sjob.RetrieveDashboardChart(userData.DatabaseKey, sjob);
            if (chartData != null && chartData.Count > 0)
            {
                _chart.labels = chartData.Select(x => UtilityFunction.GetMessageFromResource(x.Status, LocalizeResourceSetConstants.StatusDetails)).ToArray();
                _chart.datasets = new List<Datasets>();
                List<Datasets> _dataSet = new List<Datasets>();
                _dataSet.Add(new Datasets()
                {
                    data = chartData.Select(x => Convert.ToInt64(x.SanitationJobCount)).ToArray(),
                });
                if (_dataSet != null)
                {
                    _dataSet[0].backgroundColor = ColorList.ToArray();
                    _dataSet[0].borderColor = ColorList.ToArray();
                }

                _chart.datasets = _dataSet;
            }
            return _chart;
        }
        public List<SanitationJob> GetDoughnutbyPassFailChart(int QueryId)
        {
            SanitationJob sjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                TimeFrame = QueryId,
                ChartType = "JobsPassOrFail",
            };
            var chartData = sjob.RetrieveDashboardChart(userData.DatabaseKey, sjob);
            return chartData;
        }
        #endregion
        #region V2-912  
        public SanitationJob SanitationJobApprove(long SanitationJobId)
        {
            SanitationJob sanitationjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = SanitationJobId
            };

            sanitationjob.RetrieveByPKForeignKeys(userData.DatabaseKey);

            if (sanitationjob.Status == SanitationJobConstant.JobRequest && userData.Security.SanitationJob_Approve.Access == true)
            {

                {
                    sanitationjob.Status = SanitationJobConstant.Approved;
                    sanitationjob.ApproveDate = DateTime.UtcNow;
                    sanitationjob.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    sanitationjob.Update(this.userData.DatabaseKey);
                    if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count != 0)
                    {
                        return sanitationjob;
                    }
                    else
                    {
                        CreateEventLog(sanitationjob.SanitationJobId, SanitationEvents.Approved);
                    }
                }

            }


            return sanitationjob;
        }

        public SanitationJob SanitationJobComplete(long SanitationJobId)
        {
            SanitationJob sanitationjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = SanitationJobId
            };

            sanitationjob.RetrieveByPKForeignKeys(userData.DatabaseKey);

            if (userData.Security.SanitationJob_Complete.Access == true && (sanitationjob.Status == SanitationJobConstant.Approved || sanitationjob.Status == SanitationJobConstant.Scheduled))
            {

                sanitationjob.Status = SanitationJobConstant.Complete;
                sanitationjob.CompleteDate = DateTime.UtcNow;
                sanitationjob.CompleteBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                sanitationjob.Update(this.userData.DatabaseKey);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count != 0)
                {
                    return sanitationjob;
                }
                else
                {
                    CreateEventLog(sanitationjob.SanitationJobId, SanitationEvents.Complete);
                }

            }


            return sanitationjob;
        }

        public SanitationJob SanitationJobPass(long SanitationJobId)
        {
            SanitationJob sanitationjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = SanitationJobId
            };

            sanitationjob.RetrieveByPKForeignKeys(userData.DatabaseKey);

            if (userData.Security.SanitationJob_Complete.Access == true && sanitationjob.Status == SanitationJobConstant.Complete)
            {

                sanitationjob.Status = SanitationJobConstant.Pass;
                sanitationjob.PassDate = DateTime.UtcNow;
                sanitationjob.PassBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                sanitationjob.Update(this.userData.DatabaseKey);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count != 0)
                {
                    return sanitationjob;
                }
                else
                {
                    CreateEventLog(sanitationjob.SanitationJobId, SanitationEvents.Passed);
                }

            }


            return sanitationjob;
        }

        public SanitationJob SanitationJobFailVarification(FailVarificationModel failVarificationModel)
        {
            SanitationJob sanitationjob = new SanitationJob()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.Client.CallerUserName,
                SanitationJobId = failVarificationModel.SanitationJobId
            };

            sanitationjob.RetrieveByPKForeignKeys(userData.DatabaseKey);

            if (!string.IsNullOrEmpty(failVarificationModel.FailReason) && sanitationjob.SanitationJobId > 0)
            {
                sanitationjob.FailReason = failVarificationModel.FailReason;
                sanitationjob.FailComment = failVarificationModel.Comments;
                sanitationjob.Status = SanitationJobConstant.Fail;
                sanitationjob.FailDate = DateTime.UtcNow;
                sanitationjob.FailBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                sanitationjob.Update(this.userData.DatabaseKey);
                if (sanitationjob.ErrorMessages != null && sanitationjob.ErrorMessages.Count != 0)
                {
                    return sanitationjob;
                }
                else
                {
                    CreateEventLog(sanitationjob.SanitationJobId, SanitationEvents.Fail);

                    //creating New Record follow up Sanitation job
                    SanitationJob sanitationjobObj = SaveSanitationRecord(sanitationjob);
                    if (sanitationjobObj.ErrorMessages != null && sanitationjobObj.ErrorMessages.Count != 0)
                    {
                        return sanitationjobObj;

                    }
                    else
                    {


                        //New Sanitionjobtask Added for Newly created Sanitation Record
                        CreateSanitationJobTask(failVarificationModel, sanitationjobObj);
                        //creating sanitationEventlog for new Sanitation job  --create ,job Request
                        CreateEventLogForfail(sanitationjobObj.SanitationJobId, SanitationEvents.Create, failVarificationModel.SanitationJobId);
                        CreateEventLogForfail(sanitationjobObj.SanitationJobId, SanitationEvents.JobRequest, failVarificationModel.SanitationJobId);
                        sanitationjob = sanitationjobObj;
                    }

                }
            }


            return sanitationjob;
        }

        private void CreateSanitationJobTask(FailVarificationModel failVarificationModel, SanitationJob sanitationjobObj)
        {
            DataContracts.SanitationJobTask task = new DataContracts.SanitationJobTask()
            {
                SanitationJobId = failVarificationModel.SanitationJobId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };

            List<SanitationJobTask> TaskList = task.SanitationJobTask_RetrieveBy_SanitationJobId(this.userData.DatabaseKey);
            //copying all Tasks of old Sanitation job for new Sanitation job
            foreach (SanitationJobTask SanitationTask in TaskList)
            {
                SanitationJobTask sjTask = CreateSanitationJobTask(sanitationjobObj.SanitationJobId, SanitationTask);
                //if (sjTask.ErrorMessages != null && sjTask.ErrorMessages.Count != 0)
                //{
                //    break;

                //}
            }
        }

        private SanitationJob SaveSanitationRecord(SanitationJob sanitationjob)
        {
            SanitationJob sanitationjobObj = new SanitationJob
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            string newClientlookupId = "";

            if (SanitationJobConstant.SanitaionJob_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.SANIT_ANNUAL, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            sanitationjobObj.SiteId = userData.DatabaseKey.Personnel.SiteId;
            sanitationjobObj.ClientLookupId = newClientlookupId;
            sanitationjobObj.Status = SanitationJobConstant.JobRequest;
            sanitationjobObj.SourceType = SanitationJobConstant.Followup;

            sanitationjobObj.ChargeToId = sanitationjob?.ChargeToId ?? 0;
            sanitationjobObj.ChargeType = sanitationjob?.ChargeType ?? string.Empty;
            sanitationjobObj.ChargeTo_Name = sanitationjob?.ChargeTo_Name ?? string.Empty;
            sanitationjobObj.Description = sanitationjob?.Description ?? string.Empty;

            sanitationjobObj.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            sanitationjobObj.SanitationJob_CreateByFk_V2(this.userData.DatabaseKey);
            return sanitationjobObj;
        }

        internal SanitationJobTask CreateSanitationJobTask(long SanitationJobId, SanitationJobTask task)
        {
            SanitationJobTask sjTask = new SanitationJobTask();

            sjTask.SanitationJobId = SanitationJobId;
            sjTask.ClientId = userData.DatabaseKey.Personnel.ClientId;
            sjTask.TaskId = task.TaskId;
            sjTask.Status = SanitationJobConstant.Open;
            sjTask.CancelReason = string.Empty;
            sjTask.CompleteBy_PersonnelId = 0;
            sjTask.Description = task.Description;
            sjTask.SanOnDemandMasterTaskId = task.SanOnDemandMasterTaskId;
            sjTask.CreateNew_SanitationJobTask(userData.DatabaseKey, userData.Site.TimeZone);
            return sjTask;
        }
        private void CreateEventLogForfail(Int64 sanId, string Status, Int64 SourceSanitationId)
        {
            SanitationEventLog log = new SanitationEventLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = sanId;
            log.Event = Status;
            log.TransactionDate = DateTime.UtcNow;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.Comments = "";
            log.SourceId = SourceSanitationId;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region Save Work Request V2-1055
        public WorkOrder AddWorkRequestDynamic(SanitationVM objSanitationVM, ref List<string> errorMsg)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrder obj = new WorkOrder();
            WorkOrder workorder = new WorkOrder
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            string newClientlookupId = "";
            if (WorkOrderStatusConstants.Wo_AutoGenerateEnabled)
            {
                newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.WO_Annual, userData.DatabaseKey.User.DefaultSiteId, "");
            }

            workorder.SiteId = userData.DatabaseKey.Personnel.SiteId;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration().Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objSanitationVM.AddWorkRequest);
                getpropertyInfo = objSanitationVM.AddWorkRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objSanitationVM.AddWorkRequest);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = workorder.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(workorder, val);
            }
            workorder.DepartmentId = userData.DatabaseKey.Personnel.DepartmentId;
            #region V2-948
            if (userData.Site.SourceAssetAccount == true && workorder.Labor_AccountId == 0)
            {
                Equipment equipment = new Equipment
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    EquipmentId = workorder.ChargeToId
                };
                equipment.Retrieve(this.userData.DatabaseKey);
                workorder.Labor_AccountId = equipment.Labor_AccountId;
            }
            #endregion
            workorder.ChargeToId = 0;
            workorder.ChargeType = objSanitationVM.ChargeType;
            workorder.ChargeToClientLookupId = objSanitationVM.AddWorkRequest.ChargeToClientLookupId ?? string.Empty;// 
            workorder.ClientLookupId = newClientlookupId;
            workorder.SiteId = userData.DatabaseKey.Personnel.SiteId;
            workorder.Creator_PersonnelClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId.ToString();
            workorder.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            workorder.CreateMode = true;
            workorder.Status = WorkOrderStatusConstants.WorkRequest;
            workorder.SourceId = objSanitationVM.SanitationJobId ?? 0;
            workorder.SourceType = WorkOrderSourceTypes.Sanitation;

            workorder.CreateByPKForeignKeys(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (workorder.ErrorMessages != null && workorder.ErrorMessages.Count == 0)
            {
                ProcessAlert objAlert = new ProcessAlert(this.userData);
                List<long> wos = new List<long>() { workorder.WorkOrderId };
                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.WorkOrder>(AlertTypeEnum.WorkRequestApprovalNeeded, wos));
                Task CreateEventTask1 = Task.Factory.StartNew(() => CreateWorkOrderEventLog(workorder.WorkOrderId, "Create"));
                Task CreateEventTask2 = Task.Factory.StartNew(() => CreateWorkOrderEventLog(workorder.WorkOrderId, "WorkRequest"));
                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddWorkRequestUDFDynamic(objSanitationVM.AddWorkRequest, workorder.WorkOrderId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        workorder.ErrorMessages.AddRange(errors);
                    }

                }

            }
            else
            {
                errorMsg = workorder.ErrorMessages;
            }
            return workorder;
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
        public List<string> AddWorkRequestUDFDynamic(Models.Work_Order.UIConfiguration.AddWorkRequestModelDynamic woRequest, long WorkOrderId,
   List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            WorkOrderUDF woUDF = new WorkOrderUDF();
            woUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            woUDF.WorkOrderId = WorkOrderId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, woRequest);
                getpropertyInfo = woRequest.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(woRequest);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = woUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(woUDF, val);
            }

            woUDF.Create(_dbKey);
            return woUDF.ErrorMessages;
        }
        #endregion
        #region V2-1071 DevExpressPrint
        public List<SanitationJobDevExpressPrintModel> PrintDevExpressFromIndex(List<long> sanitationJobIds)
        {
            // Initialize the list to hold the print models
            var sanitationJobDevExpressPrintModelList = new List<SanitationJobDevExpressPrintModel>();

            // Initialize wrappers
            var sjWrapper = new SanitationJobWrapper(userData);
            var commonWrapper = new CommonWrapper(userData);

            // Retrieve all necessary data in one call
            var sanitationJobBunchListInfo = sjWrapper.RetrieveAllBySanitationJobV2DevExpressPrint(sanitationJobIds);

            // Extract lists from the retrieved data
            var listOfSJ = sanitationJobBunchListInfo.listOfSJ;
            var listOfSPTool = sanitationJobBunchListInfo.listOfSanitationTool;
            var listOfSJChemical = sanitationJobBunchListInfo.listOfSanitationChemical;
            var listOfSJTask = sanitationJobBunchListInfo.listOfSanitationTask;
            var listOfTimecard = sanitationJobBunchListInfo.listOfTimecard;
            var listOfCompletion = sanitationJobBunchListInfo.listOfCompletion;
            var listOfVerification = sanitationJobBunchListInfo.listOfVerification;

            // Populate notes in parallel
            var notesList = new List<Notes>();
            Parallel.ForEach(sanitationJobIds, p =>
            {
                var notes = commonWrapper.PopulateComment(p, "sanitationJob");
                lock (notesList)
                {
                    notesList.AddRange(notes);
                }
            });

            // Process each sanitation job ID
            foreach (var item in sanitationJobIds)
            {
                var sanitationJobDetails = listOfSJ.FirstOrDefault(m => m.SanitationJobId == item);
                var listOfSJTools = listOfSPTool.Where(m => m.SanitationJobId == item).ToList();
                var listOfSJChemicals = listOfSJChemical.Where(m => m.SanitationJobId == item).ToList();
                var listOfSJTasks = listOfSJTask.Where(m => m.SanitationJobId == item).ToList();
                var listOfTimecards = listOfTimecard.Where(m => m.SanitationJobId == item).ToList();
                var listOfCompletions = listOfSJ.Where(m => m.SanitationJobId == item).ToList();
                var listOfVerifications = listOfSJ.Where(m => m.SanitationJobId == item).ToList();


                // Create and bind the print model
                var sanitationJobDevExpressPrintModel = new SanitationJobDevExpressPrintModel();
                BindSanitationJobDetails(sanitationJobDetails, ref sanitationJobDevExpressPrintModel);
                BindToolsTable(listOfSJTools, ref sanitationJobDevExpressPrintModel);
                BindChemicalsTable(listOfSJChemicals, ref sanitationJobDevExpressPrintModel);
                BindTaskTable(listOfSJTasks, ref sanitationJobDevExpressPrintModel);
                BindLaborTable(listOfTimecards, ref sanitationJobDevExpressPrintModel);
                BindCompletionsTable(listOfCompletions, ref sanitationJobDevExpressPrintModel);
                BindVerificationsTable(listOfVerifications, ref sanitationJobDevExpressPrintModel);

                // Set additional properties
                sanitationJobDevExpressPrintModel.OnPremise = userData.DatabaseKey.Client.OnPremise;

                // Add the model to the list
                sanitationJobDevExpressPrintModelList.Add(sanitationJobDevExpressPrintModel);
            }

            return sanitationJobDevExpressPrintModelList;
        }
        public SanitationJob RetrieveAllBySanitationJobV2DevExpressPrint(List<long> SanitationJobIDList = null)
        {
            // Initialize a new instance of SanitationJob
            var sj = new SanitationJob
            {
                // Set the ClientId from userData
                ClientId = userData.DatabaseKey.Client.ClientId,

                // Set the SiteId from userData
                SiteId = userData.DatabaseKey.User.DefaultSiteId,

                // Convert the list of SanitationJobIDList to a comma-separated string if it is not null and has elements
                SanitationJobIDList = (SanitationJobIDList?.Count > 0) ? string.Join(",", SanitationJobIDList) : string.Empty
            };

            // Call the method to retrieve all sanitation jobs for DevExpress print
            sj.RetrieveAllBySanitationJobV2DevExpressPrint(userData.DatabaseKey, userData.Site.TimeZone);

            // Return the populated SanitationJob object
            return sj;
        }
        private void BindSanitationJobDetails(SanitationJob SanitationJobDetails, ref SanitationJobDevExpressPrintModel sanitationJobDevExpressPrintModel)
        {
            // Map basic properties from SanitationJobDetails to sanitationJobDevExpressPrintModel
            sanitationJobDevExpressPrintModel.SanitationJobId = SanitationJobDetails.SanitationJobId;
            sanitationJobDevExpressPrintModel.ClientLookupId = SanitationJobDetails.ClientLookupId;
            sanitationJobDevExpressPrintModel.ChargeToId = SanitationJobDetails.ChargeToId;
            sanitationJobDevExpressPrintModel.ChargeTo_ClientLookupId = SanitationJobDetails.ChargeTo_ClientLookupId;
            sanitationJobDevExpressPrintModel.ChargeTo_Name = SanitationJobDetails.ChargeTo_Name;
            sanitationJobDevExpressPrintModel.ChargeType = SanitationJobDetails.ChargeType;
            sanitationJobDevExpressPrintModel.Shift = SanitationJobDetails.Shift;
            sanitationJobDevExpressPrintModel.Status = SanitationJobDetails.Status;

            // Format ScheduledDate if it is not null or default
            sanitationJobDevExpressPrintModel.ScheduledDate = (SanitationJobDetails.ScheduledDate == null || SanitationJobDetails.ScheduledDate == default(DateTime))
                ? string.Empty
                : SanitationJobDetails.ScheduledDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            // Map remaining properties
            sanitationJobDevExpressPrintModel.ScheduledDuration = SanitationJobDetails.ScheduledDuration;
            sanitationJobDevExpressPrintModel.Description = SanitationJobDetails.Description;
            sanitationJobDevExpressPrintModel.AssignedTo_PersonnelId = SanitationJobDetails.AssignedTo_PersonnelId;
            sanitationJobDevExpressPrintModel.Assigned = SanitationJobDetails.Assigned;
            sanitationJobDevExpressPrintModel.CreateDate = SanitationJobDetails.CreateDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            sanitationJobDevExpressPrintModel.CreateBy = SanitationJobDetails.CreateBy;

            // Format CompleteDate if it is not null or default
            sanitationJobDevExpressPrintModel.CompleteDate = (SanitationJobDetails.CompleteDate == null || SanitationJobDetails.CompleteDate == default(DateTime))
                ? string.Empty
                : SanitationJobDetails.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            sanitationJobDevExpressPrintModel.CompleteBy = SanitationJobDetails.CompleteBy;
            sanitationJobDevExpressPrintModel.CompleteComments = SanitationJobDetails.CompleteComments;
            sanitationJobDevExpressPrintModel.PassDate = Convert.ToDateTime(SanitationJobDetails.PassDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            sanitationJobDevExpressPrintModel.PassBy = SanitationJobDetails.PassBy;
            sanitationJobDevExpressPrintModel.AssetLocation = SanitationJobDetails.AssetLocation;

            // Localization section
            #region Localization
            sanitationJobDevExpressPrintModel.spnSanitationJob = UtilityFunction.GetMessageFromResource("spnSanitationJob", LocalizeResourceSetConstants.SanitationDetails);
            sanitationJobDevExpressPrintModel.spnDetails = UtilityFunction.GetMessageFromResource("spnDetails", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnChargeToName = UtilityFunction.GetMessageFromResource("spnChargeToName", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnShift = UtilityFunction.GetMessageFromResource("spnShift", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnStatus = UtilityFunction.GetMessageFromResource("spnStatus", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnScheduledDate = UtilityFunction.GetMessageFromResource("spnScheduledDate", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnScheduledDuration = UtilityFunction.GetMessageFromResource("spnScheduledDuration", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnSanitationCreateBy = UtilityFunction.GetMessageFromResource("spnSanitationCreateBy", LocalizeResourceSetConstants.SanitationDetails);
            sanitationJobDevExpressPrintModel.spnAssigned = UtilityFunction.GetMessageFromResource("spnAssigned", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnCompletion = UtilityFunction.GetMessageFromResource("spnCompletion", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnGlobalCompleted = UtilityFunction.GetMessageFromResource("spnGlobalCompleted", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnBy = UtilityFunction.GetMessageFromResource("spnBy", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnCompleteComments = UtilityFunction.GetMessageFromResource("spnCompleteComments", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnVerification = UtilityFunction.GetMessageFromResource("spnVerification", LocalizeResourceSetConstants.SanitationDetails);
            sanitationJobDevExpressPrintModel.globalCreateDate = UtilityFunction.GetMessageFromResource("globalCreateDate", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.GlobalReason = UtilityFunction.GetMessageFromResource("GlobalReason", LocalizeResourceSetConstants.Global);
            sanitationJobDevExpressPrintModel.spnComment = UtilityFunction.GetMessageFromResource("spnComment", LocalizeResourceSetConstants.SanitationDetails);
            sanitationJobDevExpressPrintModel.spnCopyRights = UtilityFunction.GetMessageFromResource("spnCopyRights", LocalizeResourceSetConstants.Global);
            #endregion
        }
        private void BindToolsTable(List<SanitationPlanning> listOfSJTool, ref SanitationJobDevExpressPrintModel sanitationJobDevExpressPrintModel)
        {
            // Check if the list of tools is not empty
            if (listOfSJTool.Count > 0)
            {
                foreach (var item in listOfSJTool)
                {
                    // Initialize and map properties from SanitationPlanning to ToolsDevExpressPrintModel
                    var objToolDevExpressPrintModel = new ToolsDevExpressPrintModel
                    {
                        SanitationJobId = item.SanitationJobId,
                        Quantity = item.Quantity,
                        CategoryValue = item.CategoryValue,
                        Description = item.Description,
                        Instructions = item.Instructions,
                        // Localization
                        spnTools = UtilityFunction.GetMessageFromResource("spnTools", LocalizeResourceSetConstants.SanitationDetails),
                        spnQuantity = UtilityFunction.GetMessageFromResource("spnQuantity", LocalizeResourceSetConstants.Global),
                        spnTool = UtilityFunction.GetMessageFromResource("spnTool", LocalizeResourceSetConstants.SanitationDetails),
                        spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global),
                        spnInstructions = UtilityFunction.GetMessageFromResource("spnInstructions", LocalizeResourceSetConstants.SanitationDetails)
                    };

                    // Add the mapped model to the Tools collection
                    sanitationJobDevExpressPrintModel.Tools.Add(objToolDevExpressPrintModel);
                }
            }
        }
        private void BindChemicalsTable(List<SanitationPlanning> listOfSJChemicals, ref SanitationJobDevExpressPrintModel sanitationJobDevExpressPrintModel)
        {
            // Check if the list of chemicals is not empty
            if (listOfSJChemicals.Count > 0)
            {
                foreach (var item in listOfSJChemicals)
                {
                    // Initialize and map properties from SanitationPlanning to ChemicalsDevExpressPrintModel
                    var objChemicalsDevExpressPrintModel = new ChemicalsDevExpressPrintModel
                    {
                        SanitationJobId = item.SanitationJobId,
                        Quantity = item.Quantity,
                        CategoryValue = item.CategoryValue,
                        Description = item.Description,
                        Instructions = item.Instructions,
                        Dilution = item.Dilution,
                        // Localization
                        spnChemicals = UtilityFunction.GetMessageFromResource("spnChemicals", LocalizeResourceSetConstants.SanitationDetails),
                        spnQuantity = UtilityFunction.GetMessageFromResource("spnQuantity", LocalizeResourceSetConstants.Global),
                        spnChemical = UtilityFunction.GetMessageFromResource("spnChemical", LocalizeResourceSetConstants.SanitationDetails),
                        spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global),
                        spnInstructions = UtilityFunction.GetMessageFromResource("spnInstructions", LocalizeResourceSetConstants.SanitationDetails),
                        spnDilution = UtilityFunction.GetMessageFromResource("spnDilution", LocalizeResourceSetConstants.SanitationDetails)
                    };

                    // Add the mapped model to the Chemicals collection
                    sanitationJobDevExpressPrintModel.Chemicals.Add(objChemicalsDevExpressPrintModel);
                }
            }
        }
        private void BindTaskTable(List<SanitationJobTask> listOfSJTasks, ref SanitationJobDevExpressPrintModel sanitationJobDevExpressPrintModel)
        {
            // Check if the list of tasks is not empty
            if (listOfSJTasks.Count > 0)
            {
                foreach (var item in listOfSJTasks)
                {
                    // Initialize and map properties from SanitationJobTask to SJTasksDevExpressPrintModel
                    var objTasksDevExpressPrintModel = new SJTasksDevExpressPrintModel
                    {
                        SanitationJobId = item.SanitationJobId,
                        TaskId = item.TaskId,
                        Description = item.Description,
                        Completed = (item.Status == SanitationJobConstant.Complete ? "✔" : "X"),
                        Value = item.RecordedValue,
                        // Localization
                        spnTasks = UtilityFunction.GetMessageFromResource("spnTasks", LocalizeResourceSetConstants.Global),
                        spnOrder = UtilityFunction.GetMessageFromResource("spnOrder", LocalizeResourceSetConstants.Global),
                        spnDuration = UtilityFunction.GetMessageFromResource("spnDuration", LocalizeResourceSetConstants.Global),
                        spnGlobalCompleted = UtilityFunction.GetMessageFromResource("spnGlobalCompleted", LocalizeResourceSetConstants.Global),
                        spnValue = UtilityFunction.GetMessageFromResource("spnValue", LocalizeResourceSetConstants.Global),
                        spnDescription = UtilityFunction.GetMessageFromResource("spnDescription", LocalizeResourceSetConstants.Global)
                    };

                    // Add the mapped model to the Tasks collection
                    sanitationJobDevExpressPrintModel.Tasks.Add(objTasksDevExpressPrintModel);
                }
            }
        }

        private void BindLaborTable(List<Timecard> listOfTimecards, ref SanitationJobDevExpressPrintModel sanitationJobDevExpressPrintModel)
        {
            // Check if the list of timecards is not empty
            if (listOfTimecards.Count > 0)
            {
                foreach (var item in listOfTimecards)
                {
                    // Initialize and map properties from Timecard to SJLaborDevExpressPrintModel
                    var objSJLaborDevExpressPrintModel = new SJLaborDevExpressPrintModel
                    {
                        SanitationJobId = item.SanitationJobId,
                        TimecardId = item.TimecardId,
                        PersonnelID = item.PersonnelId,
                        PersonnelClientLookupId = item.PersonnelClientLookupId,
                        StartDate = Convert.ToDateTime(item.StartDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                        PersonnelName = item.NameFirst + " " + item.NameLast,
                        Hours = item.Hours,
                        // Localization
                        spnLabor = UtilityFunction.GetMessageFromResource("spnLabor", LocalizeResourceSetConstants.Global),
                        spnPersonnel = UtilityFunction.GetMessageFromResource("spnPersonnel", LocalizeResourceSetConstants.Global),
                        spnStartDate = UtilityFunction.GetMessageFromResource("spnStartDate", LocalizeResourceSetConstants.SanitationDetails),
                        spnName = UtilityFunction.GetMessageFromResource("spnName", LocalizeResourceSetConstants.Global),
                        spnHours = UtilityFunction.GetMessageFromResource("spnHours", LocalizeResourceSetConstants.Global)
                    };

                    // Add the mapped model to the Labors collection
                    sanitationJobDevExpressPrintModel.Labors.Add(objSJLaborDevExpressPrintModel);
                }
            }
        }

        private void BindCompletionsTable(List<SanitationJob> listOfCompletions, ref SanitationJobDevExpressPrintModel sanitationJobDevExpressPrintModel)
        {
            // Check if the list of completions is not empty
            if (listOfCompletions.Count > 0)
            {
                foreach (var item in listOfCompletions)
                {
                    // Initialize and map properties from SanitationJob to SJCompleteDevExpressPrintModel
                    var objSJCompleteDevExpressPrintModel = new SJCompleteDevExpressPrintModel
                    {
                        SanitationJobId = item.SanitationJobId,
                        CompleteDate = (item.CompleteDate == null || item.CompleteDate == default(DateTime))
                ? string.Empty
                : item.CompleteDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                        CompleteBy = item.CompleteBy,
                        CompleteComments = item.CompleteComments,
                        // Localization
                        spnCompletion = UtilityFunction.GetMessageFromResource("spnCompletion", LocalizeResourceSetConstants.Global),
                        spnGlobalCompleted = UtilityFunction.GetMessageFromResource("spnGlobalCompleted", LocalizeResourceSetConstants.Global),
                        spnBy = UtilityFunction.GetMessageFromResource("spnBy", LocalizeResourceSetConstants.Global),
                        spnCompleteComments = UtilityFunction.GetMessageFromResource("spnCompleteComments", LocalizeResourceSetConstants.Global)
                    };

                    // Add the mapped model to the Completions collection
                    sanitationJobDevExpressPrintModel.Completions.Add(objSJCompleteDevExpressPrintModel);
                }
            }
        }

        private void BindVerificationsTable(List<SanitationJob> listOfVerifications, ref SanitationJobDevExpressPrintModel sanitationJobDevExpressPrintModel)
        {
            // Check if the list of verifications is not empty
            if (listOfVerifications.Count > 0)
            {
                foreach (var item in listOfVerifications)
                {
                    // Initialize and map properties from SanitationJob to SJVerificationDevExpressPrintModel
                    var objSJVerificationDevExpressPrintModel = new SJVerificationDevExpressPrintModel();

                    if (item.PassDate != null && item.PassDate != default(DateTime))
                    {
                        objSJVerificationDevExpressPrintModel.VerificationStatus = "Pass";
                        objSJVerificationDevExpressPrintModel.VerificationDate = (item.PassDate == null || item.PassDate == default(DateTime))
? string.Empty
: item.PassDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        objSJVerificationDevExpressPrintModel.VerificationBy = item.PassBy;
                        objSJVerificationDevExpressPrintModel.VerificationCommentsVisible = false;
                        objSJVerificationDevExpressPrintModel.VerificationReasonVisible = false;
                        objSJVerificationDevExpressPrintModel.VerificationReason = "";
                        objSJVerificationDevExpressPrintModel.VerificationComments = "";
                    }
                    else if (item.FailDate != null && item.FailDate != default(DateTime))
                    {
                        objSJVerificationDevExpressPrintModel.VerificationStatus = "Fail";
                        objSJVerificationDevExpressPrintModel.VerificationDate = (item.FailDate == null || item.FailDate == default(DateTime))
                ? string.Empty
                : item.FailDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        objSJVerificationDevExpressPrintModel.VerificationBy = item.FailBy;
                        objSJVerificationDevExpressPrintModel.VerificationCommentsVisible = true;
                        objSJVerificationDevExpressPrintModel.VerificationReasonVisible = true;
                        objSJVerificationDevExpressPrintModel.VerificationReason = item.FailReason;
                        objSJVerificationDevExpressPrintModel.VerificationComments = item.FailComment;
                    }

                    // Localization
                    objSJVerificationDevExpressPrintModel.spnVerification = UtilityFunction.GetMessageFromResource("spnVerification", LocalizeResourceSetConstants.SanitationDetails);
                    objSJVerificationDevExpressPrintModel.spnStatus = UtilityFunction.GetMessageFromResource("spnStatus", LocalizeResourceSetConstants.Global);
                    objSJVerificationDevExpressPrintModel.spnDate = UtilityFunction.GetMessageFromResource("spnDate", LocalizeResourceSetConstants.Global);
                    objSJVerificationDevExpressPrintModel.spnBy = UtilityFunction.GetMessageFromResource("spnBy", LocalizeResourceSetConstants.Global);
                    objSJVerificationDevExpressPrintModel.GlobalReason = UtilityFunction.GetMessageFromResource("GlobalReason", LocalizeResourceSetConstants.Global);
                    objSJVerificationDevExpressPrintModel.spnComment = UtilityFunction.GetMessageFromResource("spnComment", LocalizeResourceSetConstants.Global);

                    // Add the mapped model to the Verifications collection
                    sanitationJobDevExpressPrintModel.Verifications.Add(objSJVerificationDevExpressPrintModel);
                }
            }
        }

        #endregion
    }
}
