﻿using Microsoft.Extensions.DependencyInjection;
using Repository.CommonDependenceInject;
using Repository.Persistency.Abstractions;
using Repository.Persistency.Generic;
using Repository.Persistency.Implementations;

namespace CommonDependenceInject;
public sealed class RepositoryInjectDependenceTest
{
    [Fact]
    public void AddRepositories_Should_Register_Repositories()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddRepositories();

        // Assert
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<>) && descriptor.ImplementationType == typeof(GenericRepositorio<>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<Usuario>) && descriptor.ImplementationType == typeof(UsuarioRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IAcessoRepositorioImpl) && descriptor.ImplementationType == typeof(AcessoRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(ILancamentoRepositorio) && descriptor.ImplementationType == typeof(LancamentoRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(ISaldoRepositorio) && descriptor.ImplementationType == typeof(SaldoRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IGraficosRepositorio) && descriptor.ImplementationType == typeof(GraficosRepositorioImpl)));
    }
}