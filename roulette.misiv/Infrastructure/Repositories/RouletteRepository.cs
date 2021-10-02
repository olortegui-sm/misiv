using EasyCaching.Core;
using RouletteMisiv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Infrastructure.Repositories
{
    public class RouletteRepository : IRouletteRepository
    {
        private IEasyCachingProvider _cachingProvider;
        private const string REDIS_KEY = "ROULETE_KEY";
        public RouletteRepository(IEasyCachingProvider provider)
        {
            _cachingProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
        public async Task<List<Roulette>> GetAll()
        {
            var rouletes = await _cachingProvider.GetByPrefixAsync<Roulette>(REDIS_KEY);
            if (rouletes.Values.Count == 0)
            {
                return new List<Roulette>();
            }

            return new List<Roulette>(rouletes.Select(x => x.Value.Value));
        }
        public async Task<Roulette> GetById(string Id)
        {
            var item = await _cachingProvider.GetAsync<Roulette>($"{REDIS_KEY}{Id}");
            if (!item.HasValue)
            {
                return null;
            }

            return item.Value;
        }
        public async Task<Roulette> Save(Roulette roulette)
        {
            await _cachingProvider.SetAsync($"{REDIS_KEY}{roulette.Id}", roulette, TimeSpan.FromDays(10));

            return roulette;
        }
        public async Task<Roulette> Update(string Id, Roulette roulette)
        {
            roulette.Id = Id;

            return  await Save(roulette);
        }
    }
}
