using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.IO;

namespace TblAdmin.Core.Production.Services
{
    public class Converter
    {
        string BlankLine = Environment.NewLine + Environment.NewLine;
        string FileString;

        string BookNameRaw { get; set; }
        string AuthorFirstNameRaw { get; set; }
        string AuthorLastNameRaw { get; set; }

        string BookFolderPath { get; set; }
        string FilePath { get; set; }
        string BookIdFromAdmin { get; set; }

        string ChapterHeadingPattern { get; set; }
        
        public Converter(
            string bookNameRaw,
            string authorFirstNameRaw,
            string authorLastNameRaw,
            string bookFolderPath,
            string filePath,
            string bookIdFromAdmin,
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
        }
            
        public bool Convert()
        {
            if (!GetBookFileAsString())
            {
                return false;
            }
            
            FileString.Trim();
            TitleCaseTheChapterHeadings();
            RemovePageHeadersAndFooters();
            RemoveBlankLinesWithinSentences();
            AddMarkersBetweenParagraphs();
            RemoveSpecialCharacters();
            AddEndOfParagraphPunctuation();
            AllowPunctuationInsideQuotesToEndAParagraph();
            RestoreBlankLineBetweenParagraphs();
            
            int numChapters = SplitIntoChapterFiles();
            CreateTitlesXMLFile(numChapters);
            
            return true;
        }
            
       public bool GetBookFileAsString ()
       {
            bool fileExists = File.Exists(FilePath);
            if (fileExists)
            {
                FileString = File.ReadAllText(FilePath, Encoding.GetEncoding(1252));
            }

            return fileExists;
       }

       public string GenerateTitlesXMLAsString(int numChapters)
       {
           string titlesXMLAsString = "";
           string bookNameNoSpaces = Regex.Replace(BookNameRaw, @"\s{0,}", "");

           titlesXMLAsString =
@"<?xml version=""1.0"" encoding=""iso-8859-1""?>
<library>
	<items>
		<book> 
			<title>" + BookNameRaw + @"</title>
			<author>" + AuthorLastNameRaw + ", " + AuthorFirstNameRaw + @"</author>
			<bookFolder>" + bookNameNoSpaces + @"</bookFolder>
			<numChapters>" + numChapters.ToString() + @"</numChapters>
			<ra>n</ra>
		</book>
	</items>
</library>
";
           return titlesXMLAsString;
       }

       public void TitleCaseTheChapterHeadings()
       {
           FileString = Regex.Replace(
               FileString,
               ChapterHeadingPattern,
               delegate(Match match)
               {
                   TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                   string v = match.ToString();
                   return textInfo.ToTitleCase(v);
               },
               RegexOptions.IgnoreCase | RegexOptions.Multiline
           );
       }

       public void RemovePageHeadersAndFooters()
       {
           string NumericPageNum = @"\d{1,}";
        
           // Remove page numbers alone on its own line (usually means its part of page header or footer)
           FileString = Regex.Replace(
               FileString,
               @"\s{0,}" + Environment.NewLine + @"\s{0,}" + NumericPageNum + @"\s{0,}" + Environment.NewLine + @"\s{0,}",
               BlankLine
           );

           // Remove title alone on its own line (usually means its part of page header or footer)
           FileString = Regex.Replace(
               FileString,
               @"\s{0,}" + Environment.NewLine + @"\s{0,}" + BookNameRaw + @"\s{0,}" + Environment.NewLine + @"\s{0,}",
               BlankLine,
               RegexOptions.IgnoreCase
           );

           // Remove author alone on its own line (usually means its part of page header or footer)
           string authorFullName = AuthorFirstNameRaw + " " + AuthorLastNameRaw;
           FileString = Regex.Replace(
               FileString,
               @"\s{0,}" + Environment.NewLine + @"\s{0,}" + authorFullName + @"\s{0,}" + Environment.NewLine + @"\s{0,}",
               BlankLine,
               RegexOptions.IgnoreCase
           );
       }

       public void RemoveBlankLinesWithinSentences()
       {
           // Assume it is within a sentence, if there is no ending punctuation before the blank line,
           // and the first letter in the word after the blank line is not capitalized.

           FileString = Regex.Replace(
               FileString,
               @"[a-zA-Z,;]{1,}" + @"\s{0,}" + BlankLine + @"\s{0,}" + @"[a-z]{1,}",
               delegate(Match match)
               {
                   string v = match.ToString();
                   return Regex.Replace(
                       v,
                       @"\s{0,}" + BlankLine + @"\s{0,}",
                       " "
                   );
               }
           );
       }
       public void AddMarkersBetweenParagraphs()
       {
           // Replace blank lines (and whitespace) between paragraphs with ######'s temporarily as paragraph markers.
           FileString = Regex.Replace(
               FileString,
               @"\s{0,}" + BlankLine + @"\s{0,}",
               "######"
           );
       }

       public void RemoveSpecialCharacters()
       {
           // Replace all remaining whitespace with a space to remove any special chars and line breaks
           FileString = Regex.Replace(
               FileString,
               @"\s{1,}",
               " "
           );
       }

       public void AddEndOfParagraphPunctuation()
       {
           // Prefix paragraph marker by period to make all paragraphs end in period.
           FileString = Regex.Replace(
               FileString,
               "######",
               ".######"
           );

           // Replace ".." at end of paragraph (just before the paragraph marker) with just "."
           FileString = Regex.Replace(
               FileString,
               @"\.\.######",
               ".######"
           );
           // Replace "?." at end of paragraph (just before the paragraph marker) with just "?"
           FileString = Regex.Replace(
               FileString,
               @"\?\.######",
               "?######"
           );
           // Replace "!." at end of paragraph (just before the paragraph marker) with just "!"
           FileString = Regex.Replace(
               FileString,
               @"\!\.######",
               "!######"
           );
       }

       public void AllowPunctuationInsideQuotesToEndAParagraph()
       {
           string encodedRdquo = @"\u201D";
           string decodedRdquo = HttpUtility.HtmlDecode("&rdquo;");
             
           // ---------------------------
           // Working with regular quotes
           // ---------------------------

           // Replace ".""." at end of paragraph (just before the paragraph marker) with just "."""
           FileString = Regex.Replace(
               FileString,
               @"\.""\.######",
               @".""######"
           );

           // Replace "?""." at end of paragraph (just before the paragraph marker) with just "."""
           FileString = Regex.Replace(
               FileString,
               @"\?""\.######",
               @"?""######"
           );

           // Replace "!""." at end of paragraph (just before the paragraph marker) with just "."""
           FileString = Regex.Replace(
               FileString,
               @"\!""\.######",
               @"!""######"
           );

           // Replace "-""." at end of paragraph (just before the paragraph marker) with just "..."""
           FileString = Regex.Replace(
               FileString,
               @"\-""\.######",
               @"...""######"
           );


           // -------------------
           // Working with &rdquo
           // -------------------

           // Replace ".&rdquo;." at end of paragraph (just before the paragraph marker) with just ".&rdquo;"
           FileString = Regex.Replace(
               FileString,
               @"\." + encodedRdquo + @"\.######",
               @"." + decodedRdquo + "######"
           );

           // Replace "?&rdquo;." at end of paragraph (just before the paragraph marker) with just "?&rdquo;"
           FileString = Regex.Replace(
               FileString,
               @"\?" + encodedRdquo + @"\.######",
               @"?" + decodedRdquo + @"######"
           );

           // Replace "!&rdquo;." at end of paragraph (just before the paragraph marker) with just "!&rdquo;"
           FileString = Regex.Replace(
               FileString,
               @"\!" + encodedRdquo + @"\.######",
               @"!" + decodedRdquo + @"######"
           );

           // Replace "-&rdquo;." at end of paragraph (just before the paragraph marker) with just "...&rdquo;"
           FileString = Regex.Replace(
               FileString,
               @"\-" + encodedRdquo + @"\.######",
               @"..." + decodedRdquo + "######"
           );
       }
       public void RestoreBlankLineBetweenParagraphs()
       {
           // Replace temporary paragraph markers ###### with blank line
           FileString = Regex.Replace(
                FileString,
                "######",
                BlankLine
            );
       }
       public void CreateTitlesXMLFile(int numChapters)
       {
           string titlesXMLAsString = GenerateTitlesXMLAsString(numChapters);
           string titlesXMLPath = BookFolderPath + BookIdFromAdmin + ".xml";

           File.WriteAllText(titlesXMLPath, titlesXMLAsString);
       }

       public int SplitIntoChapterFiles()
       {
           // Split into files based on the Chapter headings
           int chapterNum = 0;
           string chapterHeadingLookahead = @"(?=" + ChapterHeadingPattern + @")";// positive lookahead to include the chapter headings

           foreach (string s in Regex.Split(FileString, chapterHeadingLookahead, RegexOptions.Multiline | RegexOptions.IgnoreCase))
           {
               string chapterPathTxt = BookFolderPath + "chapter-" + chapterNum.ToString("D3") + ".txt";
               string chapterPathHtml = BookFolderPath + "chapter-" + chapterNum.ToString("D3") + ".html";

               if (chapterNum > 0)
               {
                   string s_html = s.Trim();

                   File.WriteAllText(chapterPathTxt, s_html);

                   s_html = HttpUtility.HtmlEncode(s_html);

                   // Add html p tags to paragraph separations
                   s_html = Regex.Replace(
                       s_html,
                       BlankLine,
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
" + s_html;

                   //Add html file footer
                   s_html = s_html +
@"
</body>
</html>";
                   File.WriteAllText(chapterPathHtml, s_html);
               }
               chapterNum = chapterNum + 1;

           }
           return chapterNum-1;  // return number of chapters.
       }
            
 
    }

    
}