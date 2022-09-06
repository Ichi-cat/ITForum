namespace ITForum.Domain.Errors
{
    public static class Errors
    {
        public static ErrorVm NotFound_404 { get; } = new ErrorVm { Message = "Page is not found"};
    }
}
