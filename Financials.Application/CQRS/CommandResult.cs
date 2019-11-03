using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.CQRS
{
    public class CommandResult
    {
        public bool IsSuccess { get; protected set; }
        public IError Error { get; protected set; }

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

        public virtual Task<CommandResult> AsTask()
        {
            return Task.FromResult(this);
        }
    }

    public class CommandResult<TData> : CommandResult
    {
        public TData Data { get; }

        private CommandResult(TData data, bool isSuccess)
        {
            Data = data;
            IsSuccess = isSuccess;
        }

        private CommandResult(CommandError error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static CommandResult<TData> Success(TData data)
        {
            return new CommandResult<TData>(data, true);
        }

        public new static CommandResult<TData> Fail(CommandError error)
        {
            return new CommandResult<TData>(error);
        }

        public Task<CommandResult<TData>> AsTaskTyped()
        {
            return Task.FromResult(this);
        }
    }
}
