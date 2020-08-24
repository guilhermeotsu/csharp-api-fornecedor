using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevIO.API.Data
{
    // Contexto especificado para o Identity
    public class ApplicationDbContext : IdentityDbContext
    {
        // Sempre que tiver mais de um contexto na aplicacao e necessario passar o tipo do ContextOptions
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}