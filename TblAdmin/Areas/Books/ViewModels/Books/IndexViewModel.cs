using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Base.ViewModels;

namespace TblAdmin.Areas.Books.ViewModels
{
    public class IndexViewModel
    {
        public SearchSortPageViewModel SearchSortPageParams { get; set; }
        public PagedList.IPagedList<Book> Books { get; set; }

        public IndexViewModel() 
        {
            SearchSortPageParams = new SearchSortPageViewModel();
            Books = null;
        }

        public IndexViewModel(SearchSortPageViewModel searchSortPageParams, PagedList.IPagedList<Book> books)
        {
            SearchSortPageParams = searchSortPageParams;
            Books = books;
        }
    }
}
