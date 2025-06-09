using Client.Models.Configuration.EquipmentMaster;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.Configuration.EquipmentMaster
{
    public class EquipmentMasterWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public EquipmentMasterWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<EquipmentMasterModel> GetEquipmentMasterData(bool inactiveFlag)
        {
            DataContracts.EquipmentMaster equipmentMaster = new DataContracts.EquipmentMaster();
            equipmentMaster.ClientId = userData.DatabaseKey.Client.ClientId;
            List<DataContracts.EquipmentMaster> equipmentList = equipmentMaster.EquipmentMaster_RetrieveAll(this.userData.DatabaseKey).Where(x => x.InactiveFlag == inactiveFlag).ToList();
            List<EquipmentMasterModel> EquipmentMasterModelList = new List<EquipmentMasterModel>();
            EquipmentMasterModel objEquipmentMasterModel;
            foreach(var v in equipmentList)
            {
                objEquipmentMasterModel = new EquipmentMasterModel();
                objEquipmentMasterModel.EquipmentMasterId =v.EquipmentMasterId;
                objEquipmentMasterModel.Name =v.Name??"";
                objEquipmentMasterModel.Make =v.Make??"";
                objEquipmentMasterModel.Model =v.Model??"";
                objEquipmentMasterModel.Type =v.Type??"";
                objEquipmentMasterModel.InactiveFlag =v.InactiveFlag;
                EquipmentMasterModelList.Add(objEquipmentMasterModel);
            }
            return EquipmentMasterModelList;
        }
        #endregion Search

        #region Details
        public EquipmentMasterModel GetEquipmentMasterDetails(long equipmentMasterId)
        {
            EquipmentMasterModel objEquipmentMasterModel = new EquipmentMasterModel();
            DataContracts.EquipmentMaster equip = new DataContracts.EquipmentMaster()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EquipmentMasterId = equipmentMasterId
            };
            equip.Retrieve(userData.DatabaseKey);
            objEquipmentMasterModel.EquipmentMasterId = equip.EquipmentMasterId;
            objEquipmentMasterModel.Name = equip.Name;
            objEquipmentMasterModel.Make = equip.Make;
            objEquipmentMasterModel.Model = equip.Model;
            objEquipmentMasterModel.Type = equip.Type;
            objEquipmentMasterModel.InactiveFlag = equip.InactiveFlag;
            objEquipmentMasterModel.UpdateIndex = equip.UpdateIndex;
            return objEquipmentMasterModel;
        }

        public List<DataContracts.LookupList> GetTypeList()
        {
            List<DataContracts.LookupList> objType = new Models.LookupList().RetrieveAll(userData.DatabaseKey).Where(x => x.ListName == LookupListConstants.EQUIP_TYPE).ToList();
            return objType;
        }
        #endregion Details

        #region Add_Edit EqpMaster
        public List<String> AddOrEditEqpMaster(EquipmentMasterModel equipmentMasterModel, ref string Mode, ref long equpMasterId)
        {
            DataContracts.EquipmentMaster em = new DataContracts.EquipmentMaster();
            if (equipmentMasterModel.EquipmentMasterId == 0)
            {
                Mode = "add";
                em.Name = equipmentMasterModel.Name;
                em.Type = equipmentMasterModel.Type ?? string.Empty;
                em.Model = equipmentMasterModel.Model ?? string.Empty;
                em.Make = equipmentMasterModel.Make ?? string.Empty;
                em.createEquipmentMaster(this.userData.DatabaseKey);
                if (em.ErrorMessages.Count == 0)
                {
                    equpMasterId = em.EquipmentMasterId;
                }
                return em.ErrorMessages;
            }
            else
            {
                Mode = "update";
                DataContracts.EquipmentMaster emEdit = new DataContracts.EquipmentMaster()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EquipmentMasterId = equipmentMasterModel.EquipmentMasterId,
                };
                emEdit.Retrieve(userData.DatabaseKey);
                emEdit.Name = equipmentMasterModel.Name;
                emEdit.Type = equipmentMasterModel.Type ?? string.Empty;
                emEdit.Model = equipmentMasterModel.Model ?? string.Empty;
                emEdit.Make = equipmentMasterModel.Make ?? string.Empty;
                emEdit.InactiveFlag = equipmentMasterModel.InactiveFlag;
                // Validation 
                emEdit.SaveValidation(userData.DatabaseKey);
                if (emEdit.IsValid)
                {
                    emEdit.Update(userData.DatabaseKey);
                }
                if (emEdit.ErrorMessages.Count == 0)
                {
                    equpMasterId = emEdit.EquipmentMasterId;
                }
                return emEdit.ErrorMessages;
            }           
        }
        #endregion Add_Edit EqpMaster

        #region PrevMnt
        public List<EquipmentMasterPmModel> GetPmData(long equipmentMasterId)
        {
            DataContracts.EQMaster_PMLibrary eqm = new DataContracts.EQMaster_PMLibrary()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EQMasterId = equipmentMasterId
            };
            var dataSource = EQMaster_PMLibrary.RetrieveListByEQMasterId(userData.DatabaseKey, eqm);
            List<EquipmentMasterPmModel> EquipmentMasterPmModelList = new List<EquipmentMasterPmModel>();
            EquipmentMasterPmModel objEquipmentMasterPmModel;
            foreach(var v in dataSource)
            {
                objEquipmentMasterPmModel = new EquipmentMasterPmModel();
                objEquipmentMasterPmModel.ClientLookupId =v.ClientLookupId;
                objEquipmentMasterPmModel.Description =v.Description;
                objEquipmentMasterPmModel.Frequency =v.Frequency;
                objEquipmentMasterPmModel.FrequencyType =v.FrequencyType;
                objEquipmentMasterPmModel.PMLibraryId =v.PMLibraryId;
                objEquipmentMasterPmModel.EQMaster_PMLibraryId =v.EQMaster_PMLibraryId;
                objEquipmentMasterPmModel.EQMasterId =v.EQMasterId;
                objEquipmentMasterPmModel.UpdateIndex =v.UpdateIndex;
                EquipmentMasterPmModelList.Add(objEquipmentMasterPmModel);
            }
            return EquipmentMasterPmModelList;
        }

        public List<EquipmentMasterPmModel> GetPmList()
        {
            //DataContracts.LookupListResultSet.PrevMaintLibraryLookupListResultSet PMList
            //        = new DataContracts.LookupListResultSet.PrevMaintLibraryLookupListResultSet();
            //PMList.RetrieveResults();
            return null;
        }

        internal bool DeleteEqPm(long eQMaster_PMLibraryId)
        {
            try
            {
                EQMaster_PMLibrary EQP = new EQMaster_PMLibrary()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    EQMaster_PMLibraryId = eQMaster_PMLibraryId
                };
                EQP.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EqMastPMGridDataModel> PopulatePMIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string ClientLookupId, string Description)
        {
            EqMastPMGridDataModel objEqMastPMGridDataModel;
            List<EqMastPMGridDataModel> EqMastPMGridDataModelList = new List<EqMastPMGridDataModel>();


            DataContracts.LookupListResultSet.PrevMaintLibraryLookupListResultSet PMList
                = new DataContracts.LookupListResultSet.PrevMaintLibraryLookupListResultSet();

            DataContracts.LookupListResultSet.PrevMaintLibraryLookupListResultSetTransactionParameters settings = new DataContracts.LookupListResultSet.PrevMaintLibraryLookupListResultSetTransactionParameters()
            {
                PageNumber = pageNumber, // Note that the displayed page number is 1 through N, but the stored proc assumes page 0 is the first page.
                ResultsPerPage = ResultsPerPage,//this.userData.DatabaseKey.User.ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };

            settings.ClientLookupId = ClientLookupId;
            settings.Description = Description;

            PMList.UpdateSettings(userData.DatabaseKey, settings);
            PMList.RetrieveResults();
            
            foreach (var item in PMList.Items)
            {
                objEqMastPMGridDataModel = new EqMastPMGridDataModel();
                objEqMastPMGridDataModel.PMLibraryId = item.PrevMaintLibraryId;
                objEqMastPMGridDataModel.ClientLookupId = item.ClientLookupId;
                objEqMastPMGridDataModel.Description = item.Description;
                objEqMastPMGridDataModel.TotalCount = PMList.Count;
                EqMastPMGridDataModelList.Add(objEqMastPMGridDataModel);
            }
            return EqMastPMGridDataModelList;

        }      

        public List<string> AddEqPm(EquipmentMasterPmModel equipmentMasterPMModel, ref string Mode)
        {
            EQMaster_PMLibrary EQPM = new EQMaster_PMLibrary();
            if (equipmentMasterPMModel.EQMaster_PMLibraryId == 0)
            {
                Mode = "add";
                EQPM.ClientId = userData.DatabaseKey.Client.ClientId;
                EQPM.EQMasterId = equipmentMasterPMModel.EQMasterId;
                EQPM.ClientLookupId = equipmentMasterPMModel.ClientLookupId;
                EQPM.CreateByFK(userData.DatabaseKey);
            }
            else
            {
                Mode = "update";
                EQMaster_PMLibrary eqPM = new EQMaster_PMLibrary()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    EQMasterId = equipmentMasterPMModel.EQMasterId,
                    EQMaster_PMLibraryId = equipmentMasterPMModel.EQMaster_PMLibraryId
                };
                eqPM.Retrieve(userData.DatabaseKey);
                eqPM.UpdateByFK(userData.DatabaseKey);
            }
           
            return EQPM.ErrorMessages;
        }

        public List<string> EditEqPm(EquipmentMasterPmModel equipmentMasterPMModel, ref string Mode)
        {
            EQMaster_PMLibrary eqPM = new EQMaster_PMLibrary()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EQMasterId = equipmentMasterPMModel.EQMasterId,
                EQMaster_PMLibraryId = equipmentMasterPMModel.EQMaster_PMLibraryId
            };

            eqPM.Retrieve(userData.DatabaseKey);
            //grdEQMPMLibrary_RetrievePageControls(eqPM);

            eqPM.UpdateByFK(userData.DatabaseKey);
            return eqPM.ErrorMessages;
        }
        #endregion PrevMnt
    }
}