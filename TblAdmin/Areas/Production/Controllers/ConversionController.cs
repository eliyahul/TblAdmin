using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace TblAdmin.Areas.Production.Controllers
{
    public class ConversionController : Controller
    {
        static string bookName = "MangaTouch";
        static string publisherName = "Orca Currents";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
        static string publisherPartialPath = publisherName + @"\";
        static string bookPartialPath = bookName + @"\";
        static string filePath = prefixPath + publisherPartialPath + bookPartialPath + bookName + "_FullBook_EDITED-MANUALLY.txt";
        static string fileString;
        static string blankLine = Environment.NewLine + Environment.NewLine;

        // GET: Production/Conversion
        public ActionResult Process()
        {
            bool fileExists = System.IO.File.Exists(filePath);
            if (!fileExists)
            {
                ViewBag.Results = "Could not find file with pathname: " + filePath;
                return View();
            }

            // Read file into a string
            fileString = System.IO.File.ReadAllText(filePath, Encoding.GetEncoding(1252));

            fileString = fileString.Trim();

            // Standardize the chapter heading into Camel Case followed by period.
            fileString = Regex.Replace(
                fileString, 
                "^chapter [a-z]{3,}", 
                delegate(Match match)
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    string v = match.ToString();
                    return textInfo.ToTitleCase(v) + ".";
                },
                RegexOptions.IgnoreCase | RegexOptions.Multiline
            );

            // Replace blank lines between paragraphs with ######'s temporarily to mark them
            fileString = Regex.Replace(
                fileString,
                blankLine,
                "######"
            );
            // Replace all whitespace with a space to remove any special chars and line breaks
            fileString = Regex.Replace(
                fileString,
                @"\s{1,}",
                " "
            );
            // Replace paragraph markers ###### with blank line, removing any other space between paragraphs
            fileString = Regex.Replace(
                fileString,
                @"\s{0,}######\s{0,}",
                blankLine
            );
            

            // Split into files based on the Chapter headings
            string chapterPartialPath = prefixPath + publisherPartialPath + bookPartialPath + "chapter-";
            string chapterPartialPathNumbered = "";
            string chapterPathTxt = "";
            string chapterPathHtml = "";
            string chapterHeadingPattern = @"(?=chapter [a-z]{3,})";// positive lookahead to include the chapter headings
            
            int i = 0;
            foreach (string s in Regex.Split(fileString, chapterHeadingPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase))
            {
                chapterPartialPathNumbered = chapterPartialPath + i.ToString("D3");
                chapterPathTxt = chapterPartialPathNumbered + ".txt";
                chapterPathHtml = chapterPartialPathNumbered + ".html";
                
                if (i > 0)
                {
                    string s_html = s.Trim();

                    System.IO.File.WriteAllText(chapterPathTxt, s_html, Encoding.GetEncoding(1252));

                    // Add html p tags to paragraph separations
                    s_html = Regex.Replace(
                        s_html,
                        blankLine,
                        @"</p>" + Environment.NewLine + @"<p>"
                    );

                    // Add opening and closing p tags for the chapter.
                    s_html = @"<p>" + s_html + @"</p>";

                    //Add html file header.
                    s_html = 
@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
<title></title>
</head>
<body>
"                   + s_html;

                    //Add html file footer
                    s_html = s_html + 
@"
</body>
</html>";
                    System.IO.File.WriteAllText(chapterPathHtml, s_html);
                }
                i = i + 1;
                
            }
            
            return View();
        }
            
        
    }
}