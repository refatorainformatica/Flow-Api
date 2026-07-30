//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;
//using Shared.Domain.Abstractions.Chat;

//namespace Shared.Infrastructure.Chat
//{
//    public class ChatGptService : IChatGptService
//    {
//        private readonly IConfiguration _configuration;

//        public ChatGptService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public async Task<string> SendMessageAsync(string message)
//        {
//            var apiKey = _configuration.GetSection("OpenAI:ApiKey").Value;
//            var openAi = new OpenAIAPI(apiKey);

//            var completionRequest = new CompletionRequest()
//            {
//                Model = "text-davinci-003",
//                Prompt = message,
//                MaxTokens = 512,
//                NumChoicesPerPrompt = 1,
//            };

//            var completions = await openAi.Completions.CreateCompletionsAsync(completionRequest);

//            return Regex.Replace(completions.Completions.First().Text, @"\t|\n|\r", "");
//        }
//    }
//}
