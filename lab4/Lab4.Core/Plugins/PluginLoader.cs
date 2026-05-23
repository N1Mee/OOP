using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lab4.Core.Plugins
{
    /// <summary>
    /// Information about a single attempt to load a plugin.
    /// </summary>
    public class PluginLoadEntry
    {
        public string FileName { get; set; }
        public bool Loaded { get; set; }
        public string Message { get; set; }
        public IMediaItemPlugin Instance { get; set; }
    }

    /// <summary>
    /// Discovers plugin DLLs in a folder, verifies their signatures and
    /// instantiates the <see cref="IMediaItemPlugin"/> they expose. Plugins
    /// that fail verification are reported back but never loaded — the host
    /// must not give untrusted code access to the running process.
    /// </summary>
    public class PluginLoader
    {
        private readonly string publicKeyXml;

        /// <summary>Creates a loader with the trusted public key (XML).</summary>
        public PluginLoader(string publicKeyXml)
        {
            this.publicKeyXml = publicKeyXml;
        }

        /// <summary>
        /// Scans the given folder for *.dll plugin files. For each file we
        /// expect a matching *.manifest.xml. The DLL is loaded only when
        /// the manifest verifies, otherwise the entry records the reason.
        /// </summary>
        public List<PluginLoadEntry> LoadFromFolder(string folder)
        {
            var results = new List<PluginLoadEntry>();
            if (!Directory.Exists(folder)) return results;

            foreach (var dll in Directory.GetFiles(folder, "*.dll"))
            {
                var entry = new PluginLoadEntry { FileName = Path.GetFileName(dll) };
                try
                {
                    var manifestPath = dll + ".manifest.xml";
                    if (!File.Exists(manifestPath))
                    {
                        entry.Message = "Manifest file is missing.";
                        results.Add(entry);
                        continue;
                    }

                    var manifest = PluginManifest.Load(manifestPath);
                    var verification = PluginSignature.Verify(dll, manifest, publicKeyXml);
                    if (!verification.IsValid)
                    {
                        entry.Message = "Rejected: " + verification.Reason;
                        results.Add(entry);
                        continue;
                    }

                    // LoadFrom keeps the dependency probing path consistent
                    // with the plugins folder, which matters when the plugin
                    // references additional assemblies shipped alongside it.
                    var assembly = Assembly.LoadFrom(dll);
                    var pluginType = assembly.GetTypes()
                        .FirstOrDefault(t => typeof(IMediaItemPlugin).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

                    if (pluginType == null)
                    {
                        entry.Message = "No IMediaItemPlugin implementation found.";
                        results.Add(entry);
                        continue;
                    }

                    var instance = (IMediaItemPlugin)Activator.CreateInstance(pluginType);
                    entry.Instance = instance;
                    entry.Loaded = true;
                    entry.Message = $"Loaded plugin '{instance.Name}' v{instance.Version}.";
                    results.Add(entry);
                }
                catch (Exception ex)
                {
                    entry.Message = "Load error: " + ex.Message;
                    results.Add(entry);
                }
            }

            return results;
        }
    }
}
