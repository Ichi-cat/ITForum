using ITForum.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITForum.Tests.Common
{
    public abstract class TestCommandBase:IDisposable
    {
        protected readonly ItForumDbContext Context;
        public TestCommandBase()
        {
            Context=TopicContextFactory.Create();
        }
        public void Dispose()
        {
            TopicContextFactory.Destroy(Context);
        }
    }
}
