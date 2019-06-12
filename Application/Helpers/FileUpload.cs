using System.Collections.Generic;

namespace Application.Helpers
{
    public class FileUpload
    {
        public static IEnumerable<string> AllowedExtensions => new List<string> { ".jpeg", ".jpg", ".png" };
    }
}