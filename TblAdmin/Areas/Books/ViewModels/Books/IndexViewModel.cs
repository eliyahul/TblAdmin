using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Base.ViewModels;
using System.Web.Routing;

namespace TblAdmin.Areas.Books.ViewModels.Books
{
    public class IndexViewModel
    {
        public SearchSortPageViewModel SearchSortPageParams { get; set; }
        public PagedList.IPagedList<Book> Books { get; set; }
        public SearchSortPageViewModel PublisherRouteParams { get; set; }
        public SearchSortPageViewModel NameRouteParams { get; set; }
        public SearchSortPageViewModel CreatedDateRouteParams { get; set; }
        public SearchSortPageViewModel ModifiedDateRouteParams { get; set; }
        public SearchSortPageViewModel CreateLinkRouteParams { get; set; }

        public IndexViewModel() 
        {
            SearchSortPageParams = new SearchSortPageViewModel();
            Books = null;
            PublisherRouteParams = null;
            NameRouteParams = null;
            CreatedDateRouteParams = null;
            ModifiedDateRouteParams = null;
            CreateLinkRouteParams = null;

        }

        public IndexViewModel(
            SearchSortPageViewModel searchSortPageParams, 
            PagedList.IPagedList<Book> books,
            SearchSortPageViewModel publisherRouteParams,
            SearchSortPageViewModel nameRouteParams,
            SearchSortPageViewModel createdDateRouteParams,
            SearchSortPageViewModel modifiedDateRouteParams,
            SearchSortPageViewModel createLinkRouteParams
            )
        {
            SearchSortPageParams = searchSortPageParams;
            Books = books;
            PublisherRouteParams = publisherRouteParams;
            NameRouteParams = nameRouteParams;
            CreatedDateRouteParams = createdDateRouteParams;
            ModifiedDateRouteParams = modifiedDateRouteParams;
            CreateLinkRouteParams = createLinkRouteParams;
        }
    }
}
