using DeliveryDatesGenerator.Models;

namespace DeliveryDatesGenerator
{
    public class TemporaryProductHandler : AbstractProductTypeHandler
    {
        private readonly IOrderDateProvider orderDateProvider;

        public TemporaryProductHandler(IOrderDateProvider _orderDateProvider)
        {
            orderDateProvider = _orderDateProvider;
        }

        /// <summary>
        /// This returns the remaining days of the week as DaysInAdvance value.
        /// If it is on Sunday then it will return null.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns valid int value if order for temporary product is not on sunday, else will return null</returns>
        public override int? Handle(Product product)
        {
            if (product.TypeOfProduct == ProductType.Temporary)
            {
                return orderDateProvider.RemainingDaysOfCurrentWeek();
            }
            else
            {
                return base.Handle(product);
            }
        }
    }

}