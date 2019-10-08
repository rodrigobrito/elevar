using Elevar.Security;

namespace Elevar.Tests.Security.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Item item &&
                   Id == item.Id &&
                   Description == item.Description &&
                   Price == item.Price;
        }

        public override int GetHashCode()
        {
            return HashCodeHelper
                    .Of(Id)
                    .And(Description)
                    .And(Price);
        }
    }
}
