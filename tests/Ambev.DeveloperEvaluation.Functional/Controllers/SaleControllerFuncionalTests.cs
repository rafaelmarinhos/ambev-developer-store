using Microsoft.AspNetCore.Mvc.Testing;
using Ambev.DeveloperEvaluation.WebApi;
using Xunit;
using FluentAssertions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using System.Net.Http.Json;
using System.Net;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

namespace Ambev.DeveloperEvaluation.Functional.Controllers;

public class SaleControllerFuncionalTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SaleControllerFuncionalTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    /// <summary>
    /// Tests that the invalid create sale request will send and return 400 BadRequest
    /// </summary>
    [Fact(DisplayName = "Given an invalid request, When send create sale request, Then should return 400 BadRequest")]
    public async Task GivenInvalidRequest_WhenCreateSale_ThenReturn400BadRequest()
    {
        // Arrange
        var request = new CreateSaleRequest
        {
            CustomerId = Guid.Empty,
            BranchId = Guid.NewGuid(),
            Items =
            [
                new () { ProductId = Guid.NewGuid(), Quantity = 2, Price = 10 }
            ]
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/sales", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        result.Should().NotBeNull();
        result!.Success.Should().BeFalse();
        result!.Errors.Count().Should().BeGreaterThan(0);
        result.Message.Should().BeEquivalentTo("Error on create sale");
    }

    /// <summary>
    /// Tests that the invalid create sale request will send and return 400 BadRequest
    /// </summary>
    [Fact(DisplayName = "Given a valid request, When send create sale request, Then should return 201 Created")]
    public async Task GivenAValidRequest_WhenCreateSale_ThenReturn201Created()
    {
        // Arrange
        var request = new CreateSaleRequest
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new () { ProductId = Guid.NewGuid(), Quantity = 2, Price = 10 }
            ]
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/sales", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result!.Errors.Count().Should().Be(0);
        result.Message.Should().BeEquivalentTo("Sale created successfully");
    }

    /// <summary>
    /// Tests that the invalid get sale request will send and return 400 BadRequest
    /// </summary>
    [Fact(DisplayName = "Given an invalid request, When send get sale request, Then should return 400 BadRequest")]
    public async Task GivenInvalidRequest_WhenGetSale_ThenReturn400BadRequest()
    {
        // Arrange
        var request = new GetSaleRequest
        {
            Id = Guid.Empty,            
        };

        // Act
        var response = await _client.GetAsync($"api/sales/{request.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        result.Should().NotBeNull();
        result!.Success.Should().BeFalse();
        result!.Errors.Count().Should().BeGreaterThan(0);
        result.Message.Should().BeEquivalentTo("Error on retrieved sale");
    }

    /// <summary>
    /// Tests that the invalid get sale request will send and return 400 BadRequest
    /// </summary>
    [Fact(DisplayName = "Given a valid request, When send get sale request, Then should return 200 Ok")]
    public async Task GivenAValidRequest_WhenGetSale_ThenReturn200Ok()
    {
        // Arrange        
        var createSaleRequest = new CreateSaleRequest
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new () { ProductId = Guid.NewGuid(), Quantity = 2, Price = 10 }
            ]
        };
        
        var createSaleResult = await _client.PostAsJsonAsync("api/sales", createSaleRequest);
        var createSaleResponse = await createSaleResult.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResult>>();

        // Act
        var response = await _client.GetAsync($"api/sales/{createSaleResponse!.Data!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ApiResponseWithData<GetSaleResponse>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result!.Errors.Count().Should().Be(0);
        result.Message.Should().BeEquivalentTo("Sale retrieved successfully");
    }

    /// <summary>
    /// Tests that the invalid update sale request will send and return 401 Forbid
    /// </summary>
    [Fact(DisplayName = "Given an invalid request, When send update sale request, Then should return 401 Forbid")]
    public async Task GivenInvalidRequest_WhenUpdateSale_ThenReturn401Forbid()
    {
        // Arrange
        var request = new UpdateSaleRequest
        {
            Id = Guid.NewGuid(),
            Items =
            [
                new () { ProductId = Guid.NewGuid(), Quantity = 2, Price = 10 }
            ]
        };

        // Act
        var response = await _client.PutAsJsonAsync($"api/sales/{Guid.NewGuid()}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);        
    }

    /// <summary>
    /// Tests that the invalid update sale request will send and return 400 Badrequest
    /// </summary>
    [Fact(DisplayName = "Given an invalid request, When send update sale request, Then should return 400 BadRequest")]
    public async Task GivenInvalidRequest_WhenUpdateSale_ThenReturn400BadRequest()
    {
        // Arrange
        var request = new UpdateSaleRequest
        {
            Id = Guid.Empty,
            Items =
            [
                new () { ProductId = Guid.NewGuid(), Quantity = 2, Price = 10 }
            ]
        };

        // Act
        var response = await _client.PutAsJsonAsync($"api/sales/{Guid.Empty}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        result.Should().NotBeNull();
        result!.Success.Should().BeFalse();
        result!.Errors.Count().Should().BeGreaterThan(0);
        result.Message.Should().BeEquivalentTo("Error on update sale");
    }

    /// <summary>
    /// Tests that the invalid update sale request will send and return 200 Ok
    /// </summary>
    [Fact(DisplayName = "Given an valid request, When send update sale request, Then should return 200 Ok")]
    public async Task GivenInvalidRequest_WhenUpdateSale_ThenReturn200Ok()
    {
        // Arrange        
        var productId = Guid.NewGuid();

        var createSaleRequest = new CreateSaleRequest
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new () { ProductId = productId, Quantity = 2, Price = 10 }
            ]
        };

        var createSaleResult = await _client.PostAsJsonAsync("api/sales", createSaleRequest);
        var createSaleResponse = await createSaleResult.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResult>>();

        // Arrange
        var request = new UpdateSaleRequest
        {
            Id = createSaleResponse!.Data!.Id,
            Items =
            [
                new () { ProductId = productId, Quantity = 2, Price = 10 }
            ]
        };

        // Act
        var response = await _client.PutAsJsonAsync($"api/sales/{request.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ApiResponseWithData<UpdateSaleResponse>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result!.Errors.Count().Should().Be(0);
        result.Message.Should().BeEquivalentTo("Sale updated successfully");
    }

    /// <summary>
    /// Tests that the invalid cancel sale request will send and return 400 BadRequest
    /// </summary>
    [Fact(DisplayName = "Given an invalid request, When send cancel sale request, Then should return 400 BadRequest")]
    public async Task GivenInvalidRequest_WhenCancelSale_ThenReturn400BadRequest()
    {
        // Arrange
        var request = new DeleteSaleRequest
        {
            Id = Guid.Empty,
        };

        // Act
        var response = await _client.PatchAsync($"api/sales/{request.Id}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        result.Should().NotBeNull();
        result!.Success.Should().BeFalse();
        result!.Errors.Count().Should().BeGreaterThan(0);
        result.Message.Should().BeEquivalentTo("Error on cancel sale");
    }

    /// <summary>
    /// Tests that the invalid cancel sale request will send and return 400 BadRequest
    /// </summary>
    [Fact(DisplayName = "Given a valid request, When send cancel sale request, Then should return 200 Ok")]
    public async Task GivenAValidRequest_WhenCancelSale_ThenReturn200Ok()
    {        
        // Arrange        
        var productId = Guid.NewGuid();

        var createSaleRequest = new CreateSaleRequest
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items =
            [
                new () { ProductId = productId, Quantity = 2, Price = 10 }
            ]
        };

        var createSaleResult = await _client.PostAsJsonAsync("api/sales", createSaleRequest);
        var createSaleResponse = await createSaleResult.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResult>>();

        // Act
        var response = await _client.PatchAsync($"api/sales/{createSaleResponse!.Data!.Id}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result!.Errors.Count().Should().Be(0);
        result.Message.Should().BeEquivalentTo("Sale canceled successfully");
    }
}
