using System;
using AgentTrainerHub.Config;
using AgentTrainerHub.Interfaces;
using Astryx.Abstractions.Substrate.Config;

namespace AgentTrainerHub.Mind
{
    public sealed class MindBuilder : IMindBuilder
    {
        public MindConfig BuildMind(AgentTrainerConfig config)
        {
            // TODO: Fill in using MindConfig fields
            return new MindConfig
            {
                Id = Guid.NewGuid().ToString(),
                Name = config.Name,
                Version = "1.0.0"
            };
        }
    }
}
