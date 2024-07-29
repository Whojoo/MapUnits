namespace MapUnits;

public interface IMapGeneral<TSourceA> : IMapFrom<TSourceA>
{
    TSourceB To<TSourceB>();

    TSourceB To<TSourceB>(TSourceB destination);
}