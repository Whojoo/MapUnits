using MapUnits.Interfaces;

namespace MapUnits.Core;

internal sealed class MapUnitWrapper<TSourceA, TSourceB>(
    IMapUnit<TSourceA, TSourceB> mapUnit
) : AbstractMapUnitWrapper<TSourceA, TSourceB>(mapUnit), IMapUnitWrapper<TSourceA, TSourceB>
{
}