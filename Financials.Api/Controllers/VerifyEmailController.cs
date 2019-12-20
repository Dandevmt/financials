using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.CQRS;
using Financials.Application.UserManagement.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyEmailController : ControllerBase
    {
        private readonly Dispatcher dispatcher;

        public VerifyEmailController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<Result> VerifyEmail(Guid userId, string code)
        {
            return await dispatcher.Dispatch(new VerifyEmailCommand(userId, code));
        }
    }
}