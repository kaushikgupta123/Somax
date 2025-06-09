using Admin.Models;
using Admin.Models.Common;
using Admin.Models.SupportTicket;


using Common.Constants;
using Common.Enumerations;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Admin.BusinessWrapper
{
    public class SupportTicketWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        public readonly UserData _userData;

        List<string> errorMessage = new List<string>();
        public SupportTicketWrapper(UserData userData) : base(userData)
        {
            this._userData = userData;
            _dbKey = _userData.DatabaseKey;
        }
        public UserData userData { get; set; }

        #region Search

        public List<SupportTicketModel> GetSupportTicketGridData(int CustomQueryDisplayId, int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", long? SupportTicketId = null, string Status = "", string Contact = "", string Subject = "", string Agent = "", DateTime? CreateDate = null, DateTime? CompleteDate = null, string SearchText = "")
        {
            SupportTicketModel STSearchModel;
            List<SupportTicketModel> SupportTicketModelList = new List<SupportTicketModel>();
            List<SupportTicket> STList = new List<SupportTicket>();
            SupportTicket objST = new SupportTicket();
            objST.ClientId = _userData.DatabaseKey.Client.ClientId;
            objST.SiteId = _userData.Site.SiteId;
            objST.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
            objST.SupportTicketId = SupportTicketId ?? 0;
            objST.CustomQueryDisplayId = CustomQueryDisplayId;
            objST.orderbyColumn = orderbycol;
            objST.orderBy = orderDir;
            objST.offset1 = skip;
            objST.nextrow = length;
            objST.Contact = Contact;
            objST.Subject = Subject;
            objST.Agent = Agent;
            objST.Status = Status;
            objST.SearchText = SearchText;
            objST.CreateDate = CreateDate;
            objST.CompleteDate = CompleteDate;
            STList = objST.RetrieveChunkSearch(_userData.DatabaseKey);

            foreach (var item in STList)
            {
                STSearchModel = new SupportTicketModel();
                STSearchModel.ClientId = item.ClientId;
                STSearchModel.SupportTicketId = item.SupportTicketId;
                STSearchModel.Contact = item.Contact;
                STSearchModel.Subject = item.Subject;
                STSearchModel.Status = item.Status;
                STSearchModel.Agent = item.Agent;
                if (item.CreateDate != null && item.CreateDate == default(DateTime))
                {
                    STSearchModel.CreateDate = null;
                }
                else
                {
                    STSearchModel.CreateDate = item.CreateDate;
                }
                if (item.CompleteDate != null && item.CompleteDate == default(DateTime))
                {
                    STSearchModel.CompleteDate = null;
                }
                else
                {
                    STSearchModel.CompleteDate = item.CompleteDate;
                }
                STSearchModel.TotalCount = item.TotalCount;
                SupportTicketModelList.Add(STSearchModel);
            }

            return SupportTicketModelList;
        }

        #endregion

        #region Details
        public SupportTicketModel GetDetailsById(long SupportTicketId, long ClientId)
        {
            SupportTicketModel supportTicketModel = new SupportTicketModel();
            SupportTicket supportTicket = new SupportTicket()
            {
                ClientId = ClientId,
                SupportTicketId = SupportTicketId
            };
            supportTicket.RetrieveByPKForAdmin(_dbKey);
            supportTicketModel.SupportTicketId = supportTicket.SupportTicketId;
            supportTicketModel.ClientId = supportTicket.ClientId;
            supportTicketModel.SiteId = supportTicket.SiteId;
            supportTicketModel.Status = supportTicket.Status;
            supportTicketModel.Type = supportTicket.Type;
            supportTicketModel.Subject = supportTicket.Subject;
            supportTicketModel.Description = supportTicket.Description;
            supportTicketModel.Tags = supportTicket.Tags;
            supportTicketModel.CompleteDate = supportTicket.CompleteDate;
            supportTicketModel.Contact_PersonnelId = supportTicket.Contact_PersonnelId;
            supportTicketModel.Agent_PersonnelId = supportTicket.Agent_PersonnelId;
            return supportTicketModel;
        }
        #endregion

        #region Add and Edit
        public SupportTicket SaveSupportTicket(SupportTicketModel _stModel)
        {
            SupportTicket objSupportTicket = new SupportTicket();
            if (_stModel.SupportTicketId != 0)
            {
                objSupportTicket = EditSupportTicket(_stModel);
            }
            else
            {
                objSupportTicket = AddSupportTicket(_stModel);
            }
            return objSupportTicket;
        }
        public SupportTicket AddSupportTicket(SupportTicketModel stModel)
        {
            Personnel personnel = new Personnel();
            personnel.ClientId = stModel.ClientId;
            personnel.SiteId = stModel.SiteId;
            personnel.PersonnelId = stModel.Contact_PersonnelId;
            personnel.RetrievePersonnelByPersonnelId_V2ForAdmin(_userData.DatabaseKey, personnel);
            SupportTicket supportTicket = new SupportTicket();

            supportTicket.ClientId = stModel.ClientId;
            supportTicket.SiteId = stModel.SiteId;
            supportTicket.Agent_PersonnelId = _dbKey.Personnel.PersonnelId;
            supportTicket.Contact_PersonnelId = stModel.Contact_PersonnelId;
            supportTicket.Description = stModel.Description ?? string.Empty;
            supportTicket.ContactName = personnel.NameFirst + " " + personnel.NameLast;
            supportTicket.ContactPhoneNumber = personnel.Phone1 ?? string.Empty;
            supportTicket.ContactEmail = personnel.Email ?? string.Empty;
            supportTicket.Subject = stModel.Subject;
            supportTicket.Type = stModel.Type;
            if (stModel.Tags != null)
                supportTicket.Tags = deserializeobj(stModel.Tags);
            else
                supportTicket.Tags = "";
            supportTicket.Status = STStatusConstants.Open;

            supportTicket.CreateInAdmintSite(_dbKey);
            if (supportTicket.ErrorMessages == null || supportTicket.ErrorMessages.Count == 0)
            {
                STEventLog sTEventLog = new STEventLog();
                sTEventLog.ClientId = supportTicket.ClientId;
                sTEventLog.SupportTicketId = supportTicket.SupportTicketId;
                sTEventLog.TransactionDate = DateTime.UtcNow;
                sTEventLog.Event = STStatusConstants.Open;
                sTEventLog.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
                sTEventLog.SiteId = _userData.Site.SiteId;
                sTEventLog.CreateInAdmintSite(_dbKey);
            }

            return supportTicket;

        }

        public SupportTicket EditSupportTicket(SupportTicketModel stModel)
        {
            Personnel personnel = new Personnel();
            personnel.ClientId = stModel.ClientId;
            personnel.SiteId = stModel.SiteId;
            personnel.PersonnelId = stModel.Contact_PersonnelId;
            personnel.RetrievePersonnelByPersonnelId_V2ForAdmin(_userData.DatabaseKey, personnel);
            SupportTicket supportTicket = new SupportTicket()
            {
                ClientId = stModel.ClientId,
                SupportTicketId = stModel.SupportTicketId
            };
            supportTicket.RetrieveByPKForAdmin(_dbKey);

            supportTicket.Agent_PersonnelId = stModel.Agent_PersonnelId;
            supportTicket.Contact_PersonnelId = stModel.Contact_PersonnelId;
            supportTicket.Subject = stModel.Subject;
            supportTicket.Description = stModel.Description ?? string.Empty;
            if (stModel.Tags != null)
                supportTicket.Tags = deserializeobj(stModel.Tags);
            else
                supportTicket.Tags = "";
            supportTicket.Type = stModel.Type ?? string.Empty;
            supportTicket.ContactName = personnel.NameFirst + " " + personnel.NameLast;
            supportTicket.ContactPhoneNumber = personnel.Phone1 ?? string.Empty;
            supportTicket.ContactEmail = personnel.Email ?? string.Empty;
            if (supportTicket.CompleteDate != null && supportTicket.CompleteDate.Value == default(DateTime))
            {
                supportTicket.CompleteDate = null;
            }
            if (supportTicket.CancelDate != null && supportTicket.CancelDate.Value == default(DateTime))
            {
                supportTicket.CancelDate = null;
            }

            supportTicket.UpdateInAdmintSite(_userData.DatabaseKey);
            return supportTicket;
        }
        #endregion

        #region Response
        public List<STNotes> PopulateTicketResponse(long SupportTicketId)
        {
            STNotes note = new STNotes()
            {
                SupportTicketId = SupportTicketId
            };
            List<STNotes> NotesList = note.RetrieveBySupportTicketId(_userData.DatabaseKey, _userData.Site.TimeZone);
            return NotesList;
        }
        #endregion

        #region Tags
        public List<string> SupportTicketTags()
        {
            List<string> tags = new List<string>();
            SupportTicket SO = new SupportTicket();
            SO.Retrievetags(_dbKey);
            foreach (var item in SO.TotalRecords[0])
            {
                tags.Add(item.TagName);
            }
            return tags;
        }
        public string deserializeobj(string val)
        {
            // Deserializing json data to object  
            JavaScriptSerializer js = new JavaScriptSerializer();

            var TName = "";
            dynamic blogObject = js.Deserialize<dynamic>(val);
            foreach (var item in blogObject)
            {
                TName += item["value"] + ',';
            }
            TName = TName.TrimEnd(',');
            return TName;
        }
        #endregion

        #region Notes
        public STNotes AddSTNotes(STNotesModel sTNotesModel)
        {
            STNotes sTNotes = new STNotes();

            sTNotes.SupportTicketId = sTNotesModel.SupportTicketId;
            sTNotes.From_PersonnelId = _dbKey.Personnel.PersonnelId;
            sTNotes.Content = sTNotesModel.Content ?? string.Empty;
            sTNotes.Type = sTNotesModel.Type;

            sTNotes.CreateInAdminSite(_dbKey);
            if (sTNotes.ErrorMessages == null || sTNotes.ErrorMessages.Count == 0)
            {
                STEventLog sTEventLog = new STEventLog();
                sTEventLog.ClientId = sTNotesModel.ClientId;
                sTEventLog.SiteId = sTNotesModel.SiteId;
                sTEventLog.SupportTicketId = sTNotesModel.SupportTicketId;
                sTEventLog.TransactionDate = DateTime.UtcNow;
                sTEventLog.Event = STNotesTypesConstants.Reply;
                sTEventLog.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
                sTEventLog.CreateInAdmintSite(_dbKey);
            }

            return sTNotes;

        }
        #region Save Notes        
        public List<string> SaveResponse(STNotesModel notesModel, ref string Mode)
        {
            STNotes notes = new STNotes()
            {
                STNotesId = notesModel.STNotesId
            };
            if (notesModel.STNotesId == 0)
            {
                Mode = "add";
                notes = AddSTNotes(notesModel);
            }
            else
            {
                notes.Retrieve(_userData.DatabaseKey);
                notes.Content = notesModel.Content ?? string.Empty;
                Mode = "update";
                notes.Update(_userData.DatabaseKey);
            }

            return notes.ErrorMessages;
        }
        #endregion

        #region Delete Response
        public bool DeleteResponse(long _notesId)
        {
            try
            {
                STNotes notes = new STNotes()
                {
                    STNotesId = Convert.ToInt64(_notesId)
                };
                notes.Delete(_userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion

        #region Change Status
        public SupportTicket ChangeStatus(long ClientId, long SupportTicketId, string Status)
        {
            var strStatus = "";
            SupportTicket supportTicket = new SupportTicket()
            {
                ClientId = ClientId,
                SupportTicketId = SupportTicketId
            };
            supportTicket.RetrieveByPKForAdmin(_dbKey);
            if (supportTicket.CompleteDate != null && supportTicket.CompleteDate.Value == default(DateTime))
            {
                supportTicket.CompleteDate = null;
            }
            if (supportTicket.CancelDate != null && supportTicket.CancelDate.Value == default(DateTime))
            {
                supportTicket.CancelDate = null;
            }
            if (Status == "Hold")
            {
                supportTicket.Status = STStatusConstants.Hold;
                strStatus = supportTicket.Status;
            }
            else if (Status == "Complete")
            {
                supportTicket.Status = STStatusConstants.Complete;
                supportTicket.CompleteBy_PersonnelId = _dbKey.Personnel.PersonnelId;
                supportTicket.CompleteDate = DateTime.UtcNow;
                strStatus = supportTicket.Status;
            }
            else if (Status == "Reopen")
            {
                supportTicket.Status = STStatusConstants.Open;
                supportTicket.CancelBy_PersonnelId = 0;
                supportTicket.CompleteBy_PersonnelId = 0;
                supportTicket.CancelDate = null;
                supportTicket.CompleteDate = null;
                strStatus = "ReOpen";
            }
            else if (Status == "Cancel")
            {
                supportTicket.Status = STStatusConstants.Cancel;
                supportTicket.CancelBy_PersonnelId = _dbKey.Personnel.PersonnelId;
                supportTicket.CancelDate = DateTime.UtcNow;
                strStatus = supportTicket.Status;
            }

            supportTicket.UpdateInAdmintSite(_dbKey);
            if (supportTicket.ErrorMessages == null || supportTicket.ErrorMessages.Count == 0)
            {
                STEventLog sTEventLog = new STEventLog();
                sTEventLog.ClientId = _userData.DatabaseKey.Client.ClientId;
                sTEventLog.SiteId = _userData.Site.SiteId;
                sTEventLog.SupportTicketId = SupportTicketId;
                sTEventLog.TransactionDate = DateTime.UtcNow;
                sTEventLog.Event = strStatus;
                sTEventLog.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
                sTEventLog.CreateInAdmintSite(_dbKey);
            }
            return supportTicket;
        }
        #endregion
    }
}
