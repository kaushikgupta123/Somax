using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOMAX.G4.Data.INTDataLayer.EL
{
  public class PartEL
  {
    public Int64 ClientId
    { set; get; }
    public Int64 UserInfoId
    { set; get; }
    public string UserName
    { set; get; }
    public Int64 PartId
    { set; get; }
    public Int64 SiteId
    { set; get; }
    public Int64 AreaId
    { set; get; }
    public Int64 DepartmentId
    { set; get; }
    public Int64 StoreroomId
    { set; get; }
    public string ClientLookupId
    { set; get; }
    public string Description
    { set; get; }
    public DateTime CreateDate
    { set; get; }
  }
}
