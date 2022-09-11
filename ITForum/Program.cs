using AutoMapper;
using ITForum.Application;
using ITForum.Application.Common.Mappings;
using ITForum.Persistance;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//TODO:NEED REFACTORING
IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

builder.Services.AddApplication();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new GetAssemblyMapsProfile(Assembly.GetExecutingAssembly()));
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistance(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.MapDefaultControllerRoute();
//app.MapFallbackToController("Error_404", "error");


app.Run();
