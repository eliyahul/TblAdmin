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
        string tempDirPhysicalPath = "c:\\users\\user\\documents\\visual studio 2013\\Projects\\TblAdmin\\TblAdmin\\Temp";
        string fixturesPath = @"C:\Users\User\Documents\Visual Studio 2013\Projects\TblAdmin\TblAdmin.Tests\Core\Production\Fixtures\";
                
        [SetUp]
        public void init()
        {
        }

        [Test]
        [Ignore("Implementation for this test is not complete yet.")]
        public void AA_example_ignored_testcase_to_keep_the_ignore_attribute_handy()
        {
        }

        void ConvertNonExistantFile()
        {
            converter.Convert();
        }
            

        [Test]
        public void Throws_Exception_for_nonexistant_file()
        {
            // Arrange
            string bookNameRaw = "Uncle Vanya";
            string authorFirstNameRaw = "Anton";
            string authorLastNameRaw = "Chekhov";
            int bookIdFromAdmin = 0;
            string fileName = "my-non-existant-file";

            // Act
            converter = new Converter(
                bookNameRaw,
                authorFirstNameRaw,
                authorLastNameRaw,
                tempDirPhysicalPath,
                fileName,
                bookIdFromAdmin,
                Converter.CHAPTER_AND_NUMBER
                );

            Assert.Throws(
                typeof(FileNotFoundException), 
                new TestDelegate(ConvertNonExistantFile)
            );
        }

        [Test]
        public void Converts_a_play_format()
        {
            // Arrange
            string bookNameRaw = "Uncle Vanya";
            string authorFirstNameRaw = "Anton";
            string authorLastNameRaw = "Chekhov";
            string fileName = "UncleVanya_FullBook_EDITED-MANUALLY.txt";
            int bookIdFromAdmin = 0;
            
            string actualResultsPath = tempDirPhysicalPath + @"\UncleVanya\bookfiles\";
            string expectedResultsPath = fixturesPath + @"Gutenberg\UncleVanya\ExpectedResults\";
            
            // copy fileName from fixturesPath to tempDirPhysicalPath where converter is expecting it
            if (!System.IO.File.Exists(tempDirPhysicalPath + @"\UncleVanya_FullBook_EDITED-MANUALLY.txt"))
            {
                File.Copy(
                    fixturesPath + @"Gutenberg\UncleVanya\UncleVanya_FullBook_EDITED-MANUALLY.txt", 
                    tempDirPhysicalPath + @"\UncleVanya_FullBook_EDITED-MANUALLY.txt"
                );
            }
            // Act
            converter = new Converter(
                bookNameRaw,
                authorFirstNameRaw,
                authorLastNameRaw,
                tempDirPhysicalPath,
                fileName,
                bookIdFromAdmin,
                Converter.CHAPTER_AND_NUMBER
                );
            Boolean result = converter.Convert();
            
            // Assert
            Assert.IsTrue(result);

            compare_actual_and_expected_files(expectedResultsPath, actualResultsPath, "UncleVanya");

            converter.CleanupTempFiles();// controller and tests have to clean up the converters temp files
           
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

            // Act
            
            converter = new Converter(
                bookNameRaw,
                authorFirstNameRaw,
                authorLastNameRaw,
                actualResultsPath,
                filePath,
                bookIdFromAdmin,
                Converter.CHAPTER_AND_NUMBER
                );
            Boolean result = converter.Convert();
            

            // Assert
            Assert.IsTrue(result);

            compare_actual_and_expected_files(expectedResultsPath, actualResultsPath, bookNameNoSpaces);
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
            
            converter = new Converter(
                 bookNameRaw,
                 authorFirstNameRaw,
                 authorLastNameRaw,
                 actualResultsPath,
                 filePath,
                 bookIdFromAdmin,
                 Converter.PART_CHAPTER_AND_NUMBER
                 );
            Boolean result = converter.Convert();

            // Assert
            Assert.IsTrue(result);

            compare_actual_and_expected_files(expectedResultsPath, actualResultsPath, bookNameNoSpaces);
        }

        

        private void compare_actual_and_expected_files(string expectedResultsPath, string actualResultsPath, string bookNameNoSpaces)
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

            // Verify zip file was created.
            string zipFilePath = actualResultsPath + @"../" + bookNameNoSpaces + "-Files.zip";
            Assert.IsTrue(File.Exists(zipFilePath));

            FileInfo fi = new FileInfo(zipFilePath);
            Assert.IsTrue(fi.Length > 0);
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

            converter = new Converter(
                 bookNameRaw,
                 authorFirstNameRaw,
                 authorLastNameRaw,
                 bookFolderPath,
                 filePath,
                 bookIdFromAdmin,
                 Converter.CHAPTER_AND_NUMBER
                 );
            Boolean result = converter.Convert();

            Assert.IsTrue(result);
        }
    }
}