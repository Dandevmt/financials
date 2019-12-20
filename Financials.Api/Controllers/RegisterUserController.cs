using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.CQRS;
using Financials.Dto;
using Financials.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Financials.UserManagement;

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
        public Result<string> Register([FromBody] RegisterUserDto input)
        {
            return dispatcher.Command<RegisterUserCommand,string>(new RegisterUserCommand() 
            {
                //City = input.City,
                //Country = input.Country,
                Username = input.Username,
                Email = input.Email,
                Email2 = input.Email2,
                //Phone = input.Phone,
                //FirstName = input.FirstName,
                //LastName = input.LastName,
                Password = input.Password,
                Password2 = input.Password2,
                //State = input.State,
                //Street = input.Street,
                //Zip = input.Zip
            });
        }
    }
}