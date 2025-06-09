using System.Web.Mvc;

namespace Client.Models.Common
{
    public class InstructionModel 
    {
        public long InstructionId { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public long ObjectId { get; set; }
    }
}