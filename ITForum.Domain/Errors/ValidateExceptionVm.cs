using ITForum.Domain.Errors.Generals;
using FluentValidation;

namespace ITForum.Domain.Errors
{
    public class ValidateExceptionVm : GeneralExceptionVm
    {
        public ValidateExceptionVm(ValidationException exception)
        {
            Add(new GeneralExceptionItem(400, exception.Message));
        }
    }
}
