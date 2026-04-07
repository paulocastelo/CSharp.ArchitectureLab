using Moq;
using UnitTesting.Sample.Application;
using UnitTesting.Sample.Application.Contracts;
using UnitTesting.Sample.Domain;
using Xunit;

namespace UnitTesting.Sample.Tests;

public sealed class StockMovementServiceTests
{
    [Fact]
    public async Task CreateEntry_IncreasesBalance()
    {
        // Arrange
        var product = new Product { Id = Guid.NewGuid(), CurrentBalance = 10 };
        var repository = CreateRepository(product);
        var service = new StockMovementService(repository.Object);

        // Act
        var balance = await service.CreateAsync(new CreateMovementRequest
        {
            ProductId = product.Id,
            Type = StockMovementType.Entry,
            Quantity = 5
        });

        // Assert
        Assert.Equal(15, balance);
    }

    [Fact]
    public async Task CreateExit_DecreasesBalance()
    {
        // Arrange
        var product = new Product { Id = Guid.NewGuid(), CurrentBalance = 10 };
        var repository = CreateRepository(product);
        var service = new StockMovementService(repository.Object);

        // Act
        var balance = await service.CreateAsync(new CreateMovementRequest
        {
            ProductId = product.Id,
            Type = StockMovementType.Exit,
            Quantity = 3
        });

        // Assert
        Assert.Equal(7, balance);
    }

    [Fact]
    public async Task CreateExit_WithInsufficientBalance_ThrowsException()
    {
        // Arrange
        var product = new Product { Id = Guid.NewGuid(), CurrentBalance = 2 };
        var repository = CreateRepository(product);
        var service = new StockMovementService(repository.Object);

        // Act / Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new CreateMovementRequest
        {
            ProductId = product.Id,
            Type = StockMovementType.Exit,
            Quantity = 5
        }));
    }

    [Fact]
    public async Task CreateExit_WithExactBalance_Succeeds()
    {
        // Arrange
        var product = new Product { Id = Guid.NewGuid(), CurrentBalance = 5 };
        var repository = CreateRepository(product);
        var service = new StockMovementService(repository.Object);

        // Act
        var balance = await service.CreateAsync(new CreateMovementRequest
        {
            ProductId = product.Id,
            Type = StockMovementType.Exit,
            Quantity = 5
        });

        // Assert
        Assert.Equal(0, balance);
    }

    [Fact]
    public async Task Create_WithInvalidQuantity_ThrowsException()
    {
        // Arrange
        var product = new Product { Id = Guid.NewGuid(), CurrentBalance = 5 };
        var repository = CreateRepository(product);
        var service = new StockMovementService(repository.Object);

        // Act / Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CreateAsync(new CreateMovementRequest
        {
            ProductId = product.Id,
            Type = StockMovementType.Entry,
            Quantity = 0
        }));
    }

    [Fact]
    public async Task Create_WithUnknownProduct_ThrowsNotFoundException()
    {
        // Arrange
        var repository = CreateRepository(null);
        var service = new StockMovementService(repository.Object);

        // Act / Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.CreateAsync(new CreateMovementRequest
        {
            ProductId = Guid.NewGuid(),
            Type = StockMovementType.Entry,
            Quantity = 1
        }));
    }

    private static Mock<IProductRepository> CreateRepository(Product? product)
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        repository.Setup(x => x.SaveAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        repository.Setup(x => x.AddMovementAsync(It.IsAny<StockMovement>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return repository;
    }
}
