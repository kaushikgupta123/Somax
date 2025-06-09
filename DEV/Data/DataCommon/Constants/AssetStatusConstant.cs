namespace Common.Constants
{
    public class AssetStatusConstant
    {
        public const string InService = "InService";
        public const string OutforRepair = "OutforRepair";
        public const string Scrapped = "Scrapped";
        public const string SeasonalDown = "SeasonalDown";
        public const string ScheduledDown = "ScheduledDown";
        #region V2-1133       
        public const string NotinUse = "NotinUse";//Now NotinUse will be used instead of ProductNotinUse
        public const string UnScheduledDown = "UnScheduledDown";
        #endregion
        public const string Assigned = "Assigned";
        public const string Unassigned = "Unassigned";
    }
}
