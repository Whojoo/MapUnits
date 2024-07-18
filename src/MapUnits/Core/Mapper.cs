namespace MapUnits.Core;

internal sealed class Mapper(IServiceProvider serviceProvider) : IMapper
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IMapGeneral<TSource> Map<TSource>(TSource source)
    {
        throw new NotImplementedException();
    }

    public IMapFrom<TSource> Map<TSource>()
    {
        throw new NotImplementedException();
    }
}