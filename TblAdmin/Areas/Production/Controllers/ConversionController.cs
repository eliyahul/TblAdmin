using System;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using TblAdmin.Core.Production.Services;

/*
 * WARNING - QUICK AND DIRTY CODE TO MASSAGE SOME OF OUR PROJECT SPECIFIC TEXT FILES.
 * 
 * THIS CLASS IS UNRELATED TO THE GENERAL ADMIN PROJECT DEVELOPMENT IN THE REST OF THIS REPOSITORY.
 * I JUST NEEDED A QUICK PLACE TO TRY STUFF OUT.
 */

namespace TblAdmin.Areas.Production.Controllers
{
    public class ConversionController : Controller
    {
        /*
        static string bookNameRaw = "Manga Touch";
        static string authorFirstNameRaw = "Jacqueline";
        static string authorLastNameRaw = "Pearce";
        static string publisherName = "Orca Currents";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
        static string bookIdFromAdmin = "0000";
        static string existingChapterHeading = "^chapter [a-zA-Z0-9]{1,}";
        string chapterHeadingLookahead = @"(?=chapter [a-zA-Z0-9]{1,})";// positive lookahead to include the chapter headings
        */
        /*
        static string bookNameRaw = "Power Chord";
        static string authorFirstNameRaw = "Ted";
        static string authorLastNameRaw = "Staunton";
        static string publisherName = "Orca Currents";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
        static string bookIdFromAdmin = "0000";
        static string existingChapterHeading = "^chapter [a-zA-Z0-9]{1,}";
        string chapterHeadingLookahead = @"(?=chapter [a-zA-Z0-9]{1,})";// positive lookahead to include the chapter headings
        */
        /*
        static string bookNameRaw = "Anna Karenina";
        static string authorFirstNameRaw = "Leo";
        static string authorLastNameRaw = "Tolstoy";
        static string publisherName = "Gutenberg";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
        static string bookIdFromAdmin = "0000";
        static string existingChapterHeading = "^part [a-zA-Z0-9]{1,}: chapter [a-zA-Z0-9]{1,}";
        string chapterHeadingLookahead = @"(?=part [a-zA-Z0-9]{1,}: chapter [a-zA-Z0-9]{1,})";// positive lookahead to include the chapter headings
        */
        /*
        static string bookNameRaw = "Peter Pan";
        static string authorFirstNameRaw = "James M.";
        static string authorLastNameRaw = "Barrie";
        static string publisherName = "Gutenberg";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
        static string bookIdFromAdmin = "0000";
        static string existingChapterHeading = "^chapter [a-zA-Z0-9:!\'?\", ]{1,}";
        string chapterHeadingLookahead = @"(?=chapter [a-zA-Z0-9:!\'?"", ]{1,})";// positive lookahead to include the chapter headings
        */

        /*
        static string bookNameRaw = "Lady Windermeres Fan";
        static string authorFirstNameRaw = "David";
        static string authorLastNameRaw = "Price";
        static string publisherName = "Gutenberg";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
        static string bookIdFromAdmin = "0000";
        static string existingChapterHeading = "^chapter [a-zA-Z0-9:!\'?\", ]{1,}";
        string chapterHeadingLookahead = @"(?=chapter [a-zA-Z0-9:!\'?"", ]{1,})";// positive lookahead to include the chapter headings
        */
        /*
        static string bookNameRaw = "Major Barbara";
        static string authorFirstNameRaw = "George Bernard";
        static string authorLastNameRaw = "Shaw";
        static string publisherName = "Gutenberg";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
        static string bookIdFromAdmin = "0000";
        static string existingChapterHeading = "^chapter [a-zA-Z0-9:!\'?\", ]{1,}";
        string chapterHeadingLookahead = @"(?=chapter [a-zA-Z0-9:!\'?"", ]{1,})";// positive lookahead to include the chapter headings
        */

        static string bookNameRaw = "Uncle Vanya";
        static string authorFirstNameRaw = "Anton";
        static string authorLastNameRaw = "Chekhov";
        static string publisherName = "Gutenberg";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
        static string bookIdFromAdmin = "0000";
        static string chapterHeadingPattern = "chapter [a-zA-Z0-9:!\'?\", ]{1,}";
        
        string fileNameSuffix = "_FullBook_EDITED-MANUALLY.txt";
        
        

        // GET: Production/Conversion
        public ActionResult Process()
        {

            // Remove spaces in the raw book name, eg."MangaTouch";
            string bookName = Regex.Replace(
                    bookNameRaw,
                    @"\s{0,}",
                    ""
                );
            string bookFolderPath = prefixPath + publisherName + @"\" + bookName + @"\";
            string filePath = bookFolderPath + bookName + fileNameSuffix;

            Converter myConverter = new Converter();
            bool result = myConverter.Convert(
                bookNameRaw,
                authorFirstNameRaw,
                authorLastNameRaw,
                bookFolderPath,
                filePath,
                bookIdFromAdmin,
                chapterHeadingPattern
                );

            if (result)
            {
                ViewBag.Results = "Success";
            }
            else
            {
                ViewBag.Results = "Could not find file with pathname: " + filePath;
            }

            return View();

        }        
        
    }
}