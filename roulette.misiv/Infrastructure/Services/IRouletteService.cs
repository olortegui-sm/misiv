using RouletteMisiv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Infrastructure.Services
{
    public interface IRouletteService
    {
        Task<Roulette> create();
        Task<Roulette> Open(string Id);
        Task<Roulette> Find(string Id);
        Task<Roulette> Close(string Id);
        Task<Roulette> Bet(Bet request, string UserId);
        Task<List<Roulette>> GetAll();
    }
}
