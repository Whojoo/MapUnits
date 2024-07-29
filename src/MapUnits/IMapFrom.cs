namespace MapUnits;

public interface IMapFrom<TSourceA>
{
    TSourceA From<TSourceB>(TSourceB source);
}