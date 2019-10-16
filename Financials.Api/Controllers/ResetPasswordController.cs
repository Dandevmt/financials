using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.Users.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly IUseCase<ResetPasswordInput, bool> useCase;

        public ResetPasswordController(IUseCase<ResetPasswordInput, bool> useCase)
        {
            this.useCase = useCase;
        }

        [HttpPost]
        public async Task<bool> Post([FromBody]ResetPasswordInput input)
        {
            bool success = false;
            await useCase.Handle(input, s => { success = s; });
            return success;
        }
    }
}