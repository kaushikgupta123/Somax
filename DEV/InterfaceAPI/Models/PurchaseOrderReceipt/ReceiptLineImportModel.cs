namespace InterfaceAPI.Models
{
/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2020 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person       Description
* =========== ========= ============ =============================================================
* 2020-Oct-15 V2-412    Roger Lawton Added Receipt Status
**************************************************************************************************
*/
  public class ReceiptLineImportModel
    {
        public int ExPOLineId { get; set; }
        public int ExReceiptTxnId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Status { get; set; }
    }
}