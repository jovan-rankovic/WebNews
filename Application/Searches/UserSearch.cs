﻿namespace Application.Searches
{
    public class UserSearch : Pagination
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; }
    }
}