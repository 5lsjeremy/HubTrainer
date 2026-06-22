using System;
using AgentTrainer.Core.Interfaces;
using Astryx.Abstractions.Agents;

namespace AgentTrainer.Core.Runtime
{
    public sealed class AgentTrainingSession : IAgentTrainingSession
    {
        public Guid AgentId { get; }

        public AgentTrainingSession(Guid id)
        {
            AgentId = id;
        }

        public AgentExportBundle Step(int steps)
        {
            // TODO: Hook into runtime/substrate
            return new AgentExportBundle();
        }
    }
}

