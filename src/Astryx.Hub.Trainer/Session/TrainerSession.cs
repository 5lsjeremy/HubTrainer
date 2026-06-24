using System;
using Astryx.Abstractions.Agents;
using Astryx.Abstractions.Enums;
using Astryx.Abstractions.Exceptions;
using Astryx.Abstractions.Runtime;
using Astryx.Abstractions.Substrate.Exporting;
using Astryx.Hub.Substrate.Enums;
using Astryx.Hub.Substrate.Helpers;
using Astryx.Hub.Substrate.Interfaces;
using Astryx.Hub.Substrate.Models;
using Astryx.Hub.Trainer.Math;
using Astryx.Runtime.Service;

namespace Astryx.Hub.Trainer.Session
{
    internal sealed class TrainerSession : ITrainerSession
    {
        private readonly string _sessionId = Guid.NewGuid().ToString();
        private readonly TrainerConfig _config;

        private readonly IRuntimeOrchestrator _orchestrator;
        private readonly IRuntimeService _runtime;   // concrete runtime for stepping + export events

        private TrainerStatus _status = new();
        private AgentExportBundle? _lastExport;
        private string? _actorId;

        public TrainerSession(
            TrainerConfig config,
            IRuntimeOrchestrator orchestrator,
            IRuntimeService runtime)
        {
            _config = config ?? throw SubstrateException.Create(
                "TrainerConfig is null.",
                SubstrateErrorLevel.Fatal,
                SubstrateErrorCode.InvalidContext,
                context: "Session.ctor");

            if (string.IsNullOrWhiteSpace(_config.AgentName))
            {
                throw SubstrateException.Create(
                    "TrainerConfig.AgentName is required.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "Session.ctor");
            }

            if (_config.StepsPerBatch <= 0)
            {
                throw SubstrateException.Create(
                    "TrainerConfig.StepsPerBatch must be > 0.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "Session.ctor");
            }

            _orchestrator = orchestrator ?? throw SubstrateException.Create(
                "IRuntimeOrchestrator is null.",
                SubstrateErrorLevel.Fatal,
                SubstrateErrorCode.InvalidContext,
                context: "Session.ctor");

            _runtime = runtime ?? throw SubstrateException.Create(
                "RuntimeService is null.",
                SubstrateErrorLevel.Fatal,
                SubstrateErrorCode.InvalidContext,
                context: "Session.ctor");

            // ⭐ Subscribe to export events BEFORE stepping
            RuntimeExportEvents.AgentExported += OnAgentExported;

            _status.State = TrainerState.BootstrappingRuntime;

            CreateActor();
        }

        public string SessionId => _sessionId;

        private void CreateActor()
        {
            try
            {
                _status.State = TrainerState.CreatingActor;

                var identity = IdentityMatrixFactory.CreateIdentity(_config.Seed);
                if (identity == null)
                {
                    throw SubstrateException.Create(
                        "IdentityMatrixFactory returned null.",
                        SubstrateErrorLevel.Fatal,
                        SubstrateErrorCode.InvalidVector,
                        context: "Session.CreateActor");
                }

                var actorConfig = new RuntimeActorConfig(identity, _config.AgentName);

                // ⭐ Orchestrator registers the actor internally
                var handle = _orchestrator.CreateActor(actorConfig);

                if (handle.Equals(default(ActorHandle)))
                {
                    throw SubstrateException.Create(
                        "Orchestrator returned invalid ActorHandle.",
                        SubstrateErrorLevel.Fatal,
                        SubstrateErrorCode.InvalidContext,
                        context: "Session.CreateActor");
                }

                _actorId = handle.ToString();
                _status.State = TrainerState.RegisteringActor;
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
                    "Failed to create actor.",
                    SubstrateErrorLevel.Fatal,
                    SubstrateErrorCode.InvalidContext,
                    context: "Session.CreateActor",
                    actorId: _actorId,
                    inner: ex);
            }
        }

        // ⭐ Export event handler
        private void OnAgentExported(AgentExportBundle bundle)
        {
            _lastExport = bundle;
        }

        public void Step(int steps)
        {
            if (steps <= 0)
            {
                throw SubstrateException.Create(
                    "Step count must be > 0.",
                    SubstrateErrorLevel.NonCritical,
                    SubstrateErrorCode.InvalidContext,
                    context: "Session.Step",
                    actorId: _actorId);
            }

            try
            {
                _status.State = TrainerState.Stepping;

                // ⭐ Orchestrator is the public stepping API
                var snapshot = _orchestrator.Run(new RuntimeConfig
                {
                    Steps = steps,
                    Mode = ProcessingMode.Default
                });

                if (snapshot == null)
                {
                    throw SubstrateException.Create(
                        "Orchestrator returned null StepSnapshot.",
                        SubstrateErrorLevel.Critical,
                        SubstrateErrorCode.InvalidContext,
                        context: "Session.Step",
                        actorId: _actorId);
                }

                if (snapshot.StepNumber < 0)
                {
                    throw SubstrateException.Create(
                        "StepSnapshot.Step is invalid (< 0).",
                        SubstrateErrorLevel.Critical,
                        SubstrateErrorCode.InvalidScore,
                        context: "Session.Step",
                        actorId: _actorId);
                }

                // ⭐ This is the correct step index
                _status.StepIndex = snapshot.StepNumber;
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
                    "Runtime stepping failed.",
                    SubstrateErrorLevel.Critical,
                    SubstrateErrorCode.StabilityViolation,
                    context: "Session.Step",
                    actorId: _actorId,
                    inner: ex);
            }
        }

        public AgentExportBundle Export()
        {
            if (_lastExport == null)
            {
                throw SubstrateException.Create(
                    "No export bundle available. Step() must be called first.",
                    SubstrateErrorLevel.NonCritical,
                    SubstrateErrorCode.InvalidContext,
                    context: "Session.Export",
                    actorId: _actorId);
            }

            _status.State = TrainerState.Completed;
            return _lastExport;
        }

        public TrainerStatus GetStatus() => _status;
    }
}
