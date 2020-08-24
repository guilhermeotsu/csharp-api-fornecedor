using DevIO.API.Controllers;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.V1.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/playground")]
    public class TesteController : MainController
    {
        public TesteController(
            INotifier notifier,
            IUser user) : base(notifier, user) { }

        [HttpGet]
        public string Valor() => "v1 version";
    }
}
