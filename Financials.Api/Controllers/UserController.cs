using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.UserManagement.Codes;
using Financials.Application.Configuration;
using Financials.Application.Errors;
using Financials.Application.UserManagement.Security;
using Financials.Application.UserManagement;
using Financials.Application.UserManagement.UseCases;
using Financials.Database;
using Financials.Entities;
using Financials.Infrastructure.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Financials.Application.CQRS;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase 
    {
        private readonly Dispatcher dispatcher;

        public UserController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<User> Get(string id)
        {
            return new User();
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddUserCommand input)
        {
            var result = dispatcher.Dispatch(input);
            return Ok(result);
        }
    }
}