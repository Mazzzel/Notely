using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Notely.Auth;
using Notely.Data;
using Notely.Mapping;
using Notely.Managers;
using Notely.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//------------------------------Connexion DB------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<NotelyDbContext>(options =>
    options.UseNpgsql(connectionString));

//------------------------------Mapper------------------------------
builder.Services.AddAutoMapper(cfg => { }, typeof(MapperProfile).Assembly);

//------------------------------Managers (DI)------------------------------
builder.Services.AddScoped<CompteManager>();
builder.Services.AddScoped<CoursManager>();
builder.Services.AddScoped<ChapitreManager>();
builder.Services.AddScoped<TodoManager>();
builder.Services.AddScoped<NoteManager>();
builder.Services.AddScoped<EvenementManager>();
builder.Services.AddScoped<SeanceManager>();
builder.Services.AddScoped<ExerciceSeanceManager>();
builder.Services.AddScoped<SerieManager>();

//------------------------------Services------------------------------
builder.Services.AddSingleton<IPasswordHasher, Sha256PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

//------------------------------Authentification------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.HttpContext.Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(token))
                context.Token = token;

            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.Authorized, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new MustChangePasswordRequirement()));
});
builder.Services.AddSingleton<IAuthorizationHandler, MustChangePasswordHandler>();

//------------------------------CORS (front Angular)------------------------------
var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFront", policy =>
    {
        if (!string.IsNullOrWhiteSpace(allowedOrigin))
            policy.WithOrigins(allowedOrigin).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        else
            policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

//------------------------------Proxy Render (TLS terminée en amont)------------------------------
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<NotelyDbContext>().Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFront");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
