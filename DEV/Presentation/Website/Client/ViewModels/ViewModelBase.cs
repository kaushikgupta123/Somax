using DataContracts;
using System;
using System.Collections.Generic;

namespace SomaxMVC.ViewModels
{
    [Serializable]
    public class ViewModelBase
    {
        public List<Localizations> localization { get; set; }
        //public List<UiConfiguration> uiConfig { get; set; }
    }
}