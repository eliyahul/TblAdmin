using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Moq;
using TblAdmin;
using TblAdmin.Core.Production.Services;
using System.Data.Entity;
using PagedList;
using System.Text.RegularExpressions;

namespace TblAdmin.Tests.Core.Production.Services
{
    [TestFixture]
    public class ConversionTest
    {
        private Converter converter;

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
            string bookNameRaw = "Uncle Vanya";
            string authorFirstNameRaw = "Anton";
            string authorLastNameRaw = "Chekhov";
            string publisherName = "Gutenberg";
            string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
            string bookIdFromAdmin = "0000";
            string existingChapterHeading = "^chapter [a-zA-Z0-9:!\'?\", ]{1,}";
            string chapterHeadingLookahead = @"(?=chapter [a-zA-Z0-9:!\'?"", ]{1,})";// positive lookahead to include the chapter headings
            string fileNameSuffix = "_FullBook_EDITED-MANUALLY.txt";

            string bookNameNoSpaces = Regex.Replace(bookNameRaw,  @"\s{0,}", "");
            string bookFolderPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\";
            string filePath = bookFolderPath + bookNameNoSpaces + fileNameSuffix;
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
            Assert.IsFalse(result);
        }

        [Test]
        public void Converts_a_file_which_does_exist()
        {
            // Arrange
            string bookNameRaw = "Uncle Vanya";
            string authorFirstNameRaw = "Anton";
            string authorLastNameRaw = "Chekhov";
            string bookIdFromAdmin = "0000";
            
            string publisherName = "Gutenberg";
            string fileNameSuffix = "_FullBook_EDITED-MANUALLY.txt";
            string prefixPath = @"C:\Users\User\Documents\Visual Studio 2013\Projects\TblAdmin\TblAdmin.Tests\Core\Production\Fixtures\";
            string bookNameNoSpaces = Regex.Replace(bookNameRaw, @"\s{0,}", "");
            string actualResultsPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + "ActualResults" + @"\";
            string expectedResultsPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + "ExpectedResults" + @"\";
            string filePath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + bookNameNoSpaces + fileNameSuffix;
            
            string existingChapterHeading = "^chapter [a-zA-Z0-9:!\'?\", ]{1,}";
            string chapterHeadingLookahead = @"(?=chapter [a-zA-Z0-9:!\'?"", ]{1,})";// positive lookahead to include the chapter headings
            
            
            // Act
            Boolean result = converter.Convert(
                bookNameRaw,
                authorFirstNameRaw,
                authorLastNameRaw,
                actualResultsPath,
                filePath,
                bookIdFromAdmin,
                existingChapterHeading,
                chapterHeadingLookahead
                );

            
            // Assert
            Assert.IsTrue(result);

            IEnumerable<String> expectedFilePaths = System.IO.Directory.EnumerateFiles(expectedResultsPath);
            foreach (string path in expectedFilePaths)
            {
                string expectedFileName = System.IO.Path.GetFileName(path);
                string actualPath = actualResultsPath + expectedFileName;
                Assert.IsTrue(System.IO.File.Exists(actualPath), "The following file does not exist in Actual Results: " + actualPath);

                string expectedFileString = System.IO.File.ReadAllText(path);
                string actualFileString = System.IO.File.ReadAllText(actualPath);

                Assert.AreEqual(expectedFileString, actualFileString, " *** " + expectedFileName + " *** ");
            }

            // Tear Down (only runs if all Asserts pass, so if there is a failure, I can examine the file)
            IEnumerable<String> actualFilePaths = System.IO.Directory.EnumerateFiles(actualResultsPath);
            foreach (string path in actualFilePaths)
            {
                System.IO.File.Delete(path);
            }

        }

    }
}