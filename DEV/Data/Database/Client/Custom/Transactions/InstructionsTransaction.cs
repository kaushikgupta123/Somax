
using System;
using System.Collections.Generic;

using Common.Enumerations;

using Database.Business;

namespace Database.Transactions
{

    public class Instructions_RetrieveInstructionsByObjectIdFromDatabase_V2 : Instructions_TransactionBaseClass
        {

            public Instructions_RetrieveInstructionsByObjectIdFromDatabase_V2()
            {
                // Set the database in which this table resides.
                // This must be called prior to base.PerformLocalValidation(), 
                // since that process will validate that the appropriate 
                // connection string is set.
                UseDatabase = DatabaseTypeEnum.Client;
            }

            public List<b_Instructions> InstructionsList { get; set; }

            public override void PerformLocalValidation()
            {
                base.PerformLocalValidation();
                if (Instructions.ObjectId == 0 || string.IsNullOrEmpty(Instructions.TableName))
                {
                    string message = "Instructions has an invalid object ID or table name.";
                    throw new Exception(message);
                }
            }

            public override void PerformWorkItem()
            {

            b_Instructions[] tmpArray = null;

            Instructions.RetrieveInstructionsByObjectIdFromDatabaseV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            InstructionsList = new List<b_Instructions>(tmpArray);
            }

        }
    }

