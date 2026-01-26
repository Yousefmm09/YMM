using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using YMM.Application.Abstract.Repositories;
using YMM.Data.Entities.Identity;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AppDb _appDb;
        public AuthRepo(AppDb appDb)
        {
            _appDb = appDb;
        }
        public async Task<RefreshToken> checkrfToken(string rftoken)
        {
            var token = await _appDb.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == rftoken
            && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow);
            return token;
        }

        public  async Task<RefreshToken> CreatRefreshToken(RefreshToken refreshToken)
        {
            var token = _appDb.RefreshTokens.Add(refreshToken);
            await _appDb.SaveChangesAsync();
            return token.Entity;
        }
    }
}
