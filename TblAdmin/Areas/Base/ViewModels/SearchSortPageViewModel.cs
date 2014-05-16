namespace TblAdmin.Areas.Base.ViewModels
{
    public class SearchSortPageViewModel
    {
        public string SearchString { get; set; }
        public string SortCol { get; set; }
        public string SortOrder { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public const int DEFAULT_PAGE_NUMBER = 1;
        public const int DEFAULT_PAGE_SIZE = 3;

        public SearchSortPageViewModel() 
        { 
            SearchString = "";
            SortCol = "name";
            SortOrder = "asc";
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

            SortOrder = "asc";
            if (!string.IsNullOrEmpty(sortOrder))
            {
                SortOrder = sortOrder;
            }
            
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
            
        }
    }
}
