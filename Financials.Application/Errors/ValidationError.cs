using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Errors
{
    public class ValidationError : Error
    {
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();

        protected ValidationError(ErrorCode code, string message) : base(code, message)
        {
        }

        public void ThrowIfError()
        {
            bool hasErrors = Errors.Count > 0;
            if (hasErrors)
                throw new ErrorException(this);
        }

        public ValidationError AddError(string fieldName, string message)
        {
            if (Errors.TryGetValue(fieldName, out List<string> messages))
            {
                if (messages == null)
                    messages = new List<string>();

                messages.Add(message);
            } else
            {
                Errors.Add(fieldName, new List<string>());
                Errors[fieldName].Add(message);
            }
            return this;
        }

        public override string ToString()
        {
            StringBuilder validationMessages = new StringBuilder();
            foreach(var kvp in Errors)
            {
                if (validationMessages.Length > 0)
                    validationMessages.Append(",");
                string errors = "\"" + string.Join("\",\"", kvp.Value) + "\"";
                validationMessages.Append($"{{\"field\":\"{kvp.Key}\",\"errors\":[{errors}]}}");
            }
            return $"{{\"code\":{(int)Code},\"message\":\"{Message}\",\"description\":\"{Description}\",\"validationErrors\":[{validationMessages}]}}";
        }

        public static ValidationError New()
        {
            return new ValidationError(ErrorCode.Validation, "Validation");
        }
    }
}
