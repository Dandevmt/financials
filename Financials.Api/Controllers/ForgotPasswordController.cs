﻿using Financials.Application;
using Financials.CQRS;
using Financials.Application.UserManagement.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Dto;

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
        public async Task<IActionResult> Post([FromBody] ForgotPasswordDto email)
        {
            await dispatcher.Dispatch(new ForgotPasswordCommand(email.Email));
            return Ok();
        }
    }
}
