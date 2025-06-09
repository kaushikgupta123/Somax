using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceAPI.BusinessWrapper.Interface
{
    public interface IVendorMasterImportWrapper : ICommonWrapper
    {
        /// <summary>
        /// Insert or update data into receipt header table
        /// </summary>
        /// <param name="VendorImportModelObjectList">The respective json input.</param>
        /// <returns>true = if enable. false = if not enable.</returns>
        ProcessLogModel InsertVendarMasterImportData(List<VendorMasterImportModel> VendorImportModelObjectList);

        /// <summary>
        /// Validate and import vendor master data
        /// </summary>
        /// <returns>true = if enable. false = if not enable.</returns>
        //List<VendorMasterErrorModel> VendorMasterImportValidate(long logId, List<VendorMasterImportModel> VendorMasterImportModelObjectList);
        List<VendorMasterErrorModel> VendorMasterImportValidate(long logId);
  }
}