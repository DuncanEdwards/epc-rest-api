using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Entities
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        public EpcType Type { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MaxLength(300)]
        public string AddressLine1 { get; set; }

        [MaxLength(300)]
        public string AddressLine2 { get; set; }

        [MaxLength(300)]
        public string AddressLine3 { get; set; }


    }
}
