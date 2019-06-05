using System.Collections.Generic;

namespace Application.DataTransfer
{
    public class RoleDto : BaseDto
    {
        public string Name { get; set; }

        public IEnumerable<string> Users { get; set; }
    }
}