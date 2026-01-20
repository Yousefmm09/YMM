using System;
using System.Collections.Generic;
using System.Text;

namespace YMM.Data.Entities.Identity
{
    public class UserProfile
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string UserId {  get; set; }
        public User user { get; set; }
    }
}
