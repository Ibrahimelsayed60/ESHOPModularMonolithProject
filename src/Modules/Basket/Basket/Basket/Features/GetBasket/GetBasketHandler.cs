using Basket.Basket.Dtos;
using Basket.Basket.Exceptions;
using Basket.Data;
using Basket.Data.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Basket.Features.GetBasket
{
    public record GetBasketQuery(string UserName)
    : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCartDto ShoppingCart);

    internal class GetBasketHandler(IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            // get basket with userName
            var basket = await repository.GetBasket(query.UserName, true, cancellationToken);

            //mapping basket entity to shoppingcartdto
            var basketDto = basket.Adapt<ShoppingCartDto>();

            return new GetBasketResult(basketDto);
        }
    }
}
