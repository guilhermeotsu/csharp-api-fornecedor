using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DevIO.Business.Interfaces
{
    // Interface para possibilitar a interacao com o usuario logado em todas as camadas da aplicacao
    public interface IUser
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail(); 
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
    }
}