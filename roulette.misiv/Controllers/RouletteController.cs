using Microsoft.AspNetCore.Mvc;
using RouletteMisiv.Infrastructure.Services;
using RouletteMisiv.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RouletteMisiv.Infrastructure.Exceptions.CustomException;

namespace roulette.misiv.Controllers
{
    [Route("misiv/roulette")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        IRouletteService _rouletteService;
        public RouletteController(IRouletteService rouletteService)
        {
            _rouletteService = rouletteService ?? throw new ArgumentNullException(nameof(rouletteService));
        }

        [HttpPost]
        [Route("new-rulette")]
        public async Task<IActionResult> NewRulette()
        {
            Roulette roulette = await _rouletteService.create();
            return Ok(new RouletteResponse() { Id = roulette.Id });
        }

        [HttpPut("open/{id}")]
        public async Task<IActionResult> Open([FromRoute(Name = "id")] string id)
        {
            try
            {
                await _rouletteService.Open(id);
                return Ok(new ServiceResponse() { Code = 0, Message = "Exitosa" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(new ServiceResponse() { Code = 400, Message = e.Message });
            }
        }

        [HttpPost("bet")]
        public async Task<IActionResult> Bet([FromBody] Bet request, [FromHeader(Name = "user-id")] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResponse
                {
                    Code = 0,
                    Message = "Debe ingresar los parámtros obligatorios."
                });
            }
            try
            {
                return Ok(await _rouletteService.Bet(request, userId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(new ServiceResponse() { Code = 1, Message = e.Message });
            }

        }

        [HttpPut("close/{id}")]
        public async Task<IActionResult> Close([FromRoute(Name = "id")] string id)
        {
            try
            {
                Roulette roulette = await _rouletteService.Close(id);
                return Ok(roulette);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(new ServiceResponse() { Code = 1, Message = e.Message });
            }
        }

        [HttpGet]
        [Route("geta-all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _rouletteService.GetAll());
        }
    }
}
