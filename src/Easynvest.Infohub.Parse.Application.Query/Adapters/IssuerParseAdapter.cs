using Easynvest.Infohub.Parse.Application.Query.Dtos;
using Easynvest.Infohub.Parse.Domain.Entities;
using System.Collections.Generic;

namespace Easynvest.Infohub.Parse.Application.Query.Adapters
{
    internal static class IssuerParseAdapter
    {
        internal static IssuerParseDto ToDto(this IssuerParse entity)
        {
            if (entity is null)
            {
                return null;
            }

            return new IssuerParseDto
            {
                IssuerNameCetip = entity.IssuerNameCetip,
                IssuerNameCustodyManager = entity.IssuerNameCustodyManager
            };
        }

        internal static IReadOnlyCollection<IssuerParseDto> ToDto(this IEnumerable<IssuerParse> entities)
        {
            if (entities is null)
            {
                return null;
            }

            var dtos = new List<IssuerParseDto>();

            foreach (var entity in entities)
            {
                dtos.Add(entity.ToDto());
            }

            return dtos;
        }
    }
}