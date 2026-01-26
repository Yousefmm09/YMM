using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Data.Entities.Identity;

namespace YMM.Application.Abstract.Repositories
{
    public interface IOtp
    {
        Task CreateAsync(OtpEmail otp);
        Task<OtpEmail?> GetLatestAsync(string userId);
        Task UpdateAsync(OtpEmail otp);
        Task<OtpEmail> GetOtpForUser(string userId);

    }
}
