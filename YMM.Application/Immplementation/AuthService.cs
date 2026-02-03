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
    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        try
        {
            // 1. Find user by email
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new ApiResponse<LoginResponseDto>
                (
                    Success: false,
                    Message: "Invalid email or password",
                    Data: null,
                    Errors: new[] { "User not found" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            // 2. Check if user is active
            if (!user.IsActive)
            {
                return new ApiResponse<LoginResponseDto>
                (
                    Success: false,
                    Message: "Account is deactivated",
                    Data: null,
                    Errors: new[] { "Your account has been deactivated. Please contact support." },
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            // 3. Check if email is verified
            if (!user.EmailConfirmed)
            {
                return new ApiResponse<LoginResponseDto>
                (
                    Success: false,
                    Message: "Email not verified",
                    Data: null,
                    Errors: new[] { "Please verify your email before logging in" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            // 4. Check password
            var signInResult = await _signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password,
                lockoutOnFailure: true);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    return new ApiResponse<LoginResponseDto>
                    (
                        Success: false,
                        Message: "Account locked",
                        Data: null,
                        Errors: new[] { "Your account has been locked due to multiple failed login attempts" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

                return new ApiResponse<LoginResponseDto>
                (
                    Success: false,
                    Message: "Invalid email or password",
                    Data: null,
                    Errors: new[] { "Incorrect password" },
                    TraceId: Guid.NewGuid().ToString()
                );
            }

            // 5. Generate JWT token
            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // 6. Save refresh token to database
            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // 7. Get user roles
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Customer";

            // 8. Return response
            var response = new LoginResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
                RefreshToken = refreshToken,
                TokenExpiration = DateTime.UtcNow.AddHours(24),
                Role = role
            };

            _logger.LogInformation("User logged in successfully: {Email}", dto.Email);

            return new ApiResponse<LoginResponseDto>
            (
                Success: true,
                Message: "Login successful",
                Data: response,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", dto.Email);

            return new ApiResponse<LoginResponseDto>
            (
                Success: false,
                Message: "An error occurred during login",
                Data: null,
                Errors: new[] { ex.Message },
                TraceId: Guid.NewGuid().ToString()
            );
        }
    }

    // ========================================
    // VERIFY EMAIL
    // ========================================
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

    // ========================================
    // HELPER METHODS
    // ========================================
    private async Task<string> GenerateJwtToken(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
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
        var rfToken = new RefreshToken
        {
            Token = refreshToken
        };
        var refresh= await _auth.CreatRefreshToken(rfToken);
        var refreshTokenResponse = new RefreshToken
        {
            Token = refresh.Token,
            ExpiresAt = refresh.ExpiresAt,
            IsRevoked = refresh.IsRevoked,
            UserId = refresh.UserId
        };
        var loginResponseDto = new LoginResponseDto
        {
            UserId = refresh.User.Id,
            UserName = refresh.User.UserName,
            Email = refresh.User.Email,
            Token = refresh.Token,
            RefreshToken = refresh.Token,
            TokenExpiration = refresh.ExpiresAt,
            Role = (await _userManager.GetRolesAsync(refresh.User)).FirstOrDefault() ?? "Customer"
        };

        return new ApiResponse<LoginResponseDto>
        (
            Success: true,
            Message: "Token refreshed successfully",
            Data: loginResponseDto,
            Errors: null,
            TraceId: Guid.NewGuid().ToString()
        );

    }
}