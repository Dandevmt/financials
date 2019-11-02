using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.CQRS
{
    public class CommandError: IError
    {
        public int Code { get; }
        public int HttpStatusCode { get; }
        public string Description { get; }
        private IList<CommandError> childErrors;
        public IReadOnlyList<CommandError> ChildErrors { get { return (IReadOnlyList<CommandError>)childErrors; } }

        protected CommandError(int code, int httpStatusCode, string description)
        {
            Code = code;
            HttpStatusCode = httpStatusCode;
            Description = description;
            childErrors = new List<CommandError>();
        }

        public CommandError AddError(CommandError error)
        {
            childErrors.Add(error);
            return this;
        }

        
        public static CommandError Forbidden(string description = null) => 
            new CommandError(403, 403, description ?? "Permission Denied");
        
    }
}
