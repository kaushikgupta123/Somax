using System.Web.Routing;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.Sql;
using System.Web.Hosting;
using DevExpress.DataAccess.Excel;
using DevExpress.DashboardCommon;
using Client.Models.WorkOrder;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System;
using Client.BusinessWrapper.Work_Order;

namespace Client
{
    public static class DashboardConfig {
        public static void RegisterService(RouteCollection routes) {
            routes.MapDashboardRoute("dashboardControl", "DefaultDashboard");

            DashboardFileStorage dashboardFileStorage = new DashboardFileStorage("~/DevExpressDashboards");
            DashboardConfigurator.Default.SetDashboardStorage(dashboardFileStorage);

            // Uncomment this string to allow end users to create new data sources based on predefined connection strings.
            DashboardConfigurator.Default.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());
            
            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();

            // Registers an Object data source.
            DashboardObjectDataSource objDataSource = new DashboardObjectDataSource("Analytics WO Status Data Source");
            objDataSource.DataId = "Analytics WO Data Source Data Id";
            dataSourceStorage.RegisterDataSource("objDataSource", objDataSource.SaveToXml());
            DashboardObjectDataSource objDataSource2 = new DashboardObjectDataSource("Analytics WO Model Data Source");
            objDataSource2.DataId = "Analytics WO Data Source Data Id";
            dataSourceStorage.RegisterDataSource("objDataSource2", objDataSource2.SaveToXml());
            //Caching timeout set to 5 seconds
            DashboardConfigurator.Default.ConfigureDataReloadingTimeout += (s, e) => {
                e.DataReloadingTimeout = TimeSpan.FromSeconds(5);
            };
                   
            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);
            DashboardConfigurator.Default.DataLoading += DataLoading;
        }

        static void DataLoading(object sender, DataLoadingWebEventArgs e) 
        {
            var userData = (DataContracts.UserData)HttpContext.Current.Session["userData"];
            if (e.DataId == "Analytics WO Data Source Data Id") 
            {
                int CaseNo = 5;
                if (e.Parameters.Count() > 0)
                {
                    var val = e.Parameters.FirstOrDefault().Value.ToString();
                    if (val == "Last 30 Days")
                    {
                        CaseNo = 2;
                    }
                    else if (val == "Last 60 Days")
                    {
                        CaseNo = 3;
                    }
                    else if (val == "Last 90 Days")
                    {
                        CaseNo = 4;
                    }
                    else if (val == "Last 180 Days")
                    {
                        CaseNo = 5;
                    }
                    else
                    {
                        CaseNo = 1;
                    }
                }
                WorkOrderWrapper workOrderWrapper = new WorkOrderWrapper(userData);
                var Data = workOrderWrapper.AnalyticsWOStatus(CaseNo);
                if(e.DataSourceName == "Analytics WO Status Data Source")
                {
                    e.Data = Data.WOStatusModel;
                }
                else
                {
                    e.Data = Data.WOModel;
                }
            }
        }
    }
}