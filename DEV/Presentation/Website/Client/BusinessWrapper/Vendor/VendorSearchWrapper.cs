using Client.Models;
using DataContracts;
using System.Collections.Generic;
using System.Linq;

namespace Client.BusinessWrapper
{
    //public class VendorSearchWrapper
    //{
    //    private DatabaseKey _dbKey;

    //    public VendorSearchWrapper(DatabaseKey databaseKey)
    //    {
    //        this._dbKey = databaseKey;
    //    }
    //    public List<Vendor> populateVendorDetails()
    //    {
    //        Vendor vendor = new Vendor();
    //        vendor.SiteId = _dbKey.User.DefaultSiteId;
    //        List<Vendor> vendor_list = vendor.RetrieveAll(_dbKey);
    //        return vendor_list;
    //    }
    //    public List<Vendors> getDetailsByInactiveFlag(bool inactiveFlag)
    //    {
    //        Vendor vendor = new Vendor();
    //       // vendor.SiteId = _dbKey.User.DefaultSiteId;
    //        List<Vendor> vendor_list = vendor.RetrieveAll(_dbKey).Where(x => x.InactiveFlag == inactiveFlag).ToList();
    //        List<Vendors> VendorsList = new List<Vendors>();
    //        Vendors objVendor;
    //        foreach (var v in vendor_list)
    //        {
    //            objVendor = new Vendors();
    //            objVendor.ClientId = v.ClientId;
    //            objVendor.VendorId = v.VendorId;
    //            objVendor.ClientLookupId = v.ClientLookupId;
    //            objVendor.Name = v.Name;
    //            objVendor.AddressCity = v.AddressCity;
    //            objVendor.AddressState = v.AddressState;
    //            objVendor.Type = v.Type;
    //            objVendor.Terms = v.Terms;
    //            objVendor.FOBCode = v.FOBCode;
    //            VendorsList.Add(objVendor);
    //        }
    //        return VendorsList;
    //    }
        
    //}
}