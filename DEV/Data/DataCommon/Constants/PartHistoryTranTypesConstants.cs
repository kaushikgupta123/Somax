/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-24 SOM-626  Roger Lawton        Added StockOutAdjustment
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants
{
    public class PartHistoryTranTypes
    {
        public const string Adjustment = "Adjustment";
        public const string CostChange = "CostChange";
        public const string DirectInput = "DirectInput";
        public const string PartAdd = "PartAdd";
        public const string PartDelete = "PartDelete";
        public const string PartIssue = "PartIssue";
        public const string PurchaseIssue = "PurchaseIssue";
        public const string Receipt = "Receipt";
        public const string NonPOReceipt = "NonPOReceipt";
        public const string ReceiptReverse = "ReceiptReverse";
        public const string StockOut = "StockOut";
        public const string StockOutAdjustment = "StockOutAdjustment";
        // Not Currently Used
        public const string StoreroomAdd = "StoreroomAdd";
        public const string StoreroomDelete = "StoreroomDelete";
        public const string PartMasterAssign = "PartMasterAssign";
        public const string PartMasterUnAssign = "PartMasterUnAssign";
        public const string PartMasterReAssign = "PartMasterReAssign";
        public const string IDChange = "IDChange";
        public const string Inactivate = "Inactivate";
        public const string MasterReplace = "MasterReplace";
        public const string Activate = "Activate";
        public const string ECO_REPLACE  = "IDChange";
        public const string ECO_SX_REPLACE = "MasterReplace";
        public const string TransferIssue = "TransferIssue";
        public const string TransferReceipt = "TransferReceipt";
        public const string TransferAdjust = "TransferAdjust";
    }
}