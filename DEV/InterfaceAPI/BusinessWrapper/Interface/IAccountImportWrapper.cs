using InterfaceAPI.Models.Account;
using InterfaceAPI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceAPI.BusinessWrapper.Interface
{
    public interface IAccountImportWrapper: ICommonWrapper
    {
        /// <summary>
        /// Insert or update data into import header & line tables
        /// </summary>
        /// <param name="AccountImportModelList">The respective json input.</param>
        /// <returns>true = if enable. false = if not enable.</returns>
        ProcessLogModel CreateAccountImport(List<AccountImportModel> AccountImportModelList);


        /// <summary>
        /// Check if input json parameter is valid
        /// </summary>
        /// <param name="AccountImportModelList">The respective json input.</param>
        /// <returns>true = if enable. false = if not enable.</returns>
        List<AccountErrorResponseModel> AccountImportValidate(long logId, List<AccountImportModel> AccountImportModelList);
    }
}
