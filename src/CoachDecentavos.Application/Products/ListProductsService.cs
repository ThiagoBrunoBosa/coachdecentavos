using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Application.Products.Contracts;

namespace CoachDecentavos.Application.Products;

public sealed class ListProductsService
{
    private readonly IProductRepository _productRepository;

    public ListProductsService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<ProductSummaryDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.ListPublishedAsync(cancellationToken);
        return products
            .Select(p => new ProductSummaryDto(
                p.Id, p.Slug, p.Name, p.Type, p.Price, p.Currency, p.HotmartCheckoutUrl))
            .ToList();
    }
}