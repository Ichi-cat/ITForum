using AutoMapper;
using System.Diagnostics;
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
                var instance = Activator.CreateInstance(type);
                type.GetMethod("Mapping").Invoke(instance, new[] {this} );
            });
        }
    }
}
