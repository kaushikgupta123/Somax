using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PlantLocationTree
{
    public class PlantLocationTreeModel
    {
        public long ParentId;
        public long EquipmentId;
        public string ClientLookUpId;
        public long DepartmentId;
        public long LineId;
        public long SystemInfoId;
        //public string Description;
        public string ChargeToType;
        public string DepartmentDescription;
        public string LineDescription;
        public string SystemDescription;
        public string Description;
        public string NodeName;
        public long PlantLocationId;
        public string ChargeType;
        public string Name;

        public List<PlantLocationTreeModel> tempList;
        public List<PlantLocationTreeModel> GetPlantLocationTreeModel()
        {
            List<PlantLocationTreeModel> temp = new List<PlantLocationTreeModel>()
        {
            new PlantLocationTreeModel{EquipmentId=1,ClientLookUpId="1",DepartmentId=1,DepartmentDescription="Dept1"},
            new PlantLocationTreeModel{EquipmentId=2,ClientLookUpId="2",DepartmentId=2,DepartmentDescription="Dept2"},
            new PlantLocationTreeModel{EquipmentId=3,DepartmentId=3,ClientLookUpId="3",DepartmentDescription="Dept3",LineId=1,LineDescription="Line1"},
            new PlantLocationTreeModel{EquipmentId=4,ClientLookUpId="4",DepartmentId=4,DepartmentDescription="Dept4",LineId=2,LineDescription="Line2"},
            new PlantLocationTreeModel{EquipmentId=5,ClientLookUpId="5",DepartmentId=5,DepartmentDescription="Dept5",LineId=3,LineDescription="Line3",SystemInfoId=1,SystemDescription="System1"},
            new PlantLocationTreeModel{EquipmentId=6,ClientLookUpId="6",DepartmentId=6,DepartmentDescription="Dept6",LineId=4,LineDescription="Line4",SystemInfoId=2,SystemDescription="System2"},
        };
            //tempList = temp;
            return temp;
        }


        //    public List<PlantLocationTreeModel> GetPlantLocationTreeModel()
        //    {
        //        List<PlantLocationTreeModel> temp = new List<PlantLocationTreeModel>()
        //{
        //            new PlantLocationTreeModel{EquipmentId=3,DepartmentId=3,ClientLookUpId="3",DepartmentDescription="Dept3",LineId=1,LineDescription="Line1"},
        //    new PlantLocationTreeModel{EquipmentId=1,ClientLookUpId="1",DepartmentId=1,DepartmentDescription="Dept1"},
        //    new PlantLocationTreeModel{EquipmentId=5,ClientLookUpId="5",DepartmentId=5,DepartmentDescription="Dept5",LineId=3,LineDescription="Line3",SystemInfoId=1,SystemDescription="System1"},
        //            new PlantLocationTreeModel{EquipmentId=2,ClientLookUpId="2",DepartmentId=2,DepartmentDescription="Dept2"},

        //    new PlantLocationTreeModel{EquipmentId=4,ClientLookUpId="4",DepartmentId=4,DepartmentDescription="Dept4",LineId=2,LineDescription="Line2"},

        //    new PlantLocationTreeModel{EquipmentId=6,ClientLookUpId="6",DepartmentId=6,DepartmentDescription="Dept6",LineId=4,LineDescription="Line4",SystemInfoId=2,SystemDescription="System2"},
        //};
        //        //tempList = temp;
        //        return temp;
        //    }

    }
}