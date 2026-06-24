namespace Astryx.Hub.Substrate.Models
{
    public class HubRuntimeConfig
    {
        public string PluginDirectory { get; set; } = string.Empty;
        public bool EnableHotReload { get; set; } = true;
    }
}