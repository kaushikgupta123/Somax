
using Admin.Models;
using Common.Constants;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using Utility;

namespace Admin.Common
{
    public static class UtilityFunction
    {
        private static object locker = new Object();
        private static string EncKeyVal = ConfigurationManager.AppSettings["encKeyV"].ToString();
        internal static void LogException(string FileWithPath, Exception ex, UserData user, bool mailSendingStatus = false)
        {
            lock (locker)
            {

                using (StreamWriter sW = new StreamWriter(HttpContext.Current.Server.MapPath(FileWithPath), true))
                {
                    string timestampString = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + " at " + DateTime.Now.TimeOfDay.ToString();
                    sW.WriteLine("Timestamp: " + timestampString);
                    sW.WriteLine("User: " + user.DatabaseKey.UserName ?? "Not Logged in.");
                    sW.WriteLine();
                    sW.WriteLine("Message:");
                    sW.WriteLine(ex.Message);
                    sW.WriteLine("StactTrace:");
                    sW.WriteLine(ex.StackTrace + Environment.NewLine);
                    if (ex.InnerException != null)
                    {
                        sW.WriteLine("Inner Exception Message:");
                        sW.WriteLine(ex.InnerException.Message + Environment.NewLine);
                    }

                    sW.WriteLine("####################");
                }
            }
        }

        public static string GetMessageFromResource(string resourceId, string localizeResourceSetConstants)
        {
            var SessionData = HttpContext.Current.Session["AdminUserData"];
            if (SessionData != null)
            {
                var data = HttpContext.Current.Session["AdminUserData"];
                var userData = (UserData)data;
                LoginCacheSet _logCache = new LoginCacheSet();
                List<Localizations> locGlobalCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), localizeResourceSetConstants, userData.Site.Localization);

                var result = locGlobalCache.Where(x => x.ResourceId.Equals(resourceId)).Select(x => x.Value).FirstOrDefault();
                return result;
            }
            return string.Empty;
        }


        internal static List<DropDownModel> InactiveActiveStatusTypes()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("globalActive",LocalizeResourceSetConstants.Global),value="1"},//text="Active"
                new DropDownModel{ text=GetMessageFromResource("globalInActive",LocalizeResourceSetConstants.Global),value="2"}//text="Inactive"
             };
            return ddList;
        }

     

        #region System Unavailable Message
        public static string GetAMPMWithSpace(string Time)
        {
            string ModifiedTime = string.Empty;
            if (Time.Contains("AM") || Time.Contains("am"))
            {
                ModifiedTime = Time.ToUpper().Replace("AM", " AM");
            }
            else if (Time.Contains("PM") || Time.Contains("pm"))
            {
                ModifiedTime = Time.ToUpper().Replace("PM", " PM");
            }
            else
            {
                ModifiedTime = Time.ToUpper();
            }
            return ModifiedTime;
        }
        #endregion

        #region Dashboard Grid
        internal static List<DropDownModel> DashboardGridDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text=GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="7"},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="30"},
                new DropDownModel{ text=GetMessageFromResource("spnLastNinetyDays",LocalizeResourceSetConstants.Global),value="90"}

             };
            return ddList;
        }
        #endregion

        //#region V2-806
        //public static string GetModifiedContactChipContent(List<Models.Common.UserMentionDataModel.UserMentionData> mentionDatas, string Content)
        //{
        //    //01/07/2020
        //    string myorigialContent = Content;
        //    //01/07/2020
        //    string FinalContent = string.Empty;
        //    string ModifiedComments = string.Empty;
        //    List<Models.Common.UserMentionDataModel.UserMentionData> tempUserLists = new List<Models.Common.UserMentionDataModel.UserMentionData>();
        //    //02/07/2020
        //    var UserList = mentionDatas;
        //    foreach (var myitem in UserList)
        //    {
        //        if (Content.Contains(myitem.name))
        //        {
        //            var nm = "@" + myitem.name;
        //            var matches = Regex.Matches(Content, nm, RegexOptions.IgnoreCase);
        //            if (matches.Count > 1)
        //            {
        //                for (int i = 0; i < matches.Count; i++)
        //                {
        //                    tempUserLists.Add(myitem);
        //                }
        //            }
        //            else
        //            {
        //                tempUserLists.Add(myitem);
        //            }

        //        }
        //    }
        //    int k = Content.Split('@').Length - 1;
        //    //02/07/2020
        //    if (Content.Contains("@"))
        //    {
        //        string cmmtuserss = string.Empty;
        //        string cmtComments = string.Empty;
        //        //24/06/2020
        //        for (int myindex = 0; myindex < k; myindex++)
        //        {

        //            int attheRateInmdex = UtilityFunction.IndexOfNth(Content, "@", myindex);
        //            cmtComments = Content;
        //            bool ismatched = false;
        //            //02/07/2020
        //            //foreach (var myitem in UserList)
        //            foreach (var myitem in tempUserLists)
        //            //02/07/2020
        //            {

        //                if (Content.Contains(myitem.name))
        //                {
        //                    int mypos = Content.IndexOf(myitem.name);

        //                    int i = 0;
        //                    List<int> indexarr = new List<int>();
        //                    while ((i = Content.IndexOf(myitem.name, i)) != -1)
        //                    {
        //                        // Print out the index.
        //                        indexarr.Add(i);
        //                        //Console.WriteLine(i);
        //                        i++;
        //                    }

        //                    //if (mypos == attheRateInmdex + 1)
        //                    if (indexarr.Contains(attheRateInmdex + 1))
        //                    {
        //                        cmmtuserss = myitem.name;
        //                        string[] mycommentUsers1 = cmmtuserss.Split(' ');
        //                        string FName = mycommentUsers1[0].Substring(0, 1);
        //                        string LName = !String.IsNullOrEmpty(mycommentUsers1[1]) ? mycommentUsers1[1].Substring(0, 1) : "";
        //                        //string FullName = FName + ' ' + LName;
        //                        string FullName = ' ' + FName + LName;
        //                        int commentatthjeRateIndex = UtilityFunction.IndexOfNth(cmtComments, "@", myindex);
        //                        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        //                        sb1.Append("<span class='chip'>");
        //                        //sb1.Append("<img src='/Content/Images/img_avatar.png' alt='Person' width='96' height='96'>");
        //                        //sb1.Append("<span style='color:#F15009;float:left;background-color:#4685a3;margin:0 10px 0 -25px;height: 50px;width:50px; border-radius:50%;padding:0px 8px 0px 8px;'>" + FullName + "</span>");
        //                        sb1.Append("<span class='chiplikeimage'>" + FullName.ToUpper() + "</span>");
        //                        sb1.Append("@~" + cmmtuserss);
        //                        sb1.Append("<span></span>");
        //                        sb1.Append("</span>");
        //                        cmtComments = cmtComments.Remove(commentatthjeRateIndex, myitem.name.Length + 1).Insert(commentatthjeRateIndex, sb1.ToString());
        //                        Content = cmtComments;
        //                        ModifiedComments = cmtComments;

        //                    }
        //                    ModifiedComments = ModifiedComments.Replace("@~", "");
        //                    ismatched = true;
        //                }

        //            }
        //            if (ismatched)
        //            {
        //                ModifiedComments = ModifiedComments.Replace("~", "");
        //            }
        //            else
        //            {
        //                ModifiedComments = myorigialContent;
        //            }
        //        }

        //    }
        //    else
        //    {
        //        ModifiedComments = Content;
        //    }

        //    FinalContent = ModifiedComments;
        //    return FinalContent;
        //}
        //public static int IndexOfNth(this string str, string value, int nth = 0)
        //{
        //    if (nth < 0)
        //        throw new ArgumentException("Can not find a negative index of substring in string. Must start with 0");

        //    int offset = str.IndexOf(value);
        //    for (int i = 0; i < nth; i++)
        //    {
        //        if (offset == -1) return -1;
        //        offset = str.IndexOf(value, offset + 1);
        //    }

        //    return offset;
        //}
        //#endregion

        #region V2-964
        internal static List<DropDownModel> AllClientInactiveActiveStatusTypes()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("globalAllClients",LocalizeResourceSetConstants.Global),value="0"},//text="All Clients"
                new DropDownModel{ text= GetMessageFromResource("globalActive",LocalizeResourceSetConstants.Global),value="1"},//text="Active"
                new DropDownModel{ text=GetMessageFromResource("globalInActive",LocalizeResourceSetConstants.Global),value="2"}//text="Inactive"
             };
            return ddList;
        }
        internal static List<DropDownModel> LocalizationsTypes()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= "English",value="en-us"},
                new DropDownModel{ text= "French",value="fr-fr"}
             };
            return ddList;
        }
        #endregion
    }
}