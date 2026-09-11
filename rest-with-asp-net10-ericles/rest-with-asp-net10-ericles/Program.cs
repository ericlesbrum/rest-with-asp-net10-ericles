using rest_with_asp_net10_ericles;
using rest_with_asp_net10_ericles.Configurations;
using rest_with_asp_net10_ericles.Hypermedia.Filters;
using rest_with_asp_net10_ericles.Repositories;
using rest_with_asp_net10_ericles.Repositories.Generics;
using rest_with_asp_net10_ericles.Repositories.Interfaces;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;
using rest_with_asp_net10_ericles.Services;
using rest_with_asp_net10_ericles.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddSerilogLogging();

builder.Services.AddControllers(
    options => {
        options.Filters.Add<HypermediaFilter>();
    }).AddContentNegotiation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenAPIConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddRouteConfig();

builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddHATEOASConfiguration();

builder.Services.AddDatabaseConfig(builder.Configuration);
builder.Services.AddEvolveConfig(builder.Configuration, builder.Environment);

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped(typeof(IRepository<>),typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.UseCorsConfiguration(builder.Configuration);

app.MapControllers();
app.UseHATEOASRoutes();

app.UseSwaggerSpecification();
app.UseScalarSpecification();

app.Run();
