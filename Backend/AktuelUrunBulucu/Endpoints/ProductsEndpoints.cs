using AktuelUrunBulucu.BLL.Services;
using AktuelUrunBulucu.DAL.Repositories;

namespace AktuelUrunBulucu.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (IProductRepository productRepository) =>
        {
            var products = await productRepository.GetAllAsync();

            var result = products.Select(p => new ProductResultDto(
                p.Id,
                p.Name,
                p.Category,
                p.ProductBringDate,
                p.StoreName
            )).ToList();

            return Results.Ok(result);
        })
        .WithName("GetAllProducts")
        .WithTags("Products");
    }
}
