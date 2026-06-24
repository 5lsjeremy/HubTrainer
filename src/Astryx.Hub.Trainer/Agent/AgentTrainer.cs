using System;
using Astryx.Abstractions.Exceptions;
using Astryx.Abstractions.Math;
using Astryx.Abstractions.Provenance;
using Astryx.Abstractions.Substrate.Exporting;
using Astryx.Abstractions.Substrate.Schemas;
using Astryx.Hub.Substrate.Enums;
using Astryx.Hub.Substrate.Interfaces;
using Astryx.Hub.Substrate.Models;
using Astryx.Hub.Trainer.Runtime;
using Astryx.Hub.Trainer.Session;

namespace Astryx.Hub.Trainer.Agent
{
    public sealed class AgentTrainer : IAgentTrainer
    {
        private readonly TrainerBootstrapper _bootstrapper;

        private TrainerConfig? _config;
        private TrainerStatus _status = new();

        public AgentTrainer(
            IAstryxMath math,
            IProvenanceManager provenance,
            IMindExporter mindExporter,
            IMindSchema mindSchema,
            IAstryxVectorFactory vectorFactory)
        {
            if (math == null)
                throw SubstrateException.Create(
                    "IAstryxMath is null.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "AgentTrainer.ctor");

            if (provenance == null)
                throw SubstrateException.Create(
                    "IProvenanceManager is null.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "AgentTrainer.ctor");

            if (mindExporter == null)
                throw SubstrateException.Create(
                    "IMindExporter is null.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "AgentTrainer.ctor");

            if (mindSchema == null)
                throw SubstrateException.Create(
                    "IMindSchema is null.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "AgentTrainer.ctor");

            if (vectorFactory == null)
                throw SubstrateException.Create(
                    "IAstryxVectorFactory is null.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "AgentTrainer.ctor");

            // ⭐ Bootstrapper now uses IAstryxMath + vector factory
            _bootstrapper = new TrainerBootstrapper(
                math,
                provenance,
                mindExporter,
                mindSchema);
        }

        public void Initialize(TrainerConfig config)
        {
            if (config == null)
            {
                throw SubstrateException.Create(
                    "TrainerConfig is null.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "AgentTrainer.Initialize");
            }

            _config = config;

            _status = new TrainerStatus
            {
                State = TrainerState.Uninitialized,
                StepIndex = 0,
                HasExport = false,
                LastExportStep = null,
                ErrorMessage = null
            };
        }

        public ITrainerSession CreateSession()
        {
            if (_config == null)
            {
                throw SubstrateException.Create(
                    "Trainer not initialized.",
                    SubstrateErrorLevel.Critical,
                    SubstrateErrorCode.InvalidContext,
                    context: "AgentTrainer.CreateSession");
            }

            try
            {
                _status.State = TrainerState.BootstrappingRuntime;

                // ⭐ Bootstrapper returns orchestrator + concrete RuntimeService
                var (orchestrator, runtime) = _bootstrapper.CreateRuntime();

                if (orchestrator == null || runtime == null)
                {
                    throw SubstrateException.Create(
                        "Bootstrapper returned null orchestrator or runtime.",
                        SubstrateErrorLevel.Fatal,
                        SubstrateErrorCode.InvalidContext,
                        context: "AgentTrainer.CreateSession");
                }

                // ⭐ Correct: TrainerSession + concrete RuntimeService
                var session = new TrainerSession(_config, orchestrator, runtime);

                _status.State = TrainerState.CreatingActor;

                return session;
            }
            catch (SubstrateException)
            {
                _status.State = TrainerState.Error;
                throw;
            }
            catch (Exception ex)
            {
                _status.State = TrainerState.Error;
                throw SubstrateException.Create(
                    "Failed to create trainer session.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "AgentTrainer.CreateSession",
                    inner: ex);
            }
        }

        public TrainerStatus GetStatus() => _status;
    }
}
