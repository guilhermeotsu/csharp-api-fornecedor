using System;
using System.Collections.Generic;
using System.Security.Claims;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DevIO.API.Extensions
{
  // Representacao no usuario logado
  public class AspNetUser : IUser
  {
      private readonly IHttpContextAccessor _accessor; // Utilizamos essa  interface para acessar o HttpContext em um servico
      public AspNetUser(IHttpContextAccessor accessor)
      {
        _accessor = accessor;   
      }

      public string Name => _accessor.HttpContext.User.Identity.Name;

      public Guid GetUserId()
      {
          return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
      }

      public string GetUserEmail() 
      {
          return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : null;
      }

      public bool IsAuthenticated() 
      {
          return _accessor.HttpContext.User.Identity.IsAuthenticated;
      }

      public bool IsInRole(string role) 
      {
          return _accessor.HttpContext.User.IsInRole(role);
      }

    public IEnumerable<Claim> GetClaimsIdentity()
    {
      throw new NotImplementedException();
    }
  }

  public static class ClaimsPrincipalExtension
  {
      public static string GetUserId(this ClaimsPrincipal principal)
      {
          if(principal == null) throw new ArgumentException(nameof(principal));

          var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
          return claim?.Value;
      }

      public static string GetUserEmail(this ClaimsPrincipal principal)
      {
          if(principal == null) throw new ArgumentException(nameof(principal));

          var claim = principal.FindFirst(ClaimTypes.Email);
          return claim?.Value;
      }
  }
}