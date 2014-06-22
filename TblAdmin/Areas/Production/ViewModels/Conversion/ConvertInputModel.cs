using TblAdmin.Areas.Base.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TblAdmin.Areas.Production.ViewModels.Conversion
{
    public class ConvertInputModel
    {
        [Required]
        public string BookNameRaw { get; set; }
        
        [Required]
        public string AuthorFirstNameRaw { get; set; }

        [Required]
        public string AuthorLastNameRaw { get; set; }

        [Required]
        public string BookFolderPath { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public int BookIdFromAdmin { get; set; }

        [Required]
        public string ChapterHeadingTypeID { get; set; }
        
    }
}
