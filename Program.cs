using Microsoft.OpenApi.Models;
//using PizzaStore.DB; //without EF core framework
using PizzaStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//using SQLite for persistent database
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";

builder.Services.AddEndpointsApiExplorer();

//using in memory database
/*builder.Services.AddDbContext<PizzaDb>(options =>
{
    options.UseInMemoryDatabase("items");
});*/

builder.Services.AddDbContext<PizzaDb>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API", Description = "Making the Pizzas you love", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API v1");
    });
}

app.MapGet("/", () => "Hello World!");

/* With EF core framework */

//get all pizzas
app.MapGet("/pizzas", async (PizzaDb db) =>
{
    var pizzas = await db.Pizzas.ToListAsync();
    return Results.Ok(pizzas);
});

//get a particular pizza
app.MapGet("/pizza/{id}", async (int id, PizzaDb db) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    return pizza is null ? Results.NotFound() : Results.Ok(pizza);
});

//create a pizza
app.MapPost("/pizza", async (Pizza pizza, PizzaDb db) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizzas/{pizza.Id}", pizza);
});

//update a pizza
app.MapPut("/pizza", async (Pizza pizza, PizzaDb db) =>
{
    db.Pizzas.Update(pizza);
    await db.SaveChangesAsync();
    return Results.Ok(pizza);
});

//delete a pizza
app.MapDelete("/pizza/{id}", async (int id, PizzaDb db) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null)
    {
        return Results.NotFound();
    }
    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});


/* Wthout EF core framework */
//app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));
//app.MapGet("/pizzas", () => PizzaDB.GetPizzas());
//app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));
//app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));
//app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));

app.Run();
