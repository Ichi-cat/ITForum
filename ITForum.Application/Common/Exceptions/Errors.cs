namespace ITForum.Application.Common.Exceptions
{
    public static class Errors
    {
        public static ErrorVm NotFound_404 { get; } = new ErrorVm { Message = "Page is not found" };
    }
}
