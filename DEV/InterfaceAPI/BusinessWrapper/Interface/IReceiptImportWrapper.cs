using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.PurchaseOrderReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceAPI.BusinessWrapper.Interface
{
    public interface IReceiptImportWrapper: ICommonWrapper
    {
        /// <summary>
        /// Check if input json parameter is valid
        /// </summary>
        /// <param name="ReceiptImportModelObjectList">The respective json input.</param>
        /// <returns>true = if enable. false = if not enable.</returns>
        List<ReceiptErrorResponseModel> ReceiptsImportValidate(long logId, List<ReceiptImportModel> ReceiptImportModelObjectList);

        /// <summary>
        /// Insert or update data into receipt header table
        /// </summary>
        /// <param name="ReceiptImportModelObjectList">The respective json input.</param>
        /// <returns>true = if enable. false = if not enable.</returns>
        ProcessLogModel InsertReceiptImpHdr(List<ReceiptImportModel> ReceiptImportModelObjectList);
    }
}
