using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Notification
{
    public class ApplicationCommon
    {
        #region Constant
        public const string SyncronizeCatalog = "Sync_Catalog";
        public const string SyncronizeToken = "Sync_Token";
        #endregion

        #region Private Use
        public static long GetNewID()
        {
            long _NewID = 0;
            _NewID = Convert.ToInt64(DateTime.Now.Year) * 100000000000L;
            _NewID += Convert.ToInt64(DateTime.Now.Month) * 1000000000;
            _NewID += Convert.ToInt64(DateTime.Now.Day) * 10000000;
            _NewID += Convert.ToInt64(DateTime.Now.Hour) * 100000;
            _NewID += Convert.ToInt64(DateTime.Now.Minute) * 1000;
            _NewID += Convert.ToInt64(DateTime.Now.Second) * 10;
            _NewID += (new System.Random()).Next(9);
            return _NewID;
        }

        public static long ConvertDateTimeToLong(System.DateTime _DateToConvert)
        {
            long _LongDate = 0;
            _LongDate = _DateToConvert.Year * 10000000000L;
            _LongDate += _DateToConvert.Month * 100000000;
            _LongDate += _DateToConvert.Day * 1000000;
            _LongDate += _DateToConvert.Hour * 10000;
            _LongDate += _DateToConvert.Minute * 100;
            _LongDate += _DateToConvert.Second;
            return _LongDate;
        }

        public static long ConvertDateToLong(System.DateTime _DateToConvert)
        {
            long _LongDate = 0;
            _LongDate = _DateToConvert.Year * 10000;
            _LongDate += _DateToConvert.Month * 100;
            _LongDate += _DateToConvert.Day;
            return _LongDate;
        }

        public static String GetPhysicalPath()
        {
            return (HttpContext.Current.Server.MapPath("/") + "\\Certificates\\");
        }
        public static void WriteToLog(string error, string fileName)
        {
            try
            {
                string path = string.Empty;

                path = "DescribeLog.txt";

                if (File.Exists(HttpContext.Current.Server.MapPath(path + fileName)))
                {

                    string cntnt = string.Empty;
                    using (StreamReader rd = new StreamReader((HttpContext.Current.Server.MapPath("~/DescribeLog.txt"))))
                    {
                        cntnt = rd.ReadToEnd().ToString();
                        rd.Dispose();
                    }
                    using (StreamWriter sw = new StreamWriter((HttpContext.Current.Server.MapPath("~/DescribeLog.txt"))))
                    {
                        sw.WriteLine(cntnt);
                        sw.WriteLine("==============Log Starts=================");
                        sw.WriteLine(error);
                        sw.WriteLine("================Log Ends===============");
                        sw.Dispose();
                    }
                }

            }
            catch
            {


            }

        }

        public static string GetCertificateFilePath(String _VersionType, String DeviceType)
        {
            try
            {
                string path = string.Empty;
                if (DeviceType == "2") // For iPhone
                {

                    if (Convert.ToInt16(_VersionType) == 0)
                    {
                        path = HttpContext.Current.Server.MapPath("~/Certificates/IOS/SomaxinHouseAPNS.p12"); //System.Configuration.ConfigurationSettings.AppSettings["FreeCertificatePath"].ToString() ;


                    }
                    else
                    {
                        path = HttpContext.Current.Server.MapPath("~/Certificates/IOS/SomaxinHouseAPNS.p12"); //System.Configuration.ConfigurationSettings.AppSettings["PaidCertificatePath"].ToString(); 
                    }
                }
                return path;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string GetCertificateFilePath2(String _VersionType, String DeviceType)
        {
            try
            {
                string path = string.Empty;
                if (DeviceType == "2") // For iPhone
                {

                    if (Convert.ToInt16(_VersionType) == 0)
                    {
                        path = HttpContext.Current.Server.MapPath("~/Certificates/IOS/Certificates_pushdevelopment.p12"); //System.Configuration.ConfigurationSettings.AppSettings["FreeCertificatePath"].ToString() ;


                    }
                    else
                    {
                        path = HttpContext.Current.Server.MapPath("~/Certificates/IOS/Certificates_pushdevelopment.p12"); //System.Configuration.ConfigurationSettings.AppSettings["PaidCertificatePath"].ToString(); 
                    }
                }
                return path;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string GetCertificateFilePath3(String _VersionType, String DeviceType)
        {
            try
            {
                string path = string.Empty;
                if (DeviceType == "2") // For iPhone
                {

                    if (Convert.ToInt16(_VersionType) == 0)
                    {
                        path = HttpContext.Current.Server.MapPath("~/Certificates/IOS/Certificates_push.p12"); //System.Configuration.ConfigurationSettings.AppSettings["FreeCertificatePath"].ToString() ;


                    }
                    else
                    {
                        path = HttpContext.Current.Server.MapPath("~/Certificates/IOS/Certificates_push.p12"); //System.Configuration.ConfigurationSettings.AppSettings["PaidCertificatePath"].ToString(); 
                    }
                }
                return path;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string GetCertificateFilePath4(String _VersionType, String DeviceType)
        {
            try
            {
                string path = string.Empty;
                if (DeviceType == "2") // For iPhone
                {

                    if (Convert.ToInt16(_VersionType) == 0)
                    {
                        path = HttpContext.Current.Server.MapPath("~/Certificates/IOS/APNS.p12"); //System.Configuration.ConfigurationSettings.AppSettings["FreeCertificatePath"].ToString() ;


                    }
                    else
                    {
                        path = HttpContext.Current.Server.MapPath("~/Certificates/IOS/APNS.p12"); //System.Configuration.ConfigurationSettings.AppSettings["PaidCertificatePath"].ToString(); 
                    }
                }
                return path;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


        public static bool BroadCastMessagesJson(string _MessageID, String _DeviceToken, String _VersionType, String _DeviceType)
        {
            clsPushBroadcaster _MessageBroadCaster = new clsPushBroadcaster();
            bool S = _MessageBroadCaster.BroadCastMessageToAllJson(_MessageID, _DeviceToken, _VersionType, _DeviceType);
            return S;
        }
        public static bool BroadCastMessagesJson2(string _MessageID, String _DeviceToken, String _VersionType, String _DeviceType)
        {
            clsPushBroadcaster _MessageBroadCaster = new clsPushBroadcaster();
            bool S = _MessageBroadCaster.BroadCastMessageToAllJson2(_MessageID, _DeviceToken, _VersionType, _DeviceType);
            return S;
        }
        public static bool BroadCastMessagesJson3(string _MessageID, String _DeviceToken, String _VersionType, String _DeviceType)
        {
            clsPushBroadcaster _MessageBroadCaster = new clsPushBroadcaster();
            bool S = _MessageBroadCaster.BroadCastMessageToAllJson3(_MessageID, _DeviceToken, _VersionType, _DeviceType);
            return S;
        }
        public static bool BroadCastMessagesJson4(string _MessageID, String _DeviceToken, String _VersionType, String _DeviceType)
        {
            clsPushBroadcaster _MessageBroadCaster = new clsPushBroadcaster();
            bool S = _MessageBroadCaster.BroadCastMessageToAllJson4(_MessageID, _DeviceToken, _VersionType, _DeviceType);
            return S;
        }
        #endregion

        #region Public Use for WebPages
        public bool SendIOSNotification(String Message, String sDeviceToken, String sVersionType, String sDeviceType)
        {
            bool R = false;
            try
            {
                R = ApplicationCommon.BroadCastMessagesJson(Message, sDeviceToken, sVersionType, sDeviceType);
            }
            catch (Exception e)
            {

            }
            return R;
        }
        public bool SendIOSNotification2(String Message, String sDeviceToken, String sVersionType, String sDeviceType)
        {
            bool R = false;
            try
            {
                R = ApplicationCommon.BroadCastMessagesJson2(Message, sDeviceToken, sVersionType, sDeviceType);
            }
            catch (Exception e)
            {

            }
            return R;
        }
        public bool SendIOSNotification3(String Message, String sDeviceToken, String sVersionType, String sDeviceType)
        {
            bool R = false;
            try
            {
                R = ApplicationCommon.BroadCastMessagesJson3(Message, sDeviceToken, sVersionType, sDeviceType);
            }
            catch (Exception e)
            {

            }
            return R;
        }
        public bool SendIOSNotification4(String Message, String sDeviceToken, String sVersionType, String sDeviceType)
        {
            bool R = false;
            try
            {
                R = ApplicationCommon.BroadCastMessagesJson4(Message, sDeviceToken, sVersionType, sDeviceType);
            }
            catch (Exception e)
            {

            }
            return R;
        }
        public string SendAndriodNotificationforFCM(string Message, string RegistrationID, string ApiKey, string SenderId)
        {
            //-- Create Query String --//

            //---JSon Message----------//

            string JMessage = Message;
            //---------end of JSon Message--------------
            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.Message=" + HttpUtility.UrlEncode(JMessage) + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + RegistrationID + "";

            // string postData = string.Format("registration_id={0}&data.payload={1}&collapse_key={2}", RegistrationID, HttpUtility.UrlEncode(Message), HttpUtility.UrlEncode("score_update"));

            //string postData = string.Format("registration_id={0}&data.payload={1}&collapse_key={2}", RegistrationID, HttpUtility.UrlEncode(Message), HttpUtility.UrlEncode("score_update&time_to_live=108&delay_while_idle=1"));
            //Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //-- Create GCM Request Object --//
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            Request.Headers.Add("Authorization", "key=" + ApiKey);
            //Request.Headers.Add(string.Format("Authorization: key={0}", ApiKey));            
            Request.Headers.Add(string.Format("Sender: id={0}", SenderId));
            Request.ContentLength = byteArray.Length;

            //-- Delegate Modeling to Validate Server Certificate --//
            ServicePointManager.ServerCertificateValidationCallback += delegate (
                        object
                        sender,
                        System.Security.Cryptography.X509Certificates.X509Certificate
                        pCertificate,
                        System.Security.Cryptography.X509Certificates.X509Chain pChain,
                        System.Net.Security.SslPolicyErrors pSSLPolicyErrors)
            {
                return true;
            };

            //-- Create Stream to Write Byte Array --// 
            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //-- Post a Message --//
            WebResponse Response = Request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                return "Unauthorized - need new token";
            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                return "Response from web service isn't OK";
                //Console.WriteLine("Response from web service is not OK :");
                //Console.WriteLine(((HttpWebResponse)Response).StatusDescription);
            }

            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string responseLine = Reader.ReadLine();
            Reader.Close();

            return responseLine;
        }

        public string SendAndriodNotification(string Message, string RegistrationID, string ApiKey, string SenderId)
        {
            //-- Create Query String --//

            //---JSon Message----------//

            string JMessage = Message;
            //---------end of JSon Message--------------
            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.Message=" + HttpUtility.UrlEncode(JMessage) + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + RegistrationID + "";

            // string postData = string.Format("registration_id={0}&data.payload={1}&collapse_key={2}", RegistrationID, HttpUtility.UrlEncode(Message), HttpUtility.UrlEncode("score_update"));

            //string postData = string.Format("registration_id={0}&data.payload={1}&collapse_key={2}", RegistrationID, HttpUtility.UrlEncode(Message), HttpUtility.UrlEncode("score_update&time_to_live=108&delay_while_idle=1"));
            //Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //-- Create GCM Request Object --//
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            Request.Headers.Add("Authorization", "key=" + ApiKey);
            //Request.Headers.Add(string.Format("Authorization: key={0}", ApiKey));            
            Request.Headers.Add(string.Format("Sender: id={0}", SenderId));
            Request.ContentLength = byteArray.Length;

            //-- Delegate Modeling to Validate Server Certificate --//
            ServicePointManager.ServerCertificateValidationCallback += delegate (
                        object
                        sender,
                        System.Security.Cryptography.X509Certificates.X509Certificate
                        pCertificate,
                        System.Security.Cryptography.X509Certificates.X509Chain pChain,
                        System.Net.Security.SslPolicyErrors pSSLPolicyErrors)
            {
                return true;
            };

            //-- Create Stream to Write Byte Array --// 
            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //-- Post a Message --//
            WebResponse Response = Request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                return "Unauthorized - need new token";
            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                return "Response from web service isn't OK";
                //Console.WriteLine("Response from web service is not OK :");
                //Console.WriteLine(((HttpWebResponse)Response).StatusDescription);
            }

            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string responseLine = Reader.ReadLine();
            Reader.Close();

            return responseLine;
        }
        #endregion
    }

}
