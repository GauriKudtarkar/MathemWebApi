using DeliveryDatesGenerator;
using DeliveryDatesGenerator.Models;

namespace DeliveryDatesGeneratorTests
{
    public class ProductTypeHandlerTests
    {
        [Fact]
        public void WhenTemporaryHandlersHandleMethodIsCalledAndOrderDateIsSunday_ThenReturnNull()
        {
            // Arrange
            var dummyDateProvider = new DummyOrderDateProvider(new DateTime(2022, 10, 23));
            TemporaryProductHandler temporaryProductHandler = new TemporaryProductHandler(dummyDateProvider);
            Product product = new Product
            {
                productId = 1,
                DaysInAdvance = 3, //This Field will be ignored for Temporary item 
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Sunday},
                Name = "Banana",
                Quantity = 4,
                TypeOfProduct = ProductType.Temporary
            };

            // Act
            var result = temporaryProductHandler.Handle(product);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [MemberData(nameof(DateAndRemainingDaysInWeek))]
        public void WhenTemporaryHandlersHandleMethodIsCalledAndOrderDateIsOtherThanSunday_ThenReturnRemainingDaysForWeek(DateTime orderDate, int? expectedRemainingDays)
        {
            // Arrange
            var dummyDateProvider = new DummyOrderDateProvider(orderDate);
            TemporaryProductHandler temporaryProductHandler = new TemporaryProductHandler(dummyDateProvider);
            Product product = new Product
            {
                productId = 1,
                DaysInAdvance = 3, //This Field will be ignored for Temporary item 
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Sunday },
                Name = "Banana",
                Quantity = 4,
                TypeOfProduct = ProductType.Temporary
            };

            // Act
            var actualRemainingDays = temporaryProductHandler.Handle(product);

            // Assert
            Assert.Equal(expectedRemainingDays,actualRemainingDays);
        }

        public static IEnumerable<object[]> DateAndRemainingDaysInWeek()
        {
            yield return new object[] { new DateTime(2022,10,17), 6 };
            yield return new object[] { new DateTime(2022,10,18), 5 };
            yield return new object[] { new DateTime(2022,10,19), 4 };
            yield return new object[] { new DateTime(2022,10,20), 3 };
            yield return new object[] { new DateTime(2022,10,21), 2 };
            yield return new object[] { new DateTime(2022,10,22), 1 };
        }

        [Fact]
        public void WhenExternalHandlersHandleMethodIsCalled_ThenReturn5()
        {
            // Arrange
            ExternalProductHandler externalProductHandler = new ExternalProductHandler();
            Product product = new Product
            {
                productId = 1,
                DaysInAdvance = 3, //This Field will be ignored for External item 
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Sunday },
                Name = "Banana",
                Quantity = 4,
                TypeOfProduct = ProductType.External
            };

            // Act
            var result = externalProductHandler.Handle(product);

            // Assert
            Assert.Equal(5,result);
        }

        [Fact]
        public void WhenNormalHandlersHandleMethodIsCalled_ThenReturnDaysInAdvanceValue()
        {
            // Arrange
            NormalProductHandler normalProductHandler = new NormalProductHandler();
            Product product = new Product
            {
                productId = 1,
                DaysInAdvance = 3,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Sunday },
                Name = "Banana",
                Quantity = 4,
                TypeOfProduct = ProductType.Normal
            };

            // Act
            var result = normalProductHandler.Handle(product);

            // Assert
            Assert.Equal(product.DaysInAdvance, result);
        }
    }
}
