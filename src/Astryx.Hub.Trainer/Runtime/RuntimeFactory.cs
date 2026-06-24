using Astryx.Abstractions.Runtime;
using Astryx.Engine;
using Astryx.Runtime;
using Astryx.Runtime.Service;

namespace Astryx.Hub.Trainer.Runtime
{
    public static class RuntimeFactory
    {
        public static (IRuntimeOrchestrator orchestrator, IRuntimeService runtimeService)
            CreateRuntime()
        {
            var bootstrapper = new RuntimeBootstrapper();

            // Create the orchestrator using the real runtime pipeline
            var orchestrator = bootstrapper.CreateOrchestrator();

            // ⭐ Extract the runtime service from the orchestrator
            var runtimeService = orchestrator.RuntimeService;

            return (orchestrator, runtimeService);
        }
    }
}