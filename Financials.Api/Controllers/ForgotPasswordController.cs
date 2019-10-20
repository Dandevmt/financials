using Financials.Application;
using Financials.Application.CQRS;
using Financials.Application.UserManagement.UseCases;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {

        private readonly Dispatcher dispatcher;

        public ForgotPasswordController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string email)
        {
            await dispatcher.Dispatch(new ForgotPasswordCommand(email));
            return Ok();
        }
    }
}
