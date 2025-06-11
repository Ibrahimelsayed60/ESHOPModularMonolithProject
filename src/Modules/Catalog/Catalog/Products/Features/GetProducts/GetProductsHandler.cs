using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;


namespace Catalog.Products.Features.GetProducts
{

    public record GetProductsQuery():IQuery<GetProductsResult>;

    public record GetProductsResult(IEnumerable<ProductDto> Products);

    public class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await dbContext.Products.AsNoTracking().OrderBy(p => p.Name).ToListAsync();

            var productDtos = products.Adapt<List<ProductDto>>();
           

            return new GetProductsResult(productDtos);
        }

        
    }
}
