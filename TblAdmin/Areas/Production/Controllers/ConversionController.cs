using System;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using TblAdmin.Core.Production.Services;
using TblAdmin.Areas.Production.ViewModels.Conversion;
using TblAdmin.Areas.Base.Controllers;
using System.IO;

/*
 * THIS CLASS IS UNRELATED TO THE GENERAL ADMIN PROJECT DEVELOPMENT IN THE REST OF THIS REPOSITORY.
 * I JUST NEEDED A QUICK PLACE TO TRY STUFF OUT.
 */

namespace TblAdmin.Areas.Production.Controllers
{
    public class ConversionController : BaseController
    {
        private string saveUploadedFile(HttpPostedFileBase uploadedFile)
        {
            const string UPLOAD_DIR = "~/Uploads";
                
            if (uploadedFile.ContentLength > 0)
            {
                string fileName = Path.GetFileName(uploadedFile.FileName);
                string uploadedFilePath = Path.Combine(Server.MapPath(UPLOAD_DIR), fileName);
                uploadedFile.SaveAs(uploadedFilePath);

                return uploadedFilePath;
            }
            return "";
        }

        public ActionResult Convert()
        {
            ConvertInputModel cim = new ConvertInputModel();
            return View(cim);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Convert(ConvertInputModel cim)
        {
            const string TEMP_DIR = "~/Temp";
            string tempBookFolderPath = Server.MapPath(TEMP_DIR);

            if (ModelState.IsValid)
            {
                string uploadedFilePath = saveUploadedFile(cim.BookFile);
                if (uploadedFilePath == "")
                {
                    ViewBag.Results = "Uploaded file was empty.";
                    return View(cim);
                }

                
                //Remove zip file - put it here, because if we put it after we call serveZipFile, since the serving
                // of the zip file is delayed until after view is rendered, it will not have a zip file to
                // serve because it will already have been deleted.
                string bookNameNoSpaces = Regex.Replace(cim.BookNameRaw, @"\s", "");
                string zipFileName = bookNameNoSpaces + "-Files.zip";
                string zipFilePath = Path.Combine(tempBookFolderPath, zipFileName);
                
                if (System.IO.File.Exists(zipFilePath))
                {
                    System.IO.File.Delete(zipFilePath);
                }
                

                string chapterHeadingPattern = Converter.ChapterHeadings[cim.ChapterHeadingTypeID].Pattern;
                Converter myConverter = new Converter(
                    cim.BookNameRaw,
                    cim.AuthorFirstNameRaw,
                    cim.AuthorLastNameRaw,
                    tempBookFolderPath,
                    uploadedFilePath,
                    cim.BookIdFromAdmin,
                    chapterHeadingPattern
                );

                Boolean result = myConverter.Convert();
                if (!result)
                {
                    ViewBag.Results = "Could not find uploaded file";
                    return View(cim);
                }

                serveZipFile(zipFilePath, zipFileName);
                
                //Remove uploaded file
                if (System.IO.File.Exists(uploadedFilePath))
                {
                    System.IO.File.Delete(uploadedFilePath);
                }

                //Remove all temporary generated chapter and title xml files
                string bookFilesSubFolderPath = Path.Combine(tempBookFolderPath, "bookfiles");
                string[] filePaths = Directory.GetFiles(bookFilesSubFolderPath);
                foreach (string filePath in filePaths)
                    System.IO.File.Delete(filePath);

            }

            // remove the uploaded file the temporary generated book files and the zip file.
            ViewBag.Results = "Success ! Your files are being sent to you now. ";
            return View(cim);
        }
    }
}