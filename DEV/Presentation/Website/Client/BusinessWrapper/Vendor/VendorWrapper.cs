using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Vendor.UIConfiguration;

using Common.Constants;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Client.BusinessWrapper
{
    public class VendorWrapper : CommonWrapper
    {
        //Test
        private DatabaseKey _dbKey;
        private UserData userData;
        public VendorWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<VendorsModel> GetVendorList(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0, string _vendor = "", string _name = null, string _addresscity = null, string _addressstate = null, string _type = null, string _terms = null, string _fobcode = null, int inactiveFlag = 0, string IsExternal = "", string searchText = "")
        {
            List<VendorsModel> VendorsModelList = new List<VendorsModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            VendorsModel vendorsModel;
            Vendor vendor = new Vendor();
            vendor.ClientId = userData.DatabaseKey.Client.ClientId;
            vendor.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            vendor.CustomQueryDisplayId = inactiveFlag;
            vendor.orderbyColumn = orderbycol;
            vendor.orderBy = orderDir;
            vendor.offset1 = skip;
            vendor.nextrow = length;
            vendor.ClientLookupId = _vendor;
            vendor.Name = _name;
            vendor.AddressCity = _addresscity;
            vendor.AddressState = _addressstate;
            vendor.Type = _type;
            vendor.Terms = _terms;
            vendor.FOBCode = _fobcode;
            if (!string.IsNullOrEmpty(IsExternal))
            {
                if (IsExternal.Equals("true"))
                {
                    vendor.External = "1";
                }
                else if (IsExternal.Equals("false"))
                {
                    vendor.External = "0";
                }
            }
            vendor.SearchText = searchText;

            var vList = vendor.RetrieveChunkSearch(userData.DatabaseKey);

            foreach (var item in vList)
            {
                vendorsModel = new VendorsModel();
                vendorsModel.VendorId = item.VendorId;
                vendorsModel.ClientLookupId = item.ClientLookupId;
                vendorsModel.Name = item.Name;
                vendorsModel.AddressCity = item.AddressCity;
                vendorsModel.AddressState = item.AddressState;
                vendorsModel.Type = item.Type;
                vendorsModel.Terms = item.Terms;
                vendorsModel.FOBCode = item.FOBCode;
                vendorsModel.IsExternal = item.IsExternal;
                vendorsModel.TotalCount = item.totalCount;
                VendorsModelList.Add(vendorsModel);
            }        

            return VendorsModelList;
        }

        #endregion

        #region Add-Edit

        public VendorsModel PopulateDropdownControls(VendorsModel objVendors = null)
        {
            if (objVendors == null)
            {
                objVendors = new VendorsModel();
            }
            var AllLookUpLists = GetAllLookUpList();
            if (AllLookUpLists != null)
            {
                List<DataContracts.LookupList> objLookupFOBCode = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();
                if (objLookupFOBCode != null)
                {
                    objVendors.LookupFOBList = objLookupFOBCode.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

                List<DataContracts.LookupList> objLookupTerms = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();
                if (objLookupTerms != null)
                {
                    objVendors.LookupTermList = objLookupTerms.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

                List<DataContracts.LookupList> objLookupType = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_TYPE).ToList();
                if (objLookupType != null)
                {
                    objVendors.LookupTypeList = objLookupType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }

            return objVendors;
        }
        public Vendor AddVendorDetails(VendorsModel objVendor)
        {
            Vendor vendor = new Vendor();
            vendor.SiteId = userData.DatabaseKey.User.DefaultSiteId;

            vendor.ClientLookupId = objVendor.ClientLookupId;
            vendor.Name = objVendor.Name;
            vendor.PhoneNumber = objVendor.PhoneNumber;
            vendor.CustomerAccount = objVendor.CustomerAccount;
            vendor.EmailAddress = objVendor.Email;
            vendor.Website = objVendor.Website;
            vendor.FOBCode = objVendor.FOBCode;
            vendor.FaxNumber = objVendor.Fax;
            vendor.Terms = objVendor.Terms;
            vendor.Type = objVendor.Type;
            vendor.InactiveFlag = objVendor.InactiveFlag;

            vendor.Address1 = objVendor.Address1;
            vendor.Address2 = objVendor.Address2;
            vendor.Address3 = objVendor.Address3;
            vendor.AddressCity = objVendor.AddressCity;
            vendor.AddressState = objVendor.AddressState;
            vendor.AddressPostCode = objVendor.PostalCode;
            vendor.AddressCountry = objVendor.Country;

            vendor.RemitUseBusiness = objVendor.RemitUseBusiness;
            vendor.RemitAddress1 = objVendor.RemitAddress1;
            vendor.RemitAddress2 = objVendor.RemitAddress2;
            vendor.RemitAddress3 = objVendor.RemitAddress3;
            vendor.RemitCity = objVendor.RemitAddressCity;
            vendor.RemitState = objVendor.RemitAddressState;
            vendor.RemitPostCode = objVendor.RemitPostalCode;
            vendor.RemitCountry = objVendor.RemitCountry;
            vendor.SendPunchoutPOURL = objVendor.SendPunchoutPOURL;//V2-582
            vendor.SendPunchoutPOEmail = objVendor.SendPunchoutPOEmail;//V2-587
            vendor.CreateWithValidation(_dbKey);
            return vendor;
        }
        public List<String> ChangeVendorId(ChangeVendorIDModel _changeVendorIDModel)
        {
            List<string> EMsg = new List<string>();
            if (_changeVendorIDModel.VendorId > 0)
            {
                Vendor _vendor = new Vendor()
                {
                    VendorId = _changeVendorIDModel.VendorId,
                    ClientId = _dbKey.Client.ClientId,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId
                };
                _vendor.Retrieve(_dbKey);
                _vendor.ClientLookupId = _changeVendorIDModel.NewClientLookupId;
                _vendor.ChangeClientLookupId(userData.DatabaseKey);
                if (_vendor.ErrorMessages.Count == 0)
                {
                    EMsg = _vendor.ErrorMessages;
                    string Event = "ChangeID";
                    string Comments = "Previous Vendor ID – " + _changeVendorIDModel.OldClientLookupId;
                    EMsg = CreateVendorEvent(_changeVendorIDModel.VendorId, Event, Comments);
                }
                else
                {
                    EMsg = _vendor.ErrorMessages;
                }
            }
            return EMsg;
        }
        #endregion

        #region Details
        public VendorsModel populateVendorDetails(long vendorId)
        {
            VendorsModel objVen = new VendorsModel();
            Vendor obj = new Vendor()
            {
                ClientId = _dbKey.Client.ClientId,
                VendorId = vendorId
            };
            obj.Retrieve(_dbKey);
            objVen = initializeControls(obj);
            if (obj.VendorMasterId != 0)
            {
                VendorMaster vm = new VendorMaster();
                vm.ClientId = userData.DatabaseKey.Client.ClientId;
                vm.VendorMasterId = obj.VendorMasterId;
                vm.Retrieve(userData.DatabaseKey);
                List<long> noemaillist = new List<long>() { 3565 };
                if (noemaillist.Contains(vm.ExVendorId))
                {
                    objVen.ExVendorStat = false;
                }
                else
                {
                    objVen.ExVendorStat = true;
                }
            }

            return objVen;
        }
        public VendorsModel initializeControls(Vendor obj)
        {
            VendorsModel objVen = new VendorsModel();
            objVen.ClientLookupId = obj.ClientLookupId;
            objVen.Name = obj.Name;
            objVen.VendorId = obj.VendorId;
            objVen.ClientId = obj.ClientId;
            objVen.CustomerAccount = obj.CustomerAccount;
            objVen.Email = obj.EmailAddress;
            objVen.Website = obj.Website;
            objVen.FOBCode = obj.FOBCode;
            objVen.PhoneNumber = obj.PhoneNumber;
            objVen.Fax = obj.FaxNumber;
            objVen.Terms = obj.Terms;
            objVen.Type = obj.Type;
            objVen.InactiveFlag = obj.InactiveFlag;
            objVen.IsExternal = obj.IsExternal;

            objVen.Address1 = obj.Address1;
            objVen.Address2 = obj.Address2;
            objVen.Address3 = obj.Address3;
            objVen.AddressCity = obj.AddressCity;
            objVen.AddressState = obj.AddressState;
            objVen.PostalCode = obj.AddressPostCode;
            objVen.Country = obj.AddressCountry;

            objVen.RemitUseBusiness = obj.RemitUseBusiness;
            objVen.RemitAddress1 = obj.RemitAddress1;
            objVen.RemitAddress2 = obj.RemitAddress2;
            objVen.RemitAddress3 = obj.RemitAddress3;
            objVen.RemitAddressCity = obj.RemitCity;
            objVen.RemitAddressState = obj.RemitState;
            objVen.RemitPostalCode = obj.RemitPostCode;
            objVen.RemitCountry = obj.RemitCountry;
            objVen.VendorMasterId = obj.VendorMasterId;

            objVen.PunchoutIndicator = obj.PunchoutIndicator;
            objVen.VendorDomain = obj.VendorDomain;
            objVen.VendorIdentity = obj.VendorIdentity;
            objVen.SharedSecret = obj.SharedSecret;
            objVen.SenderDomain = obj.SenderDomain;
            objVen.SenderIdentity = obj.SenderIdentity;
            objVen.PunchoutURL = obj.PunchoutURL;
            objVen.AutoSendPunchOutPO = obj.AutoSendPunchOutPO;
            objVen.SendPunchoutPOURL = obj.SendPunchoutPOURL;//V2-582
            objVen.SendPunchoutPOEmail = obj.SendPunchoutPOEmail;//V2-587
            return objVen;
        }



        //public Vendors PopulateDropdownControls(Vendors objVendors)
        //{
        //    List<DataContracts.LookupList> objLookupFOBCode = new Models.LookupList().RetrieveAll(_dbKey).Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();
        //    List<DataContracts.LookupList> objLookupTerms = new Models.LookupList().RetrieveAll(_dbKey).Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();
        //    List<DataContracts.LookupList> objLookupType = new Models.LookupList().RetrieveAll(_dbKey).Where(x => x.ListName == LookupListConstants.VENDOR_TYPE).ToList();

        //    objVendors.LookupFOBList = objLookupFOBCode.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
        //    objVendors.LookupTermList = objLookupTerms.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
        //    objVendors.LookupTypeList = objLookupType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });

        //    return objVendors;
        //}


        public Vendor VendorEdit(VendorsModel _vendor, long objectId)
        {
            string emptyString = string.Empty;

            Vendor objVendor = new Vendor()
            {
                ClientId = _dbKey.Client.ClientId,
                VendorId = objectId
            };
            objVendor.Retrieve(_dbKey);

            objVendor.SiteId = _dbKey.User.DefaultSiteId;
            #region details
            objVendor.VendorId = _vendor.VendorId;
            objVendor.ClientLookupId = _vendor.ClientLookupId;
            objVendor.Name = _vendor.Name != null ? _vendor.Name : emptyString; ;
            objVendor.PhoneNumber = _vendor.PhoneNumber != null ? _vendor.PhoneNumber : emptyString;
            objVendor.CustomerAccount = _vendor.CustomerAccount != null ? _vendor.CustomerAccount : emptyString;
            objVendor.EmailAddress = _vendor.Email != null ? _vendor.Email : emptyString;
            objVendor.Website = _vendor.Website != null ? _vendor.Website : emptyString;
            objVendor.FOBCode = _vendor.FOBCode != null ? _vendor.FOBCode : emptyString;
            objVendor.PhoneNumber = _vendor.PhoneNumber != null ? _vendor.PhoneNumber : emptyString;
            objVendor.FaxNumber = _vendor.Fax != null ? _vendor.Fax : emptyString;
            objVendor.Terms = _vendor.Terms != null ? _vendor.Terms : emptyString;
            objVendor.Type = _vendor.Type != null ? _vendor.Type : emptyString;
            objVendor.InactiveFlag = _vendor.InactiveFlag;
            #endregion

            #region business address 
            objVendor.Address1 = _vendor.Address1 != null ? _vendor.Address1 : emptyString;
            objVendor.Address2 = _vendor.Address2 != null ? _vendor.Address2 : emptyString;
            objVendor.Address3 = _vendor.Address3 != null ? _vendor.Address3 : emptyString;
            objVendor.AddressCity = _vendor.AddressCity != null ? _vendor.AddressCity : emptyString;
            objVendor.AddressState = _vendor.AddressState != null ? _vendor.AddressState : emptyString;
            objVendor.AddressPostCode = _vendor.PostalCode != null ? _vendor.PostalCode : emptyString;
            objVendor.AddressCountry = _vendor.Country != null ? _vendor.Country : emptyString;
            #endregion

            #region remit address
            objVendor.RemitUseBusiness = _vendor.RemitUseBusiness;
            objVendor.RemitAddress1 = _vendor.RemitAddress1 != null ? _vendor.RemitAddress1 : emptyString;
            objVendor.RemitAddress2 = _vendor.RemitAddress2 != null ? _vendor.RemitAddress2 : emptyString;
            objVendor.RemitAddress3 = _vendor.RemitAddress3 != null ? _vendor.RemitAddress3 : emptyString; ;
            objVendor.RemitCity = _vendor.RemitAddressCity != null ? _vendor.RemitAddressCity : emptyString;
            objVendor.RemitState = _vendor.RemitAddressState != null ? _vendor.RemitAddressState : emptyString;
            objVendor.RemitPostCode = _vendor.RemitPostalCode != null ? _vendor.RemitPostalCode : emptyString;
            objVendor.RemitCountry = _vendor.RemitCountry != null ? _vendor.RemitCountry : emptyString;
            #endregion
            #region punchout
            if(_vendor.VendorConfigurePunchOutSecurity && _vendor.IsSitePunchOut)
            { 
            objVendor.PunchoutIndicator = _vendor.PunchoutIndicator;
            objVendor.VendorDomain = _vendor.VendorDomain != null ? _vendor.VendorDomain : emptyString;
            objVendor.VendorIdentity = _vendor.VendorIdentity != null ? _vendor.VendorIdentity : emptyString;
            objVendor.SharedSecret = _vendor.SharedSecret != null ? _vendor.SharedSecret : emptyString;
            objVendor.SenderDomain = _vendor.SenderDomain != null ? _vendor.SenderDomain : emptyString;
            objVendor.SenderIdentity = _vendor.SenderIdentity != null ? _vendor.SenderIdentity : emptyString;
            objVendor.PunchoutURL = _vendor.PunchoutURL != null ? _vendor.PunchoutURL : emptyString;
            objVendor.AutoSendPunchOutPO = _vendor.AutoSendPunchOutPO;
            objVendor.SendPunchoutPOURL = _vendor.SendPunchoutPOURL != null ? _vendor.SendPunchoutPOURL : emptyString;//V2-582
            objVendor.SendPunchoutPOEmail = _vendor.SendPunchoutPOEmail != null ? _vendor.SendPunchoutPOEmail : emptyString ;//V2-587
            }
            #endregion
            objVendor.Update(_dbKey);
            return objVendor;
        }

        public List<string> MakeActiveInactive(bool InActiveFlag, long VendorId)
        {
            Vendor objVendor = new Vendor()
            {
                ClientId = _dbKey.Client.ClientId,
                VendorId = VendorId
            };
            objVendor.Retrieve(_dbKey);
            objVendor.SiteId = _dbKey.User.DefaultSiteId;
            objVendor.InactiveFlag = !InActiveFlag;
            objVendor.Update(_dbKey);
            return objVendor.ErrorMessages;

        }

        public Vendor ValidateVendorStatusChange(long VendorId, string flag, string clientLookupId)
        {
            Vendor objVendor = new Vendor()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                VendorId = VendorId,
                Flag = flag,
                ClientLookupId = clientLookupId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };
            objVendor.CheckVendorIsInactivateorActivate(userData.DatabaseKey);
            return objVendor;
        }

        public List<string> CreateVendorEvent(long VendorId, string Event,string Comments)
        {
            VendorEventLog vendorEventLog = new VendorEventLog();
            vendorEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            vendorEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            vendorEventLog.VendorId = VendorId;
            vendorEventLog.TransactionDate = DateTime.UtcNow;           
            vendorEventLog.Event = Event;
            vendorEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            vendorEventLog.Comments = Comments;
            vendorEventLog.SourceId = 0;
            vendorEventLog.Create(userData.DatabaseKey);
            return vendorEventLog.ErrorMessages;
        }
        #endregion

        #region Contact
        public List<ContactModel> PopulateVendorContact(long _vendorId)
        {
            Contact contact = new Contact()
            {
                ObjectId = _vendorId,
                TableName = "Vendor"
            };
            ContactModel objContact;
            List<ContactModel> ContactsList = new List<ContactModel>();
            List<Contact> ContactList = contact.RetrieveByObjectId(_dbKey);
            foreach (var v in ContactList)
            {
                objContact = new ContactModel();
                objContact.Name = v.Name;
                objContact.Phone1 = v.Phone1;
                objContact.Email1 = v.Email1;
                objContact.OwnerName = v.OwnerName;
                objContact.VendorId = v.ObjectId;
                objContact.updatedindex = v.UpdateIndex;
                objContact.ContactId = v.ContactId;
                ContactsList.Add(objContact);
            }
            return ContactsList;
        }
        public List<string> ContactAdd(ContactModel contactModel, long objectId)
        {
            Contact contact = new Contact()
            {
                TableName = "Vendor",
                OwnerId = _dbKey.User.UserInfoId,
                OwnerName = _dbKey.User.FullName,
                ObjectId = objectId,
                Name = contactModel.Name,
                Address1 = contactModel.Address1,
                Address2 = contactModel.Address2,
                Address3 = contactModel.Address3,
                AddressCity = contactModel.AddressCity,
                AddressCountry = contactModel.AddressCountry,
                AddressPostCode = contactModel.AddressPostCode,
                AddressState = contactModel.AddressState,
                Phone1 = contactModel.Phone1,
                Phone2 = contactModel.Phone2,
                Phone3 = contactModel.Phone3,
                Email1 = contactModel.Email1,
                Email2 = contactModel.Email2
            };
            contact.Create(_dbKey);
            return contact.ErrorMessages;
        }
        public ContactModel ShowEditContact(long contactId, long vendorId, string ClientLookupId, int updatedIndex)
        {
            ContactModel _ContactModel = new ContactModel();
            Contact contact = new Contact() { ContactId = contactId };
            contact.Retrieve(_dbKey);

            _ContactModel.Name = contact.Name;
            _ContactModel.Phone1 = contact.Phone1;
            _ContactModel.Phone2 = contact.Phone2;
            _ContactModel.Phone3 = contact.Phone3;
            _ContactModel.Email1 = contact.Email1;
            _ContactModel.Email2 = contact.Email2;
            _ContactModel.VendorId = vendorId;
            _ContactModel.ClientLookupId = ClientLookupId;
            _ContactModel.ContactId = contactId;
            _ContactModel.updatedindex = updatedIndex;
            return _ContactModel;
        }
        public List<string> ContactEdit(ContactModel contactModel, long VendorID)
        {
            string emptyString = string.Empty;
            Contact contact = new Contact() { ContactId = contactModel.ContactId };
            contact.Retrieve(_dbKey);

            contact.OwnerId = _dbKey.User.UserInfoId;
            contact.OwnerName = _dbKey.User.FullName;
            contact.TableName = "Vendor";
            contact.ObjectId = VendorID;
            contact.Name = contactModel.Name;
            contact.Address1 = contactModel.Address1 != null ? contactModel.Address1 : emptyString;
            contact.Address2 = contactModel.Address2 != null ? contactModel.Address2 : emptyString;
            contact.Address3 = contactModel.Address3 != null ? contactModel.Address3 : emptyString;
            contact.AddressCity = contactModel.AddressCity != null ? contactModel.AddressCity : emptyString;
            contact.AddressCountry = contactModel.AddressCountry != null ? contactModel.AddressCountry : emptyString;
            contact.AddressPostCode = contactModel.AddressPostCode != null ? contactModel.AddressPostCode : emptyString;
            contact.AddressState = contactModel.AddressState != null ? contactModel.AddressState : emptyString;
            contact.Phone1 = contactModel.Phone1 != null ? contactModel.Phone1 : emptyString;
            contact.Phone2 = contactModel.Phone2 != null ? contactModel.Phone2 : emptyString;
            contact.Phone3 = contactModel.Phone3 != null ? contactModel.Phone3 : emptyString;
            contact.Email1 = contactModel.Email1 != null ? contactModel.Email1 : emptyString;
            contact.Email2 = contactModel.Email2 != null ? contactModel.Email2 : emptyString;
            contact.UpdateIndex = contact.UpdateIndex;

            contact.Update(_dbKey);
            return contact.ErrorMessages;
        }
        public bool ContactDelete(string _contactId)
        {
            try
            {
                Contact contact = new Contact();
                contact.ContactId = Convert.ToInt64(_contactId);
                contact.Delete(_dbKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Part
        public List<PartVendorXrefModel> PopulateParts(long objectId)
        {
            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = _dbKey.User.DefaultSiteId,
                VendorId = objectId

            };

            List<Part_Vendor_Xref> xList = new List<Part_Vendor_Xref>();
            xList = Part_Vendor_Xref.RetrieveListByVendorId(_dbKey, pvx);
            PartVendorXrefModel objPartVendorXrefModel;

            List<PartVendorXrefModel> xrefList = new List<PartVendorXrefModel>();
            foreach (var v in xList)
            {
                objPartVendorXrefModel = new PartVendorXrefModel();
                objPartVendorXrefModel.Part = v.Part_ClientLookupId;
                objPartVendorXrefModel.PartDescription = v.Part_Description;
                objPartVendorXrefModel.CatalogNumber = v.CatalogNumber;
                objPartVendorXrefModel.Manufacturer = v.Manufacturer;
                objPartVendorXrefModel.ManufacturerID = v.ManufacturerId;
                objPartVendorXrefModel.OrderQuantity = v.OrderQuantity;
                objPartVendorXrefModel.OrderUnit = v.OrderUnit;
                objPartVendorXrefModel.Price = v.Price;
                objPartVendorXrefModel.PartID = v.PartId;
                objPartVendorXrefModel.PartVendorXrefId = v.Part_Vendor_XrefId;
                objPartVendorXrefModel.PreferredVendor = v.PreferredVendor;
                objPartVendorXrefModel.updatedindex = v.UpdateIndex;
                objPartVendorXrefModel.VendorId = v.VendorId;
                objPartVendorXrefModel.VendorClientLookupId = v.Vendor_ClientLookupId;
                xrefList.Add(objPartVendorXrefModel);
            }
            return xrefList;
        }
        public List<DataContracts.LookupList> PopulateUnitOfMeasure()
        {
            List<DataContracts.LookupList> UOMLookupList = new List<DataContracts.LookupList>();
            var AllLookUpLists = GetAllLookUpList();
            if (AllLookUpLists != null)
            {
                UOMLookupList = AllLookUpLists.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
            }
            return UOMLookupList;
        }
        public List<string> AddPartVendorXref(PartVendorXrefModel _PVXModel, long objectId)
        {
            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                VendorId = objectId,
                Vendor_ClientLookupId = _PVXModel.VendorClientLookupId,
                SiteId = userData.Site.SiteId,

            };
            string part_ClientLookupId = _PVXModel.Part;
            Part pt = new Part { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = part_ClientLookupId };
            pt.RetrieveByClientLookUpIdNUPCCode(_dbKey);
            pvx.PartId = pt.PartId;
            pvx.CatalogNumber = _PVXModel?.CatalogNumber ?? string.Empty;
            pvx.Manufacturer = _PVXModel?.Manufacturer ?? string.Empty;
            pvx.ManufacturerId = _PVXModel?.ManufacturerID ?? string.Empty;
            pvx.OrderQuantity = _PVXModel?.OrderQuantity ?? 0;
            pvx.OrderUnit = _PVXModel?.OrderUnit ?? string.Empty;
            pvx.Price = _PVXModel.Price ?? 0;
            pvx.PreferredVendor = _PVXModel.PreferredVendor;
            pvx.Part_ClientLookupId = _PVXModel.Part;

            pvx.ValidateAdd(_dbKey);
            if (pvx.ErrorMessages.Count == 0)
            {
                pvx.Create(_dbKey);
            }
            return pvx.ErrorMessages;
        }
        public List<string> UpdatePartVendorXref(PartVendorXrefModel _PVXModel, long objectId)
        {
            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                VendorId = objectId,
                Part_Vendor_XrefId = _PVXModel.PartVendorXrefId

            };
            pvx.Retrieve(_dbKey);
            string part_ClientLookupId = _PVXModel.Part;
            Part pt = new Part { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = part_ClientLookupId };
            pt.RetrieveByClientLookUpIdNUPCCode(_dbKey);

            pvx.PartId = pt.PartId;
            pvx.CatalogNumber = _PVXModel?.CatalogNumber ?? string.Empty;
            pvx.Manufacturer = _PVXModel?.Manufacturer ?? string.Empty;
            pvx.ManufacturerId = _PVXModel?.ManufacturerID ?? string.Empty;
            pvx.OrderQuantity = _PVXModel?.OrderQuantity ?? 0;
            pvx.OrderUnit = _PVXModel?.OrderUnit ?? string.Empty;
            pvx.Price = _PVXModel.Price ?? 0;
            pvx.PreferredVendor = _PVXModel.PreferredVendor;
            pvx.Part_ClientLookupId = part_ClientLookupId;
            pvx.ValidateSave(_dbKey);
            if (pvx.ErrorMessages.Count == 0)
            {
                pvx.Update(_dbKey);
            }
            return pvx.ErrorMessages;
        }
        public bool DeletePart(long _PartVendorXrefId, long vendorId)
        {
            try
            {
                Part_Vendor_Xref pvx = new Part_Vendor_Xref()
                {
                    ClientId = _dbKey.Client.ClientId,
                    Part_Vendor_XrefId = _PartVendorXrefId
                };
                pvx.Delete(_dbKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PunchOutSupport
        public Vendor PunchOutSetUp(VendorPunchoutSetupModel punchoutSetup)
        {
            string emptyString = string.Empty;
            Vendor objVendor = new Vendor()
            {
                ClientId = _dbKey.Client.ClientId,
                VendorId = punchoutSetup.VendorId
            };
            objVendor.Retrieve(_dbKey);

            objVendor.SiteId = _dbKey.User.DefaultSiteId;
            objVendor.VendorId = punchoutSetup.VendorId;
            objVendor.PunchoutIndicator = punchoutSetup.PunchoutIndicator;
            objVendor.VendorDomain = punchoutSetup.VendorDomain != null ? punchoutSetup.VendorDomain : emptyString;
            objVendor.VendorIdentity = punchoutSetup.VendorIdentity != null ? punchoutSetup.VendorIdentity : emptyString;
            objVendor.SharedSecret = punchoutSetup.SharedSecret != null ? punchoutSetup.SharedSecret : emptyString;
            objVendor.SenderDomain = punchoutSetup.SenderDomain != null ? punchoutSetup.SenderDomain : emptyString;
            objVendor.SenderIdentity = punchoutSetup.SenderIdentity != null ? punchoutSetup.SenderIdentity : emptyString;
            objVendor.PunchoutURL = punchoutSetup.PunchoutURL != null ? punchoutSetup.PunchoutURL : emptyString;
            objVendor.AutoSendPunchOutPO = punchoutSetup.AutoSendPunchOutPO;
            objVendor.Update(_dbKey);
            return objVendor;
        }
        #endregion
        #region Add dynamic V2-642
        public Vendor AddVendorDynamic(VendorsVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            Vendor vendor = new Vendor();

            vendor.ClientId = userData.DatabaseKey.Client.ClientId;
            vendor.SiteId = userData.DatabaseKey.User.DefaultSiteId;
           
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddVendor, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.AddVendor);
                getpropertyInfo = objVM.AddVendor.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.AddVendor);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = vendor.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(vendor, val);
            }
            vendor.CreateWithValidation(_dbKey);
            if (vendor.ErrorMessages != null && vendor.ErrorMessages.Count == 0 && configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
            {
                IEnumerable<string> errors = AddVendorUDFDynamic(objVM.AddVendor, vendor.VendorId, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    vendor.ErrorMessages.AddRange(errors);
                }
            }

            return vendor;
        }
        public List<string> AddVendorUDFDynamic(AddVendorModelDynamic venodor, long VendorId, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            VendorUDF vendorUDF = new VendorUDF();
            vendorUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            vendorUDF.VendorId = VendorId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, venodor);
                getpropertyInfo = venodor.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(venodor);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = vendorUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(vendorUDF, val);
            }

            vendorUDF.Create(_dbKey);
            return vendorUDF.ErrorMessages;
        }
        private void AssignDefaultOrNullValue(ref object val, Type t)
        {
            if (t.Equals(typeof(long?)))
            {
                val = val ?? 0;
            }
            else if (t.Equals(typeof(DateTime?)))
            {
                //val = val ?? null;
            }
            else if (t.Equals(typeof(decimal?)))
            {
                val = val ?? 0M;
            }
            else if (t.Name == "String")
            {
                val = val ?? string.Empty;
            }
        }
        #endregion
        #region Edit dynamic V2-642
        public Vendor EditVendorDynamic(VendorsVM objVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            Vendor vendor = new Vendor()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                VendorId = Convert.ToInt64(objVM.EditVendor.VendorId)
            };
            vendor.Retrieve(_dbKey);

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditVendor, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objVM.EditVendor);
                getpropertyInfo = objVM.EditVendor.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objVM.EditVendor);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = vendor.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(vendor, val);
            }
            vendor.Update(_dbKey);
            List<string> errors = new List<string>();
            if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                 errors = EditVendorUDFDynamic(objVM.EditVendor, configDetails);
                
            }
            if (errors != null && errors.Count() > 0)
            {
                vendor.ErrorMessages.AddRange(errors);
            }
            return vendor;
        }
        public List<string> EditVendorUDFDynamic(EditVendorModelDynamic vendor, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            VendorUDF vendorUDF = new VendorUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorId = vendor.VendorId
            };
            vendorUDF = vendorUDF.RetrieveByVendorId(_dbKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, vendor);
                getpropertyInfo = vendor.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(vendor);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = vendorUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(vendorUDF, val);
            }
            if (vendorUDF.VendorUDFId > 0)
            {
                vendorUDF.Update(_dbKey);
            }
            else
            {
                vendorUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                vendorUDF.VendorId = vendor.VendorId;
                vendorUDF.Create(_dbKey);
            }

            return vendorUDF.ErrorMessages;
        }
        public EditVendorModelDynamic RetrieveVendorDetailsByVendorId(long VendorId)
        {
            EditVendorModelDynamic editVendorModelDynamic = new EditVendorModelDynamic();
            var details = RetrieveVendorByVendorId(VendorId);
            VendorUDF vendorUDF = RetrieveVendorUDFByVendorId(VendorId);

            editVendorModelDynamic = MapVendorUDFDataForEdit(editVendorModelDynamic, vendorUDF);
            editVendorModelDynamic = MapVendorDataForEdit(editVendorModelDynamic, details.Item1);
            return editVendorModelDynamic;
        }
        public Tuple<Vendor, bool> RetrieveVendorByVendorId(long VendorId)
        {
            Vendor vendor = new Vendor()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                VendorId = VendorId
            };
            vendor.Retrieve(_dbKey);
            var ExVendorStat = false;
            if (vendor.VendorMasterId != 0)
            {
                VendorMaster vm = new VendorMaster();
                vm.ClientId = userData.DatabaseKey.Client.ClientId;
                vm.VendorMasterId = vendor.VendorMasterId;
                vm.Retrieve(userData.DatabaseKey);
                List<long> noemaillist = new List<long>() { 3565 };
                if (noemaillist.Contains(vm.ExVendorId))
                {
                    ExVendorStat = false;
                }
                else
                {
                    ExVendorStat = true;
                }
            }

            return Tuple.Create(vendor, ExVendorStat);
        }
        public VendorUDF RetrieveVendorUDFByVendorId(long VendorId)
        {
            VendorUDF vendorUDF = new VendorUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorId = VendorId
            };

            vendorUDF = vendorUDF.RetrieveByVendorId(this.userData.DatabaseKey);
            return vendorUDF;
        }
        private EditVendorModelDynamic MapVendorUDFDataForEdit(EditVendorModelDynamic editVendorModelDynamic, VendorUDF vendorUDF)
        {
            if (vendorUDF != null)
            {
                editVendorModelDynamic.VendorUDFId = vendorUDF.VendorUDFId;

                editVendorModelDynamic.Text1 = vendorUDF.Text1;
                editVendorModelDynamic.Text2 = vendorUDF.Text2;
                editVendorModelDynamic.Text3 = vendorUDF.Text3;
                editVendorModelDynamic.Text4 = vendorUDF.Text4;

                if (vendorUDF.Date1 != null && vendorUDF.Date1 == DateTime.MinValue)
                {
                    editVendorModelDynamic.Date1 = null;
                }
                else
                {
                    editVendorModelDynamic.Date1 = vendorUDF.Date1;
                }
                if (vendorUDF.Date2 != null && vendorUDF.Date2 == DateTime.MinValue)
                {
                    editVendorModelDynamic.Date2 = null;
                }
                else
                {
                    editVendorModelDynamic.Date2 = vendorUDF.Date2;
                }
                if (vendorUDF.Date3 != null && vendorUDF.Date3 == DateTime.MinValue)
                {
                    editVendorModelDynamic.Date3 = null;
                }
                else
                {
                    editVendorModelDynamic.Date3 = vendorUDF.Date3;
                }
                if (vendorUDF.Date4 != null && vendorUDF.Date4 == DateTime.MinValue)
                {
                    editVendorModelDynamic.Date4 = null;
                }
                else
                {
                    editVendorModelDynamic.Date4 = vendorUDF.Date4;
                }

                editVendorModelDynamic.Bit1 = vendorUDF.Bit1;
                editVendorModelDynamic.Bit2 = vendorUDF.Bit2;
                editVendorModelDynamic.Bit3 = vendorUDF.Bit3;
                editVendorModelDynamic.Bit4 = vendorUDF.Bit4;

                editVendorModelDynamic.Numeric1 = vendorUDF.Numeric1;
                editVendorModelDynamic.Numeric2 = vendorUDF.Numeric2;
                editVendorModelDynamic.Numeric3 = vendorUDF.Numeric3;
                editVendorModelDynamic.Numeric4 = vendorUDF.Numeric4;

                editVendorModelDynamic.Select1 = vendorUDF.Select1;
                editVendorModelDynamic.Select2 = vendorUDF.Select2;
                editVendorModelDynamic.Select3 = vendorUDF.Select3;
                editVendorModelDynamic.Select4 = vendorUDF.Select4;
            }
            return editVendorModelDynamic;
        }
        public EditVendorModelDynamic MapVendorDataForEdit(EditVendorModelDynamic editVendorModelDynamic, Vendor vendor)
        {
            editVendorModelDynamic.VendorId = vendor.VendorId;
            editVendorModelDynamic.ClientLookupId = vendor.ClientLookupId;
            editVendorModelDynamic.Address1 = vendor.Address1;
            editVendorModelDynamic.Address2 = vendor.Address2;
            editVendorModelDynamic.Address3 = vendor.Address3;
            editVendorModelDynamic.AddressCity = vendor.AddressCity;
            editVendorModelDynamic.AddressCountry = vendor.AddressCountry;
            editVendorModelDynamic.AddressPostCode = vendor.AddressPostCode;
            editVendorModelDynamic.AddressState = vendor.AddressState;
            editVendorModelDynamic.CustomerAccount = vendor.CustomerAccount;
            editVendorModelDynamic.EmailAddress = vendor.EmailAddress;
            editVendorModelDynamic.FaxNumber = vendor.FaxNumber;
            editVendorModelDynamic.FOBCode = vendor.FOBCode;
            editVendorModelDynamic.InactiveFlag = vendor.InactiveFlag;
            editVendorModelDynamic.Name = vendor.Name;
            editVendorModelDynamic.PhoneNumber = vendor.PhoneNumber;
            editVendorModelDynamic.RemitAddress1 = vendor.RemitAddress1;
            editVendorModelDynamic.RemitAddress2 = vendor.RemitAddress2;
            editVendorModelDynamic.RemitAddress3 = vendor.RemitAddress3;
            editVendorModelDynamic.RemitCity = vendor.RemitCity;
            editVendorModelDynamic.RemitCountry = vendor.RemitCountry;
            editVendorModelDynamic.RemitPostCode = vendor.RemitPostCode;
            editVendorModelDynamic.RemitState = vendor.RemitState;
            editVendorModelDynamic.RemitUseBusiness = vendor.RemitUseBusiness;
            editVendorModelDynamic.Terms = vendor.Terms;
            editVendorModelDynamic.Type = vendor.Type;
            editVendorModelDynamic.Website = vendor.Website;
            editVendorModelDynamic.VendorMasterId = vendor.VendorMasterId;
            editVendorModelDynamic.AutoEmailPO = vendor.AutoEmailPO;
            editVendorModelDynamic.IsExternal = vendor.IsExternal;
            editVendorModelDynamic.PunchoutIndicator = vendor.PunchoutIndicator;
            editVendorModelDynamic.VendorDomain = vendor.VendorDomain;
            editVendorModelDynamic.VendorIdentity = vendor.VendorIdentity;
            editVendorModelDynamic.SharedSecret = vendor.SharedSecret;
            editVendorModelDynamic.SenderDomain = vendor.SenderDomain;
            editVendorModelDynamic.SenderIdentity = vendor.SenderIdentity;
            editVendorModelDynamic.PunchoutURL = vendor.PunchoutURL;
            editVendorModelDynamic.AutoSendPunchOutPO = vendor.AutoSendPunchOutPO;
            editVendorModelDynamic.SendPunchoutPOURL = vendor.SendPunchoutPOURL;
            editVendorModelDynamic.SendPunchoutPOEmail = vendor.SendPunchoutPOEmail;
            return editVendorModelDynamic;
        }

        #endregion
        #region View Vendor Dynamic
        public ViewVendorModelDynamic MapVendorUDFDataForView(ViewVendorModelDynamic viewVendorModelDynamic, VendorUDF vendorUDF)
        {
            if (vendorUDF != null)
            {
                viewVendorModelDynamic.VendorUDFId = vendorUDF.VendorUDFId;
                viewVendorModelDynamic.Text1 = vendorUDF.Text1;
                viewVendorModelDynamic.Text2 = vendorUDF.Text2;
                viewVendorModelDynamic.Text3 = vendorUDF.Text3;
                viewVendorModelDynamic.Text4 = vendorUDF.Text4;

                if (vendorUDF.Date1 != null && vendorUDF.Date1 == DateTime.MinValue)
                {
                    viewVendorModelDynamic.Date1 = null;
                }
                else
                {
                    viewVendorModelDynamic.Date1 = vendorUDF.Date1;
                }
                if (vendorUDF.Date2 != null && vendorUDF.Date2 == DateTime.MinValue)
                {
                    viewVendorModelDynamic.Date2 = null;
                }
                else
                {
                    viewVendorModelDynamic.Date2 = vendorUDF.Date2;
                }
                if (vendorUDF.Date3 != null && vendorUDF.Date3 == DateTime.MinValue)
                {
                    viewVendorModelDynamic.Date3 = null;
                }
                else
                {
                    viewVendorModelDynamic.Date3 = vendorUDF.Date3;
                }
                if (vendorUDF.Date4 != null && vendorUDF.Date4 == DateTime.MinValue)
                {
                    viewVendorModelDynamic.Date4 = null;
                }
                else
                {
                    viewVendorModelDynamic.Date4 = vendorUDF.Date4;
                }

                viewVendorModelDynamic.Bit1 = vendorUDF.Bit1;
                viewVendorModelDynamic.Bit2 = vendorUDF.Bit2;
                viewVendorModelDynamic.Bit3 = vendorUDF.Bit3;
                viewVendorModelDynamic.Bit4 = vendorUDF.Bit4;

                viewVendorModelDynamic.Numeric1 = vendorUDF.Numeric1;
                viewVendorModelDynamic.Numeric2 = vendorUDF.Numeric2;
                viewVendorModelDynamic.Numeric3 = vendorUDF.Numeric3;
                viewVendorModelDynamic.Numeric4 = vendorUDF.Numeric4;

                viewVendorModelDynamic.Select1 = vendorUDF.Select1;
                viewVendorModelDynamic.Select2 = vendorUDF.Select2;
                viewVendorModelDynamic.Select3 = vendorUDF.Select3;
                viewVendorModelDynamic.Select4 = vendorUDF.Select4;

                viewVendorModelDynamic.Select1Name = vendorUDF.Select1Name;
                viewVendorModelDynamic.Select2Name = vendorUDF.Select2Name;
                viewVendorModelDynamic.Select3Name = vendorUDF.Select3Name;
                viewVendorModelDynamic.Select4Name = vendorUDF.Select4Name;
            }
            return viewVendorModelDynamic;
        }
        public ViewVendorModelDynamic MapVendorDataForView(ViewVendorModelDynamic viewVendorModelDynamic, Vendor vendor)
        {
            viewVendorModelDynamic.VendorId = vendor.VendorId;
            viewVendorModelDynamic.ClientLookupId = vendor.ClientLookupId;
            viewVendorModelDynamic.Address1 = vendor.Address1;
            viewVendorModelDynamic.Address2 = vendor.Address2;
            viewVendorModelDynamic.Address3 = vendor.Address3;
            viewVendorModelDynamic.AddressCity = vendor.AddressCity;
            viewVendorModelDynamic.AddressCountry = vendor.AddressCountry;
            viewVendorModelDynamic.AddressPostCode = vendor.AddressPostCode;
            viewVendorModelDynamic.AddressState = vendor.AddressState;
            viewVendorModelDynamic.CustomerAccount = vendor.CustomerAccount;
            viewVendorModelDynamic.EmailAddress = vendor.EmailAddress;
            viewVendorModelDynamic.FaxNumber = vendor.FaxNumber;
            viewVendorModelDynamic.FOBCode = vendor.FOBCode;
            viewVendorModelDynamic.InactiveFlag = vendor.InactiveFlag;
            viewVendorModelDynamic.Name = vendor.Name;
            viewVendorModelDynamic.PhoneNumber = vendor.PhoneNumber;
            viewVendorModelDynamic.RemitAddress1 = vendor.RemitAddress1;
            viewVendorModelDynamic.RemitAddress2 = vendor.RemitAddress2;
            viewVendorModelDynamic.RemitAddress3 = vendor.RemitAddress3;
            viewVendorModelDynamic.RemitCity = vendor.RemitCity;
            viewVendorModelDynamic.RemitCountry = vendor.RemitCountry;
            viewVendorModelDynamic.RemitPostCode = vendor.RemitPostCode;
            viewVendorModelDynamic.RemitState = vendor.RemitState;
            viewVendorModelDynamic.RemitUseBusiness = vendor.RemitUseBusiness;
            viewVendorModelDynamic.Terms = vendor.Terms;
            viewVendorModelDynamic.Type = vendor.Type;
            viewVendorModelDynamic.Website = vendor.Website;
            viewVendorModelDynamic.VendorMasterId = vendor.VendorMasterId;
            viewVendorModelDynamic.AutoEmailPO = vendor.AutoEmailPO;
            viewVendorModelDynamic.IsExternal = vendor.IsExternal;
            viewVendorModelDynamic.PunchoutIndicator = vendor.PunchoutIndicator;
            viewVendorModelDynamic.VendorDomain = vendor.VendorDomain;
            viewVendorModelDynamic.VendorIdentity = vendor.VendorIdentity;
            viewVendorModelDynamic.SharedSecret = vendor.SharedSecret;
            viewVendorModelDynamic.SenderDomain = vendor.SenderDomain;
            viewVendorModelDynamic.SenderIdentity = vendor.SenderIdentity;
            viewVendorModelDynamic.PunchoutURL = vendor.PunchoutURL;
            viewVendorModelDynamic.AutoSendPunchOutPO = vendor.AutoSendPunchOutPO;
            viewVendorModelDynamic.SendPunchoutPOURL = vendor.SendPunchoutPOURL;
            viewVendorModelDynamic.SendPunchoutPOEmail = vendor.SendPunchoutPOEmail;
            return viewVendorModelDynamic;
        }
        #endregion

        #region EmailConfigurationSetup V2-750
        public Vendor EmailConfigurationSetUp(VendorEmailConfigurationSetupModel vendorEmailConfigurationSetupModel)
        {
            string emptyString = string.Empty;
            Vendor objVendor = new Vendor()
            {
                ClientId = _dbKey.Client.ClientId,
                VendorId = vendorEmailConfigurationSetupModel.VendorId
            };
            objVendor.Retrieve(_dbKey);

            objVendor.SiteId = _dbKey.User.DefaultSiteId;
            objVendor.EmailAddress = vendorEmailConfigurationSetupModel.Email;
            objVendor.AutoEmailPO = vendorEmailConfigurationSetupModel.AutoEmailPO;
            objVendor.Update(_dbKey);
            return objVendor;
        }
        #endregion

        #region V2-929
        public List<VendorInsuranceModel> PopulateVendorInsuranceGrid(int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", long? VendorId = null)
        {
            VendorInsurance vendorInsurance = new VendorInsurance();
            VendorInsuranceModel viModel;
            List<VendorInsuranceModel> VendorInsuranceModelList = new List<VendorInsuranceModel>();
            vendorInsurance.ClientId = userData.DatabaseKey.Client.ClientId;
            vendorInsurance.orderbyColumn = orderbycol;
            vendorInsurance.orderBy = orderDir;
            vendorInsurance.offset1 = Convert.ToString(skip);
            vendorInsurance.nextrow = Convert.ToString(length);

            vendorInsurance.VendorId = VendorId.HasValue ? VendorId.Value : 0;

            List<VendorInsurance> materialRequestList = vendorInsurance.RetrieveChunkSearchByVendorId(this.userData.DatabaseKey);
            if (materialRequestList != null)
            {
                foreach (var item in materialRequestList)
                {
                    viModel = new VendorInsuranceModel();
                    viModel.VendorInsuranceId = item.VendorInsuranceId;
                    viModel.Company = item.Company;
                    viModel.Contact = item.Contact;
                    if (item.ExpireDate != null && item.ExpireDate == default(DateTime))
                    {
                        viModel.ExpireDate = null;
                    }
                    else
                    {
                        viModel.ExpireDate = item.ExpireDate;
                    }
                    viModel.Amount = item.Amount;
                    viModel.Vendor_InsuranceSource = item.Vendor_InsuranceSource;
                    viModel.TotalCount = item.TotalCount;
                    VendorInsuranceModelList.Add(viModel);
                }
            }
            return VendorInsuranceModelList;
        }
        public VendorInsurance SaveVendorInsurance(VendorInsuranceModel VIModel)
        {
            VendorInsurance objVendorInsurance = new VendorInsurance();
            if (VIModel.VendorInsuranceId != 0)
            {
                objVendorInsurance = EditVendorInsurance(VIModel);
            }
            else
            {
                objVendorInsurance = AddVendorInsurance(VIModel);
            }
            return objVendorInsurance;
        }
        public VendorInsurance AddVendorInsurance(VendorInsuranceModel vendorInsuranceModel)
        {
            VendorInsurance vendorInsurance = new VendorInsurance();
            vendorInsurance.ClientId = userData.DatabaseKey.Client.ClientId;
            vendorInsurance.VendorId = vendorInsuranceModel.VendorId;
            vendorInsurance.Active = true;
            vendorInsurance.Amount = vendorInsuranceModel?.Amount ?? 0;
            vendorInsurance.Company= vendorInsuranceModel.Company;
            vendorInsurance.Contact= vendorInsuranceModel.Contact;
            vendorInsurance.ExpireDate = vendorInsuranceModel.ExpireDate;
            vendorInsurance.EffectiveDate = vendorInsuranceModel.EffectiveDate;
            vendorInsurance.InsuranceCertificate = vendorInsuranceModel.InsuranceCertificate;
            vendorInsurance.PKGSent = vendorInsuranceModel.PKGSent;
            vendorInsurance.SentVia = vendorInsuranceModel.SentVia;
            vendorInsurance.Create(_dbKey);

            Vendor vendor = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId= vendorInsuranceModel.VendorId
            };
            vendor.Retrieve(_dbKey);
            vendor.InsuranceSource = vendorInsurance.VendorInsuranceId;
            vendor.InsuranceExpireDate = vendorInsuranceModel.ExpireDate;
            if(vendor.InsuranceOverrideDate!=null && vendor.InsuranceOverrideDate==DateTime.MinValue)
            {
                vendor.InsuranceOverrideDate = null;
            }
            vendor.Update(_dbKey);

            return vendorInsurance;

        }
        public VendorInsuranceModel PopulateVendorInsuranceDetails(long VendorInsuranceId)
        {
            VendorInsurance vendorInsurance = new VendorInsurance()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorInsuranceId = VendorInsuranceId
            };
            vendorInsurance.Retrieve(_dbKey);
            VendorInsuranceModel vendorInsuranceModel = new VendorInsuranceModel();
            vendorInsuranceModel.VendorInsuranceId=vendorInsurance.VendorInsuranceId;
            vendorInsuranceModel.AdditionalInsurance = vendorInsurance.AdditionalInsurance;
            vendorInsuranceModel.Amount = vendorInsurance?.Amount ?? 0;
            vendorInsuranceModel.Amount_Medical = vendorInsurance?.Amount_Medical ?? 0;
            vendorInsuranceModel.Amount_OCC = vendorInsurance?.Amount_OCC ?? 0;
            vendorInsuranceModel.Amount_PIN = vendorInsurance?.Amount_PIN ?? 0;
            vendorInsuranceModel.Company = vendorInsurance.Company;
            vendorInsuranceModel.Contact = vendorInsurance.Contact;
            vendorInsuranceModel.ExpireDate = vendorInsurance.ExpireDate;
            vendorInsuranceModel.InsuranceCertificate = vendorInsurance.InsuranceCertificate;
            vendorInsuranceModel.LiabilityAgreement = vendorInsurance.LiabilityAgreement;
            vendorInsuranceModel.PKGContractorRecBy = vendorInsurance.PKGContactorRecBy;
            vendorInsuranceModel.PKGReceiveBy = vendorInsurance.PKGReceiveBy;
            vendorInsuranceModel.PKGSent = vendorInsurance.PKGSent;
            vendorInsuranceModel.SentVia = vendorInsurance.SentVia;
            vendorInsuranceModel.PreQualifySurvey = vendorInsurance.PreQualifySurvey;
            vendorInsuranceModel.EffectiveDate = vendorInsurance.EffectiveDate;

            return vendorInsuranceModel;
        }
        public VendorInsurance EditVendorInsurance(VendorInsuranceModel vendorInsuranceModel)
        {
            VendorInsurance vendorInsurance = new VendorInsurance()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorInsuranceId = vendorInsuranceModel.VendorInsuranceId
            };
            vendorInsurance.Retrieve(_dbKey);
            vendorInsurance.AdditionalInsurance = vendorInsuranceModel.AdditionalInsurance;
            vendorInsurance.Amount = vendorInsuranceModel?.Amount ?? 0;
            vendorInsurance.Amount_Medical = vendorInsuranceModel?.Amount_Medical ?? 0;
            vendorInsurance.Amount_OCC = vendorInsuranceModel?.Amount_OCC ?? 0;
            vendorInsurance.Amount_PIN = vendorInsuranceModel?.Amount_PIN ?? 0;
            vendorInsurance.Company = vendorInsuranceModel.Company;
            vendorInsurance.Contact = vendorInsuranceModel.Contact ?? string.Empty;
            vendorInsurance.ExpireDate = vendorInsuranceModel.ExpireDate;
            vendorInsurance.EffectiveDate = vendorInsuranceModel.EffectiveDate;
            vendorInsurance.InsuranceCertificate = vendorInsuranceModel.InsuranceCertificate ?? string.Empty;
            vendorInsurance.LiabilityAgreement = vendorInsuranceModel.LiabilityAgreement;
            vendorInsurance.PKGContactorRecBy = vendorInsuranceModel.PKGContractorRecBy;
            vendorInsurance.PKGReceiveBy = vendorInsuranceModel.PKGReceiveBy;
            vendorInsurance.PKGSent = vendorInsuranceModel.PKGSent;
            vendorInsurance.SentVia = vendorInsuranceModel.SentVia ?? string.Empty;
            vendorInsurance.PreQualifySurvey = vendorInsuranceModel.PreQualifySurvey;
            vendorInsurance.Update(_dbKey);
            if (vendorInsurance.ErrorMessages == null || vendorInsurance.ErrorMessages.Count() == 0)
            {
                Vendor vendor = new Vendor()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    VendorId = vendorInsuranceModel.VendorId
                };
                vendor.Retrieve(_dbKey);
                if (vendor.InsuranceSource == vendorInsuranceModel.VendorInsuranceId)
                {
                    vendor.InsuranceExpireDate = vendorInsuranceModel.ExpireDate;
                    if (vendor.InsuranceOverrideDate != null && vendor.InsuranceOverrideDate == DateTime.MinValue)
                    {
                        vendor.InsuranceOverrideDate = null;
                    }
                    vendor.Update(_dbKey);
                }
            }
            return vendorInsurance;

        }

        public bool DeleteVendorInsurance(long VendorInsuranceId)
        {
            try
            {
                VendorInsurance vendorInsurance = new VendorInsurance()
                {
                    ClientId = _dbKey.Client.ClientId,
                    VendorInsuranceId = VendorInsuranceId
                };
                vendorInsurance.Delete(_dbKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public VendorInsuranceWidgetModel PopulateVendorInsuranceWidgetDetails(long VendorId)
        {
            Vendor vendor = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = VendorId
            };
            vendor.Retrieve(_dbKey);
            VendorInsuranceWidgetModel vendorInsuranceModel = new VendorInsuranceWidgetModel();
            vendorInsuranceModel.VendorId = vendor.VendorId;
            vendorInsuranceModel.VendorClientLookupId = vendor.ClientLookupId;
            vendorInsuranceModel.InsuranceRequired = vendor.InsuranceRequired;
            vendorInsuranceModel.ExpModRate = vendor?.ExpModRate ?? 0;
            vendorInsuranceModel.OSHARate = vendor?.OSHARate ?? 0;
            vendorInsuranceModel.NAICSCode = vendor.NAICSCode;
            vendorInsuranceModel.SICCode = vendor.SICCode;
            vendorInsuranceModel.ClassCode = vendor.ClassCode;
            vendorInsuranceModel.InsuranceExpireDate = vendor.InsuranceExpireDate;
            vendorInsuranceModel.InsuranceOverride = vendor.InsuranceOverride;
            vendorInsuranceModel.InsuranceOverrideDate = vendor.InsuranceOverrideDate;
            vendorInsuranceModel.ContractorOwner = vendor.ContractorOwner;
            if(vendorInsuranceModel.ContractorOwner>0)
            {
                Personnel personnel = new Personnel()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    PersonnelId = vendorInsuranceModel?.ContractorOwner ?? 0
                };
                personnel.Retrieve(_dbKey);
                vendorInsuranceModel.ContractorOwner_ClientLookupId=personnel.ClientLookupId;
            }

            return vendorInsuranceModel;
        }
        public Vendor VendorUpdateForVendorInsurance(long VendorId,bool InsuranceOverride)
        {
            Vendor vendor = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = VendorId
            };
            vendor.Retrieve(_dbKey);
            vendor.InsuranceOverride= InsuranceOverride;
            if (vendor.InsuranceOverride == true)
            {
                vendor.InsuranceOverrideDate = DateTime.UtcNow;
            }
            else
            {
                vendor.InsuranceOverrideDate = null;
            }
            if (vendor.InsuranceExpireDate != null && vendor.InsuranceExpireDate == DateTime.MinValue)
            {
                vendor.InsuranceExpireDate = null;
            }
            vendor.Update(_dbKey);
            return vendor;

        }
        public Vendor UpdateForVendorInsuranceInformation(VendorInsuranceWidgetModel vendorInsuranceWidgetModel)
        {
            Vendor vendor = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = vendorInsuranceWidgetModel.VendorId
            };
            vendor.Retrieve(_dbKey);
            vendor.ContractorOwner = vendorInsuranceWidgetModel?.ContractorOwner ?? 0;
            vendor.SICCode = vendorInsuranceWidgetModel.SICCode ?? string.Empty;
            vendor.OSHARate = vendorInsuranceWidgetModel?.OSHARate ?? 0;
            vendor.ExpModRate = vendorInsuranceWidgetModel?.ExpModRate ?? 0;
            vendor.NAICSCode = vendorInsuranceWidgetModel.NAICSCode ?? string.Empty;
            vendor.ClassCode = vendorInsuranceWidgetModel.ClassCode ?? string.Empty;
            if (vendor.InsuranceOverrideDate != null && vendor.InsuranceOverrideDate == DateTime.MinValue)
            {
                vendor.InsuranceOverrideDate = null;
            }
            if (vendor.InsuranceExpireDate != null && vendor.InsuranceExpireDate == DateTime.MinValue)
            {
                vendor.InsuranceExpireDate = null;
            }
            vendor.Update(_dbKey);
            return vendor;

        }
        #endregion

        #region V2-933
        public List<VendorAssetMgtModel> PopulateVendorAssetManagementGrid(int skip = 0, int length = 0, string orderbycol = "",
            string orderDir = "", long? VendorId = null)
        {
            VendorAssetMgt vendorAssetMgt = new VendorAssetMgt();
            VendorAssetMgtModel vamModel;
            List<VendorAssetMgtModel> VendorAssetMgtModelList = new List<VendorAssetMgtModel>();
            vendorAssetMgt.ClientId = userData.DatabaseKey.Client.ClientId;
            vendorAssetMgt.orderbyColumn = orderbycol;
            vendorAssetMgt.orderBy = orderDir;
            vendorAssetMgt.offset1 = Convert.ToString(skip);
            vendorAssetMgt.nextrow = Convert.ToString(length);
            vendorAssetMgt.VendorId = VendorId.HasValue ? VendorId.Value : 0;

            List<VendorAssetMgt> datatList = vendorAssetMgt.RetrieveChunkSearchByVendorId(this.userData.DatabaseKey);
            if (datatList != null)
            {
                foreach (var item in datatList)
                {
                    vamModel = new VendorAssetMgtModel();
                    vamModel.VendorAssetMgtId = item.VendorAssetMgtId;
                    vamModel.Company = item.Company;
                    vamModel.Contact = item.Contact;
                    vamModel.Contract = item.Contract;
                    if (item.ExpireDate != null && item.ExpireDate == default(DateTime))
                    {
                        vamModel.ExpireDate = null;
                    }
                    else
                    {
                        vamModel.ExpireDate = item.ExpireDate;
                    }
                    vamModel.AssetMgtSource = item.AssetMgtSource;
                    vamModel.TotalCount = item.TotalCount;
                    VendorAssetMgtModelList.Add(vamModel);
                }
            }
            return VendorAssetMgtModelList;
        }

        public VendorAssetMgtWidgetModal PopulateVendorAssetManagementHeader(long? VendorId = null)
        {
            VendorAssetMgt vendorAssetMgt = new VendorAssetMgt();
            List<VendorAssetMgtModel> VendorAssetMgtModelList = new List<VendorAssetMgtModel>();
            vendorAssetMgt.ClientId = userData.DatabaseKey.Client.ClientId;
            vendorAssetMgt.VendorId = VendorId.HasValue ? VendorId.Value : 0;
            VendorAssetMgtWidgetModal vAssetMgtModel = new VendorAssetMgtWidgetModal();
            VendorAssetMgt datatList = vendorAssetMgt.RetrieveAssetMgtHederByVendorId(this.userData.DatabaseKey);
            if (datatList != null)
            {
                vAssetMgtModel.VendorId = datatList.VendorId;
                vAssetMgtModel.AssetMgtRequired = datatList.AssetMgtRequired;
                if (datatList.AssetMgtExpireDate != null && datatList.AssetMgtExpireDate == default(DateTime))
                {
                    vAssetMgtModel.AssetMgtExpireDate = null;
                }
                else
                {
                    vAssetMgtModel.AssetMgtExpireDate = datatList.AssetMgtExpireDate;
                }
                vAssetMgtModel.AssetMgtOverride = datatList.AssetMgtOverride;
                if (datatList.AssetMgtOverrideDate != null && datatList.AssetMgtOverrideDate == default(DateTime))
                {
                    vAssetMgtModel.AssetMgtOverrideDate = null;
                }
                else
                {
                    vAssetMgtModel.AssetMgtOverrideDate = datatList.AssetMgtOverrideDate;
                }
                vAssetMgtModel.Company = datatList.Company;
                vAssetMgtModel.Contact = datatList.Contact;
                vAssetMgtModel.Contract = datatList.Contract;
                vAssetMgtModel.VendorAssetMgtId = datatList.VendorAssetMgtId;
            }   
            
              
            
            return vAssetMgtModel;
        }
        public VendorAssetMgt SaveVendorAssetMgt(VendorAssetMgtModel VAModel)
        {
            VendorAssetMgt objVendorAssetMgt = new VendorAssetMgt();
            if (VAModel.VendorAssetMgtId != 0)
            {
                objVendorAssetMgt = EditVendorAssetMgt(VAModel);
            }
            else
            {
                objVendorAssetMgt = AddVendorAssetMgt(VAModel);
            }
            return objVendorAssetMgt;
        }
        public VendorAssetMgt AddVendorAssetMgt(VendorAssetMgtModel vendorAssetMgtModel)
        {
            VendorAssetMgt vendorAssetMgt = new VendorAssetMgt();
            vendorAssetMgt.ClientId = userData.DatabaseKey.Client.ClientId;
            vendorAssetMgt.VendorId = vendorAssetMgtModel.VendorId;
            vendorAssetMgt.Active = true;
            vendorAssetMgt.Company = vendorAssetMgtModel.Company;
            vendorAssetMgt.Contact = vendorAssetMgtModel.Contact;
            vendorAssetMgt.Contract = vendorAssetMgtModel.Contract;
            vendorAssetMgt.ExpireDate = vendorAssetMgtModel.ExpireDate;
            vendorAssetMgt.EffectiveDate = vendorAssetMgtModel.EffectiveDate;
            vendorAssetMgt.PKGSent = vendorAssetMgtModel.PKGSent;
            vendorAssetMgt.SentVia = vendorAssetMgtModel.SentVia;
            vendorAssetMgt.Create(_dbKey);

            Vendor vendor = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = vendorAssetMgtModel.VendorId
            };
            vendor.Retrieve(_dbKey);
            vendor.AssetMgtSource = vendorAssetMgt.VendorAssetMgtId;
            vendor.AssetMgtExpireDate = vendorAssetMgtModel.ExpireDate;
            if (vendor.AssetMgtOverrideDate != null && vendor.AssetMgtOverrideDate == DateTime.MinValue)
            {
                vendor.AssetMgtOverrideDate = null;
            }
            vendor.Update(_dbKey);

            return vendorAssetMgt;

        }
        public VendorAssetMgt EditVendorAssetMgt(VendorAssetMgtModel vendorAssetMgtModel)
        {
            VendorAssetMgt vendorAssetMgt = new VendorAssetMgt()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorAssetMgtId = vendorAssetMgtModel.VendorAssetMgtId
            };
            vendorAssetMgt.Retrieve(_dbKey);
            vendorAssetMgt.Company = vendorAssetMgtModel.Company;
            vendorAssetMgt.Contact = vendorAssetMgtModel.Contact ?? string.Empty;
            vendorAssetMgt.Contract = vendorAssetMgtModel.Contract ?? string.Empty;
            vendorAssetMgt.ExpireDate = vendorAssetMgtModel.ExpireDate;
            vendorAssetMgt.EffectiveDate = vendorAssetMgtModel.EffectiveDate;
            vendorAssetMgt.PKGContactorRecBy = vendorAssetMgtModel.PKGContactorRecBy;
            vendorAssetMgt.PKGReceiveBy = vendorAssetMgtModel.PKGReceiveBy;
            vendorAssetMgt.PKGSent = vendorAssetMgtModel.PKGSent;
            vendorAssetMgt.SentVia = vendorAssetMgtModel.SentVia ?? string.Empty;
            vendorAssetMgt.Update(_dbKey);
            if (vendorAssetMgt.ErrorMessages == null || vendorAssetMgt.ErrorMessages.Count() == 0)
            {
                Vendor vendor = new Vendor()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.Site.SiteId,
                    VendorId = vendorAssetMgtModel.VendorId
                };
                vendor.Retrieve(_dbKey);
                if (vendor.AssetMgtSource == vendorAssetMgtModel.VendorAssetMgtId)
                {
                    vendor.AssetMgtExpireDate = vendorAssetMgtModel.ExpireDate;
                    if (vendor.AssetMgtOverrideDate != null && vendor.AssetMgtOverrideDate == DateTime.MinValue)
                    {
                        vendor.AssetMgtOverrideDate = null;
                    }
                    vendor.Update(_dbKey);
                }
            }
            return vendorAssetMgt;

        }

        public bool DeleteVendorAssetMgt(long VendorAssetMgtId)
        {
            try
            {
                VendorAssetMgt vendorAssetMgt = new VendorAssetMgt()
                {
                    ClientId = _dbKey.Client.ClientId,
                    VendorAssetMgtId = VendorAssetMgtId
                };
                vendorAssetMgt.Delete(_dbKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public VendorAssetMgtModel PopulateVendorAssetMgtDetails(long VendorAssetMgtId)
        {
            VendorAssetMgt vendorAssetMgt = new VendorAssetMgt()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorAssetMgtId = VendorAssetMgtId
            };
            vendorAssetMgt.Retrieve(_dbKey);
            VendorAssetMgtModel vendorAssetMgtmodel = new VendorAssetMgtModel();
            vendorAssetMgtmodel.VendorAssetMgtId = vendorAssetMgt.VendorAssetMgtId;
            vendorAssetMgtmodel.PKGContactorRecBy = vendorAssetMgt.PKGContactorRecBy;
            vendorAssetMgtmodel.PKGReceiveBy = vendorAssetMgt.PKGReceiveBy;
            vendorAssetMgtmodel.PKGSent = vendorAssetMgt.PKGSent;
            vendorAssetMgtmodel.SentVia = vendorAssetMgt.SentVia;
            vendorAssetMgtmodel.Company = vendorAssetMgt.Company;
            vendorAssetMgtmodel.Contact = vendorAssetMgt.Contact;
            vendorAssetMgtmodel.Contract = vendorAssetMgt.Contract;
            vendorAssetMgtmodel.ExpireDate = vendorAssetMgt.ExpireDate;
            vendorAssetMgtmodel.EffectiveDate = vendorAssetMgt.EffectiveDate;
            return vendorAssetMgtmodel;
        }
        public Vendor VendorUpdateForVendorAssetMgt(long VendorId, bool VendorAssetMgOverride)
        {
            Vendor vendor = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.Site.SiteId,
                VendorId = VendorId
            };
            vendor.Retrieve(_dbKey);
            vendor.AssetMgtOverride = VendorAssetMgOverride;
            if (vendor.AssetMgtOverride == true)
            {
                vendor.AssetMgtOverrideDate = DateTime.UtcNow;
            }
            else
            {
                vendor.AssetMgtOverrideDate = null;
            }
            if (vendor.AssetMgtExpireDate != null && vendor.AssetMgtExpireDate == DateTime.MinValue)
            {
                vendor.AssetMgtExpireDate = null;
            }
            vendor.Update(_dbKey);
            return vendor;

        }
        #endregion
    }
}