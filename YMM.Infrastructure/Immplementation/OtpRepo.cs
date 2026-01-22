using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Data.Entities.Identity;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class OtpRepo : IOtp
    {
        private readonly AppDb _appDb;
        public OtpRepo(AppDb appDb)
        {
         _appDb = appDb;  
        }
        public async Task CreateAsync(OtpEmail otp)
        {
            var otps =  await _appDb.otpEmails.AddAsync(otp);
            await _appDb.SaveChangesAsync();
        }

        public async Task<OtpEmail?> GetLatestAsync(string userId)
        {
            return  _appDb.otpEmails
               .OrderByDescending(o => o.CreatedAt)
               .Where(x=>x.UserId==userId&&!x.IsUsed)
               .FirstOrDefault();
        }

        public async Task<OtpEmail> GetOtpForUser(string userId)
        {
            var otp = await _appDb.otpEmails
                .OrderByDescending(x=>x.CreatedAt).Where(x => x.UserId == userId).FirstOrDefaultAsync();
            return otp;
        }

        public async Task UpdateAsync(OtpEmail otp)
        {
            _appDb.otpEmails.Update(otp);
            await _appDb.SaveChangesAsync();
        }
    }
}
