using Easynvest.Infohub.Parse.Application.Query.Dtos;
using Easynvest.Infohub.Parse.Domain.Entities;
using System.Collections.Generic;

namespace Easynvest.Infohub.Parse.Application.Query.Adapters
{
    internal static class BondParseAdapter
    {
        internal static BondParseDto ToDto(this BondParse entity)
        {
            if(entity is null)
            {
                return null;
            }

            return new BondParseDto
            {
                BondIndex = entity.BondIndex,
                BondType = entity.BondType,
                IdCustodyManagerBond = entity.IdCustodyManagerBond,
                IsAntecipatedSell = entity.IsAntecipatedSell
            };
        }

        internal static IReadOnlyCollection<BondParseDto> ToDto(this IEnumerable<BondParse> entities)
        {
            if (entities is null)
            {
                return null;
            }

            var dtos = new List<BondParseDto>();

            foreach(var entity in entities)
            {
                dtos.Add(entity.ToDto());
            }

            return dtos;
        }
    }
}
