namespace DeliveryDatesGenerator
{
    public interface IOrderDateProvider
    {
        DateTimeOffset GetOrderDate();

        int? RemainingDaysOfCurrentWeek();
    }
}
