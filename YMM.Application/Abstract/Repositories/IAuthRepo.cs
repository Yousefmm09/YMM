using System;
using System.Collections.Generic;
using System.Text;
using YMM.Application.Dto.Auth;
using YMM.Application.Dto.Response;
using YMM.Data.Entities.Identity;

namespace YMM.Application.Abstract.Repositories
{
    public interface IAuthRepo
    {
         Task<RefreshToken> CreatRefreshToken(string refreshToken);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
        Task<RefreshToken> checkrfToken(string rftoken);
        Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto dto);
        Task<ApiResponse<string>> ResendVerificationEmailAsync(string email);
        Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordRequestDto dto);
        Task<ApiResponse<string>> LogoutAsync(string userId);
        Task<string> VerifyOtpAsync(string email,string otp);
    }
}
