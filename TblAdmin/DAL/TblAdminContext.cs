using TblAdmin.Models;
using System.Data.Entity;

namespace TblAdmin.DAL
{
    public class TblAdminContext : DbContext
    {
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}