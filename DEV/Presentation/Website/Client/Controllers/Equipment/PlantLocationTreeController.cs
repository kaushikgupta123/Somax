using Client.Models.InventoryCheckout;
using Client.Models.PurchaseRequest;
using Client.Models.Work_Order;
using Client.Models;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Client.Models.Equipment;
using Client.Models.MasterSanitationSchedule;
using Client.Models.PlantLocationTree;
using Client.Models.PurchaseOrder;

namespace Client.Controllers
{
    public class PlantLocationTreeController : BaseController
    {

        #region
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult PlantLocationTree()
        {
            PlantLocation plantLocation = new PlantLocation()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<PlantLocation> plantLocationList = plantLocation.RetrieveForTreeList(this.userData.DatabaseKey);
            plantLocationList = plantLocationList.Where(x => plantLocationList.Any(y => (x.ParentId == 0 || y.PlantLocationId == x.ParentId &&
                                                        x.InActiveFlag == false))).ToList();
            return PartialView("_PlantLocationTree", plantLocationList);
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult MapPlantLocationTree(long EquipmentId, int plantLocationId)
        {
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            equipment.Retrieve(userData.DatabaseKey);
            equipment.PlantLocationId = Convert.ToInt64(plantLocationId);
            equipment.UpdateForPlantLocation(userData.DatabaseKey);
            return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Plant equipment Hirerchy
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult PlantLocationEquipmentTree()
        {
            PlantLocation plantLocation = new PlantLocation()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId

            };
            List<PlantLocation> plantLocationList = plantLocation.RetrieveForTreeList(this.userData.DatabaseKey);

            List<PlantLocation> pLocationList = plantLocation.RetrieveForTreeList(this.userData.DatabaseKey);
            plantLocationList = pLocationList.Where(x => plantLocationList.Any(y => (x.ParentId == 0 || y.PlantLocationId == x.ParentId &&
                                                        x.InActiveFlag == false))).ToList();
            Equipment EquipmentLists = new Equipment();
            List<Equipment> Equipment = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey);
            List<PlantLocation> plhlist = new List<PlantLocation>();
            List<EquipmentHierarchyLookUp> equip = Equipment
                                                        .Where(x => Equipment.Any(y => (x.PlantLocationId > 0 && x.InactiveFlag == false)))
                                                        .Select(i =>
                                                                new EquipmentHierarchyLookUp
                                                                {
                                                                    ClientLookUpId = i.ClientLookupId,
                                                                    Name = i.Name,
                                                                    EquipmentId = i.EquipmentId,
                                                                    PlantLocationId = i.PlantLocationId
                                                                }).ToList();
            Dictionary<long, EquipmentHierarchyLookUp> Edict = equip.ToDictionary(eloc => eloc.EquipmentId);

            foreach (var pl in plantLocationList)
            {
                PlantLocation plh1 = new PlantLocation();
                {
                    plh1.ParentId = pl.ParentId;
                    plh1.Description = pl.Description;
                    plh1.PlantLocationId = pl.PlantLocationId;
                    plh1.ChargeToType = "PlantLocation";
                    plhlist.Add(plh1);
                }
                foreach (EquipmentHierarchyLookUp Eloc in Edict.Values)
                {
                    PlantLocation plh2 = new PlantLocation();

                    if (Eloc.PlantLocationId == pl.PlantLocationId)
                    {
                        plh2.ParentId = pl.PlantLocationId;
                        plh2.Description = Eloc.ChargeType == "Equipment" ? Eloc.ClientLookUpId + "(" + Eloc.Name + ")" : pl.Description;
                        plh2.PlantLocationId = Eloc.ChargeType == "Equipment" ? Eloc.EquipmentId : pl.PlantLocationId;
                        plh2.ChargeToType = "Equipment";

                        plhlist.Add(plh2);
                    }
                }
            }
            return PartialView("~/Views/SanitationJob/_PlantLocationEquimentTree.cshtml", plhlist);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult PlantLocationClientLookUpEquipmentTree()
        {
            PlantLocation plantLocation = new PlantLocation()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId

            };
            List<PlantLocation> plantLocationList = plantLocation.RetrieveForTreeList(this.userData.DatabaseKey);

            List<PlantLocation> pLocationList = plantLocation.RetrieveForTreeList(this.userData.DatabaseKey);
            plantLocationList = pLocationList.Where(x => plantLocationList.Any(y => (x.ParentId == 0 || y.PlantLocationId == x.ParentId &&
                                                        x.InActiveFlag == false))).ToList();

            Equipment EquipmentLists = new Equipment();
            List<Equipment> Equipment = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey);

            List<MasterSanitationClientLookUpModel> plhlist = new List<MasterSanitationClientLookUpModel>();

            List<EquipmentHierarchyLookUp> equip = Equipment
                                                        .Where(x => Equipment.Any(y => (x.PlantLocationId > 0 && x.InactiveFlag == false)))
                                                        .Select(i =>
                                                                new EquipmentHierarchyLookUp
                                                                {
                                                                    ClientLookUpId = i.ClientLookupId,
                                                                    Name = i.Name,
                                                                    EquipmentId = i.EquipmentId,
                                                                    PlantLocationId = i.PlantLocationId
                                                                }).ToList();
            Dictionary<long, EquipmentHierarchyLookUp> Edict = equip.ToDictionary(eloc => eloc.EquipmentId);

            foreach (var pl in plantLocationList)
            {
                MasterSanitationClientLookUpModel plh1 = new MasterSanitationClientLookUpModel();
                {
                    plh1.ParentId = pl.ParentId;
                    plh1.Description = pl.Description;
                    plh1.PlantLocationId = pl.PlantLocationId;
                    plh1.ClientLookUpId = pl.Description;
                    plh1.ChargeToType = "PlantLocation";
                    plhlist.Add(plh1);
                }
                foreach (EquipmentHierarchyLookUp Eloc in Edict.Values)
                {
                    MasterSanitationClientLookUpModel plh2 = new MasterSanitationClientLookUpModel();

                    if (Eloc.PlantLocationId == pl.PlantLocationId)
                    {
                        plh2.ParentId = pl.PlantLocationId;
                        plh2.Description = Eloc.ChargeType == "Equipment" ? Eloc.ClientLookUpId + "(" + Eloc.Name + ")" : pl.Description;
                        plh2.PlantLocationId = Eloc.ChargeType == "Equipment" ? Eloc.EquipmentId : pl.PlantLocationId;
                        plh2.ClientLookUpId = Eloc.ChargeType == "Equipment" ? Eloc.ClientLookUpId : pl.Description;
                        plh2.ChargeToType = "Equipment";
                        plhlist.Add(plh2);
                    }
                }
            }
            return PartialView("~/Views/MasterSanitationSchedule/_PlantLocationClientLookUpEquipmentTree.cshtml", plhlist);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult MapPlantLocationEquipTree(long SanitationId, int plantLocationId)
        {
            SanitationJob sanitation = new SanitationJob()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SanitationJobId = SanitationId
            };
            sanitation.Retrieve(userData.DatabaseKey);
            sanitation.PlantLocationId = Convert.ToInt64(plantLocationId);
            sanitation.Update(userData.DatabaseKey);
            return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
        }
        public class EquipmentHierarchyLookUp
        {
            public EquipmentHierarchyLookUp()
            {
                this.EquipmentId = 0;
                this.ClientLookUpId = string.Empty;
                this.PlantLocationId = 0;
                this.Level = 0;
                this.InactiveFlag = false;
                this.ChargeType = "Equipment";
                Children = new List<EquipmentHierarchyLookUp>();

            }
            public long PlantLocationId { get; set; }
            public long EquipmentId { get; set; }
            public string ClientLookUpId { get; set; }
            public string Name { get; set; }
            public int Level { get; set; }
            public bool InactiveFlag { get; set; }
            public string ChargeType { get; set; }
            public List<EquipmentHierarchyLookUp> Children { get; set; }
        }
        #endregion

        #region Equipment Tree
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult EquipmentHierarchyTree()
        {
            HashSet<Equipment> P_Equipment = new HashSet<Equipment>();
            Equipment EquipmentLists = new Equipment();
            List<Equipment> equipments = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey)
                .Where(w => w.InactiveFlag == false)
                .OrderBy(o => o.ClientLookupId)
                .ToList();

            foreach (var item in equipments.Where(w => w.ParentId == 0))
            {
                Equipment objEquip = new Equipment();
                objEquip.EquipmentId = item.EquipmentId;
                objEquip.ParentId = item.ParentId;
                objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                P_Equipment.Add(objEquip);
                SetLevel(item, equipments, P_Equipment);
            }
            var x = P_Equipment.ToList();

            Models.InventoryCheckout.InventoryCheckVM inventoryCheckVM = new Models.InventoryCheckout.InventoryCheckVM();
            EquipmentTreeModel objEquipmentTreeModel = new EquipmentTreeModel();
            objEquipmentTreeModel.Children = x;
            inventoryCheckVM.equipmentTreeModel = objEquipmentTreeModel;
            LocalizeControls(inventoryCheckVM, LocalizeResourceSetConstants.PartDetails);
            return PartialView("_EquipmentHierarchyTree", inventoryCheckVM);
        }
        private void SetLevel(Equipment equipment, List<Equipment> eList, HashSet<Equipment> hashSet)
        {
            var x = eList.Where(w => w.ParentId == equipment.EquipmentId).OrderBy(o => o.ClientLookupId);
            if (x == null)
                return;
            else
            {
                foreach (var item in x)
                {
                    Equipment objEquip = new Equipment();
                    objEquip.EquipmentId = item.EquipmentId;
                    objEquip.ParentId = item.ParentId;
                    objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                    hashSet.Add(objEquip);
                    SetLevel(item, eList, hashSet);
                }
            }
            return;
        }

        private void SetLevel(ProcessSystemTreeModel processSystem, List<ProcessSystemTreeModel> processSystemList, HashSet<ProcessSystemTreeModel> hashSet)
        {
            var x = processSystemList.Where(w => w.ParentId == processSystem.Id).OrderBy(o => o.Name);
            if (x == null)
                return;
            else
            {
                foreach (var item in x)
                {
                    hashSet.Add(item);
                    SetLevel(item, processSystemList, hashSet);
                }
            }
            return;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult MapEquipmentHierarchyTree(long _EquipmentId)
        {
            return Json(_EquipmentId, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PurchaseRequest Line Item EqpHierarchy Tree

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult LineItemEquipmentHierarchyTree()
        {
            HashSet<Equipment> P_Equipment = new HashSet<Equipment>();

            Equipment EquipmentLists = new Equipment();
            List<Equipment> equipments = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey)
                .Where(w => w.InactiveFlag == false)
                .OrderBy(o => o.ClientLookupId)
                .ToList();

            foreach (var item in equipments.Where(w => w.ParentId == 0))
            {
                Equipment objEquip = new Equipment();
                objEquip.EquipmentId = item.EquipmentId;
                objEquip.ParentId = item.ParentId;
                objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                P_Equipment.Add(objEquip);
                SetLevel(item, equipments, P_Equipment);
            }
            var x = P_Equipment.ToList();

            PurchaseRequestVM purchaseRequestVM = new PurchaseRequestVM();
            LiEquipmentTreeModel objLiEquipmentTreeModel = new LiEquipmentTreeModel();
            objLiEquipmentTreeModel.Children = x;

            purchaseRequestVM.liEquipmentTreeModel = objLiEquipmentTreeModel;

            LocalizeControls(purchaseRequestVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_PurchaseReqLineItemEquipmentTree", purchaseRequestVM);
        }
        #endregion

        #region WorkOrder EqpHierarchy Tree
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult WorkOrderEquipmentHierarchyTree()
        {
            HashSet<Equipment> P_Equipment = new HashSet<Equipment>();

            Equipment EquipmentLists = new Equipment();
            List<Equipment> equipments = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey)
                .Where(w => w.InactiveFlag == false)
                .OrderBy(o => o.ClientLookupId)
                .ToList();

            foreach (var item in equipments.Where(w => w.ParentId == 0))
            {
                Equipment objEquip = new Equipment();
                objEquip.EquipmentId = item.EquipmentId;
                objEquip.ParentId = item.ParentId;
                objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                P_Equipment.Add(objEquip);
                SetLevel(item, equipments, P_Equipment);
            }
            var x = P_Equipment.ToList();
            WorkOrderVM workOrderVM = new WorkOrderVM();
            Models.Work_Order.WoEquipmentTreeModel objWoEquipmentTreeModel = new Models.Work_Order.WoEquipmentTreeModel();
            objWoEquipmentTreeModel.Children = x;
            workOrderVM.woEquipmentTreeModel = objWoEquipmentTreeModel;
            LocalizeControls(workOrderVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_WorkOrderEquipmentTree", workOrderVM);
        }
        #endregion
        #region WorkOrder EqpHierarchy Tree for 611
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult WorkOrderEquipmentHierarchyTreeDynamic()
        {
            HashSet<Equipment> P_Equipment = new HashSet<Equipment>();

            Equipment EquipmentLists = new Equipment();
            List<Equipment> equipments = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey)
                .Where(w => w.InactiveFlag == false)
                .OrderBy(o => o.ClientLookupId)
                .ToList();

            foreach (var item in equipments.Where(w => w.ParentId == 0))
            {
                Equipment objEquip = new Equipment();
                objEquip.EquipmentId = item.EquipmentId;
                objEquip.ParentId = item.ParentId;
                objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                P_Equipment.Add(objEquip);
                SetLevel(item, equipments, P_Equipment);
            }
            var x = P_Equipment.ToList();
            WorkOrderVM workOrderVM = new WorkOrderVM();
            Models.Work_Order.WoEquipmentTreeModel objWoEquipmentTreeModel = new Models.Work_Order.WoEquipmentTreeModel();
            objWoEquipmentTreeModel.Children = x;
            workOrderVM.woEquipmentTreeModel = objWoEquipmentTreeModel;
            LocalizeControls(workOrderVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_WorkOrderEquipmentTreeDynamic", workOrderVM);
        }
        #endregion

        #region Equipment EqpHierarchy Tree
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult EqEquipmentHierarchyTree()
        {
            HashSet<Equipment> P_Equipment = new HashSet<Equipment>();
            Equipment EquipmentLists = new Equipment();
            List<Equipment> equipments = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey)
                .Where(w => w.InactiveFlag == false)
                .OrderBy(o => o.ClientLookupId)
                .ToList();

            foreach (var item in equipments.Where(w => w.ParentId == 0))
            {
                Equipment objEquip = new Equipment();
                objEquip.EquipmentId = item.EquipmentId;
                objEquip.ParentId = item.ParentId;
                objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                P_Equipment.Add(objEquip);
                SetLevel(item, equipments, P_Equipment);
            }
            var x = P_Equipment.ToList();
            EquipmentCombined objComb = new EquipmentCombined();
            EqEquipmentTreeModel eqEquipmentTreeModel = new EqEquipmentTreeModel();
            eqEquipmentTreeModel.Children = x;
            objComb.eqEquipmentTreeModel = eqEquipmentTreeModel;
            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_EquipmentTree", objComb);
        }
        
        #endregion

        #region WorkOrder ProcessSystem tree
        public ActionResult ProcessSystemTree()
        {
            //fetch all record from equipment list
            Equipment EquipmentLists = new Equipment();
            List<Equipment> Equipments = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey)//.Where(w=> w.InactiveFlag == false)
                .ToList();

            //Generate Tree  – the Process/System Equipment Hierarchy Tree 
            ProcessSystem processSystem = new ProcessSystem()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };

            List<ProcessSystem> processSystemList = processSystem.RetrieveForTreeList(userData.DatabaseKey);

            processSystemList.ForEach(p =>
            {
                p.Process = p.Process + "-desc";
            }
            );

            int i = 1000000000;
            List<ProcessSystemTreeModel> finalProcessSystemTreeModelList = new List<ProcessSystemTreeModel>();
            List<ProcessSystemTreeModel> tempProcessSystemTreeModelList = new List<ProcessSystemTreeModel>();

            // Listing all distinct Process from ProcessSystem table and adding Temporary Id
            processSystemList
                .GroupBy(g => g.Process)
                .Select(x => x.First())
                .ToList()
                .ForEach(e =>
                {
                    ProcessSystemTreeModel mdl = new ProcessSystemTreeModel();

                    mdl.Id = "processid" + i.ToString();
                    mdl.Name = e.Process;
                    mdl.ParentId = "0";
                    mdl.ProcessSystemId = e.ProcessSystemId;

                    finalProcessSystemTreeModelList.Add(mdl);

                    i++;
                });

            // Creating a Temporary List to add elements to the main list, as in loop couldn't modify the enumerated elementes
            tempProcessSystemTreeModelList = finalProcessSystemTreeModelList.Select(s => s).ToList();

            // Finding and Adding linked Systems of Processes
            tempProcessSystemTreeModelList
                .GroupJoin(
                processSystemList,
                k1 => k1.Name,
                k2 => k2.Process,
                (p, q) => new
                {
                    x = q.Select(s => new ProcessSystemTreeModel
                    {
                        Id = "systemid" + s.ProcessSystemId.ToString(),
                        Name = s.System + "-desc",
                        ParentId = p.Id,
                        TempId = s.ProcessSystemId,
                        ProcessSystemId = s.ProcessSystemId
                    })
                })
                .SelectMany(s => s.x)
                .ToList()
                .ForEach(e =>
                {
                    ProcessSystemTreeModel mdl = new ProcessSystemTreeModel();

                    mdl.Id = e.Id;
                    mdl.Name = e.Name;
                    mdl.ParentId = e.ParentId;
                    mdl.TempId = e.TempId;

                    finalProcessSystemTreeModelList.Add(mdl);

                    i++;
                });

            // Clearing the temp list
            // Again populate it for tempory purpose to loop through and Add linked Equiepments of System in the finalList/ main list 
            tempProcessSystemTreeModelList.Clear();
            tempProcessSystemTreeModelList = finalProcessSystemTreeModelList
                .Where(w => w.ParentId != "0")
                .Select(s => s)
                .ToList();

            var equip = tempProcessSystemTreeModelList
                .Select(s1 =>
                {
                    List<ProcessSystemTreeModel> ProcessSystemTreeModelList = new List<ProcessSystemTreeModel>();

                    var equipments = Equipments
                    .Where(w => w.ProcessSystemId > 0 && w.ProcessSystemId == s1.TempId)
                    .Select(s2 => new Equipment
                    {
                        EquipmentId = s2.EquipmentId,
                        ParentId = s2.ParentId,
                        ClientLookupId = s2.ClientLookupId + " ( " + s2.Name + " )"
                    })
                    .ToList();

                    if (equipments == null)
                    {
                        ProcessSystemTreeModelList.Add(new ProcessSystemTreeModel
                        {
                            Id = s1.Id,
                            Name = s1.Name,
                            ParentId = s1.ParentId,
                            TempId = s1.TempId
                        });

                        return ProcessSystemTreeModelList;
                    }

                    foreach (var equipment in equipments)
                    {
                        HashSet<Equipment> P_Equipment = new HashSet<Equipment>();

                        ProcessSystemTreeModel objProcessSystemTreeModel = new ProcessSystemTreeModel();
                        objProcessSystemTreeModel.Id = equipment.EquipmentId.ToString();
                        objProcessSystemTreeModel.ParentId = s1.Id;
                        objProcessSystemTreeModel.TempId = equipment.EquipmentId;
                        objProcessSystemTreeModel.Name = equipment.ClientLookupId + " ( " + equipment.Name + " )";
                        ProcessSystemTreeModelList.Add(objProcessSystemTreeModel);

                        SetLevel(equipment, Equipments, P_Equipment);

                        ProcessSystemTreeModelList.AddRange(P_Equipment.Select(s => new ProcessSystemTreeModel
                        {
                            Id = s.EquipmentId.ToString(),
                            Name = s.ClientLookupId + " ( " + s.Name + " )",
                            ParentId = s.ParentId.ToString(),
                            TempId = equipment.EquipmentId
                        }).ToList());
                    }

                    return ProcessSystemTreeModelList;
                })
                .SelectMany(s => s);
            finalProcessSystemTreeModelList.AddRange(equip);

            // Ordering the Hierarchy 
            HashSet<ProcessSystemTreeModel> hashSet = new HashSet<ProcessSystemTreeModel>();

            foreach (var proccessSystem in finalProcessSystemTreeModelList.Where(w => w.ParentId == "0"))
            {
                hashSet.Add(proccessSystem);
                SetLevel(proccessSystem, finalProcessSystemTreeModelList, hashSet);
            }

            // Final List
            EquipmentCombined objComb = new EquipmentCombined();
            objComb.processSystemTreeModel = hashSet
                                             .Select(s => new ProcessSystemTreeModel
                                             {
                                                 Id = s.Id,
                                                 Name = s.Name,
                                                 ParentId = s.ParentId,
                                                 TempId = s.TempId
                                             })
                                            .ToList();

            LocalizeControls(objComb, LocalizeResourceSetConstants.EquipmentDetails);
            return PartialView("_ProcessSystemTree", objComb);
        }
        #endregion

        #region PlantLocation Tree
        public ActionResult PlantLocationTreeLookup()
        {
            HashSet<PlantLocationTreeModel> P_Department = new HashSet<PlantLocationTreeModel>();
            HashSet<PlantLocationTreeModel> P_Line = new HashSet<PlantLocationTreeModel>();
            HashSet<PlantLocationTreeModel> P_System = new HashSet<PlantLocationTreeModel>();
            PlantLocationTreeModel ptm = new PlantLocationTreeModel();

            //----------------------------------------------------
            PlantLocationTreeModel ptmDb = new PlantLocationTreeModel();
            Equipment obj = new Equipment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
            };
            var dataList = obj.Equipment_GetAllDeptLineSys(this.userData.DatabaseKey);         
            List<PlantLocationTreeModel> plTree = new List<PlantLocationTreeModel>();
            PlantLocationTreeModel objPlantLocationTreeModel;
            foreach (var v in dataList)
            {
                objPlantLocationTreeModel = new PlantLocationTreeModel();
                objPlantLocationTreeModel.ClientLookUpId = v.ClientLookupId;
                objPlantLocationTreeModel.EquipmentId = v.EquipmentId;
                objPlantLocationTreeModel.DepartmentDescription = v.Dept_Desc;
                objPlantLocationTreeModel.LineDescription = v.Line_Desc;
                objPlantLocationTreeModel.SystemDescription = v.System_Desc;
                objPlantLocationTreeModel.DepartmentId = v.DepartmentId;
                objPlantLocationTreeModel.LineId = v.LineId;
                objPlantLocationTreeModel.SystemInfoId = v.SystemInfoId;
                objPlantLocationTreeModel.ParentId = v.ParentId;
                plTree.Add(objPlantLocationTreeModel);
            }
            foreach (var v in plTree)
            {
                PlantLocationTreeModel objEquip = new PlantLocationTreeModel();
                SetLevel(v);
                objEquip.EquipmentId = v.EquipmentId;
                objEquip.ParentId = v.ParentId;
                objEquip.Description = v.Description;
                objEquip.ClientLookUpId = v.ClientLookUpId;             
             
                if (objEquip.ParentId == 0)
                {
                    objEquip.NodeName = "Department";
                    P_Department.Add(objEquip);
                }
                else if(objEquip.ParentId == 1)
                {
                    objEquip.NodeName = "Line";
                    P_Line.Add(objEquip);
                }
                else if (objEquip.ParentId == 2)
                {
                    objEquip.NodeName = "System";
                    P_System.Add(objEquip);
                }                
            }
             P_Department.UnionWith(P_Line);
            P_Department.UnionWith(P_System);          
            var plist = P_Department                        
                        .ToList();
            return PartialView("_PlantLocationLookupTree", plist);
        }
        public ActionResult SanitationPlantLocationTreeLookup()
        {
            HashSet<PlantLocationTreeModel> P_Department = new HashSet<PlantLocationTreeModel>();
            HashSet<PlantLocationTreeModel> P_Line = new HashSet<PlantLocationTreeModel>();
            HashSet<PlantLocationTreeModel> P_System = new HashSet<PlantLocationTreeModel>();
            PlantLocationTreeModel ptm = new PlantLocationTreeModel();
           
            PlantLocationTreeModel ptmDb = new PlantLocationTreeModel();
            Equipment obj = new Equipment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
            };
            var dataList = obj.Equipment_GetAllDeptLineSys(this.userData.DatabaseKey);
            List<PlantLocationTreeModel> plTree = new List<PlantLocationTreeModel>();
            PlantLocationTreeModel objPlantLocationTreeModel;
            foreach (var v in dataList)
            {
                objPlantLocationTreeModel = new PlantLocationTreeModel();
                objPlantLocationTreeModel.ClientLookUpId = v.ClientLookupId;
                objPlantLocationTreeModel.EquipmentId = v.EquipmentId;
                objPlantLocationTreeModel.DepartmentDescription = v.Dept_Desc;
                objPlantLocationTreeModel.LineDescription = v.Line_Desc;
                objPlantLocationTreeModel.SystemDescription = v.System_Desc;
                objPlantLocationTreeModel.DepartmentId = v.DepartmentId;
                objPlantLocationTreeModel.LineId = v.LineId;
                objPlantLocationTreeModel.SystemInfoId = v.SystemInfoId;
                objPlantLocationTreeModel.ParentId = v.ParentId;
                objPlantLocationTreeModel.Name = v.Name;
                plTree.Add(objPlantLocationTreeModel);
            }
            foreach (var v in plTree)
            {
                PlantLocationTreeModel objEquip = new PlantLocationTreeModel();
                SetLevel(v);
                objEquip.EquipmentId = v.EquipmentId;
                objEquip.ParentId = v.ParentId;
                objEquip.Description = v.Description;
                objEquip.ClientLookUpId = v.ClientLookUpId;

                if (objEquip.ParentId == 0)
                {
                    objEquip.NodeName = "Department";
                    P_Department.Add(objEquip);
                }
                else if (objEquip.ParentId == 1)
                {
                    objEquip.NodeName = "Line";
                    P_Line.Add(objEquip);
                }
                else if (objEquip.ParentId == 2)
                {
                    objEquip.NodeName = "System";
                    P_System.Add(objEquip);
                }
            }
            P_Department.UnionWith(P_Line);
            P_Department.UnionWith(P_System);
            var plist = P_Department
                        .ToList();
            return PartialView("_SanitationPlantLocationLookupTree", plist);
        }
        public ActionResult WRPlantLocationTreeLookup()
        {
            HashSet<PlantLocationTreeModel> P_Department = new HashSet<PlantLocationTreeModel>();
            HashSet<PlantLocationTreeModel> P_Line = new HashSet<PlantLocationTreeModel>();
            HashSet<PlantLocationTreeModel> P_System = new HashSet<PlantLocationTreeModel>();
            PlantLocationTreeModel ptm = new PlantLocationTreeModel();

            PlantLocationTreeModel ptmDb = new PlantLocationTreeModel();
            Equipment obj = new Equipment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
            };
            var dataList = obj.Equipment_GetAllDeptLineSys(this.userData.DatabaseKey);
            List<PlantLocationTreeModel> plTree = new List<PlantLocationTreeModel>();
            PlantLocationTreeModel objPlantLocationTreeModel;
            foreach (var v in dataList)
            {
                objPlantLocationTreeModel = new PlantLocationTreeModel();
                objPlantLocationTreeModel.ClientLookUpId = v.ClientLookupId;
                objPlantLocationTreeModel.EquipmentId = v.EquipmentId;
                objPlantLocationTreeModel.DepartmentDescription = v.Dept_Desc;
                objPlantLocationTreeModel.LineDescription = v.Line_Desc;
                objPlantLocationTreeModel.SystemDescription = v.System_Desc;
                objPlantLocationTreeModel.DepartmentId = v.DepartmentId;
                objPlantLocationTreeModel.LineId = v.LineId;
                objPlantLocationTreeModel.SystemInfoId = v.SystemInfoId;
                objPlantLocationTreeModel.ParentId = v.ParentId;
                plTree.Add(objPlantLocationTreeModel);
            }
            foreach (var v in plTree)
            {
                PlantLocationTreeModel objEquip = new PlantLocationTreeModel();
                SetLevel(v);
                objEquip.EquipmentId = v.EquipmentId;
                objEquip.ParentId = v.ParentId;
                objEquip.Description = v.Description;
                objEquip.ClientLookUpId = v.ClientLookUpId;

                if (objEquip.ParentId == 0)
                {
                    objEquip.NodeName = "Department";
                    P_Department.Add(objEquip);
                }
                else if (objEquip.ParentId == 1)
                {
                    objEquip.NodeName = "Line";
                    P_Line.Add(objEquip);
                }
                else if (objEquip.ParentId == 2)
                {
                    objEquip.NodeName = "System";
                    P_System.Add(objEquip);
                }
            }
            P_Department.UnionWith(P_Line);
            P_Department.UnionWith(P_System);
            var plist = P_Department
                        .ToList();
            return PartialView("_WorkRequestPlantLocationLookupTree", plist);
        }
        private void SetLevel(PlantLocationTreeModel item)
        {
            HashSet<PlantLocationTreeModel> hashSet = new HashSet<PlantLocationTreeModel>();
            if (item.EquipmentId > 0)
            {
                if (item.DepartmentId > 0)
                {
                    if (item.LineId > 0)
                    {
                        if (item.SystemInfoId > 0)
                        {
                            item.ParentId = 2;
                            item.ClientLookUpId = item.ClientLookUpId + "(" + item.SystemDescription + ")";
                            item.Description = item.SystemDescription;
                           // item.NodeName = "System";
                            hashSet.Add(item);
                        }
                        else
                        {
                            item.ParentId = 1;
                            item.ClientLookUpId = item.ClientLookUpId + "(" + item.LineDescription + ")";
                            item.Description = item.LineDescription;
                           // item.NodeName = "Line";
                            hashSet.Add(item);
                        }
                    }
                    else
                    {
                        item.ParentId = 0;
                        item.ClientLookUpId = item.ClientLookUpId + "(" + item.DepartmentDescription + ")";
                        item.Description = item.DepartmentDescription;
                        //item.NodeName = "Department";
                        hashSet.Add(item);
                    }
                }
            }
        }
        #endregion

        #region Santitaion Asset Tree
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult SanitationEquipmentHierarchyTree()
        {
            HashSet<Equipment> P_Equipment = new HashSet<Equipment>();

            Equipment EquipmentLists = new Equipment();
            List<Equipment> equipments = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey)
                .Where(w => w.InactiveFlag == false)
                .OrderBy(o => o.ClientLookupId)
                .ToList();

            foreach (var item in equipments.Where(w => w.ParentId == 0))
            {
                Equipment objEquip = new Equipment();
                objEquip.EquipmentId = item.EquipmentId;
                objEquip.ParentId = item.ParentId;
                objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                P_Equipment.Add(objEquip);
                SetLevel(item, equipments, P_Equipment);
            }
            var x = P_Equipment.ToList();
            WorkOrderVM workOrderVM = new WorkOrderVM();
            Models.Work_Order.WoEquipmentTreeModel objWoEquipmentTreeModel = new Models.Work_Order.WoEquipmentTreeModel();
            objWoEquipmentTreeModel.Children = x;
            workOrderVM.woEquipmentTreeModel = objWoEquipmentTreeModel;
            LocalizeControls(workOrderVM, LocalizeResourceSetConstants.PurchaseRequest);
            return PartialView("_SanitationEquipmentTree", workOrderVM);
        }
        #endregion

        #region PurchaseOrder Line Item EqpHierarchy Tree

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult POLineItemEquipmentHierarchyTree()
        {
            HashSet<Equipment> P_Equipment = new HashSet<Equipment>();

            Equipment EquipmentLists = new Equipment();
            List<Equipment> equipments = EquipmentLists.GetAllEquipment(this.userData.DatabaseKey)
                .Where(w => w.InactiveFlag == false)
                .OrderBy(o => o.ClientLookupId)
                .ToList();

            foreach (var item in equipments.Where(w => w.ParentId == 0))
            {
                Equipment objEquip = new Equipment();
                objEquip.EquipmentId = item.EquipmentId;
                objEquip.ParentId = item.ParentId;
                objEquip.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                P_Equipment.Add(objEquip);
                SetLevel(item, equipments, P_Equipment);
            }
            var x = P_Equipment.ToList();

            PurchaseOrderVM purchaseRequestVM = new PurchaseOrderVM();
            POLiEquipmentTreeModel objLiEquipmentTreeModel = new POLiEquipmentTreeModel();
            objLiEquipmentTreeModel.Children = x;

            purchaseRequestVM.pOLiEquipmentTreeModel = objLiEquipmentTreeModel;

            LocalizeControls(purchaseRequestVM, LocalizeResourceSetConstants.PurchaseOrder);
            return PartialView("_PurchaseOrderLineItemEquipmentTree", purchaseRequestVM);
        }
        #endregion
    }
}