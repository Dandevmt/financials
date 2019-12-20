using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Financials.Common.Email
{
    public abstract class EmailMessage
    {
        public virtual string To { get; set; }
        public virtual string From { get; set; } = "donotreply@ofbbutte.com";
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual Dictionary<string, string> AdditionalValues { get; set; }
        public abstract string Template { get; set; }

        public void SetBodyFromTemplate(bool isTest = true)
        {
            try
            {
                string template = Template;
                var props = GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).ToDictionary(p => p.Name, p => p.GetValue(this)?.ToString() ?? "");
                foreach (var property in props)
                {
                    template = template.Replace($"{{{{{property.Key}}}}}", property.Value);
                }
                if (AdditionalValues != null)
                {
                    foreach (var kvp in AdditionalValues)
                    {
                        template = template.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);
                    }
                }
                Body = template;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
