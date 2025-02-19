using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using FluentAssertions;

using Xunit;

/// <summary>
/// Contains unit tests for the User entity class.
/// Tests cover status changes and validation scenarios.
/// </summary>
namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    /// <summary>
    /// Tests that when create a new sale all fields are correct
    /// </summary>
    [Fact(DisplayName = "Id should be not null when create a new sale")]
    public void Given_CreateNewSale_When_Return_Then_IdHasValue()
    {
        // Arrange
        var sale = new Sale();

        // Assert
        sale.Id.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that when create a new sale and add a item
    /// </summary>
    [Fact(DisplayName = "Id should be not null when create a new sale")]
    public void Given_CreateSale_When_AddItem_Then_TotalAmountHasValue()
    {
        var faker = new Faker();

        // Arrange
        var sale = new Sale();

        // Act        
        sale.AddItem(faker.Random.Guid(), 1, 10);

        // Assert
        sale.TotalAmount.Should().BeGreaterThan(0);
    }

    /// <summary>
    /// Tests that when create a new sale and add a item total amount is correct
    /// </summary>
    [Fact(DisplayName = "Total amount should calculate correctly")]
    public void Given_CreateSale_When_AddItem_Then_TotalAmountShouldBe_50()
    {
        var faker = new Faker();

        // Arrange
        var sale = new Sale();

        // Act        
        sale.AddItem(faker.Random.Guid(), 1, 10);
        sale.AddItem(faker.Random.Guid(), 2, 20);

        // Assert
        sale.TotalAmount.Should().Be(50);
    }

    /// <summary>
    /// Tests that when create a new sale and add a item total amount is correct after cancel item
    /// </summary>
    [Fact(DisplayName = "Total amount should calculate correctly after cancel item")]
    public void Given_CreateSale_When_AddItem_Then_TotalAmountShouldBe_40_AfterCancelItem()
    {
        var faker = new Faker();

        // Arrange
        var sale = new Sale();

        var productId_01 = new SaleItem(faker.Random.Guid(), 1, 10);
        var productId_02 = new SaleItem(faker.Random.Guid(), 2, 20);

        // Act        
        sale.AddItem(productId_01);
        sale.AddItem(productId_02);

        // Assert
        sale.TotalAmount.Should().Be(50);

        // Cancel item
        sale.CancelItem(productId_01.ProductId);

        // Assert
        sale.TotalAmount.Should().Be(40);
    }

    /// <summary>
    /// Tests that when create a new sale and add a item total amount is correct after update item
    /// </summary>
    [Fact(DisplayName = "Total amount should calculate correctly after cancel item")]
    public void Given_CreateSale_When_AddItem_Then_TotalAmountShouldBe_70_AfterUpdateItem()
    {
        var faker = new Faker();

        // Arrange
        var sale = new Sale();

        var productId_01 = new SaleItem(faker.Random.Guid(), 1, 10);
        var productId_02 = new SaleItem(faker.Random.Guid(), 2, 20);

        // Act        
        sale.AddItem(productId_01);
        sale.AddItem(productId_02);

        // Assert
        sale.TotalAmount.Should().Be(50);

        // UpdateItem item
        sale.UpdateItem(productId_01.ProductId, 2, 15);

        // Assert
        sale.TotalAmount.Should().Be(70);
    }

    /// <summary>
    /// Tests that when create a new sale and add a two items calculate discounts correctely
    /// </summary>
    [Fact(DisplayName = "Discounts should calculate correctly when add items")]
    public void Given_CreateSale_When_AddItems_Then_CalculeDiscountsCorrectly()
    {
        var faker = new Faker();

        // Arrange
        var sale = new Sale();

        // items < 4 = no discounts
        // = $ 30,00
        var productId_01 = new SaleItem(faker.Random.Guid(), 3, 10);

        // 4 <= items >= 10 = 10%
        // = (10 * 10) - 10% = $ 90,00
        var productId_02 = new SaleItem(faker.Random.Guid(), 9, 10);

        // items <= 20
        // = (15 * 10) - 20% = $ 120,00
        var productId_03 = new SaleItem(faker.Random.Guid(), 15, 10);

        // Act        
        sale.AddItem(productId_01);
        sale.AddItem(productId_02);
        sale.AddItem(productId_03);

        // Assert
        sale.TotalAmount.Should().Be(270);
        sale.Items.First(f => f.ProductId == productId_01.ProductId).TotalAmount.Should().Be(30);
        sale.Items.First(f => f.ProductId == productId_02.ProductId).TotalAmount.Should().Be(90);
        sale.Items.First(f => f.ProductId == productId_03.ProductId).TotalAmount.Should().Be(150);

        sale.Discount.Should().Be(39);
        sale.Items.First(f => f.ProductId == productId_01.ProductId).Discount.Should().Be(0);
        sale.Items.First(f => f.ProductId == productId_02.ProductId).Discount.Should().Be(9);
        sale.Items.First(f => f.ProductId == productId_03.ProductId).Discount.Should().Be(30);
    }
}
