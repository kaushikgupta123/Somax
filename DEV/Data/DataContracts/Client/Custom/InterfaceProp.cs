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
  public partial class InterfaceProp : DataContractBase, IStoredProcedureValidation
  {

    
    public void CheckIsActive(DatabaseKey dbKey)
    {
      InterfaceProp_CheckInterfaceIsActive trans = new InterfaceProp_CheckInterfaceIsActive()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName
      };

      trans.InterfaceProp = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      UpdateFromDatabaseObject(trans.InterfaceProp);

    }
    public void RetrieveInterfaceProperties(DatabaseKey dbKey)
    {
      InterfaceProp_RetrieveInterfaceProperties trans = new InterfaceProp_RetrieveInterfaceProperties()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName
      };

      trans.InterfaceProp = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      UpdateFromDatabaseObject(trans.InterfaceProp);

    }

    public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
    {
      List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
      return errors;
    }

        public List<InterfaceProp> RetrieveInterfacePropertiesByClientIdSiteId(DatabaseKey dbKey)
        {
            InterfaceProp_RetrieveInterfacePropertiesByClientIdSiteId trans = new InterfaceProp_RetrieveInterfacePropertiesByClientIdSiteId();
            {
                CallerUserInfoId = dbKey.User.UserInfoId;
                CallerUserName = dbKey.UserName;
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<InterfaceProp> InterfacePropList = new List<InterfaceProp>();
            foreach (b_InterfaceProp InterfaceProp in trans.InterfacePropList)
            {
                InterfaceProp tmpInterfaceProp = new InterfaceProp();

                tmpInterfaceProp.UpdateFromDatabaseObject(InterfaceProp);
                InterfacePropList.Add(tmpInterfaceProp);
            }
            return InterfacePropList;
        }
  }
}