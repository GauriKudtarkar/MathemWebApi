namespace DeliveryDatesGenerator
{
    public class OrderDateProvider : IOrderDateProvider
    {
        public virtual DateTimeOffset GetOrderDate() => DateTime.Now.Date;

        public virtual int? RemainingDaysOfCurrentWeek()
        {
                int orderDay = (int)GetOrderDate().DayOfWeek;
                if (orderDay == 0)//for Sunday, return null
                {
                    return null;
                }
                else
                {
                    return 7 - orderDay;
                }
        }
    }
}
