using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.PartMasterResponseImport
{
    public class PartMasterResponseImportJSONModel
    {

        public PartMasterResponseImportJSONModel()
        {
            ECO_REQUEST_NUM = "";
            REQUEST_TYPE = "";
            ORACLE_PART_NUM = "";
            ORACLE_PART_ID = "";
            STATUS = "";

        }
        public string ECO_REQUEST_NUM { get; set; }
        public string REQUEST_TYPE { get; set; }
        public string ORACLE_PART_NUM { get; set; }
        public string ORACLE_PART_ID { get; set; }
        public string STATUS { get; set; }

    }
}