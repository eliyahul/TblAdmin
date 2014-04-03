using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TblAdmin.Models
{
    public class Publisher
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}