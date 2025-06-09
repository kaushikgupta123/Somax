using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class UIView: DataContractBase
    {
        #region Retrieve All UI View by clientid
        public List<UIView> RetrieveAllCustom(DatabaseKey dbKey)
        {
            UIView_RetrieveAll trans = new UIView_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.UIViewList = new List<b_UIView>();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.UIViewList);
        }
        public static List<UIView> UpdateFromDatabaseObjectList(List<b_UIView> dbObjs)
        {
            List<UIView> result = new List<UIView>();

            foreach (b_UIView dbObj in dbObjs)
            {
                UIView tmp = new UIView();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        #endregion
    }
}
