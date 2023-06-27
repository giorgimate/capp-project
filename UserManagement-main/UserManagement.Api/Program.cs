using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Services;
using UserManagement.Domain.Models.Domain;
using UserManagement.Facade.Helpers.Implementations;
using UserManagement.Facade.Helpers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Description = "Standard authorization header using the bearer scheme, e.g \"bearer {token} \"",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});

	c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHttpContextAccessor();	

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IHelperMethods, HelperMethods>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
options.TokenValidationParameters = new TokenValidationParameters
{
	ValidateIssuerSigningKey = true,
	IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
	ValidateIssuer = false,
	ValidateAudience = false
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
