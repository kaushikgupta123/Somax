namespace InterfaceAPI.Models.BBU.PartCategoryMasterImport
{
    public class PartCategoryMasterImportJSONModel
  {

    public PartCategoryMasterImportJSONModel()
    {
      CATEGORY_NAME = "";
      CATEGORY_CODE = "";
      DESCRIPTION = "";
      START_DATE_ACTIVE = "";
      END_DATE_ACTIVE = "";
      ENABLED_FLAG = "";
    }
    public string CATEGORY_NAME { get; set; }
    public string CATEGORY_CODE { get; set; }
    public string DESCRIPTION { get; set; }
    public string START_DATE_ACTIVE { get; set; }
    public string END_DATE_ACTIVE { get; set; }
    public string ENABLED_FLAG { get; set; }
    /*
        public PartCategoryMasterImportJSONModel()
        {
            ORACLE_CATEGORY = "";
            ORACLE_CATEGORY_DESCRIPTION = "";
            STATUS = "";
        }
        public string ORACLE_CATEGORY { get; set; }
        public string ORACLE_CATEGORY_DESCRIPTION { get; set; }
        public string STATUS { get; set; }
        */
    }
}