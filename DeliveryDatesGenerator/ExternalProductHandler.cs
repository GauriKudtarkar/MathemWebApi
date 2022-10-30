using DeliveryDatesGenerator.Models;

namespace DeliveryDatesGenerator
{
    public class ExternalProductHandler : AbstractProductTypeHandler
    {
        /// <summary>
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns 5 as DaysInAdvance value</returns>
        public override int? Handle(Product product)
        {
            if (product.TypeOfProduct == ProductType.External)
            {
                return 5;
            }
            else
            {
                return base.Handle(product);
            }
        }
    }

}