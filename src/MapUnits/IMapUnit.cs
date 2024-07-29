namespace MapUnits;

public interface IMapUnit<TSourceA, TSourceB>
{
    TSourceA CreateSourceAFromSourceB(TSourceB sourceB, IMapper mapper);
    TSourceB CreateSourceBFromSourceA(TSourceA sourceA, IMapper mapper);

    TSourceA MapSourceAFromSourceB(TSourceB sourceB, TSourceA existingSourceA, IMapper mapper);
    TSourceB MapSourceBFromSourceA(TSourceA sourceA, TSourceB existingSourceB, IMapper mapper);
}