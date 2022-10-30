using DeliveryDatesGenerator;

namespace DeliveryDatesGeneratorTests
{
    public class DummyOrderDateProvider : OrderDateProvider
    {
        public DummyOrderDateProvider(DateTimeOffset _orderDate)
        {
            orderDate = _orderDate;
        }

        public DateTimeOffset orderDate { get; }

        public override DateTimeOffset GetOrderDate()
        {
            return orderDate;
        }
    }
}
