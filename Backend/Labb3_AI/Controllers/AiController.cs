using Labb3_AI.DTOs;
using Labb3_AI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Labb3_AI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("invitation")]
        public async Task<IActionResult> Invitation([FromBody] RequestDTO request)
        {
            var invitations = await _aiService.GenerateInvitationAsync(request);
            return Ok(invitations);

        }

        [HttpPost("agenda")]
        public async Task<IActionResult> Agenda([FromBody] RequestDTO prompt)
        {
            var agenda = await _aiService.GenerateAgendaAsync(prompt);
            return Ok(agenda);
        }

        [HttpPost("summarize")]
        public async Task<IActionResult> Summarize([FromBody] RequestDTO prompt)
        {
            var summarize = await _aiService.SummarizeMeetingNotesAsync(prompt);
            return Ok(summarize);
        }
    }
}
