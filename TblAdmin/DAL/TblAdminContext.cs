using TblAdmin.Models;
using TblAdmin.Areas.Books.Models;
using System.Data.Entity;

namespace TblAdmin.DAL
{
    public class TblAdminContext : DbContext
    {
        // virtual - to allow for mocking.

        public virtual DbSet<Publisher> Publishers { get; set; } 
        public virtual DbSet<Book> Books { get; set; }
    }
}