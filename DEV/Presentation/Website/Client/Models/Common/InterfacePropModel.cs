using System;

namespace Client.Models.Common
{
    [Serializable]
    public class InterfacePropModel
    {
        public string InterfaceType { get; set; }     
        public bool InUse { get; set; }
        public bool Switch1 { get; set; }
        public bool Switch2 { get; set; }
        public bool Switch3 { get; set; }
    }
}