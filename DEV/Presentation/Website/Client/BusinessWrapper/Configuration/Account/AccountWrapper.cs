using Client.Models.Configuration.Account;
using DataContracts;
using System;
using Client.Common;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Client.BusinessWrapper.Configuration.AccountConfig
{
    public class AccountWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public AccountWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Account
        public List<AccountModel> PopulateAccountList()
        {
            UserDetails userdet = new UserDetails();
            userdet.UserInfoId = userData.DatabaseKey.User.UserInfoId;
            userdet.ClientId = userData.DatabaseKey.Client.ClientId;
            userdet.RetrieveUserDetailsByUserInfoID(this.userData.DatabaseKey);
            List<AccountModel> accountList = new List<AccountModel>();
            AccountModel model;
            DataContracts.Account account = new DataContracts.Account();

            if(userdet.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userdet.IsSuperUser==true )
            {
                //account.SiteName = SiteName;
                List<DataContracts.Account> lst = account.RetrieveForSearchforSuperUser(this.userData.DatabaseKey);

                foreach (var item in lst)
                {
                    model = new AccountModel();
                    model.AccountId = item.AccountId;
                    model.DepartmentId = item.DepartmentId;
                    model.ClientLookupId = item.ClientLookupId;
                    model.Name = item.Name;
                    model.SiteId = item.SiteId;
                    model.SiteName = item.SiteName;
                    model.InactiveFlag = item.InactiveFlag;
                    model.IsExternal = item.IsExternal;
                    model.IsSuperUser = userdet.IsSuperUser;
                    model.PackageLevel = userdet.PackageLevel;
                    accountList.Add(model);
                }
            }
            else
            {
                account.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
                List<DataContracts.Account> lst = account.RetrieveForSearch_V2(this.userData.DatabaseKey);

                foreach (var item in lst)
                {
                    model = new AccountModel();
                    model.AccountId = item.AccountId;
                    model.SiteId = item.SiteId;                 // RKL - 2020-Oct-20
                    model.DepartmentId = item.DepartmentId;
                    model.ClientLookupId = item.ClientLookupId;
                    model.Name = item.Name;
                    model.InactiveFlag = item.InactiveFlag;
                    model.IsExternal = item.IsExternal;
                    accountList.Add(model);
                }
            }
            return accountList;
        }
        internal AccountModel AccountDetails(long AccountId)
        {
            AccountModel returnModel;
            DataContracts.Account account = new DataContracts.Account()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AccountId = AccountId
            };
            account.RetrieveforDetails(this.userData.DatabaseKey);
            returnModel = PopulateAccountModel(account);
            return returnModel;
        }
        internal AccountModel PopulateAccountModel(DataContracts.Account aobj)
        {
            AccountModel oModel = new AccountModel();
            oModel.AccountId = aobj.AccountId;
            oModel.DepartmentId = aobj.DepartmentId;
            oModel.ClientLookupId = aobj.ClientLookupId;
            oModel.Name = aobj.Name;
            oModel.InactiveFlag = aobj.InactiveFlag;
            oModel.IsExternal = aobj.IsExternal;
            oModel.UpdateIndex = aobj.UpdateIndex;
            oModel.SiteName = aobj.SiteName;
            return oModel;
        }
        internal List<String> UpdateAccount(AccountModel obj, ref long AccountID)
        {
            DataContracts.Account account = new DataContracts.Account()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AccountId = obj.AccountId
            };
            account.Retrieve(this.userData.DatabaseKey);

            account.ClientLookupId = account.ClientLookupId;
            account.Name = obj.Name;
            account.InactiveFlag = account.InactiveFlag;
            account.UpdateIndex = obj.UpdateIndex;
            account.DepartmentId = this.userData.DatabaseKey.Personnel.DepartmentId;
            //account.ParentId
            account.Update(this.userData.DatabaseKey);
            AccountID = account.AccountId;
            return account.ErrorMessages;
        }
        internal List<String> InsertAccount(AccountModel obj, ref long AccountID)
        {
            DataContracts.Account account = new DataContracts.Account();
            account.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            account.DepartmentId = this.userData.DatabaseKey.Personnel.DepartmentId;
            account.ClientLookupId = obj.ClientLookupId.Trim();
            account.Name = obj.Name;
            account.UpdateIndex = obj.UpdateIndex;

            account.CreateWithValidation(userData.DatabaseKey);
            AccountID = account.AccountId;
            return account.ErrorMessages;
        }

        internal Account ValidateActiveInactiveAcount(long AccountId, bool InactiveFlag)
        {
            Account account = new Account()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AccountId = AccountId,
                InactiveFlag = InactiveFlag,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            account.CheckAccountIsInactivateorActivate(userData.DatabaseKey);
            return account;
        }
        public List<string> MakeActiveInactive(long AccountId,bool InActiveFlag,int UpdateIndex)
        {
            Account account = new Account()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                AccountId = AccountId,
                InactiveFlag = !InActiveFlag,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                UpdateIndex= UpdateIndex
            };
            account.UpdateByActivateorInactivate(userData.DatabaseKey);
            return account.ErrorMessages;
        }

        public List<string> CreateAccountEvent(long AccountId, bool InActiveFlag)
        {
            AccountEventLog accountevent = new AccountEventLog();
            accountevent.ClientId = userData.DatabaseKey.User.ClientId;
            accountevent.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            accountevent.AccountId = AccountId;
            accountevent.TransactionDate = DateTime.UtcNow;
            if (InActiveFlag)
            {
                accountevent.Event = ActivationStatusConstant.Activate;
            }

            else
            {
                accountevent.Event = ActivationStatusConstant.InActivate;
            }
            accountevent.PersonnelId= userData.DatabaseKey.Personnel.PersonnelId;
            accountevent.Comments = string.Empty;
            accountevent.SourceTable= string.Empty;
            accountevent.SourceId = 0;
            accountevent.Create(userData.DatabaseKey);
            return accountevent.ErrorMessages;

        }
        #endregion

        #region Notes
        public List<NoteModel> PopulateNoteList(long AccountId)
        {
            List<NoteModel> noteList = new List<NoteModel>();
            NoteModel model;
            Notes note = new Notes()
            {
                ObjectId = AccountId,
                TableName = "Account"
            };
            List<DataContracts.Notes> lst = note.RetrieveByObjectId(this.userData.DatabaseKey, this.userData.Site.TimeZone);

            foreach (var item in lst)
            {
                model = new NoteModel();
                model.NotesId = item.NotesId;
                model.Subject = item.Subject;
                model.OwnerName = item.OwnerName;
                model.OwnerId = item.OwnerId;
                model.UpdateIndex = item.UpdateIndex;
                model.ModifiedDate = item.ModifiedDate;
                noteList.Add(model);
            }
            return noteList;
        }

        public NoteModel RetriveNoteById(long? NoteId, long accountId)
        {
            NoteModel retObj = new NoteModel();
            Notes notes = new Notes()
            {
                NotesId = NoteId ?? 0
            };
            notes.Retrieve(this.userData.DatabaseKey);
            retObj = PopulateNoteModel(notes, accountId);
            return retObj;
        }
        public List<String> AddNote(NoteModel objNote, ref string Mode)
        {
            Notes notes = new Notes()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                OwnerId = this.userData.DatabaseKey.User.UserInfoId,
                OwnerName = this.userData.DatabaseKey.User.FullName,
                Subject = objNote.Subject,
                Content = objNote.Content,
                Type = objNote.Type,
                ObjectId = objNote.AccountID,
                TableName = "Account"
            };
            notes.Create(this.userData.DatabaseKey);
            return notes.ErrorMessages;
        }
        public List<String> UpdateNote(NoteModel objNote)
        {
            Notes notes = new Notes()
            {
                NotesId = objNote.NotesId
            };
            notes.Retrieve(this.userData.DatabaseKey);

            notes.OwnerId = this.userData.DatabaseKey.User.UserInfoId;
            notes.OwnerName = this.userData.DatabaseKey.User.FullName;
            notes.Subject = objNote.Subject ?? "No Subject";
            notes.Content = objNote.Content;
            notes.Type = objNote.Type ?? "";
            notes.UpdateIndex = objNote.UpdateIndex;
            notes.TableName = "Account";
            notes.ObjectId = objNote.AccountID;
            notes.Update(this.userData.DatabaseKey);
            return notes.ErrorMessages;
        }
        public List<string> AddNote(NoteModel NoteModel)
        {
            Notes notes = new Notes()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                OwnerId = this.userData.DatabaseKey.User.UserInfoId,
                OwnerName = this.userData.DatabaseKey.User.FullName,
                Subject = NoteModel.Subject ?? "No Subject",
                Content = NoteModel.Content,
                Type = NoteModel.Type,
                ObjectId = NoteModel.AccountID,
                TableName = "Account"
            };
            notes.Create(this.userData.DatabaseKey);
            return notes.ErrorMessages;
        }
        public bool DeleteNote(long NotesId)
        {
            try
            {
                Notes notes = new Notes() { NotesId = NotesId };
                notes.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }
        }
        internal NoteModel PopulateNoteModel(DataContracts.Notes aobj, long accountId)
        {
            NoteModel oModel = new NoteModel();
            oModel.Subject = aobj.Subject;
            oModel.Content = aobj.Content;
            oModel.Type = aobj.Type;
            oModel.NotesId = aobj.NotesId;
            oModel.UpdateIndex = aobj.UpdateIndex;
            oModel.AccountID = accountId;
            return oModel;
        }
        #endregion

        #region Change-AccountId Pop-Up

        internal List<string> ChangeAccountID(AccountConfigVM objVM, ref long UpdateIndex)
        {
            Account acc = new Account();
            acc.ClientId = this.userData.DatabaseKey.Client.ClientId;
            acc.SiteId = this.userData.DatabaseKey.Personnel.SiteId;
            acc.AccountId = objVM.changeAccountIdModel.AccountId;
            acc.ClientLookupId = objVM.changeAccountIdModel.ClientLookupId.Trim();
            acc.UpdateIndex = objVM.changeAccountIdModel.UpdateIndex;
            acc.ChangeClientLookupId(this.userData.DatabaseKey);
            UpdateIndex = acc.UpdateIndex;
            return acc.ErrorMessages;
        }
        #endregion

        //V2-402

        //#region GetSitelist
        //public List<SelectListItem> GetSites()
        //{
        //    Site site = new Site();
        //    site.ClientId = userData.DatabaseKey.Personnel.ClientId;
        //    site.AuthorizedUser = userData.DatabaseKey.User.UserInfoId;

        //    List<Site> obj_Site = site.RetrieveAuthorizedForUser(userData.DatabaseKey);

        //    var Sites = obj_Site.Select(x => new SelectListItem { Text = x.Name + "-" + x.Description, Value = x.SiteId.ToString() }).ToList();
        //    return Sites;
        //}
        //#endregion
    }
}