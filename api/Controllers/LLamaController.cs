using LLama.Common;
using LLama.Sampling;
using LLama;
using Microsoft.AspNetCore.Mvc;
using LLama.Abstractions;
using api.Models;
using api.Services;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace api.Controllers
{
    // TODO: kör [Authorize] senare i hela controllern
    [ApiController]
    [Route("[controller]")]
    public class LLamaController : ControllerBase
    {
        private readonly HistoryService _historyService;
        private readonly IConfiguration _configuration;

        // Store several system prompts mapped to different historical figures
        private static readonly Dictionary<string, string> _systemPrompts = new()
        {
            {
                "1",
                "You are Donald Trump. Talk and act like him."
            },
            {
                "abraham-lincoln",
                "You are Abraham Lincoln. Speak in his thoughtful, measured tone."
            },
            {
                "theodore-roosevelt",
                "You are Theodore Roosevelt. Be bold, energetic, and use his distinct manner of speech."
            },
            {
                "donald-duck",
                "You are Donald Duck. Be funny, angry, and use his distinct manner of speech."
            },
            {
                "gollum",
                "You are Gollum. Be evil, sly, and use his distinct manner of speech."
            },
            {
                "santa",
                "You are santa. Be funny, happy, and use his distinct manner of speech."
            },
            {
                "3",
                "You are socrates. Use the socratic method."
            },
            {
                "snoop dogg",
                "You are snoop dogg. Be happy, high, and use his distinct manner of speech."
            },
            {
                "2",
                "You are Joe Biden. Be forgetful, dement, and use his distinct manner of speech."
            }
        };

        // In case someone picks an ID that’s not in our dictionary, pick a default:
        private const string DefaultSystemPromptId = "1";

        public LLamaController(IConfiguration configuration, HistoryService historyService)
        {
            _configuration = configuration;
            _historyService = historyService;
        }

        // TODO: Byt [FromBody] Chathistory.Message till modell som finns i WPF, "Send"
        [HttpPost("send/{presidentId?}")]
        public async Task<IActionResult> SendMessage([FromRoute] string presidentId, [FromBody] Send send)
        {
            if (string.IsNullOrWhiteSpace(send.Message))
                return BadRequest("User input cannot be empty.");

            if (send.Message.Length > 2048)
                return BadRequest("User input cannot exceed 2048 characters.");

            string modelPath = _configuration["ModelPath"]!;
            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 2048,
                GpuLayerCount = 5,
            };

            var model = LLamaWeights.LoadFromFile(parameters);
            var context = model.CreateContext(parameters);
            var executor = new InteractiveExecutor(context);

            var inferenceParams = new InferenceParams()
            {
                AntiPrompts = new List<string> { "User:" },
                MaxTokens = 2048, 
            };

            // Look up the system prompt by ID, or fall back to default if not found
            if (string.IsNullOrEmpty(presidentId))
            {
                presidentId = DefaultSystemPromptId;
            }

            if (!_systemPrompts.ContainsKey(presidentId.ToLower()))
            {
                presidentId = DefaultSystemPromptId;
            }

            // Add the system prompt to the prompt text
            var systemPrompt = _systemPrompts[presidentId.ToLower()];

            ChatSession session = new ChatSession(executor, new ChatHistory());
            ChatHistory history = new ChatHistory();

            if (send.SessionId != null)
            {
                history = await _historyService.GetHistoryAsync(send.SessionId);
                session = new ChatSession(executor, history);
            } else
            {
                history.AddMessage(AuthorRole.System, systemPrompt);
                session.AddSystemMessage(systemPrompt);
            }

            var response = string.Empty;
            await foreach (
                var text
                in session.ChatAsync(
                    new ChatHistory.Message(AuthorRole.User, send.Message),
                    inferenceParams))
            {
                response += text;
            }

            response = response.Split(new[] { "Assistant:", "Donald Trump:", "Joe Biden:", "Socrates:", "President Biden:" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()?.Trim() ?? response.Trim();
            response = response.Split("User:").FirstOrDefault()?.Trim() ?? response.Trim();
            response = response.Split("\n").FirstOrDefault()?.Trim() ?? response.Trim();
            response = Regex.Replace(response, @"[^\u0020-\u007E\u00A0-\u00FF]", "");

            // important
            if (send.SessionId == null)
            {
                history.AddMessage(AuthorRole.User, send.Message);
                history.AddMessage(AuthorRole.Assistant, response);
            }

            // TODO: error handling?
            // Save the session history using HistoryService
            string sessionId = send.SessionId ?? _historyService.GenerateSessionId();
            await _historyService.SaveHistoryAsync(sessionId, history);

            return Ok(new { Response = response, SessionId = sessionId });
        }

        [HttpGet("chat/{sessionId}")]
        public async Task<IActionResult> GetChatHistory(string sessionId)
        {
            var json = await _historyService.GetHistoryAsync(sessionId);

            if (json != null)
            {
                return Ok(json);
            }

            return NotFound();
        }
    }
}
