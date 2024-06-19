using Microsoft.AspNetCore.Mvc;

using ProiectDiploma;

[ApiController]
[Route("[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly OpenAIService openAIService;

    public ChatbotController(OpenAIService openAIService)
    {
        this.openAIService = openAIService;
    }

    [HttpGet]
    public async Task <IActionResult> GetResponse(string input)
    {
        var response = await openAIService.GetResponseFromOpenAIAsync(input);
        return Ok(response);
    }
}
