﻿using Basket.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Basket.Features.UpdateItemPriceInBasket
{
    public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price)
    : ICommand<UpdateItemPriceInBasketResult>;
    public record UpdateItemPriceInBasketResult(bool IsSuccess);
    public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
    {
        public UpdateItemPriceInBasketCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class UpdateItemPriceInBasketHandler(BasketDbContext dbContext)
        : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
    {
        public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
        {
            //Find Shopping Cart Items with a give ProductId
            //Iterate items and Update Price of every item with incoming command.Price
            //save to database
            //return result

            var itemsToUpdate = await dbContext.ShoppingCartItems
                  .Where(x => x.ProductId == command.ProductId)
                  .ToListAsync(cancellationToken);

            if (!itemsToUpdate.Any())
            {
                return new UpdateItemPriceInBasketResult(false);
            }

            foreach (var item in itemsToUpdate)
            {
                item.UpdatePrice(command.Price);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateItemPriceInBasketResult(true);
        }
    }
}
