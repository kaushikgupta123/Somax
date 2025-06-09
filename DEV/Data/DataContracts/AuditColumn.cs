using System;
using System.Xml.Serialization;
using Common.Extensions;

namespace DataContracts
{

    [Serializable]
    [XmlRoot("Column")]
    [XmlType("Column")]
    public class AuditColumn
    {
        [XmlAttribute]
        public string Name { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        #region Public Methods
        public string GetOldValue(DatabaseKey dbKey)
        {
            return GetActualText(dbKey, OldValue);
        }

        public string GetNewValue(DatabaseKey dbKey)
        {
            return GetActualText(dbKey, NewValue);
        }
        #endregion
        

        #region Private Methods
        private string GetActualText(DatabaseKey dbKey, string value)
        {
            string text = string.Empty;

            switch (Name.ToLower())
            {
                case "siteid":
                    Site site = new Site() { SiteId = value.ToLong() };
                    site.Retrieve(dbKey);
                    text = site.FullName;
                    break;
                case "areaid":
                    Area area = new Area() { AreaId = value.ToLong() };
                    area.Retrieve(dbKey);
                    text = area.Description;
                    break;
                case "departmentid":
                    Department department = new Department() { DepartmentId = value.ToLong() };
                    department.Retrieve(dbKey);
                    text = department.Description;
                    break;
                case "storeroomid":
                    Storeroom storeroom = new Storeroom() { StoreroomId = value.ToLong() };
                    storeroom.Retrieve(dbKey);
                    text = storeroom.FullName;
                    break;
                default:
                    text = value;
                    break;
            }

            return text;
        }
        #endregion
        
    }

}