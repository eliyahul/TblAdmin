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

/*
 * THIS CLASS IS UNRELATED TO THE GENERAL ADMIN PROJECT DEVELOPMENT IN THE REST OF THIS REPOSITORY.
 * I JUST NEEDED A QUICK PLACE TO TRY STUFF OUT.
 */

namespace TblAdmin.Areas.Production.Controllers
{
    public class ConversionController : BaseController
    {
        
        public ActionResult Convert()
        {
            // upload the file
            ConvertInputModel cim = new ConvertInputModel();
            return View(cim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Convert(ConvertInputModel cim)
        {

            if (ModelState.IsValid)
            {
                string chapterHeadingPattern = Converter.ChapterHeadings[cim.ChapterHeadingTypeID].Pattern;
                Converter myConverter = new Converter(
                    cim.BookNameRaw,
                    cim.AuthorFirstNameRaw,
                    cim.AuthorLastNameRaw,
                    cim.BookFolderPath,
                    cim.FilePath,
                    cim.BookIdFromAdmin,
                    chapterHeadingPattern
                );
                Boolean result = myConverter.Convert();

                // create a zip file
                // serve the zip file

                if (result)
                {
                    // redirect to success view and supply the zip with all the files
                    ViewBag.Results = "Success ! Your files are being sent to you now. ";
                }
                else
                {
                    ViewBag.Results = "Could not find file with pathname: " + cim.FilePath;
                }
            }
            
            string bookNameNoSpaces = Regex.Replace(cim.BookNameRaw, @"\s", "");
            string zipFilePath = cim.BookFolderPath + bookNameNoSpaces + "Files.zip";
            HttpContext.Response.ContentType = "application/zip";
            HttpContext.Response.WriteFile(zipFilePath);
            HttpContext.Response.AddHeader("content-disposition", "fileName=" + bookNameNoSpaces + "-Files.zip");
            return View(cim);
        }
    }
}