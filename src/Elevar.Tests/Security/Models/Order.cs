using System;
using System.Collections.Generic;
using Elevar.Security;

namespace Elevar.Tests.Security.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public IList<Delivery> Deliveries { get; private set; } = new List<Delivery>();

        public override bool Equals(object obj)
        {
            var order = obj as Order;
            return order != null &&
                   Id == order.Id &&
                   CustomerName == order.CustomerName &&
                   EqualityComparer<IList<Delivery>>.Default.Equals(Deliveries, order.Deliveries);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper
                   .Of(Id)
                   .And(CustomerName)
                   .AndEach(Deliveries);
        }

        public static Order CreateDefault()
        {
            return new Order
            {
                Id = 1,
                CustomerName = "Rodrigo Brito",
                Deliveries = new List<Delivery>
                {
                    new Delivery
                    {
                        Id = 1,
                        Date = new DateTime(2018, 6, 2),
                        Items = new List<Item>
                        {
                            new Item
                            {
                                Id = 1,
                                Description = "Mobile Phone",
                                Price = 1500.45m
                            }
                        }
                    }
                }
            };
        }
    }
}
