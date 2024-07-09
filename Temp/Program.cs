using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string keyVaultUri = builder.Configuration["KeyVaultUri"];
string appConfigUri = builder.Configuration["AppConfigUri"];
string managedIdentityClientId = builder.Configuration["ManagedIdentityClientId"];

var credentialOptions = new DefaultAzureCredentialOptions
{
	ManagedIdentityClientId = managedIdentityClientId
};

System.Console.WriteLine($"AppConfigUri: {appConfigUri}");
System.Console.WriteLine($"ManagedIdentityClientId: {managedIdentityClientId}");
System.Console.WriteLine($"AppConfigUri: {keyVaultUri}");

if (!string.IsNullOrEmpty(keyVaultUri))
{
	builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential(credentialOptions));
}

if (!string.IsNullOrEmpty(appConfigUri) && !string.IsNullOrEmpty(managedIdentityClientId))
{
	builder.Configuration.AddAzureAppConfiguration(options =>
	{
		options.Connect(new Uri(appConfigUri), new ManagedIdentityCredential(managedIdentityClientId));

		//	options.Connect(new Uri(appConfigUri), new DefaultAzureCredential(credentialOptions));
		// .ConfigureKeyVault(kv =>
		// {
		// 	kv.SetCredential(new DefaultAzureCredential(credentialOptions));
		// });
	});
}

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
