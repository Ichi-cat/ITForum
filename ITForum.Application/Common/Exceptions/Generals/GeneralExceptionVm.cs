using FluentValidation;

namespace ITForum.Application.Common.Exceptions.Generals
{
    public class GeneralExceptionVm
    {
        public int Code { get; init; } = 500;
        public IEnumerable<GeneralExceptionItem> Errors { get; private set; } = null!;
        public GeneralExceptionVm(int code, string message)
        {
            Code = code;
            Errors = new List<GeneralExceptionItem> { new GeneralExceptionItem(message) };
        }
        public GeneralExceptionVm(int code, AuthenticationError authenticationError)
        {
            Code = code;
            Errors = authenticationError.Errors.Select(e => new GeneralExceptionItem(e));
        }
        public GeneralExceptionVm(int code, ModelValidationException modelValidationException)
        {
            Code = code;
            Errors = modelValidationException.Errors.Select(e => new GeneralExceptionItem(e));
        }
        public GeneralExceptionVm(int code, ValidationException exception)
        {
            Code = code;
            Errors = new List<GeneralExceptionItem> { new GeneralExceptionItem(exception.Message) };
        }
    }
    public class GeneralExceptionItem
    {
        public GeneralExceptionItem(string message)
        {
            Message = message;
        }

        public string Message { get; init; } = "Internal server error";
    }
}
