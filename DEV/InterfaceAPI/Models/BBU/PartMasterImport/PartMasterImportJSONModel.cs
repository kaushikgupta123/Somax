using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.PartMasterImport
{
    public class PartMasterImportJSONModel
    {
        public PartMasterImportJSONModel()
        {
            SOMAXITEM_DAT = "";
            ORACLE_PART_ID = "";
            ORACLE_PART_NUMBER = "";
            ORACLE_PLANT_ID = 0;
            PRICE = "";
            UUID = "";
            SHORT_DESCRIPTION = "";
            LONG_DESCRIPTION = "";
            MANUFACTURER = "";
            MFG_PART_NUMBER = "";
            ATL_PART_NUMBER_1 = "";
            ATL_PART_NUMBER_2 = "";
            ATL_PART_NUMBER_3 = "";
            PRIMARY_UNIT_OF_MEASURE = "";
            UNIT_OF_MEASURE_DESCRIPTION = "";
            UPC = "";
            UNSPSC_CODE_ID = "";
            UNSPSC_DESCRIPTION = "";
            IMAGE_URL = "";
            ENABLED_FLAG = "";
        }       
        public string SOMAXITEM_DAT { get; set; }
        public string ORACLE_PART_ID { get; set; }
        public string ORACLE_PART_NUMBER { get; set; }
        public Int64 ORACLE_PLANT_ID { get; set; }       
        public string PRICE { get; set; }
        public string UUID { get; set; }
        public string SHORT_DESCRIPTION { get; set; }
        public string LONG_DESCRIPTION { get; set; }
        public string MANUFACTURER { get; set; }       
        public string MFG_PART_NUMBER { get; set; }
        public string ATL_PART_NUMBER_1 { get; set; }
        public string ATL_PART_NUMBER_2 { get; set; }
        public string ATL_PART_NUMBER_3 { get; set; }
        public string PRIMARY_UNIT_OF_MEASURE { get; set; }
        public string UNIT_OF_MEASURE_DESCRIPTION { get; set; }
        public string UPC { get; set; }
        public string UNSPSC_CODE_ID { get; set; }
        public string UNSPSC_DESCRIPTION { get; set; }
        public string IMAGE_URL { get; set; }
        public string ENABLED_FLAG { get; set; }
    }
}