namespace MiniERP.Application.Abstractions;

public interface IMapper<TSource, TDestination>
{
    TDestination Map(TSource source);
    TSource Map(TDestination destination);
}