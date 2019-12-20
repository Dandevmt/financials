using System.Threading.Tasks;

namespace Financials.CQRS
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent evnt);
    }
}