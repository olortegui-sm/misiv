using RouletteMisiv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Infrastructure.Repositories
{
    public interface IRouletteRepository
    {
        Task<List<Roulette>> GetAll();
        Task<Roulette> GetById(string Id);
        Task<Roulette> Save(Roulette roulette);
        Task<Roulette> Update(string Id, Roulette roulette);
    }
}
