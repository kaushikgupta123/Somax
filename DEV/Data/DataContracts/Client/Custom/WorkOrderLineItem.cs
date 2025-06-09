using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class WorkOrderLineItem : DataContractBase, IStoredProcedureValidation
    {
        public string PartClientLookupId { get; set; }
        public decimal TotalCost { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string AccountClientLookupId { get; set; }
        public string ErrorMessageRow { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityToDate { get; set; }
        public decimal CurrentAverageCost { get; set; }
        public decimal CurrentAppliedCost { get; set; }
        public decimal CurrentOnHandQuantity { get; set; }
        public string StockType { get; set; }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            throw new NotImplementedException();
        }
    }
}
