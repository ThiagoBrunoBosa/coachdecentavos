using CoachDecentavos.Application.Products;

namespace CoachDecentavos.Api.Routes;

public static class ProductRoutes
{
    public static RouteGroupBuilder MapProductRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/products", async (
            ListProductsService listProductsService,
            CancellationToken cancellationToken) =>
        {
            var products = await listProductsService.ExecuteAsync(cancellationToken);
            return Results.Ok(products);
        }).AllowAnonymous();

        group.MapGet("/products/{slug}", async (
            string slug,
            GetProductService getProductService,
            CancellationToken cancellationToken) =>
        {
            var product = await getProductService.ExecuteAsync(slug, cancellationToken);
            return Results.Ok(product);
        }).AllowAnonymous();

        return group;
    }
}