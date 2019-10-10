using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.Users.UseCases;
using Financials.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterUserController : ControllerBase
    {
        private readonly IUseCase<RegisterUserInput, User> registerUserUseCase;

        public RegisterUserController(IUseCase<RegisterUserInput, User> registerUserUseCase)
        {
            this.registerUserUseCase = registerUserUseCase;
        }

        [HttpPost]
        public async Task<User> Register([FromBody] RegisterUserInput input)
        {
            User user = null;
            await registerUserUseCase.Handle(input, u => user = u);
            return user;
        }
    }
}