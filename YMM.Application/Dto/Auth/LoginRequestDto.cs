using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Application.Dto.Auth
{
    public class LoginRequestDto
    {
        
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
