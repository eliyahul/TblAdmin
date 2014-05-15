using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TblAdmin.Areas.Books.Models
{
    public class Publisher
    {
        public int ID { get; set; }

        [Display(Name = "Publisher")]
        public string Name { get; set; }

        [Display (Name = "Created")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}