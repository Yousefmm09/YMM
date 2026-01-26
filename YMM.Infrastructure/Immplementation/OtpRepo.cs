using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            await _appDb.OtpEmails.AddAsync(otp);
            await _appDb.SaveChangesAsync();
        }

        public async Task<OtpEmail?> GetLatestAsync(string userId)
        {
            return await _appDb.OtpEmails
               .OrderByDescending(o => o.CreatedAt)
               .Where(x => x.UserId == userId && !x.IsUsed)
               .FirstOrDefaultAsync();
        }

        public async Task<OtpEmail?> GetOtpForUser(string userId)
        {
            var otp = await _appDb.OtpEmails
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
            return otp;
        }

        public async Task UpdateAsync(OtpEmail otp)
        {
            _appDb.OtpEmails.Update(otp);
            await _appDb.SaveChangesAsync();
        }
    }
}
