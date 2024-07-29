namespace MapUnits.Interfaces;

internal interface IMapUnitWrapper
{
}

internal interface IMapUnitWrapper<TSourceA, TSourceB> : IMapUnitWrapper
{
    TSourceA Map(TSourceB source, IMapper mapper);

    TSourceA Map(TSourceB source, TSourceA destination, IMapper mapper);

    TSourceB Map(TSourceA source, IMapper mapper);

    TSourceB Map(TSourceA source, TSourceB destination, IMapper mapper);
}