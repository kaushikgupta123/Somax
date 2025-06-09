using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTDataLayer.EL
{
    public class LookupListEL
    {
        public Int64 ClientId
        { set; get; }
        public Int64 LookupListId
        { set; get; }
        public Int64 SiteID
        { set; get; }
        public Int64 AreaId
        { set; get; }       
        public Int64 DepartmentId
        { set; get; }
        public Int64 StoreRoomId
        { set; get; }
        public string ListName
        { set; get; }
        public string ListValue
        { set; get; }
        public string Filter
        { set; get; }
        public string Description
        { set; get; }
        public string CreateBy
        { set; get; }
        public DateTime CreateDate
        { set; get; }
        public string ModifyBy
        { set; get; }
        public DateTime ModifyDate
        { set; get; }

        public bool InactiveFlag
        { set; get; }

        public Int64 UpdateIndex
        { set; get; }
        public string Language { set; get; }
        public string Culture { set; get; }
    }
}
