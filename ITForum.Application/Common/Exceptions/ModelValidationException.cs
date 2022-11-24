using ITForum.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ITForum.Api.exceptions
{
    public class ModelValidationException : Exception
    {
        public IEnumerable<string> Errors;
        public ModelValidationException(IEnumerable<ModelError> errors)
        {
            Errors = errors.Select(e => e.ErrorMessage);
        }
    }
}
