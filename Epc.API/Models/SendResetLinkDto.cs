using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Epc.API.Models
{
    public class SendResetLinkDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string ResetLink { get; set; }

        public bool IsNewUser = false;

    }
}
