using TblAdmin.Areas.Books.Models;

namespace TblAdmin.Areas.Books.ViewModels
{
    public class IndexViewModel
    {
        public string SearchString { get; set; }
        public string SortCol { get; set; }
        public string SortOrder { get; set; }
        public string NextSortOrder { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public PagedList.IPagedList<Book> Books { get; set; }

        public const int DEFAULT_PAGE_NUMBER = 1;
        public const int DEFAULT_PAGE_SIZE = 3;

        public IndexViewModel() 
        { 
            SearchString = "";
            SortCol = "name";
            SortOrder = "asc";
            NextSortOrder = "desc";
            Page = DEFAULT_PAGE_NUMBER;
            PageSize = DEFAULT_PAGE_SIZE;
            Books = null;
        }

        public IndexViewModel(
                string searchString, 
                string sortCol, 
                string sortOrder,
                string nextSortOrder,
                int page,
                int pageSize,
                PagedList.IPagedList<Book> books
            )
        {
            SearchString =  searchString;

            SortCol = "name";
            if (!string.IsNullOrEmpty(sortCol))
            {
                SortCol = sortCol;
            }

            SortOrder = "asc";
            if (!string.IsNullOrEmpty(sortOrder))
            {
                SortOrder = sortOrder;
            }

            NextSortOrder = nextSortOrder;
            
            if ((page <= 0) || (page > 1000000))
            {
                Page = 1;
            }
            else
            {
                Page = page;
            }

            if ((pageSize <= 0) || (pageSize > 1000000))
            {
                PageSize = 3;
            }
            else
            {
                PageSize = pageSize;
            }
            
            Books = books;
        }
    }
}
