using System.Reflection;
using MapUnits.Core;
using Microsoft.Extensions.DependencyInjection;
using Throw;

namespace MapUnits.Setup;

public static class MapUnitsSetupExtensions
{
    public static IServiceCollection AddMapUnits(this IServiceCollection services)
        => services.AddMapUnits([Assembly.GetEntryAssembly()]);

    public static IServiceCollection AddMapUnits(this IServiceCollection services, List<Assembly?> assemblies)
    {
        // Add mapper to DI
        services.AddSingleton<IMapper, Mapper>();

        // Find all MapUnits (TODO: Deep search assembly?)
        var mapUnitTypes = GetMapUnitTypes(assemblies);

        // Add the MapUnits to their wrappers and DI
        CreateWrappersAndAddToServiceCollection(mapUnitTypes, services);

        return services;
    }

    private static List<Type> GetMapUnitTypes(List<Assembly?> assemblies)
    {
        // Some assembly methods return Assembly? So lets filter out the null, just to be safe
        var availableAssemblies = assemblies
            .Where(assembly => assembly is not null)
            .Select(assembly => assembly!);

        // Find all MapUnits (TODO: Deep search assembly?)
        var mapUnitTypes = availableAssemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => !type.GetTypeInfo().IsAbstract)
            .Where(type => type
                .GetInterfaces()
                .Where(typeInterface => typeInterface.IsGenericType)
                .Any(typeInterFace => typeInterFace.GetGenericTypeDefinition() == typeof(IMapUnit<,>)))
            .ToList();

        mapUnitTypes
            .ThrowIfNull($"No MapUnits found in the following assemblies {string.Join(';', availableAssemblies.Select(a => a.GetName()))}")
            .IfEmpty();

        return mapUnitTypes;
    }

    private static void CreateWrappersAndAddToServiceCollection(List<Type> mapUnitTypes, IServiceCollection services)
    {
        foreach (var mapUnitType in mapUnitTypes)
        {
            CreateWrapperAndAddToServiceCollection(mapUnitType, services);
        }
    }

    private static void CreateWrapperAndAddToServiceCollection(Type mapUnitType, IServiceCollection services)
    {
        var genericArguments = mapUnitType.GetGenericArguments();
        var genericArgumentsReversed = genericArguments.Reverse().ToArray();

        // Shouldn't throw anything, but check anyway to make the syntax sugar happy.
        genericArguments
            .ThrowIfNull()
            .IfCountNotEquals(2);

        // Instantiate the IMapUnit
        var mapUnit = Activator.CreateInstance(mapUnitType);

        // Create the class types for the wrappers (typeof().MakeGeneric)
        var wrapperType = typeof(MapUnitWrapper<,>).MakeGenericType(genericArguments);
        var biDirectionalWrapperType = typeof(BiDirectionalMapUnitWrapper<,>).MakeGenericType(genericArgumentsReversed);
        var interfaceWrapperType = typeof(MapUnitWrapper<,>).MakeGenericType(genericArguments);
        var interfaceBiDirectionalWrapperType = typeof(BiDirectionalMapUnitWrapper<,>).MakeGenericType(genericArgumentsReversed);

        // Instantiate the wrappers
        var wrapper = Activator.CreateInstance(wrapperType, [mapUnit]);
        var biDirectionalWrapper = Activator.CreateInstance(biDirectionalWrapperType, [mapUnit]);

        wrapper.ThrowIfNull();
        biDirectionalWrapper.ThrowIfNull();

        // Add the wrappers to the IServiceCollection
        services.AddSingleton(interfaceWrapperType, wrapper);
        services.AddSingleton(interfaceBiDirectionalWrapperType, biDirectionalWrapper);
    }
}