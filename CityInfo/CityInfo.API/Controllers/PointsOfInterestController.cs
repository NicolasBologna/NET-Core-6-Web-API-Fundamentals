using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsOfInterest")] //OJOOO ACA, esto es super clave. Ya que esto es hijo de cities necesito que primero me indique la ciudad!
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId) // Aca es lo mismo retornar IActionResult. TODO: ver por que.
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
                return NotFound();

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")] // El name se lo da para usarlo en el POST.
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
                return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);

            if (pointOfInterest == null)
                return NotFound();

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city is null)
            {
                return NotFound();
            }

            var maxPointOfInteresId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

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
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(c => c.Id == cityId);
            if (city is null)
                return NotFound();

            var pointOfInterestToRemove = CitiesDataStore.Current.Cities
                .FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterestToRemove is null)
                return NotFound();

            CitiesDataStore.Current.Cities
                .Remove(pointOfInterestToRemove);

            return NoContent();
        }
    }
}
