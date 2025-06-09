using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class DataConstant : DataContractBase
    {
        public List<DataConstant> UpdateFromDatabaseLocaleForConstantType(List<b_DataConstant> dbObjs)
        {
            List<DataConstant> result = new List<DataConstant>();

            foreach (b_DataConstant dbObj in dbObjs)
            {
                DataConstant tmp = new DataConstant();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.Value = dbObj.Value;
                tmp.LocalName = dbObj.LocalName;
                result.Add(tmp);
            }
            return result;
        }
        public List<DataConstant> RetrieveLocaleForConstantType_V2(DatabaseKey dbKey)
        {
            DataConstant_RetrieveLocaleForConstantType_V2 trans = new DataConstant_RetrieveLocaleForConstantType_V2();
            trans.DataConstant = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseLocaleForConstantType(trans.dataConstants);
        }

        public List<DataConstant> RetrieveLocaleForConstantTypeWithId_V2(DatabaseKey dbKey)
        {
            DataConstant_RetrieveLocaleForConstantTypeWithId_V2 trans = new DataConstant_RetrieveLocaleForConstantTypeWithId_V2();
            trans.DataConstant = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseWithIdLocaleForConstantType(trans.dataConstants);
        }

        public List<DataConstant> UpdateFromDatabaseWithIdLocaleForConstantType(List<b_DataConstant> dbObjs)
        {
            List<DataConstant> result = new List<DataConstant>();

            foreach (b_DataConstant dbObj in dbObjs)
            {
                DataConstant tmp = new DataConstant();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.DataConstantId = dbObj.DataConstantId;
                tmp.LocalName = dbObj.LocalName;
                result.Add(tmp);
            }
            return result;
        }
    }
}
