using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//string appConfUri = 
//string clientId = builder.Configuration["ManagedIdentityClientId"];

string appConfigUri = "";
var credentialOptions = new DefaultAzureCredentialOptions
{
	ManagedIdentityClientId = ""
};

builder.Configuration.AddAzureAppConfiguration(options =>
{
	options.Connect(new Uri(appConfigUri), new DefaultAzureCredential(credentialOptions))
					.ConfigureKeyVault(kv =>
					{
						kv.SetCredential(new DefaultAzureCredential(credentialOptions));
					});
});

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
