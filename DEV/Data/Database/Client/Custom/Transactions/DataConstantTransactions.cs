using Common.Enumerations;
using Database.Business;
using System.Collections.Generic;

namespace Database
{
    public class DataConstant_RetrieveLocaleForConstantType_V2 : DataConstant_TransactionBaseClass
    {
        public DataConstant_RetrieveLocaleForConstantType_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }
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
        public List<b_DataConstant> dataConstants { get; set; }  
        public override void PerformWorkItem()
        {
            List<b_DataConstant> tmpArray = null;
            DataConstant.RetrieveLocaleForConstantType_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            dataConstants = tmpArray;
        }
    }

    public class DataConstant_RetrieveLocaleForConstantTypeWithId_V2 : DataConstant_TransactionBaseClass
    {
        public DataConstant_RetrieveLocaleForConstantTypeWithId_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }
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
        public List<b_DataConstant> dataConstants { get; set; }
        public override void PerformWorkItem()
        {
            List<b_DataConstant> tmpArray = null;
            DataConstant.RetrieveLocaleForConstantTypeWithId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            dataConstants = tmpArray;
        }
    }
}
