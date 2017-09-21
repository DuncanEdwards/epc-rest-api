using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Epc.API.Models
{
    public class SendResetLinkDto
    {
        [Required]
        public string Email { get; set; }

    }
}
