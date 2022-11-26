using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ITForum.Application.Common.Exceptions
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
