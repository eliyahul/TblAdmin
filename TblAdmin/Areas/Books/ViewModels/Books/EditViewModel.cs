using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Base.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TblAdmin.Areas.Books.ViewModels.Books
{
    public class EditViewModel
    {
        public SearchSortPageViewModel SearchSortPageParams { get; set; }
        public Book Book { get; set; }
        public IEnumerable<SelectListItem> Publishers;

        public EditViewModel()
        {
            SearchSortPageParams = new SearchSortPageViewModel();
            Book = null;
            Publishers = null;
        }

        public EditViewModel(SearchSortPageViewModel searchSortPageParams, Book book, IEnumerable<SelectListItem> publishers)
        {
            SearchSortPageParams = searchSortPageParams;
            Book = book;
            Publishers = publishers;
        }
    }
}
