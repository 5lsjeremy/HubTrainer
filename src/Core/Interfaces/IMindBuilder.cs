using AgentTrainer.Core.Config;
using Astryx.Abstractions.Agents;

namespace AgentTrainer.Core.Interfaces
{
    public interface IMindBuilder
    {
        MindConfig BuildMind(AgentTrainerConfig config);
    }
}

