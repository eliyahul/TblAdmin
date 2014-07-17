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
        private string saveUploadedFile(HttpPostedFileBase uploadedFile, string uploadDir)
        {
                
            if (uploadedFile.ContentLength > 0)
            {
                string fileName = Path.GetFileName(uploadedFile.FileName);
                string uploadedFilePath = Path.Combine(uploadDir, fileName);
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
            
            if (ModelState.IsValid)
            {
                string tempDirPhysicalPath = Server.MapPath(TEMP_DIR);

                // Upload to the book folder
                string uploadedFilePath = saveUploadedFile(cim.BookFile, tempDirPhysicalPath);
                if (uploadedFilePath == "")
                {
                    ViewBag.Results = "Uploaded file was empty.";
                    return View(cim);
                }

                //Convert
                Converter myConverter = new Converter(
                    cim.BookNameRaw,
                    cim.AuthorFirstNameRaw,
                    cim.AuthorLastNameRaw,
                    tempDirPhysicalPath,
                    uploadedFilePath,
                    cim.BookIdFromAdmin,
                    cim.ChapterHeadingTypeID
                );

                Boolean result = myConverter.Convert();
                if (!result)
                {
                    ViewBag.Results = "Could not find uploaded file";
                    return View(cim);
                }

                serveZipFile(myConverter.ZipFilePath, myConverter.ZipFileName);
                
            }

            ViewBag.Results = "Success ! Your files are being sent to you now. ";
            return View(cim);
        }
    }
}