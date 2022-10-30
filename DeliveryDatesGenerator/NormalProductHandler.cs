using DeliveryDatesGenerator.Models;

namespace DeliveryDatesGenerator
{
    
    public class NormalProductHandler : AbstractProductTypeHandler
    {
        /// <summary>
        /// This will return the value of "DaysInAdvance" Field from product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>This will return the value of "DaysInAdvance" Field from product</returns>
        public override int? Handle(Product product)
        {
            if (product.TypeOfProduct == ProductType.Normal)
            {
                return product.DaysInAdvance;
            }
            else
            {
                return base.Handle(product);
            }
        }
    }

}