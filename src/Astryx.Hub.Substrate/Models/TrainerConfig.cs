namespace Astryx.Hub.Substrate.Models
{
    public class TrainerConfig
    {
        public string AgentName { get; set; } = string.Empty;

        // Seed used to drive deterministic identity / runtime setup
        public int Seed { get; set; }

        // Steps per training batch
        public int StepsPerBatch { get; set; } = 1;

        // Optional path to persona/profile JSON (if using import)
        public string? PersonaConfigPath { get; set; }
    }
}