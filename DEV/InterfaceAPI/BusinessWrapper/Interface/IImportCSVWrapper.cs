using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceAPI.BusinessWrapper.Interface
{
    public interface IImportCSVWrapper : ICommonWrapper
    {
        /// <summary>
        /// Read csv files from FTP 
        /// </summary>
        /// <returns>true = if enable. false = if not enable.</returns>
        string GetCsvFromFTPFile(string FTPAddress, string ftp_user, string ftp_pass, string fileName);

    }
}
