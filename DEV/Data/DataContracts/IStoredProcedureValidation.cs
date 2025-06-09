using System.Collections.Generic;

namespace DataContracts
{
    public interface IStoredProcedureValidation
    {
        List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey);
    }
}
