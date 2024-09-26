using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopEase.Logger;
using ShopEase.Model.Config;
using ShopEase.Model.Settings;
using ShopEase_Api;
using ShopEase_Api.Middleware;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllRequests", builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

// Logger Configuration
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

// Application Setting & SMTP Settings Configuration read from appsettings.json
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("SMTPSettings"));

// JWT Token Configuration
var key = Encoding.UTF8.GetBytes(Convert.ToString(builder.Configuration["AppSettings:JWT_Secret"]));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.Configure<DataConfig>(builder.Configuration.GetSection("Data"));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-IN");
    options.SupportedCultures = new List<CultureInfo>() { new CultureInfo("en-IN"), };
});

//builder.Services.AddControllers().AddNewtonsoftJson(x => { x.SerializerSettings.ContractResolver = new DefaultContractResolver(); });
builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
builder.Services.ConfigureLoggerService();
RegisterService.RegisterServices(builder.Services);

//Add Swagger for the API documentation
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ShopEase Api",
        Version = "v1",
        Description = "ShopEase Api",
    });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});

builder.Services.AddControllers();
var app = builder.Build();

app.UseRequestLocalization();
app.UseCors("AllRequests");
app.UseRouting();
app.UseMiddleware<CustomMiddleware>();
app.UseAuthorization();
app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopEase Api"));
}

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();