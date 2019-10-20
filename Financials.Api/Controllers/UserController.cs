using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.UserManagement.Codes;
using Financials.Application.Configuration;
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
using Financials.Dto;

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
        public async Task<CommandResult> Post([FromBody] AddUserDto input)
        {
            return await dispatcher.Dispatch(new AddUserCommand() 
            {
                City = input.City,
                Country = input.Country,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                State = input.State,
                Street = input.Street,
                Zip = input.Zip
            });
        }
    }
}