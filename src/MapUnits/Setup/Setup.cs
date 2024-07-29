using System.Reflection;
using MapUnits.Core;
using MapUnits.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Throw;

namespace MapUnits.Setup;

public static class MapUnitsSetupExtensions
{
    public static IServiceCollection AddMapUnits(this IServiceCollection services)
        => services.AddMapUnits([Assembly.GetEntryAssembly()]);

    public static IServiceCollection AddMapUnits(this IServiceCollection services, List<Assembly?> assemblies)
    {
        services.AddSingleton(MapperFactory.CreateMapper(assemblies));

        return services;
    }
}