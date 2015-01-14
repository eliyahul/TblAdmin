namespace TblAdmin.Areas.Base.ViewModels
{
    public class SearchSortPageViewModel
    {
        public string SearchString { get; set; }
        public string SortCol { get; set; }
        public string SortColOrder { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public const int DEFAULT_PAGE_NUMBER = 1;
        public const int DEFAULT_PAGE_SIZE = 3;

        public SearchSortPageViewModel() 
        { 
            SearchString = "";
            SortCol = "name";
            SortColOrder = "asc";
            Page = DEFAULT_PAGE_NUMBER;
            PageSize = DEFAULT_PAGE_SIZE;
        }

        public SearchSortPageViewModel(
            string searchString, 
            string sortCol, 
            string sortOrder,
            int page,
            int pageSize
            )
        {
            SearchString =  searchString;

            SortCol = "name";
            if (!string.IsNullOrEmpty(sortCol))
            {
                SortCol = sortCol;
            }

            SortColOrder = "asc";
            if (!string.IsNullOrEmpty(sortOrder))
            {
                SortColOrder = sortOrder;
            }

            Page = page;
            if ((page <= 0) || (page > 1000000))
            {
                Page = DEFAULT_PAGE_NUMBER;
            }

            PageSize = pageSize; 
            if ((pageSize <= 0) || (pageSize > 1000000))
            {
                PageSize = DEFAULT_PAGE_SIZE;
            }
            
        }
    }
}
