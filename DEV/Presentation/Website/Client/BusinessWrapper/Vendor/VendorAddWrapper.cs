using Client.BusinessWrapper.Common;
using Client.Models;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.BusinessWrapper
{
    //public class VendorAddWrapper : CommonWrapper
    //{
    //    private DatabaseKey _dbKey;
    //    private UserData userData;
    //    public VendorAddWrapper(UserData userData):base(userData)
    //    {
    //        this.userData = userData;
    //        _dbKey = userData.DatabaseKey;
    //    }
    //    public Vendors PopulateDropdownControls(DatabaseKey _dbKey)
    //    {
            
    //        Vendors objVendors = new Vendors();
    //        var AllLookUpLists = GetAllLookUpList();
    //        if (AllLookUpLists != null)
    //        {
    //            List<DataContracts.LookupList> objLookupFOBCode = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();
    //            if (objLookupFOBCode != null)
    //            {
    //                objVendors.LookupFOBList = objLookupFOBCode.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
    //            }

    //            List<DataContracts.LookupList> objLookupTerms = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();
    //            if (objLookupTerms != null)
    //            {
    //                objVendors.LookupTermList = objLookupTerms.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
    //            }

    //            List<DataContracts.LookupList> objLookupType = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_TYPE).ToList();
    //            if (objLookupType != null)
    //            {
    //                objVendors.LookupTypeList = objLookupType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
    //            }
    //        }
    //        return objVendors;
    //    }
    //    public Vendor AddVendorDetails(DatabaseKey databaseKey, out bool check, Vendor vendor)
    //    {
    //        vendor.CreateWithValidation(_dbKey);
    //        if (vendor.ErrorMessages.Count == 0)
    //        {
    //            check = false;
    //        }
    //        else
    //        {
    //            check = true;
    //        }
    //        return vendor;
    //    }
    //}
}