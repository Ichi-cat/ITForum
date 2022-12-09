namespace ITForum.Application.Common.Exceptions
{
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException():base("User is not authorized"){}
    }
}
