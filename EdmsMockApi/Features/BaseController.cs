using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EdmsMockApi.Dtos.Errors;
using EdmsMockApi.Json.ActionResults;
using EdmsMockApi.Json.Serializer;
using Microsoft.AspNetCore.Mvc;

namespace EdmsMockApi.Features
{
    public class BaseController : ControllerBase
    {
        protected readonly IJsonFieldsSerializer JsonFieldsSerializer;

        public BaseController(IJsonFieldsSerializer jsonFieldsSerializer)
        {
            JsonFieldsSerializer = jsonFieldsSerializer;
        }

        protected async Task<IActionResult> Error(
            HttpStatusCode statusCode = (HttpStatusCode) 422,
            string propertyKey = "", 
            string errorMessage = "")
        {
            var errors = new Dictionary<string, List<string>>();

            if (!string.IsNullOrEmpty(errorMessage) && !string.IsNullOrEmpty(propertyKey))
            {
                var errorsList = new List<string>() { errorMessage };
                errors.Add(propertyKey, errorsList);
            }

            foreach (var model in ModelState)
            {
                var errorMessages = model.Value.Errors.Select(m => m.ErrorMessage);

                var validErrorMessages = new List<string>();
                validErrorMessages.AddRange(errorMessages.Where(message => !string.IsNullOrEmpty(message)));

                if (validErrorMessages.Count > 0)
                {
                    if (errors.ContainsKey(model.Key))
                        errors[model.Key].AddRange(validErrorMessages);
                    else
                        errors.Add(model.Key, validErrorMessages.ToList());
                }
            }

            var errorsRootObject = new ErrorsRootObject { Errors = errors };

            var errorsJson = JsonFieldsSerializer.Serialize(errorsRootObject, null);

            return await Task.FromResult<IActionResult>(new ErrorActionResult(errorsJson, statusCode));
        }
    }
}