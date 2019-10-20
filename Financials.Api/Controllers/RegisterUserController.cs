using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.CQRS;
using Financials.Application.UserManagement.UseCases;
using Financials.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterUserController : ControllerBase
    {
        private readonly Dispatcher dispatcher;

        public RegisterUserController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<CommandResult> Register([FromBody] RegisterUserCommand input)
        {
            return await dispatcher.Dispatch(input); ;
        }
    }
}