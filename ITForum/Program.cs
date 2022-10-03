using ITForum.Application;
using ITForum.Application.Common.Mappings;
using ITForum.Application.Interfaces;
using ITForum.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ITForum.Application.Topics.Services;
using Microsoft.Extensions.FileProviders;
using System.Text;
using NLog.Web;
using ITForum.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//TODO:NEED REFACTORING
IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddApplication();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new GetAssemblyMapsProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new GetAssemblyMapsProfile(typeof(IItForumDbContext).Assembly));
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ITForum api",
        Description = "It's our pet project",
        Contact = new OpenApiContact
        {
            Name = "Our Contact",
            Email = "PashaMS@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "Empty license",
            Url = new Uri("httsp://license.com")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddPersistance(configuration);
builder.Services.AddTransient<IBufferedFileUploadService, BufferedFileUploadLocalService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["AuthOptions:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["AuthOptions:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthOptions:Key"])),//todo: âûíåñòè â îòäåëüíûé ôàéë, äîáàâèòü ñåêðåòíûé êëþ÷
        ValidateIssuerSigningKey = true
    };
});

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
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles")),
    RequestPath = "/UploadedFiles"
});

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
//app.MapFallbackToController("Error_404", "error");


app.Run();
