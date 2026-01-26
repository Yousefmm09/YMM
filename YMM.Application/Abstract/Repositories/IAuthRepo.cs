using System;
using System.Collections.Generic;
using System.Text;
using YMM.Data.Entities.Identity;

namespace YMM.Application.Abstract.Repositories
{
    public interface IAuthRepo
    {
        Task<RefreshToken> CreatRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> checkrfToken(string rftoken);
    }
}
