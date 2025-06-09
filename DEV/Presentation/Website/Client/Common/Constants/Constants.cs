namespace Client.Common
{
    public class SomaxAppConstants
    {
        public const string LOGINURL = "~/LogIn/SomaxLogIn";
        public const string SessionExpiaryUrl = "~/error/SessionExpired";
        public const string SomaxLogoForMailTemplate = "/images/logo.png";
        public const string HeaderMailTemplate = "/images/headerBg.jpg";
        public const string FooterMailTemplate = "/images/footerBg.jpg";
        public const string UserAddSubject = "New User Added";
        public const string ResetPasswordSubject = "Temporary Password Generated";
        public const string PunchOutSetUpReturn = "/PurchaseRequest/PRCheckOut";
        #region System Defaults
        internal const string PartCostCalc = "Average";
        #endregion

    }
}