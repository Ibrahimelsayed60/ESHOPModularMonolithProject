using Catalog.Products.Models;
using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.Events
{
    public record ProductCreatedEvent(Product Product):IDomainEvent;


    
    
}
