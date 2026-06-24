using Astryx.Abstractions.Math;
using Astryx.Abstractions.Provenance;
using Astryx.Abstractions.Runtime;
using Astryx.Abstractions.Substrate.Exporting;
using Astryx.Abstractions.Substrate.Schemas;
using Astryx.Runtime.Service;

namespace Astryx.Hub.Trainer.Runtime
{
    public sealed class TrainerBootstrapper
    {
        private readonly IAstryxMath _math;
        private readonly IProvenanceManager _provenance;
        private readonly IMindExporter _mindExporter;
        private readonly IMindSchema _mindSchema;

        public TrainerBootstrapper(
            IAstryxMath math,
            IProvenanceManager provenance,
            IMindExporter mindExporter,
            IMindSchema mindSchema)
        {
            _math = math;
            _provenance = provenance;
            _mindExporter = mindExporter;
            _mindSchema = mindSchema;
        }

        public (IRuntimeOrchestrator orchestrator, IRuntimeService runtimeService)
            CreateRuntime()
        {
            // Delegate to the canonical runtime bootstrapper.
            // RuntimeBootstrapper wires pipeline, behavior engine, orchestrator, and runtime correctly.
            var (orchestrator, concreteRuntime) = RuntimeFactory.CreateRuntime();

            // Upcast concrete runtime to interface for TrainerSession.
            IRuntimeService runtimeService = concreteRuntime;

            return (orchestrator, runtimeService);
        }
    }
}