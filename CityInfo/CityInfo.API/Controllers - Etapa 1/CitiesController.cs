using Microsoft.AspNetCore.Mvc;

/*Routing: Routing matches a request URI to an action on a controller*/
/*En net 6 la forma preferida de routing es a traves de endpoint routing +Info en core/fundamentals/routing */

namespace CityInfo.API.ControllersEtapa1
{
    [ApiController]
    [Route("api/Etapa1/cities")]
    public class CitiesController : ControllerBase //controller deriva de ControllerBase
    {
        [HttpGet] //("api/cities") ya no se necesita mas aca pq agregamos el route
        public JsonResult GetCities()
        {
            return new JsonResult(
                new List<object>
                {
                    new { id = 1, name = "New york City"},
                    new { id = 2, name = "California"}
                });
        }
    }
}
