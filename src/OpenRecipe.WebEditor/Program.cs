using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenRecipe.WebEditor;
using OpenRecipe.WebEditor.Data;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IndexedDbAccessor>();
builder.Services.AddScoped(typeof(EntityRepository<>));

var app = builder.Build();

using var scope = app.Services.CreateScope();

await app.RunAsync();
