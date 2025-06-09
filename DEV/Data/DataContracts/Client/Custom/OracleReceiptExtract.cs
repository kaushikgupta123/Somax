/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2011 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2011-Dec-09 20110019 Roger Lawton        Added ClientLookupId to search results
* 2011-Dec-14 20110039 Roger Lawton        Added Lookuplist validation
* 2014-Aug-10 SOM-280  Roger Lawton        Modified UpdateFromDataObjectList to include 
*                                          LaborAccountClientLookupId
* 2015-Mar-03 SOM-590  Roger Lawton        Removed validation on columns we do not support
* 2015-Sep-14 SOM-805  Roger Lawton        Location - Show Location.ClientLookupId if FACILITIES
***************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Common.Structures;
using Common.Extensions;
using Database.Client.Custom.Business;

namespace DataContracts
{
    public partial class OracleReceiptExtract : DataContractBase
    {
    #region Properties
        public long SiteId { get; set; }
        public string SOMAXRequisitionNumber { get; set; }
        public Int64 SOMAXRequisitionID { get; set; }
        public string SOMAXRequisitionDescription { get; set; }
        public Int64 OracleVendorID { get; set; }
        public Int64 OracleVendorSiteId { get; set; }
        public string RequestedBy { get; set; }
        public Int64 SOMAXRequisitionLineItemId { get; set; }
        public int SOMAXRequisitionLineNumber { get; set; }
        public string OraclePlantId { get; set; }
        public DateTime NeedByDate { get; set; }
        public Int64 OraclePartID { get; set; }

        public string OraclePartNumber { get; set; }
        public long OracleSourceDocumentId { get; set; }
        public string OracleSourceDocumentNumber { get; set; }
        public long OracleSourceDocumentLineId { get; set; }

        public int SourceDocumentLineNumber { get; set; }
        public string UNSPSCCodeIDTree { get; set; }
        public string LineDescription { get; set; }
        public decimal Quantity { get; set; }
        public string UOMPurchasing { get; set; }
        public decimal UnitCost { get; set; }
        public string ExpenseAccount { get; set; }
    public long ClientId { get; set; }
        #endregion



  
        public List<OracleReceiptExtract> OraclePurchaseRequestExtract_ExtractData(DatabaseKey dbKey)
        {
            OraclePurchaseRequestExtract_ExtractData trans = new OraclePurchaseRequestExtract_ExtractData()          
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ClientId = this.ClientId;
           
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();                       
           
            List<OracleReceiptExtract> OracleReceiptExtractList = new List<OracleReceiptExtract>();
            foreach (b_OracleReceiptExtract oracleReceiptExtract in trans.OracleReceiptExtractList)
            {
                OracleReceiptExtract tmpOracleReceiptExtract = new OracleReceiptExtract();

                tmpOracleReceiptExtract.UpdateFromDatabaseObjectForRetriveByInactiveFlag(oracleReceiptExtract);
                OracleReceiptExtractList.Add(tmpOracleReceiptExtract);
            }
            return OracleReceiptExtractList;
        }


        public void UpdateFromDatabaseObjectForRetriveByInactiveFlag(b_OracleReceiptExtract dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.SOMAXRequisitionNumber = dbObj.ClientLookupId;
            this.SOMAXRequisitionID = dbObj.PurchaseRequestId;
            this.SOMAXRequisitionDescription = dbObj.Reason;
            this.OracleVendorID = dbObj.ExVendorId;
            this.OracleVendorSiteId = dbObj.ExVendorSiteId;
            this.RequestedBy = dbObj.ExOracleUserId;
            this.SOMAXRequisitionLineItemId = dbObj.PurchaseRequestLineItemId;
            this.SOMAXRequisitionLineNumber = dbObj.PurchaseRequestLineNumber;
            this.OraclePlantId = dbObj.ExOrganizationId;
            this.NeedByDate = dbObj.RequiredDate;
            this.OraclePartID = dbObj.ExPartId;
            this.OraclePartNumber = dbObj.PartMasterClientLookupId;
            this.OracleSourceDocumentId = dbObj.ExVendorCatalogSourceId;
            this.OracleSourceDocumentNumber = dbObj.ExSourceDocument;
            this.OracleSourceDocumentLineId = dbObj.ExVendorCatalogItemSourceId;
            this.SourceDocumentLineNumber = dbObj.VendorCatalogLineNummber;
            this.UNSPSCCodeIDTree = dbObj.Category;
            this.UOMPurchasing = dbObj.PurchaseUOM;
            this.LineDescription = dbObj.Description;
            this.UnitCost = dbObj.UnitCost;
            this.ExpenseAccount = dbObj.ExpenseAccount;
            this.Quantity = dbObj.OrderQuantity;
            
        }


        public b_OracleReceiptExtract ToDatabaseObject()
        {
            b_OracleReceiptExtract dbObj = new b_OracleReceiptExtract();
            dbObj.ClientId = this.ClientId;
     
            return dbObj;
        }      

    }
}
