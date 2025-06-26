using DevMindSpeed.DTO;
using DevMindSpeed.Models;

namespace DevMindSpeed.Services.GameIDataService
{
    public interface IDataService
    {

        Task<(Session session, Question firstQuestion)> StartNewGameAsync(string playerName, int difficulty);
        Task<SubmitAnswerResponse> SubmitAnswerAsync(int gameId, SubmitAnswerRequest request, string scheme, string host);
        Task<EndGameResponse> EndGameAsync(int gameId);



    }
}
