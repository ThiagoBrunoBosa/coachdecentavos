using CoachDecentavos.Application.Common.Exceptions;
using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Application.Products.Contracts;

namespace CoachDecentavos.Application.Products;

public sealed class GetProductService
{
    private readonly IProductRepository _productRepository;

    public GetProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDetailDto> ExecuteAsync(string slug, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetBySlugAsync(slug.Trim().ToLowerInvariant(), cancellationToken);
        if (product is null || !product.IsPublished)
            throw new NotFoundException("Product not found.");

        return new ProductDetailDto(
            product.Id,
            product.Slug,
            product.Name,
            product.Description,
            product.Type,
            product.Price,
            product.Currency,
            product.HotmartCheckoutUrl);
    }
}