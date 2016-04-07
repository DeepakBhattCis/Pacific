using PacificBBExtranet.Web.Helpers;
using PacificBBExtranet.Web.Helpers.Azure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PacificBBExtranet.Web.Controllers
{
    public class FileController : Controller
    {

        // GET: Files
        //Aca para hacer las cosas correctas no deberia hacer esto, deberia guardar el file en cache, mostrar un preview del lado del cliente
        //y solo subirlo a azure si el usuario da el okay.
        //Pero decidi implementarlo de esta forma, tambien sirve y es mas rapida
        public JsonResult UploadResortLogo()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                var ResortModel = ApplicationServices.PropertyDetailsService.GetUserResort();
                var logoFileName = "logo-" + ResortModel.ResortID;

                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] fileData = target.ToArray();

                var url = new AzureHelper(WebConstants.Azure.connectionString)
                    .SaveToBlob(fileData, WebConstants.Azure.ResortsContainer, logoFileName, "image/jpg");

                return Json(url, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
    }
}