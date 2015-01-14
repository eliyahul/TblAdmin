using TblAdmin.Areas.Base.ViewModels;

namespace TblAdmin.Areas.Base.ViewModels
{
    // This class is needed in every admin controller to pass around the SearchSortPage params in the url 
    // from page to page.

    public class RecordViewModel
    {
        public SearchSortPageViewModel SearchSortPageParams { get; set; }
        public int Id { get; set; }

        public RecordViewModel() 
        {
            SearchSortPageParams = new SearchSortPageViewModel();
            Id = 0;
        }

        public RecordViewModel(SearchSortPageViewModel searchSortPageParams, int id)
        {
            SearchSortPageParams = searchSortPageParams;
            Id = id;
        }
    }
}
