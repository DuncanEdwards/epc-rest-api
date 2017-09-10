using System;
using System.ComponentModel.DataAnnotations;

namespace Epc.API.Entities
{
    public class User
    {

        #region Public Properties

        [Key]
        public Guid Id { get; set; }

        [Required]
        public UserType Type { get; set; }

        [Required]
        [MaxLength(300)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(300)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(300)]
        public string Email { get; set; }

        [MaxLength(64)]
        public string Password { get; set; }

        [MaxLength(100)]
        public Guid? RememberToken { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        #endregion


    }
}
