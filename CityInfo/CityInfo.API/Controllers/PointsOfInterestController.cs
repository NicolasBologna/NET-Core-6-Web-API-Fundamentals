using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsOfInterest")] //OJOOO ACA, esto es super clave. Ya que esto es hijo de cities necesito que primero me indique la ciudad!
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService localMailService, CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService)); //Los null check los podes agregar todos con el linter
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId) // Aca es lo mismo retornar IActionResult. TODO: ver por que.
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
                return NotFound();

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")] // El name se lo da para usarlo en el POST.
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            try
            {
                var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"No se encontró la ciudad {cityId}"); // va a ser logueado dependiendo el nivel de log en appsettings.
                    return NotFound();
                }

                var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);

                if (pointOfInterest == null)
                    return NotFound();

                return Ok(pointOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception con la ciudad {cityId}", ex);
                return StatusCode(500, "Ocurrio un problema con tu request."); //Esto es clave.
            }

        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city is null)
            {
                return NotFound();
            }

            var maxPointOfInteresId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var newPointOfInterest = new PointOfInterestDto
            {
                Id = ++maxPointOfInteresId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(newPointOfInterest);

            return CreatedAtRoute(//CreatedAtRoute es para q devuelva 201, el 200 de post.
                "GetPointOfInterest", //El primer parámetro es el Name del endpoint que hace el Get
                new //El segundo los parametros q necesita ese endpoint
                {
                    cityId = cityId,
                    pointOfInterestId = newPointOfInterest.Id
                },
                newPointOfInterest);//El tercero es el objeto creado. 
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
                return NotFound();

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
                return NotFound();

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        //Usamos el DTO de Update pq no tiene ID, si usaramos el base tendriamos que validar manualmente que no este modificando el id.
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city is null)
                return NotFound();

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
                return NotFound();

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities
                .FirstOrDefault(c => c.Id == cityId);
            if (city is null)
                return NotFound();

            var pointOfInterestToRemove = _citiesDataStore.Cities
                .FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestToRemove is null)
                return NotFound();

            _citiesDataStore.Cities
                .Remove(pointOfInterestToRemove);
            _localMailService.Send($"Se borro un punto de interés", $"Se elimino el punto {pointOfInterestToRemove.Name}");

            return NoContent();
        }
    }
}
