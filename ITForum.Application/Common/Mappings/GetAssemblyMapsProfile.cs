using AutoMapper;
using ITForum.Application.Interfaces;
using System.Reflection;

namespace ITForum.Application.Common.Mappings
{
    public class GetAssemblyMapsProfile : Profile
    {
        public GetAssemblyMapsProfile(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(type
                => type.GetInterfaces().Any(iType
                => iType == typeof(IMap))).ToList();

            types.ForEach(type =>
            {
                dynamic instance = Activator.CreateInstance(type);
                instance.Mapping(this);
            });
        }
    }
}
