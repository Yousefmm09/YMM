using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Application.Dto.Auth
{
    public class RegisterRequestDto
    {
      
        public string UserName { get; set; }


        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }

       
        public string Country { get; set; }

        
        public int Age { get; set; }
    }
}
