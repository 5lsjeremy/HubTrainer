namespace Astryx.Hub.Substrate.Enums
{
    public enum TrainerState
    {
        Uninitialized,          // Before any config or runtime bootstrap
        BootstrappingRuntime,   // RuntimeBootstrapper wiring math, substrate, engines
        CreatingActor,          // AgentActorFactory building IAgentActor from identity/profile
        RegisteringActor,       // RuntimeService registering the actor for stepping
        Stepping,               // RuntimeService.StepMany + ActorUpdatePipeline.Tick
        ApplyingBehavior,       // BehaviorEngine option selection + Apply(...)
        ExportingBundle,        // RuntimeExportEvents.AgentExported (Mind/Persona/Provenance)
        Completed,              // Training run finished, no more steps
        Error                   // Any failure in bootstrap/step/export
    }
}