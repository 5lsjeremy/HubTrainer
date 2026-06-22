using Astryx.Abstractions.Agents;

namespace AgentTrainerHub.Export
{
    public static class ExportBundleSerializer
    {
        public static string ToJson(AgentExportBundle bundle)
        {
            return System.Text.Json.JsonSerializer.Serialize(bundle);
        }
    }
}
