using Astryx.Abstractions.Agents;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace AgentTrainer.Core.Export
{
    public static class ExportBundleSerializer
    {
        public static string ToJson(AgentExportBundle bundle)
        {
            return JsonConvert.SerializeObject(
                bundle,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
