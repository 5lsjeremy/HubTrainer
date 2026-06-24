using Astryx.Hub.Substrate.Enums;

namespace Astryx.Hub.Substrate.Models
{
    public class TrainerStatus
    {
        public TrainerState State { get; set; } = TrainerState.Uninitialized;

        // Mirrors ActorState.StepIndex
        public int StepIndex { get; set; }

        // True after a valid AgentExportBundle has been produced
        public bool HasExport { get; set; }

        // Step number of the last export, if any
        public int? LastExportStep { get; set; }

        // Error details if State == Error
        public string? ErrorMessage { get; set; }
    }
}