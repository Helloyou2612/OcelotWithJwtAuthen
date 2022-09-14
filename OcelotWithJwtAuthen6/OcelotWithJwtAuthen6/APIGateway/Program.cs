using System.Text;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var logging = builder.Logging;
var services = builder.Services;
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(9050); // to listen for incoming http connection on port 9050
    //options.ListenAnyIP(7001, configure => configure.UseHttps()); // to listen for incoming https connection on port 7001
});

configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("gateway.json", false, true)
    .AddEnvironmentVariables();

// Add services to the container
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

services.AddAuthentication(o => { o.DefaultAuthenticateScheme = "TestKey"; })
    .AddJwtBearer("TestKey", x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = tokenValidationParameters;
    });

services.AddOcelot();
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.UseOcelot().Wait();

app.MapControllers();
app.Run();