using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TblAdmin.Areas.Books.Models
{
    public class Book
    {
        public int ID { get; set; }
        public int causeDBSeedInitToRun { get; set; }
        public string Name { get; set; }

        [Display(Name = "Created")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        
        [Display(Name = "Modified")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedDate { get; set; }

        public int PublisherID { get; set; }
        public virtual Publisher Publisher { get; set; }
        
    }
}
