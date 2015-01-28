namespace TblAdmin.Areas.Base.ViewModels
{
    public class SearchSortPageViewModel
    {
        public string SearchString { get; set; }
        public string SortCol { get; set; }
        public string CurrentOrder { get; set; }
        public string NextOrder { get; set; }
        public bool ToggleOrder { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public const int DEFAULT_PAGE_NUMBER = 1;
        public const int DEFAULT_PAGE_SIZE = 3;

        public const string SORT_ORDER_ASC = "asc";
        public const string SORT_ORDER_DESC = "desc";

        public SearchSortPageViewModel() 
        { 
            SearchString = "";
            SortCol = "name";
            NextOrder = SORT_ORDER_DESC;
            CurrentOrder = SORT_ORDER_ASC;
            ToggleOrder = false;
            Page = DEFAULT_PAGE_NUMBER;
            PageSize = DEFAULT_PAGE_SIZE;
        }

        public SearchSortPageViewModel(
            string searchString, 
            string sortCol,
            string currentOrder,
            string nextOrder,
            bool toggleOrder,
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

            CurrentOrder = SORT_ORDER_ASC;
            if (!string.IsNullOrEmpty(nextOrder))
            {
                CurrentOrder = currentOrder;
            }

            NextOrder = SORT_ORDER_DESC;
            if (!string.IsNullOrEmpty(nextOrder))
            {
                NextOrder = nextOrder;
            }

            ToggleOrder = toggleOrder;

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

        public SearchSortPageViewModel ShallowCopy()
        {
            return (SearchSortPageViewModel) this.MemberwiseClone();
        }
    }
}
