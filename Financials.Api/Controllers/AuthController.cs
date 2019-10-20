using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.UserManagement.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUseCase<LoginCommand, string> loginUseCase;

        public AuthController(IUseCase<LoginCommand, string> loginUseCase)
        {
            this.loginUseCase = loginUseCase;
        }


        [HttpPost]
        public string CreateToken([FromBody] LoginCommand login)
        {
            string jwtToken = null;
            loginUseCase.Handle(login, token => jwtToken = token);
            return jwtToken;
        }
    }
}