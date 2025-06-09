namespace Client.Models.Configuration.SanitationOnDemandLibrary
{
    public class SanitationOnDemandLibVM: LocalisationBaseVM
    {
        public SanitationOnDemandLibModel sanitationOnDemandLibModel { get; set; }
        public SanitationOnDemandLibPrintModel sanitationOnDemandLibPrintModel { get; set; }
        public SanitOnDemandLibTaskModel sanitOnDemandLibTaskModel { get; set; }
        public SanitationOnDemandSecurityModel sanitationOnDemandSecurityModel { get; set; }
    }
}