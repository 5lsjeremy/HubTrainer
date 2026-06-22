using AgentTrainerHub.Config;

namespace AgentTrainerHub.Interfaces
{
    public interface IMindBuilder
    {
        MindConfig BuildMind(AgentTrainerConfig config);
    }
}
