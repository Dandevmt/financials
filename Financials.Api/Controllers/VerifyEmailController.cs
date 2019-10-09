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
    public class VerifyEmailController : ControllerBase
    {
        private readonly IUseCase<VerifyEmailInput, bool> verifyEmailUseCase;

        public VerifyEmailController(IUseCase<VerifyEmailInput, bool> verifyEmailUseCase)
        {
            this.verifyEmailUseCase = verifyEmailUseCase;
        }

        [HttpGet]
        public bool VerifyEmail(Guid userId, string code)
        {
            bool verified = false;
            verifyEmailUseCase.Handle(new VerifyEmailInput() { UserId = userId, Code = code }, ver => 
            {
                verified = ver;            
            });
            return verified;
        }
    }
}