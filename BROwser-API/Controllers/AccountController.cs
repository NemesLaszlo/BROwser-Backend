﻿using Application.Email;
using BROwser_API.DTOs;
using BROwser_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BROwser_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private const string EMAIL_VALIDATION_PATTERN = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))){2,63}\.?$";

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;
        private readonly ITokenService _tokenService;
        private readonly EmailSender _emailSender;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager, ITokenService tokenService, EmailSender emailSender)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Email form validation
        /// </summary>
        /// <param name="email">Email Address</param>
        /// <returns>It is valid or not</returns>
        private bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var pattern = EMAIL_VALIDATION_PATTERN;
            const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

            var emailValidator = new Regex(pattern, options);

            return emailValidator.IsMatch(email);
        }

        /// <summary>
        /// UserDTO creation to return with the must have and needed user information
        /// ( with the token )
        /// </summary>
        /// <param name="user">AppUser</param>
        /// <returns>UserDTO</returns>
        private async Task<UserDTO> CreateUserObjectAsync(AppUser user)
        {
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = await  _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }

        /// <summary>
        /// Generate refresh token to the user
        /// </summary>
        /// <param name="user">AppUser</param>
        /// <returns></returns>
        private async Task SetRefreshToken(AppUser user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        /// <summary>
        /// User logging in
        /// </summary>
        /// <param name="loginDto">Login infromations</param>
        /// <returns>UserDTO</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            if (!IsValidEmailAddress(loginDto.Email))
            {
                ModelState.AddModelError("email", "Invalid Email Form");
                return ValidationProblem();
            }

            var user = await _userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null)
            {
                ModelState.AddModelError("email", "Invalid Email");
                return ValidationProblem();
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("email", "Email not confirmed");
                return ValidationProblem();
            }

            var result = await _singInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("password", "Invalid Password");
                return ValidationProblem();
            }

            await SetRefreshToken(user);
            return await CreateUserObjectAsync(user);
        }

        /// <summary>
        /// New User registration
        /// </summary>
        /// <param name="registerDto">Datas to register a new user</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            if (!IsValidEmailAddress(registerDto.Email))
            {
                ModelState.AddModelError("email", "Invalid Email Form");
                return ValidationProblem();
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email Taken");
                return ValidationProblem();
            }

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "Username Taken");
                return ValidationProblem();
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username,
                DateOfBirth = registerDto.DateOfBirth
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            // Email verification - email send part
            var origin = Request.Headers["origin"]; // request comes from
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var verifyUrl = $"{origin}/account/verifyEmail?token={token}&email={user.Email}";
            var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>Click to verify email</a></p>";

            await _emailSender.SendEmailAsync(user.Email, "Please verify email", message);

            return Ok();
        }

        /// <summary>
        /// Verify email address after registration
        /// </summary>
        /// <param name="token">Token for the verification</param>
        /// <param name="email">Email to verify</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("verifyEmail")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized();

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("email", "Could not verify email address");
                return ValidationProblem();
            }

            return Ok();
        }

        /// <summary>
        /// Resend the verification email
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("resendEmailConfirmationLink")]
        public async Task<IActionResult> ResendEmailConfirmationLink(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized();

            var origin = Request.Headers["origin"]; // request comes from
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var verifyUrl = $"{origin}/account/verifyEmail?token={token}&email={user.Email}";
            var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>Click to verify email</a></p>";

            await _emailSender.SendEmailAsync(user.Email, "Please verify email", message);

            return Ok();
        }

        /// <summary>
        /// Password reset
        /// </summary>
        /// <param name="token">Token for the password reset</param>
        /// <param name="email">Email to verify user and reset his/him password</param>
        /// <param name="resetDto">New password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("passwordReset")]
        public async Task<IActionResult> PasswordReset(string token, string email, PasswordResetDTO resetDto)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized();

            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, resetDto.newPassword);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("password", "Could not reset password");
                return ValidationProblem();
            }

            return Ok();
        }

        /// <summary>
        /// Resend password reset email
        /// </summary>
        /// <param name="email">Email to reset user password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("passwordResetRequest")]
        public async Task<IActionResult> PasswordResetRequest(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized();

            var origin = Request.Headers["origin"]; // request comes from
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var resetUrl = $"{origin}/account/passwordReset?token={token}&email={user.Email}";
            var message = $"<p>Please click the below link to reset your password:</p><p><a href='{resetUrl}'>Click to reset password</a></p>";

            await _emailSender.SendEmailAsync(user.Email, "Password reset", message);

            return Ok();
        }

        /// <summary>
        /// Actual logged in user information request with the token.
        /// </summary>
        /// <returns>UserDTO</returns>
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
            return await CreateUserObjectAsync(user);
        }

        /// <summary>
        /// Generate new Refrest token to the logged in user
        /// </summary>
        /// <returns></returns>
        [HttpPost("refreshToken")]
        public async Task<ActionResult<UserDTO>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _userManager.Users
                .Include(r => r.RefreshTokens)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            if (user == null) return Unauthorized();

            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (oldToken != null && !oldToken.IsActive) return Unauthorized();

            return await CreateUserObjectAsync(user);
        }

    }
}
