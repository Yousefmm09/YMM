using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;
using YMM.Application.Dto.Response;
using YMM.Data.Entities.Identity;

namespace YMM.Application.Immplementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthRepo _authRepo;
        private readonly IEmailService _emailService;
        private readonly IOtp _otp;
        public AuthService( UserManager<User> userManager,IEmailService emailService,IOtp otp ,IHttpContextAccessor httpContext, IConfiguration configuration, IAuthRepo authRepo, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _httpContext = httpContext;
            _configuration = configuration;
            _authRepo = authRepo;
            _signInManager = signInManager;
            _emailService = emailService;
            _otp = otp;
        }
        public async Task<string> DeleteAsync()
        {
            var user = _httpContext.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            var userEmail = await _userManager.FindByEmailAsync(user);
            if (userEmail == null)
                return "not found";
            var deleteUser = await _userManager.DeleteAsync(userEmail);
            if (deleteUser.Succeeded)
                return "the user is deleted";
            return "the user not register";

        }

        public async Task<ApiResponse<UserRegisterDto>> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var checkPassword = userRegisterDto.Password;
            if (checkPassword != userRegisterDto.ConfirmPassowrd)
                return new ApiResponse<UserRegisterDto>
                    (
                    Success: false,
                    Message: "The Password and confirm password not match",
                    Data: null,
                    Errors: null,
                    TraceId: Guid.NewGuid().ToString()
                    );
            var userName = await _userManager.FindByEmailAsync(userRegisterDto.EmailAddress);
            if (userName != null)
                return new ApiResponse<UserRegisterDto>
                   (
                   Success: false,
                   Message: "The Emai is exist",
                   Data: null,
                   Errors: null,
                   TraceId: Guid.NewGuid().ToString()
                   );
            var newUser = new User
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.EmailAddress,
                PhoneNumber = userRegisterDto.PhoneNumber,
                Country = userRegisterDto.Country,
                Age = userRegisterDto.Age,
            };
            var userDto = new UserRegisterDto
            {
                UserName = newUser.UserName,
                PhoneNumber = newUser.PhoneNumber,
                Country = newUser.Country,
                Age = newUser.Age,
            };
            var user = await _userManager.CreateAsync(newUser, checkPassword);
            var role =  await _userManager.AddToRoleAsync(newUser, "Customer");

            var otp = await CreatOtp(newUser.Id, newUser.Email);
            return new ApiResponse<UserRegisterDto>
                   (
                   Success: user != null ? true : false,
                   Message: user != null ? "The User is Register Success, Please Verfiy your email" : "The User Register faild",
                   Data: userDto,
                   Errors: user.Errors.Select(x => x.Description),
                   TraceId: Guid.NewGuid().ToString()
                   );
        }
        public async Task<ApiResponse<UserLoginRepsonse>> LoginUser(UserLoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return new ApiResponse<UserLoginRepsonse>
                   (
                   Success: false,
                   Message: "Not found User",
                   Data: null,
                   Errors: null,
                   TraceId: Guid.NewGuid().ToString()
                   );

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
           var checkEMailConfirmation= await _userManager.IsEmailConfirmedAsync(user);
            if (checkEMailConfirmation == false)
            {
                return new ApiResponse<UserLoginRepsonse>
                     (
                     Success: false,
                     Message: "Please Verify your email",
                     Data:null,
                     Errors: null,
                     TraceId: Guid.NewGuid().ToString()
                     );
            }
            if (checkPassword.Succeeded)
            {
                var token = await CreatJwtToken(user);
                var RefreshToken = CreatRefreshToken();

                var rfToken = new RefreshToken
                {
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.AddDays(4),
                    IsRevoked = false,
                    Token = CreatRefreshToken()
                };
                await _authRepo.CreatRefreshToken(rfToken);
                return new ApiResponse<UserLoginRepsonse>
                     (
                     Success: false,
                     Message: "Success to creat Jwt Token",
                     Data: new UserLoginRepsonse
                     {
                         success = true,
                         AccessToken = token.AccessToken,
                         RefreshToken = rfToken.Token
                     },
                     Errors: null,
                     TraceId: Guid.NewGuid().ToString()
                     );
            }
            return new ApiResponse<UserLoginRepsonse>
                (
                Success: false,
                Message: "not Correct Password",
                Data: null,
                Errors: null,
                TraceId: Guid.NewGuid().ToString()
                );
        }
        public async Task<UserLoginRepsonse> CreatJwtToken(User user)
        {
            // creat Ket
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            // creat signture
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
            };
            var roles = await  _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }
            // creat token
            var token = new JwtSecurityToken
                (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claim,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: sign
                );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new UserLoginRepsonse
            {
                success = true,
                AccessToken = accessToken,
            };
        }
        //renew AccessToken
        public async Task<RefreshTokenDto> CreatRefreshToken(string token)
        {
            var rftoken = await _authRepo.checkrfToken(token);
            if (token == null)
                throw new SecurityTokenException("Invalid Token");
            rftoken.IsRevoked = true;
            var newRfToken = new RefreshToken
            {
                Token = Hashing(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                UserId = rftoken.UserId
            };
            await _authRepo.CreatRefreshToken(newRfToken);
            var newAccessToken = await CreatJwtToken(rftoken.User);

            return new RefreshTokenDto
            {
                AccessToken = newAccessToken.AccessToken,
                Token = newRfToken.Token,
            };
        }
        private static string CreatRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
        private static string Hashing()
        {
            string token = CreatRefreshToken();
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }
        public async Task<ApiResponse<UserRegisterDto>> UpdateAsync(UserRegisterDto userRegisterDto)
        {
            var user = await _userManager.FindByEmailAsync(userRegisterDto.EmailAddress);
            if (user == null)
                return new ApiResponse<UserRegisterDto>
                  (
                  Success: false,
                  Message: "Not found user",
                  Data: null,
                  Errors: null,
                  TraceId: Guid.NewGuid().ToString()
                  );
            user.UserName = userRegisterDto.UserName;
            user.Age = userRegisterDto.Age;
            user.PhoneNumber = userRegisterDto.PhoneNumber;
            user.Country = userRegisterDto.Country;
            var updateUserDto = new UserRegisterDto
            {
                UserName = user.UserName,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber,
                Country = user.Country,
            };
            var upUser = await _userManager.UpdateAsync(user);
            return new ApiResponse<UserRegisterDto>
                  (
                  Success: user != null ? true : false,
                  Message: user != null ? "The User is updated Success" : "The User updated faild",
                  Data: updateUserDto,
                  Errors: null,
                  TraceId: Guid.NewGuid().ToString()
                  );
        }

        public async Task<string> CreatOtp(string userId, string email)
        {
            var user =await  _userManager.FindByIdAsync(userId);
            if (user == null)
                return "not found user";
            var otp = Helper.GenerateOtp.Otp();
            var oEmail = new OtpEmail
            {
                UserId = user.Id,
                IsUsed = false,
                Attmeps = 0,
                CreatedAt = DateTime.UtcNow,
                OtpHash = Helper.GenerateOtp.Hash(otp),
                ExpiresAt=DateTime.UtcNow.AddMinutes(5)
            };
           await  _otp.CreateAsync(oEmail);
            await _emailService.SendEmail(email, $"Your OTP code is: {otp}");
            return ("OTP sent successfully");

        }
        public async Task<string> VerifytOtp(string email, string OtpCode)
        {
            var user= await _userManager.FindByEmailAsync(email);
            if (user == null) return "not found email";
            var userId =await _userManager.GetUserIdAsync(user);
            if (userId== null) return "not found user";
            var otp = await _otp.GetLatestAsync(user.Id);

            if (otp == null)
            {
                throw new Exception("OTP not found");
            }

            if (otp.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("OTP expired");
            }

            if (otp.Attmeps >= 5)
            {
                throw new Exception("Too many attempts");
            }

            var hashOtp = Helper.GenerateOtp.Hash(OtpCode);
            if(otp.OtpHash!=hashOtp)
            {
                otp.Attmeps++;
               await _otp.UpdateAsync(otp);
                return "the otp code wrong";
            }
            otp.IsUsed = true;
            otp.Attmeps += 1;
            await _otp.UpdateAsync(otp);
            if (user.EmailConfirmed==false)
            {
                user.EmailConfirmed = true;
               await  _userManager.UpdateAsync(user);
                return "the email is confirmed";
            }
            return ("OTP verified successfully");
        }
        public async Task<string> ReSendOtpCodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return "User not found";

            if (user.EmailConfirmed)
                return "Email already verified";

            var userId = await _userManager.GetUserIdAsync(user);
            if (string.IsNullOrEmpty(userId))
                return "Invalid user";

            var existingOtp = await _otp.GetLatestAsync(userId);

            if (existingOtp != null)
            {
                // OTP still valid → do not resend
                if (!existingOtp.IsExpired() && existingOtp.Attmeps < 5)
                    return "OTP already sent, please check your email";

                // invalidate old OTP
                existingOtp.IsUsed=true;
                await _otp.UpdateAsync(existingOtp);
            }

            await CreatOtp(userId, user.Email);

            return "OTP has been resent to your email";
        }

        public async Task<ApiResponse<ForgetPasswordDto>> ForgetPassword(ForgetPasswordDto dto)
        {
            var userId =  _httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var User= await _userManager.FindByIdAsync(userId);
            if(userId==null)
                return new ApiResponse<ForgetPasswordDto>(Success:false,Message:"please login first",Data:null
                    ,Errors:null,TraceId: Guid.NewGuid().ToString());
            var forgetPassord = await _userManager.ChangePasswordAsync(User, dto.CurrentPassword, dto.NewPassword);
            if(!forgetPassord.Succeeded)
                return new ApiResponse<ForgetPasswordDto>(Success: false, Message: "the operation not completed", Data: null
                   , Errors: forgetPassord.Errors, TraceId: Guid.NewGuid().ToString());

            return new ApiResponse<ForgetPasswordDto>(Success: false, Message: "the password is change success", Data: null
                   , Errors: null, TraceId: Guid.NewGuid().ToString());
        }

        public async Task<string> ChangePassword(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.email);
            if (user == null)
                return "If the email exists, a reset link will be sent.";

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //expire the token
            

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token)
            );

            var request = _httpContext.HttpContext!.Request;

            var resetUrl =
                $"{request.Scheme}://{request.Host}/api/Auth/ResetPassword" +
                $"?email={user.Email}&token={encodedToken}";

            await _emailService.SendResetPasswordEmailAsync(
                user.Email,
                user.UserName!,
                resetUrl
            );

            return "Check your email for reset password link.";
        }


        public async  Task<string> ResetPassword(string email,string token, string NewPassowrd)
        {
            var user =await  _userManager.FindByEmailAsync(email);
            if (user == null)
                return "not found user";
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var checktoken = await _userManager.ResetPasswordAsync(user, decodedToken, NewPassowrd);
            if (!checktoken.Succeeded)
                return   checktoken.Errors.Select(x => x.Description).ToString();

            return "the Password Changed Success";
        }
    }
}
