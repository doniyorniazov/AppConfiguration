using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string appConfigCS = builder.Configuration["AppConfigCS"];
string appConfigUri = builder.Configuration["AppConfigUri"];
string managedIdentityClientId = builder.Configuration["ManagedIdentityClientId"];

System.Console.WriteLine("AppConfigUri: " + appConfigUri);
System.Console.WriteLine("ManagedIdentityClientId: " + managedIdentityClientId);

builder.Configuration.AddAzureAppConfiguration(options =>
		options.Connect(
				new Uri(appConfigUri),
				new ManagedIdentityCredential(managedIdentityClientId)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
