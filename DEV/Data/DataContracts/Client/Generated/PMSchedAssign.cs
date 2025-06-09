using Database.Business;
using Database.Transactions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
 /// <summary>
 /// Business object that stores a record from the PMSchedAssign table.
 /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class PMSchedAssign : DataContractBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PMSchedAssign()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_PMSchedAssign dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PMSchedAssignId = dbObj.PMSchedAssignId;
            this.PrevMaintSchedId = dbObj.PrevMaintSchedId;
            this.PersonnelId = dbObj.PersonnelId;
            this.ScheduledHours = dbObj.ScheduledHours;

            // Turn on auditing
            AuditEnabled = true;
        }

        private void Initialize()
        {
            b_PMSchedAssign dbObj = new b_PMSchedAssign();
            UpdateFromDatabaseObject(dbObj);

            // Turn off auditing for object initialization
            AuditEnabled = false;
        }

        public b_PMSchedAssign ToDatabaseObject()
        {
            b_PMSchedAssign dbObj = new b_PMSchedAssign();
            dbObj.ClientId = this.ClientId;
            dbObj.PMSchedAssignId = this.PMSchedAssignId;
            dbObj.PrevMaintSchedId = this.PrevMaintSchedId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.ScheduledHours = this.ScheduledHours;
            return dbObj;
        }

        #endregion

        #region Transaction Methods

        public void Create(DatabaseKey dbKey)
        {
            PMSchedAssign_Create trans = new PMSchedAssign_Create();
            trans.PMSchedAssign = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.PMSchedAssign);
        }

        public void Retrieve(DatabaseKey dbKey)
        {
            PMSchedAssign_Retrieve trans = new PMSchedAssign_Retrieve();
            trans.PMSchedAssign = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PMSchedAssign);
        }

        public void Update(DatabaseKey dbKey)
        {
            PMSchedAssign_Update trans = new PMSchedAssign_Update();
            trans.PMSchedAssign = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PMSchedAssign);
        }

        public void Delete(DatabaseKey dbKey)
        {
            PMSchedAssign_Delete trans = new PMSchedAssign_Delete();
            trans.PMSchedAssign = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }

        protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.PMSchedAssignId;
            return BuildChangeLogDbObject(dbKey);
        }

        #endregion

        #region Private Variables

        private long _ClientId;
        private long _PMSchedAssignId;
        private long _PrevMaintSchedId;
        private long _PersonnelId;
        private decimal _ScheduledHours;
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
        /// PMSchedAssignId property
        /// </summary>
        [DataMember]
        public long PMSchedAssignId
        {
            get { return _PMSchedAssignId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PMSchedAssignId); }
        }

        /// <summary>
        /// PrevMaintSchedId property
        /// </summary>
        [DataMember]
        public long PrevMaintSchedId
        {
            get { return _PrevMaintSchedId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PrevMaintSchedId); }
        }

        /// <summary>
        /// PersonnelId property
        /// </summary>
        [DataMember]
        public long PersonnelId
        {
            get { return _PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PersonnelId); }
        }

        /// <summary>
        /// ScheduledHours property
        /// </summary>
        [DataMember]
        public decimal ScheduledHours
        {
            get { return _ScheduledHours; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _ScheduledHours); }
        }
        #endregion


    }
}
