using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;
using YMM.Application.Dto.Response;
using YMM.Data.Entities;
using YMM.Data.Entities.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IEmailService _emailService;
    private readonly IAuthRepo _auth;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        ILogger<AuthService> logger,
        IEmailService emailService,
        IAuthRepo auth)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _logger = logger;
        _emailService = emailService;
        _auth = auth;
    }
    public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto dto)
    {
      var register= await _auth.RegisterAsync(dto);
        return register;
    }
    public async Task<ApiResponse<string>> VerifyEmailAsync(VerifyEmailRequestDto dto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
            {
                return new ApiResponse<string>
                (
                    Success: false,
                    Message: "User not found",
                    Data: null,
                    Errors: new[] { "Invalid user ID" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            if (user.EmailConfirmed)
            {
                return new ApiResponse<string>
                (
                    Success: false,
                    Message: "Email already verified",
                    Data: null,
                    Errors: new[] { "This email has already been verified" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }
            var codeBytes = WebEncoders.Base64UrlDecode(dto.Token);
            var decodedCode = Encoding.UTF8.GetString(codeBytes);
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (!result.Succeeded)
            {
                return new ApiResponse<string>
                (
                    Success: false,
                    Message: "Email verification failed",
                    Data: null,
                    Errors: result.Errors.Select(e => e.Description),
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            _logger.LogInformation("Email verified successfully for user: {UserId}", dto.UserId);

            return new ApiResponse<string>
            (
                Success: true,
                Message: "Email verified successfully. You can now login.",
                Data: "Email verified",
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying email for user: {UserId}", dto.UserId);

            return new ApiResponse<string>
            (
                Success: false,
                Message: "An error occurred during email verification",
                Data: null,
                Errors: new[] { ex.Message },
                TraceId: Guid.NewGuid().ToString()
            );
        }
    }

    public async Task<ApiResponse<string>> ResendVerificationEmailAsync(string email)
    {
        var emailResend= await _auth.ResendVerificationEmailAsync(email);
        return emailResend;
    }

    public Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
    {
        var forgotPassword= _auth.ForgotPasswordAsync(dto);
        return forgotPassword;
    }

    public Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordRequestDto dto)
    {
        var resetPassword= _auth.ResetPasswordAsync(dto);
        return resetPassword;
    }

    public Task<ApiResponse<string>> LogoutAsync(string userId)
    {
        var logout= _auth.LogoutAsync(userId);
        return logout;
    }

    public async Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(string refreshToken)
    {
       var rtoken=await _auth.CreatRefreshToken(refreshToken);
        var loginResponseDto = new LoginResponseDto
        {
            UserId = rtoken.User.Id,
            UserName = rtoken.User.UserName,
            Email = rtoken.User.Email,
            Token = rtoken.Token,
            RefreshToken = rtoken.Token,
            TokenExpiration = rtoken.ExpiresAt,
            Role = (await _userManager.GetRolesAsync(rtoken.User)).FirstOrDefault() ?? "Customer"
        };

        return new ApiResponse<LoginResponseDto>
        (
            Success: true,
            Message: "Token refreshed successfully",
            Data: null,
            Errors: null,
            TraceId: Guid.NewGuid().ToString()
        );

    }
    public async Task<string> VerifyOtpAsync(string email,string otp)
    {
        var res=await _auth.VerifyOtpAsync(email,otp);
        return res;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
    {
         var res=await _auth.LoginAsync(dto);
        return res;
    }
}