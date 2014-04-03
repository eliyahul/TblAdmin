using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TblAdmin.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public int PublisherID { get; set; }
        public virtual Publisher Publisher { get; set; }
        
    }
}
