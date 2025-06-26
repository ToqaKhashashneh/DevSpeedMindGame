using DevMindSpeed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevMindSpeed.DTO;
using DevMindSpeed.Services.GameIDataService;

namespace DevMindSpeed.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IDataService _gameService;
        public GameController(IDataService gameService)
        {
            _gameService = gameService;
        }



        [HttpPost("start")]
        public async Task<IActionResult> StartNewGame([FromQuery] string PlayerName, [FromQuery] int difficulty)
        {
            if (string.IsNullOrEmpty(PlayerName) || difficulty < 1 || difficulty > 4)
                return BadRequest(" Player Name Is Required or Invalid Difficulty Level.");

            var (session, firstQuestion) = await _gameService.StartNewGameAsync(PlayerName, difficulty);

            return Ok(new
            {
                message = $"Hello {PlayerName}, find your submit API URL below",
                submit_url = $"{Request.Scheme}://{Request.Host}/game/submit/{session.Id}",
                question = firstQuestion.QuestionContent,
                time_started = session.StartTime
            });
        }

        [HttpPost("{gameId}/submit")]
        public async Task<IActionResult> SubmitNextQuestion(int gameId, [FromBody] SubmitAnswerRequest request)
        {
            var scheme = Request.Scheme;
            var host = Request.Host.ToString();

            var response = await _gameService.SubmitAnswerAsync(gameId, request, scheme, host);
            return Ok(response);
        }


        [HttpGet("{gameId}/end")]
        public async Task<IActionResult> EndGame(int gameId)
        {
            var result = await _gameService.EndGameAsync(gameId);
            return Ok(result);
        }







    }
}
