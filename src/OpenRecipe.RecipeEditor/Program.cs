using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenCookbook.RecipeEditor;
using OpenRecipe.Domain.Serialization;
using OpenRecipe.Serialization;

namespace OpenRecipe.RecipeEditor
{
    internal static class Program
    {
        public static IHost? CurrentHost { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            CurrentHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<RecipeSerializer>();

                    services.AddScoped<RecipeForm>();
                })
                .Build();

            using IServiceScope serviceScope = CurrentHost.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var mainForm = provider.GetRequiredService<RecipeForm>();

            Application.Run(mainForm);
        }
    }
}