using System;
using AgentTrainerHub.Config;

namespace AgentTrainerHub.Interfaces
{
    public interface IAgentTrainerHub
    {
        Guid CreateAgent(MindConfig mind);
        AgentExportBundle StepAgent(Guid agentId, int steps);
        MindConfig CreateMind(AgentTrainerConfig config);
    }
}
