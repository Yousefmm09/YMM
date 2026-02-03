using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;
using YMM.Application.Dto.Response;
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

        public AuthRepo(AppDb context, UserManager<User> userManager, IEmailService emailService,ILogger<AuthRepo> logger)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
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
                await _context.SaveChangesAsync();

                // 7. Generate email verification token
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));
                var sendEmail =
                // 8. Send verification email
                await _emailService.SendEmail(newUser.Email, "ConfirmationEmail the token is " + encodedToken);

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
        public async Task<RefreshToken> checkrfToken(string rftoken)
        {
            var token = await _context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == rftoken
            && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow);
            return token;
        }

        public  async Task<RefreshToken> CreatRefreshToken(RefreshToken refreshToken)
        {
            var token = _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return token.Entity;
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
