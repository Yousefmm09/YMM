using System;
using System.Collections.Generic;
using System.Text;
using YMM.Application.Dto.Auth;
using YMM.Application.Dto.Response;

namespace YMM.Application.Abstract.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<UserRegisterDto>> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<ApiResponse<UserRegisterDto>> UpdateAsync(UserRegisterDto userRegisterDto);
        Task<string> DeleteAsync();
        Task<string> CreatOtp(string userId, string email);
        Task<string> VerifytOtp(string email, string OtpCode);
        
        Task<ApiResponse<UserLoginRepsonse>> LoginUser(UserLoginDto dto);
        Task<RefreshTokenDto> CreatRefreshToken(string token);

        Task<string> ReSendOtpCodeAsync(string email);

        Task<ApiResponse<ForgetPasswordDto>> ForgetPassword(ForgetPasswordDto dto);

        Task<string> ChangePassword(ChangePasswordDto dto);

        Task<string> ResetPassword(string email, string token, string NewPassword);
    }
}
