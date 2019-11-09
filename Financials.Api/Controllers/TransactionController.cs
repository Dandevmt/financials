using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financials.CQRS;
using Financials.Application.Transactions.Commands;
using Financials.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly Dispatcher dispatcher;

        public TransactionController(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        [HttpPost]
        public Task<CommandResult> Post([FromBody]AddTransactionDto input)
        {
            return dispatcher.Dispatch(new AddTransactionCommand() 
            {
                Amount = input.Amount,
                CheckNumber = input.CheckNumber,
                Date = input.Date,
                Description = input.Description,
                GoodsOrServicesGiven = input.GoodsOrServicesGiven,
                Type = (Application.Transactions.Commands.TransactionType)input.Type,
                UserId = input.UserId
            });
        }
    }
}