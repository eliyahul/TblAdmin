using TblAdmin.Areas.Base.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TblAdmin.Areas.Production.ViewModels.Conversion
{
    public class ConvertViewModel
    {
        [Display(Name = "Book Title")]
        public string BookNameRaw { get; set; }
        
        [Display(Name = "Author's First Name")]
        public string AuthorFirstNameRaw { get; set; }
        
        [Display(Name = "Author's Last Name")]
        public string AuthorLastNameRaw { get; set; }
        
        [Display(Name = "Path to book's folder")]
        public string BookFolderPath { get; set; }
        
        [Display(Name = "Path to file")]
        public string FilePath { get; set; }
        
        [Display(Name = "Book's ID from Admin")]
        public int BookIdFromAdmin { get; set; }
        
        public const int CHAPTER_AND_NUMBER = 1;
        public const int CHAPTER_NUMBER_AND_NAME = 2;
        public const int PART_CHAPTER_AND_NUMBER = 3;

        //public string ChapterHeadingPattern { get; set; }
        //string chapterHeadingPattern = "chapter [a-zA-Z0-9:!\'?\"-, ]{1,}";
        //string chapterHeadingPattern = "Chapter [a-zA-Z0-9]{1,}: [a-zA-Z0-9:!\'?\"-, ]{1,}";
        //string chapterHeadingPattern = "part [a-zA-Z0-9]{1,}: chapter [a-zA-Z0-9:!\'?\"-, ]{1,}";

        public int ChapterHeadingTypeID { get; set; }
        
        [Display(Name = "Chapter Heading Format")]
        public IEnumerable<SelectListItem> ChapterHeadingTypes = new[]
            {
             new SelectListItem { Text = "Chapter One or Chapter 1", Value = CHAPTER_AND_NUMBER.ToString(), Selected = true },
             new SelectListItem { Text = "Chapter 1: The First Day", Value = CHAPTER_NUMBER_AND_NAME.ToString() },
             new SelectListItem { Text = "Part 1: Chapter 1", Value = PART_CHAPTER_AND_NUMBER.ToString() }
            };
        
        public ConvertViewModel()
        {
            BookNameRaw = "";
            AuthorFirstNameRaw = "";
            AuthorLastNameRaw = "";
            BookFolderPath = "";
            FilePath = "";
            BookIdFromAdmin = 0;
            ChapterHeadingTypeID = CHAPTER_AND_NUMBER;
        }

        public ConvertViewModel(
            string bookNameRaw,
            string authorFirstNameRaw,
            string authorLastNameRaw,
            string bookFolderPath,
            string filePath,
            int bookIdFromAdmin,
            int chapterHeadingTypeID
            )
        {
            BookNameRaw = bookNameRaw;
            AuthorFirstNameRaw = authorFirstNameRaw;
            AuthorLastNameRaw = authorLastNameRaw;
            BookFolderPath = bookFolderPath;
            FilePath = filePath;
            BookIdFromAdmin = bookIdFromAdmin;
            ChapterHeadingTypeID = chapterHeadingTypeID;
        }
    }
}
