namespace DataContracts
{
    public interface IPermission
    {
        long SiteId { get; set; }
        long AreaId { get; set; }
        long DepartmentId { get; set; }
        long StoreroomId { get; set; }
    }
}
