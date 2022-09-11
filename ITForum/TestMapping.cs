using AutoMapper;
using ITForum.Application.Common.Mappings;

namespace ITForum.Api
{
    public class TestMapping : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TestMappingSource, TestMapping>();
        }
    }
}
