using LMS.Data;
using LMS.Interfaces.ServicesInterface;
using LMS.Interfaces;
using LMS.Repositories;
using LMS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using LMS.Interfaces.RepoInterface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		var jwtSettings = builder.Configuration.GetSection("JwtSettings");
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSettings["Issuer"],           // match your "iss" field
			ValidAudience = jwtSettings["Audience"],     // match your "aud" field
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)) // Use a strong secret key
		};
	});
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
	options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
	options.AddPolicy("AdminOrUserPolicy", policy =>
		policy.RequireRole("Admin", "User"));
});

builder.Services.AddDbContext<LibraryDBContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IReservationServices, ReservationServices>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		In=ParameterLocation.Header,
		Name="Authorization",
		Type=SecuritySchemeType.ApiKey
	});
	options.OperationFilter<SecurityRequirementsOperationFilter>();
	options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Library Management System", Version = "v1" });
	options.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.DefaultModelsExpandDepth(-1);
	});
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
