using System;

namespace Client.Models.Work_Order
{
    public class WorkOrderPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string AssetLocation { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Shift { get; set; }
        public string AssetGroup1ClientlookupId { get; set; }
        public string AssetGroup2ClientlookupId { get; set; }
        public string AssetGroup3ClientlookupId { get; set; } 
        public string Priority { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Creator { get; set; }
        public string Assigned { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public string FailureCode { get; set; }
        public DateTime? ActualFinishDate { get; set; }
        public decimal ActualDuration { get; set; }
        public string SourceType { get; set; }//<!--Added on 23/06/2020-->
                                              //<!--(Added on 25/06/2020)-->
        public DateTime? RequiredDate { get; set; }
        public string ProjectClientLookupId { get; set; } //V2-850
        public bool DownRequired { get; set; }//V2-892
        public string PlannerFullName { get; set; } //V2-1078
        public long AssetGroup1Id { get; set; }
        public long AssetGroup2Id { get; set; }
        public long AssetGroup3Id { get; set; }

        public string AssetGroup1AdvSearchId { get; set; }
        public string AssetGroup2AdvSearchId { get; set; }
        public string AssetGroup3AdvSearchId { get; set; }

        //<!--(Added on 25/06/2020)-->
        //public string CompleteComments { get; set; }     
        public string TxtSearchval { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int? OffSetVal { get; set; }
        public int NextRow { get; set; }
        public long LoggedInUserPEID { get; set; }
        public string StartCreateDate { get; set; }
        public string EndCreateDate { get; set; }
        public string StartScheduled { get; set; }
        public string EndScheduled { get; set; }
        public string StartActualFinish { get; set; }
        public string EndActualFinish { get; set; }
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string AssignedFullName { get; set; }        
       
        public long WorkOrderId { get; set; }
        
    }
}