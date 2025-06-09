namespace Common.Constants
{
    /// <summary>
    /// These contants are used for identifying field names for the Search and Report fields, in order to persist the information on page load 
    /// and to reference the controls within a report configuration (see the ReportDefinitionDataStructure settings property).
    /// </summary>
    public class SearchCategoryConstants 
    {
        // TBL_ constants represent the source data table.
        public const string TBL_EQUIPMENT = "Equipment";
        public const string TBL_PERSONNEL = "Personnel";
        public const string TBL_LOCATION = "Location";
        public const string TBL_PROJECT = "Project";
        public const string TBL_WORKORDER = "WorkOrder";
        public const string TBL_LOOKUPLIST = "LookupList";
        public const string TBL_PART = "Part";
        public const string TBL_PARTMASTER = "PartMaster";
        public const string TBL_VENDOR = "Vendor";
        public const string TBL_CATAGORY = "PartCategoryMaster";        
        public const string TBL_ACCOUNT = "Account";
        public const string TBL_CRAFT = "Craft";
        public const string TBL_STOREROOM = "Storeroom";
        public const string TBL_LOCKOUT = "Lockout";
        public const string TBL_METER = "Meter";
        public const string TBL_PURCHASEORDER = "PurchaseOrder";
        public const string TBL_PREVMAINTMASTER = "PrevMaintMaster";
        // GRP_ constants represent a report grouping.
        public const string GRP_EQUIPMENT_LIST = "equiplist";
        public const string TBL_MANUFACTUREMASTER = "ManufacturerMaster";

        // P_ constants represent properties available to a report or search.
        public const string P_QUERY = "query";
        public const string P_SITE = "site";
        public const string P_AREA = "area";
        public const string P_DEPARTMENT = "dept";
        public const string P_TYPE = "type";
        public const string P_STATUS = "status";
        public const string P_DATES = "dates";
        public const string P_STARTDATE = "startdate";
        public const string P_ENDDATE = "enddate";
        public const string P_COLUMNS = "columns";
        public const string P_SEARCHTEXT = "searchtext";
        public const string P_USELIKE = "uselike";
        public const string P_STOCKTYPE = "stocktype";
        public const string P_ACCOUNTID = "acctid";
        public const string P_STOREROOM = "storeroom";
        public const string P_CREW = "crew";
        public const string P_CRAFT = "craft";
        public const string P_SHIFT = "shift";
        public const string P_CHARGETYPE = "chargetype";
        public const string P_SCHEDULETYPE = "schedtype";
        public const string P_PRIORITY = "priority";
        public const string P_SOURCE = "source";
    }
    public class LookupListSortOrderConstants
    {
        public const string S_NONE = "none";
        public const string S_VALUE = "value";
        public const string S_TEXT = "text";
    }
}
