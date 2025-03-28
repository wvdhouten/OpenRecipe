using Microsoft.JSInterop;
using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Data
{
    public class StorageContext : IndexedDbAccessor
    {
        public EntityContainer<SettingEntity> Settings { get; private set; } = null!;

        public RecipeContainer Recipes { get; private set; } = null!;

        public StorageContext(IJSRuntime runtime) : base(runtime)
        {
        }

        public async Task InitializeAsync()
        {
            var containers = InitContainers();

            await InitializeAsync(containers);

            // TODO: Make Generic
            await Settings.RefreshAsync();
            await Recipes.RefreshAsync();
        }

        private IEnumerable<string> InitContainers()
        {
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                if (TryInitContainer(propertyType, out var container))
                {
                    var instance = Activator.CreateInstance(propertyType, this) 
                        ?? throw new InvalidOperationException($"Failed to create instance of {propertyType}");
                   
                    property.SetValue(this, instance);

                    yield return container;
                }
            }
        }

        private static bool TryInitContainer(Type? property, out string container)
        {
            container = string.Empty;

            if (property == null)
                return false;

            if (!property.IsGenericType || property.GetGenericTypeDefinition() != typeof(EntityContainer<>))
                return TryInitContainer(property.BaseType, out container);

            container = property.GenericTypeArguments.First().Name;
            return true;            
        }
    }
}
