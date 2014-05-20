using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Base.ViewModels;

namespace TblAdmin.Areas.Books.ViewModels.Books
{
    public class DeleteViewModel
    {
        public SearchSortPageViewModel SearchSortPageParams { get; set; }
        public int Id { get; set; }

        public DeleteViewModel() 
        {
            SearchSortPageParams = new SearchSortPageViewModel();
            Id = 0;
        }

        public DeleteViewModel(SearchSortPageViewModel searchSortPageParams, int id)
        {
            SearchSortPageParams = searchSortPageParams;
            Id = id;
        }
    }
}
