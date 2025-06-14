/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2020 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

using Database;
using Database.Business;

namespace DataContracts
{
    /// <summary>
    /// Business object that stores a record from the ReportListing table.
    /// </summary>
   
    public partial class ReportFavorites : DataContractBase 
    {
        #region Properties
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string ReportGroup { get; set; }
        public string SourceName { get; set; }
        public bool UseSP { get; set; }
        public string PrimarySortColumn { get; set; }
        public string SecondarySortColumn { get; set; }
        public bool IsGrouped { get; set; }
        public string GroupColumn { get; set; }
        public bool IncludePrompt { get; set; }
        public string Prompt1Source { get; set; }
        public string Prompt1Type { get; set; }
        public string Prompt1List { get; set; }
        public string Prompt1ListSource { get; set; }
        public string Prompt2Source { get; set; }
        public string Prompt2Type { get; set; }
        public string Prompt2List { get; set; }
        public string Prompt2ListSource { get; set; }
        public string SaveType { get; set; }
        #endregion
        public List<ReportFavorites> RetrieveMyFavorites(DatabaseKey dbKey)
        {
            ReportFavorites_RetrieveMyFavorites trans = new ReportFavorites_RetrieveMyFavorites();
            trans.ReportFavorites = this.ToDatabaseObject();
            
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseReportFavorites(trans.ReportFavoritesList);
        }
        public List<ReportFavorites> UpdateFromDatabaseReportFavorites(List<b_ReportFavorites> dbObjs)
        {
            List<ReportFavorites> result = new List<ReportFavorites>();

            foreach (b_ReportFavorites dbObj in dbObjs)
            {
                ReportFavorites tmp = new ReportFavorites();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.ReportName = dbObj.ReportName;
                tmp.Description = dbObj.Description;
                tmp.ReportGroup = dbObj.ReportGroup;
                tmp.SourceName = dbObj.SourceName;
                tmp.UseSP = dbObj.UseSP;
                tmp.PrimarySortColumn = dbObj.PrimarySortColumn;
                tmp.SecondarySortColumn = dbObj.SecondarySortColumn;
                tmp.IsGrouped = dbObj.IsGrouped;
                tmp.GroupColumn = dbObj.GroupColumn;
                tmp.IncludePrompt = dbObj.IncludePrompt;
                tmp.Prompt1Source = dbObj.Prompt1Source;
                tmp.Prompt1Type = dbObj.Prompt1Type;
                tmp.Prompt1ListSource = dbObj.Prompt1ListSource;
                tmp.Prompt1List = dbObj.Prompt1List;
                tmp.Prompt2Source = dbObj.Prompt2Source;
                tmp.Prompt2Type = dbObj.Prompt2Type;
                tmp.Prompt2ListSource = dbObj.Prompt2ListSource;
                tmp.Prompt2List = dbObj.Prompt2List;
                tmp.SaveType = dbObj.SaveType;
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateMyFavorites(DatabaseKey dbKey)
        {
            ReportFavorites_UpdateMyFavorites trans = new ReportFavorites_UpdateMyFavorites();
            trans.ReportFavorites = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
    }
}
