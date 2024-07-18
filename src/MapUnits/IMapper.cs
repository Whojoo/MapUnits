namespace MapUnits;

public interface IMapper
{
    IMapGeneral<TSource> Map<TSource>(TSource source);

    IMapFrom<TSource> Map<TSource>();
}