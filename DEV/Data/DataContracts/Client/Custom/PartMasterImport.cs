using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//using SOMAX.G4.Business.Localization;
using Common.Extensions;

using Database;
using Database.Business;

namespace DataContracts
{
  //, IStoredProcedureValidation
  public partial class PartMasterImport : DataContractBase
  {
    public string _ClientLookupID { get; set; }
    public int error_message_count { get; set; }

    public List<PartMasterImport> PartMasterImportRetrieveAll(DatabaseKey dbKey)
    {
      PartMasterImport_RetrieveAll trans = new PartMasterImport_RetrieveAll()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName
      };

      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      return UpdateFromDatabaseObjectList(trans.PartMasterImportList);
    }

    public static List<PartMasterImport> UpdateFromDatabaseObjectList(List<b_PartMasterImport> dbObjs)
    {
      List<PartMasterImport> result = new List<PartMasterImport>();

      foreach (b_PartMasterImport dbObj in dbObjs)
      {
        PartMasterImport tmp = new PartMasterImport();
        tmp.UpdateFromDatabaseObject(dbObj);
        result.Add(tmp);
      }
      return result;
    }

    public void RetrieveClientLookupID(DatabaseKey dbKey)
    {
      PartMasterClientLookUpID_Retrieve trans = new PartMasterClientLookUpID_Retrieve();
      trans._ClientLookupID = this._ClientLookupID;
      trans.PartMasterImport = ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      UpdateFromDatabaseObject(trans.PartMasterImport);
    }


    public void Create_PartMasterProcessInterface(DatabaseKey dbKey)
    {


      if (IsValid)
      {
        PartMasterImport_ProcessInterface trans = new PartMasterImport_ProcessInterface()
        {
          CallerUserInfoId = dbKey.User.UserInfoId,
          CallerUserName = dbKey.UserName,
        };
        trans.PartMasterImport = this.ToDatabaseObject();
        trans.dbKey = dbKey.ToTransDbKey();
        trans.Execute();

        // The create procedure may have populated an auto-incremented key. 
        UpdateFromDatabaseObject(trans.PartMasterImport);
        this.error_message_count = trans.PartMasterImport.error_message_count;
      }
    }


  }
}
