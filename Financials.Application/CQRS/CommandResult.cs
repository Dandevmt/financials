using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.CQRS
{
    public class CommandResult
    {
        public bool IsSuccess { get; protected set; }
        public CommandError Error { get; protected set; }

        public static CommandResult Success()
        {
            return new CommandResult()
            {
                IsSuccess = true
            };
        }

        public static CommandResult Fail()
        {
            return new CommandResult()
            {
                IsSuccess = false
            };
        }

        public static CommandResult Fail(CommandError error)
        {
            return new CommandResult()
            {
                IsSuccess = false,
                Error = error
            };
        }

        public CommandResult AddError(CommandError error)
        {
            Error.AddError(error);
            return this;
        }

        public Task<CommandResult> AsTask()
        {
            return Task.FromResult(this);
        }
    }

    public class CommandResult<TData> : CommandResult
    {
        public TData Data { get; }

        public CommandResult(TData data, bool isSuccess)
        {
            Data = data;
            IsSuccess = isSuccess;
        }

        public static CommandResult Success(TData data)
        {
            return new CommandResult<TData>(data, true);
        }
    }
}
