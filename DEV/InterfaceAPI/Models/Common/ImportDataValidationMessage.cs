using System.Collections.Generic;

namespace InterfaceAPI.Models.Common
{
    public class ImportDataValidationMessage
    {
        public ImportDataValidationMessage()
        {
            ErrorMessage = new List<string>();
        }
        public int ItemNumber { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}