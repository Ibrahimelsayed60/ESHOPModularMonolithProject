using Basket.Basket.Dtos;
using Basket.Basket.Exceptions;
using Basket.Data;
using Basket.Data.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Basket.Features.AddItemIntoBasket
{
    public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;
    public record AddItemIntoBasketResult(Guid Id);
    public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
    {
        public AddItemIntoBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }

    internal class AddItemIntoBasketHandler(IBasketRepository repository)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
    {
        public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
        {
            // Add shopping cart item into shopping cart
            var shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);

            shoppingCart.AddItem(
                    command.ShoppingCartItem.ProductId,
                    command.ShoppingCartItem.Quantity,
                    command.ShoppingCartItem.Color,
                    command.ShoppingCartItem.Price,
                    command.ShoppingCartItem.ProductName);

            await repository.SaveChangesAsync(command.UserName, cancellationToken);

            return new AddItemIntoBasketResult(shoppingCart.Id);
        }
    }
}
