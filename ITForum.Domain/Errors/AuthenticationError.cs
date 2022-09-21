using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ITForum.Domain.Errors
{
    public class AuthenticationError : Exception
    {
        public AuthenticationError(IEnumerable<IdentityError> identityErrors)
        {
            this.identityErrors = identityErrors;
        }
        public IEnumerable<IdentityError> identityErrors;
    }
}
