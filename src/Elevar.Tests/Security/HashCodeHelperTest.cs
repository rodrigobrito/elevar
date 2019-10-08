using System;
using Elevar.Security;
using Elevar.Tests.Security.Models;
using Xunit;

namespace Elevar.Tests.Security
{
    public class HashCodeHelperTest
    {
        [Fact]
        public void HashCode_SuccessfulMatch_SameOrders()
        {
            var firstOrderHash = Order.CreateDefault().GetHashCode();
            var secondOrderHash = Order.CreateDefault().GetHashCode();
            Assert.Equal(firstOrderHash, secondOrderHash);
        }

        [Fact]
        public void HashCode_SuccessfulMatch_ChangedOrders()
        {
            var firstOrder = Order.CreateDefault();
            var secondOrder = Order.CreateDefault();

            firstOrder.Id = 1054;
            secondOrder.Id = 1054;

            firstOrder.Deliveries[0].Date = new DateTime(2018, 04, 04);
            secondOrder.Deliveries[0].Date = new DateTime(2018, 04, 04);

            firstOrder.Deliveries[0].Items[0].Price = 5.4M;
            secondOrder.Deliveries[0].Items[0].Price = 5.4M;

            var firstOrderHash = firstOrder.GetHashCode();
            var secondOrderHash = secondOrder.GetHashCode();

            Assert.Equal(firstOrderHash, secondOrderHash);
        }

        [Fact]
        public void HashCode_FailedlMatch_ChangedOrders()
        {
            var firstOrder = Order.CreateDefault();
            var secondOrder = Order.CreateDefault();

            secondOrder.Id = 1054;

            var firstOrderHash = firstOrder.GetHashCode();
            var secondOrderHash = secondOrder.GetHashCode();

            Assert.NotEqual(firstOrderHash, secondOrderHash);
        }

        [Fact]
        public void HashCode_SuccessfulMatch_ForCollections()
        {
            var deliveries1 = Order.CreateDefault().Deliveries;
            var deliveries2 = Order.CreateDefault().Deliveries;

            var hash1 = HashCodeHelper.Of(deliveries1).GetHashCode();
            var hash2 = HashCodeHelper.Of(deliveries2).GetHashCode();

            Assert.NotEqual(hash1, hash2);
        }
    }
}
