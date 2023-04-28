using General.Domain.Types;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared.Domain.Base;

namespace General.Infrastructure.DomainConfigurations.Converters;

public class ReactionTypeConverter : ValueConverter<ReactionType, int>
{
    public ReactionTypeConverter() : base(v => v.Id, 
        v => Enumeration.GetById<ReactionType>(v))
    {
    }
}