using System;

namespace ITForum.Application.Common.Exceptions
{
    internal class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) not found.") { }
    }
}
