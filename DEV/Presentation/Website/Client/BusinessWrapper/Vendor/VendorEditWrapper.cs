using Client.Models;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.BusinessWrapper
{
    //public class VendorEditWrapper
    //{
    //    private DatabaseKey _dbKey;
    //    private long _objectId { get; set; }
    //   // private UserData userData;
    //    public VendorEditWrapper(DatabaseKey databaseKey, long objectId)
    //    {
    //        this._dbKey = databaseKey;
    //        this._objectId = objectId;
    //    }

    //    #region Details
    //    public Vendors populateVendorDetails()
    //    {
    //        Vendors objVen = new Vendors();
    //        Vendor obj = new Vendor()
    //        {
    //            ClientId = _dbKey.Client.ClientId,
    //            VendorId = _objectId
    //        };
    //        obj.Retrieve(_dbKey);
    //        objVen = initializeControls(obj);
            
    //        return objVen;
    //    }
    //    public Vendors initializeControls(Vendor obj)
    //    {
    //        Vendors objVen = new Vendors();
    //        objVen.ClientLookupId = obj.ClientLookupId;
    //        objVen.Name = obj.Name;
    //        objVen.VendorId = obj.VendorId;
    //        objVen.ClientId = obj.ClientId;
    //        objVen.CustomerAccount = obj.CustomerAccount;
    //        objVen.Email = obj.EmailAddress;
    //        objVen.Website = obj.Website;
    //        objVen.FOBCode = obj.FOBCode;
    //        objVen.PhoneNumber = obj.PhoneNumber;
    //        objVen.Fax = obj.FaxNumber;
    //        objVen.Terms = obj.Terms;
    //        objVen.Type = obj.Type;
    //        objVen.InactiveFlag = obj.InactiveFlag;

    //        objVen.Address1 = obj.Address1;
    //        objVen.Address2 = obj.Address2;
    //        objVen.Address3 = obj.Address3;
    //        objVen.AddressCity = obj.AddressCity;
    //        objVen.AddressState = obj.AddressState;
    //        objVen.PostalCode = obj.AddressPostCode;
    //        objVen.Country = obj.AddressCountry;

    //        objVen.RemitUseBusiness = obj.RemitUseBusiness;
    //        objVen.RemitAddress1 = obj.RemitAddress1;
    //        objVen.RemitAddress2 = obj.RemitAddress2;
    //        objVen.RemitAddress3 = obj.RemitAddress3;
    //        objVen.RemitAddressCity = obj.RemitCity;
    //        objVen.RemitAddressState = obj.RemitState;
    //        objVen.RemitPostalCode = obj.RemitPostCode;
    //        objVen.RemitCountry = obj.RemitCountry;
    //        return objVen;
    //    }
    //    public Vendors PopulateDropdownControls(Vendors objVendors)
    //    {
    //        List<DataContracts.LookupList> objLookupFOBCode = new Models.LookupList().RetrieveAll(_dbKey).Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();
    //        List<DataContracts.LookupList> objLookupTerms = new Models.LookupList().RetrieveAll(_dbKey).Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();
    //        List<DataContracts.LookupList> objLookupType = new Models.LookupList().RetrieveAll(_dbKey).Where(x => x.ListName == LookupListConstants.VENDOR_TYPE).ToList();

    //        objVendors.LookupFOBList = objLookupFOBCode.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
    //        objVendors.LookupTermList = objLookupTerms.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
    //        objVendors.LookupTypeList = objLookupType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });

    //        return objVendors;
    //    }
    //    public Vendor VendorEdit(DatabaseKey _dbKey, Vendors _vendor, long objectId)
    //    {
    //        string emptyString = string.Empty;

    //        Vendor objVendor = new Vendor()
    //        {
    //            ClientId = _dbKey.Client.ClientId,
    //            VendorId = objectId
    //        };
    //        objVendor.Retrieve(_dbKey);

    //        objVendor.SiteId = _dbKey.User.DefaultSiteId;
    //        #region details
    //        objVendor.VendorId = _vendor.VendorId;
    //        objVendor.ClientLookupId = _vendor.ClientLookupId;
    //        objVendor.Name = _vendor.Name != null ? _vendor.Name : emptyString; ;
    //        objVendor.PhoneNumber = _vendor.PhoneNumber != null ? _vendor.PhoneNumber : emptyString;
    //        objVendor.CustomerAccount = _vendor.CustomerAccount != null ? _vendor.CustomerAccount : emptyString;
    //        objVendor.EmailAddress = _vendor.Email != null ? _vendor.Email : emptyString;
    //        objVendor.Website = _vendor.Website != null ? _vendor.Website : emptyString;
    //        objVendor.FOBCode = _vendor.FOBCode != null ? _vendor.FOBCode : emptyString;
    //        objVendor.PhoneNumber = _vendor.PhoneNumber != null ? _vendor.PhoneNumber : emptyString;
    //        objVendor.FaxNumber = _vendor.Fax != null ? _vendor.Fax : emptyString;
    //        objVendor.Terms = _vendor.Terms != null ? _vendor.Terms : emptyString;
    //        objVendor.Type = _vendor.Type != null ? _vendor.Type : emptyString;
    //        objVendor.InactiveFlag = _vendor.InactiveFlag;
    //        #endregion

    //        #region business address 
    //        objVendor.Address1 = _vendor.Address1 != null ? _vendor.Address1 : emptyString;
    //        objVendor.Address2 = _vendor.Address2 != null ? _vendor.Address2 : emptyString;
    //        objVendor.Address3 = _vendor.Address3 != null ? _vendor.Address3 : emptyString;
    //        objVendor.AddressCity = _vendor.AddressCity != null ? _vendor.AddressCity : emptyString;
    //        objVendor.AddressState = _vendor.AddressState != null ? _vendor.AddressState : emptyString;
    //        objVendor.AddressPostCode = _vendor.PostalCode != null ? _vendor.PostalCode : emptyString;
    //        objVendor.AddressCountry = _vendor.Country != null ? _vendor.Country : emptyString;
    //        #endregion

    //        #region remit address
    //        objVendor.RemitUseBusiness = _vendor.RemitUseBusiness;
    //        objVendor.RemitAddress1 = _vendor.RemitAddress1 != null ? _vendor.RemitAddress1 : emptyString;
    //        objVendor.RemitAddress2 = _vendor.RemitAddress2 != null ? _vendor.RemitAddress2 : emptyString;
    //        objVendor.RemitAddress3 = _vendor.RemitAddress3 != null ? _vendor.RemitAddress3 : emptyString; ;
    //        objVendor.RemitCity = _vendor.RemitAddressCity != null ? _vendor.RemitAddressCity : emptyString;
    //        objVendor.RemitState = _vendor.RemitAddressState != null ? _vendor.RemitAddressState : emptyString;
    //        objVendor.RemitPostCode = _vendor.RemitPostalCode != null ? _vendor.RemitPostalCode : emptyString;
    //        objVendor.RemitCountry = _vendor.RemitCountry != null ? _vendor.RemitCountry : emptyString;
    //        #endregion

    //        objVendor.Update(_dbKey);
    //        return objVendor;
    //    }
    //    #endregion

    //    #region Contact
    //    public List<ContactModel> PopulateVendorContact(long _vendorId, DatabaseKey _dbKey)
    //    {
    //        Contact contact = new Contact()
    //        {
    //            ObjectId = _vendorId,
    //            TableName = "Vendor"
    //        };
    //        ContactModel objContact;
    //        List<ContactModel> ContactsList = new List<ContactModel>();
    //        List<Contact> ContactList = contact.RetrieveByObjectId(_dbKey);
    //        foreach (var v in ContactList)
    //        {
    //            objContact = new ContactModel();
    //            objContact.Name = v.Name;
    //            objContact.Phone1 = v.Phone1;
    //            objContact.Email1 = v.Email1;
    //            objContact.OwnerName = v.OwnerName;
    //            objContact.VendorId = v.ObjectId;
    //            objContact.updatedindex = v.UpdateIndex;
    //            objContact.ContactId = v.ContactId;
    //            ContactsList.Add(objContact);
    //        }
    //        return ContactsList;
    //    }

    //    public List<string> ContactAdd(DatabaseKey _dbKey, ContactModel contactModel, long objectId)
    //    {
    //        Contact contact = new Contact()
    //        {
    //            TableName = "Vendor",
    //            OwnerId = _dbKey.User.UserInfoId,
    //            OwnerName = _dbKey.User.FullName,
    //            ObjectId = objectId,
    //            Name = contactModel.Name,
    //            Address1 = contactModel.Address1,
    //            Address2 = contactModel.Address2,
    //            Address3 = contactModel.Address3,
    //            AddressCity = contactModel.AddressCity,
    //            AddressCountry = contactModel.AddressCountry,
    //            AddressPostCode = contactModel.AddressPostCode,
    //            AddressState = contactModel.AddressState,
    //            Phone1 = contactModel.Phone1,
    //            Phone2 = contactModel.Phone2,
    //            Phone3 = contactModel.Phone3,
    //            Email1 = contactModel.Email1,
    //            Email2 = contactModel.Email2
    //        };
    //        contact.Create(_dbKey);
    //        return contact.ErrorMessages;
    //    }

    //    public ContactModel ShowEditContact(DatabaseKey _dbKey, long contactId, long vendorId, string ClientLookupId, int updatedIndex)
    //    {
    //        ContactModel _ContactModel = new ContactModel();
    //        Contact contact = new Contact() { ContactId = contactId };
    //        contact.Retrieve(_dbKey);

    //        _ContactModel.Name = contact.Name;
    //        _ContactModel.Phone1 = contact.Phone1;
    //        _ContactModel.Phone2 = contact.Phone2;
    //        _ContactModel.Phone3 = contact.Phone3;
    //        _ContactModel.Email1 = contact.Email1;
    //        _ContactModel.Email2 = contact.Email2;
    //        _ContactModel.VendorId = vendorId;
    //        _ContactModel.ClientLookupId = ClientLookupId;
    //        _ContactModel.ContactId = contactId;
    //        _ContactModel.updatedindex = updatedIndex;
    //        return _ContactModel;
    //    }
    //    public List<string> ContactEdit(DatabaseKey _dbKey, ContactModel contactModel, long objectId)
    //    {
    //        string emptyString = string.Empty;
    //        Contact contact = new Contact() { ContactId = contactModel.ContactId };
    //        contact.Retrieve(_dbKey);

    //        contact.OwnerId = _dbKey.User.UserInfoId;
    //        contact.OwnerName = _dbKey.User.FullName;
    //        contact.TableName = "Vendor";
    //        contact.ObjectId = _objectId;
    //        contact.Name = contactModel.Name;
    //        contact.Address1 = contactModel.Address1 != null ? contactModel.Address1 : emptyString;
    //        contact.Address2 = contactModel.Address2 != null ? contactModel.Address2 : emptyString;
    //        contact.Address3 = contactModel.Address3 != null ? contactModel.Address3 : emptyString;
    //        contact.AddressCity = contactModel.AddressCity != null ? contactModel.AddressCity : emptyString;
    //        contact.AddressCountry = contactModel.AddressCountry != null ? contactModel.AddressCountry : emptyString;
    //        contact.AddressPostCode = contactModel.AddressPostCode != null ? contactModel.AddressPostCode : emptyString;
    //        contact.AddressState = contactModel.AddressState != null ? contactModel.AddressState : emptyString;
    //        contact.Phone1 = contactModel.Phone1 != null ? contactModel.Phone1 : emptyString;
    //        contact.Phone2 = contactModel.Phone2 != null ? contactModel.Phone2 : emptyString;
    //        contact.Phone3 = contactModel.Phone3 != null ? contactModel.Phone3 : emptyString;
    //        contact.Email1 = contactModel.Email1 != null ? contactModel.Email1 : emptyString;
    //        contact.Email2 = contactModel.Email2 != null ? contactModel.Email2 : emptyString;
    //        contact.UpdateIndex = contact.UpdateIndex;

    //        contact.Update(_dbKey);
    //        return contact.ErrorMessages;
    //    }

    //    public bool ContactDelete(DatabaseKey _dbKey, string _contactId)
    //    {
    //        try
    //        {
    //            Contact contact = new Contact();
    //            contact.ContactId = Convert.ToInt64(_contactId);
    //            contact.Delete(_dbKey);
    //            return true;
    //        }
    //        catch(Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }
    //    #endregion

    //    #region Part
    //    public List<PartVendorXrefModel> PopulateParts(DatabaseKey _dbKey, long objectId)
    //    {
    //        Part_Vendor_Xref pvx = new Part_Vendor_Xref()
    //        {
    //            ClientId = _dbKey.Client.ClientId,
    //            SiteId = _dbKey.User.DefaultSiteId,
    //            VendorId = objectId

    //        };
            
    //        List<Part_Vendor_Xref> xList = new List<Part_Vendor_Xref>();
    //        xList = Part_Vendor_Xref.RetrieveListByVendorId(_dbKey, pvx);
    //        PartVendorXrefModel objPartVendorXrefModel;

    //        List<PartVendorXrefModel> xrefList = new List<PartVendorXrefModel>();
    //        foreach (var v in xList)
    //        {
    //            objPartVendorXrefModel = new PartVendorXrefModel();
    //            objPartVendorXrefModel.Part = v.Part_ClientLookupId;
    //            objPartVendorXrefModel.PartDescription = v.Part_Description;
    //            objPartVendorXrefModel.CatalogNumber = v.CatalogNumber;
    //            objPartVendorXrefModel.Manufacturer = v.Manufacturer;
    //            objPartVendorXrefModel.ManufacturerID = v.ManufacturerId;
    //            objPartVendorXrefModel.OrderQuantity = v.OrderQuantity;
    //            objPartVendorXrefModel.OrderUnit = v.OrderUnit;
    //            objPartVendorXrefModel.Price = v.Price;
    //            objPartVendorXrefModel.PartID = v.PartId;
    //            objPartVendorXrefModel.PartVendorXrefId = v.Part_Vendor_XrefId;
    //            objPartVendorXrefModel.PreferredVendor = v.PreferredVendor;
    //            objPartVendorXrefModel.updatedindex = v.UpdateIndex;
    //            objPartVendorXrefModel.VendorId = v.VendorId;
    //            objPartVendorXrefModel.VendorClientLookupId = v.Vendor_ClientLookupId;
    //            xrefList.Add(objPartVendorXrefModel);
    //        }
    //        return xrefList;
    //    }
    //    public List<DataContracts.LookupList> PopulateUnitOfMeasure(DatabaseKey _dbKey)
    //    {
    //        List<DataContracts.LookupList> objLookup = new Models.LookupList().RetrieveAll(_dbKey).Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
    //        return objLookup;
    //    }
    //    public List<string> AddPartVendorXref(DatabaseKey _dbKey, PartVendorXrefModel _PVXModel, long objectId)
    //    {
    //        Part_Vendor_Xref pvx = new Part_Vendor_Xref()
    //        {
    //            ClientId = _dbKey.Client.ClientId,
    //            VendorId = objectId
    //        };
    //        string part_ClientLookupId = _PVXModel.Part;
    //        Part pt = new Part { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = part_ClientLookupId };
    //        pt.RetrieveByClientLookUpIdNUPCCode(_dbKey);
    //        pvx.PartId = pt.PartId;
    //        pvx.CatalogNumber = _PVXModel?.CatalogNumber ?? string.Empty;
    //        pvx.Manufacturer = _PVXModel?.Manufacturer ?? string.Empty;
    //        pvx.ManufacturerId = _PVXModel?.ManufacturerID ?? string.Empty;
    //        pvx.OrderQuantity = _PVXModel?.OrderQuantity ?? 0;
    //        pvx.OrderUnit = _PVXModel?.OrderUnit ?? string.Empty;
    //        pvx.Price = _PVXModel.Price ?? 0;
    //        pvx.PreferredVendor = _PVXModel.PreferredVendor;
    //        pvx.Part_ClientLookupId = _PVXModel.Part;

    //        pvx.ValidateAdd(_dbKey);
    //        if (pvx.ErrorMessages.Count == 0)
    //        {
    //            pvx.Create(_dbKey);
    //        }
    //        return pvx.ErrorMessages;
    //    }
    //    public List<string> UpdatePartVendorXref(DatabaseKey _dbKey, PartVendorXrefModel _PVXModel, long objectId)
    //    {
    //        Part_Vendor_Xref pvx = new Part_Vendor_Xref()
    //        {
    //            ClientId = _dbKey.Client.ClientId,
    //            VendorId = objectId,
    //            Part_Vendor_XrefId = _PVXModel.PartVendorXrefId

    //        };
    //        pvx.Retrieve(_dbKey);
    //        string part_ClientLookupId = _PVXModel.Part;
    //        Part pt = new Part { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = part_ClientLookupId };
    //        pt.RetrieveByClientLookUpIdNUPCCode(_dbKey);

    //        pvx.PartId = pt.PartId;
    //        pvx.CatalogNumber = _PVXModel?.CatalogNumber ?? string.Empty;
    //        pvx.Manufacturer = _PVXModel?.Manufacturer ?? string.Empty;
    //        pvx.ManufacturerId = _PVXModel?.ManufacturerID ?? string.Empty;
    //        pvx.OrderQuantity = _PVXModel?.OrderQuantity ?? 0;
    //        pvx.OrderUnit = _PVXModel?.OrderUnit ?? string.Empty;
    //        pvx.Price = _PVXModel.Price ?? 0;
    //        pvx.PreferredVendor = _PVXModel.PreferredVendor;

    //        pvx.ValidateSave(_dbKey);
    //        if (pvx.ErrorMessages.Count == 0)
    //        {
    //            pvx.Update(_dbKey);
    //        }
    //        return pvx.ErrorMessages;
    //    }
    //    public bool DeletePart(long _PartVendorXrefId, long vendorId)
    //    {            
    //        try
    //        {
    //            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
    //            {
    //                ClientId = _dbKey.Client.ClientId,
    //                Part_Vendor_XrefId = _PartVendorXrefId
    //            };
    //            pvx.Delete(_dbKey);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    #endregion

    //    #region Note
    //    public List<string> AddVendorNotes(NotesModel notesModel, string tableName)
    //    {
    //        Notes notes = new Notes()
    //        {
    //            OwnerId = _dbKey.User.UserInfoId,
    //            OwnerName = _dbKey.User.FullName,
    //            Subject = notesModel.Subject,
    //            Content = notesModel.Content,
    //            Type = notesModel.Type,
    //            TableName = tableName,
    //            ObjectId = notesModel.VendorId,
    //            UpdateIndex = notesModel.updatedindex,
    //            NotesId = notesModel.NotesId
    //        };
    //        if (notesModel.NotesId == 0)
    //        {
    //            notes.Create(_dbKey);
    //        }
    //        else
    //        {
    //            notes.ObjectId = notesModel.VendorId;
    //            notes.Update(_dbKey);
    //        }
    //        return notes.ErrorMessages;
    //    }
    //    public bool DeleteNote(long _notesId)
    //    {           
    //        try
    //        {
    //            Notes notes = new Notes()
    //            {
    //                NotesId = Convert.ToInt64(_notesId)
    //            };
    //            notes.Delete(_dbKey);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }
    //    public NotesModel EditVendorNotes(long objectId, long notesId)
    //    {
    //        NotesModel objNotesModel = new Models.NotesModel();
    //        Notes note = new Notes() { NotesId = notesId };
    //        note.Retrieve(_dbKey);

    //        objNotesModel.NotesId = note.NotesId;
    //        objNotesModel.updatedindex = note.UpdateIndex;
    //        objNotesModel.Subject = note.Subject;
    //        objNotesModel.Content = note.Content;
    //        objNotesModel.ObjectId = objectId;
    //        objNotesModel.updatedindex = note.UpdateIndex;
    //        return objNotesModel;
    //    }
    //    #endregion
    //}
}