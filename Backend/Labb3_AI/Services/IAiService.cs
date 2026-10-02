using Labb3_AI.DTOs;

namespace Labb3_AI.Services
{
    public interface IAiService
    {
        Task<string> GenerateInvitationAsync(RequestDTO request);
        Task<string> GenerateAgendaAsync(RequestDTO request);
        Task<string> SummarizeMeetingNotesAsync(RequestDTO request);
    }
}
