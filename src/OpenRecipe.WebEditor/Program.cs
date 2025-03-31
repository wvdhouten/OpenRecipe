using Alkaline64.Injectable.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenRecipe.WebEditor;
using OpenRecipe.WebEditor.Components;
using OpenRecipe.WebEditor.Infrastructure;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<Toaster>();
builder.Services.AddSingleton<Downloader>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.RegisterInjectables<Program>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

await scope.ServiceProvider.GetRequiredService<StorageContext>().InitializeAsync();

await app.RunAsync();
