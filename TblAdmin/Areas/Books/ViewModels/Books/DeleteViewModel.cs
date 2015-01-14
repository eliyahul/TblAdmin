using TblAdmin.Areas.Books.Models;
using TblAdmin.Areas.Base.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TblAdmin.Areas.Books.ViewModels.Books
{
    public class DeleteViewModel
    {
        public SearchSortPageViewModel SearchSortPageParams { get; set; }
        public Book Book { get; set; }
        
        public DeleteViewModel() 
        {
            SearchSortPageParams = new SearchSortPageViewModel();
            Book = null;
        }

        public DeleteViewModel(SearchSortPageViewModel searchSortPageParams, Book book)
        {
            SearchSortPageParams = searchSortPageParams;
            Book = book;
        }
    }
}
