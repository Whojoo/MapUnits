using MapUnits.Interfaces;
using Throw;

namespace MapUnits.Core;

internal sealed class WrapperLookupService
{
    private readonly Dictionary<Tuple<Type, Type>, IMapUnitWrapper> _wrapperDict = new();

    public void AddWrappersFor(Type sourceAType, Type sourceBType, IMapUnitWrapper wrapper, IMapUnitWrapper reversedWrapper)
    {
        var wrapperTuple = new Tuple<Type, Type>(sourceAType, sourceBType);
        var reversedWrapperTuple = new Tuple<Type, Type>(sourceBType, sourceAType);

        _wrapperDict
            .Throw($"Duplicate MapUnit configuration detected for types {sourceAType.FullName} and {sourceBType.FullName}")
            .IfTrue(dict => dict.ContainsKey(wrapperTuple))
            .IfTrue(dict => dict.ContainsKey(reversedWrapperTuple));

        _wrapperDict[wrapperTuple] = wrapper;
        _wrapperDict[reversedWrapperTuple] = reversedWrapper;
    }

    public IMapUnitWrapper<TSourceA, TSourceB> GetWrapper<TSourceA, TSourceB>()
    {
        var sourceAType = typeof(TSourceA);
        var sourceBType = typeof(TSourceB);
        var tuple = new Tuple<Type, Type>(sourceAType, sourceBType);

        var wrapperUntyped = _wrapperDict[tuple];

        wrapperUntyped.ThrowIfNull($"Could not find MapUnit for {sourceAType.FullName} and {sourceBType.FullName}");

        return (IMapUnitWrapper<TSourceA, TSourceB>)wrapperUntyped;
    }
}