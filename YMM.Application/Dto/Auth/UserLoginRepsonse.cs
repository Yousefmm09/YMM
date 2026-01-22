using System;
using System.Collections.Generic;
using System.Text;

namespace YMM.Application.Dto.Auth
{
    public class UserLoginRepsonse
    {
        public bool success { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
