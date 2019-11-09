using Financials.CQRS;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financials.Api.DependencyInjection
{
    public class ProviderWrapper : IProvider
    {
        private readonly Container container;

        public ProviderWrapper(Container container)
        {
            this.container = container;
        }

        public object GetService(Type type)
        {
            return container.GetInstance(type);
        }
    }
}
