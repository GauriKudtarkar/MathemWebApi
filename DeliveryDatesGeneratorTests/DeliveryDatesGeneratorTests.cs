using DeliveryDatesGenerator;
using DeliveryDatesGenerator.Models;
using Xunit;

namespace DeliveryDatesGeneratorTests
{
    public class DeliveryDatesGeneratorTests
    {
        private readonly IDeliveryDatesCalculator deliveryDatesCalculator;
        private DateTime orderDate;

        public DeliveryDatesGeneratorTests()
        {
            orderDate = new DateTime(2022, 10, 22);
            var dummyDateProvider = new DummyOrderDateProvider(orderDate);
            deliveryDatesCalculator = new DeliveryDatesCalculator(new NormalProductHandler(), new ExternalProductHandler(), new TemporaryProductHandler(dummyDateProvider), dummyDateProvider);
        }

        [Fact]
        public void WhenProductsDeliveryDaysDoesNotContainCommonWeekDay_ThenReturnBlankList()
        {
            // Arrange

            HashSet<Product> products = new HashSet<Product>() {
            new Product{
                productId = 1,
                DaysInAdvance = 3,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Sunday } ,
                Name = "Banana",
                Quantity = 4,
                TypeOfProduct = ProductType.Normal
            },
            new Product{
                productId = 1,
                DaysInAdvance = 3,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Wednesday } ,
                Name = "Sugar",
                Quantity = 1,
                TypeOfProduct = ProductType.Normal
            },
            new Product{
                productId = 1,
                DaysInAdvance = 3,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday } ,
                Name = "Oranges",
                Quantity = 4,
                TypeOfProduct = ProductType.Normal
            }
            };
            // Act
            var deliveryDates = deliveryDatesCalculator.GetDeliveryDates("12345", products);
            var result = deliveryDatesCalculator.SortDeliveryDates(deliveryDates);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void WhenProductsDeliveryDaysContainCommonWeekDayAllNormalItemsWithDaysInAdvanceProperty_ThenReturnedDatesFallsOnCommonDeliveryDays()
        {
            // Arrange

            HashSet<Product> products = new HashSet<Product>() {
            new Product{
                productId = 1,
                DaysInAdvance = 3,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Sunday, DayOfWeek.Wednesday } ,
                Name = "Banana",
                Quantity = 4,
                TypeOfProduct = ProductType.Normal
            },
            new Product{
                productId = 1,
                DaysInAdvance = 0,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday } ,
                Name = "Sugar",
                Quantity = 1,
                TypeOfProduct = ProductType.Normal
            },
            new Product{
                productId = 1,
                DaysInAdvance = 6,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Wednesday } ,
                Name = "Oranges",
                Quantity = 4,
                TypeOfProduct = ProductType.Normal
            }
            };
            // Act
            var deliveryDates = deliveryDatesCalculator.GetDeliveryDates("12345", products);
            var result = deliveryDatesCalculator.SortDeliveryDates(deliveryDates);

            // Assert
            var commonDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday };
            foreach (var dd in result)
            {
                Assert.Contains(dd.DeliveryDate.DayOfWeek, commonDays);
            }
        }

        [Fact]
        public void WhenProductsDeliveryDaysContainCommonWeekDayAllNormalItemsWithDaysInAdvanceProperty_ThenReturnedDatesAreLaterThanHigestDaysInAdvance()
        {
            // Arrange

            HashSet<Product> products = new HashSet<Product>() {
            new Product{
                productId = 1,
                DaysInAdvance = 3,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Sunday, DayOfWeek.Wednesday } ,
                Name = "Banana",
                Quantity = 4,
                TypeOfProduct = ProductType.Normal
            },
            new Product{
                productId = 1,
                DaysInAdvance = 0,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday } ,
                Name = "Sugar",
                Quantity = 1,
                TypeOfProduct = ProductType.Normal
            },
            new Product{
                productId = 1,
                DaysInAdvance = 6,
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Wednesday } ,
                Name = "Oranges",
                Quantity = 4,
                TypeOfProduct = ProductType.Normal
            }
            };
            // Act
            var deliveryDates = deliveryDatesCalculator.GetDeliveryDates("12345", products);
            var result = deliveryDatesCalculator.SortDeliveryDates(deliveryDates);

            // Assert
            var HigestDaysInAdvance = products.OrderByDescending(x => x.DaysInAdvance).First().DaysInAdvance;
            foreach (var dd in result)
            {
                Assert.True(dd.DeliveryDate.Subtract(orderDate).TotalDays > HigestDaysInAdvance);
            }
        }

        [Fact]
        public void WhenProductsListContainsOnlyExternalItems_ThenDaysInAdvanceValueIs5()
        {
            // Arrange

            HashSet<Product> products = new HashSet<Product>() {
            new Product{
                productId = 1,
                DaysInAdvance = 3,//This Field will be ignored for External item
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Sunday, DayOfWeek.Wednesday, DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday } ,
                Name = "Banana",
                Quantity = 4,
                TypeOfProduct = ProductType.External
            }
            };
            // Act
            var deliveryDates = deliveryDatesCalculator.GetDeliveryDates("12345", products);
            var result = deliveryDatesCalculator.SortDeliveryDates(deliveryDates);

            // Assert
            foreach (var dd in result)
            {
                Assert.True(dd.DeliveryDate.Subtract(orderDate).TotalDays > 5);
            }
        }

        [Fact]
        public void WhenProductsListContainsOnlyTemporaryItems_ThenDaysInAdvanceValueAreDecidedByRemainingDaysInWeek()
        {
            // Arrange
            HashSet<Product> products = new HashSet<Product>() {
            new Product{
                productId = 1,
                DaysInAdvance = 3, //This Field will be ignored for Temporary item 
                DeliveryDays = new HashSet<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Sunday, DayOfWeek.Wednesday, DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday } ,
                Name = "Banana",
                Quantity = 4,
                TypeOfProduct = ProductType.Temporary
            }
            };
            // Act
            var deliveryDates = deliveryDatesCalculator.GetDeliveryDates("12345", products);
            var result = deliveryDatesCalculator.SortDeliveryDates(deliveryDates);

            // Assert
            int? remainingDaysInWeek = orderDate.DayOfWeek == DayOfWeek.Sunday ? null : (7 - (int)orderDate.DayOfWeek);

            if (remainingDaysInWeek.HasValue)
            {
                foreach (var dd in result)
                {
                    Assert.True(dd.DeliveryDate.Subtract(orderDate).TotalDays > remainingDaysInWeek.Value);
                }
            }
            else
            {
                Assert.Empty(result);
            }
        }

        [Fact]
        public void WhenDeliveryDatesContainsGreenDeliveriesWithinNext3Days_ThenThoseDeliveryDatesHasHighPriorityAndComesOnTheTopOfTheList()
        {
            // Arrange
            List<DeliveryDateDetail> deliveryDates = new List<DeliveryDateDetail>() {
                new DeliveryDateDetail
                {
                    DeliveryDate = new DateTime(2022,10,24),
                    IsGreenDelivery = true,
                    PostalCode = "15172"
                },
                new DeliveryDateDetail{
                    DeliveryDate = new DateTime(2022,10,30),
                    IsGreenDelivery = false,
                    PostalCode = "15172"
                },
                new DeliveryDateDetail{
                    DeliveryDate = new DateTime(2022,10,26),
                    IsGreenDelivery = false,
                    PostalCode = "15172"
                },
                new DeliveryDateDetail{
                    DeliveryDate = new DateTime(2022,10,28),
                    IsGreenDelivery = true,
                    PostalCode = "15172"
                },
                new DeliveryDateDetail
                {
                    DeliveryDate = new DateTime(2022,10,23),
                    IsGreenDelivery = false,
                    PostalCode = "15172"
                }
            };
            // Act
            var result = deliveryDatesCalculator.SortDeliveryDates(deliveryDates).ToList();

            // Assert
            Assert.Equal(new DateTime(2022, 10, 24), result[0].DeliveryDate);
            Assert.True(result[0].IsGreenDelivery);
            Assert.Equal(new DateTime(2022, 10, 23), result[1].DeliveryDate);
            Assert.False(result[1].IsGreenDelivery);
            Assert.Equal(new DateTime(2022, 10, 26), result[2].DeliveryDate);
            Assert.False(result[2].IsGreenDelivery);
            Assert.Equal(new DateTime(2022, 10, 28), result[3].DeliveryDate);
            Assert.True(result[3].IsGreenDelivery);
        }

        [Fact]
        public void WhenDeliveryDatesContainsGreenDeliveriesAfterNext3Days_ThenDeliveryDatesAreSortedAsPerAscDate()
        {
            // Arrange
            List<DeliveryDateDetail> deliveryDates = new List<DeliveryDateDetail>() {
                new DeliveryDateDetail
                {
                    DeliveryDate = new DateTime(2022,10,24),
                    IsGreenDelivery = false,
                    PostalCode = "15172"
                },
                new DeliveryDateDetail{
                    DeliveryDate = new DateTime(2022,10,30),
                    IsGreenDelivery = true,
                    PostalCode = "15172"
                },
                new DeliveryDateDetail{
                    DeliveryDate = new DateTime(2022,10,26),
                    IsGreenDelivery = true,
                    PostalCode = "15172"
                },
                new DeliveryDateDetail{
                    DeliveryDate = new DateTime(2022,10,28),
                    IsGreenDelivery = true,
                    PostalCode = "15172"
                },
                new DeliveryDateDetail
                {
                    DeliveryDate = new DateTime(2022,10,23),
                    IsGreenDelivery = false,
                    PostalCode = "15172"
                }
            };
            // Act
            var result = deliveryDatesCalculator.SortDeliveryDates(deliveryDates).ToList();

            // Assert
            Assert.Equal(new DateTime(2022, 10, 23), result[0].DeliveryDate);
            Assert.False(result[0].IsGreenDelivery);
            Assert.Equal(new DateTime(2022, 10, 24), result[1].DeliveryDate);
            Assert.False(result[1].IsGreenDelivery);
            Assert.Equal(new DateTime(2022, 10, 26), result[2].DeliveryDate);
            Assert.True(result[2].IsGreenDelivery);
            Assert.Equal(new DateTime(2022, 10, 28), result[3].DeliveryDate);
            Assert.True(result[3].IsGreenDelivery);
        }
    }
}