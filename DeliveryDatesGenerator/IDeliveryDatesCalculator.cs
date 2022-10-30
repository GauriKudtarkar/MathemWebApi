using DeliveryDatesGenerator.Models;

namespace DeliveryDatesGenerator
{
    public interface IDeliveryDatesCalculator
    {
        IEnumerable<DeliveryDateDetail> GetDeliveryDates(string postalCode, HashSet<Product> products);

        IEnumerable<DeliveryDateDetail> SortDeliveryDates(IEnumerable<DeliveryDateDetail> deliveryDates);
    }
}