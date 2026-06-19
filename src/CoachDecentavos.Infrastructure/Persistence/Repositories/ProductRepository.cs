using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoachDecentavos.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Product>> ListPublishedAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Products
            .Where(x => x.IsPublished)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

    public Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        => _dbContext.Products.FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);

    public Task<Product?> GetByHotmartProductIdAsync(string hotmartProductId, CancellationToken cancellationToken = default)
        => _dbContext.Products.FirstOrDefaultAsync(x => x.HotmartProductId == hotmartProductId, cancellationToken);
}