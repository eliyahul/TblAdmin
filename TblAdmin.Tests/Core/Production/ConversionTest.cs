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
            string bookNameRaw = "";
            string authorFirstNameRaw = "";
            string authorLastNameRaw = "";
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
            int bookIdFromAdmin = 0;

            run_converter(bookNameRaw, authorFirstNameRaw, authorLastNameRaw, bookIdFromAdmin);
        }

        [Test]
        public void Converts_a_novel_format_modified_with_special_cases()
        {
            // Arrange
            string bookNameRaw = "Manga Touch";
            string authorFirstNameRaw = "Jacqueline";
            string authorLastNameRaw = "Pearce";
            int bookIdFromAdmin = 4421;

            run_converter(bookNameRaw, authorFirstNameRaw, authorLastNameRaw, bookIdFromAdmin);
        }

        [Test]
        public void Converts_a_novel_format_with_parts_and_chapters()
        {
            // Arrange
            string bookNameRaw = "Anna Karenina";
            string authorFirstNameRaw = "Leo";
            string authorLastNameRaw = "Tolstoy";
            int bookIdFromAdmin = 4425;

            run_converter(bookNameRaw, authorFirstNameRaw, authorLastNameRaw, bookIdFromAdmin);
        }

        private void run_converter(string bookNameRaw, string authorFirstNameRaw, string authorLastNameRaw, int bookIdFromAdmin)
        {
            string bookNameNoSpaces = Regex.Replace(bookNameRaw, @"\s{0,}", "");

            string editedFileSuffix = "_FullBook_EDITED-MANUALLY.txt";
            string fileName = bookNameNoSpaces + editedFileSuffix;
            
            string actualResultsPath = tempDirPhysicalPath + @"\" + bookNameNoSpaces + @"\bookfiles\";
            string expectedResultsPath = fixturesPath + bookNameNoSpaces + @"\ExpectedResults\";

            // copy fileName from fixturesPath to tempDirPhysicalPath where converter is expecting it
            if (!System.IO.File.Exists(tempDirPhysicalPath + @"\" + fileName))
            {
                File.Copy(
                    fixturesPath + bookNameNoSpaces + @"\" + fileName,
                    tempDirPhysicalPath + @"\" + fileName
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

            compare_actual_and_expected_files(expectedResultsPath, actualResultsPath, bookNameNoSpaces);

            //converter.CleanupTempFiles();// controller and tests have to clean up the converters temp files
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
            int bookIdFromAdmin = 4441;

            run_converter(bookNameRaw, authorFirstNameRaw, authorLastNameRaw, bookIdFromAdmin);
        }
    }
}