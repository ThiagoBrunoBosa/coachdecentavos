using CoachDecentavos.Domain.Entities;

namespace CoachDecentavos.Application.Common.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> ListPublishedAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Product?> GetByHotmartProductIdAsync(string hotmartProductId, CancellationToken cancellationToken = default);
}