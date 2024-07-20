using MapUnits.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Throw;

namespace MapUnits.Core;

internal sealed class Mapper(IServiceProvider serviceProvider) : IMapper
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public TResult Map<TResult, TSource>(TSource source)
    {
        var mapUnitWrapper = _serviceProvider.GetService<IMapUnitWrapper<TResult, TSource>>();
        mapUnitWrapper.ThrowIfNull($"No MapUnit found for result {typeof(TResult)} and source {typeof(TSource)}");

        return mapUnitWrapper.Map(source, this);
    }

    public TResult Map<TResult, TSource>(TSource source, TResult destination)
    {
        var mapUnitWrapper = _serviceProvider.GetService<IMapUnitWrapper<TResult, TSource>>();
        mapUnitWrapper.ThrowIfNull($"No MapUnit found for result {typeof(TResult)} and source {typeof(TSource)}");

        return mapUnitWrapper.Map(source, destination, this);
    }
}