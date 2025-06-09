﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.Knowledgebase
{
    public class KnowledgebaseSearchModel
    {
        public long KBTopicsId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string CategoryName { get; set; }
        public string Folder { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
    }
}