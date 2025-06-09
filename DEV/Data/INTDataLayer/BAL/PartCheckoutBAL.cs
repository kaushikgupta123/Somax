/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person            Description
* =========== ========= ================= =======================================================
* 2015-Apr-03 SOM-626   Roger Lawton      Added SiteId as a parameter to GetPartRetrieveByPKOrUpc
*                                         Removed UPCCode Parameter from GetPartRetrieveByPKOrUpc
*                                         Added new method to retrieve storeroom from checkout
*                                         Changed the stored procedure used by 
*                                         RetrieveChargeTypeIdByClientLookupId - old one could 
*                                         return multiple values
**************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOMAX.G4.Data.INTDataLayer.DAL;
using SOMAX.G4.Data.INTDataLayer.EL;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;


namespace SOMAX.G4.Data.INTDataLayer.BAL {
    public class PartCheckoutBAL {
        /*  Developed by INT .New Add -1
         *  Return a single row depending on  siteid  from Storeroom table 
         * @objUserEL  List variable type UserEL 
         * @ClientLookupId bigint client lookupid of the charge type  table depending on charge type name
         * @strSite  string  siteId
         * @Description string  store description
         */
        public static DataSet StoreroomRetrieveLookupListBySearchCriteria(UserEL objUserEL, int strSite, string Name, string Description, string ordercolumn, string conString, string OrderDirection = "asc") {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Storeroom_RetrieveLookupListBySearchCriteria")) {

                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
                proc.AddNVarcharPara("@Name", 128, Name);
                proc.AddNVarcharPara("@Description", 128, Description);
                proc.AddBigIntegerPara("@SiteId", strSite);
                proc.AddNVarcharPara("@OrderColumn", 128, ordercolumn);
                proc.AddNVarcharPara("@OrderDirection", 128, OrderDirection);


                DataSet dsStore = new DataSet();

                dsStore = proc.GetDataSet(conString);
                return dsStore;
            }

        }

        // SOM-626 - Changed SP - Old one could return multiple values 
        public static DataSet RetrieveChargeTypeIdByClientLookupId(UserEL objUserEL, int strSite, string Type, string ClientLookupId, string conString) {
            using (ProcedureExecute proc = new ProcedureExecute("usp_ChargeType_RetrieveChargeToIdByClientLookupId")) {
                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 256, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@SiteId", strSite);
                proc.AddNVarcharPara("@Type", 15, Type);
                proc.AddNVarcharPara("@ClientLookupId", 31, ClientLookupId);

                DataSet dsStore = new DataSet();

                dsStore = proc.GetDataSet(conString);
                return dsStore;
            }
            /*
             using (ProcedureExecute proc = new ProcedureExecute("usp_ChargeType_RetrieveClientLookupIdBySearchCriteria"))
              {
                  proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                  proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                  proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
                  proc.AddBigIntegerPara("@ParentSiteId", strSite);
                  proc.AddNVarcharPara("@Type", 128, Type);
                  proc.AddNVarcharPara("@ClientLookupId", 128, ClientLookupId);

                  DataSet dsStore = new DataSet();

                  dsStore = proc.GetDataSet(conString);
                  return dsStore;
              }
            */
        }

        /*  Developed by INT .New Add -1
       *  Return a single row depending on  Client lookupid from Part table 
       * @objUserEL  List variable type UserEL 
       * @ClientLookupId bigint client lookupid of the part table
       */
        public static DataSet RetrievePartIdByClientLookupId(UserEL objUserEL, string ClientLookupId, string conString) {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Part_RetrieveClientLookupIdBySearchCriteria")) {

                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@SiteId", objUserEL.SiteId);
                proc.AddNVarcharPara("@ClientLookupId", 128, ClientLookupId);




                DataSet dsStore = new DataSet();

                dsStore = proc.GetDataSet(conString);
                return dsStore;
            }

        }

        /*  Developed by INT .New Add -1
         *  Return a single row depending on  Client lookupid or UPCCode from Part table 
         * @objUserEL  List variable type UserEL 
         * @ClientLookupId bigint client lookupid of the part table
         * @UPCCode  string UPCCode of the parttable*/
        public static DataSet GetPartRetrieveByPKOrUpc(UserEL objUserEL, string ClientLookupId, string conString)
            //public static DataSet GetPartRetrieveByPKOrUpc(UserEL objUserEL, string ClientLookupId, string UPCCode, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Part_RetrieveByPKOrUpc")) {
                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@SiteId", objUserEL.SiteId);
                proc.AddNVarcharPara("@ClientLookupId", 31, ClientLookupId);

                DataSet dsStore = new DataSet();

                dsStore = proc.GetDataSet(conString);
                return dsStore;
            }
        }
        // SOM-626 - New Method to get PartStoreroom based on client + PartId
        // Currently retriving one records
        // If we go to multiple storerooms - we may have multiple records
        public static DataSet GetStoreroomByPartId(UserEL objUserEL, long partid, string conString) {
            using (ProcedureExecute proc = new ProcedureExecute("usp_PartStoreroom_RetrieveByPartId")) {
                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@PartId", partid);

                DataSet dsStore = new DataSet();

                dsStore = proc.GetDataSet(conString);
                return dsStore;
            }
        }

        public static int UpdateEquipment_Parts_Xref_Crosscheck(UserEL objUserEL, string EquipmentId, string PartId, string Comment, decimal QuantityNeeded,
               decimal QuantityUsed, Int64 SiteId, string conString) {
            int xID = 0;

            int outp = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_Equipment_Parts_Xref_Crosscheck");
            proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
            proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddNVarcharPara("@strEquipmentId", 300, EquipmentId);
            proc.AddNVarcharPara("@PartClientLookupId", 128, PartId);
            proc.AddNVarcharPara("@Comment", 128, Comment);
            proc.AddDecimalPara("@QuantityNeeded", 6, 15, QuantityNeeded);
            proc.AddDecimalPara("@QuantityUsed", 6, 15, QuantityUsed);
            proc.AddIntegerPara("@Equipment_Parts_XrefId", 0, QyParameterDirection.Output);

            xID = proc.RunActionQuery(conString);
            xID = Convert.ToInt32(proc.GetParaValue("@Equipment_Parts_XrefId"));
            return xID;
        }
        /*  Developed by INT .New Add -1
         * Save  Data to partHistory Table And also  Parts_Xref_Crosscheck done
         * @objUserEL  List variable type UserEL
         * @SiteId type Long 
         * @ChargeType_Primary type string  ChargeType Name 
         * @ChargeToId_Primary    type longint Charge type Id
         * @TransactionQuantity type Decimal   Quantityamount
         * @PerformAdjustment type bool if yes then cross reference checking*/

        public static int AddPartHistory_PartIssue(UserEL objUserEL, Int64 SiteId, Int64 PartId, string ChargeType_Primary, Int64 ChargeToId_Primary,
               decimal TransactionQuantity, bool PerformAdjustment, string conString) {
            int xID = 0;


            ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_PartIssue");
            proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
            proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddBigIntegerPara("@PartId", Convert.ToInt64(PartId));
            proc.AddNVarcharPara("@ChargeType_Primary", 15, ChargeType_Primary);

            proc.AddBigIntegerPara("@ChargeToId_Primary", ChargeToId_Primary);
            proc.AddDecimalPara("@TransactionQuantity", 6, 15, TransactionQuantity);
            proc.AddBooleanPara("@PerformAdjustment", PerformAdjustment);
            proc.AddIntegerPara("@PartHistoryId", 0, QyParameterDirection.Output);
            proc.AddBigIntegerPara("@PerformedById", objUserEL.PerformedById);
            proc.AddBigIntegerPara("@RequestorId", objUserEL.RequestorId);
            int result = proc.RunActionQuery(conString);
            xID = Convert.ToInt32(proc.GetParaValue("@PartHistoryId"));
            return xID;
        }

        public static int AddWOCompleteWorkBenchPartHistory_PartIssue
            (UserEL objUserEL, Int64 SiteId, Int64 PartId, string ChargeType_Primary, Int64 ChargeToId_Primary,
              decimal TransactionQuantity, bool PerformAdjustment, string flag, string conString) {
            int xID = 0;


            ProcedureExecute proc = new ProcedureExecute("usp_WOCompletion_PartHistory_PartIssue");
            proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
            proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddBigIntegerPara("@PartId", Convert.ToInt64(PartId));
            proc.AddNVarcharPara("@ChargeType_Primary", 15, ChargeType_Primary);
            proc.AddBigIntegerPara("@ChargeToId_Primary", ChargeToId_Primary);
            proc.AddDecimalPara("@TransactionQuantity", 6, 15, TransactionQuantity);
            proc.AddBooleanPara("@PerformAdjustment", PerformAdjustment);
            proc.AddNVarcharPara("@Flag", 100, flag);
            proc.AddIntegerPara("@PartHistoryId", 0, QyParameterDirection.Output);

            int result = proc.RunActionQuery(conString);
            xID = Convert.ToInt32(proc.GetParaValue("@PartHistoryId"));
            return xID;
        }

    }
}
