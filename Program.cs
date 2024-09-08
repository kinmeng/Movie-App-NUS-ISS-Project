using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieApp.Controllers;
using MovieApp.Data;
using MovieApp.Services;
using MovieApp.Models;
using Microsoft.Extensions.DependencyInjection;
using MovieApp.Manager;
using MovieApp.Configuration;
using System.Text.Json;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

var builder = WebApplication.CreateBuilder(args);

// ======   START AWS CONFIGS   ======
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings")); // Optionally bind settings to a strongly-typed class

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddSingleton<IAmazonSecretsManager>(sp =>
{
    return new AmazonSecretsManagerClient(RegionEndpoint.APSoutheast2); // Replace with your preferred region
});

builder.Services.AddAWSService<IAmazonSimpleSystemsManagement>();
builder.Services.AddSingleton<SecretsManagerService>();

var secretsManager = builder.Services.BuildServiceProvider().GetRequiredService<IAmazonSecretsManager>();
var request = new GetSecretValueRequest { SecretId = "MovieApiConnectionString" };
var response = await secretsManager.GetSecretValueAsync(request);
var secrets = new Dictionary<string, string>();

if (response.SecretString != null)
{
    var secretJson = JsonDocument.Parse(response.SecretString);
    foreach (var kvp in secretJson.RootElement.EnumerateObject())
    {
        var tmdbSettings = kvp.Value.GetString();
        secrets[kvp.Name] = tmdbSettings;
    }
}

// Get the existing configuration
var configuration = builder.Configuration;

// Update the specific section with the fetched secret
var tmdbSettingsSection = configuration.GetSection("TmdbSettings");
var apiKey = secrets["ApiKey"]; 
tmdbSettingsSection["ApiKey"] = apiKey;

// ======   END AWS CONFIGS     ======

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Movies");
    options.Conventions.AuthorizePage("/Reviews");
});
builder.Services.Configure<TmdbSettings>(builder.Configuration.GetSection("TmdbSettings"));
builder.Services.AddHttpClient<MovieService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<ApplicationUserManager>();
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.MapRazorPages();


//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.Run();
