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
    public class VerifyEmailController : ControllerBase
    {
        private readonly IUseCase<VerifyEmailCommand, bool> verifyEmailUseCase;

        public VerifyEmailController(IUseCase<VerifyEmailCommand, bool> verifyEmailUseCase)
        {
            this.verifyEmailUseCase = verifyEmailUseCase;
        }

        [HttpGet]
        public bool VerifyEmail(Guid userId, string code)
        {
            bool verified = false;
            verifyEmailUseCase.Handle(new VerifyEmailCommand() { UserId = userId, Code = code }, ver => 
            {
                verified = ver;            
            });
            return verified;
        }
    }
}