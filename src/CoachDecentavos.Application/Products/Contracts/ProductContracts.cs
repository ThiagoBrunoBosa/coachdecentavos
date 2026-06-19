using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Application.Products.Contracts;

public sealed record ProductSummaryDto(
    Guid Id,
    string Slug,
    string Name,
    ProductType Type,
    decimal Price,
    string Currency,
    string? HotmartCheckoutUrl);

public sealed record ProductDetailDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    ProductType Type,
    decimal Price,
    string Currency,
    string? HotmartCheckoutUrl);