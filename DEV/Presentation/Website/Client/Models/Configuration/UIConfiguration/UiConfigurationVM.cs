using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Configuration.UIConfiguration
{
    public class UiConfigurationVM : LocalisationBaseVM
    {
        public UiConfigurationVM()
        {
            
        }
       public UIViewModel uiViewModel { get; set; }
        public DataDictionaryModel dataDictionaryModel { get; set; }
        public List<AvailableUIConfigurationModel> AvailableListModel { get; set; }
        public List<SelectedUIConfigurationMedel> SelectedListModel { get; set; }
        public long UIViewId { get; set; }
        public string hiddenSelectedList { get; set; }
        public string hiddenAvailableList { get; set; }
        public List<SelectedListParam> selectedListParam { get; set; }
        public ColumnSettingConfigModel columnSettingConfigModel { get; set; }        
        public List<DataDictionaryModel> DataDictionaryListModel { get; set; }
        public AddSectionModel addSectiomModel { get; set; }
        public UDFLookupListModel udfLookuplistModel { get; set; }
        public List<UDFLookupListModel> udfLookuplistModelList { get; set; }
    }
}