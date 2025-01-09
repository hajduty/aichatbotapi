using LLama.Common;
using System.Text.Json;

namespace api.Services
{
    // TODO: add user identifier to history, so not any user can access any history based on sessionId
    public class HistoryService
    {
        private readonly string HistoryFolder = "data/history";
        
        public HistoryService()
        {
            if (!Directory.Exists(HistoryFolder))
            {
                Directory.CreateDirectory(HistoryFolder);
            }
        }

        public string GenerateSessionId()
        {
            return $"session_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")}";
        }

        public async Task SaveHistoryAsync(string sessionId, ChatHistory chatHistory)
        {
            string filePath = Path.Combine(HistoryFolder, $"{sessionId}.json");
            string json = JsonSerializer.Serialize(chatHistory, new JsonSerializerOptions { WriteIndented = true });

            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
