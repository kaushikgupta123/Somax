using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Extensions;

using Database;
using Database.Business;
using Data.Database;

namespace DataContracts
{
  public partial class VendorCatalogImport : DataContractBase
  {
    public int error_message_count;
    public List<VendorCatalogImport> VendorCatalogImportRetrieveAll(DatabaseKey dbKey)
    {
      VendorCatalogImport_RetrieveAll trans = new VendorCatalogImport_RetrieveAll()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName
      };

      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      return UpdateFromDatabaseObjectList(trans.VendorCatalogImportList);
    }

    public List<VendorCatalogImport> UpdateFromDatabaseObjectList(List<b_VendorCatalogImport> vendorCatalogImportList)
    {
      List<VendorCatalogImport> result = new List<VendorCatalogImport>();

      foreach (b_VendorCatalogImport dbObj in vendorCatalogImportList)
      {
        VendorCatalogImport tmp = new VendorCatalogImport();
        tmp.UpdateFromDatabaseObject(dbObj);
        result.Add(tmp);
      }
      return result;
    }



    public void RetrieveForImportCheck(DatabaseKey dbKey)
    {
      VendorCatalogImport_RetrieveForImportCheck trans = new VendorCatalogImport_RetrieveForImportCheck();
      trans.VendorCatalogImport = ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      UpdateFromDatabaseObject(trans.VendorCatalogImport);
    }


    public void Create_VendorCatalogProcessInterface(DatabaseKey dbKey)
    {
      VendorCatalogImport_ProcessInterface trans = new VendorCatalogImport_ProcessInterface()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName,
      };
      trans.VendorCatalogImport = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure may have populated an auto-incremented key. 
      UpdateFromDatabaseObject(trans.VendorCatalogImport);
      this.error_message_count = trans.VendorCatalogImport.error_message_count;

    }


  }
}
