using ITForum.Application;
using ITForum.Application.Common.Mappings;
using ITForum.Application.Interfaces;
using ITForum.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using System.Text;
using NLog.Web;
using ITForum.Api.Middleware;
using Microsoft.AspNetCore.Identity;
using ITForum.Application.Services;
using ITForum.Domain.ItForumUser;
using ITForum.Api.Additional;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();


builder.Services.AddApplication();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new GetAssemblyMapsProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new GetAssemblyMapsProfile(typeof(IItForumDbContext).Assembly));
});
builder.Services.AddControllers(options =>
{
    options.Filters.Clear();
    options.Filters.Add(new ValidationFilterAttribute());
});
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @$"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer {Initialize.CreateTestUserJwt(builder.Configuration)}'",
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
        Version = "0.0.1",
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
builder.Services.AddPersistance(builder.Configuration);
builder.Services.AddTransient<IBufferedFileUploadService, BufferedFileUploadLocalService>();

builder.Services.AddMailKit(optionBuilder =>
{
    optionBuilder.UseMailKit(new MailKitOptions()
    {
        Server = builder.Configuration["Smtp:Host"],
        Port = builder.Configuration.GetValue<int>("Smtp:Port"),
        SenderName = builder.Configuration["Smtp:From:Name"],
        SenderEmail = builder.Configuration["Smtp:From:Email"],

        
        Account = builder.Configuration["Smtp:Login"],
        Password = builder.Configuration["Smtp:Password"],
        Security = builder.Configuration.GetValue<bool>("Smtp:EnableSsl")
    });
});
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["AuthOptions:Issuer"],
    ValidateAudience = true,
    ValidAudience = builder.Configuration["AuthOptions:Audience"],
    ValidateLifetime = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthOptions:Key"])),//todo: âûíåñòè â îòäåëüíûé ôàéë, äîáàâèòü ñåêðåòíûé êëþ÷
    ValidateIssuerSigningKey = true,
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = false;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", builder =>
    {
        builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, UserRoles.Admin));
    });
    options.AddPolicy("RequireUserRole", builder =>
    {
        builder.RequireAssertion(x => (x.User.HasClaim(ClaimTypes.Role, UserRoles.User)
                                   || x.User.HasClaim(ClaimTypes.Role, UserRoles.Admin))
                                   && x.User.HasClaim(ClaimTypes.Role, UserRoles.Blocked) == false);
    });
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.AllowedForNewUsers = false;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddHttpClient();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ItForumDbContext>();
    var userManager = services.GetRequiredService<UserManager<ItForumUser>>();
    var roleManager = services.GetRequiredService<RoleManager<ItForumRole>>();
    try
    {
        await Initialize.Initial(context, userManager, roleManager);
        await Initialize.CreateTestUser(userManager, roleManager);
    }
    catch (Exception ex) { }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseCors("AllowAll");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();
app.UseCustomExceptionHandler();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles")),
    RequestPath = "/UploadedFiles"
});

app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
//app.MapFallbackToController("Error_404", "error");


app.Run();