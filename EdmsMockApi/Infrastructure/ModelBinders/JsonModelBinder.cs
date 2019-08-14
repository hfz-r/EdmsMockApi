using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using EdmsMockApi.Delta;
using EdmsMockApi.Helpers;
using EdmsMockApi.Infrastructure.Attributes;
using EdmsMockApi.Json.Maps;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EdmsMockApi.Infrastructure.ModelBinders
{
    public class JsonModelBinder<T> : IModelBinder where T : class, new()
    {
        private readonly IJsonHelper _jsonHelper;
        private readonly IJsonPropertyMapper _jsonPropertyMapper;

        public JsonModelBinder(IJsonHelper jsonHelper, IJsonPropertyMapper jsonPropertyMapper)
        {
            _jsonHelper = jsonHelper;
            _jsonPropertyMapper = jsonPropertyMapper;
        }

        #region Private methods

        private Dictionary<string, object> GetPropertyValuePairs(ModelBindingContext bindingContext)
        {
            Dictionary<string, object> result = null;

            if (bindingContext.ModelState.IsValid)
            {
                try
                {
                    var rootPropertyName = _jsonHelper.GetRootPropertyName<T>();

                    result = _jsonHelper.GetRequestJsonDictionaryFromStream(bindingContext.HttpContext.Request.Body, true);
                    result = (Dictionary<string, object>) result[rootPropertyName];
                }
                catch (Exception ex)
                {
                    bindingContext.ModelState.AddModelError("json", ex.Message);
                }
            }

            return result;
        }

        private void ValidateModel(ModelBindingContext bindingContext, Dictionary<string, object> propertyValuePairs, T dto)
        {
            var dtoProperties = dto.GetType().GetProperties();
            foreach (var property in dtoProperties)
            {
                if (!(property.PropertyType.GetCustomAttribute(typeof(BaseValidationAttribute)) is BaseValidationAttribute validationAttribute))
                    validationAttribute = property.GetCustomAttribute(typeof(BaseValidationAttribute)) as BaseValidationAttribute;

                if (validationAttribute != null)
                {
                    validationAttribute.Validate(property.GetValue(dto));
                    var errors = validationAttribute.GetErrors();

                    if (errors.Count > 0)
                    {
                        foreach (var error in errors)
                            bindingContext.ModelState.AddModelError(error.Key, error.Value);
                    }
                }
            }
        }

        #endregion

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyValuePairs = GetPropertyValuePairs(bindingContext);

            if (propertyValuePairs == null)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            if (bindingContext.ModelState.IsValid)
            {
                Delta<T> delta = null;

                if (bindingContext.ModelState.IsValid)
                {
                    delta = new Delta<T>(_jsonPropertyMapper, propertyValuePairs);
                    ValidateModel(bindingContext, propertyValuePairs, delta.Dto);
                }

                if (bindingContext.ModelState.IsValid)
                {
                    bindingContext.Model = delta;
                    bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                }
            }
            else bindingContext.Result = ModelBindingResult.Failed();


            return Task.CompletedTask;
        }
    }
}