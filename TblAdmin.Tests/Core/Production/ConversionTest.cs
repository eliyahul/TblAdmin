using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Moq;
using TblAdmin;
using TblAdmin.Domain.Production.Services;
using System.Data.Entity;
using PagedList;
using System.Text.RegularExpressions;

namespace TblAdmin.Tests.Core.Production.Services
{
    [TestFixture]
    public class ConversionTest
    {
        private Converter converter;

        static string bookNameRaw = "Uncle Vanya";
        static string authorFirstNameRaw = "Anton";
        static string authorLastNameRaw = "Chekhov";
        static string publisherName = "Gutenberg";
        static string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
        static string bookIdFromAdmin = "0000";
        static string existingChapterHeading = "^chapter [a-zA-Z0-9:!\'?\", ]{1,}";
        static string chapterHeadingLookahead = @"(?=chapter [a-zA-Z0-9:!\'?"", ]{1,})";// positive lookahead to include the chapter headings
        static string fileNameSuffix = "_FullBook_EDITED-MANUALLY.txt";

        // Remove spaces in the raw book name, eg."MangaTouch";
        static string bookName = Regex.Replace(
                bookNameRaw,
                @"\s{0,}",
                ""
            );
        static string bookFolderPath = prefixPath + publisherName + @"\" + bookName + @"\";
        string filePath = bookFolderPath + bookName + fileNameSuffix;
        
        [SetUp]
        public void init()
        {
            

            

            
            

            converter = new Converter();
        }

        [Test]
        [Ignore("Implementation for this test is not complete yet.")]
        public void AA_example_ignored_testcase_to_keep_the_ignore_attribute_handy()
        {
        }

        [Test]
        public void Gives_message_for_nonexistant_file()
        {
            // Arrange
            filePath = prefixPath + "my-non-existant-file";
            string expectedResult = "Could not find file with pathname: " + filePath;
            

            // Act
            Boolean result = converter.Convert(
                bookNameRaw,
                authorFirstNameRaw,
                authorLastNameRaw,
                bookFolderPath,
                filePath,
                bookIdFromAdmin,
                existingChapterHeading,
                chapterHeadingLookahead
                );

            // Assert
            Assert.IsFalse (result);
        }
    }
}