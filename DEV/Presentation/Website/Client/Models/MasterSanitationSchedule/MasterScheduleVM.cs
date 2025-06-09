using DataContracts;
using System.Collections.Generic;

namespace Client.Models.MasterSanitationSchedule
{
    public class MasterScheduleVM : LocalisationBaseVM
    {
        public MasterSanitationScheduleModel MasterSanitModel { get; set; }
        public MasterSanitTaskModel MasterSanTaskModel { get; set; }
        public MasterSanitNotesModel MasterSanNotesModel { get; set; }
        public MasterSanitAttachmentModel MasterSanAttachmentModel { get; set; }
        public MasterSanitationPlanningModel MasterSanPlanningModel { get; set; }
        public MasterSanitationPrintModel MasterSanitPrintModel { get; set; }
        public Security security { get; set; }
        public int attachmentCount { get; set; }
        // V2-609
        public bool AssetTree { get; set; }
        #region Job-genaration
        public SanitationJobGenerationModel sanitationJobGenerationModel { get; set; }
        public SanitationJobDetailsModel JobDetailsModel { get; set; }
        public List<DataContracts.SanitationPlanning> SanitationToolPrint { get; set; }
        public List<DataContracts.SanitationPlanning> PopulateChemicalsPrint { get; set; }
        public List<DataContracts.SanitationJobTask> SanitationTaskPrint { get; set; }
        public List<DataContracts.Timecard> laborList { get; set; }
        #endregion
    }
}