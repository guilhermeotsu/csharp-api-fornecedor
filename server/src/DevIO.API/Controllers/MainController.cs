using System;
using System.Linq;
using DevIO.Business.Interfaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DevIO.API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notifier;
        public readonly IUser AppUser;
        protected Guid UserId { get; set; } = Guid.Empty;
        protected bool UserAuthenticated { get; set; } = false;

        protected MainController(INotifier notifier, IUser appUser)
        {
            AppUser = appUser;
            _notifier = notifier;

            if (appUser.IsAuthenticated())
            {
                UserId = appUser.GetUserId();
                UserAuthenticated = true;
            }
        }
        // Adicionando resposta para erros customizados
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                NotifierErroModelInvalid(modelState);

            return CustomResponse();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperationIsValid())
                return Ok(new
                {
                    success = true,
                    data = result
                });

            return BadRequest(new
            {
                success = false,
                errors = _notifier.GetNotifications().Select(m => m.Message)
            });
        }

        protected bool OperationIsValid()
        {
            return !_notifier.HasNotification();
        }

        protected void NotifierErroModelInvalid(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMessage = error.Exception == null ? error.ErrorMessage : error.Exception.Message;

                NotifierError(errorMessage);
            }
        }

        protected void NotifierError(string errorMessage)
        {
            _notifier.Handle(new Notification(errorMessage));
        }
    }
}