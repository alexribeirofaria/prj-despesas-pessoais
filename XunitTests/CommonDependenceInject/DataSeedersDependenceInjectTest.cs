﻿using Microsoft.Extensions.DependencyInjection;
using Despesas.DataSeeders.CommonDependenceInject;
using DataSeeders;

namespace CommonDependenceInject;

public sealed class DataSeedersDependenceInjectTests
{
    [Fact]
    public void RunDataSeeders_Should_Invoke_SeedData_On_IDataSeeder()
    {
        // Arrange
        var services = new ServiceCollection();                
        var dataSeederMock = new Mock<IDataSeeder>();                
        services.AddTransient(_ => dataSeederMock.Object);               
        var serviceProvider = services.BuildServiceProvider();

        // Act
        DataSeedersDependenceInject.RunDataSeeders(serviceProvider);

        // Assert
        dataSeederMock.Verify(ds => ds.SeedData(), Times.Once);
    }
}
