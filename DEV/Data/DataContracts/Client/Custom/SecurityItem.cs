/*
 *  Added By Indusnet Technologies
 */

using System.Collections.Generic;
using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    public partial class SecurityItem:DataContractBase
    {

        #region Properties
        public bool Protected { get; set; }

        private string _LocalizedSecurityItemName;
      
        public string LocalizedSecurityItemName
        {
            get 
            {
                return(string.IsNullOrEmpty(_LocalizedSecurityItemName) ? ItemName : _LocalizedSecurityItemName);
            }
            set
            {
                _LocalizedSecurityItemName = value;
            }
        }


        public long AccessingClientId { get; set; }

        public string SecurityItemTab { get; set; }
        public string PackageLevel { get; set; }
        public int ProductGrouping { get; set; }
        #endregion

        public void UpdateFromDatabaseObjectExtended(b_SecurityItem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Protected = dbObj.Protected;
            this.AccessingClientId = dbObj.AccessClientId;
            this.SecurityItemTab = dbObj.SecurityItemTab;
            this.PackageLevel = dbObj.PackageLevel;
            this.ProductGrouping = dbObj.ProductGrouping;
        }
        public b_SecurityItem ToDatabaseObjectExtended()
        {
            b_SecurityItem dbObj = this.ToDatabaseObject();
            dbObj.Protected = this.Protected;
            dbObj.AccessClientId = this.AccessingClientId;
            dbObj.SecurityItemTab = this.SecurityItemTab;
            dbObj.PackageLevel = this.PackageLevel;
            dbObj.ProductGrouping = this.ProductGrouping;
            return dbObj;
        }


        public static List<SecurityItem> UpdateFromDatabaseObjectListExtended(List<b_SecurityItem> dbObjs)
        {
            List<SecurityItem> result = new List<SecurityItem>();

            foreach (b_SecurityItem dbObj in dbObjs)
            {
                SecurityItem tmp = new SecurityItem();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public int Createtemplate(DatabaseKey dbKey)
        {
            SecurityItem_CreateTemplate trans = new SecurityItem_CreateTemplate();
            trans.SecurityItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (trans.NoOfTemplateCreated);
        }


        public List<SecurityItem> RetrieveAllByClientAndSecurityProfile(DatabaseKey dbKey)
        {
            SecurityItem_RetrieveAllByClientAndSecurityProfile trans = new SecurityItem_RetrieveAllByClientAndSecurityProfile()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SecurityItem = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectListExtended(trans.SecurityItemList);
        }

        public List<SecurityItem> CustomRetrieveAllV2(DatabaseKey dbKey)
        {
            CustomSecurityItem_RetrieveAll_V2 trans = new CustomSecurityItem_RetrieveAll_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SecurityItem = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectListExtended(trans.SecurityItemList);
        }
    }


}
