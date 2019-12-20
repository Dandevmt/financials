using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.CQRS;
using Financials.Application.UserManagement.Commands;
using Financials.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly Dispatcher dispatcher;

        public ResetPasswordController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<Result> Post([FromBody]ResetPasswordDto input)
        {
            return await dispatcher.Dispatch(new ResetPasswordCommand() 
            {
                UserId = input.UserId,
                NewPassword = input.NewPassword,
                NewPassword2 = input.NewPassword2,
                OldPassword = input.OldPassword,
                ResetCode = input.ResetCode,
                ValidateOnly = input.ValidateOnly
            });
        }
    }
}