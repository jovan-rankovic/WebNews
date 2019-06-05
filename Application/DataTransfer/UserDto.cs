using System.Collections.Generic;

namespace Application.DataTransfer
{
    public class UserDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Role { get; set; }
        public IEnumerable<string> Articles { get; set; }
    }
}