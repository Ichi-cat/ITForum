namespace ITForum.Application.Common.Exceptions
{
    public class UploadFileException : Exception
    {
        public UploadFileException(string message, Exception e) : base(message, e){}
        public UploadFileException(string message) : base(message){}
    }
}
