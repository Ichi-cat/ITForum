using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ITForum.Application.Common.Exceptions
{
    public class AuthenticationError : Exception
    {
        public IEnumerable<string> Errors;
        public AuthenticationError(IEnumerable<IdentityError> identityErrors)
        {
            Errors = identityErrors.Select(error => error.Description);
        }
        public AuthenticationError(IEnumerable<string> errors)
        {
            Errors = errors;
        }
        public AuthenticationError(IEnumerable<ModelError> errors)
        {
            Errors = errors.Select(e => e.ErrorMessage);
        }
    }
}
