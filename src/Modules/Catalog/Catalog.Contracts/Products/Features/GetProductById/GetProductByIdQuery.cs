using Catalog.Contracts.Products.Dtos;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Contracts.Products.Features.GetProductById
{
    public record GetProductByIdQuery(Guid Id)
        : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(ProductDto Product);
}
