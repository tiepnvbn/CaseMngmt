using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using CaseMngmt.Models.Database;
using CaseMngmt.Models.AutoMapper;
using CaseMngmt.Models.ApplicationUsers;
using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Service.Customers;
using CaseMngmt.Service.Companies;
using CaseMngmt.Service.Types;
using CaseMngmt.Service.Keywords;
using CaseMngmt.Service.Templates;
using CaseMngmt.Service.Cases;
using CaseMngmt.Service.CaseKeywords;
using CaseMngmt.Service.CompanyTemplates;
using CaseMngmt.Service.FileUploads;
using CaseMngmt.Repository.Companies;
using CaseMngmt.Repository.Customers;
using CaseMngmt.Repository.Types;
using CaseMngmt.Repository.Keywords;
using CaseMngmt.Repository.Templates;
using CaseMngmt.Repository.Cases;
using CaseMngmt.Repository.CaseKeywords;
using CaseMngmt.Repository.CompanyTemplates;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
                ));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("LocalhostPolicy", builder =>
        {
            builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowCredentials();
        });
    });
}


#region Register Service & Repository

builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();

builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();

builder.Services.AddTransient<ITypeService, TypeService>();
builder.Services.AddTransient<ITypeRepository, TypeRepository>();

builder.Services.AddTransient<IKeywordService, KeywordService>();
builder.Services.AddTransient<IKeywordRepository, KeywordRepository>();

builder.Services.AddTransient<ITemplateService, TemplateService>();
builder.Services.AddTransient<ITemplateRepository, TemplateRepository>();

builder.Services.AddTransient<ICaseService, CaseService>();
builder.Services.AddTransient<ICaseRepository, CaseRepository>();

builder.Services.AddTransient<ICaseKeywordService, CaseKeywordService>();
builder.Services.AddTransient<ICaseKeywordRepository, CaseKeywordRepository>();

builder.Services.AddTransient<ITypeService, TypeService>();
builder.Services.AddTransient<ITypeRepository, TypeRepository>();

builder.Services.AddTransient<ICompanyTemplateService, CompanyTemplateService>();
builder.Services.AddTransient<ICompanyTemplateRepository, CompanyTemplateRepository>();

builder.Services.AddTransient<IFileUploadService, FileUploadService>();

#endregion


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
});

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiCaseManagement", Version = "v1" });
        c.AddSecurityDefinition(
            "token",
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Name = HeaderNames.Authorization
            }
        );
        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "token"
                        },
                    },
                    Array.Empty<string>()
                }
            }
        );
    }
);
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CustomProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

//Configure the HTTP-request pipeline
if (app.Environment.IsDevelopment())
{
    //app.UseItToSeedSqlServer();
    //configure other services
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseRouting();

app.MapFallbackToFile("/index.html");

app.UseCors("LocalhostPolicy");

app.Run();


