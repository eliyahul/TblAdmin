using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Base.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TblAdmin.Areas.Books.ViewModels.Books
{
    public class DetailsViewModel
    {
        public SearchSortPageViewModel SearchSortPageParams { get; set; }
        public Book Book { get; set; }

        public DetailsViewModel()
        {
            SearchSortPageParams = new SearchSortPageViewModel();
            Book = null;
        }

        public DetailsViewModel(SearchSortPageViewModel searchSortPageParams, Book book)
        {
            SearchSortPageParams = searchSortPageParams;
            Book = book;
        }
    }
}
