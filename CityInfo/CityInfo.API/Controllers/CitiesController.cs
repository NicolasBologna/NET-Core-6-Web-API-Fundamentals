using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase //controller deriva de ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore)
        {
            this._citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet] //("api/cities") ya no se necesita mas aca pq agregamos el route
        /*public JsonResult GetCities() //Botón derecho peek definition o alt+f12 ||| JsonResults implementa IActionResults
        {
            return new JsonResult( CitiesDataStore.Current.Cities);
        }*/ //Esto lo cambiamos por lo mismo, ahora queremos usar el OkObjectResult de mas abajo.

        [HttpGet] //("api/cities") ya no se necesita mas aca pq agregamos el route
        public ActionResult<IEnumerable<CityDto>> GetCities() //Botón derecho peek definition o alt+f12 ||| JsonResults implementa IActionResults
        {
            return Ok(_citiesDataStore.Cities);
        }

        [HttpGet("{id}")] //el 404 es solo cuando no puede hacer el route, en este caso si le mando un id q no existe no me va a devolver automaticamente un 404 si no un 200.
        /*OJO de no devolver un 500 cuando el error es del usuario.*/
        /*public JsonResult GetCity(int id)
        {
            return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id));
        }*/ //Esto ya no lo usamos poruqe con esto hay que hacer manejes para devolver los Status Codes. -> por eso pasamos al siguiente

        public ActionResult<CityDto> GetCity(int id)
        {
            var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == id);
            if (cityToReturn == null)
                return NotFound();
            return Ok(cityToReturn);
        }
    }
}
