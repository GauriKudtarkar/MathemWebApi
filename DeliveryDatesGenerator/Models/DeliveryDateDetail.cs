namespace DeliveryDatesGenerator.Models
{
    public class DeliveryDateDetail
    {
        public string PostalCode { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public bool IsGreenDelivery { get; set; }

        public int GetPriority(DateTimeOffset orderDate)
        {
                if (DeliveryDate.Subtract(orderDate).TotalDays <= 3)
                {
                    if (IsGreenDelivery)
                    {
                        return 1;
                    }
                }
                    return 0;
        }
    }
}