﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Repository.Mapping;
public sealed class CategoriaMapTest
{
    [Fact]
    public void EntityConfiguration_IsValid()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<RegisterContext>().UseInMemoryDatabase(databaseName: "CategoriaMapTest").Options;

        using (var context = new RegisterContext(options, Usings.GetLogerFactory()))
        {
            var builder = new ModelBuilder(new ConventionSet());
            var configuration = new CategoriaMap();

            configuration.Configure(builder.Entity<Categoria>());

            var model = builder.Model;
            var entityType = model.FindEntityType(typeof(Categoria));

            // Act

            var idProperty = entityType?.FindProperty("Id");
            var descricaoProperty = entityType?.FindProperty("Descricao");
            var usuarioIdProperty = entityType?.FindProperty("UsuarioId");
            var tipoCategoriaProperty = entityType?.FindNavigation("TipoCategoria");

            // Assert
            Assert.NotNull(idProperty);
            Assert.NotNull(descricaoProperty);
            Assert.NotNull(usuarioIdProperty);
            Assert.NotNull(tipoCategoriaProperty);

            Assert.True(idProperty.IsPrimaryKey());
            Assert.False(usuarioIdProperty.IsNullable);
            Assert.True(descricaoProperty.IsNullable);
            Assert.Equal(100, descricaoProperty.GetMaxLength());
        }
    }
}
