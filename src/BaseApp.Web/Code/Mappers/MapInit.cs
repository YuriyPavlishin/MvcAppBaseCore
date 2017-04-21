using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace BaseApp.Web.Code.Mappers
{
    public static class MapInit
    {
        public static MapperConfiguration CreateConfiguration()
        {
            var profileType = typeof(MapperBase);
            // Get an instance of each MapperBase in the executing assembly.
            var profiles = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => profileType.IsAssignableFrom(t)
                    && t.GetConstructor(Type.EmptyTypes) != null)
                .Select(Activator.CreateInstance)
                .Cast<MapperBase>()
                .ToList();

            var config = new MapperConfiguration(cfg =>
                {
                    profiles.ForEach(cfg.AddProfile);
                });
            config.AssertConfigurationIsValid();

            return config;
        }
    }
}