using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenRecipe.WebEditor;
using OpenRecipe.WebEditor.Components;
using OpenRecipe.WebEditor.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<Toaster>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IndexedDbAccessor>();
builder.Services.AddSingleton<StorageContext>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

await scope.ServiceProvider.GetRequiredService<StorageContext>().InitializeAsync();

await app.RunAsync();
