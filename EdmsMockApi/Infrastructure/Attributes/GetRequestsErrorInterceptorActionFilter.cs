using System.Collections.Generic;
using System.IO;
using System.Net;
using EdmsMockApi.Dtos.Errors;
using EdmsMockApi.Json.ActionResults;
using EdmsMockApi.Json.Serializer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EdmsMockApi.Infrastructure.Attributes
{
    public class GetRequestsErrorInterceptorActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            var jsonFieldSerializer = actionExecutedContext.HttpContext.RequestServices.GetRequiredService<IJsonFieldsSerializer>();

            if (actionExecutedContext.Exception != null && !actionExecutedContext.ExceptionHandled)
            {
                var error = new KeyValuePair<string, List<string>>("internal_server_error", new List<string> { "please, contact the administrator." });

                actionExecutedContext.Exception = null;
                actionExecutedContext.ExceptionHandled = true;
                SetError(actionExecutedContext, error, jsonFieldSerializer);
            }
            else if (actionExecutedContext.HttpContext.Response != null && (HttpStatusCode)actionExecutedContext.HttpContext.Response.StatusCode != HttpStatusCode.OK)
            {
                string responseBody;
                using (var streamReader = new StreamReader(actionExecutedContext.HttpContext.Response.Body))
                {
                    responseBody = streamReader.ReadToEnd();
                }

                // reset reader position.
                actionExecutedContext.HttpContext.Response.Body.Position = 0;

                var defaultWebApiErrorsModel = JsonConvert.DeserializeObject<DefaultErrorsModel>(responseBody);
                if (!string.IsNullOrEmpty(defaultWebApiErrorsModel.Message) &&
                    !string.IsNullOrEmpty(defaultWebApiErrorsModel.MessageDetail))
                {
                    var error = new KeyValuePair<string, List<string>>("lookup_error", new List<string> { "Not found!" });
                    SetError(actionExecutedContext, error, jsonFieldSerializer);
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        private static void SetError(ActionExecutedContext actionExecutedContext, KeyValuePair<string, List<string>> error, IJsonFieldsSerializer jsonFieldsSerializer)
        {
            var bindingError = new Dictionary<string, List<string>> { { error.Key, error.Value } };
            var errorsRootObject = new ErrorsRootObject
            {
                Errors = bindingError
            };

            var errorJson = jsonFieldsSerializer.Serialize(errorsRootObject, null);

            actionExecutedContext.Result = new ErrorActionResult(errorJson, HttpStatusCode.BadRequest);
        }
    }
}