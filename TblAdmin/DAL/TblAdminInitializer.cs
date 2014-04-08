using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TblAdmin.Areas.Books.Models;

namespace TblAdmin.DAL
{
    public class TblAdminInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TblAdminContext>
    {
        protected override void Seed(TblAdminContext context)
        {
            //Populate Publishers

            var Publishers = new List<Publisher>
            {
                new Publisher{Name="Raven Tree Press", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Publisher{Name="Candlewick Press", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Publisher{Name="Annick Press", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Publisher{Name="Lobster Press", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Publisher{Name="Somerville House Publishing", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Publisher{Name="Charlesbridge Books", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Publisher{Name="Chronicle Books", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now}
            };

            Publishers.ForEach(p => context.Publishers.Add(p));
            context.SaveChanges();
            
            // Populate Books

            var Books = new List<Book>
            {
                new Book{PublisherID=1, Name="Lobo and the Rabbit Stew", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Book{PublisherID=2, Name="Mercy Watson Goes for a Ride", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Book{PublisherID=3, Name="50 Below Zero", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Book{PublisherID=4, Name="ABC Letters in the Library", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Book{PublisherID=5, Name="Abra Cadabra and the Tooth Witch", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Book{PublisherID=6, Name="Ace Lacewing: Bad Bugs Are My Business", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now},
                new Book{PublisherID=7, Name="Little Oink", CreatedDate=DateTime.Now, ModifiedDate=DateTime.Now}
            };

            Books.ForEach(b => context.Books.Add(b));

            // Save all the changes to the context.

            context.SaveChanges();
        }
    }
}