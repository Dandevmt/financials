using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.CQRS;
using Financials.Application.UserManagement.Commands;
using Financials.Application.UserManagement.Queries;
using Financials.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Dispatcher dispatcher;

        public AuthController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }


        [HttpPost]
        public async Task<CommandResult> CreateToken([FromBody] LoginDto login)
        {
            return await dispatcher.Dispatch(new LoginCommand(login.Email, login.Password));
        }

        [HttpGet]
        public async Task<CommandResult<UserDto>> GetLoggedInUser()
        {
            var t = await dispatcher.Dispatch<GetCurrentUserQuery, UserDto>(new GetCurrentUserQuery());
            return t;
        }
    }
}