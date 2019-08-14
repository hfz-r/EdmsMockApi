using EdmsMockApi.Dtos.DataProfiles;
using EdmsMockApi.Dtos.Errors;
using EdmsMockApi.Dtos.ProfileFields;
using EdmsMockApi.Dtos.Profiles;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EdmsMockApi.Infrastructure
{
    public static class SwaggerExtensions
    {
        public static void AddApiVersion(this SwaggerGenOptions options, IServiceCollection service)
        {
            var provider = service.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new Info()
                {
                    Title = $"Mock API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                });
            }
        }

        public static void AddSwaggerGenOptions(this SwaggerGenOptions options)
        {
            options.DocumentFilter<RemoveBogusDefinitionsDocumentFilter>();
        }
    }

    internal class RemoveBogusDefinitionsDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            /* Disable unwanted definition */
            //swaggerDoc.Definitions.Remove(nameof(DataProfilesRootObject));
            //swaggerDoc.Definitions.Remove(nameof(ErrorsRootObject));
            //swaggerDoc.Definitions.Remove(nameof(ProfilesRootObject));
            //swaggerDoc.Definitions.Remove(nameof(ProfileFieldsRootObject));
        }
    }
}