using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Common.Constants;
using Client.Models.FleetMeter;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
namespace Client.BusinessWrapper.FleetMeter
{
    public class FleetMeterWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public FleetMeterWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region search
        public List<FleetMeterModel> GetFleetMeterGridData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string name = "", string make = "", string model = "", string vin = "", string startreadingDate = "", string endreadingDate = "", string searchText = "")
        {
            FleetMeterModel fleetMeterModel;
            List<FleetMeterModel> FleetMeterModelList = new List<FleetMeterModel>();
            List<Equipment> equipmentList = new List<Equipment>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
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
            equipment.VIN = vin;
            equipment.ReadingDateStart = startreadingDate;
            equipment.ReadingDateEnd = endreadingDate;
            equipment.SearchText = searchText;
            equipmentList = equipment.FleetMeterReadingRetrieveChunkSearchV2(_dbKey, userData.Site.TimeZone);
           bool ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            foreach (var item in equipmentList)
            {
                fleetMeterModel = new FleetMeterModel();
                fleetMeterModel.EquipmentId = item.EquipmentId;
                fleetMeterModel.ClientLookupId = item.ClientLookupId;
                fleetMeterModel.FleetMeterReadingId = item.FleetMeterReadingId;
                fleetMeterModel.Name = item.Name;
                fleetMeterModel.VIN = item.VIN;
                fleetMeterModel.Make = item.Make;
                fleetMeterModel.Model = item.Model;
                fleetMeterModel.ReadingDate = item.FMRReadingDate;
                fleetMeterModel.NoOfDays = item.NoofDays;
                fleetMeterModel.Meter2Indicator = item.Meter2Indicator;
                fleetMeterModel.ReadingLine1 = item.MeterReadingL1;
                fleetMeterModel.ReadingLine2 = item.MeterReadingL2;
                fleetMeterModel.SourceType = item.SourceType;
                fleetMeterModel.EquipmentImage = !string.IsNullOrEmpty(item.EquipmentImage) ? item.EquipmentImage : commonWrapper.GetNoImageUrl();
                if(ClientOnPremise)
                {
                    fleetMeterModel.EquipmentImage = UtilityFunction.PhotoBase64ImgSrc(fleetMeterModel.EquipmentImage);
                }
                fleetMeterModel.Void = (item.Action == "0" ? false : true);
                fleetMeterModel.SourceId = item.SourceId;
                fleetMeterModel.TotalCount = item.TotalCount;
                FleetMeterModelList.Add(fleetMeterModel);
            }

            return FleetMeterModelList;
        }

        #endregion
         
        #region Add
        public List<string> FleetMeterAdd(FleetMeterModel fleetMeterModel)
        {
            Equipment equipment = new DataContracts.Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = fleetMeterModel.EquipmentId,
                ObjectName = AttachmentTableConstant.FleetMeterReading
            };
            equipment.RetrieveByPKForeignKeys_V2(_dbKey);
            var fmdate = fleetMeterModel.CurrentReadingDate;
            DateTime FMDateTime = DateTime.ParseExact(fmdate + " " + fleetMeterModel.CurrentReadingTime, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);

            List<string> errList = new List<string>();

            //--for meter1
            if (fleetMeterModel.MetersAssociated == "single")
            {
                //--validation for single meter
                equipment.Reading = fleetMeterModel.Meter1CurrentReading;
                if(!fleetMeterModel.Meter1Void)  //--for non-void record add
                {
                    equipment.CheckMeter1CurrentReading(_dbKey);
                    if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                    {
                        return equipment.ErrorMessages;
                    }
                    else
                    {
                        //--update equipment
                        equipment.Meter1CurrentReading = fleetMeterModel.Meter1CurrentReading;
                        equipment.Meter1CurrentReadingDate = FMDateTime;
                        equipment.FirstMeterVoid = fleetMeterModel.Meter1Void;
                        equipment.UpdateForFleetMeter(_dbKey);

                        //--add to fleetmeter
                        FleetMeterReading fleetMeter = new FleetMeterReading();
                        fleetMeter.ClientId = this.userData.DatabaseKey.User.ClientId;
                        fleetMeter.EquipmentId = equipment.EquipmentId;
                        fleetMeter.Meter2Indicator = false;
                        fleetMeter.Reading = fleetMeterModel.Meter1CurrentReading;
                        fleetMeter.ReadingDate = FMDateTime;
                        fleetMeter.SourceId = 0;
                        fleetMeter.SourceType = SourceTypeConstant.Manual; 
                        fleetMeter.Void = fleetMeterModel.Meter1Void;
                        fleetMeter.Create(_dbKey);
                        return fleetMeter.ErrorMessages;
                    }
                }
                else   //-- for void record add
                {
                    //--add to fleetmeter only
                    FleetMeterReading fleetMeter = new FleetMeterReading();
                    fleetMeter.ClientId = this.userData.DatabaseKey.User.ClientId;
                    fleetMeter.EquipmentId = equipment.EquipmentId;
                    fleetMeter.Meter2Indicator = false;
                    fleetMeter.Reading = fleetMeterModel.Meter1CurrentReading;
                    fleetMeter.ReadingDate = FMDateTime;
                    fleetMeter.SourceId = 0;
                    fleetMeter.SourceType = SourceTypeConstant.Manual;
                    fleetMeter.Void = fleetMeterModel.Meter1Void;
                    fleetMeter.Create(_dbKey);
                    return fleetMeter.ErrorMessages;
                }
            }
            //--for meter1 and 2
            else
            {
                equipment.Meter1CurrentReading = fleetMeterModel.Meter1CurrentReading;
                equipment.Meter2CurrentReading = fleetMeterModel.Meter2CurrentReading;
                equipment.FirstMeterVoid = fleetMeterModel.Meter1Void;
                equipment.SecondMeterVoid = fleetMeterModel.Meter2Void;

                if (fleetMeterModel.Meter1Void && fleetMeterModel.Meter2Void)  //--for both void record add
                {
                    //--add to fleetmeter only

                    //--Primary Meter
                    FleetMeterReading fleetMeter = new FleetMeterReading();
                    fleetMeter.ClientId = this.userData.DatabaseKey.User.ClientId;
                    fleetMeter.EquipmentId = equipment.EquipmentId;
                    fleetMeter.ReadingDate = FMDateTime;
                    fleetMeter.SourceId = 0;
                    fleetMeter.SourceType = SourceTypeConstant.Manual;
                    fleetMeter.Meter2Indicator = false;
                    fleetMeter.Reading = fleetMeterModel.Meter1CurrentReading;
                    fleetMeter.Void = fleetMeterModel.Meter1Void;
                    fleetMeter.Create(_dbKey);
                    if (fleetMeter.ErrorMessages != null && fleetMeter.ErrorMessages.Count > 0)
                    {
                        errList = fleetMeter.ErrorMessages;
                    }

                    //--Secondary Meter
                    FleetMeterReading fleetMeter2 = new FleetMeterReading();
                    fleetMeter2.ClientId = this.userData.DatabaseKey.User.ClientId;
                    fleetMeter2.EquipmentId = equipment.EquipmentId;
                    fleetMeter2.ReadingDate = FMDateTime;
                    fleetMeter2.SourceId = 0;
                    fleetMeter2.SourceType = SourceTypeConstant.Manual;
                    fleetMeter2.Meter2Indicator = true;
                    fleetMeter2.Reading = fleetMeterModel.Meter2CurrentReading;
                    fleetMeter2.Void = fleetMeterModel.Meter2Void;
                    fleetMeter2.Create(_dbKey);
                    if (fleetMeter2.ErrorMessages != null && fleetMeter2.ErrorMessages.Count > 0)
                    {
                        errList.AddRange(fleetMeter2.ErrorMessages);
                    }
                    return errList;
                }

                else  //--for either non-void record add
                {
                    //--validation for both meters
                    
                    equipment.CheckBothMeterReading(_dbKey);
                    if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                    {
                        return equipment.ErrorMessages;
                    }
                    else
                    {
                        //--update equipment
                        equipment.Meter1CurrentReadingDate = FMDateTime;
                        equipment.Meter2CurrentReadingDate = FMDateTime;
                        equipment.FirstMeterVoid = fleetMeterModel.Meter1Void;
                        equipment.SecondMeterVoid = fleetMeterModel.Meter2Void;
                        equipment.UpdateForFleetMeter(_dbKey);

                        //--add to fleetmeter

                        //--Primary Meter
                        FleetMeterReading fleetMeter = new FleetMeterReading();
                        fleetMeter.ClientId = this.userData.DatabaseKey.User.ClientId;
                        fleetMeter.EquipmentId = equipment.EquipmentId;
                        fleetMeter.ReadingDate = FMDateTime;
                        fleetMeter.SourceId = 0;
                        fleetMeter.SourceType = SourceTypeConstant.Manual;
                        fleetMeter.Meter2Indicator = false;
                        fleetMeter.Reading = fleetMeterModel.Meter1CurrentReading;
                        fleetMeter.Void = fleetMeterModel.Meter1Void;
                        fleetMeter.Create(_dbKey);
                        if (fleetMeter.ErrorMessages != null && fleetMeter.ErrorMessages.Count > 0)
                        {
                            errList = fleetMeter.ErrorMessages;
                        }

                        //--Secondary Meter
                        FleetMeterReading fleetMeter2 = new FleetMeterReading();
                        fleetMeter2.ClientId = this.userData.DatabaseKey.User.ClientId;
                        fleetMeter2.EquipmentId = equipment.EquipmentId;
                        fleetMeter2.ReadingDate = FMDateTime;
                        fleetMeter2.SourceId = 0;
                        fleetMeter2.SourceType = SourceTypeConstant.Manual;
                        fleetMeter2.Meter2Indicator = true;
                        fleetMeter2.Reading = fleetMeterModel.Meter2CurrentReading;
                        fleetMeter2.Void = fleetMeterModel.Meter2Void;
                        fleetMeter2.Create(_dbKey);
                        if (fleetMeter2.ErrorMessages != null && fleetMeter2.ErrorMessages.Count > 0)
                        {
                            errList.AddRange(fleetMeter2.ErrorMessages);
                        }
                        return errList;
                    }
                }
            }
        }

        #endregion

        #region VoidUnVoid
        public bool VoidFleetMeter(long fleetMeterReadingId)
        {
            //--retrieve fleet meter record
            FleetMeterReading fleetMeterReading = new FleetMeterReading()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                FleetMeterReadingId = fleetMeterReadingId
            };
            fleetMeterReading.Retrieve(_dbKey);

            //--retrieve equipment record
            Equipment equipment = new DataContracts.Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = fleetMeterReading.EquipmentId,
                FleetMeterReadingId = fleetMeterReading.FleetMeterReadingId,
                Meter2Indicator= fleetMeterReading.Meter2Indicator
            };
            equipment.RetrieveByPKForeignKeys_V2(_dbKey);

            //--update fleet meter & equipment
            equipment.UpdateForVoidbyFleetMeterandEquipmentId(_dbKey);
            if(equipment.ErrorMessages==null || equipment.ErrorMessages.Count==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<string> UnvoidFleetMeter(long fleetMeterReadingId)
        {
            //--retrieve fleet meter record
            FleetMeterReading fleetMeterReading = new FleetMeterReading()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                FleetMeterReadingId = fleetMeterReadingId
            };
            fleetMeterReading.Retrieve(_dbKey);

            //--retrieve equipment record
            Equipment equipment = new DataContracts.Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = fleetMeterReading.EquipmentId,
                FleetMeterReadingId = fleetMeterReading.FleetMeterReadingId,
                TableName= AttachmentTableConstant.FleetMeterReading,              
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
                //--update equipment
                if(fleetMeterReading.Meter2Indicator)
                {
                    //--meter2indicator true
                    equipment.Meter2CurrentReading = fleetMeterReading.Reading;
                    equipment.Meter2CurrentReadingDate = fleetMeterReading.ReadingDate;
                    equipment.Update(_dbKey);
                }
                else
                {
                    //--meter2indicator false
                    equipment.Meter1CurrentReading = fleetMeterReading.Reading;
                    equipment.Meter1CurrentReadingDate = fleetMeterReading.ReadingDate;
                    equipment.Update(_dbKey);
                }
            }
            return errList;
        }
        #endregion
    }
}