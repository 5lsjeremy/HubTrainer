using System;
using AgentTrainer.Core.Config;
using Astryx.Abstractions.Agents;

namespace AgentTrainer.Core.Interfaces
{
    public interface IAgentTrainerHub
    {
        Guid CreateAgent(MindConfig mind);
        AgentExportBundle StepAgent(Guid agentId, int steps);
        MindConfig CreateMind(AgentTrainerConfig config);
    }
}

