﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.Security;
using Financials.Application.Security.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUseCase<LoginInput, string> loginUseCase;

        public AuthController(IUseCase<LoginInput, string> loginUseCase)
        {
            this.loginUseCase = loginUseCase;
        }


        [HttpPost]
        public string CreateToken([FromBody] LoginInput login)
        {
            string jwtToken = null;
            loginUseCase.Handle(login, token => jwtToken = token);
            return jwtToken;
        }
    }
}