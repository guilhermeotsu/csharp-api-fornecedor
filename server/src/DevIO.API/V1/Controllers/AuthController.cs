using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DevIO.API.Controllers;
using DevIO.API.Extensions;
using DevIO.API.ViewModels;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevIO.API.V1.Controllers
{
    /*
      Para customizar mensagens de erro basta criar um classe herdando de IdentityErrorDescriber e Adicionar dentro das configs do Identity
    */
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/account")]
    public class AuthController : MainController
    {
        // Faz o trabalho de fazer a autenticacao do usuario
        private readonly SignInManager<IdentityUser> _signInManager;

        // Responsavel pela criacao do usuario
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings,
            INotifier notifier,
            IUser user
        ) : base(notifier, user)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(
            RegisterUserViewModel registerUserViewModel
        )
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            // criando um Identity User
            var user = new IdentityUser
            {
                UserName = registerUserViewModel.Email,
                Email = registerUserViewModel.Email,
                EmailConfirmed = true
            };

            // gerando o usuario 
            var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);

            if (result.Succeeded)
            {
                // Logando o usuario
                await _signInManager.SignInAsync(user, false);
                return CustomResponse(await GenerateJwt(user.Email));
            }

            foreach (var error in result.Errors)
                NotifierError(error.Description);

            return CustomResponse(registerUserViewModel);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(
          LoginUserViewModel loginUserViewModel
        )
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            // Logando apenas com o usuario (email) e senha
            var result = await _signInManager.PasswordSignInAsync(
              loginUserViewModel.Email,
              loginUserViewModel.Password,
              false,
              true // Caso o usuario tente acessar tente acessar varias vezes com dados invalidos
            );

            if (result.Succeeded) return CustomResponse(await GenerateJwt(loginUserViewModel.Email));

            if (result.IsLockedOut)
            {
                NotifierError("User blocked after invalid attempts"); ;
                return CustomResponse(loginUserViewModel);
            }

            NotifierError("User or password don't match");
            return CustomResponse(loginUserViewModel);
        }

        private async Task<LoginResponseViewModel> GenerateJwt(string email)
        {
            // Pegando os dados para adicionar no Token
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user); // Pegando as Claims do usuario (do banco)
            var userRoles = await _userManager.GetRolesAsync(user);

            // Adicionando as claims padrao
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // Id do Token
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            // Adicionando as roles no Token na colecao de claim
            foreach (var userRole in userRoles)
                claims.Add(new Claim("role", userRole));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidOn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHour),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHour).TotalHours,
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(p => new ClaimViewModel { Type = p.Type, Value = p.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(
            year: 1970,
            month: 1,
            day: 1,
            hour: 0,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
          )).TotalSeconds);
    }
}
