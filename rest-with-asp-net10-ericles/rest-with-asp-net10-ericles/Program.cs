using rest_with_asp_net10_ericles;
using rest_with_asp_net10_ericles.Configurations;
using rest_with_asp_net10_ericles.Repositories;
using rest_with_asp_net10_ericles.Repositories.Generics;
using rest_with_asp_net10_ericles.Repositories.Interfaces;
using rest_with_asp_net10_ericles.Repositories.Interfaces.Generic;
using rest_with_asp_net10_ericles.Services;
using rest_with_asp_net10_ericles.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddSerilogLogging();

builder.Services.AddControllers();

builder.Services.AddDatabaseConfig(builder.Configuration);
builder.Services.AddEvolveConfig(builder.Configuration, builder.Environment);

builder.Services.AddScoped(typeof(IRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
