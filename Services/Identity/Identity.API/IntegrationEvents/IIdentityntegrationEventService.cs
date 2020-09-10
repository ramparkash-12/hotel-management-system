using System.Threading.Tasks;
using Foundation.EventBus.Events;

namespace Identity.API.IntegrationEvents
{
    public interface IIdentityntegrationEventService
    {
        void PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}