using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Base.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TblAdmin.Areas.Books.ViewModels.Books
{
    public class CreateInputModel
    {
        public SearchSortPageViewModel SearchSortPageParams { get; set; }
        public Book Book { get; set; }

        public CreateInputModel() 
        {
            SearchSortPageParams = new SearchSortPageViewModel();
            Book = null;
        }

        public CreateInputModel(SearchSortPageViewModel searchSortPageParams, Book book)
        {
            SearchSortPageParams = searchSortPageParams;
            Book = book;
        }
    }
}
