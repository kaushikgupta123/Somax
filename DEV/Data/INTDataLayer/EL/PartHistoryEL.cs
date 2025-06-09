using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOMAX.G4.Data.INTDataLayer.EL
{
   public class PartHistoryEL
    {
       public Int64 ClientId
       { set; get; }
       public Int64 PartHistoryId
       { set; get; }
       public Int64 PartId
       { set; get; }

       public Int64  PartStoreroomId 
       { set; get; }

       public Int64 AccountId
       { set; get; }

       public decimal AverageCostBefore
       { set; get; }
       public decimal AverageCostAfter
       { set; get; }
       public string ChargeType_Primary
       { set; get; }

       public Int64 ChargeToId_Primary
       { set; get; }

       public string Comments
       { set; get; }

       public decimal Cost
       { set; get; }

       public decimal CostAfter
       { set; get; }
       public decimal CostBefore
       { set; get; }

       public string Description
       { set; get; }

       public Int64 DepartmentId
       { set; get; }

       public Int64 PerformedById
       { set; get; }

       public decimal QtyAfter
       { set; get; }
       public decimal QtyBefore
       { set; get; }

       public Int64 RequestorId
       { set; get; }

       public string StockType
       { set; get; }

       public Int64 StoreroomId
       { set; get; }

       public DateTime TransactionDate
       { set; get; }

       public decimal TransactionQuantity
       { set; get; }

       public string TransactionType
       { set; get; }

       public string UnitofMeasure
       { set; get; }

       public string CreatedBy
       { set; get; }

       public DateTime CreatedDate
       { set; get; }

       public string IssueTo
       { set; get; }

       public Int64 Quentity
       { set; get; }

       public string ErrorMessage
       { set; get; }
    }
}
