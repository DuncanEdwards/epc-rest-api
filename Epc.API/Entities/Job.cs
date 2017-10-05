using System;
using System.ComponentModel.DataAnnotations;


namespace Epc.API.Entities
{
    public class Job
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime JobDate { get; set; }

        [Required]
        public EpcType Type { get; set; }


    }
}
