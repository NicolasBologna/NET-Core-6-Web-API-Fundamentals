Usar la plantilla de .NET 6

propeties -> launchsettings.json: aca hay 2 perfiles que van a ser con los que se va a ejecutar la app.

borra todo lo ref a weatherForecast
 
`WebApplication implementa IApplicationBuilder, es importante saberlo porque te da un mecanismo para configurar una request pipeline.

Ver bien el cap The ASP.NET Core Request Pipeline & Middleware para entender y tener las diapos de los middleware.

El cap 3 habla bien sobre la estructura de MVC. +info https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/overview/understanding-models-views-and-controllers-cs


/*Routing: Routing matches a request URI to an action on a controller*/
/*En net 6 la forma preferida de routing es a traves de endpoint routing +Info en core/fundamentals/routing */

Content Negotiation: The process of selecting the best representation for a given response when there are multiple representations available. +Info en aspnet/core/web-api/advanced/formatting
Esto se tiene q tener en cuenta sobre todo cuando tenemos varios clientes.
.NET por defecto retorna JSON, si queremos otro formato hay que especificar cual vamos a aceptar.
Si queremos que nos patee y no nos devuelva el formato por default (Json) cuando le pedimos un XML hay que agregarle la opción que sigue:

	```C#
	builder.Services.AddControllers(options =>
	{
		options.ReturnHttpNotAcceptable = true;
	});```


Cap 4, passing data to the API hay varias formas de hacerlo y ahi lo menciona

El tema de las validaciones, esta el DataAnottation pero no lo recomienda. En cambio usar fluentValidation. Igual en el curso va a usar dataAnnotations

OJO con el patch que tiene un poco mas de vueltas. Patch básicamente es un json con una lista de operaciones que pueden ser agregar, remover, reemplazar aplicadas a un recurso.

## Cap 5

Hay dos tipos de servicios? Service collection y en el program class?/????? ver en aspnet/core/fundamentals/dependency-injection

Loggin, adentro de appsettings.json esta el Json por env que tiene la config x defecto de loggin