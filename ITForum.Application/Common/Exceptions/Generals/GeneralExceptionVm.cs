namespace ITForum.Application.Common.Exceptions.Generals
{
    public class GeneralExceptionVm
    {
        public IList<GeneralExceptionItem> errors { get; private set; } = new List<GeneralExceptionItem>();
        public GeneralExceptionVm Add(GeneralExceptionItem item)
        {
            errors.Add(item);
            return this;
        }
        public GeneralExceptionVm Add(int code, string message)
        {
            errors.Add(new GeneralExceptionItem(code, message));
            return this;
        }
    }
    public class GeneralExceptionItem
    {
        public GeneralExceptionItem(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; init; } = 500;
        public string Message { get; init; } = "Internal server error";
    }
}
