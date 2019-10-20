using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
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
        private readonly IUseCase<RegisterUserCommand, User> registerUserUseCase;

        public RegisterUserController(IUseCase<RegisterUserCommand, User> registerUserUseCase)
        {
            this.registerUserUseCase = registerUserUseCase;
        }

        [HttpPost]
        public async Task<User> Register([FromBody] RegisterUserCommand input)
        {
            User user = null;
            await registerUserUseCase.Handle(input, u => user = u);
            return user;
        }
    }
}