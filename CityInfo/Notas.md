Usar la plantilla de .NET 6

propeties -> launchsettings.json: aca hay 2 perfiles que van a ser con los que se va a ejecutar la app.

borra todo lo ref a weatherForecast
 
`WebApplication implementa IApplicationBuilder, es importante saberlo porque te da un mecanismo para configurar una request pipeline.

Ver bien el cap The ASP.NET Core Request Pipeline & Middleware para entender y tener las diapos de los middleware.

El cap 3 habla bien sobre la estructura de MVC.

/*Routing: Routing matches a request URI to an action on a controller*/
/*En net 6 la forma preferida de routing es a traves de endpoint routing +Info en core/fundamentals/routing */

Content Negotiation: The process of selecting the best representation for a given response when there are multiple representations available.