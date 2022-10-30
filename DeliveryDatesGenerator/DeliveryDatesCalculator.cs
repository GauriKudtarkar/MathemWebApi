using DeliveryDatesGenerator.Models;

namespace DeliveryDatesGenerator
{
    public class DeliveryDatesCalculator : IDeliveryDatesCalculator
    {
        private readonly NormalProductHandler normalProductHandler;
        private readonly ExternalProductHandler externalProductHandler;
        private readonly TemporaryProductHandler temporaryProductHandler;
        private readonly IOrderDateProvider orderDateProvider;

        public DeliveryDatesCalculator(NormalProductHandler _normalProductHandler, ExternalProductHandler _externalProductHandler, TemporaryProductHandler _temporaryProductHandler, IOrderDateProvider _orderDateProvider)
        {
            normalProductHandler = _normalProductHandler;
            externalProductHandler = _externalProductHandler;
            temporaryProductHandler = _temporaryProductHandler;
            orderDateProvider = _orderDateProvider;
            this.normalProductHandler.SetNext(externalProductHandler).SetNext(temporaryProductHandler);
        }

        /// <summary>
        /// Given a postal code and list of products, this method returns the valid delivery dates within next 14 days
        /// </summary>
        /// <param name="postalCode">Postal Code</param>
        /// <param name="products">List of products</param>
        /// <returns></returns>
        public IEnumerable<DeliveryDateDetail> GetDeliveryDates(string postalCode, HashSet<Product> products)
        {
            List<DeliveryDateDetail> deliveryDates = new List<DeliveryDateDetail>();
            HashSet<DayOfWeek> commonWeekdays = GetCommonDeliveryDaysForOrder(products);
            //if no common weekday is found then return blank list of DeliveryDates
            if (!commonWeekdays.Any())
            { return deliveryDates.ToList(); }

            int? daysInAdvanceForOrder = GetDaysInAdvanceForOrder(products);
            if (daysInAdvanceForOrder.HasValue)
            {
                deliveryDates.AddRange(CalculateDeliveryDatesBasedOnDaysInAdvance(daysInAdvanceForOrder.Value, commonWeekdays, postalCode));
            }
            return deliveryDates;
        }

        /// <summary>
        /// Sort the given delivery dates list first by priority (if it is greenDelivery then high priority) if they are within next 3 days, else sorted by dates.
        /// </summary>
        /// <param name="deliveryDates">List of delivery dates</param>
        /// <returns></returns>
        public IEnumerable<DeliveryDateDetail> SortDeliveryDates(IEnumerable<DeliveryDateDetail> deliveryDates)
        {
            return deliveryDates.OrderByDescending(d => d.GetPriority(orderDateProvider.GetOrderDate())).ThenBy(d => d.DeliveryDate);
        }

        private IEnumerable<DeliveryDateDetail> CalculateDeliveryDatesBasedOnDaysInAdvance(int daysInAdvanceForOrder, HashSet<DayOfWeek> commonWeekdays, string postalCode)
        {
            List<DeliveryDateDetail> deliveryDates = new List<DeliveryDateDetail>();
            var startPeriod = orderDateProvider.GetOrderDate().AddDays(daysInAdvanceForOrder + 1);
            var endPeriod = orderDateProvider.GetOrderDate().AddDays(14);
            do
            {
                if (commonWeekdays.Contains(startPeriod.DayOfWeek))
                {
                    deliveryDates.Add(new DeliveryDateDetail { PostalCode = postalCode, DeliveryDate = startPeriod, IsGreenDelivery = IsGreenDelivery(startPeriod.Day) });
                }
                startPeriod = startPeriod.AddDays(1);
            }
            while (endPeriod >= startPeriod);
            return deliveryDates;
        }

        /// <summary>
        /// If date is multiple of 7 then it is greenDelivery.
        /// </summary>
        /// <param name="day"></param>
        /// <returns>True if its green else false</returns>
        private bool IsGreenDelivery(int day)
        {
            var greenDeliveryDays = new HashSet<int>() { 7, 14, 21, 28 };
            if (greenDeliveryDays.Contains(day))
                return true;
            else
                return false;
        }

        /// <summary>
        /// This method checks if all the products have any common weekdays as delivery days or not. 
        /// </summary>
        /// <param name="products"></param>
        /// <returns>This will return a set of weekdays which are common for all products</returns>
        private HashSet<DayOfWeek> GetCommonDeliveryDaysForOrder(HashSet<Product> products)
        {
            HashSet<DayOfWeek> commonWeekdays = new HashSet<DayOfWeek>();
            int counter = -1;
            foreach (Product product in products)
            {
                counter++;
                if (counter == 0)
                {
                    commonWeekdays = product.DeliveryDays;
                    continue;
                }
                else
                {
                    commonWeekdays.IntersectWith(product.DeliveryDays);
                }
            }
            return commonWeekdays;
        }

        /// <summary>
        /// This method returns the DaysInAdvance for all types of product.
        /// </summary>
        /// <param name="products"></param>
        /// <returns>Returns valid int value else return null if product type does not fall into specific category</returns>
        private int? GetDaysInAdvanceForOrder(HashSet<Product> products)
        {
            int? highestDaysInAdvance = 0;
            foreach (var product in products)
            {
                int? daysInAdvance = normalProductHandler.Handle(product);
                if (daysInAdvance == null)
                {
                    highestDaysInAdvance = null;
                    break;
                }
                highestDaysInAdvance = daysInAdvance > highestDaysInAdvance ? daysInAdvance : highestDaysInAdvance;
            }
            return highestDaysInAdvance;
        }
    }
}
