using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.PartMasterRequestExport
{
    public class PartMasterRequestExportModel
    {
        public string SOMAXECOREQ_DAT
        {
            get { return "SOMAXECOREQ-DAT".PadRight(15); }
        }
        public string ECO_REQUEST_NUM { get; set; }
        public string REQUEST_TYPE { get; set; }
        public string ORACLE_PLANT_ID { get; set; }
        public string SOMAX_PART_CLIENT_LOOKUP_ID { get; set; }
        public string ORACLE_PART_NUM { get; set; }
        public string ORACLE_PART_ID { get; set; }
        public string ORACLE_REPLACE_PART_NUM { get; set; }
        public string ORACLE_REPLACE_PART_ID { get; set; }

    }
}