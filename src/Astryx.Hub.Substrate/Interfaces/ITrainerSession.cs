using Astryx.Abstractions.Agents;
using Astryx.Hub.Substrate.Models;

namespace Astryx.Hub.Substrate.Interfaces
{
    public interface ITrainerSession
    {
        string SessionId { get; }

        TrainerStatus GetStatus();

        // Step the runtime N times (delegates to RuntimeService.StepMany)
        void Step(int steps);

        // Export the final bundle from the runtime (AgentExportBundle)
        AgentExportBundle Export();
    }
}