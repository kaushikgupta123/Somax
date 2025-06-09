using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.PurchaseOrder;
using System.Collections.Generic;

namespace InterfaceAPI.BusinessWrapper.Interface
{
  public interface IPOImportWrapper : ICommonWrapper
  {

        /// <summary>
        /// Insert or update data into import header & line tables
        /// </summary>
        /// <param name="PurchaseOrderImportModelList">The respective json input.</param>
        /// <returns>true = if enable. false = if not enable.</returns>
        ProcessLogModel CreatePOImportHeader(List<PurchaseOrderImportModel> PurchaseOrderImportModelList);


        /// <summary>
        /// Check if input json parameter is valid
        /// </summary>
        /// <param name="PurchaseOrderImportModelList">The respective json input.</param>
        /// <returns>true = if enable. false = if not enable.</returns>
        List<ImportErrorResponseModel> POImportValidate(long logId, List<PurchaseOrderImportModel> PurchaseOrderImportModelList);
  }
}