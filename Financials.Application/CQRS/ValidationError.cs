using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Financials.Application.CQRS
{
    public class ValidationError : CommandError
    {
        public IDictionary<string, IList<string>> FieldErrors { get; }
        public bool HasError 
        {
            get { return FieldErrors.Count > 0; }
        }

        protected ValidationError(int code, int httpStatusCode, string description) : base(code, httpStatusCode, description)
        {
            FieldErrors = new Dictionary<string, IList<string>>();
        }

        public ValidationError AddError(string field, string error)
        {
            if(FieldErrors.TryGetValue(field, out IList<string> list))
            {
                list.Add(error);
            } else
            {
                FieldErrors.Add(field, new List<string> { error });
            }
            return this;
        }

        public static ValidationError New() =>
            new ValidationError(400, 400, "Validation");
                
    }
}
