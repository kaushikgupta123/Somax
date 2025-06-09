namespace Client.Models
{
    public class DataTableLanguageRoot
    {
        public DataTableLanguageRoot()
        {
            oPaginate = new OPaginate();
            oAria = new OAria();
        }
        public string sEmptyTable { get; set; }
        public string sInfo { get; set; }
        public string sInfoEmpty { get; set; }
        public string sInfoFiltered { get; set; }
        public string sInfoPostFix { get; set; }
        public string sInfoThousands { get; set; }
        public string sLengthMenu { get; set; }
        public string sLoadingRecords { get; set; }
        public string sProcessing { get; set; }
        public string sSearch { get; set; }
        public string sZeroRecords { get; set; }
        public OPaginate oPaginate { get; set; }
        public OAria oAria { get; set; }
    }
    public class OPaginate
    {
        public string sFirst { get; set; }
        public string sPrevious { get; set; }
        public string sNext { get; set; }
        public string sLast { get; set; }
    }

    public class OAria
    {
        public string sSortAscending { get; set; }
        public string sSortDescending { get; set; }
    }

}