using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;

namespace Database
{
    public class TechSpecs_RetrieveBySiteId : TechSpecs_TransactionBaseClass
    {
        public List<b_TechSpecs> TechSpecsList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_TechSpecs> tmpArray = null;

            TechSpecs.RetrieveBySiteIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            TechSpecsList = new List<b_TechSpecs>();
            foreach (b_TechSpecs tmpObj in tmpArray)
            {
                TechSpecsList.Add(tmpObj);
            }
        }
    }

    public class TechSpecs_CheckKeyAndDeleteByPk : TechSpecs_TransactionBaseClass
    {
        public List<b_TechSpecs> TechSpecsList { get; set; }
        public Int64 TechSpecId { get; set; }
        public bool Flag { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            TechSpecs_CheckKeyAndDeleteByPk trans = new TechSpecs_CheckKeyAndDeleteByPk()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            TechSpecs.CheckKeyAndDeleteByPK(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            Flag = TechSpecs.Flag;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
        public void UpdateFromDatabaseObject(b_TechSpecs dbObj)
        {
            this.TechSpecs.ClientLookupId = dbObj.ClientLookupId;
            this.TechSpecs.Description = dbObj.Description;
            this.TechSpecs.InactiveFlag = dbObj.InactiveFlag;
            this.TechSpecs.UnitOfMeasure = dbObj.UnitOfMeasure;
        }
    }
}
