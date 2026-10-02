using System.ComponentModel.DataAnnotations;

namespace Labb3_AI.DTOs
{
    public class RequestDTO
    {
        [Required]
        [MinLength(3)]
        public string Prompt { get; set; } = string.Empty;
    }
}
