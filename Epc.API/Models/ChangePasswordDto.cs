﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Epc.API.Models
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

    }
}
