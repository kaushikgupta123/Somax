using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INTDataLayer.DAL;
using INTDataLayer.EL;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;

namespace INTDataLayer.BAL
{
    public class ClientBAL
    {
        public static DataTable RetrieveClientbyCId(long CallerUserInfoId, long ClientId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Client_RetrieveByPK"))
            {

                proc.AddBigIntegerPara("@CallerUserInfoId", CallerUserInfoId);
                proc.AddBigIntegerPara("@clientId", ClientId);

                DataTable dsStore = new DataTable();

                dsStore = proc.GetTable(conString);

                
                return dsStore;
            }

        }

        /* ClientEL clientel = new ClientEL();
                if (dsStore.Rows.Count > 0)
                {
                    clientel.ClientId = Convert.ToInt64(dsStore.Rows[0]["ClientId"]);
                    clientel.CompanyName = Convert.ToString(dsStore.Rows[0]["CompanyName"]);
                    clientel.LegalName = Convert.ToString(dsStore.Rows[0]["LegalName"]);
                    clientel.PrimaryContact = Convert.ToString(dsStore.Rows[0]["PrimaryContact"]);
                    clientel.NumberOfEmployees = Convert.ToInt32(dsStore.Rows[0]["NumberOfEmployees"]);
                    clientel.AnnualSales = Convert.ToInt64(dsStore.Rows[0]["AnnualSales"]);
                    clientel.TaxIDNumber = Convert.ToString(dsStore.Rows[0]["TaxIDNumber"]);
                    clientel.VATNumber = Convert.ToString(dsStore.Rows[0]["VATNumber"]);
                    clientel.Email = Convert.ToString(dsStore.Rows[0]["Email"]);
                    clientel.Website = Convert.ToString(dsStore.Rows[0]["Website"]);
                    clientel.Status = Convert.ToString(dsStore.Rows[0]["Status"]);
                    clientel.BusinessType = Convert.ToString(dsStore.Rows[0]["BusinessType"]);
                    clientel.DateEstablished = Convert.ToDateTime(dsStore.Rows[0]["DateEstablished"]);
                    clientel.NumberOfLocations = Convert.ToInt32(dsStore.Rows[0]["NumberOfLocations"]);
                    clientel.OfficerName = Convert.ToString(dsStore.Rows[0]["OfficerName"]);
                    clientel.OfficerTitle = Convert.ToString(dsStore.Rows[0]["OfficerTitle"]);
                    clientel.OfficerPhone = Convert.ToString(dsStore.Rows[0]["OfficerPhone"]);
                    clientel.DunnsNumber = Convert.ToString(dsStore.Rows[0]["DunnsNumber"]);
                    clientel.PackageLevel = Convert.ToString(dsStore.Rows[0]["PackageLevel"]);
                    clientel.AppUsers = Convert.ToInt32(dsStore.Rows[0]["AppUsers"]);
                    clientel.MaxAppUsers = Convert.ToInt32(dsStore.Rows[0]["MaxAppUsers"]);
                    clientel.PhoneUsers = Convert.ToInt32(dsStore.Rows[0]["PhoneUsers"]);
                    clientel.MaxPhoneUsers = Convert.ToInt32(dsStore.Rows[0]["MaxPhoneUsers"]);
                    clientel.PrimarySICCode = Convert.ToString(dsStore.Rows[0]["PrimarySICCode"]);
                    clientel.NAICSCode = Convert.ToString(dsStore.Rows[0]["NAICSCode"]);
                    clientel.Sites = Convert.ToInt32(dsStore.Rows[0]["Sites"]);
                    clientel.MinorityStatus = Convert.ToString(dsStore.Rows[0]["MinorityStatus"]);
                    clientel.Localization = Convert.ToString(dsStore.Rows[0]["Localization"]);
                    clientel.DefaultTimeZone = Convert.ToString(dsStore.Rows[0]["DefaultTimeZone"]);
                    clientel.DefaultCustomerManager = Convert.ToString(dsStore.Rows[0]["DefaultCustomerManager"]);
                    clientel.MaxAttempts = Convert.ToInt32(dsStore.Rows[0]["MaxAttempts"]);
                    clientel.MaxTimeOut = Convert.ToInt32(dsStore.Rows[0]["MaxTimeOut"]);
                    clientel.ConnectionString = Convert.ToString(dsStore.Rows[0]["ConnectionString"]);
                    clientel.TabletUsers = Convert.ToInt32(dsStore.Rows[0]["TabletUsers"]);
                    clientel.MaxTabletUsers = Convert.ToInt32(dsStore.Rows[0]["MaxTabletUsers"]);
                    clientel.UIConfiguration = Convert.ToString(dsStore.Rows[0]["UIConfiguration"]);
                    clientel.WOPrintMessage = Convert.ToString(dsStore.Rows[0]["WOPrintMessage"]);
                    clientel.PurchaseTermsandConds = Convert.ToString(dsStore.Rows[0]["PurchaseTermsandConds"]);
                    clientel.UpdateIndex = Convert.ToInt64(dsStore.Rows[0]["UpdateIndex"]);

                } */


    }
}
