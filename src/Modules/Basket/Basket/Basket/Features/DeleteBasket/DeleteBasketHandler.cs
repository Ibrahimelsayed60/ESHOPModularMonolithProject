using Basket.Basket.Exceptions;
using Basket.Data;
using Basket.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Basket.Features.DeleteBasket
{
    public record DeleteBasketCommand(string UserName)
    : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);

    internal class DeleteBasketHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            //Delete Basket entity from command object
            //save to database
            //return result

            await repository.DeleteBasket(command.UserName, cancellationToken);

            return new DeleteBasketResult(true);
        }
    }
}
