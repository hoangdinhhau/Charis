using Charis.Charis.ModelView.Catalog.UserModel;
using Charis.ModelView.Catalog.UserModel;
using Charis.Utilities.Exceptions;
using Charis.WebApp.Services.UserService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Charis.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserClientService _userClientService;
        private readonly IConfiguration _configuration;

        public UserController(IUserClientService userClientService, IConfiguration configuration)

        {
            _userClientService = userClientService;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var token = await _userClientService.Login(loginRequest);
            var userPrincipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString(SystemContants.AppSettings.Token, token);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties
                );
            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        [Route("facebook-login")]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        [Route("facebook-response")]
        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value,
                });
            var userCreate = new UserCreateRequest()
            {
                Email = result.Principal.FindFirstValue(ClaimTypes.Email),
                FullName = result.Principal.FindFirstValue(ClaimTypes.Name),
                BirthDay = Convert.ToDateTime("01/01/2000"),
                Address = "",
                PassWord = "",
                RoleId = 3
            };
            var user = await _userClientService.GetByEmail(userCreate.Email);
            if (user == null)
            {
                await _userClientService.CreateUser(userCreate);
            }
            var loginRequest = new LoginRequest()
            {
                Email = userCreate.Email,
                Password = userCreate.PassWord
            };
            var token = await _userClientService.Login(loginRequest);
            HttpContext.Session.SetString(SystemContants.AppSettings.Token, token);

            var userPrincipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties
                );
            return RedirectToAction("Index", "Home");
        }

        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value,
                });
            var userCreate = new UserCreateRequest()
            {
                Email = result.Principal.FindFirstValue(ClaimTypes.Email),
                FullName = result.Principal.FindFirstValue(ClaimTypes.Name),
                BirthDay = Convert.ToDateTime("01/01/2000"),
                Address = "",
                PassWord = "",
                RoleId = 3
            };
            var user = await _userClientService.GetByEmail(userCreate.Email);
            if (user == null)
            {
                await _userClientService.CreateUser(userCreate);
            }
            var loginRequest = new LoginRequest()
            {
                Email = userCreate.Email,
                Password = userCreate.PassWord
            };
            var token = await _userClientService.Login(loginRequest);
            HttpContext.Session.SetString(SystemContants.AppSettings.Token, token);
            var userPrincipal = this.ValidateToken(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties
                );
            return RedirectToAction("Index", "Home");
        }
    }
}