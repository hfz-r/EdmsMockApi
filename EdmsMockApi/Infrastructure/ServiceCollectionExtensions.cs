using System.Collections.Generic;
using EdmsMockApi.Converters;
using EdmsMockApi.Delta;
using EdmsMockApi.Helpers;
using EdmsMockApi.Infrastructure.ModelBinders;
using EdmsMockApi.Json.Maps;
using EdmsMockApi.Json.Serializer;
using EdmsMockApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EdmsMockApi.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApis(this IServiceCollection service)
        {
            //model-binder
            service.AddScoped(typeof(ParametersModelBinder<>));
            service.AddScoped(typeof(JsonModelBinder<>));

            //services
            service.AddScoped<IDocufloSdkService, DocufloSdkService>();

            //helpers
            service.AddScoped<IMappingHelper, MappingHelper>();
            service.AddScoped<IJsonHelper, JsonHelper>();
            service.AddScoped<IDtoHelper, DtoHelper>();

            //json rest serializer
            service.AddScoped<IJsonFieldsSerializer, JsonFieldsSerializer>();

            //converters
            service.AddScoped<IObjectConverter, ObjectConverter>();
            service.AddScoped<IApiTypeConverter, ApiTypeConverter>();

            //maps
            service.AddScoped<IJsonPropertyMapper, JsonPropertyMapper>();

            service.AddSingleton<Dictionary<string, object>>();
        }
    }
}