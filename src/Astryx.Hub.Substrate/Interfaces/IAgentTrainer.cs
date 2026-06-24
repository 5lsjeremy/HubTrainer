using Astryx.Hub.Substrate.Models;

namespace Astryx.Hub.Substrate.Interfaces
{
    public interface IAgentTrainer
    {
        // Initialize trainer with config (seed, steps, persona path, etc.)
        void Initialize(TrainerConfig config);

        // Create a new training session (runtime + actor)
        ITrainerSession CreateSession();

        // High-level trainer status (state, step, export presence, error)
        TrainerStatus GetStatus();
    }
}