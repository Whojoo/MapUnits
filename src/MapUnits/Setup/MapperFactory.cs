using System.Reflection;
using MapUnits.Core;
using MapUnits.Interfaces;
using Throw;

namespace MapUnits.Setup;

internal static class MapperFactory
{
    internal static IMapper CreateMapper()
        => CreateMapper([Assembly.GetEntryAssembly()]);

    internal static IMapper CreateMapper(List<Assembly?> assemblies)
    {
        var wrapperLookupService = new WrapperLookupService();
        var mapper = new Mapper(wrapperLookupService);

        // Find all MapUnits (TODO: Deep search assembly?)
        var mapUnitTypes = GetMapUnitTypes(assemblies);

        // Add the MapUnits to their wrappers and DI
        CreateWrappersAndAddToWrapperLookup(mapUnitTypes, wrapperLookupService);

        return mapper;
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

    private static void CreateWrappersAndAddToWrapperLookup(List<Type> mapUnitTypes, WrapperLookupService wrapperLookupService)
    {
        foreach (var mapUnitType in mapUnitTypes)
        {
            CreateWrapperAndAddToServiceCollection(mapUnitType, wrapperLookupService);
        }
    }

    private static void CreateWrapperAndAddToServiceCollection(Type mapUnitType, WrapperLookupService wrapperLookupService)
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

        // Instantiate the wrappers
        var wrapper = Activator.CreateInstance(wrapperType, [mapUnit]);
        var reversedWrapper = Activator.CreateInstance(biDirectionalWrapperType, [mapUnit]);

        wrapper.ThrowIfNull();
        reversedWrapper.ThrowIfNull();

        // Add the wrappers to lookup service
        wrapperLookupService.AddWrappersFor(
            sourceAType: genericArguments[0],
            sourceBType: genericArguments[1],
            wrapper: (IMapUnitWrapper)wrapper,
            reversedWrapper: (IMapUnitWrapper)reversedWrapper
        );
    }
}