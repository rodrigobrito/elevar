using System;
using System.Collections.Generic;
using Elevar.Security;

namespace Elevar.Tests.Security.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public IList<Item> Items { get; set; } = new List<Item>();

        public override bool Equals(object obj)
        {
            return obj is Delivery delivery &&
                   Id == delivery.Id &&
                   Date == delivery.Date &&
                   EqualityComparer<IList<Item>>.Default.Equals(Items, delivery.Items);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper
                  .Of(Id)
                  .And(Date)
                  .AndEach(Items);
        }
    }
}
