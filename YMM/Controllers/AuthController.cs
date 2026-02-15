using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _Auth;
        public AuthController(IAuthService authService)
        {
            _Auth = authService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestDto dto)
        {
            if(ModelState.IsValid)
            {
                var regist=await _Auth.RegisterAsync(dto);
                if(regist == null) 
                    return BadRequest(regist);
                return Ok(regist);
            }
            return BadRequest(ModelState.Select(x => x.Value));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var login = await _Auth.LoginAsync(dto);
                if (login == null)
                    return BadRequest(login);
                return Ok(login);
            }
            return BadRequest(ModelState.Select(x => x.Value));
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            if (ModelState.IsValid)
            {
                var rfToken = await _Auth.RefreshTokenAsync(token);
                if (rfToken == null)
                    return BadRequest(rfToken);
                return Ok(rfToken);
            }
            return BadRequest(ModelState.Select(x => x.Value));
        }
        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(string email,string otp)
        {
            if (ModelState.IsValid)
            {
                var rfToken = await _Auth.VerifyOtpAsync(email,otp);
                if (rfToken == null)
                    return BadRequest(rfToken);
                return Ok(rfToken);
            }
            return BadRequest(ModelState.Select(x => x.Value));
        }
        //[HttpPost("ResendOtp")]
        //public async Task<IActionResult> ResendOtp(string email)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var rfToken = await _Auth.ReSendOtpCodeAsync(email);
        //        if (rfToken == null)
        //            return BadRequest(rfToken);
        //        return Ok(rfToken);
        //    }
        //    return BadRequest(ModelState.Select(x => x.Value));
        //}
        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequestDto dto)
        {
            if(ModelState.IsValid)
            {
                var verifyEmail= await _Auth.VerifyEmailAsync(dto);
                return Ok(verifyEmail);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromForm] ForgotPasswordRequestDto dto)
        {
            if(ModelState.IsValid)
            {
                var fPassword= await _Auth.ForgotPasswordAsync(dto);
                return Ok(fPassword);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromQuery] ResetPasswordRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var cPassword = await _Auth.ResetPasswordAsync(dto);
                return Ok(cPassword);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequestDto dto)
        {
            if(ModelState.IsValid)
            {
                var rPassword= await _Auth.ResetPasswordAsync(dto);
                return Ok(rPassword);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("ResetPassword")]
        public IActionResult ResetPasswordPage([FromQuery] string email, [FromQuery] string token)
        {
            if(ModelState.IsValid)
            {
                // Here you would typically return a view for resetting the password.
                // Since this is an API controller, we can just return a message.
                return Ok(new { Email = email, Token = token, Message = "Render reset password page here." });
            }
            return BadRequest(ModelState);
        }
    }
}
