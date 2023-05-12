using back.data;
using Microsoft.EntityFrameworkCore;

var corsAllowedOrigins = "https://localhost";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
options.AddPolicy(corsAllowedOrigins, policy =>
{
	policy.AllowAnyOrigin();
	policy.AllowAnyHeader();
}
));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
{
	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add services to the container.

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

app.UseCors(corsAllowedOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
