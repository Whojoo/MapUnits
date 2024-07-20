using System.Reflection;
using MapUnits.Core;
using Microsoft.Extensions.DependencyInjection;
using Throw;

namespace MapUnits.Setup;

public static class MapUnitsSetupExtensions
{
    public static IServiceCollection AddMapUnits(this IServiceCollection services)
        => services.AddMapUnits([Assembly.GetEntryAssembly()]);

    public static IServiceCollection AddMapUnits(this IServiceCollection services, List<Assembly> assemblies)
    {
        // Add mapper to DI
        services.AddSingleton<IMapper, Mapper>();

        // Find all MapUnits
        var mapUnitTypes = assemblies.SelectMany(assembly => assembly.GetTypes())
            .Where(typeof(IMapUnit<,>).IsAssignableFrom)
            .Where(t => !t.GetTypeInfo().IsAbstract)
            .ToList();

        mapUnitTypes
            .ThrowIfNull($"No MapUnits found in the following assemblies {string.Join(';', assemblies.Select(a => a.GetName()))}")
            .IfEmpty();

        // Add the MapUnits to their wrappers
        AddMapUnitsToServiceCollection(mapUnitTypes, services);

        // Add MapUnits wrappers to DI

        return services;
    }

    private static void AddMapUnitsToServiceCollection(List<Type> mapUnitTypes, IServiceCollection services)
    {
        foreach (var mapUnitType in mapUnitTypes)
        {
            AddMapUnitToServiceCollection(mapUnitType, services);
        }
    }

    private static void AddMapUnitToServiceCollection(Type mapUnitType, IServiceCollection services)
    {
        var genericArguments = mapUnitType.GetGenericArguments();

        // Shouldn't throw anything, but check anyway to make the syntax sugar happy.
        genericArguments
            .ThrowIfNull()
            .IfCountNotEquals(2);

        // Instantiate the IMapUnit
        // Create the class types for the wrappers (typeof().MakeGeneric)
        // Instantiate the wrappers
        // Add the wrappers to the IServiceCollection
    }
}