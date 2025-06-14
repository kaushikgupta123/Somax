﻿/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2017 by SOMAX Inc.
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
  /// Business object that stores a record from the ImportLog table.
  /// </summary>
  [Serializable()]
  [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
  public partial class ImportLog : DataContractBase
  {
    #region Constructors
    /// <summary>
    /// Default constructor.
    /// </summary>
    public ImportLog()
    {
      Initialize();
    }

    public void Clear()
    {
      Initialize();
    }

    public void UpdateFromDatabaseObject(b_ImportLog dbObj)
    {
      this.ClientId = dbObj.ClientId;
      this.ImportLogId = dbObj.ImportLogId;
      this.ProcessName = dbObj.ProcessName;
      this.Transactions = dbObj.Transactions;
      this.RunDate = dbObj.RunDate;
      this.RunBy = dbObj.RunBy;
      this.NewTransactions = dbObj.NewTransactions;
      this.SuccessfulTransactions = dbObj.SuccessfulTransactions;
      this.FailedTransactions = dbObj.FailedTransactions;
      this.FileName = dbObj.FileName;
      this.Message = dbObj.Message;
      this.CompleteDate = dbObj.CompleteDate;

      // Turn on auditing
      AuditEnabled = true;
    }

    private void Initialize()
    {
      b_ImportLog dbObj = new b_ImportLog();
      UpdateFromDatabaseObject(dbObj);

      // Turn off auditing for object initialization
      AuditEnabled = false;
    }

    public b_ImportLog ToDatabaseObject()
    {
      b_ImportLog dbObj = new b_ImportLog();
      dbObj.ClientId = this.ClientId;
      dbObj.ImportLogId = this.ImportLogId;
      dbObj.ProcessName = this.ProcessName;
      dbObj.Transactions = this.Transactions;
      dbObj.RunDate = this.RunDate;
      dbObj.RunBy = this.RunBy;
      dbObj.NewTransactions = this.NewTransactions;
      dbObj.SuccessfulTransactions = this.SuccessfulTransactions;
      dbObj.FailedTransactions = this.FailedTransactions;
      dbObj.FileName = this.FileName;
      dbObj.Message = this.Message;
      dbObj.CompleteDate = this.CompleteDate;
      return dbObj;
    }

    #endregion

    #region Transaction Methods

    public void Create(DatabaseKey dbKey)
    {
      ImportLog_Create trans = new ImportLog_Create();
      trans.ImportLog = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure may have populated an auto-incremented key. 
      UpdateFromDatabaseObject(trans.ImportLog);
    }

    public void Retrieve(DatabaseKey dbKey)
    {
      ImportLog_Retrieve trans = new ImportLog_Retrieve();
      trans.ImportLog = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      UpdateFromDatabaseObject(trans.ImportLog);
    }

    public void Update(DatabaseKey dbKey)
    {
      ImportLog_Update trans = new ImportLog_Update();
      trans.ImportLog = this.ToDatabaseObject();
      trans.ChangeLog = GetChangeLogObject(dbKey);
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure changed the Update Index.
      UpdateFromDatabaseObject(trans.ImportLog);
    }

    public void Delete(DatabaseKey dbKey)
    {
      ImportLog_Delete trans = new ImportLog_Delete();
      trans.ImportLog = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
    }

    protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
    {
      AuditTargetObjectId = this.ImportLogId;
      return BuildChangeLogDbObject(dbKey);
    }

    #endregion

    #region Private Variables

    private long _ClientId;
    private long _ImportLogId;
    private string _ProcessName;
    private int _Transactions;
    private DateTime? _RunDate;
    private string _RunBy;
    private int _NewTransactions;
    private int _SuccessfulTransactions;
    private int _FailedTransactions;
    private string _FileName;
    private string _Message;
    private DateTime? _CompleteDate;
    #endregion

    #region Properties


    /// <summary>
    /// ClientId property
    /// </summary>
    [DataMember]
    public long ClientId
    {
      get { return _ClientId; }
      set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ClientId); }
    }

    /// <summary>
    /// ImportLogId property
    /// </summary>
    [DataMember]
    public long ImportLogId
    {
      get { return _ImportLogId; }
      set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ImportLogId); }
    }

    /// <summary>
    /// ProcessName property
    /// </summary>
    [DataMember]
    public string ProcessName
    {
      get { return _ProcessName; }
      set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ProcessName); }
    }

    /// <summary>
    /// Transactions property
    /// </summary>
    [DataMember]
    public int Transactions
    {
      get { return _Transactions; }
      set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _Transactions); }
    }

    /// <summary>
    /// RunDate property
    /// </summary>
    [DataMember]
    public DateTime? RunDate
    {
      get { return _RunDate; }
      set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _RunDate); }
    }

    /// <summary>
    /// RunBy property
    /// </summary>
    [DataMember]
    public string RunBy
    {
      get { return _RunBy; }
      set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _RunBy); }
    }

    /// <summary>
    /// NewTransactions property
    /// </summary>
    [DataMember]
    public int NewTransactions
    {
      get { return _NewTransactions; }
      set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _NewTransactions); }
    }

    /// <summary>
    /// SuccessfulTransactions property
    /// </summary>
    [DataMember]
    public int SuccessfulTransactions
    {
      get { return _SuccessfulTransactions; }
      set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _SuccessfulTransactions); }
    }

    /// <summary>
    /// FailedTransactions property
    /// </summary>
    [DataMember]
    public int FailedTransactions
    {
      get { return _FailedTransactions; }
      set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _FailedTransactions); }
    }

    /// <summary>
    /// FileName property
    /// </summary>
    [DataMember]
    public string FileName
    {
      get { return _FileName; }
      set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _FileName); }
    }

    /// <summary>
    /// Message property
    /// </summary>
    [DataMember]
    public string Message
    {
      get { return _Message; }
      set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Message); }
    }

    /// <summary>
    /// CompleteDate property
    /// </summary>
    [DataMember]
    public DateTime? CompleteDate
    {
      get { return _CompleteDate; }
      set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _CompleteDate); }
    }
    #endregion


  }
}
