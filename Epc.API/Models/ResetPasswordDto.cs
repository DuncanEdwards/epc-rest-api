using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Epc.API.Models
{
    public class ResetPasswordDto
    {
        [Required]
        public Guid RememberToken { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
