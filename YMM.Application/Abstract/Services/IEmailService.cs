using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YMM.Application.Abstract.Services
{
    public interface IEmailService
    {
        public Task<string> SendEmail(string email, string message);
        Task SendResetPasswordEmailAsync(
       string toEmail,
       string userName,
       string resetUrl
   );
    }
}
