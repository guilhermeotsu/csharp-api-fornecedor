using DevIO.API.Controllers;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevIO.API.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/playground")]
    public class TesteController : MainController
    {
        private readonly ILogger _logger;
        public TesteController(
            INotifier notifier,
            ILogger<TesteController> logger,
            IUser user) : base(notifier, user)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Valor()
        {
            // KissLog - ferramenta gratuita 
            _logger.LogTrace("Log de Trace");
            _logger.LogDebug("Log de Debug");
            _logger.LogInformation("Log de Informaçao");
            _logger.LogWarning("Log de Aviso");
            _logger.LogError("Log de erro");
            _logger.LogCritical("Log problema critico");

            return "v2 version";
        }

    }
}
