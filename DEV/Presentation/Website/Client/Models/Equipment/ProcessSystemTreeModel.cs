using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Equipment
{
    public class ProcessSystemTreeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public long TempId { get; set; }
        public long ProcessSystemId { get; set; }
    }
}