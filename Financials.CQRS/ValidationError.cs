using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Financials.CQRS
{
    public class ValidationError : Error, IError
    {
        public IDictionary<string, IList<string>> FieldErrors { get; }
        public bool HasError 
        {
            get { return FieldErrors.Count > 0; }
        }

        protected ValidationError(int code, string message) : base(code, message)
        {
            FieldErrors = new Dictionary<string, IList<string>>();
        }

        public ValidationError AddError(string field, string error)
        {
            string fieldCamel = char.ToLowerInvariant(field[0]) + field.Substring(1);
            if (FieldErrors.TryGetValue(fieldCamel, out IList<string> list))
            {
                list.Add(error);
            } else
            {
                FieldErrors.Add(fieldCamel, new List<string> { error });
            }
            return this;
        }

        public static ValidationError New() =>
            new ValidationError(400, "Validation");
                
    }
}
