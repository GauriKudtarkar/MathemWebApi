using DeliveryDatesGenerator;

namespace MathemWebApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDependenciesForDeliveryDatesCalculator(this IServiceCollection Services)
        {
            Services.AddTransient<IDeliveryDatesCalculator, DeliveryDatesCalculator>();
            Services.AddTransient<IOrderDateProvider, OrderDateProvider>();
            Services.AddSingleton<NormalProductHandler>();
            Services.AddSingleton<ExternalProductHandler>();
            Services.AddSingleton<TemporaryProductHandler>();
        }
    }
}

