using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.Application.UserManagement.Commands;
using Financials.CQRS;
using Financials.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddTenantToUserController : ControllerBase
    {
        private readonly Dispatcher dispatcher;

        public AddTenantToUserController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<CommandResult> Post([FromBody] AddTenantToUserDto dto)
        {
            return await dispatcher.Dispatch(new AddTenantToUserCommand(dto.TenantId, dto.FederationCode, dto.ValidateOnly));
        }
    }
}