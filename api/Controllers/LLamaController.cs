using LLama.Common;
using LLama.Sampling;
using LLama;
using Microsoft.AspNetCore.Mvc;
using LLama.Abstractions;

namespace api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class LLamaController : ControllerBase
	{
		private readonly InteractiveExecutor _executor;
		private readonly InferenceParams _inferenceParams;

		// Store several system prompts mapped to different historical figures
		private static readonly Dictionary<string, string> _systemPrompts = new()
		{
			{
				"donald-trump",
				"<|im_start|>system\nYou are Donald Trump. Talk and act like him.\n<|im_end|>"
			},
			{
				"abraham-lincoln",
				"<|im_start|>system\nYou are Abraham Lincoln. Speak in his thoughtful, measured tone.\n<|im_end|>"
			},
			{
				"theodore-roosevelt",
				"<|im_start|>system\nYou are Theodore Roosevelt. Be bold, energetic, and use his distinct manner of speech.\n<|im_end|>"
			},
			{
				"donald-duck",
				"<|im_start|>system\nYou are Donald Duck. Be funny, angry, and use his distinct manner of speech.\n<|im_end|>"
			},
			{
				"gollum",
				"<|im_start|>system\nYou are Gollum. Be evil, sly, and use his distinct manner of speech.\n<|im_end|>"
			},
			{
				"santa",
				"<|im_start|>system\nYou are santa. Be funny, happy, and use his distinct manner of speech.\n<|im_end|>"
			},
			{
				"socrates",
				"<|im_start|>system\nYou are socrates. Use the socratic method.\n<|im_end|>"
			},
			{
				"snoop dogg",
				"<|im_start|>system\nYou are snoop dogg. Be happy, high, and use his distinct manner of speech.\n<|im_end|>"
			},
			{
				"joe",
				"<|im_start|>system\nYou are Joe Biden. Be forgetful, dement, and use his distinct manner of speech.\n<|im_end|>"
			},
			{
				"yoda",
				"<|im_start|>system\nYou are Yoda. Be wise, insightful, and use his distinct manner of speech.\n<|im_end|>"
			}
		};

		// In case someone picks an ID that’s not in our dictionary, pick a default:
		private const string DefaultSystemPromptId = "donald-trump";

		public LLamaController(IConfiguration configuration)
		{
			// Modify this to your own local path/model
			string modelPath = configuration["ModelPath"]!;
			var parameters = new ModelParams(modelPath)
			{
				ContextSize = 2048,
				GpuLayerCount = 5
			};

			var model = LLamaWeights.LoadFromFile(parameters);
			var context = model.CreateContext(parameters);
			_executor = new InteractiveExecutor(context);

			// These inference parameters can be tuned as well.
			_inferenceParams = new InferenceParams()
			{
				MaxTokens = 2048,
				AntiPrompts = new List<string> { "<|im_end|>", "User:", "Assistant:" }
			};
		}

		[HttpPost("send/{presidentId?}")]
		public async Task<IActionResult> SendMessage(
			[FromRoute] string presidentId,
			[FromBody] ChatHistory.Message userInput)
		{
			if (string.IsNullOrWhiteSpace(userInput.Content))
				return BadRequest("User input cannot be empty.");

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

			// Construct the final prompt with user’s message
			var prompt =
				$"{systemPrompt}" +          // e.g. You are Abraham Lincoln...
				$"<|im_start|>user\n{userInput.Content}<|im_end|>\n" +
				$"<|im_start|>assistant\n";

			var response = string.Empty;
			await foreach (var text in _executor.InferAsync(prompt, _inferenceParams))
			{
				response += text;
			}

			// Don’t include anything after or including <|im_end|>
			response = response.Split("<|im_end|>").FirstOrDefault()?.Trim() ?? response.Trim();

			return Ok(new { Response = response });
		}
	}
}
