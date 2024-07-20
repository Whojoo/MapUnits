namespace MapUnits.Core;

internal abstract class AbstractMapUnitWrapper<TSourceA, TSourceB>(
    IMapUnit<TSourceA, TSourceB> mapUnit
)
{
    private readonly IMapUnit<TSourceA, TSourceB> _mapUnit = mapUnit;

    public TSourceA Map(TSourceB source, IMapper mapper)
        => _mapUnit.CreateSourceAFromSourceB(source, mapper);

    public TSourceA Map(TSourceB source, TSourceA destination, IMapper mapper)
        => _mapUnit.MapSourceAFromSourceB(source, destination, mapper);

    public TSourceB Map(TSourceA source, IMapper mapper)
        => _mapUnit.CreateSourceBFromSourceA(source, mapper);

    public TSourceB Map(TSourceA source, TSourceB destination, IMapper mapper)
        => _mapUnit.MapSourceBFromSourceA(source, destination, mapper);
}