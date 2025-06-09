using Common.Structures;
using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class UIConfig : DataContractBase
    {

        #region Property
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string isHide { get; set; }
        public string isRequired { get; set; }
        public string Isexternal { get; set; }

        #endregion
        public List<UIConfig> UIConfigRetrieveHiddenByViewOrTable(DatabaseKey dbKey)
        {
            UIConfig_RetrieveHiddenByViewOrTable trans = new UIConfig_RetrieveHiddenByViewOrTable()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UIConfig = this.ToDatabaseObject();
            trans.UIConfig.isHide = this.isHide;
            trans.UIConfig.isRequired = this.isRequired;
            trans.UIConfig.Isexternal = this.Isexternal;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.UIConfigList));

        }


        public static List<UIConfig> UpdateFromDatabaseObjectList(List<b_UIConfig> dbObjs)
        {
            List<UIConfig> result = new List<UIConfig>();

            foreach (b_UIConfig dbObj in dbObjs)
            {
                UIConfig tmp = new UIConfig();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.CreateDate = dbObj.CreateDate;
                tmp.CreateBy = dbObj.CreateBy;
                tmp.ModifyDate = dbObj.ModifyDate;
                tmp.ModifyBy = dbObj.ModifyBy;
                result.Add(tmp);
            }
            return result;
        }
    }
}