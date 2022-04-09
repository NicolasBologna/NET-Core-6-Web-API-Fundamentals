#region Esto viene en el template, bueno no todo pq hay cosas q tengo q meter en el medio
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers(); //Se pueden usar varios, pero este es el que menos cosas te mete por ej: builder.Services.AddMvc();
                                       //https://dotnettutorials.net/lesson/difference-between-addmvc-and-addmvccore-method/
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    // OSEA, todo lo que viene desde aca es la pipeline HTTP
    // Esto va pasando desde aca para abajo.
    // Si genera la response en algun momento la response vuelve y no ejecuta lo sig?
    // Lo que no entiendo es por que entra una sola vez.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(); //Esto genera el JSON con la especificación de SWagger
        app.UseSwaggerUI(); //Esto lo unico que hace es renderizar ese JSON
    }

    app.UseHttpsRedirection();

    app.UseRouting(); //Le dice a la pipeline de middlewares donde tomar las decisiones de ruteo. Basicamente aca elije para que endpoint ir

app.UseAuthorization();

    //app.MapControllers(); esto no lo usa pq hace otro approach

#endregion

//Estos 2 ... los agregamos al middleware 
app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
}); // Acá le dice a la pipeline que ejecute el endpoint // TODO: Repasar tood lo de routing

app.Run();
