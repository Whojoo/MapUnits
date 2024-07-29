using Throw;

namespace MapUnits.Core;

internal sealed class Mapper(WrapperLookupService wrapperLookupService) : IMapper
{
    private readonly WrapperLookupService _wrapperLookupService = wrapperLookupService;

    public TResult Map<TResult, TSource>(TSource source)
    {
        var mapUnitWrapper = _wrapperLookupService.GetWrapper<TResult, TSource>();
        mapUnitWrapper.ThrowIfNull($"No MapUnit found for result {typeof(TResult)} and source {typeof(TSource)}");

        return mapUnitWrapper.Map(source, this);
    }

    public TResult Map<TResult, TSource>(TSource source, TResult destination)
    {
        var mapUnitWrapper = _wrapperLookupService.GetWrapper<TResult, TSource>();
        mapUnitWrapper.ThrowIfNull($"No MapUnit found for result {typeof(TResult)} and source {typeof(TSource)}");

        return mapUnitWrapper.Map(source, destination, this);
    }
}