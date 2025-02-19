using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating sales test data using the Bogus library.
/// This class centralizes all sales test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleHandlerTestData
{
    public static CreateSaleResult GenerateCreateSaleResult()
    {
        var faker = new Faker();

        return new CreateSaleResult()
        {
            Id = faker.Random.Guid(),
            Number = 101010
        };
    }

    public static Sale GenerateSale()
    {
        var faker = new Faker();

        return new Sale()
        {
            Id = faker.Random.Guid(),
            Number = faker.Random.Long(),
        };
    }

    public static Sale GenerateSaleWithItems()
    {
        var faker = new Faker();

        var sale = new Sale()
        {
            Id = faker.Random.Guid(),
            CustomerId = faker.Random.Guid(),
            BranchId = faker.Random.Guid(),
            Number = faker.Random.Long()
        };

        sale.AddItem(faker.Random.Guid(), 10, 100);
        return sale;
    }
}
