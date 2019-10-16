using Financials.Application;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {

        private readonly IUseCase<string, bool> resetUseCase;

        public ForgotPasswordController(IUseCase<string, bool> resetUseCase)
        {
            this.resetUseCase = resetUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string email)
        {
            await resetUseCase.Handle(email, emailSent => { });
            return Ok();
        }
    }
}
