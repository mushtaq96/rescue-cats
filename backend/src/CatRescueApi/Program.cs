using CatRescueApi.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using CatRescueApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// read secret key configurations from appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers(); // register controllers, endpoints will be recognized by swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IUserService, UserService>();
// AddScoped registers the service with the DI container and creates a new instance of the service for each HTTP request
builder.Services.AddScoped<ICatService, CatService>(); // register the CatService with the DI container 
builder.Services.AddScoped<IBreedService, BreedService>(); // register the BreedService with the DI container
builder.Services.AddScoped<IAdoptionService, AdoptionService>();
// Add FluentValidation validators from the assembly containing the specified type
builder.Services.AddValidatorsFromAssemblyContaining<AdoptionValidator>();
// cors policy to allow requests from frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors();
// routes defined in the controllers will be recognized by the routing middleware
app.MapControllers();

app.Run();
