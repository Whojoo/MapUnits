using MapUnits.Interfaces;

namespace MapUnits.Core;

internal sealed class BiDirectionalMapUnitWrapper<TSourceA, TSourceB>(
    IMapUnit<TSourceA, TSourceB> mapUnit
) : AbstractMapUnitWrapper<TSourceA, TSourceB>(mapUnit), IMapUnitWrapper<TSourceB, TSourceA>
{
}