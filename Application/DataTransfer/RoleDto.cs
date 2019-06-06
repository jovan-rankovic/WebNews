using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DataTransfer
{
    public class RoleDto : BaseDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must contain at least 3 letters.")]
        [MaxLength(15, ErrorMessage = "Name can have up to 15 letters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Letters only.")]
        public string Name { get; set; }

        public IEnumerable<string> Users { get; set; }
    }
}