using System.Threading.Tasks;

namespace EventBus.Common.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
