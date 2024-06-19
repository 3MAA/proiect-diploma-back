using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using Newtonsoft.Json;

namespace ProiectDiploma

{
    public class OpenAIService
    {
        private readonly OpenAIClient openAIClient;
        public OpenAIService(OpenAIClient openAIClient)
        {
            this.openAIClient = openAIClient;
        }
        public async Task<string> GetResponseFromOpenAIAsync(string prompt)
        {
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
    {
        new ChatRequestUserMessage(prompt)
    },
                AzureExtensionsOptions = new AzureChatExtensionsOptions()
                {
                    Extensions =
        {
            new AzureCognitiveSearchChatExtensionConfiguration()
            {
                SearchEndpoint = new Uri("https://emasss.search.windows.net/"),
                IndexName = "emaindex",
                Key = "9nZyfKnZVZlS39vZn7qszacQ4PJ5UkG1XtgqVTJdzYAzSeBBS0BK",
                QueryType = AzureCognitiveSearchQueryType.Simple,
            },
        },
                },
                DeploymentName = "gpt432k",
                MaxTokens = 800,
                Temperature = 0,
            };

            var response = await openAIClient.GetChatCompletionsAsync(
                chatCompletionsOptions);

            var message = response.Value.Choices[0].Message.Content;
            if (message!.Contains("I'm sorry"))
                return "Îmi pare rău, nu cred că am înțeles întrebarea ta. Te rog să reformulezi întrebarea sau să îmi oferi mai multe detalii pentru a te putea ajuta.";
            else if (message!.Contains("The requested information"))
                return "Îmi pare rău, dar nu pot răspunde la această întrebare.";
            return RemoveTextBetweenBrackets(message);
        }
        static string RemoveTextBetweenBrackets(string input)
        {
            return Regex.Replace(input, @"\s+\[\w+\d+\]", "");
        }
    }
}
