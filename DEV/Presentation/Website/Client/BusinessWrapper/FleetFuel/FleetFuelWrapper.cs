using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Common.Constants;
using Client.Models.FleetFuel;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
namespace Client.BusinessWrapper.FleetFuel
{
    public class FleetFuelWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public FleetFuelWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search
        public List<FleetFuelSearchModel> GetFleetFuelGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string name = "", string make = "", string model = "", string vIN = "", string startreadingDate = "", string endreadingDate = "", string searchText = "")
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            FleetFuelSearchModel fleetFuelSearchModel;
            List<FleetFuelSearchModel> fleetFuelSearchModelList = new List<FleetFuelSearchModel>();
            List<Equipment> equipmentList = new List<Equipment>();
            Equipment equipment = new Equipment();
            equipment.ClientId = this.userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.OrderbyColumn = orderbycol;
            equipment.OrderBy = orderDir;
            equipment.OffSetVal = skip;
            equipment.NextRow = length;
            equipment.ClientLookupId = clientLookupId;
            equipment.Name = name;
            equipment.Make = make;
            equipment.Model = model;
            equipment.VIN = vIN;
            equipment.ReadingStartDate = startreadingDate;
            equipment.ReadingEndDate = endreadingDate;
            equipment.SearchText = searchText;
            equipmentList = equipment.FleetFuelRetrieveChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);
            bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            foreach (var item in equipmentList)
            {
                fleetFuelSearchModel = new FleetFuelSearchModel();
                fleetFuelSearchModel.EquipmentId = item.EquipmentId;
                fleetFuelSearchModel.FuelTrackingId = item.FuelTrackingId;
                fleetFuelSearchModel.Void = item.Void;
                fleetFuelSearchModel.ClientLookupId = item.ClientLookupId;
                fleetFuelSearchModel.ImageUrl = !string.IsNullOrEmpty(item.EquipImage) ? item.EquipImage : commonWrapper.GetNoImageUrl();
                if (ClientOnPremise)
                {
                    fleetFuelSearchModel.ImageUrl = UtilityFunction.PhotoBase64ImgSrc(fleetFuelSearchModel.ImageUrl);
                }
                fleetFuelSearchModel.Name = item.Name;
                fleetFuelSearchModel.VIN = item.VIN;
                fleetFuelSearchModel.Make = item.Make;
                fleetFuelSearchModel.Model = item.Model;
                fleetFuelSearchModel.ReadingDate = item.ReadingDate;
                fleetFuelSearchModel.FuelAmount = item.FuelAmount;
                fleetFuelSearchModel.FuelUnits = item.FuelUnits;
                fleetFuelSearchModel.UnitCost = item.UnitCost;
                fleetFuelSearchModel.TotalCost = item.TotalCost;
                fleetFuelSearchModel.TotalCount = item.TotalCount;
                fleetFuelSearchModel.FleetMeterReadingId = item.FleetMeterReadingId;
                fleetFuelSearchModel.Reading = item.Reading;
                fleetFuelSearchModelList.Add(fleetFuelSearchModel);
            }

            return fleetFuelSearchModelList;
        }
        #endregion
        #region Add Or Edit 
        public Equipment AddOrEditFleetFuel(string FF_ClientLookupId, FleetFuelVM objFTM)
        {
            FleetFuelModel objFleetAsset = new FleetFuelModel();
            Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = FF_ClientLookupId };
            equipment.RetrieveByClientLookupId(_dbKey);
            var date = objFTM.FleetFuelModel.MtrCurrentReadingDate;
            DateTime FFDateTime = DateTime.ParseExact(date + " " + objFTM.FleetFuelModel.StartTimeValue, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
            List<string> errList = new List<string>();
            equipment.Reading = objFTM.FleetFuelModel.Reading;
            if (objFTM.FleetFuelModel.Void == false)
            {
                if (objFTM.FleetFuelModel.FuelTrackingId == 0  || equipment.Meter1CurrentReading != objFTM.FleetFuelModel.FltMrtReading || objFTM.FleetFuelModel.FltMrtReading > objFTM.FleetFuelModel.Reading)
                {
                    
                    equipment.CheckMeter1CurrentReading(userData.DatabaseKey);
                }
               
            }
            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
            {
                return equipment;
            }
            else 
            {
                if (objFTM.FleetFuelModel.FuelTrackingId == 0)
                {                  

                    #region Insert in Fleet Fuel Tracking Table
                    FuelTracking fuelTracking = new FuelTracking();
                    fuelTracking.ClientId = this.userData.DatabaseKey.User.ClientId;
                    fuelTracking.EquipmentId = equipment.EquipmentId;
                    fuelTracking.FuelType = objFTM.FleetFuelModel.FuelType;
                    fuelTracking.FuelAmount = objFTM.FleetFuelModel.FuelAmount;
                    fuelTracking.FuelUnit = objFTM.FleetFuelModel.FuelUnit;
                    fuelTracking.UnitCost = objFTM.FleetFuelModel.UnitCost;
                    fuelTracking.ReadingDate = FFDateTime;

                    fuelTracking.Create(this.userData.DatabaseKey);
                    #endregion

                    #region Tasking 
                    Task[] tasks = new Task[2];

                    #region Insert into Fleetmeter reading
                    FleetMeterReading fleetMeter = new FleetMeterReading();
                    fleetMeter.ClientId = this.userData.DatabaseKey.User.ClientId;
                    fleetMeter.EquipmentId = equipment.EquipmentId;
                    fleetMeter.Reading = objFTM.FleetFuelModel.Reading;
                    fleetMeter.ReadingDate = FFDateTime;
                    fleetMeter.SourceId = fuelTracking.FuelTrackingId;
                    fleetMeter.SourceType = SourceTypeConstant.Fuel;
                    fleetMeter.Void = objFTM.FleetFuelModel.Void;
                    tasks[0] = Task.Factory.StartNew(() => fleetMeter.Create(this.userData.DatabaseKey));
                    #endregion

                    #region Update Equipment Table

                    if(objFTM.FleetFuelModel.Void == false)
                    {
                        equipment.ClientId = this.userData.DatabaseKey.User.ClientId;
                        equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
                        equipment.ClientLookupId = FF_ClientLookupId;
                        equipment.Meter1CurrentReading = objFTM.FleetFuelModel.Reading;
                        equipment.meter1currentreadingdate = FFDateTime.ToString();                      
                        tasks[1] = Task.Factory.StartNew(() => equipment.EquipmentUpdateFORFuelTracking(this.userData.DatabaseKey));
                    }
                   
                    #endregion
                    Task.WaitAll();
                    #endregion
                }
                else
                {
                    FleetFuelModel objFleetFuel = new FleetFuelModel();
                    string emptyValue = string.Empty;
                    FuelTracking fuelTracking = new FuelTracking()
                    {
                        ClientId = this.userData.DatabaseKey.Client.ClientId,
                        FuelTrackingId = Convert.ToInt64(objFTM.FleetFuelModel.FuelTrackingId),
                        EquipmentId = Convert.ToInt64(objFTM.FleetFuelModel.EquipmentID)
                    };
                    fuelTracking.FuelType = objFTM.FleetFuelModel.FuelType != null ? objFTM.FleetFuelModel.FuelType : emptyValue;
                    fuelTracking.FuelAmount = objFTM.FleetFuelModel.FuelAmount;
                    fuelTracking.FuelUnit = objFTM.FleetFuelModel.FuelUnit != null ? objFTM.FleetFuelModel.FuelUnit : emptyValue;
                    fuelTracking.UnitCost = objFTM.FleetFuelModel.UnitCost;
                    fuelTracking.ReadingDate = FFDateTime;
                    fuelTracking.Update(this.userData.DatabaseKey);

                    #region Tasking
                    Task[] tasks = new Task[2];

                    #region Fleet Mrter reading Update
                    FleetMeterReading fleetMeter = new FleetMeterReading()
                    {
                        ClientId = this.userData.DatabaseKey.Client.ClientId,
                        EquipmentId = Convert.ToInt64(objFTM.FleetFuelModel.EquipmentID),
                        SourceId = Convert.ToInt64(objFTM.FleetFuelModel.FuelTrackingId),
                        FleetMeterReadingId = Convert.ToInt64(objFTM.FleetFuelModel.FleetMeterReadingId)
                    };
                    fleetMeter.Reading = objFTM.FleetFuelModel.Reading;
                    fleetMeter.ReadingDate = FFDateTime;
                    fleetMeter.SourceType = SourceTypeConstant.Fuel;
                    fleetMeter.Void = objFTM.FleetFuelModel.Void;
                    tasks[0] = Task.Factory.StartNew(() => fleetMeter.Update(this.userData.DatabaseKey));
                    #endregion

                    #region Equipment Update
                    if (objFTM.FleetFuelModel.Void == false)
                    {
                        equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
                        equipment.ClientLookupId = FF_ClientLookupId;
                        equipment.Meter1CurrentReading = objFTM.FleetFuelModel.Reading;
                        equipment.meter1currentreadingdate = FFDateTime.ToString();
                        tasks[1] = Task.Factory.StartNew(() => equipment.EquipmentUpdateFORFuelTracking(this.userData.DatabaseKey));
                    }
                    #endregion
                    Task.WaitAll();
                    #endregion
                }
            } 
           
            return equipment;
        }     

        #endregion
        public FleetFuelModel initializeDetailsControls(Equipment obj)
        {
            FleetFuelModel objFleetFuel = new FleetFuelModel();
           objFleetFuel.EquipmentID = Convert.ToString(obj.EquipmentId);
            objFleetFuel.Meter1CurrentReading = obj.Meter1CurrentReading;
            objFleetFuel.Reading = obj.Reading;
            objFleetFuel.FltMrtReading = obj.Reading;
            objFleetFuel.Void = obj.Void;
            objFleetFuel.FuelAmount = obj.FuelAmount;
            objFleetFuel.UnitCost = obj.UnitCost;
            objFleetFuel.FuelType = obj.FuelType;
            objFleetFuel.ClientLookupId = obj.ClientLookupId;
            objFleetFuel.EquipId = obj.EquipmentId;
            objFleetFuel.FuelUnit = obj.FuelUnit !=""? obj.FuelUnit:"Units";
            DateTime dateAndTime = Convert.ToDateTime(obj.ReadingDate);
            string onlyDate = dateAndTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            string onlyTime = dateAndTime.ToString("HH:mm:ss");
            var time = DateTime.ParseExact(onlyTime, "HH:mm:ss", null).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US"));
            objFleetFuel.MtrCurrentReadingDate = onlyDate;
            objFleetFuel.StartTimeValue = time;
            objFleetFuel.FleetMeterReadingId = obj.FleetMeterReadingId;
            objFleetFuel.Mtr1CurrentReadingDate = obj.Meter1CurrentReadingDate.ToString();
            objFleetFuel.Meter1Units = obj.Meter1Units;
            return objFleetFuel;
        }

        public FleetFuelModel GetEditFleetFuelDetailsById(long EquipmentId,long FuelTrackingId)
        {
            FleetFuelModel objFleetModel = new FleetFuelModel();
            DataContracts.Equipment equipment = new DataContracts.Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId,
                FuelTrackingId= FuelTrackingId
            };
            equipment.RetrieveByEquipmentIdandFuelTrackingId(_dbKey);
            objFleetModel = initializeDetailsControls(equipment);
            return objFleetModel;
        }

        #region Delete FuelTracking
        public bool DeleteFuelTracking(long fuelTrackingId)
        {
            try
            {
                bool dltResult = false;
                FuelTracking fuelTracking = new FuelTracking()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    FuelTrackingId = fuelTrackingId
                };
                fuelTracking.Retrieve(this.userData.DatabaseKey);
                if(fuelTracking.Del==false)
                {
                    dltResult = true;
                    fuelTracking.Del = true;
                    fuelTracking.Update(userData.DatabaseKey);
                }
                return dltResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        #endregion

        #region FleetFuel Void Unvoid
        public List<string> ValidateFleetFuelForUnvoid(long _eqid, int mtrreadingid, bool Meter2Indicator)
        {
            try
            {
                //--retrieve fleet meter record
                FleetMeterReading fleetMeterReading = new FleetMeterReading()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    FleetMeterReadingId = mtrreadingid
                };
                fleetMeterReading.Retrieve(_dbKey);

                //--retrieve equipment record
                Equipment equipment = new DataContracts.Equipment()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EquipmentId = fleetMeterReading.EquipmentId,
                    FleetMeterReadingId = fleetMeterReading.FleetMeterReadingId,
                    TableName = AttachmentTableConstant.FleetMeterReading,
                    Meter2Indicator = fleetMeterReading.Meter2Indicator
                };
                equipment.RetrieveByPKForeignKeys_V2(_dbKey);
                //--validate for unvoid
                equipment.ValidiationForFleetMeterandFuelTrackingUnvoid(_dbKey);
                var errList = equipment.ErrorMessages;
                if (errList != null && errList.Count > 0)
                {
                    return errList;
                    // return false;
                }
                else
                {
                    //--update fleet meter 
                    fleetMeterReading.Void = false;
                    fleetMeterReading.Update(_dbKey);
                   
                        //--meter2indicator false
                        equipment.Meter1CurrentReading = fleetMeterReading.Reading;
                        equipment.Meter1CurrentReadingDate = fleetMeterReading.ReadingDate;
                        equipment.Update(_dbKey);
                  
                }
                return errList;
                //-----------------//
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Equipment ValidateFleetFuelForvoid(long _eqid, int mtrreadingid, string FF_ClientLookupId)
        {
            try
            {
                Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = FF_ClientLookupId, EquipmentId = _eqid };
                equipment.RetrieveByClientLookupId(_dbKey);
                equipment.UpdateEquipmentForVoidFromFuelTracking(userData.DatabaseKey);
                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    return equipment;
                }
                else
                {
                    FleetMeterReading fleetMeterReading = new FleetMeterReading()
                    {
                        ClientId = this.userData.DatabaseKey.Client.ClientId,
                        FleetMeterReadingId = mtrreadingid
                    };
                    fleetMeterReading.Retrieve(this.userData.DatabaseKey);
                    if (fleetMeterReading.Void == false)
                    {
                        fleetMeterReading.Void = true;
                        fleetMeterReading.Update(userData.DatabaseKey);
                    }
                }
                return equipment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

}
