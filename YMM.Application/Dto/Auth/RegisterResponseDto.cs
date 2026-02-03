using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Application.Dto.Auth
{
    public class RegisterResponseDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }
        public bool EmailVerificationRequired { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
