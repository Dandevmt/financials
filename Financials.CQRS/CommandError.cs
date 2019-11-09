using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.CQRS
{
    public class CommandError : IError
    {
        public int Code { get; }
        public int HttpStatusCode { get; }
        public string Description { get; }
        private IList<IError> childErrors;
        public IReadOnlyList<IError> ChildErrors { get { return (IReadOnlyList<IError>)childErrors; } }

        protected CommandError(int code, int httpStatusCode, string description)
        {
            Code = code;
            HttpStatusCode = httpStatusCode;
            Description = description;
            childErrors = new List<IError>();
        }

        public IError AddError(IError error)
        {
            childErrors.Add(error);
            return this;
        }

        
        public static CommandError Forbidden(string description = null) => 
            new CommandError(403, 403, description ?? "Permission Denied");
        
    }
}
