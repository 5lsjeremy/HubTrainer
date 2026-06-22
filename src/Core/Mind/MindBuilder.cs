using System;
using AgentTrainer.Core.Config;
using AgentTrainer.Core.Interfaces;
using Astryx.Abstractions.Agents;

namespace AgentTrainer.Core.Mind
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

