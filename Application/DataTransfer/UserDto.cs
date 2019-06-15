using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransfer
{
    public class UserDto : BaseDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "First name must contain at least 3 letters.")]
        [MaxLength(30, ErrorMessage = "First name can have up to 30 letters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Letters only.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Last name must contain at least 3 letters.")]
        [MaxLength(30, ErrorMessage = "Last name can have up to 30 letters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Letters only.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Email must contain at least 3 characters.")]
        [MaxLength(60, ErrorMessage = "Email can have up to 60 characters.")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must contain at least 6 characters.")]
        [MaxLength(32, ErrorMessage = "Password can have up to 32 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int? RoleId { get; set; }

        public string Role { get; set; }
        public IEnumerable<string> Articles { get; set; }
        public IEnumerable<string> Comments { get; set; }
    }
}