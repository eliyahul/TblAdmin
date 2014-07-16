using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using TblAdmin.Areas.Base.ViewModels;
using TblAdmin.Core.Production.Services;
using System.Web;

namespace TblAdmin.Areas.Production.ViewModels.Conversion
{
    public class ConvertInputModel
    {
        [Display(Name = "Title")]
        [Required]
        public string BookNameRaw { get; set; }
        
        [Display(Name = "Author's First Name")]
        [Required]
        public string AuthorFirstNameRaw { get; set; }
        
        [Display(Name = "Author's Last Name")]
        [Required]
        public string AuthorLastNameRaw { get; set; }
        
        [Display(Name = "ID from Admin")]
        [Required]
        public int BookIdFromAdmin { get; set; }

        [Display(Name = "Chapter Heading Format")]
        [Required]
        public int ChapterHeadingTypeID { get; set; }
        
        [Display(Name = "Book File")]
        [Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase BookFile { get; set; }
        
        

        public IEnumerable<SelectListItem> ChapterHeadingTypes = new[]
        {
            new SelectListItem 
            { 
                Text = Converter.ChapterHeadings[Converter.CHAPTER_AND_NUMBER].Name, 
                Value = Converter.CHAPTER_AND_NUMBER.ToString(), 
                Selected = true 
            },
            new SelectListItem 
            { 
                Text = Converter.ChapterHeadings[Converter.CHAPTER_NUMBER_AND_NAME].Name, 
                Value = Converter.CHAPTER_NUMBER_AND_NAME.ToString() 
            },
            new SelectListItem 
            { 
                Text = Converter.ChapterHeadings[Converter.PART_CHAPTER_AND_NUMBER].Name, 
                Value = Converter.PART_CHAPTER_AND_NUMBER.ToString() 
            },
            new SelectListItem 
            { 
                Text = Converter.ChapterHeadings[Converter.BOOK_CHAPTER_AND_NUMBER].Name, 
                Value = Converter.BOOK_CHAPTER_AND_NUMBER.ToString() 
            },
            new SelectListItem 
            { 
                Text = Converter.ChapterHeadings[Converter.VOLUME_BOOK_CHAPTER_AND_NUMBER].Name, 
                Value = Converter.VOLUME_BOOK_CHAPTER_AND_NUMBER.ToString() 
            }
        };
        
    }
}
