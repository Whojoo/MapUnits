namespace MapUnits;

public interface IMapUnit<TSourceA, SourceB>
{
    SourceB MapTo(TSourceA sourceA, IMapper mapper);

    SourceB MapTo(TSourceA sourceA, SourceB sourceB, IMapper mapper);

    TSourceA MapFrom<TSourceB>(TSourceB sourceB, IMapper mapper);
}