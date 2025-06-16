
using GizmoGrid._01.Data;
using GizmoGrid._01.Middleware;
using GizmoGrid._01.Repository.ApiRepo;
using GizmoGrid._01.Repository.FlowRepo;
using GizmoGrid._01.Repository.SchemaRepo;
using GizmoGrid._01.Services;
using GizmoGrid._01.Services.ApiServices;
using GizmoGrid._01.Services.AuthServices;
using GizmoGrid._01.Services.FlowDiagramService;
using GizmoGrid._01.Services.ImageUploader;
using GizmoGrid._01.Services.ProjectService;
using GizmoGrid._01.Services.SchemaServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

builder.Services.AddDbContext<CodePlannerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection for services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IFlowDiagramService, FlowDiagramService>();
builder.Services.AddScoped<IImageUploader, ImageUploaderService>();
builder.Services.AddScoped<IFlowDiagramRepository, FlowDiagramRepository>();
builder.Services.AddScoped<ISchemaRepo, SchemaRepo>();
builder.Services.AddScoped<ISchemaInterface, SchemaService>();
builder.Services.AddScoped<IApiRepoInterface, ApiRepoService>();
builder.Services.AddScoped<IApidiagramInterface,ApiDiagramService>();


// Swagger with JWT Bearer auth config
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GizmoGrid",
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token in the format: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// CORS policy - Allow all origins (adjust for production!)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GetUserIMiddleware>();

app.MapControllers();

app.Run();
