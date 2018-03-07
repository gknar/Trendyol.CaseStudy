using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Trendyol.CaseStudy.Publisher.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid) context.Result = new ValidationFailedResult(context.ModelState);
        }

        public class ValidationFailedResult : ObjectResult
        {
            public ValidationFailedResult(ModelStateDictionary modelState)
                : base(new ValidationResultModel(modelState))
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity;
            }
        }

        public class ValidationError
        {
            public ValidationError(string field, string message)
            {
                Field = field != string.Empty ? field : null;
                Message = message;
            }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Field { get; }

            public string Message { get; }
        }

        public class ValidationResultModel
        {
            public ValidationResultModel(ModelStateDictionary modelState)
            {
                Message = "Validation Failed";
                Success = false;
                Code = StatusCodes.Status422UnprocessableEntity;
                Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
            }

            public string Message { get; }
            public bool Success { get; }
            public int Code { get; }

            public List<ValidationError> Errors { get; }
        }
    }
}