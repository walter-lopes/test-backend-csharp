using Easynvest.Infohub.Parse.Domain.Entities;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Repositories;
using Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache
{
    public class IssuerParseCacheRepository : IIssuerParseRepository
    {
        private readonly ICache _cache;
        private readonly IIssuerParseRepository _repository;
        private readonly string _key = "IssuerParse";

        public IssuerParseCacheRepository(IIssuerParseRepository repository, ICache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task Create(IssuerParse parse)
        {
            var key = GetKeyValue(parse);

            var issuerDto = new IssuerParseCacheDto(parse.IssuerNameCustodyManager, parse.IssuerNameCetip);      

            await _repository.Create(parse);

            _cache.Set(key, issuerDto);
        }

        public async Task Delete(IssuerParse parse)
        {
            var key = GetKeyValue(parse);

            _cache.DeleteByKey<IssuerParseCacheDto>(key);

            await _repository.Delete(parse);
        }

        public async Task<IReadOnlyCollection<IssuerParse>> GetAll()
        {
            var issuers = _cache.GetAll<IssuerParseCacheDto>();

            if (issuers.Any())
                return IssuerParseCacheDto.ToDomain(issuers);

            return await _repository.GetAll();
        }

        public async Task<IssuerParse> GetBy(string issuerNameCetip)
        {
            var key = GetKeyValue(issuerNameCetip);

            var issuerDto = _cache.Get<IssuerParseCacheDto>(key);

            if (issuerDto is null)
                return await _repository.GetBy(issuerNameCetip);

            return issuerDto.ToDomain();
        }

        public async Task Update(IssuerParse parse)
        {
            await _repository.Update(parse);

            var key = GetKeyValue(parse.IssuerNameCetip);

            var issuerParseDto = new IssuerParseCacheDto(parse.IssuerNameCustodyManager, parse.IssuerNameCetip);

            _cache.Set(key, issuerParseDto);
        }

        private string GetKeyValue(IssuerParse parse) => $"{_key}:{parse.IssuerNameCetip}";

        private string GetKeyValue(string issuerNameCetip) => $"{_key}:{issuerNameCetip}";
    }
}
