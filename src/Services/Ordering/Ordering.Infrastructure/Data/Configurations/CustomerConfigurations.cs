using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;
public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion(
                //Valor que se va a guardar en la BD como Id o key
                customerId => customerId.Value,
                //Manera en la que al recuperar de la BD se va a usar para convertir al modelo
                //El campo guardado se convierte con la especificacion, en este caso el valor guardado en la BD
                //se pasa al factory method que devuelve un nuevo cusomerId
                dbId => CustomerId.Of(dbId));

        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();

        builder.Property(c => c.Email).HasMaxLength(255);

        builder.HasIndex(c => c.Email).IsUnique();
    }
}
