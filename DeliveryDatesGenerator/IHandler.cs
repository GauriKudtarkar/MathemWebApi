using DeliveryDatesGenerator.Models;

namespace DeliveryDatesGenerator
{
    public interface IHandler
    {
        IHandler SetNext(IHandler handler);

        int? Handle(Product product);
    }

}