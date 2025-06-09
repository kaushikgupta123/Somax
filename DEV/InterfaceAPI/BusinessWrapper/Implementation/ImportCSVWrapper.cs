using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataContracts;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
    public class ImportCSVWrapper : CommonWrapper, IImportCSVWrapper
    {
        public ImportCSVWrapper(UserData userData) : base(userData)
        {
        }
        private string fileString;

        public string GetCsvFromFTPFile(string FTPAddress, string ftp_user, string ftp_pass, string fileName)
        {
            string JString = string.Empty;
            try
            {
                ////---Get file from FTP

                FtpWebRequest ftpRequest2 = (FtpWebRequest)WebRequest.Create(FTPAddress + fileName);
                ftpRequest2.Credentials = new NetworkCredential(ftp_user, ftp_pass);
                ftpRequest2.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse response = (FtpWebResponse)ftpRequest2.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                //---Convert csv to DataTable
                StreamReader sr = new StreamReader(responseStream);
                string[] headers = sr.ReadLine().Split(',');
                DataTable dt = new DataTable();
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
                //----convert DataTable to Json
                JString = JsonConvert.SerializeObject(dt);
            }
            catch (Exception e)
            {
                if (e.Message == "Unable to connect to the remote server")
                {
                    JString = e.Message + ".";
                }
                else
                {
                    JString = "Source file not found.";
                }
            }
            return JString;
        }
        
    }
}