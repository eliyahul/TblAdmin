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
using System.IO;

namespace TblAdmin.Tests.Core.Production.Services
{
    [TestFixture]
    public class ConversionTest
    {
        private Converter converter;

        [SetUp]
        public void init()
        {
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
            int bookIdFromAdmin = 0;
            string chapterHeadingPattern = "^chapter [a-zA-Z0-9:!\'?\", ]{1,}";
            string fileNameSuffix = "_FullBook_EDITED-MANUALLY.txt";

            string bookNameNoSpaces = Regex.Replace(bookNameRaw,  @"\s{0,}", "");
            string bookFolderPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\";
            string filePath = bookFolderPath + bookNameNoSpaces + fileNameSuffix;
            filePath = prefixPath + "my-non-existant-file";
            string expectedResult = "Could not find file with pathname: " + filePath;


            // Act
            converter = new Converter(
                bookNameRaw,
                authorFirstNameRaw,
                authorLastNameRaw,
                bookFolderPath,
                filePath,
                bookIdFromAdmin,
                chapterHeadingPattern
                );
            Boolean result = converter.Convert();

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Converts_a_play_format()
        {
            // Arrange
            string bookNameRaw = "Uncle Vanya";
            string authorFirstNameRaw = "Anton";
            string authorLastNameRaw = "Chekhov";
            int bookIdFromAdmin = 0;
            
            string publisherName = "Gutenberg";
            string fileNameSuffix = "_FullBook_EDITED-MANUALLY.txt";
            string prefixPath = @"C:\Users\User\Documents\Visual Studio 2013\Projects\TblAdmin\TblAdmin.Tests\Core\Production\Fixtures\";
            string bookNameNoSpaces = Regex.Replace(bookNameRaw, @"\s{0,}", "");
            string actualResultsPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + "ActualResults" + @"\";
            string expectedResultsPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + "ExpectedResults" + @"\";
            string filePath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + bookNameNoSpaces + fileNameSuffix;

            string chapterHeadingPattern = "chapter [a-zA-Z0-9:!\'?\", ]{1,}";


            // Act
            converter = new Converter(
                bookNameRaw,
                authorFirstNameRaw,
                authorLastNameRaw,
                actualResultsPath,
                filePath,
                bookIdFromAdmin,
                chapterHeadingPattern
                );
            Boolean result = converter.Convert();
            
            // Assert
            Assert.IsTrue(result);

            compare_actual_and_expected_files(expectedResultsPath, actualResultsPath);

        }

        [Test]
        public void Converts_a_novel_format_modified_with_special_cases()
        {
            // Arrange
            string bookNameRaw = "Manga Touch";
            string authorFirstNameRaw = "Jacqueline";
            string authorLastNameRaw = "Pearce";
            int bookIdFromAdmin = 4421;

            string publisherName = "Orca Currents";
            string fileNameSuffix = "_FullBook_EDITED-MANUALLY_MODIFIED_FOR_TESTING_PARAG_ENDING_PUNCTUATION.txt";
            //string fileNameSuffix = "_FullBook_EDITED-MANUALLY.txt";
            string prefixPath = @"C:\Users\User\Documents\Visual Studio 2013\Projects\TblAdmin\TblAdmin.Tests\Core\Production\Fixtures\";
            string bookNameNoSpaces = Regex.Replace(bookNameRaw, @"\s{0,}", "");
            string actualResultsPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + "ActualResults" + @"\";
            string expectedResultsPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + "ExpectedResults" + @"\";
            string filePath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + bookNameNoSpaces + fileNameSuffix;

            string chapterHeadingPattern = "chapter [a-zA-Z0-9:!\'?\", ]{1,}";
            

            // Act
            
            converter = new Converter(
                bookNameRaw,
                authorFirstNameRaw,
                authorLastNameRaw,
                actualResultsPath,
                filePath,
                bookIdFromAdmin,
                chapterHeadingPattern
                );
            Boolean result = converter.Convert();
            

            // Assert
            Assert.IsTrue(result);

            compare_actual_and_expected_files(expectedResultsPath, actualResultsPath);

        }

        [Test]
        public void Converts_a_novel_format_with_parts_and_chapters()
        {
            // Arrange
            string bookNameRaw = "Anna Karenina";
            string authorFirstNameRaw = "Leo";
            string authorLastNameRaw = "Tolstoy";
            int bookIdFromAdmin = 4425;

            string publisherName = "Gutenberg";
            string fileNameSuffix = "_FullBook_EDITED-MANUALLY-ABBREVIATED_FOR_TESTING.txt";
            
            string prefixPath = @"C:\Users\User\Documents\Visual Studio 2013\Projects\TblAdmin\TblAdmin.Tests\Core\Production\Fixtures\";
            string bookNameNoSpaces = Regex.Replace(bookNameRaw, @"\s{0,}", "");
            string actualResultsPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + "ActualResults" + @"\";
            string expectedResultsPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + "ExpectedResults" + @"\";
            string filePath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\" + bookNameNoSpaces + fileNameSuffix;

            string chapterHeadingPattern = "part [a-zA-Z0-9]{1,}: chapter [a-zA-Z0-9:!\'?\", ]{1,}";

            converter = new Converter(
                 bookNameRaw,
                 authorFirstNameRaw,
                 authorLastNameRaw,
                 actualResultsPath,
                 filePath,
                 bookIdFromAdmin,
                 chapterHeadingPattern
                 );
            Boolean result = converter.Convert();

            // Assert
            Assert.IsTrue(result);

           compare_actual_and_expected_files(expectedResultsPath, actualResultsPath);

        }

        public bool compare_actual_and_expected_files(string expectedResultsPath, string actualResultsPath)
        {
            IEnumerable<String> expectedFilePaths = Directory.EnumerateFiles(expectedResultsPath);
            Assert.AreNotEqual(expectedFilePaths.Count(), 0, "There were no files in the ExpectedResultsDirectory");

            foreach (string path in expectedFilePaths)
            {
                string expectedFileName = Path.GetFileName(path);
                string actualPath = actualResultsPath + expectedFileName;
                Assert.IsTrue(File.Exists(actualPath), "The following file does not exist in Actual Results: " + actualPath);

                string expectedFileString = File.ReadAllText(path);
                string actualFileString = File.ReadAllText(actualPath);

                Assert.AreEqual(expectedFileString, actualFileString, " *** " + expectedFileName + " *** ");
            }

            // Tear Down (only runs if all Asserts pass, so if there is a failure, I can examine the file)
            IEnumerable<String> actualFilePaths = Directory.EnumerateFiles(actualResultsPath);
            foreach (string path in actualFilePaths)
            {
                File.Delete(path);
            }
            return true;
        }

        [Test]
        public void Converts_driver()
        {
            // Arrange
            string bookNameRaw = "Oracle";
            string authorFirstNameRaw = "Alex";
            string authorLastNameRaw = "Van Tol";
            string publisherName = "Orca Currents";
            
            int bookIdFromAdmin = 0;
            
            string fileNameSuffix = "_FullBook_EDITED-MANUALLY.txt";
            string prefixPath = @"C:\Users\User\Documents\clients\Ronnie\Production\Books\";
            string bookNameNoSpaces = Regex.Replace(bookNameRaw, @"\s{0,}", "");
            string bookFolderPath = prefixPath + publisherName + @"\" + bookNameNoSpaces + @"\";
            string filePath = bookFolderPath + bookNameNoSpaces + fileNameSuffix;

            //string chapterHeadingPattern = "part [a-zA-Z0-9]{1,}: chapter [a-zA-Z0-9:!\'?\"-, ]{1,}";
            //string chapterHeadingPattern = "Chapter [a-zA-Z0-9]{1,}: [a-zA-Z0-9:!\'?\"-, ]{1,}";
            string chapterHeadingPattern = "chapter [a-zA-Z0-9:!\'?\"-, ]{1,}";

            converter = new Converter(
                 bookNameRaw,
                 authorFirstNameRaw,
                 authorLastNameRaw,
                 bookFolderPath,
                 filePath,
                 bookIdFromAdmin,
                 chapterHeadingPattern
                 );
            Boolean result = converter.Convert();

            Assert.IsTrue(result);

        }
    }
}