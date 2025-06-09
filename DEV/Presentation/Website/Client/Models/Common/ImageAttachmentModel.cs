using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.Common
{
    public class ImageAttachmentModel
    {
        public long AttachmentId { get; set; }
        public string ObjectName { get; set; }
        public long ObjectId { get; set; }
        public string AttachmentURL { get; set; }
        public long UploadedBy_PersonnelId { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public bool Image { get; set; }
        public bool Profile { get; set; }
        public int TotalCount { get; set; }
    }
}