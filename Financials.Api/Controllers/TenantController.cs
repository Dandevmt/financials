using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.CQRS;
using Financials.Application.UserManagement.Commands;
using Financials.Application.UserManagement.Queries;
using Financials.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly Dispatcher dispatcher;

        public TenantController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpPost]
        public async Task<CommandResult> Add([FromBody] AddTenantDto input)
        {
            return await dispatcher.Dispatch(new AddTenantCommand(input.Id, input.Name));
        }

        [HttpGet]
        public async Task<CommandResult<TenantDto>> Get(string id)
        {
            return await dispatcher.Dispatch<GetTenantQuery, TenantDto>(new GetTenantQuery(id));
        }

        [HttpGet]
        public async Task<CommandResult<IList<TenantDto>>> Get()
        {
            return await dispatcher.Dispatch<GetAllTenantsQuery, IList<TenantDto>>(new GetAllTenantsQuery());
        }
    }
}