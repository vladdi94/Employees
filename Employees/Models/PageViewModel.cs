namespace Employees.Models
{
    public class PageViewModel(int count, int pagenumber, int pagesize)
    {
        public int PageNumber { get; } = pagenumber;
        public int TotalPages { get; } = (int)Math.Ceiling(count / (double)pagesize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
