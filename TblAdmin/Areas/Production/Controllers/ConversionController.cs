using System;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;

namespace TblAdmin.Areas.Production.Controllers
{
    public class ConversionController : Controller
    {
        static string bookNameRaw = "Manga Touch";
        static string authorFirstNameRaw = "Jacqueline";
        static string authorLastNameRaw = "Pearce";
        static string publisherName = "Orca Currents";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";

        // Remove spaces in the raw book name, eg."MangaTouch";
        static string bookName = Regex.Replace(
                bookNameRaw,
                @"\s{0,}",
                ""
            );
        static string fileToWorkOn = bookName + "_FullBook_EDITED-MANUALLY.txt";
        static string existingChapterHeading = "^chapter [a-z]{3,}";
        string chapterHeadingLookahead = @"(?=chapter [a-z]{3,})";// positive lookahead to include the chapter headings
        string authorFullName = authorFirstNameRaw + " " + authorLastNameRaw;
        static string bookFolderPath = prefixPath + publisherName + @"\" + bookName + @"\";
        string filePath = bookFolderPath + fileToWorkOn;
        string fileString;
        string blankLine = Environment.NewLine + Environment.NewLine;
        //string myRdquo = @"\u201D";
        string numericPageNum = @"\d{1,}";

        // GET: Production/Conversion
        public ActionResult Process()
        {
            ViewBag.Results = "Success";
            
            // Verify file exists.
            bool fileExists = System.IO.File.Exists(filePath);
            if (!fileExists)
            {
                ViewBag.Results = "Could not find file with pathname: " + filePath;
                return View();
            }

            // Read file into a string and trim it.
            fileString = System.IO.File.ReadAllText(filePath, Encoding.GetEncoding(1252));
            fileString = fileString.Trim();

            // Standardize the chapter heading into Camel Case followed by period.
            fileString = Regex.Replace(
                fileString, 
                existingChapterHeading, 
                delegate(Match match)
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    string v = match.ToString();
                    return textInfo.ToTitleCase(v) + ".";
                },
                RegexOptions.IgnoreCase | RegexOptions.Multiline
            );

            // Remove page numbers alone on its own line (usually means its part of page header or footer)
            fileString = Regex.Replace(
                fileString,
                @"\s{0,}" + Environment.NewLine + @"\s{0,}" + numericPageNum + @"\s{0,}" + Environment.NewLine + @"\s{0,}",
                blankLine
            );

            // Remove title alone on its own line (usually means its part of page header or footer)
            fileString = Regex.Replace(
                fileString,
                @"\s{0,}" + Environment.NewLine + @"\s{0,}" + bookNameRaw + @"\s{0,}" + Environment.NewLine + @"\s{0,}",
                blankLine,
                RegexOptions.IgnoreCase
            );

            // Remove author alone on its own line (usually means its part of page header or footer)
            fileString = Regex.Replace(
                fileString,
                @"\s{0,}" + Environment.NewLine + @"\s{0,}" + authorFullName + @"\s{0,}" + Environment.NewLine + @"\s{0,}",
                blankLine,
                RegexOptions.IgnoreCase
            );

            // Before putting in the paragraph markers, replace blank lines within a sentence with a space. 
            // Assume it is within a sentence, if there is no ending punctuation before the blank line,
            // and the first letter in the word after the blank line is not capitalized.
            
            fileString = Regex.Replace(
                fileString,
                @"\w{1,}" + @"\s{0,}" + blankLine + @"\s{0,}" + @"[a-z]{1,}", 
                delegate(Match match)
                {
                    string v = match.ToString();
                    return Regex.Replace(
                        v,
                        @"\s{0,}" + blankLine + @"\s{0,}",
                        " "
                    );
                },
                RegexOptions.IgnoreCase
            );

            // Replace blank lines (and whitespace) between paragraphs with ######'s temporarily as paragraph markers.
            fileString = Regex.Replace(
                fileString,
                @"\s{0,}" + blankLine + @"\s{0,}",
                "######"
            );
            // Replace all remaining whitespace with a space to remove any special chars and line breaks
            fileString = Regex.Replace(
                fileString,
                @"\s{1,}",
                " "
            );

            // Prefix paragraph marker by period to make all paragraphs end in period.
            fileString = Regex.Replace(
                fileString,
                "######",
                ".######"
            );

            // Replace ".." at end of paragraph (just before the paragraph marker) with just "."
            fileString = Regex.Replace(
                fileString,
                @"\.\.######",
                ".######"
            );
            // Replace "?." at end of paragraph (just before the paragraph marker) with just "?"
            fileString = Regex.Replace(
                fileString,
                @"\?\.######",
                "?######"
            );
            // Replace "!." at end of paragraph (just before the paragraph marker) with just "!"
            fileString = Regex.Replace(
                fileString,
                @"\!\.######",
                "!######"
            );

            // Replace ".""." at end of paragraph (just before the paragraph marker) with just "."""
            fileString = Regex.Replace(
                fileString,
                @"\.""\.######",
                @".""######"
            );

            // Replace "?""." at end of paragraph (just before the paragraph marker) with just "."""
            fileString = Regex.Replace(
                fileString,
                @"\?""\.######",
                @"?""######"
            );

            // Replace "!""." at end of paragraph (just before the paragraph marker) with just "."""
            fileString = Regex.Replace(
                fileString,
                @"\!""\.######",
                @"!""######"
            );

            // Replace "-""." at end of paragraph (just before the paragraph marker) with just "..."""
            fileString = Regex.Replace(
                fileString,
                @"\-""\.######",
                @"...""######"
            );
            /*
            // Replace ".&rdquo;." at end of paragraph (just before the paragraph marker) with just ".&rdquo;"
            fileString = Regex.Replace(
                fileString,
                @"\." + myRdquo + @"\.######",
                @".\&rdquo######"
            );

            // Replace "?&rdquo;." at end of paragraph (just before the paragraph marker) with just "?&rdquo;"
            fileString = Regex.Replace(
                fileString,
                @"\?" + myRdquo + @"\.######",
                @"?" + myRdquo + @"######"
            );

            // Replace "!&rdquo;." at end of paragraph (just before the paragraph marker) with just "!&rdquo;"
            fileString = Regex.Replace(
                fileString,
                @"\!" + myRdquo + @"\.######",
                @"!" + myRdquo + @"######"
            );

            // Replace "-&rdquo;." at end of paragraph (just before the paragraph marker) with just "...&rdquo;"
            fileString = Regex.Replace(
                fileString,
                @"\-" + myRdquo + @"\.######",
                @"..." + myRdquo + "######"
            );
            
            // Replace all "&rdquo;" with "$$$$$"
            fileString = Regex.Replace(
                fileString,
                myRdquo,
                myRdquo
               // @"$$$$$"
            );
            */
            // Replace paragraph markers ###### with blank line
            fileString = Regex.Replace(
                 fileString,
                 "######",
                 blankLine
             );

            // Split into files based on the Chapter headings
            int i = 0;
            foreach (string s in Regex.Split(fileString, chapterHeadingLookahead, RegexOptions.Multiline | RegexOptions.IgnoreCase))
            {
                string chapterPathTxt = bookFolderPath + "chapter-" + i.ToString("D3") + ".txt";
                string chapterPathHtml = bookFolderPath + "chapter-" + i.ToString("D3") + ".html";
                
                if (i > 0)
                {
                    string s_html = s.Trim();
                    s_html = HttpUtility.HtmlEncode(s_html);

                    System.IO.File.WriteAllText(chapterPathTxt, s_html);

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