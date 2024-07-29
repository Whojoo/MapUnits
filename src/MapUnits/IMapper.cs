namespace MapUnits;

public interface IMapper
{
    // IMapGeneral<TSource> Map<TSource>(TSource source);

    // IMapFrom<TSource> Map<TSource>();

    TResult Map<TResult, TSource>(TSource source);

    TResult Map<TResult, TSource>(TSource source, TResult destination);
}