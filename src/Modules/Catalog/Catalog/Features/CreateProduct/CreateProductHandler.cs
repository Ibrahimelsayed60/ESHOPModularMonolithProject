using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Features.CreateProduct
{

    public record CreateProductCommand(String Name, List<string> Category, string Description, string ImageFile, decimal Price) : IRequest<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        public Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
