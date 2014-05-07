using TblAdmin.Areas.Books.Models;

namespace TblAdmin.Areas.Books.ViewModels
{
    public class IndexViewModel
    {
        public string searchString { get; set; }
        public string currentSortCol { get; set; }
        public string currentSortOrder { get; set; }
        public string nextSortOrder { get; set; }
        public int? page { get; set; }
        public int? pageSize { get; set; }
        public PagedList.IPagedList<Book> books { get; set; }
    }
}
