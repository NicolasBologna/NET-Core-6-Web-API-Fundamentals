using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        // Para poder devolver un archivo hace falta agregarlo a la carpeta del proyecto y en las propiedades poner un Copy Always.
        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            //Demo, en realidad deberias buscar por id
            var pathToFile = "README.md";

            //Checkeo que el archivo existe
            if (!System.IO.File.Exists(pathToFile))
                return NotFound();

            var file = System.IO.File.ReadAllBytes(pathToFile);
            //return File(file, "text/plain", Path.GetFileName(pathToFile)); //Esto claramente no va a andar pq estamos devolviendo un PDF como texto.

            //Para que ande hay q agregar el servicio FileExtensionContentTypeProvider en program.cs
            if (!_fileExtensionContentTypeProvider.TryGetContentType(
                pathToFile, out var contentType))
            {
                contentType = "application/octet-stream"; //Este tipo es generico para contenido binario.
            }

            return File(file, contentType, Path.GetFileName(pathToFile));

        }
    }
}
