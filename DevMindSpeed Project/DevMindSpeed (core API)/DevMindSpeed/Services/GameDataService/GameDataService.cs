using Azure.Core;
using DevMindSpeed.DTO;
using DevMindSpeed.Models;
using DevMindSpeed.Services.GameIDataService;
using Microsoft.EntityFrameworkCore;

namespace DevMindSpeed.Services.GameDataService
{
    public class GameDataService : IDataService
    {

        private readonly MyDbContext _context;
        private readonly Random _rand = new();

        public GameDataService(MyDbContext context)
        {
            _context = context;
        }



        public async Task<(Session session, Question firstQuestion)> StartNewGameAsync(string playerName, int difficulty)
        {
            var session = new Session
            {
                Name = playerName,
                Difficulty = difficulty,
                StartTime = DateTime.UtcNow
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            var questions = new List<Question>();
            for (int i = 0; i < 5; i++)
            {
                var (text, answer) = GenerateQuestion(difficulty);
                questions.Add(new Question
                {
                    SessionId = session.Id,
                    QuestionContent = text,
                    CorrectAnswer = answer,
                    TimeStarted = DateTime.UtcNow
                });
            }

            _context.Questions.AddRange(questions);
            await _context.SaveChangesAsync();

            return (session, questions.First());
        }

       public async Task<SubmitAnswerResponse> SubmitAnswerAsync(int gameId, SubmitAnswerRequest request, string scheme, string host)
        {
            var session = await _context.Sessions
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.Id == gameId);

            if (session == null)
                return new SubmitAnswerResponse { Message = "Game session not found." };

            if (session.EndTime != null)
                return new SubmitAnswerResponse { Message = "Game session has already ended." };

            var currentQuestion = session.Questions.FirstOrDefault(q => q.SubmittedAnswer == null);

            if (currentQuestion == null)
            {
                session.EndTime = DateTime.UtcNow;
                session.TotalTimeSpentSeconds = (session.EndTime - session.StartTime)?.TotalSeconds;
                await _context.SaveChangesAsync();

                int totalCorrect = session.Questions.Count(q => q.Score == 1);

                return new SubmitAnswerResponse
                {
                    Message = "Game over!",

                };
            }

            currentQuestion.SubmittedAnswer = request.SubmittedAnswer;
            currentQuestion.TimeSubmitted = DateTime.UtcNow;
            currentQuestion.TimeTakenSeconds = (currentQuestion.TimeSubmitted - currentQuestion.TimeStarted)?.TotalSeconds;
            currentQuestion.Score = Math.Abs(currentQuestion.SubmittedAnswer.Value - currentQuestion.CorrectAnswer) < 0.0001 ? 1.0f : 0.0f;

            await _context.SaveChangesAsync();
            var nextQuestion = session.Questions.FirstOrDefault(q => q.SubmittedAnswer == null);

            int attempted = session.Questions.Count(q => q.SubmittedAnswer != null);
            int correct = session.Questions.Count(q => q.Score == 1);
            if (nextQuestion == null)
            {
                session.EndTime = DateTime.UtcNow;
                session.TotalTimeSpentSeconds = (session.EndTime - session.StartTime)?.TotalSeconds;
                await _context.SaveChangesAsync();

                return new SubmitAnswerResponse
                {
                   
                    Message = "Game over!",
          
                };
            }

            return new SubmitAnswerResponse
            {
                Result = currentQuestion.Score == 1
                    ? $"Good job {session.Name}, your answer is correct!"
                    : $"Sorry {session.Name}, your answer is incorrect.",
                TimeTaken = currentQuestion.TimeTakenSeconds,
                NextQuestion = new NextQuestionDto
                {
                    SubmitUrl = $"{scheme}://{host}/game/submit/{gameId}",
                    Question = nextQuestion.QuestionContent
                },
                CurrentScore = $"{correct} / {attempted}"
            };

      


        }


        public async Task<EndGameResponse> EndGameAsync(int gameId)
        {
            var session = await _context.Sessions
                .Include(s => s.Questions)
                .FirstOrDefaultAsync(s => s.Id == gameId);

            if (session == null) return new EndGameResponse { Message = "Game not found." };

            if (session.EndTime == null)
            {
                session.EndTime = DateTime.UtcNow;
                session.TotalTimeSpentSeconds =
                    (session.EndTime - session.StartTime)?.TotalSeconds;

                await _context.SaveChangesAsync();
            }

            var totalQuestions = session.Questions.Count;
            var correctAnswers = session.Questions.Count(q => q.Score == 1);

            var bestQuestion = session.Questions
                .Where(q => q.Score == 1 && q.TimeTakenSeconds != null)
                .OrderBy(q => q.TimeTakenSeconds)
                .FirstOrDefault();

            var bestScore = bestQuestion != null
                ? new BestScoreDto
                {
                    Question = bestQuestion.QuestionContent,
                    Answer = bestQuestion.CorrectAnswer,
                    TimeTaken = bestQuestion.TimeTakenSeconds
                }
                : null;

            var history = session.Questions.Select(q => new QuestionHistoryDto
            {
                Question = q.QuestionContent,
                Submitted = q.SubmittedAnswer,
                Correct = q.CorrectAnswer,
                TimeTaken = q.TimeTakenSeconds
            }).ToList();

            return new EndGameResponse
            {
                Name = session.Name,
                Difficulty = session.Difficulty,
                CurrentScore = $"{correctAnswers} / {totalQuestions}",
                TotalTimeSpent = session.TotalTimeSpentSeconds,
                BestScore = bestScore,
                History = history
            };
        }

        private (string expression, float correctAnswer) GenerateQuestion(int difficulty)
        {
            int numberOfOperands = difficulty + 1;
            int digitLength = difficulty;

            string[] mathOperators = new[] { "+", "-", "*", "/" };
            string expression = "";

            for (int i = 0; i < numberOfOperands; i++)
            {
                int minValue = digitLength == 1 ? 0 : (int)Math.Pow(10, digitLength - 1);
                int maxValue = (int)Math.Pow(10, digitLength) - 1;

                int operand = _rand.Next(minValue, maxValue + 1);
                expression += operand.ToString();

                if (i < numberOfOperands - 1)
                {
                    string selectedOperator = mathOperators[_rand.Next(mathOperators.Length)];
                    expression += " " + selectedOperator + " ";
                }
            }

            try
            {
                var dataTable = new System.Data.DataTable();
                var result = dataTable.Compute(expression, "");
                return (expression, Convert.ToSingle(result));
            }
            catch
            {
                return ("1 + 1", 2);
            }
        }


    }
}
