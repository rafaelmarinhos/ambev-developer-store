using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating sales test data using the Bogus library.
/// This class centralizes all sales test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateSaleHandlerTestData
{
    public static CreateSaleResult GenerateCreateSaleResult()
    {
        return new CreateSaleResult()
        {
            Id = Guid.NewGuid(),
            Number = 101010
        };
    }
}
