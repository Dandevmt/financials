﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application;
using Financials.Application.UserManagement.Codes;
using Financials.Application.Configuration;
using Financials.Application.UserManagement.Security;
using Financials.Application.UserManagement;
using Financials.Application.UserManagement.Commands;
using Financials.Database;
using Financials.Entities;
using Financials.Infrastructure.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Financials.CQRS;
using Financials.Dto;
using Financials.Application.UserManagement.Queries;
using System.Text.Json;

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
        public async Task<CommandResult<UserForTenantDto>> Get(string tenantId, string id)
        {
            return await dispatcher.Dispatch<GetUserQuery, UserForTenantDto>(new GetUserQuery(id, tenantId));
        }

        [HttpGet]
        [Route("all")]
        public async Task<CommandResult<IEnumerable<UserForTenantDto>>> GetAll(string tenantId)
        {
            try
            {
                var res  = await dispatcher.Dispatch<GetAllUsersQuery, IEnumerable<UserForTenantDto>>(new GetAllUsersQuery(tenantId));
                return res;
            } catch (Exception ex)
            {
                throw ex;
            }
            
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
                Zip = input.Zip,
                ValidateOnly = input.ValidateOnly,
                TenantId = input.TenantId
            });
        }
    }
}