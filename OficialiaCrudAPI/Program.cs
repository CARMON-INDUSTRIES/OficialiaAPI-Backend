using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OficialiaCrudAPI.Data;
using OficialiaCrudAPI.DTO;
using OficialiaCrudAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://oficialia-frontend-login.vercel.app",
                              "http://localhost:3000", "http://localhost:3001")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                      });
});

builder.Services.AddDbContext<AppDataDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information)
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDataDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(Options =>
{
    Options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    Options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

builder.Services.AddScoped<ICorrespondenciaService, CorrespondenciaService>();
builder.Services.AddScoped<IComunidadesService, ComunidadesService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IImportanciaService, ImportanciaService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

// Agrega el middleware de enrutamiento
app.UseRouting();

// Configura CORS después del enrutamiento
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
