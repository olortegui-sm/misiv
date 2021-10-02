using RouletteMisiv.Infrastructure.Exceptions;
using RouletteMisiv.Infrastructure.Repositories;
using RouletteMisiv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RouletteMisiv.Infrastructure.Exceptions.CustomException;

namespace RouletteMisiv.Infrastructure.Services
{
    public class RouletteService : IRouletteService
    {
        IRouletteRepository _rouletteRepository;
        public RouletteService(IRouletteRepository rouletteRepository)
        {
            _rouletteRepository = rouletteRepository ?? throw new ArgumentNullException(nameof(rouletteRepository));
        }

        public async Task<Roulette> create()
        {
            Roulette roulette = new Roulette()
            {
                Id = Guid.NewGuid().ToString(),
                IsOpen = false,
                Open = null,
                Close = null
            };
            await _rouletteRepository.Save(roulette);

            return roulette;
        }

        public async Task<Roulette> Find(string Id)
        {
            return await _rouletteRepository.GetById(Id);
        }

        public async Task<Roulette> Open(string Id)
        {
            Roulette roulette = await _rouletteRepository.GetById(Id);
            if (roulette == null)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "No se encontró la ruleta."
                });
            }
            if (roulette.Open != null)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "Ruleta ya está aperturada."
                });
            }
            roulette.Open = DateTime.UtcNow;
            roulette.IsOpen = true;

            return await _rouletteRepository.Update(Id, roulette);
        }

        public async Task<Roulette> Close(string Id)
        {
            Roulette roulette = await _rouletteRepository.GetById(Id);
            var validateRoulette = await validateCloseRoulette(roulette);
            if (!string.IsNullOrEmpty(validateRoulette))
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = validateRoulette
                });
            }
            Random randon = new Random();
            var winner = randon.Next(0, 36);
            foreach(var item in roulette.numberBets)
            {
                if (item.Number == winner)
                {
                    item.IsWinningNumber = true;
                    foreach (var bet in item.betDetails)
                    {
                        if (bet.IsColor)
                            bet.TotalWon = bet.AmountBet * Constants.Constants.BET_COLOR;
                        else
                            bet.TotalWon = bet.AmountBet * Constants.Constants.BET_NUMBER;
                    }
                }
            }
            roulette.Close = DateTime.UtcNow;
            roulette.IsOpen = false;

            return await _rouletteRepository.Update(Id, roulette);
        }

        public async Task<Roulette> Bet(Bet request, string UserId)
        {
            if (request.money > Constants.Constants.MAXIMUM_AMOUNT || request.money < Constants.Constants.MINIMUM_AMOUNT)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "Debe ingresar un monto permitido"
                });
            }
            Roulette roulette = await _rouletteRepository.GetById(request.RouletteId);
            if (roulette == null)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "No se encontró la ruleta."
                });
            }
            if (roulette.IsOpen == false)
            {
                throw new CustomException(400, new ServiceResponse
                {
                    Code = 0,
                    Message = "La ruleta está cerrada."
                });
            }
            foreach (var item in roulette.numberBets)
            {
                if (item.Number == request.position)
                {
                    BetDetail betDetail = new BetDetail();
                    betDetail.Player = UserId;
                    betDetail.AmountBet = request.money;
                    betDetail.IsColor = request.IsColor;
                    item.betDetails.Add(betDetail);
                }
            }

            return await _rouletteRepository.Update(roulette.Id, roulette);
        }

        public async Task<List<Roulette>> GetAll()
        {
            return  await _rouletteRepository.GetAll();
        }

        private async Task<string> validateCloseRoulette(Roulette roulette)
        {
            await Task.FromResult(0);
            string messge = string.Empty;
            if (roulette == null)
            {
                messge = "No se encontró la ruleta.";
            }
            if (roulette.Close != null)
            {
                messge = "Ruleta ya está cerrada.";
            }
            if (!roulette.IsOpen)
            {
                messge = "Ruleta no está abierta.";
            }
            return messge;
        }
        private async Task<string> validateBet(Roulette roulette)
        {
            await Task.FromResult(0);
            string messge = string.Empty;
            if (roulette == null)
            {
                messge = "No se encontró la ruleta.";
            }
            if (roulette.Close != null)
            {
                messge = "Ruleta ya está cerrada.";
            }
            if (!roulette.IsOpen)
            {
                messge = "Ruleta no está abierta.";
            }
            return messge;
        }
    }
}
