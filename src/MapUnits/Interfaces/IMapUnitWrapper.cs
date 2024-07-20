namespace MapUnits.Interfaces;

internal interface IMapUnitWrapper<TSourceA, TSourceB>
{
    TSourceA Map(TSourceB source, IMapper mapper);

    TSourceA Map(TSourceB source, TSourceA destination, IMapper mapper);

    TSourceB Map(TSourceA source, IMapper mapper);

    TSourceB Map(TSourceA source, TSourceB destination, IMapper mapper);
}