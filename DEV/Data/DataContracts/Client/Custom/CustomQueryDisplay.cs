/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person          Description
* =========== ======== ================ ============================================================
* 2016-Aug-17 SOM-1049 Roger Lawton     Changed to use Case Number instead of ID - ID can change
****************************************************************************************************
 */

using Database;
using Database.Business;
using System.Collections.Generic;

namespace DataContracts
{
    public partial class CustomQueryDisplay
    {
        public static List<KeyValuePair<string, string>> RetrieveKVPItemsByTableAndLanguage(DatabaseKey databaseKey, string tableName, string language, string culture)
        {
            RetrieveCustomQueryDisplayByTableAndLanguageTransaction trans = new RetrieveCustomQueryDisplayByTableAndLanguageTransaction()
            {
                dbKey = databaseKey.ToTransDbKey(),
                TableName = tableName,
                Culture = culture,
                Language = language
            };

            trans.Execute();
            List<KeyValuePair<string, string>> results = new List<KeyValuePair<string, string>>();
            foreach (b_CustomQueryDisplay cqd in trans.Items)
            {
                results.Add(new KeyValuePair<string,string>(cqd.Key, cqd.DisplayText));
            }
            return results;
        }

        public static List<KeyValuePair<string, string>> RetrieveQueryItemsByTableAndLanguage(DatabaseKey databaseKey, string tableName, string language, string culture)
        {
            RetrieveCustomQueryDisplayByTableAndLanguageTransaction trans = new RetrieveCustomQueryDisplayByTableAndLanguageTransaction()
            {
                dbKey = databaseKey.ToTransDbKey(),
                TableName = tableName,
                Culture = culture,
                Language = language
            };

            trans.Execute();
            List<KeyValuePair<string, string>> results = new List<KeyValuePair<string, string>>();
            foreach (b_CustomQueryDisplay cqd in trans.Items)
            {
              results.Add(new KeyValuePair<string, string>(cqd.CaseNo.ToString(), cqd.DisplayText));
              //results.Add(new KeyValuePair<string, string>(cqd.CustomQueryDisplayId.ToString(), cqd.DisplayText));
            }
            return results;
        }

    }
}
