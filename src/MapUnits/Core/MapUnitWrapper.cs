using Microsoft.Extensions.DependencyInjection;

using Throw;

namespace MapUnits.Core;

internal sealed class MapUnitWrapper<TSourceA>(
    IMapper mapper,
    IServiceProvider serviceProvider) : IMapGeneral<TSourceA>
{
    internal TSourceA? SourceA { get; set; }

    private readonly IMapper _mapper = mapper;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public TSourceA From<TSourceB>(TSourceB sourceB)
    {
        SourceA = GetMapUnit<TSourceB>().MapFrom(sourceB, _mapper);
        return SourceA;
    }

    public TSourceB To<TSourceB>()
    {
        SourceA.ThrowIfNull();
        return GetMapUnit<TSourceB>().MapTo(SourceA, _mapper);
    }

    public TSourceB To<TSourceB>(TSourceB sourceB)
    {
        SourceA.ThrowIfNull();
        return GetMapUnit<TSourceB>().MapTo(SourceA, sourceB, _mapper);
    }

    private IMapUnit<TSourceA, TSourceB> GetMapUnit<TSourceB>()
        => _serviceProvider.GetRequiredService<IMapUnit<TSourceA, TSourceB>>();
}