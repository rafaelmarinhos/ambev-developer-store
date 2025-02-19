using Ambev.DeveloperEvaluation.Application.Sales;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;
using Microsoft.AspNetCore.Http;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

/// <summary>
/// Provides methods for generating sales test data using the Bogus library.
/// This class centralizes all sales test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleHandlerTestData
{
    public static Guid Product001 = new Guid("B064E397-9248-4029-9AB5-8036AC3C5178");
    public static Guid Product002 = new Guid("22CB3A1E-730B-424C-A21B-77ABF28F28E8");
    public static Guid Product003 = new Guid("5E24BA2D-AA63-4AE5-AD23-FF04366BB5AE");

    public static CreateSaleCommand GenerateCreateSaleCommand()
    {
        var faker = new Faker();

        return new CreateSaleCommand()
        {
            CustomerId = faker.Random.Guid(),
            BranchId = faker.Random.Guid()
        };
    }

    public static UpdateSaleCommand GenerateUpdateSaleCommand(Guid id)
    {
        var faker = new Faker();

        return new UpdateSaleCommand()
        {
            Id = id
        };
    }    
}
