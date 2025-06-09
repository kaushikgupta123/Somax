using Client.Models;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class SensorEditController : Controller
    {
        public UserData userData { get; set; }
        public ActionResult Index()
        {
            Sensor obj = new Sensor();
            obj = (Sensor)TempData.Peek("SensorData");
            return View("~/Views/SensorEdit/SensorEdit.cshtml", obj);
        }
        public ActionResult GaugeChart()
        {
            Sensor obj = new Sensor();
            obj = (Sensor)TempData.Peek("SensorData");
            var sensor = new Sensor
            {
                ClientId = "",
                AssignedTo_PersonnelId = "",
                Equipment_Sensor_XrefId = "",
                EquipmentId = "",
                LastReading = Convert.ToString(obj.LastReading),
                SensorAlertProcedureId = "",
                SensorAppId = "",
                SensorId = "",
                SensorName = "",
            };
            return Json(sensor, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDateRange(string _startDate, string _endDate)
        {
            if (_startDate != null && _endDate != null)
            {
                Formmodel.startDate = Convert.ToDateTime(_startDate);
                Formmodel.endDate = Convert.ToDateTime(_endDate);
            }
            else
            {
                Formmodel.startDate = DateTime.MinValue;
                Formmodel.endDate = DateTime.Today;
            }
            #region Code for line chart            
            var data = System.Web.HttpContext.Current.Session["userData"];
            userData = (UserData)data;
            Sensor obj = new Sensor();
            obj = (Sensor)TempData.Peek("SensorData");
            DataContracts.Equipment_Sensor_Xref Exref = new DataContracts.Equipment_Sensor_Xref()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.UserName,
                Equipment_Sensor_XrefId = Convert.ToInt64(obj.Equipment_Sensor_XrefId)
            };

            Exref.RetrieveByExrefId(userData.DatabaseKey);
            long SensorId = Exref.SensorId;
            // Retrieve from DB 
            DataContracts.SensorReading sr = new DataContracts.SensorReading()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                CallerUserName = userData.DatabaseKey.UserName,
                SensorID =Convert.ToInt32(SensorId),
                BackDate = Convert.ToDateTime(Formmodel.startDate),
                CurrentDate = Convert.ToDateTime(Formmodel.endDate)
            };
            List<SensorReading> SRList = sr.RetrieveAll(userData.DatabaseKey, userData.Site.TimeZone);
            #endregion Code for line chart
            return new JsonResult { Data = SRList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };    
        }       
        public ActionResult Resultgrid()
        {            
            List<SensorReading> gd = new List<SensorReading>();
            var data = System.Web.HttpContext.Current.Session["userData"];
            userData = (UserData)data;
            Sensor obj = new Sensor();
            obj = (Sensor)TempData.Peek("SensorData");
            DataContracts.SensorReading sensorReading = new DataContracts.SensorReading()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                SensorID = Convert.ToInt32(obj.SensorId)
            };
            gd = sensorReading.RetrieveBySensorID(userData.DatabaseKey, userData.Site.TimeZone);
           return Json(new { data = gd }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetResult()
        {
            return null;
        }
        public class Griddata
        {
            public string MessageDate { get; set; }
            public int PlotValues { get; set; }
        }
        public static class Formmodel
        {
            public static DateTime startDate { get; set; }
            public static DateTime endDate { get; set; }
        }       
    }
}