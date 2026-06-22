using System;
using Astryx.Abstractions.Agents;

namespace AgentTrainer.Core.Interfaces
{
    public interface IAgentTrainingSession
    {
        Guid AgentId { get; }
        AgentExportBundle Step(int steps);
    }
}

