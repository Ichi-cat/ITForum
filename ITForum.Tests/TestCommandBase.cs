using ITForum.Persistence;

namespace ITForum.Tests
{
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly ItForumDbContext context;
        public TestCommandBase()
        {
            context = TopicUnitTest.Create();
        }
        public void Dispose()
        {
            TopicUnitTest.Destroy(context);
        }
    }
}
