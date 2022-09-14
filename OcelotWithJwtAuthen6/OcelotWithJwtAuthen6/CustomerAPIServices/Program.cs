using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var logging = builder.Logging;
var services = builder.Services;

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9051); // to listen for incoming http connection on port 9051
    //options.ListenAnyIP(7001, configure => configure.UseHttps()); // to listen for incoming https connection on port 7001
});

configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables();
// Add services to the container.
var audienceConfig = configuration.GetSection("Audience");

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(audienceConfig["Secret"]));
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = audienceConfig["Iss"],
    ValidateAudience = true,
    ValidAudience = audienceConfig["Aud"],
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    RequireExpirationTime = false,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};

services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = "TestKey";
    })
    .AddJwtBearer("TestKey", x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
