using System;

namespace AgentTrainerHub.Interfaces
{
    public interface IAgentTrainingSession
    {
        Guid AgentId { get; }
        AgentExportBundle Step(int steps);
    }
}
