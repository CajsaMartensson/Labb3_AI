using Google.GenAI;
using Google.GenAI.Types;
using Labb3_AI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace Labb3_AI.Services
{
    public class AiService : IAiService
    {
        private readonly string _apiKey;

        public AiService(IConfiguration config)
        {
            _apiKey = config["GEMINI_API_KEY"];
        }

        public async Task<string> GenerateInvitationAsync(RequestDTO userPrompt)
        {
            var invitationSystemPrompt = "Du är en professionell assistent vars enda uppgift är att skriva mötesinbjudningar på svenska. " +
                "Du ska ENDAST svara med en välformulerad mötesinbjudan baserat på användarens underlag." +
                "Om användarens inmatning inte handlar om att skapa en mötesinbjudan eller saknar tillräcklig information," +
                "ska du artigt svara att du endast kan hjälpa till med mötesinbjudningar. " +
                "Du ska inte fatta egna beslut om mötet utöver det som framgår av texten.";

            return await SendToAiAsync(invitationSystemPrompt, userPrompt.Prompt);
        }

        public async Task<string> GenerateAgendaAsync(RequestDTO userPrompt)
        {
            var agendaSystemPrompt = "Du är en professionell assistent vars enda uppgift är att skapa mötesagendor på svenska. " +
                "Du ska ENDAST svara med en strukturerad mötesagenda med punkter baserat på användarens underlag. " +
                "Om användarens inmatning inte handlar om en mötesagenda, ska du svara att du endast kan hjälpa till med agendor. " +
                "Du ska inte fatta egna beslut åt användaren.";

            return await SendToAiAsync(agendaSystemPrompt, userPrompt.Prompt);
        }

        public async Task<string> SummarizeMeetingNotesAsync(RequestDTO userPrompt)
        {
            var summarizeSystemPrompt = "Du är en professionell assistent vars enda uppgift är att sammanfatta mötesanteckningar på svenska. " +
                "Du ska ENDAST svara med en kort och kärnfull sammanfattning av de inskickade mötesanteckningarna. " +
                "Om texten inte utgör mötesanteckningar, meddela att du endast sammanfattar anteckningar. " +
                "Du ska inte lägga till egen information eller fatta beslut.";

            return await SendToAiAsync(summarizeSystemPrompt, userPrompt.Prompt);
        }

        public async Task<string> SendToAiAsync(string systemPrompt, string userPrompt)
        {
            var client = new Client(apiKey: _apiKey);

            string outputSchema = @"
                {
                  ""type"": ""object"",
                  ""properties"": {
                        ""text"": { ""type"": ""string"" }
                      },
                      ""required"": [ ""text""]
                    }
                  ";

            var config = new GenerateContentConfig
            {
                ResponseJsonSchema = JsonNode.Parse(outputSchema),
                ResponseMimeType = "application/json",
                SystemInstruction = new Content
                {
                    Parts = [
                        new Part{
                            Text = systemPrompt
                        }
                    ]
                }
            };

            var response = await client.Models.GenerateContentAsync("gemini-3.5-flash-lite", contents: userPrompt, config);


            return response.Candidates[0].Content.Parts[0].Text;
        }
    }
}
