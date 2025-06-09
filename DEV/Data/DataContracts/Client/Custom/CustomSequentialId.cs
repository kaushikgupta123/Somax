/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using  Database;
using  Database.Business;

namespace  DataContracts
{
    /// <summary>
    /// The CustomSequentialId API is used to retrieve the next available ID for a client. 
    /// This can be used to supply Ids for a work order, for example. 
    /// </summary>
    public partial class CustomSequentialId : DataContractBase
    {
        #region Property
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public string PRPrefix { get; set; }

        public string KeyList { get; set; }
        public string POPrefix { get; set; }
        public int PRUpdateIndexOut { get; set; }
        public int POUpdateIndexOut { get; set; }
        public string Key { get; set; }
        public string Prefix { get; set; }
        #endregion
        public static string GetNextId(DatabaseKey dbKey, string idKey, long siteId, string custom,string customSuffix=null)
        {
            /*
             * The GetNextId method will retrieve the NextSeed, Pattern, UpdateIndex and ModifiedDate.
             * The Pattern will replace the following character sequences. These are not case sensitive.
             *  {YY} : Replaced with current 2 digit year. Seed resets every year, unless more frequent reset is indicated.
             *  {YYYY} : Replaced with current 4 digit year. Seed resets every year, unless more frequent reset is indicated.
             *  {MM} : Replaced with current 2 digit month. Seed resets every month, unless more frequent reset is indicated.
             *  {DD} : Replaced with current 2 digit day. Seed resets every day.
             *  {SSSS} : Replaced with 4 digit NextSeed.
             *  {SSSSSS} : Replaced with 6 digit NextSeed.
             *  {SSSSSSSS} : Replaced with 8 digit NextSeed.
             *  {SSSSSSSSSS} : Replaced with 10 digit NextSeed.
             *  {C} : Replaced with custom string, provided by invoking application
             *  
             *  The last ModifiedDate is checked against the current date to see if a reset is required. If so, 
             *  a second, reset request is sent. The reset procedure will return the next seed, using the update index to determine
             *  if another process already reset the seed. 
             */
            

            // Retrieve Pattern, ModifiedDate and NextSeed. 

            CustomSequentialId_RetrieveNext trans = new CustomSequentialId_RetrieveNext()
            {
                dbKey = dbKey.ToTransDbKey(),
                Key = idKey,
                SiteId = siteId
            };
            trans.CustomIdResult = new b_CustomIdResult();
            trans.Execute();

            string ucPattern = trans.CustomIdResult.Pattern.ToUpperInvariant();

            // Determine reset criteria
            bool resetSeed = false;
            if (ucPattern.Contains("{DD}"))
            {
                // Daily reset is required. 

            }
            else if (ucPattern.Contains("{MM}"))
            {
                // Monthly reset is required. 
            }
            else if (ucPattern.Contains("{YY}") || ucPattern.Contains("{YYYY}"))
            {
                // Yearly reset is required. 
            }

            if (resetSeed)
            {
                CustomSequentialId_ResetAndRetrieveNext newTrans = new CustomSequentialId_ResetAndRetrieveNext()
                {
                    dbKey = dbKey.ToTransDbKey(),
                    Key = idKey,
                    SiteId = siteId,
                    UpdateIndex = trans.CustomIdResult.UpdateIndex
                };
                newTrans.Execute();
                trans.CustomIdResult = newTrans.CustomIdResult;
            }

            return ReplacePattern(trans.CustomIdResult.Pattern, trans.CustomIdResult.LastSeed, custom, customSuffix);
        }

        private static string ReplacePattern(string pattern, long newSeed, string custom ,string customSuffix=null)
        {
            string result = pattern.ToUpperInvariant();

            string yy = (DateTime.Now.Year % 100).ToString("00");
            string yyyy = DateTime.Now.Year.ToString();
            string mm = DateTime.Now.Month.ToString("00");
            string dd = DateTime.Now.Day.ToString("00");
            string _3s = newSeed.ToString("000");
            string _4s = newSeed.ToString("0000");
            string _6s = newSeed.ToString("000000");
            string _8s = newSeed.ToString("00000000");
            string _10s = newSeed.ToString("0000000000");

            result = result.Replace("{YY}", yy);
            result = result.Replace("{YYYY}", yyyy);
            result = result.Replace("{MM}", mm);
            result = result.Replace("{DD}", dd);
            result = result.Replace("{SSS}", _3s);
            result = result.Replace("{SSSS}", _4s);
            result = result.Replace("{SSSSSS}", _6s);
            result = result.Replace("{SSSSSSSS}", _8s);
            result = result.Replace("{SSSSSSSSSS}", _10s);
            result = result.Replace("{C}", custom.ToUpperInvariant());
            result = result.Replace("{CC}", custom.ToUpperInvariant()); //V2-1112
            result = result.Replace("{CCC}", customSuffix?.ToUpperInvariant()); //V2-1112

            return result;
        }


        public List<CustomSequentialId> RetrieveByClientIdandSiteIdandKey_V2(DatabaseKey dbKey)
        {
            CustomSequentialId_RetrieveByClientIdandSiteIdandKey_V2 trans = new CustomSequentialId_RetrieveByClientIdandSiteIdandKey_V2();
            trans.CustomIdResult =this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return(UpdateFromDatabaseObject(trans.custList));
        }


        public static List<CustomSequentialId> UpdateFromDatabaseObject(List<b_CustomIdResult> dbObjs)
        {
            List<CustomSequentialId> result = new List<CustomSequentialId>();
            foreach (b_CustomIdResult dbObj in dbObjs)
            {
                CustomSequentialId tmp = new CustomSequentialId();                
                tmp.ClientId = dbObj.ClientId;
                tmp.SiteId = dbObj.SiteId;
                tmp.Prefix = dbObj.Prefix;
                tmp.Key = dbObj.Key;
                result.Add(tmp);
            }
              return result;
        }


        public void UpdatePrefixbyKey_V2(DatabaseKey dbKey)
        {
            if (IsValid)
            {
                CustomId_UpdateUpdatePrefixbyKey_V2 trans = new CustomId_UpdateUpdatePrefixbyKey_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.CustomIdResult = this.ToExtendedDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromExtendedDatabaseObject(trans.CustomIdResult);
            }
        }



        public void UpdateFromExtendedDatabaseObject(b_CustomIdResult dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.PRPrefix = dbObj.PRPrefix;
            this.POPrefix = dbObj.POPrefix;
        }


        public b_CustomIdResult ToExtendedDatabaseObject()
        {
            b_CustomIdResult dbObj = new b_CustomIdResult();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.KeyList = this.KeyList;
            dbObj.PRPrefix = this.PRPrefix;
            dbObj.POPrefix = this.POPrefix;

            return dbObj;
        }


     
    }
}
