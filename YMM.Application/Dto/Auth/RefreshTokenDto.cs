using System;
using System.Collections.Generic;
using System.Text;

namespace YMM.Application.Dto.Auth
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public string AccessToken { get; set; }
    }
}
