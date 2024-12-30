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
        private readonly ChatHistory _chatHistory;
        private readonly InferenceParams _inferenceParams;

        public LLamaController()
        {
            string modelPath = @"C:\Users\hajde\Downloads\Meta-Llama-3-8B-Instruct-IQ2_XXS.gguf";
            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 2048,
                GpuLayerCount = 5
            };

            var model = LLamaWeights.LoadFromFile(parameters);
            var context = model.CreateContext(parameters);
            _executor = new InteractiveExecutor(context);
            _chatHistory = new ChatHistory();

            _chatHistory.AddMessage(AuthorRole.System,
                "<|im_start|>system\nYou are a helpful AI assistant. Give direct and clear responses.<|im_end|>");

            _inferenceParams = new InferenceParams()
            {
                MaxTokens = 2048,
                AntiPrompts = new List<string> { "<|im_end|>", "User:", "Assistant:" }
            };
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatHistory.Message userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput.Content))
                return BadRequest("User input cannot be empty.");

            var prompt = $"<|im_start|>user\n{userInput.Content}<|im_end|>\n<|im_start|>assistant\n";

            var response = string.Empty;
            await foreach (var text in _executor.InferAsync(prompt, _inferenceParams))
            {
                response += text;
            }

            response = response.Split("<|im_end|>").FirstOrDefault()?.Trim() ?? response.Trim();
            return Ok(new { Response = response });
        }
    }
}
