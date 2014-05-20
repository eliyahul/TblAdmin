using TblAdmin.Areas.Base.ViewModels;

namespace TblAdmin.Areas.Base.ViewModels
{
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
