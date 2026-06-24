using Astryx.Hub.Substrate.Models;

namespace Astryx.Hub.Substrate.Interfaces
{
    public interface IHubRuntime
    {
        // Initialize the host runtime (plugin directory, hot reload, etc.)
        void Initialize(HubRuntimeConfig config);

        // Create a trainer session with a given config
        ITrainerSession CreateSession(TrainerConfig config);

        // Host-level update (e.g., per-frame tick if you want deltaTime)
        void Update(float deltaTime);
    }
}