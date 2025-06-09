using System;
using System.Collections.Generic;

namespace InterfaceAPI.Models
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
{
  public class ReceiptImportModel
    {
        public ReceiptImportModel()
        {
            ReceiptItems = new List<ReceiptLineImportModel>();
        }
        public int ClientId { get; set; }
        public string PONumber { get; set; }
        public int ExPOID { get; set; }
        public int ExReceiptId { get; set; }
        public string ExReceiptNo { get; set; }
        public DateTime Receiptdate { get; set; }
        // V2-412
        public string Status { get; set; }
        public List<ReceiptLineImportModel> ReceiptItems { get; set; }
    }
}