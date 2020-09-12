using System.Threading.Tasks;
using Foundation.EventBus.Events;

namespace Identity.API.IntegrationEvents
{
    public interface IIdentityntegrationEventService
    {
        Task SaveEventAndIdentityContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}