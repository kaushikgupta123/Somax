using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.PartMasterResponseImport
{
    public class PartMasterResponseImportModel
    {

        public PartMasterResponseImportModel()
        {
            ClientId = 0;
            PartMasterResponseId = 0;
            PartMasterRequestId = 0;
            RequestType = "";
            EXPartNumber = "";
            EXPartId = 0;
            Status = "";
            ErrorMessage = "";
            LastProcessed = DateTime.MinValue;
            ImportLogId = 0;
            CreateBy = "";
            CreateDate = DateTime.MinValue;
            UpdateIndex = 0;
        }
        public long ClientId { get; set; }
        public long PartMasterResponseId { get; set; }
        public long PartMasterRequestId { get; set; }
        public string RequestType { get; set; }
        public string EXPartNumber { get; set; }
        public long EXPartId { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? LastProcessed { get; set; }
        public long ImportLogId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int UpdateIndex { get; set; }

    }
}