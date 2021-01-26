namespace Autobot.Infrastructure.Model
{
    public class FilterUser
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }
}
