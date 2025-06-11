using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using MediatR;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Catalog.Products.Features.CreateProduct
{

    public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductHandler(CatalogDbContext dbContext) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create product entity from command object
            // save to database
            // return result

            var product = CreateNewProduct(command.Product);
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);
        }

        private Product CreateNewProduct(ProductDto productDto)
        {
            var product = Product.Create(
                Guid.NewGuid(),
                productDto.Name,
                productDto.Category,
                productDto.Description,
                productDto.ImageFile,
                productDto.Price
                );

            return product;
        }
    }
}
