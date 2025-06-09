using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

using Database;
using Database.Business;
using Common.Extensions;

namespace DataContracts
{
    public partial class PRHeaderUDF : DataContractBase
    {
        #region Variables
        public string Select1Name { get; set; }
        public string Select2Name { get; set; }
        public string Select3Name { get; set; }
        public string Select4Name { get; set; }
        #region V2-945
        public string Text1Label { get; set; }
        public string Text2Label { get; set; }
        public string Text3Label { get; set; }
        public string Text4Label { get; set; }
        public string Date1Label { get; set; }
        public string Date2Label { get; set; }
        public string Date3Label { get; set; }
        public string Date4Label { get; set; }
        public string Bit1Label { get; set; }
        public string Bit2Label { get; set; }
        public string Bit3Label { get; set; }
        public string Bit4Label { get; set; }
        public string Numeric1Label { get; set; }
        public string Numeric2Label { get; set; }
        public string Numeric3Label { get; set; }
        public string Numeric4Label { get; set; }
        public string Select1Label { get; set; }
        public string Select2Label { get; set; }
        public string Select3Label { get; set; }
        public string Select4Label { get; set; }
        #endregion
        #endregion

        public PRHeaderUDF RetrieveByPurchaseRequestId(DatabaseKey dbKey)
        {
            PRHeaderUDF_RetrieveByPurchaseRequestId trans = new PRHeaderUDF_RetrieveByPurchaseRequestId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.PRHeaderUDF = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            PRHeaderUDF objPrHeaderUDF = new PRHeaderUDF();
            objPrHeaderUDF.UpdateFromDatabaseObjectExtended(trans.PRHeaderUDF);
            return objPrHeaderUDF;
        }
        public void UpdateFromDatabaseObjectExtended(b_PRHeaderUDF dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PRHeaderUDFId = dbObj.PRHeaderUDFId;
            this.PurchaseRequestId = dbObj.PurchaseRequestId;
            this.Text1 = dbObj.Text1;
            this.Text2 = dbObj.Text2;
            this.Text3 = dbObj.Text3;
            this.Text4 = dbObj.Text4;
            this.Date1 = dbObj.Date1;
            this.Date2 = dbObj.Date2;
            this.Date3 = dbObj.Date3;
            this.Date4 = dbObj.Date4;
            this.Bit1 = dbObj.Bit1;
            this.Bit2 = dbObj.Bit2;
            this.Bit3 = dbObj.Bit3;
            this.Bit4 = dbObj.Bit4;
            this.Numeric1 = dbObj.Numeric1;
            this.Numeric2 = dbObj.Numeric2;
            this.Numeric3 = dbObj.Numeric3;
            this.Numeric4 = dbObj.Numeric4;
            this.Select1 = dbObj.Select1;
            this.Select2 = dbObj.Select2;
            this.Select3 = dbObj.Select3;
            this.Select4 = dbObj.Select4;
            this.Select1Name = dbObj.Select1Name;
            this.Select2Name = dbObj.Select2Name;
            this.Select3Name = dbObj.Select3Name;
            this.Select4Name = dbObj.Select4Name;

            AuditEnabled = true;
        }
        #region V2-945
        public void UpdateFromDatabaseObjectPRHeaderUDFtPrintExtended(b_PRHeaderUDF dbObj, string Timezone)
        {
            this.PurchaseRequestId = dbObj.PurchaseRequestId;
            this.Text1 = dbObj.Text1;
            this.Text2 = dbObj.Text2;
            this.Text3 = dbObj.Text3;
            this.Text4 = dbObj.Text4;
            if (dbObj.Date1 != null && dbObj.Date1 != DateTime.MinValue)
            {
                this.Date1 = dbObj.Date1.ToUserTimeZone(Timezone);
            }
            else
            {
                this.Date1 = null;
            }
            if (dbObj.Date2 != null && dbObj.Date2 != DateTime.MinValue)
            {
                this.Date2 = dbObj.Date2.ToUserTimeZone(Timezone);
            }
            else
            {
                this.Date2 = null;
            }
            if (dbObj.Date3 != null && dbObj.Date3 != DateTime.MinValue)
            {
                this.Date3 = dbObj.Date3.ToUserTimeZone(Timezone);
            }
            else
            {
                this.Date3 = null;
            }
            if (dbObj.Date4 != null && dbObj.Date4 != DateTime.MinValue)
            {
                this.Date4 = dbObj.Date4.ToUserTimeZone(Timezone);
            }
            else
            {
                this.Date4 = null;
            }
            this.Bit1 = dbObj.Bit1;
            this.Bit2 = dbObj.Bit2;
            this.Bit3 = dbObj.Bit3;
            this.Bit4 = dbObj.Bit4;
            this.Numeric1 = dbObj.Numeric1;
            this.Numeric2 = dbObj.Numeric2;
            this.Numeric3 = dbObj.Numeric3;
            this.Numeric4 = dbObj.Numeric4;
            this.Select1 = dbObj.Select1;
            this.Select2 = dbObj.Select2;
            this.Select3 = dbObj.Select3;
            this.Select4 = dbObj.Select4;
            this.Text1Label = dbObj.Text1Label;
            this.Text2Label = dbObj.Text2Label;
            this.Text3Label = dbObj.Text3Label;
            this.Text4Label = dbObj.Text4Label;
            this.Date1Label = dbObj.Date1Label;
            this.Date2Label = dbObj.Date2Label;
            this.Date3Label = dbObj.Date3Label;
            this.Date4Label = dbObj.Date4Label;
            this.Bit1Label = dbObj.Bit1Label;
            this.Bit2Label = dbObj.Bit2Label;
            this.Bit3Label = dbObj.Bit3Label;
            this.Bit4Label = dbObj.Bit4Label;
            this.Numeric1Label = dbObj.Numeric1Label;
            this.Numeric2Label = dbObj.Numeric2Label;
            this.Numeric3Label = dbObj.Numeric3Label;
            this.Numeric4Label = dbObj.Numeric4Label;
            this.Select1Label = dbObj.Select1Label;
            this.Select2Label = dbObj.Select2Label;
            this.Select3Label = dbObj.Select3Label;
            this.Select4Label = dbObj.Select4Label;
        }
        #endregion
    }
}
