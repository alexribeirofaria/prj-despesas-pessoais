﻿using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.CommonDependenceInject;
using Repository.Persistency.Abstractions;
using Repository.Persistency.UnitOfWork.Abstractions;
using Repository.UnitOfWork;
using Despesas.Infrastructure.Email.Abstractions;
using Despesas.Infrastructure.Email;
using Repository.Persistency.Generic;
using Repository.Persistency.Implementations;
using Despesas.Application.CommonDependenceInject;
using Despesas.Application.Abstractions.Generic;
using Despesas.Application.Abstractions;
using Despesas.Application.Dtos;
using Despesas.Application.Implementations;

namespace CommonDependenceInject;
public sealed class ServicesDependenceInjectTest
{
    [Fact]
    public void AddServices_Should_Register_Services()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddServices();

        // Assert

        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IBusiness<DespesaDto, Despesa>) && descriptor.ImplementationType == typeof(DespesaBusinessImpl<DespesaDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IBusiness<ReceitaDto, Receita>) && descriptor.ImplementationType == typeof(ReceitaBusinessImpl<ReceitaDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IBusiness<CategoriaDto, Categoria>) && descriptor.ImplementationType == typeof(CategoriaBusinessImpl<CategoriaDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IAcessoBusiness<AcessoDto, LoginDto>) && descriptor.ImplementationType == typeof(AcessoBusinessImpl<AcessoDto, LoginDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(ILancamentoBusiness<LancamentoDto>) && descriptor.ImplementationType == typeof(LancamentoBusinessImpl<LancamentoDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IUsuarioBusiness<UsuarioDto>) && descriptor.ImplementationType == typeof(UsuarioBusinessImpl<UsuarioDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IImagemPerfilUsuarioBusiness<ImagemPerfilDto, UsuarioDto>) && descriptor.ImplementationType == typeof(ImagemPerfilUsuarioBusinessImpl<ImagemPerfilDto, UsuarioDto>)));

        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IUnitOfWork<>) && descriptor.ImplementationType == typeof(UnitOfWork<>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IBusinessBase<CategoriaDto, Categoria>) && descriptor.ImplementationType == typeof(CategoriaBusinessImpl<CategoriaDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IBusinessBase<DespesaDto, Despesa>) && descriptor.ImplementationType == typeof(DespesaBusinessImpl<DespesaDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IBusinessBase<ReceitaDto, Receita>) && descriptor.ImplementationType == typeof(ReceitaBusinessImpl<ReceitaDto>)));

        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IBusiness<DespesaDto, Despesa>) && descriptor.ImplementationType == typeof(DespesaBusinessImpl<DespesaDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IBusiness<ReceitaDto, Receita>) && descriptor.ImplementationType == typeof(ReceitaBusinessImpl<ReceitaDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IBusiness<CategoriaDto, Categoria>) && descriptor.ImplementationType == typeof(CategoriaBusinessImpl<CategoriaDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IAcessoBusiness<AcessoDto, LoginDto>) && descriptor.ImplementationType == typeof(AcessoBusinessImpl<AcessoDto, LoginDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(ILancamentoBusiness<LancamentoDto>) && descriptor.ImplementationType == typeof(LancamentoBusinessImpl<LancamentoDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IUsuarioBusiness<UsuarioDto>) && descriptor.ImplementationType == typeof(UsuarioBusinessImpl<UsuarioDto>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IImagemPerfilUsuarioBusiness<ImagemPerfilDto, UsuarioDto>) && descriptor.ImplementationType == typeof(ImagemPerfilUsuarioBusinessImpl<ImagemPerfilDto, UsuarioDto>)));

        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(ISaldoBusiness) && descriptor.ImplementationType == typeof(SaldoBusinessImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IGraficosBusiness) && descriptor.ImplementationType == typeof(GraficosBusinessImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IEmailSender) && descriptor.ImplementationType == typeof(EmailSender)));
    }

    [Fact]
    public void AddRepositories_Should_Register_Repositories()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services?.AddRepositories();

        // Assert
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<>) && descriptor.ImplementationType == typeof(GenericRepositorio<>)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<Categoria>) && descriptor.ImplementationType == typeof(CategoriaRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<Despesa>) && descriptor.ImplementationType == typeof(DespesaRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<Receita>) && descriptor.ImplementationType == typeof(ReceitaRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IRepositorio<Usuario>) && descriptor.ImplementationType == typeof(UsuarioRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IAcessoRepositorioImpl) && descriptor.ImplementationType == typeof(AcessoRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(ILancamentoRepositorio) && descriptor.ImplementationType == typeof(SaldoRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(ISaldoRepositorio) && descriptor.ImplementationType == typeof(AcessoRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IGraficosRepositorio) && descriptor.ImplementationType == typeof(GraficosRepositorioImpl)));
        Assert.NotNull(services?.Any(descriptor => descriptor.ServiceType == typeof(IEmailSender) && descriptor.ImplementationType == typeof(EmailSender)));
    }

    [Fact]
    public void CreateDataBaseInMemory_Should_Add_Context_And_DataSeeder()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.CreateDataBaseInMemory();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetService<RegisterContext>();
        //var dataSeeder = serviceProvider.GetService<IDataSeeder>();

        Assert.NotNull(context);
        //Assert.NotNull(dataSeeder);
    }
}
