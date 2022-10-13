using FluentValidation;
using ITForum.Application.Common.Exceptions.Generals;

namespace ITForum.Application.Common.Exceptions
{
    public class ValidateExceptionVm : GeneralExceptionVm
    {
        public ValidateExceptionVm(ValidationException exception)
        {
            Add(new GeneralExceptionItem(400, exception.Message));
        }
    }
}
