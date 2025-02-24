# DeveloperStore-API

## About

The `DeveloperStore-API` is a prototype for managing sales within the **Sales** domain. Built using .NET Core 8 and PostgreSQL, it provides a complete CRUD for sales records, including details like below: 

-   Sale number
-   Date when the sale was made
-   Customer
-   Total sale amount
-   Branch where the sale was made
-   Products
-   Quantities
-   Unit prices
-   Discounts
-   Total amount for each item
-   Cancelled/Not Cancelled

## Architecture Overview

The application consists of a WebApi that contains operations for the following domains: Users, Auth and Sales. The project structure is shown below:

![2](https://github.com/user-attachments/assets/811456e8-1272-40d4-a70d-902637b895be)


### Project architecture implements the most important and used concerns as:

 - DDD - Domain Driven Design (Layers and Domain Model Pattern)
 - Clean Code
 - Domain Events
 - Domain Validations
 - CQRS
 - Repository
 - Tests (Unit tests, Integration tests and Functional tests)

## Technologies / Components implemented

 - Components / Services:
 
	 -   .NET Core 8
	 -   PostgreSQL
	 -   Docker  (Containerization)
	 -   Serilog  (Logging)
	 -   Husky  (Semantic commits)
	 -   MediatR  (CQRS pattern)
	 -   FluentResults (https://github.com/altmann/FluentResults)
	 -   RESTful APIs
	 -   Swagger UI with JWT support
	 
 - Testing
 
	 -   XUnit
	 -   FluentAssertions
	 -   Bogus  (Test data generation)
	 -   NSubstitute  (Mocking)
	 -   TestContainers  (Integration tests with containerized dependencies)
	 
##  Domain Events

Domain events have been created for the following scenarios:

     - SaleCreated
     - SaleModified
     - SaleCancelled
     - ItemCancelled

![3](https://github.com/user-attachments/assets/96e3b637-dd70-4b04-a008-1cd42b36e98a)


For each captured event, only the log is being performed. From here, you can use Rebus to send messages to the MessageBroker in order to implement integration events as shown below:

```csharp
public class SaleCreatedDomainEventHandler : INotificationHandler<SaleCreatedDomainEvent>
{
    private readonly ILogger<SaleCreatedDomainEventHandler> _logger;

    public SaleCreatedDomainEventHandler(ILogger<SaleCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Here you can use Rebus or MassTransit to send integration events to a MessageBroker

        _logger.LogInformation(
            "{Handler} | Sale created | Sale: {SaleId}",
            nameof(SaleCreatedDomainEventHandler),
            notification.SaleId
        );

        await Task.CompletedTask;
    }
}
```

## Getting Started

You can run the DeveloperStore-API project in your environment using Docker. Make sure you have installed docker in your environment. (Get Docker Installation)

Clone DeveloperStore-API repository and navigate to the ```/src/Ambev.DeveloperEvaluation.Application``` folder and then:

    docker-compose up --build

## Testing

You can test the DeveloperStore-API project in two ways, via Swagger or importing the postman collection with the calls already ready.

- **Swagger**

1. Create a user through the `POST: api/users` endpoint;
2. Authenticate the newly created user through the `POST: api/auth/login` endpoint to obtain the `JWT` token;
3. Provide the `JWT` token in the **Authorize** field as shown in the image below, after which you will be able to make calls to the **Sales** endpoints;

![1](https://github.com/user-attachments/assets/bc82c49d-f753-4324-84c6-16a71e9a3188)

- **Postman**

1. You can import the postman collection below with the calls already ready with all the necessary payload, you will only need to carry out the authentication process and insertion of the token as described in the explanation session via swagger.

Postman collection: [DeveloperStore-API.postman_collection.json](https://github.com/user-attachments/files/18940189/DeveloperStore-API.postman_collection.json)

## TO-DO

Based on the overall project requirements, some items that could have been implemented and improvements and development options:

- Improve swagger documentation
- Implement Sales.GetAll() method with query parameters (_page, _size, _order)
- Send integration events to a message broker using Rebus/MassTransit
- Use a separate database or at least a separate way to query the database records (use Dapper for query for example)




