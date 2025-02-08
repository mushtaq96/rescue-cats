using CatRescueApi.Data;
using CatRescueApi.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using CatRescueApi.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // register controllers, endpoints will be recognized by swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IUserService, UserService>();
// AddScoped registers the service with the DI container and creates a new instance of the service for each HTTP request
builder.Services.AddScoped<ICatService, CatService>(); // register the CatService with the DI container 
builder.Services.AddScoped<IBreedService, BreedService>(); // register the BreedService with the DI container
// Add DbContext configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add FluentValidation validators from the assembly containing the specified type
builder.Services.AddValidatorsFromAssemblyContaining<AdoptionValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// routes defined in the controllers will be recognized by the routing middleware
app.MapControllers();

app.Run();
