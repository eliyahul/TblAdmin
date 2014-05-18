using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Base.ViewModels;

namespace TblAdmin.Areas.Books.ViewModels.Books
{
    public class EditViewModel
    {
        public SearchSortPageViewModel SearchSortPageParams { get; set; }
        public int Id { get; set; }

        public EditViewModel() 
        {
            SearchSortPageParams = new SearchSortPageViewModel();
            Id = 0;
        }

        public EditViewModel(SearchSortPageViewModel searchSortPageParams, int id)
        {
            SearchSortPageParams = searchSortPageParams;
            Id = id;
        }
    }
}
