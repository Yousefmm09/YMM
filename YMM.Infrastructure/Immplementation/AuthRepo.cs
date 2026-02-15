using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;
using YMM.Application.Dto.Response;
using YMM.Application.Helper;
using YMM.Data.Entities;
using YMM.Data.Entities.Identity;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class AuthRepo : IAuthRepo
    {
        private readonly AppDb _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthRepo> _logger;
        private readonly IEmailService _emailService;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthRepo(AppDb context, UserManager<User> userManager, IConfiguration configuration, IEmailService emailService,ILogger<AuthRepo> logger, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<ApiResponse<RegisterResponseDto>> RegisterAsync(RegisterRequestDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Check if email already exists
                var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<RegisterResponseDto>
                    (
                        Success: false,
                        Message: "Email already registered",
                        Data: null,
                        Errors: new[] { "A user with this email already exists" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

                // 2. Check if username already exists
                var existingUsername = await _userManager.FindByNameAsync(dto.UserName);
                if (existingUsername != null)
                {
                    return new ApiResponse<RegisterResponseDto>
                    (
                        Success: false,
                        Message: "Username already taken",
                        Data: null,
                        Errors: new[] { "This username is already in use" },
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

                // 3. Create new user
                var newUser = new User
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Country = dto.Country,
                    Age = dto.Age,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    EmailConfirmed = false
                };

                // 4. Create user with password
                var createResult = await _userManager.CreateAsync(newUser, dto.Password);

                if (!createResult.Succeeded)
                {
                    return new ApiResponse<RegisterResponseDto>
                    (
                        Success: false,
                        Message: "User registration failed",
                        Data: null,
                        Errors: createResult.Errors.Select(e => e.Description),
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

                // 5. Assign default role
                var roleResult = await _userManager.AddToRoleAsync(newUser, "Customer");

                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(newUser);
                    return new ApiResponse<RegisterResponseDto>
                    (
                        Success: false,
                        Message: "Failed to assign user role",
                        Data: null,
                        Errors: roleResult.Errors.Select(e => e.Description),
                        TraceId: Guid.NewGuid().ToString()
                    );
                }

                // 6. Create empty cart for user
                var cart = new Cart
                {
                    UserId = newUser.Id,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.Carts.AddAsync(cart);

                // 7. Generate email verification Otp
                var sendOtp =  GenerateOtp.Otp();
                var HashOtp = GenerateOtp.Hash(sendOtp);
                var OEmail = new OtpEmail
                {
                    UserId= newUser.Id,
                    CreatedAt= DateTime.UtcNow,
                    Attmeps=0,
                    IsUsed=false,
                    OtpHash=HashOtp,
                    ExpiresAt=DateTime.UtcNow.AddMinutes(10)
                };
                await _context.OtpEmails.AddAsync(OEmail);
                await _context.SaveChangesAsync();
                //var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                //var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));
                var sendEmail = await _emailService.SendEmail(newUser.Email, " The otp to verify email " + sendOtp);
                await transaction.CommitAsync();

                // 9. Return response
                var response = new RegisterResponseDto
                {
                    UserId = newUser.Id,
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber,
                    Country = newUser.Country,
                    Age = newUser.Age,
                    Role = "Customer",
                    EmailVerificationRequired = true,
                    CreatedAt = newUser.CreatedAt
                };
                
                _logger.LogInformation("User registered successfully: {Email}", dto.Email);

                return new ApiResponse<RegisterResponseDto>
                (
                    Success: true,
                    Message: "Registration successful. Please check your email to verify your account.",
                    Data: response,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                );
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error during user registration for email: {Email}", dto.Email);

                return new ApiResponse<RegisterResponseDto>
                (
                    Success: false,
                    Message: "An error occurred during registration",
                    Data: null,
                    Errors: new[] { ex.Message },
                    TraceId: Guid.NewGuid().ToString()
                );
            }
        }
        public async Task<string> VerifyOtpAsync(string email,string otp)
        {
            if (string.IsNullOrEmpty(otp))
                return "Please enter the OTP";

            var userEmail = await _userManager.FindByEmailAsync(email);
            if (userEmail == null) return "User not found";

            var OtpE = await _context.OtpEmails
                .Where(x => x.UserId == userEmail.Id)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
            var hashedOtp = GenerateOtp.Hash(otp);
            if (OtpE.OtpHash != hashedOtp)
            {
                OtpE.Attmeps += 1; 
                await _context.SaveChangesAsync();
                return "Incorrect OTP";
            }
            if (OtpE.Attmeps > 5)
            {
                OtpE.IsUsed = true;
                return "your Otp has maximum for attemps,Please resend email otp";
            }
            if (OtpE.IsUsed==true)
                return "your Otp is used ,Please resend Otp Email";
            if (OtpE.ExpiresAt < DateTime.UtcNow)
                return "your Otp is is expire now .Please resend Otp Email";

            if (OtpE!=null)
            {
                OtpE.Attmeps +=1;
                OtpE.IsUsed = true;
                var user = await _userManager.FindByIdAsync(OtpE.UserId);
                user.EmailConfirmed = true;
                await _context.SaveChangesAsync();
                return "The Otp is True ,and your account is now verify";
            }

            return "Please put your otp ";
        }
        public async Task<RefreshToken> checkrfToken(string rftoken)
        {
            var token = await _context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == rftoken
            && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow);
            return token;
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
                var rfToken = new RefreshToken
                {
                    Token = refreshToken,
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false,
                };
                await _context.AddAsync( rfToken );
                await _context.SaveChangesAsync();
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
        public async Task<RefreshToken> CreatRefreshToken(string refreshToken)
        {
            var rftoken=await _context.RefreshTokens.Include(x=>x.User).Where(x=>x.Token==refreshToken).FirstOrDefaultAsync();
            if(rftoken==null || rftoken.IsRevoked || rftoken.ExpiresAt<DateTime.UtcNow)
                throw new Exception ("Invalid or expired refresh token");
            rftoken.IsRevoked= true;
            var newRfToken = GenerateRefreshToken();
            var rfToken = new RefreshToken
            {
                Token = newRfToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                UserId = rftoken.UserId
            };
            await _context.RefreshTokens.AddAsync(rfToken);
            await _context.SaveChangesAsync();
            return rftoken;
        }

        public Task<ApiResponse<string>> ResendVerificationEmailAsync(string email)
        {
            // impelment now 
            var user = _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Task.FromResult(new ApiResponse<string>
                (
                    Success: false,
                    Message: "User not found",
                    Data: null,
                    Errors: new[] { "No user associated with this email" },
                    TraceId: Guid.NewGuid().ToString()
                ));
            }
            var emailToken =  _userManager.GenerateEmailConfirmationTokenAsync(user.Result);
            var sendEmailToken = System.Web.HttpUtility.UrlEncode(emailToken.Result);
            var sendEmail =  _emailService.SendEmail(sendEmailToken, "ConfirmationEmail");
            return Task.FromResult(new ApiResponse<string>
            (
                Success: true,
                Message: "Verification email resent successfully",
                Data: "Please check your email to verify your account.",
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            ));
        }

        public Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
        {
            var user =  _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Task.FromResult(new ApiResponse<string>
                (
                    Success: false,
                    Message: "User not found",
                    Data: null,
                    Errors: new[] { "No user associated with this email" },
                    TraceId: Guid.NewGuid().ToString()
                ));
            }
            var resetToken =  _userManager.GeneratePasswordResetTokenAsync(user.Result);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken.Result));
            var sendEmail =  _emailService.SendEmail(dto.Email, "ResetPasswordEmail , the token is " + encodedToken);
            return Task.FromResult(new ApiResponse<string>
            (
                Success: true,
                Message: "Password reset email sent successfully",
                Data: "Please check your email to reset your password.",
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            ));
        }

        public Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            var user =  _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return Task.FromResult(new ApiResponse<string>
                (
                    Success: false,
                    Message: "User not found",
                    Data: null,
                    Errors: new[] { "No user associated with this ID" },
                    TraceId: Guid.NewGuid().ToString()
                ));
            }
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(dto.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            var resetResult =  _userManager.ResetPasswordAsync(user.Result, decodedToken, dto.NewPassword);
            if (!resetResult.Result.Succeeded)
            {
                return Task.FromResult(new ApiResponse<string>
                (
                    Success: false,
                    Message: "Password reset failed",
                    Data: null,
                    Errors: resetResult.Result.Errors.Select(e => e.Description),
                    TraceId: Guid.NewGuid().ToString()
                ));
            }
            return Task.FromResult(new ApiResponse<string>
            (
                Success: true,
                Message: "Password reset successful",
                Data: "Your password has been reset successfully.",
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            ));
        }

        public Task<ApiResponse<string>> LogoutAsync(string userId)
        {
            var user =  _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Task.FromResult(new ApiResponse<string>
                (
                    Success: false,
                    Message: "User not found",
                    Data: null,
                    Errors: new[] { "No user associated with this ID" },
                    TraceId: Guid.NewGuid().ToString()
                ));
            }
            // Here you would typically revoke the user's tokens or clear their session
            return Task.FromResult(new ApiResponse<string>
            (
                Success: true,
                Message: "Logout successful",
                Data: "You have been logged out successfully.",
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
            ));
        }
    }
}
