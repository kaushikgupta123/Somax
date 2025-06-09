/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
* 2015-Mar-24 SOM-585  Roger Lawton        Localized the Status
****************************************************************************************************
 */
using System.Collections.Generic;
using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    public partial class Localizations : DataContractBase
    {
        public List<Localizations> LocalizationsRetrieveAll(DatabaseKey dbKey)
        {
            Localizations_RetrieveAll trans = new Localizations_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.LocalizationsList);
        }
        public static List<Localizations> UpdateFromDatabaseObjectList(List<b_Localizations> dbObjs)
        {
            List<Localizations> result = new List<Localizations>();

            foreach (b_Localizations dbObj in dbObjs)
            {
                Localizations tmp = new Localizations();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public List<Localizations> RetrieveByResourceSet(DatabaseKey dbKey)
        {

            Localizations_RetrieveByResourceSet trans = new Localizations_RetrieveByResourceSet()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Localizations = this.ToDatabaseObject();
            trans.Localizations.ResourceSet = this.ResourceSet;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            var result =  UpdateFromDatabaseObjectList(trans.LocalizationsList);
            return result;

        }


    }

}
