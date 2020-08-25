using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Template.Configuration
{

    public class PluginConfiguration : BasePluginConfiguration
    {
        // store configurable settings your plugin might need
        public bool Active { get; set; }
        public string Token { get; set; }
        public string Addresses { get; set; }

        public PluginConfiguration()
        {
            // set default options here
			Active = true;
			Token = "";
			Addresses = "";
        }
    }
}
