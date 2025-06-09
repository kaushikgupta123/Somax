using Client.ActionFilters;
using Client.Controllers.Common;
using Client.Models;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class SensorSearchController : SomaxBaseController
    {       
        [HttpGet]
        [CheckUserSecurity(securityType = SecurityConstants.SensorSearch)]
        public ActionResult Index()
        {         
            Equipment_Sensor_Xref ESXR = new Equipment_Sensor_Xref();
            List<DataContracts.Equipment_Sensor_Xref> EquipmentSensorList = ESXR.RetrieveAllEquipmentSensorData(userData.DatabaseKey);
            return View("~/Views/SensorSearch/SensorSearch.cshtml");
        }
        [HttpGet]
        public JsonResult GetSensors()
        {     
            Equipment_Sensor_Xref ESXR = new Equipment_Sensor_Xref();
            List<DataContracts.Equipment_Sensor_Xref> EquipmentSensorList = ESXR.RetrieveAllEquipmentSensorData(userData.DatabaseKey);
            return Json(new { data = EquipmentSensorList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SensorEdit()
        {
            return View("~/Views/SensorEdit/SensorEdit.cshtml");
        }

        [HttpPost]
        public ActionResult GetSelectedRow(string _sensorId, string _sensorName, string _equipmentClientLookupId, string _lastReading, string _equipmentName, string _equipment_Sensor_XrefId)
        {
            var sensor = new Sensor
            {
                ClientId = "",
                AssignedTo_PersonnelId = "",
                EquipmentClientLookupId = _equipmentClientLookupId,
                LastReading = _lastReading,
                SensorAlertProcedureId = "",
                SensorAppId = "",
                SensorId = _sensorId,
                SensorName = _sensorName,
                EquipmentName = _equipmentName,
                Equipment_Sensor_XrefId = _equipment_Sensor_XrefId,
            };
            TempData["SensorData"] = sensor;
            TempData.Keep("SensorData");
            return new JsonResult { Data = sensor, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }    
}