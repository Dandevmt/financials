using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.Codes;
using Financials.Application.Configuration;
using Financials.Application.Errors;
using Financials.Application.Security;
using Financials.Application.Users;
using Financials.Application.Users.UseCases;
using Financials.Database;
using Financials.Entities;
using Financials.Infrastructure.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase 
    {
        private readonly IUseCase<AddUserInput, User> addUserUseCase;

        public UserController(IUseCase<AddUserInput, User> addUserUseCase)
        {
            this.addUserUseCase = addUserUseCase;
        }

        [HttpGet]
        public async Task<User> Get(string id)
        {
            return new User();
        }

        [HttpPost, Authorize]
        public IActionResult Post([FromBody] AddUserInput input)
        {
            User user = null;
            addUserUseCase.Handle(input, u => user = u);
            return Ok(user);

        }
    }
}