using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Notification
{
    public class clsPushBroadcaster
    {
        #region Private Use
        // Get the APNS cert

        private SslStream sslStream;
        private X509Certificate2 getServerCert(String _VersionType, String _DeviceType)
        {
            X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(ApplicationCommon.GetCertificateFilePath(_VersionType, _DeviceType)), "admin", X509KeyStorageFlags.MachineKeySet);
            return cert;
        }
        private X509Certificate2 getServerCert2(String _VersionType, String _DeviceType)
        {
            X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(ApplicationCommon.GetCertificateFilePath2(_VersionType, _DeviceType)), "admin", X509KeyStorageFlags.MachineKeySet);
            return cert;
        }
        private X509Certificate2 getServerCert3(String _VersionType, String _DeviceType)
        {
            X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(ApplicationCommon.GetCertificateFilePath3(_VersionType, _DeviceType)), "admin", X509KeyStorageFlags.MachineKeySet);
            return cert;
        }
        private X509Certificate2 getServerCert4(String _VersionType, String _DeviceType)
        {
            X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(ApplicationCommon.GetCertificateFilePath4(_VersionType, _DeviceType)), "admin", X509KeyStorageFlags.MachineKeySet);
            return cert;
        }


        // Make the connection to the APNS server
        private bool ConnectToAPNS(String _VersionType, String _DeviceType)
        {

            X509Certificate2 _PushCertificate = null;
            X509Certificate2Collection certs = new X509Certificate2Collection();

            // Add the Apple cert to our collection
            _PushCertificate = getServerCert(_VersionType, _DeviceType);

            //ApplicationCommon.WriteToLog(_PushCertificate.ToString());
            if (_PushCertificate == null)
                return false;

            certs.Add(_PushCertificate);

            // Apple development server address
            string apsHost = null;

            if (_PushCertificate.ToString().ToLower().Contains("somax"))
            {
                apsHost = "gateway.push.apple.com";
            }
            else
            {
                apsHost = "gateway.sandbox.push.apple.com";
            }

            try
            {
                // Create a TCP socket connection to the Apple server on port 2195
                TcpClient tcpClient = new TcpClient(apsHost, 2195);
                // Create a new SSL stream over the connection
                sslStream = new SslStream(tcpClient.GetStream());
                sslStream.AuthenticateAsClient(apsHost, certs, SslProtocols.Default, false);
                return true;
            }
            catch (Exception e)
            {
                //Modify on 01-Sep-2012
                ApplicationCommon.WriteToLog("TcpClient tcpClient = new TcpClient(apsHost, 2195) Error", "");
                ApplicationCommon.WriteToLog(e.ToString(), "");
            }
            return false;
        }
        private bool ConnectToAPNS2(String _VersionType, String _DeviceType)
        {

            X509Certificate2 _PushCertificate = null;
            X509Certificate2Collection certs = new X509Certificate2Collection();

            // Add the Apple cert to our collection
            _PushCertificate = getServerCert2(_VersionType, _DeviceType);

            //ApplicationCommon.WriteToLog(_PushCertificate.ToString());
            if (_PushCertificate == null)
                return false;

            certs.Add(_PushCertificate);

            // Apple development server address
            string apsHost = null;

            if (_PushCertificate.ToString().ToLower().Contains("somax"))
            {
                apsHost = "gateway.sandbox.push.apple.com";
            }
            else
            {
                apsHost = "gateway.sandbox.push.apple.com";
            }

            try
            {
                // Create a TCP socket connection to the Apple server on port 2195
                TcpClient tcpClient = new TcpClient(apsHost, 2195);
                // Create a new SSL stream over the connection
                sslStream = new SslStream(tcpClient.GetStream());
                sslStream.AuthenticateAsClient(apsHost, certs, SslProtocols.Default, false);
                return true;
            }
            catch (Exception e)
            {
                //Modify on 01-Sep-2012
                ApplicationCommon.WriteToLog("TcpClient tcpClient = new TcpClient(apsHost, 2195) Error", "");
                ApplicationCommon.WriteToLog(e.ToString(), "");
            }
            return false;
        }
        private bool ConnectToAPNS3(String _VersionType, String _DeviceType)
        {

            X509Certificate2 _PushCertificate = null;
            X509Certificate2Collection certs = new X509Certificate2Collection();

            // Add the Apple cert to our collection
            _PushCertificate = getServerCert3(_VersionType, _DeviceType);

            //ApplicationCommon.WriteToLog(_PushCertificate.ToString());
            if (_PushCertificate == null)
                return false;

            certs.Add(_PushCertificate);

            // Apple development server address
            string apsHost = null;

            if (_PushCertificate.ToString().ToLower().Contains("somax"))
            {
                apsHost = "gateway.push.apple.com";
            }
            else
            {
                apsHost = "gateway.sandbox.push.apple.com";
            }

            try
            {
                // Create a TCP socket connection to the Apple server on port 2195
                TcpClient tcpClient = new TcpClient(apsHost, 2195);
                // Create a new SSL stream over the connection
                sslStream = new SslStream(tcpClient.GetStream());
                sslStream.AuthenticateAsClient(apsHost, certs, SslProtocols.Default, false);
                return true;
            }
            catch (Exception e)
            {
                //Modify on 01-Sep-2012
                ApplicationCommon.WriteToLog("TcpClient tcpClient = new TcpClient(apsHost, 2195) Error", "");
                ApplicationCommon.WriteToLog(e.ToString(), "");
            }
            return false;
        }
        private bool ConnectToAPNS4(String _VersionType, String _DeviceType)
        {

            X509Certificate2 _PushCertificate = null;
            X509Certificate2Collection certs = new X509Certificate2Collection();

            // Add the Apple cert to our collection
            _PushCertificate = getServerCert4(_VersionType, _DeviceType);

            //ApplicationCommon.WriteToLog(_PushCertificate.ToString());
            if (_PushCertificate == null)
                return false;

            certs.Add(_PushCertificate);

            // Apple development server address
            string apsHost = null;

            if (_PushCertificate.ToString().ToLower().Contains("somax"))
            {
                apsHost = "gateway.push.apple.com";
            }
            else
            {
                apsHost = "gateway.sandbox.push.apple.com";
            }

            try
            {
                // Create a TCP socket connection to the Apple server on port 2195
                TcpClient tcpClient = new TcpClient(apsHost, 2195);
                // Create a new SSL stream over the connection
                sslStream = new SslStream(tcpClient.GetStream());
                sslStream.AuthenticateAsClient(apsHost, certs, SslProtocols.Default, false);
                return true;
            }
            catch (Exception e)
            {
                //Modify on 01-Sep-2012
                ApplicationCommon.WriteToLog("TcpClient tcpClient = new TcpClient(apsHost, 2195) Error", "");
                ApplicationCommon.WriteToLog(e.ToString(), "");
            }
            return false;
        }//--New p12 file(APNS.p12) for Production Server

        // Used to convert device token from string to byte[]
        private static byte[] HexToData(string hexString)
        {
            if (hexString == null)
            {
                return null;
            }

            if (hexString.Length % 2 == 1)
            {
                hexString = '0' + hexString;
            }
            // Up to you whether to pad the first or last byte
            byte[] data = new byte[hexString.Length / 2];

            for (int i = 0; i <= data.Length - 1; i++)
            {
                data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return data;
        }

        private bool PushMessageChatJson(string clientToken, string _Message)
        {
            //String cToken = "yourdevicetoken....";
            String cToken = clientToken;
            String cAlert = _Message;
            int iBadge = 1;

            try
            {
                // Ready to create the push notification
                byte[] buf = new byte[256];
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);

                bw.Write(new byte[] {
            0,
            0,
            32
            });

                byte[] deviceToken = HexToData(cToken);
                bw.Write(deviceToken);
                bw.Write(Convert.ToByte(0));
                // Create the APNS payload - new.caf is an audio file saved in the application bundle on the device
                string msg = "";
                msg = _Message;
                // Write the data out to the stream
                bw.Write(Convert.ToByte(msg.Length));
                bw.Write(msg.ToCharArray());
                bw.Flush();

                if (sslStream != null)
                {
                    sslStream.Write(ms.ToArray());
                    return true;
                }
            }
            catch (Exception e)
            {
                ApplicationCommon.WriteToLog("PUSH MESSAGE FAILED", "");
                ApplicationCommon.WriteToLog(e.ToString(), "");
            }
            return false;
        }

        public bool BroadCastMessageToAllJson(string _MessageText, String _DeviceToken, String _VersionType, String _DeviceType)
        {
            //This function will be called by thread and will send messages to all the devices which are enrolled
            bool status = false;
            string strClientToken = string.Empty;

            try
            {
                //Load Certificate
                //Connects to APNS            

                if (this.ConnectToAPNS(_VersionType, _DeviceType))
                {
                    strClientToken = _DeviceToken;
                    status = PushMessageChatJson(strClientToken, _MessageText);

                    //ApplicationCommon.WriteToLog("Connects to APNS END");
                    //return "success 8";
                }

                return status;
            }
            catch (Exception ex)
            {
                ApplicationCommon.WriteToLog("Connects to APNS FAILED", "");
                return false;
            }
            finally
            {
                if (sslStream != null)
                    sslStream.Dispose();
                sslStream = null;
            }
        }

        public bool BroadCastMessageToAllJson2(string _MessageText, String _DeviceToken, String _VersionType, String _DeviceType)
        {
            //This function will be called by thread and will send messages to all the devices which are enrolled
            bool status = false;
            string strClientToken = string.Empty;

            try
            {
                //Load Certificate
                //Connects to APNS            

                if (this.ConnectToAPNS2(_VersionType, _DeviceType))
                {
                    strClientToken = _DeviceToken;
                    status = PushMessageChatJson(strClientToken, _MessageText);

                    //ApplicationCommon.WriteToLog("Connects to APNS END");
                    //return "success 8";
                }

                return status;
            }
            catch (Exception ex)
            {
                ApplicationCommon.WriteToLog("Connects to APNS FAILED", "");
                return false;
            }
            finally
            {
                if (sslStream != null)
                    sslStream.Dispose();
                sslStream = null;
            }
        }
        public bool BroadCastMessageToAllJson3(string _MessageText, String _DeviceToken, String _VersionType, String _DeviceType)
        {
            //This function will be called by thread and will send messages to all the devices which are enrolled
            bool status = false;
            string strClientToken = string.Empty;

            try
            {
                //Load Certificate
                //Connects to APNS            

                if (this.ConnectToAPNS3(_VersionType, _DeviceType))
                {
                    strClientToken = _DeviceToken;
                    status = PushMessageChatJson(strClientToken, _MessageText);

                    //ApplicationCommon.WriteToLog("Connects to APNS END");
                    //return "success 8";
                }

                return status;
            }
            catch (Exception ex)
            {
                ApplicationCommon.WriteToLog("Connects to APNS FAILED", "");
                return false;
            }
            finally
            {
                if (sslStream != null)
                    sslStream.Dispose();
                sslStream = null;
            }
        }
        public bool BroadCastMessageToAllJson4(string _MessageText, String _DeviceToken, String _VersionType, String _DeviceType)
        {
            //This function will be called by thread and will send messages to all the devices which are enrolled
            bool status = false;
            string strClientToken = string.Empty;

            try
            {
                //Load Certificate
                //Connects to APNS            

                if (this.ConnectToAPNS4(_VersionType, _DeviceType))
                {
                    strClientToken = _DeviceToken;
                    status = PushMessageChatJson(strClientToken, _MessageText);

                    //ApplicationCommon.WriteToLog("Connects to APNS END");
                    //return "success 8";
                }

                return status;
            }
            catch (Exception ex)
            {
                ApplicationCommon.WriteToLog("Connects to APNS FAILED", "");
                return false;
            }
            finally
            {
                if (sslStream != null)
                    sslStream.Dispose();
                sslStream = null;
            }
        }

        #endregion
    }
}
