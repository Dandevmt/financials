using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.CQRS;
using Financials.Application.UserManagement.Commands;
using Financials.Dto;
using Financials.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterUserController : ControllerBase
    {
        private readonly Dispatcher dispatcher;

        public RegisterUserController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<CommandResult> Register([FromBody] RegisterUserDto input)
        {
            return await dispatcher.Dispatch(new RegisterUserCommand() 
            {
                City = input.City,
                Country = input.Country,
                Email = input.Email,
                Phone = input.Phone,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Password = input.Password,
                Password2 = input.Password2,
                State = input.State,
                Street = input.Street,
                Zip = input.Zip,
                ValidateOnly = input.ValidateOnly
            });
        }
    }
}