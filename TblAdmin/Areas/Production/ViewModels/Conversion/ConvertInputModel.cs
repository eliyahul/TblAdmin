using TblAdmin.Areas.Base.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TblAdmin.Areas.Production.ViewModels.Conversion
{
    public class ConvertInputModel
    {
        public string BookNameRaw { get; set; }
        public string AuthorFirstNameRaw { get; set; }
        public string AuthorLastNameRaw { get; set; }
        public string BookFolderPath { get; set; }
        public string FilePath { get; set; }
        public int BookIdFromAdmin { get; set; }
        public string ChapterHeadingPattern { get; set; }
        //public IEnumerable<SelectListItem> ChapterHeadingTypes;
        
        public ConvertInputModel()
        {
            BookNameRaw = "";
            AuthorFirstNameRaw = "";
            AuthorLastNameRaw = "";
            BookFolderPath = "";
            FilePath = "";
            BookIdFromAdmin = 0;
            ChapterHeadingPattern = "";
            //ChapterHeadingTypes = null;
        }

        public ConvertInputModel(
            string bookNameRaw,
            string authorFirstNameRaw,
            string authorLastNameRaw,
            string bookFolderPath,
            string filePath,
            int bookIdFromAdmin,
            string chapterHeadingPattern
            )
        {
            BookNameRaw = bookNameRaw;
            AuthorFirstNameRaw = authorFirstNameRaw;
            AuthorLastNameRaw = authorLastNameRaw;
            BookFolderPath = bookFolderPath;
            FilePath = filePath;
            BookIdFromAdmin = bookIdFromAdmin;
            ChapterHeadingPattern = chapterHeadingPattern;
            //ChapterHeadingTypes = null;
        }
    }
}
