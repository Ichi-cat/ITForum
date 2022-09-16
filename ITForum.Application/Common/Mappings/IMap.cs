using AutoMapper;

namespace ITForum.Application.Common.Mappings
{
    public interface IMap<T>
    {
        void Mapping(Profile profile);
    }
}
